using AgendaUni.Models;
using AgendaUni.Services;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Microsoft.Maui.Controls;

public class SelectableDay : BaseViewModel
{
    public DayOfWeek Day { get; }
    public string Name { get; }

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    public ICommand ToggleSelectionCommand { get; }

    public SelectableDay(DayOfWeek day)
    {
        Day = day;
        var culture = new CultureInfo("pt-BR");
        var dayName = culture.DateTimeFormat.GetDayName(day);
        Name = culture.TextInfo.ToTitleCase(dayName.Split('-')[0]);
        ToggleSelectionCommand = new Command(() => IsSelected = !IsSelected);
    }
}


[QueryProperty(nameof(ClassScheduleId), "ClassScheduleId")]
public class ClassScheduleViewModel : BaseViewModel
{
    private readonly ClassService _classService;
    private readonly ClassScheduleService _classScheduleService;

    public ObservableCollection<Class> Classes { get; set; } = new();
    public ObservableCollection<SelectableDay> SelectableDays { get; set; }

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

        SelectableDays = new ObservableCollection<SelectableDay>(
            System.Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().Where(d => d != DayOfWeek.Saturday && d != DayOfWeek.Sunday).Select(d => new SelectableDay(d))
        );

        _ = LoadClassesAsync();
        if (ClassScheduleId == 0)
        {
            CurrentClassSchedule = new ClassSchedule();
        }
    }

    private async Task LoadClassScheduleAsync(int classScheduleId)
    {
        CurrentClassSchedule = await _classScheduleService.GetClassScheduleByIdAsync(classScheduleId);
        foreach (var day in SelectableDays)
        {
            day.IsSelected = day.Day == CurrentClassSchedule.DayOfWeek;
        }
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

        var selectedDays = SelectableDays.Where(d => d.IsSelected).ToList();
        if (!selectedDays.Any())
        {
            await ShowMessageAsync("Selecione pelo menos um dia da semana.", "Aviso");
            return;
        }

        bool isSuccess = true;
        string finalMessage;
        
        if (CurrentClassSchedule.Id == 0) // Create mode
        {
            int successCount = 0;
            foreach (var day in selectedDays)
            {
                var newSchedule = new ClassSchedule
                {
                    ClassId = SelectedClass.Id,
                    ClassTime = CurrentClassSchedule.ClassTime,
                    DayOfWeek = day.Day
                };
                var result = await _classScheduleService.AddClassScheduleAsync(newSchedule);
                if (result.IsSuccess)
                {
                    successCount++;
                }
                isSuccess &= result.IsSuccess;
            }
            finalMessage = $"{successCount} de {selectedDays.Count} horários salvos com sucesso.";
        }
        else // Edit mode
        {
            if (selectedDays.Count > 1)
            {
                await ShowMessageAsync("A edição só permite alterar para um único dia. Para salvar em múltiplos dias, crie um novo registro.", "Aviso");
                return;
            }

            CurrentClassSchedule.ClassId = SelectedClass.Id;
            CurrentClassSchedule.DayOfWeek = selectedDays.Single().Day;
            var result = await _classScheduleService.UpdateClassScheduleAsync(CurrentClassSchedule);
            isSuccess = result.IsSuccess;
            finalMessage = result.Message;
        }

        await ShowMessageAsync(finalMessage, isSuccess ? "Sucesso" : "Aviso");

        if (isSuccess)
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