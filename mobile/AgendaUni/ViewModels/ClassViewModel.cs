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

        public ICommand SaveClassCommand { get; }
        public ICommand DeleteClassCommand { get; }

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
                Title = value?.Id == 0 ? "Registrar Aula" : "Editar Aula";
            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public ClassViewModel(ClassService classService)
        {
            _classService = classService;

            SaveClassCommand = new Command(async () => await SaveClassAsync());
            DeleteClassCommand = new Command(async () => await DeleteClass());
            if (ClassId == 0)
            {
                CurrentClass = new Class();
            }
        }

        private async Task LoadClassAsync(int classId)
        {
            CurrentClass = await _classService.GetClassByIdAsync(classId);
        }

        private async Task SaveClassAsync()
        {
            try
            {
                if (CurrentClass == null)
                    return;

                if (CurrentClass.Id == 0)
                {
                    await _classService.AddClassAsync(CurrentClass);
                    await ShowMessageAsync("Aula cadastrada com sucesso!", "Sucesso");
                }
                else
                {
                    await _classService.UpdateClassAsync(CurrentClass);
                    await ShowMessageAsync("Aula atualizada com sucesso!", "Sucesso");
                }

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Ocorreu um erro ao salvar a aula.\n{ex.Message}", "OK");
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


        private async Task ShowMessageAsync(string message, string title = "Aviso")
        {
            await Application.Current.MainPage.DisplayAlert(title, message, "OK");
        }
    }
}
