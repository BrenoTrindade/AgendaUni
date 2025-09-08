using AgendaUni.Models;
using AgendaUni.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

public class AbsenceViewModel : BaseViewModel
{
    private readonly AbsenceService _absenceService;
    private readonly ClassService _classService;

    public ObservableCollection<Class> Classes { get; set; } = new();
    public Absence NewAbsence { get; set; } = new Absence { AbsenceDate = DateTime.Now };
    public ICommand RegisterAbsenceCommand { get; }
    public Class SelectedClass { get; set; }

    public AbsenceViewModel(AbsenceService absenceService, ClassService classService)
    {
        _absenceService = absenceService;
        _classService = classService;

        RegisterAbsenceCommand = new Command(async () => await AddAbsence());

        _ = LoadClassesAsync();
    }

    private async Task LoadClassesAsync()
    {
        var classes = await _classService.GetAllClassesAsync();
        Classes.Clear();

        foreach (var item in classes)
            Classes.Add(item);
    }

    private async Task AddAbsence()
    {
        NewAbsence.ClassId = SelectedClass?.Id ?? 0;

        var result = await _absenceService.RegisterAbsenceAsync(NewAbsence);
        await ShowMessageAsync(result.Message, result.IsSuccess ? "Sucesso" : "Aviso");

        if (result.IsSuccess)
        {
            NewAbsence = new Absence { AbsenceDate = DateTime.Now };
            OnPropertyChanged(nameof(NewAbsence));
        }
    }

    private async Task ShowMessageAsync(string message, string title = "Aviso")
    {
        await Application.Current.MainPage.DisplayAlert(title, message, "OK");
    }
}
