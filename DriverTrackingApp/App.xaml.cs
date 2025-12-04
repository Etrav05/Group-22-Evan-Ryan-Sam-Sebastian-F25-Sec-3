namespace DriverTrackingApp
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; set; } // public static service provider to access services throughout the app

        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent(); // Initializes the XAML components like usual
            Services = serviceProvider; // This will store the DI provider so everything can retrieve dependencies (services) when needed

            // Show LoginPage first via a DI
            var loginPage = Services.GetRequiredService<LoginPage>(); // Creates the login page
            MainPage = new NavigationPage(loginPage);                // This is a navigation page so we can push/pop the login on top of it
        }
    }
}