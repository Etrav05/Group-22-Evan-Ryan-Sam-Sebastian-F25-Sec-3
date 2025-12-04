namespace DriverTrackingApp
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; set; }

        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            Services = serviceProvider;

            // Show LoginPage first via a DI
            var loginPage = Services.GetRequiredService<LoginPage>();
            MainPage = new NavigationPage(loginPage);
        }
    }
}