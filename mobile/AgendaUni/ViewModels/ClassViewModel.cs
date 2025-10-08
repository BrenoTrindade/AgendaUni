using System.Collections.ObjectModel;
using System.Windows.Input;
using AgendaUni.Models;
using AgendaUni.Services;
using Microsoft.Maui.Controls;

namespace AgendaUni.ViewModels
{
    [QueryProperty(nameof(ClassId), "ClassId")]
    public class ClassViewModel : BaseViewModel
    {
        private readonly ClassService _classService;
        private readonly AbsenceService _absenceService;
        private readonly ClassScheduleService _scheduleService;
        private readonly EventService _eventService;

        public ICommand SaveClassCommand { get; }
        public ICommand DeleteClassCommand { get; }
        public ICommand DeleteAbsenceCommand { get; }
        public ICommand DeleteScheduleCommand { get; }
        public ICommand DeleteEventCommand { get; }
        public ICommand AddAbsenceCommand { get; }
        public ICommand AddScheduleCommand { get; }
        public ICommand AddEventCommand { get; }
        public ICommand GoToAbsenceDetailsCommand { get; }
        public ICommand GoToScheduleDetailsCommand { get; }
        public ICommand GoToEventDetailsCommand { get; }


        private int _classId;
        public int ClassId
        {
            get => _classId;
            set
            {
                _classId = value;
                if (value > 0)
                {
                    LoadClassAsync(value);
                }
            }
        }

        private Class _currentClass;
        public Class CurrentClass
        {
            get => _currentClass;
            set
            {
                SetProperty(ref _currentClass, value);
                Title = value?.Id == 0 ? "Registrar Matéria" : $"Gerenciar Matéria";
            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        private string _absencesSummary;
        public string AbsencesSummary
        {
            get => _absencesSummary;
            set => SetProperty(ref _absencesSummary, value);
        }


        public ObservableCollection<Absence> Absences { get; } = new();
        public ObservableCollection<ClassScheduleDisplayViewModel> Schedules { get; } = new();
        public ObservableCollection<Event> Events { get; } = new();

        public ClassViewModel(ClassService classService, AbsenceService absenceService, ClassScheduleService scheduleService, EventService eventService)
        {
            _classService = classService;
            _absenceService = absenceService;
            _scheduleService = scheduleService;
            _eventService = eventService;

            SaveClassCommand = new Command(async () => await SaveClassAsync());
            DeleteClassCommand = new Command(async () => await DeleteClass());

            AddAbsenceCommand = new Command(async () => await GoToAddAbsence());
            AddScheduleCommand = new Command(async () => await GoToAddSchedule());
            AddEventCommand = new Command(async () => await GoToAddEvent());


            DeleteAbsenceCommand = new Command(async (param) => await DeleteAbsence((int)param));
            DeleteScheduleCommand = new Command(async (param) => await DeleteSchedule((int)param));
            DeleteEventCommand = new Command(async (param) => await DeleteEvent((int)param));

            GoToAbsenceDetailsCommand = new Command(async (param) => await GoToAbsenceDetails(param as Absence));
            GoToScheduleDetailsCommand = new Command(async (param) => await GoToScheduleDetails(param as ClassScheduleDisplayViewModel));
            GoToEventDetailsCommand = new Command(async (param) => await GoToEventDetails(param as Event));


            if (ClassId == 0)
            {
                CurrentClass = new Class();
                AbsencesSummary = $@"Faltas";
            }
        }
        private async Task LoadClassAsync(int classId)
        {
            var classToLoad = await _classService.GetClassByIdAsync(classId);

            if (classToLoad != null)
            {
                await LoadRelatedClassDataAsync(classToLoad);
            }
        }
        private async Task LoadRelatedClassDataAsync(Class classObj)
        {
            CurrentClass = classObj;

            var classId = classObj.Id;

            Absences.Clear();
            var absences = await _absenceService.GetAbsencesByClassIdAsync(classId);
            foreach (var absence in absences)
            {
                Absences.Add(absence);
            }

            Schedules.Clear();
            var schedules = await _scheduleService.GetSchedulesByClassIdAsync(classId);
            foreach (var schedule in schedules)
            {
                Schedules.Add(new ClassScheduleDisplayViewModel(schedule));
            }

            Events.Clear();
            var events = await _eventService.GetEventsByClassIdAsync(classId);
            foreach (var evt in events)
            {
                Events.Add(evt);
            }

            AbsencesSummary = $@"Faltas {Absences?.Count ?? 0}/{CurrentClass?.MaximumAbsences ?? 0}";
        }

        private async Task SaveClassAsync()
        {
            try
            {
                if (CurrentClass == null)
                    return;

                if (string.IsNullOrWhiteSpace(CurrentClass.ClassName))
                {
                    await ShowMessageAsync("O nome da matéria é obrigatório.", "Aviso");
                    return;
                }

                if (CurrentClass.MaximumAbsences <= 0)
                {
                    await ShowMessageAsync("O máximo de faltas é obrigatório e deve ser maior que zero.", "Aviso");
                    return;
                }

                if (CurrentClass.Id == 0)
                {
                    var result = await _classService.AddClassAsync(CurrentClass);
                    var savedClass = result.Data;
                    await ShowMessageAsync("Aula cadastrada com sucesso!", "Sucesso");
                    ClassId = savedClass.Id;
                }
                else
                {
                    await _classService.UpdateClassAsync(CurrentClass);
                    await ShowMessageAsync("Aula atualizada com sucesso!", "Sucesso");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", "Ocorreu um erro ao salvar a aula.\n" + ex.Message, "OK");
            }
        }

        private async Task DeleteClass()
        {
            if (CurrentClass.Id == 0)
            {
                await ShowMessageAsync("Não é possível excluir uma aula que ainda não foi salva.", "Aviso");
                return;
            }

            bool userConfirmed = await Application.Current.MainPage.DisplayAlert("Confirmar Exclusão", "Tem certeza de que deseja excluir esta aula?", "Sim", "Não");

            if (userConfirmed)
            {
                await _classService.DeleteClassAsync(CurrentClass.Id);
                await ShowMessageAsync("Aula excluída com sucesso!", "Sucesso");
                await Shell.Current.GoToAsync("..");
            }
        }
        private async Task DeleteEvent(int eventId)
        {
            if (eventId == 0)
            {
                await ShowMessageAsync("Não é possível excluir o evento.", "Aviso");
                return;
            }

            bool userConfirmed = await Application.Current.MainPage.DisplayAlert("Confirmar Exclusão", "Tem certeza de que deseja excluir este evento?", "Sim", "Não");

            if (userConfirmed)
            {
                await _eventService.DeleteEventAsync(eventId);
                await ShowMessageAsync("Evento excluído com sucesso!", "Sucesso");

                var eventToRemove = Events.FirstOrDefault(s => s.Id == eventId);

                if (eventToRemove != null)
                {
                    Events.Remove(eventToRemove);
                }
            }
        }

        private async Task DeleteSchedule(int scheduleId)
        {
            if (scheduleId == 0)
            {
                await ShowMessageAsync("Não é possível excluir o horário.", "Aviso");
                return;
            }

            bool userConfirmed = await Application.Current.MainPage.DisplayAlert("Confirmar Exclusão", "Tem certeza de que deseja excluir este horário?", "Sim", "Não");

            if (userConfirmed)
            {
                await _scheduleService.DeleteClassScheduleAsync(scheduleId);
                await ShowMessageAsync("Horário excluído com sucesso!", "Sucesso");

                var scheduleToRemove = Schedules.FirstOrDefault(s => s.Id == scheduleId);

                if (scheduleToRemove != null)
                {
                    Schedules.Remove(scheduleToRemove);
                }

            }
        }

        private async Task DeleteAbsence(int absenceId)
        {
            if (absenceId == 0)
            {
                await ShowMessageAsync("Não foi possível excluir a falta.", "Aviso");
                return;
            }

            bool userConfirmed = await Application.Current.MainPage.DisplayAlert("Confirmar Exclusão", "Tem certeza de que deseja excluir esta falta?", "Sim", "Não");

            if (userConfirmed)
            {
                await _absenceService.DeleteAbsenceAsync(absenceId);
                await ShowMessageAsync("Falta excluída com sucesso!", "Sucesso");

                var absenceToRemove = Absences.FirstOrDefault(s => s.Id == absenceId);

                if (absenceToRemove != null)
                {
                    Absences.Remove(absenceToRemove);
                    AbsencesSummary = $@"Faltas {Absences?.Count ?? 0}/{CurrentClass?.MaximumAbsences ?? 0}";
                }

            }
        }

        private async Task GoToAddAbsence()
        {
            await Shell.Current.GoToAsync($"AbsencePage?ClassId={CurrentClass.Id}");
        }

        private async Task GoToAddSchedule()
        {
            await Shell.Current.GoToAsync($"ClassSchedulePage?ClassId={CurrentClass.Id}");
        }

        private async Task GoToAddEvent()
        {
            await Shell.Current.GoToAsync($"EventPage?ClassId={CurrentClass.Id}");
        }

        private async Task GoToAbsenceDetails(Absence absence)
        {
            if (absence == null)
                return;

            await Shell.Current.GoToAsync($"AbsencePage?AbsenceId={absence.Id}");
        }

        private async Task GoToScheduleDetails(ClassScheduleDisplayViewModel schedule)
        {
            if (schedule == null)
                return;

            await Shell.Current.GoToAsync($"ClassSchedulePage?ScheduleId={schedule.Id}");
        }

        private async Task GoToEventDetails(Event evt)
        {
            if (evt == null)
                return;

            await Shell.Current.GoToAsync($"EventPage?EventId={evt.Id}");
        }


        private async Task ShowMessageAsync(string message, string title = "Aviso")
        {
            await Application.Current.MainPage.DisplayAlert(title, message, "OK");
        }
    }
}