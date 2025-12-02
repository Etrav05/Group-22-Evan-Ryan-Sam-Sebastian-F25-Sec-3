using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.ApplicationModel;

namespace DriverTrackingApp
{
    public partial class MainPage : ContentPage
    {
        private TelemetryService _telemetry = new TelemetryService(); // Create a new telemery service instance to use in this page

        public MainPage()
        {
            InitializeComponent();

            StartTripBtn.Clicked += StartTripBtn_Clicked;
        }
        private void StartTripBtn_Clicked(object sender, EventArgs e)
        {
            Task.Run(() => _telemetry.StartAsync());
            Console.WriteLine("Telemetry started!");
        }

        /*
        private void EndTripBtn_Clicked(object sender, EventArgs e)
        {
            StopTelemetry();
        }
        */

        private void OnTripHistoryClicked(object sender, EventArgs e) {
            // Fake click action
            DisplayAlert("Trip History", $"Trip History clicked 1 times!", "OK");
        }
    }
}
