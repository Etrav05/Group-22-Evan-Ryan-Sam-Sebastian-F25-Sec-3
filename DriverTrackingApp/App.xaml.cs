namespace DriverTrackingApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Wrap login page in navigation so PushAsync works
            return new Window(new NavigationPage(new LoginPage()));
        }
    }
}