using AgendaUni.Common.Enums;
using AgendaUni.Models;
using AgendaUni.Services;
using Plugin.Maui.Calendar.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AgendaUni.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly ClassService _classService;

        public EventCollection Events { get; set; }
        public ICommand FilterCalendarCommand { get; }
        public CultureInfo Culture => new CultureInfo("pt-BR");

        private bool _showAllSelected = true;
        public bool ShowAllSelected
        {
            get => _showAllSelected;
            set => SetProperty(ref _showAllSelected, value);
        }

        private bool _showEventsSelected;
        public bool ShowEventsSelected
        {
            get => _showEventsSelected;
            set => SetProperty(ref _showEventsSelected, value);
        }

        private bool _showClassesSelected;
        public bool ShowClassesSelected
        {
            get => _showClassesSelected;
            set => SetProperty(ref _showClassesSelected, value);
        }

        private bool _showAbsencesSelected;
        public bool ShowAbsencesSelected
        {
            get => _showAbsencesSelected;
            set => SetProperty(ref _showAbsencesSelected, value);
        }

        private List<Class> _allClasses = new List<Class>();

        public class CalendarDisplayEvent
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public EventType Type { get; set; }
        }

        public MainPageViewModel(ClassService classService)
        {
            _classService = classService;
            Events = new EventCollection();
            FilterCalendarCommand = new Command<string>(ApplyFilter);
        }

        public override async Task OnAppearingAsync()
        {
            await LoadDataAsync();
            ApplyFilter("All");
        }

        private async Task LoadDataAsync()
        {
            IsBusy = true;
            _allClasses = await _classService.GetAllClassesAsync() ?? new List<Class>();
            IsBusy = false;
        }

        private void ApplyFilter(string filter)
        {
            Events.Clear();

            bool showClasses = filter == "All" || filter == "Classes";
            bool showEvents = filter == "All" || filter == "Events";
            bool showAbsences = filter == "All" || filter == "Absences";

            foreach (var cls in _allClasses)
            {
                if (showClasses && cls.Schedules != null)
                {
                    AddClassSchedulesToCalendar(cls);
                }

                if (showEvents && cls.Events != null)
                {
                    AddEventsToCalendar(cls);
                }

                if (showAbsences && cls.Absences != null)
                {
                    AddAbsencesToCalendar(cls);
                }
            }

            UpdateFilterSelectionState(filter);

            OnPropertyChanged(nameof(Events));
        }

        private void AddEventToCalendar(DateTime date, CalendarDisplayEvent eventToAdd)
        {
            var dateOnly = date.Date;
            if (!Events.ContainsKey(dateOnly))
            {
                Events[dateOnly] = new List<CalendarDisplayEvent>();
            }
            (Events[dateOnly] as List<CalendarDisplayEvent>)?.Add(eventToAdd);
        }

        private void AddClassSchedulesToCalendar(Class cls)
        {
            var today = DateTime.Today;
            foreach (var schedule in cls.Schedules)
            {
                for (int i = 0; i < 30; i++)
                {
                    var futureDate = today.AddDays(i);
                    if (futureDate.DayOfWeek == schedule.DayOfWeek)
                    {
                        var calendarEvent = new CalendarDisplayEvent
                        {
                            Name = cls.ClassName ?? "Aula",
                            Description = $"Horário: {schedule.ClassTime:hh\\:mm}",
                            Type = EventType.ClassSchedule
                        };
                        AddEventToCalendar(futureDate, calendarEvent);
                    }
                }
            }
        }

        private void AddEventsToCalendar(Class cls)
        {
            foreach (var ev in cls.Events)
            {
                var calendarEvent = new CalendarDisplayEvent
                {
                    Name = $"Evento: {cls.ClassName ?? "Geral"}",
                    Description = ev.Description,
                    Type = EventType.Event
                };
                AddEventToCalendar(ev.EventDate, calendarEvent);
            }
        }

        private void AddAbsencesToCalendar(Class cls)
        {
            foreach (var absence in cls.Absences)
            {
                var calendarEvent = new CalendarDisplayEvent
                {
                    Name = $"Falta: {cls.ClassName ?? "Aula"}",
                    Description = $"Motivo: {absence.AbsenceReason}",
                    Type = EventType.Absence
                };
                AddEventToCalendar(absence.AbsenceDate, calendarEvent);
            }
        }

        private void UpdateFilterSelectionState(string filter)
        {
            ShowAllSelected = filter == "All";
            ShowEventsSelected = filter == "Events";
            ShowClassesSelected = filter == "Classes";
            ShowAbsencesSelected = filter == "Absences";
        }
    }
}