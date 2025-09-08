using AgendaUni.ViewModels;

namespace AgendaUni.Views;

public partial class AbsencePage : ContentPage
{
    public AbsencePage(AbsenceViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
