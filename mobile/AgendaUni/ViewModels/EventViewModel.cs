using AgendaUni.Models;
using AgendaUni.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Maui.Controls;

[QueryProperty(nameof(EventId), "EventId")]
public class EventViewModel : BaseViewModel
{
    private readonly EventService _eventService;
    private readonly ClassService _classService;

    public ObservableCollection<Class> Classes { get; set; } = new();
    private Event _currentEvent;
    public Event CurrentEvent
    {
        get => _currentEvent;
        set
        {
            SetProperty(ref _currentEvent, value);
            Title = value?.Id == 0 ? "Registrar Evento" : "Editar Evento";
            if (value != null)
            {
                SelectedClass = Classes.FirstOrDefault(c => c.Id == value.ClassId);
            }
        }
    }

    public ICommand SaveEventCommand { get; }
    public ICommand DeleteEventCommand { get; }

    private int _eventId;
    public int EventId
    {
        get => _eventId;
        set
        {
            _eventId = value;
            if (value > 0)
            {
                LoadEventAsync(value);
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

    public EventViewModel(EventService eventService, ClassService classService)
    {
        _eventService = eventService;
        _classService = classService;

        SaveEventCommand = new Command(async () => await SaveEvent());
        DeleteEventCommand = new Command(async () => await DeleteEvent());

        _ = LoadClassesAsync();
        if (EventId == 0)
        {
            CurrentEvent = new Event { EventDate = DateTime.Now };
        }
    }

    private async Task LoadEventAsync(int eventId)
    {
        CurrentEvent = await _eventService.GetEventByIdAsync(eventId);
    }

    private async Task LoadClassesAsync()
    {
        var classes = await _classService.GetAllClassesAsync();
        Classes.Clear();

        foreach (var item in classes)
            Classes.Add(item);
    }

    private async Task SaveEvent()
    {
        CurrentEvent.ClassId = SelectedClass?.Id ?? 0;

        var result = CurrentEvent.Id == 0
            ? await _eventService.AddEventAsync(CurrentEvent)
            : await _eventService.UpdateEventAsync(CurrentEvent);

        await ShowMessageAsync(result.Message, result.IsSuccess ? "Sucesso" : "Aviso");

        if (result.IsSuccess)
        {
            await Shell.Current.GoToAsync("..");
        }
    }

    private async Task DeleteEvent()
    {
        if (CurrentEvent.Id == 0)
        {
            await ShowMessageAsync("Não é possível excluir um evento que ainda não foi salvo.", "Aviso");
            return;
        }

        bool userConfirmed = await Application.Current.MainPage.DisplayAlert("Confirmar Exclusão", "Tem certeza de que deseja excluir este evento?", "Sim", "Não");

        if (userConfirmed)
        {
            var result = await _eventService.DeleteEventAsync(CurrentEvent.Id);
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