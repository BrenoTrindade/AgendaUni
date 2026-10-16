using AgendaUni.ViewModels;

namespace AgendaUni.Views;

public partial class NotificationsPage : ContentPage
{
	private readonly NotificationsViewModel _viewModel;

	public NotificationsPage(NotificationsViewModel viewModel)
	{
		_viewModel = viewModel;
		InitializeComponent();
		BindingContext = viewModel;
	}

    private async void OnEventToggled(object sender, ToggledEventArgs args)
    {
        if (args.Value == _viewModel.EventNotificationsEnabled) return;
        try
        {
            await _viewModel.SetEventNotificationsEnabledAsync(args.Value);
        }
        catch (Exception)
        {
            await DisplayAlert("Não foi possível atualizar", "Tente alterar os lembretes novamente.", "OK");
        }
        finally
        {
            EventSwitch.IsToggled = _viewModel.EventNotificationsEnabled;
        }
    }

    private async void OnScheduleToggled(object sender, ToggledEventArgs args)
    {
        if (args.Value == _viewModel.ClassScheduleNotificationsEnabled) return;
        try
        {
            await _viewModel.SetClassScheduleNotificationsEnabledAsync(args.Value);
        }
        catch (Exception)
        {
            await DisplayAlert("Não foi possível atualizar", "Tente alterar os lembretes novamente.", "OK");
        }
        finally
        {
            ScheduleSwitch.IsToggled = _viewModel.ClassScheduleNotificationsEnabled;
        }
    }
}
