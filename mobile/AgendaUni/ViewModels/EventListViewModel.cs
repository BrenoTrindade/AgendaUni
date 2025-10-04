using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using AgendaUni.Models;
using AgendaUni.Services;
using AgendaUni.Views;

namespace AgendaUni.ViewModels
{
    public class EventListViewModel : BaseViewModel
    {
        private readonly EventService _eventService;
        public ObservableCollection<Event> Events { get; }

        public ICommand AddEventCommand { get; }
        public ICommand EditEventCommand { get; }

        private Event _selectedEvent;
        public Event SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                SetProperty(ref _selectedEvent, value);
                if (value != null)
                {
                    EditEventCommand.Execute(value);
                }
            }
        }

        public EventListViewModel(EventService eventService)
        {
            _eventService = eventService;
            Events = new ObservableCollection<Event>();
            AddEventCommand = new Command(async () => await GoToEventPage());
            EditEventCommand = new Command<Event>(async (eventObj) => await GoToEventPage(eventObj));

            LoadEventsCommand = new Command(async () => await LoadEventsAsync());
        }

        public ICommand LoadEventsCommand { get; }

        private bool isFirstLoad = true;

        private async Task LoadEventsAsync()
        {

            var events = await _eventService.GetAllEventsAsync();
            if (!events.Any() && isFirstLoad)
            {
                await GoToEventPage();
                isFirstLoad = false;
            }
            else
            {
                Events.Clear();
                foreach (var e in events)
                {
                    Events.Add(e);
                }
            }

        }

        private async Task GoToEventPage(Event eventObj = null)
        {
            var navParam = new ShellNavigationState($"{nameof(EventPage)}");
            if (eventObj != null)
            {
                var navigationParameters = new Dictionary<string, object>
                {
                    { "EventId", eventObj.Id }
                };
                await Shell.Current.GoToAsync(navParam, navigationParameters);
            }
            else
            {
                await Shell.Current.GoToAsync(navParam);
            }
        }
    }
}
