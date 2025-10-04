using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using AgendaUni.Models;
using AgendaUni.Services;
using AgendaUni.Views;

namespace AgendaUni.ViewModels
{
    public class AbsenceListViewModel : BaseViewModel
    {
        private readonly AbsenceService _absenceService;
        public ObservableCollection<Absence> Absences { get; }

        public ICommand AddAbsenceCommand { get; }
        public ICommand EditAbsenceCommand { get; }

        private Absence _selectedAbsence;
        public Absence SelectedAbsence
        {
            get => _selectedAbsence;
            set
            {
                SetProperty(ref _selectedAbsence, value);
                if (value != null)
                {
                    EditAbsenceCommand.Execute(value);
                }
            }
        }

        public AbsenceListViewModel(AbsenceService absenceService)
        {
            _absenceService = absenceService;
            Absences = new ObservableCollection<Absence>();
            AddAbsenceCommand = new Command(async () => await GoToAbsencePage());
            EditAbsenceCommand = new Command<Absence>(async (absence) => await GoToAbsencePage(absence));

            LoadAbsencesCommand = new Command(async () => await LoadAbsencesAsync());
        }

        public ICommand LoadAbsencesCommand { get; }

        private bool isFirstLoad = true;
        private async Task LoadAbsencesAsync()
        {
            var absences = await _absenceService.GetAllAbsencesAsync();
            if (!absences.Any() && isFirstLoad)
            {
                await GoToAbsencePage();
                isFirstLoad = false;
            }
            else
            {
                Absences.Clear();
                foreach (var a in absences)
                {
                    Absences.Add(a);
                }
            }
        }

        private async Task GoToAbsencePage(Absence absence = null)
        {
            if (absence != null)
            {
                var navigationParameters = new Dictionary<string, object>
                {
                    { "AbsenceId", absence.Id }
                };
                await Shell.Current.GoToAsync($"{nameof(AbsencePage)}", navigationParameters);
            }
            else
            {
                await Shell.Current.GoToAsync($"{nameof(AbsencePage)}");
            }
        }
    }
}
