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

    public Class Class
    {
        set
        {
            (BindingContext as ClassViewModel).CurrentClass = value;
        }
    }
}
