using AgendaUni.Models;
using AgendaUni.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Maui.Controls;

[QueryProperty(nameof(ClassScheduleId), "ClassScheduleId")]
public class ClassScheduleViewModel : BaseViewModel
{
    private readonly ClassService _classService;
    private readonly ClassScheduleService _classScheduleService;

    public ObservableCollection<Class> Classes { get; set; } = new();
    public ObservableCollection<DayOfWeek> DaysOfWeek { get; set; } = new(System.Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>());

    private ClassSchedule _currentClassSchedule;
    public ClassSchedule CurrentClassSchedule
    {
        get => _currentClassSchedule;
        set
        {
            SetProperty(ref _currentClassSchedule, value);
            Title = value?.Id == 0 ? "Registrar Horário" : "Editar Horário";
            if (value != null)
            {
                SelectedClass = Classes.FirstOrDefault(c => c.Id == value.ClassId);
            }
        }
    }

    private Class _selectedClass;
    public Class SelectedClass
    {
        get => _selectedClass;
        set => SetProperty(ref _selectedClass, value);
    }

    private string _title;
    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public ICommand SaveClassScheduleCommand { get; }
    public ICommand DeleteClassScheduleCommand { get; }

    private int _classScheduleId;
    public int ClassScheduleId
    {
        get => _classScheduleId;
        set
        {
            _classScheduleId = value;
            if (value > 0)
            {
                LoadClassScheduleAsync(value);
            }
        }
    }

    public ClassScheduleViewModel(ClassScheduleService classScheduleService, ClassService classService)
    {
        _classScheduleService = classScheduleService;
        _classService = classService;

        SaveClassScheduleCommand = new Command(async () => await SaveClassSchedule());
        DeleteClassScheduleCommand = new Command(async () => await DeleteClassSchedule());

        _ = LoadClassesAsync();
        if (ClassScheduleId == 0)
        {
            CurrentClassSchedule = new ClassSchedule();
        }
    }

    private async Task LoadClassScheduleAsync(int classScheduleId)
    {
        CurrentClassSchedule = await _classScheduleService.GetClassScheduleByIdAsync(classScheduleId);
    }

    private async Task LoadClassesAsync()
    {
        var classes = await _classService.GetAllClassesAsync();
        Classes.Clear();
        foreach (var item in classes)
            Classes.Add(item);
    }

    private async Task SaveClassSchedule()
    {
        if (SelectedClass == null)
        {
            await ShowMessageAsync("Selecione uma aula.", "Aviso");
            return;
        }

        CurrentClassSchedule.ClassId = SelectedClass.Id;

        var result = CurrentClassSchedule.Id == 0
            ? await _classScheduleService.AddClassScheduleAsync(CurrentClassSchedule)
            : await _classScheduleService.UpdateClassScheduleAsync(CurrentClassSchedule);

        await ShowMessageAsync(result.Message, result.IsSuccess ? "Sucesso" : "Aviso");

        if (result.IsSuccess)
        {
            await Shell.Current.GoToAsync("..");
        }
    }

    private async Task DeleteClassSchedule()
    {
        if (CurrentClassSchedule.Id == 0)
        {
            await ShowMessageAsync("Não é possível excluir um horário que ainda não foi salvo.", "Aviso");
            return;
        }

        bool userConfirmed = await Application.Current.MainPage.DisplayAlert("Confirmar Exclusão", "Tem certeza de que deseja excluir este horário?", "Sim", "Não");

        if (userConfirmed)
        {
            var result = await _classScheduleService.DeleteClassScheduleAsync(CurrentClassSchedule.Id);
            await ShowMessageAsync(result.Message, result.IsSuccess ? "Sucesso" : "Aviso");

            if (result.IsSuccess)
            {
                await Shell.Current.GoToAsync("..");
            }
        }
    }


    private async Task ShowMessageAsync(string message, string title = "Aviso")
    {
        await Application.Current.MainPage.DisplayAlert(title, message, "OK");
    }
}
