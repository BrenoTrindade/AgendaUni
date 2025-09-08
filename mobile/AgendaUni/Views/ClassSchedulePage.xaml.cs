namespace AgendaUni.Views;

public partial class ClassSchedulePage : ContentPage
{
	public ClassSchedulePage(ClassScheduleViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}