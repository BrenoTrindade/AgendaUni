using AgendaUni.ViewModels;

namespace AgendaUni.Views;

public partial class EventListPage : ContentPage
{
    public EventListPage(EventListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as EventListViewModel)?.LoadEventsCommand.Execute(null);
    }
}
