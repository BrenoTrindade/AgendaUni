namespace AgendaUni
{
    public partial class App : Application
    {
        public App(AppDbContext dbContext)
        {
            InitializeComponent();
            dbContext.InitializeDatabase();
            Current.UserAppTheme = AppTheme.Light;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}