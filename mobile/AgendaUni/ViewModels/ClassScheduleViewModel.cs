using AgendaUni.Models;
using AgendaUni.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

public class ClassScheduleViewModel : BaseViewModel
{
    private readonly ClassService _classService;

    private readonly ClassScheduleService _classScheduleService;
    public bool MondaySelected { get; set; }
    public bool TuesdaySelected { get; set; }
    public bool WednesdaySelected { get; set; }
    public bool ThursdaySelected { get; set; }
    public bool FridaySelected { get; set; }
    public ObservableCollection<Class> Classes { get; set; } = new();
    public ClassSchedule _classSchedule { get; set; } = new();
    public Class SelectedClass { get; set; }
    public ICommand RegisterClassScheduleCommand { get; }
    public ICommand ToggleDayCommand => new Command<string>(ToggleDay);
    public ClassScheduleViewModel(ClassScheduleService classScheduleServic, ClassService classService)
    {
        _classScheduleService = classScheduleServic;
        _classService = classService;

        RegisterClassScheduleCommand = new Command(async () => await AddClassSchedule());
        
        _ = LoadClassesAsync();
    }

    private async Task LoadClassesAsync()
    {
        var classes = await _classService.GetAllClassesAsync();

        foreach (var item in classes)
            Classes.Add(item);
    }

    private async Task AddClassSchedule()
    {
        var selectedDays = GetSelectedDaysList();

        if (SelectedClass == null || SelectedClass.Id == 0)
        {
            await ShowMessageAsync("Selecione uma aula.", "Aviso");
            return;
        }

        if (selectedDays.Count == 0)
        {
            await ShowMessageAsync("Selecione ao menos um dia da semana.", "Aviso");
            return;
        }

        bool allSuccess = true;
        string finalMessage = "";

        foreach (var day in selectedDays)
        {
            var schedule = new ClassSchedule
            {
                ClassId = SelectedClass.Id,
                DayOfWeek = day,
                ClassTime = ClassTime
            };

            var result = await _classScheduleService.AddClassScheduleAsync(schedule);

            if (!result.IsSuccess)
            {
                allSuccess = false;
                finalMessage += $"{day}: {result.Message}\n";
            }
        }

        if (allSuccess)
        {
            finalMessage = "Horários cadastrados com sucesso.";
            _classSchedule = new ClassSchedule();
            OnPropertyChanged(nameof(ClassTime));
        }

        await ShowMessageAsync(finalMessage.Trim(), allSuccess ? "Sucesso" : "Aviso");
    }

    private void ToggleDay(string day)
    {
        switch (day)
        {
            case "Monday":
                MondaySelected = !MondaySelected;
                OnPropertyChanged(nameof(MondaySelected));
                break;
            case "Tuesday":
                TuesdaySelected = !TuesdaySelected;
                OnPropertyChanged(nameof(TuesdaySelected));
                break;
            case "Wednesday":
                WednesdaySelected = !WednesdaySelected;
                OnPropertyChanged(nameof(WednesdaySelected));
                break;
            case "Thursday":
                ThursdaySelected = !ThursdaySelected;
                OnPropertyChanged(nameof(ThursdaySelected));
                break;
            case "Friday":
                FridaySelected = !FridaySelected;
                OnPropertyChanged(nameof(FridaySelected));
                break;
        }
    }
    private List<DayOfWeek> GetSelectedDaysList()
    {
        var selectedDays = new List<DayOfWeek>();

        if (MondaySelected) selectedDays.Add(DayOfWeek.Monday);
        if (TuesdaySelected) selectedDays.Add(DayOfWeek.Tuesday);
        if (WednesdaySelected) selectedDays.Add(DayOfWeek.Wednesday);
        if (ThursdaySelected) selectedDays.Add(DayOfWeek.Thursday);
        if (FridaySelected) selectedDays.Add(DayOfWeek.Friday);

        return selectedDays;
    }


    public TimeSpan ClassTime
    {
        get => _classSchedule.ClassTime;
        set
        {
            if (_classSchedule.ClassTime != value)
            {
                _classSchedule.ClassTime = value;
                OnPropertyChanged(nameof(ClassTime));
            }
        }
    }

    private async Task ShowMessageAsync(string message, string title = "Aviso")
    {
        await Application.Current.MainPage.DisplayAlert(title, message, "OK");
    }

}
