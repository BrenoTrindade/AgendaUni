using AgendaUni.ViewModels;

namespace AgendaUni.Views;

public partial class ClassScheduleListPage : ContentPage
{
    public ClassScheduleListPage(ClassScheduleListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as ClassScheduleListViewModel)?.LoadClassSchedulesCommand.Execute(null);
    }
}
