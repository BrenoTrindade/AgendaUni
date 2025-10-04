using AgendaUni.Models;
using AgendaUni.ViewModels;

namespace AgendaUni.Views;

[QueryProperty(nameof(Absence), "Absence")]
public partial class AbsencePage : ContentPage
{
    public AbsencePage(AbsenceViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    public Absence Absence
    {
        set
        {
            (BindingContext as AbsenceViewModel).CurrentAbsence = value;
        }
    }
}
