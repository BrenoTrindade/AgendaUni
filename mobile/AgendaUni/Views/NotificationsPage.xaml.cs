using AgendaUni.ViewModels;

namespace AgendaUni.Views;

public partial class NotificationsPage : ContentPage
{
	public NotificationsPage(NotificationsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
