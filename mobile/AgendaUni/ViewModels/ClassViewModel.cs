using System.Collections.ObjectModel;
using System.Windows.Input;
using AgendaUni.Models;
using AgendaUni.Services;

namespace AgendaUni.ViewModels
{
    public class ClassViewModel : BaseViewModel
    {
        private readonly ClassService _classService;

        public ICommand RegisterClassCommand { get; }

        private Class _newClass;

        public Class NewClass
        {
            get => _newClass;
            set => SetProperty(ref _newClass, value);
        }
        public Dictionary<DayOfWeek, bool> SelectedDays { get; set; } = new();

        public ClassViewModel(ClassService classService)
        {
            _classService = classService;

            RegisterClassCommand = new Command(async () => await AddClassAsync());
            NewClass = new Class();

        }

        private async Task AddClassAsync()
        {
            try
            {
                if (NewClass == null)
                    return;

                await _classService.AddClassAsync(NewClass);

                await ShowMessageAsync("Aula cadastrada com sucesso!", "Sucesso");

                NewClass = new Class();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Ocorreu um erro ao cadastrar a aula.\n{ex.Message}", "OK");
            }
        }
        private async Task ShowMessageAsync(string message, string title = "Aviso")
        {
            await Application.Current.MainPage.DisplayAlert(title, message, "OK");
        }
    }
}
