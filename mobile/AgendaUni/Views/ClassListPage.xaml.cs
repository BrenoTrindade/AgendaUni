using AgendaUni.ViewModels;

namespace AgendaUni.Views;

public partial class ClassListPage : ContentPage
{
    public ClassListPage(ClassListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as ClassListViewModel)?.LoadClassesCommand.Execute(null);
    }
}
