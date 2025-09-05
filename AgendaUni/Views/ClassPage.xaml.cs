using AgendaUni.ViewModels;

namespace AgendaUni.Views; 


public partial class ClassPage : ContentPage
{
    public ClassPage(ClassViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

}
