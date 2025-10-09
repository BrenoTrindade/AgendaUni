using AgendaUni.Models;
using AgendaUni.ViewModels;

namespace AgendaUni.Views;

[QueryProperty(nameof(Class), "Class")]
public partial class ClassPage : ContentPage
{
    public ClassPage(ClassViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ClassViewModel vm && vm.ClassId > 0)
        {
            vm.ClassId = vm.ClassId;
        }
    }

    public Class Class
    {
        set
        {
            (BindingContext as ClassViewModel).CurrentClass = value;
        }
    }

}
