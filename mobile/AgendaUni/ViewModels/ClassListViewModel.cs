using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using AgendaUni.Models;
using AgendaUni.Services;
using AgendaUni.Views;

namespace AgendaUni.ViewModels
{
    public class ClassListViewModel : BaseViewModel
    {
        private readonly ClassService _classService;
        public ObservableCollection<Class> Classes { get; }

        public ICommand AddClassCommand { get; }
        public ICommand EditClassCommand { get; }

        private Class _selectedClass;
        public Class SelectedClass
        {
            get => _selectedClass;
            set
            {
                SetProperty(ref _selectedClass, value);
                if (value != null)
                {
                    EditClassCommand.Execute(value);
                }
            }
        }

        public ClassListViewModel(ClassService classService)
        {
            _classService = classService;
            Classes = new ObservableCollection<Class>();
            AddClassCommand = new Command(async () => await GoToClassPage());
            EditClassCommand = new Command<Class>(async (classObj) => await GoToClassPage(classObj));

            LoadClassesCommand = new Command(async () => await LoadClassesAsync());
        }

        public ICommand LoadClassesCommand { get; }

        private bool isFirstLoad = true;

        private async Task LoadClassesAsync()
        {
            var classes = await _classService.GetAllClassesAsync();
            if (!classes.Any() && isFirstLoad)
            {
                await GoToClassPage();
                isFirstLoad = false;
            }
            else
            {
                Classes.Clear();
                foreach (var c in classes)
                {
                    Classes.Add(c);
                }
            }
        }

        private async Task GoToClassPage(Class classObj = null)
        {
            if (classObj != null)
            {
                var navigationParameters = new Dictionary<string, object>
                {
                    { "ClassId", classObj.Id }
                };
                await Shell.Current.GoToAsync($"{nameof(ClassPage)}", navigationParameters);
            }
            else
            {
                await Shell.Current.GoToAsync($"{nameof(ClassPage)}");
            }
        }
    }
}
