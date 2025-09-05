using AgendaUni.ViewModels;

namespace AgendaUni.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel mainPageViewModel)
        {
            InitializeComponent();
            BindingContext = mainPageViewModel;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is BaseViewModel baseViewModel)
            {
                await baseViewModel.OnAppearingAsync();
            }
        }
    }
}
