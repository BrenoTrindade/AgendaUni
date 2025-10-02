using AgendaUni.Models;
using AgendaUni.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

public class EventViewModel : BaseViewModel
{
    private readonly EventService _eventService;
    private readonly ClassService _classService;

    public ObservableCollection<Class> Classes { get; set; } = new();
    public Event NewEvent { get; set; } = new Event { EventDate = DateTime.Now };
    public ICommand RegisterEventCommand { get; }
    public Class SelectedClass { get; set; }

    public EventViewModel(EventService eventService, ClassService classService)
    {
        _eventService = eventService;
        _classService = classService;

        RegisterEventCommand = new Command(async () => await AddEvent());

        _ = LoadClassesAsync();
    }

    private async Task LoadClassesAsync()
    {
        var classes = await _classService.GetAllClassesAsync();
        Classes.Clear();

        foreach (var item in classes)
            Classes.Add(item);
    }

    private async Task AddEvent()
    {
        NewEvent.ClassId = SelectedClass?.Id ?? 0;

        var result = await _eventService.AddEventAsync(NewEvent);
        await ShowMessageAsync(result.Message, result.IsSuccess ? "Sucesso" : "Aviso");

        if (result.IsSuccess)
        {
            NewEvent = new Event { EventDate = DateTime.Now };
            OnPropertyChanged(nameof(NewEvent));
        }
    }

    private async Task ShowMessageAsync(string message, string title = "Aviso")
    {
        await Application.Current.MainPage.DisplayAlert(title, message, "OK");
    }
}