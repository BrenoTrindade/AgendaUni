namespace AgendaUni.Views;

public partial class EventPage : ContentPage
{
    public EventPage(EventViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}