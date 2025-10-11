using AgendaUni.Models;
using AgendaUni.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Maui.Controls;

[QueryProperty(nameof(AbsenceId), "AbsenceId")]
[QueryProperty(nameof(ClassId), "ClassId")]
public class AbsenceViewModel : BaseViewModel
{
    private readonly AbsenceService _absenceService;
    private readonly ClassService _classService;

    public ObservableCollection<Class> Classes { get; set; } = new();
    private Absence _currentAbsence;
    public Absence CurrentAbsence
    {
        get => _currentAbsence;
        set
        {
            SetProperty(ref _currentAbsence, value);
            Title = value?.Id == 0 ? "Registrar Falta" : "Editar Falta";
            if (value != null)
            {
                SelectedClass = Classes.FirstOrDefault(c => c.Id == value.ClassId);
            }
        }
    }

    private int _classId;
    public int ClassId
    {
        get => _classId;
        set
        {
            _classId = value;
            if (value > 0 && AbsenceId == 0)
            {
                CurrentAbsence.ClassId = value;
                SelectedClass = Classes.FirstOrDefault(c => c.Id == value);
            }
        }
    }

    private int _absenceId;
    public int AbsenceId
    {
        get => _absenceId;
        set
        {
            _absenceId = value;
            if (value > 0)
            {
                LoadAbsenceAsync(value);
            }
        }
    }

    public ICommand SaveAbsenceCommand { get; }
    public ICommand DeleteAbsenceCommand { get; }
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

    public AbsenceViewModel(AbsenceService absenceService, ClassService classService)
    {
        _absenceService = absenceService;
        _classService = classService;

        SaveAbsenceCommand = new Command(async () => await SaveAbsence());
        DeleteAbsenceCommand = new Command(async () => await DeleteAbsence());

        _ = LoadClassesAsync();
        if (AbsenceId == 0)
        {
            CurrentAbsence = new Absence { AbsenceDate = DateTime.Now };
        }
    }

    private async Task LoadAbsenceAsync(int absenceId)
    {
        CurrentAbsence = await _absenceService.GetAbsenceByIdAsync(absenceId);
    }

    private async Task LoadClassesAsync()
    {
        var classes = await _classService.GetAllClassesAsync();
        Classes.Clear();

        foreach (var item in classes)
            Classes.Add(item);
    }

    private async Task SaveAbsence()
    {
        CurrentAbsence.ClassId = SelectedClass.Id;

        var result = CurrentAbsence.Id == 0
            ? await _absenceService.AddAbsenceAsync(CurrentAbsence)
            : await _absenceService.UpdateAbsenceAsync(CurrentAbsence);

        await ShowMessageAsync(result.Message, result.IsSuccess ? "Sucesso" : "Aviso");

        if (result.IsSuccess)
        {
            await Shell.Current.GoToAsync("..");
        }
    }

    private async Task DeleteAbsence()
    {
        if (CurrentAbsence.Id == 0)
        {
            await ShowMessageAsync("Não é possível excluir uma falta que ainda não foi salva.", "Aviso");
            return;
        }

        bool userConfirmed = await Application.Current.MainPage.DisplayAlert("Confirmar Exclusão", "Tem certeza de que deseja excluir esta falta?", "Sim", "Não");

        if (userConfirmed)
        {
            var result = await _absenceService.DeleteAbsenceAsync(CurrentAbsence.Id);
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
