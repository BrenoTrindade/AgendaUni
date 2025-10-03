using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using AgendaUni.Models;
using AgendaUni.Services;
using AgendaUni.Views;

namespace AgendaUni.ViewModels
{
    public class ClassScheduleListViewModel : BaseViewModel
    {
        private readonly ClassScheduleService _classScheduleService;
        public ObservableCollection<ClassSchedule> ClassSchedules { get; }

        public ICommand AddClassScheduleCommand { get; }
        public ICommand EditClassScheduleCommand { get; }

        private ClassSchedule _selectedClassSchedule;
        public ClassSchedule SelectedClassSchedule
        {
            get => _selectedClassSchedule;
            set
            {
                SetProperty(ref _selectedClassSchedule, value);
                if (value != null)
                {
                    EditClassScheduleCommand.Execute(value);
                }
            }
        }

        public ClassScheduleListViewModel(ClassScheduleService classScheduleService)
        {
            _classScheduleService = classScheduleService;
            ClassSchedules = new ObservableCollection<ClassSchedule>();
            AddClassScheduleCommand = new Command(async () => await GoToClassSchedulePage());
            EditClassScheduleCommand = new Command<ClassSchedule>(async (classSchedule) => await GoToClassSchedulePage(classSchedule));

            LoadClassSchedulesCommand = new Command(async () => await LoadClassSchedulesAsync());
        }

        public ICommand LoadClassSchedulesCommand { get; }

        private bool isFirstLoad = true;

        private async Task LoadClassSchedulesAsync()
        {
            var classSchedules = await _classScheduleService.GetAllClassSchedulesAsync();
            if (!classSchedules.Any() && isFirstLoad)
            {
                await GoToClassSchedulePage();
                isFirstLoad = false;
            }
            else
            {
                ClassSchedules.Clear();
                foreach (var cs in classSchedules)
                {
                    ClassSchedules.Add(cs);
                }
            }
            isFirstLoad = false;
        }

        private async Task GoToClassSchedulePage(ClassSchedule classSchedule = null)
        {
            if (classSchedule != null)
            {
                var navigationParameters = new Dictionary<string, object>
                {
                    { "ClassScheduleId", classSchedule.Id }
                };
                await Shell.Current.GoToAsync($"{nameof(ClassSchedulePage)}", navigationParameters);
            }
            else
            {
                await Shell.Current.GoToAsync($"{nameof(ClassSchedulePage)}");
            }
        }
    }
}
