using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.ApplicationModel;

namespace DriverTrackingApp
{
    public partial class MainPage : ContentPage
    {
        private readonly TelemetryService _telemetry;  // Create a new telemery service instance to use in this page

        public MainPage(TelemetryService telemetry)
        {
            InitializeComponent();
            _telemetry = telemetry;

            StartTripBtn.Clicked += StartTripBtn_Clicked;
            EndTripBtn.Clicked += EndTripBtn_Clicked;
        }
        private async void StartTripBtn_Clicked(object sender, EventArgs e)
        {
            await _telemetry.StartAsync();  // now runs on MAIN/UI THREAD (Permissions again)
        }

        private async void EndTripBtn_Clicked(object sender, EventArgs e)
        {
            await _telemetry.StopAsync(); 
        }

        private void TripHistory_Clicked(object sender, EventArgs e) 
        {
            DisplayAlert("Trip History", $"Trip History clicked", "OK"); // Fake click action
        }
    }
}
