using AgendaUni.Models;
using AgendaUni.ViewModels;

namespace AgendaUni.Views;

[QueryProperty(nameof(ClassSchedule), "ClassSchedule")]
public partial class ClassSchedulePage : ContentPage
{
    public ClassSchedulePage(ClassScheduleViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    public ClassSchedule ClassSchedule
    {
        set
        {
            (BindingContext as ClassScheduleViewModel).CurrentClassSchedule = value;
        }
    }
}