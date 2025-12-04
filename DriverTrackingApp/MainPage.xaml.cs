using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.ApplicationModel;

namespace DriverTrackingApp
{
    public partial class MainPage : ContentPage
    {
        private readonly TelemetryService _telemetry;  // Create a new telemery service instance to use in this page

        public MainPage(TelemetryService telemetry) // This constructor will be called via MAUI's DI system (auto gets the service)
        {
            InitializeComponent();   // This just loads the UI
            _telemetry = telemetry; // Saves the injected service so we can use it again

            StartTripBtn.Clicked += StartTripBtn_Clicked;       // Register the events we want to listen for
            EndTripBtn.Clicked += EndTripBtn_Clicked;
            TripHistoryBtn.Clicked += TripHistory_Clicked;
        }
        private async void StartTripBtn_Clicked(object sender, EventArgs e) // Async because it waits inside the mainpage for its event
        {
            await _telemetry.StartAsync(); // now runs on MAIN/UI THREAD (Needed for permissions)
        }                                 // Await makes it so that the UI doesnt freeze while the service is finishing

        private async void EndTripBtn_Clicked(object sender, EventArgs e)
        {
            await _telemetry.StopAsync(); 
        }

        private void TripHistory_Clicked(object sender, EventArgs e)  // TODO: Implement trip history page
        {
            DisplayAlert("Trip History", $"Trip History clicked", "OK"); // Fake click action
        }
    }
}
