using AgendaUni.Models;
using AgendaUni.ViewModels;

namespace AgendaUni.Views;

[QueryProperty(nameof(Event), "Event")]
public partial class EventPage : ContentPage
{
    public EventPage(EventViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    public Event Event
    {
        set
        {
            (BindingContext as EventViewModel).CurrentEvent = value;
        }
    }
}