using AgendaUni.ViewModels;

namespace AgendaUni.Views;

public partial class AbsenceListPage : ContentPage
{
    public AbsenceListPage(AbsenceListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as AbsenceListViewModel)?.LoadAbsencesCommand.Execute(null);
    }
}
