using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.ApplicationModel;
using DriverTrackingApp.Services;

namespace DriverTrackingApp
{
    public partial class MainPage : ContentPage
    {
        private readonly TelemetryService _telemetry;  // Create a new telemery service instance to use in this page
        private readonly GetTrips _getTrips;

        private readonly TripHistoryPage _tripHistoryPage;

        public MainPage(TelemetryService telemetry, TripHistoryPage tripHistoryPage, GetTrips getTrips) // This constructor will be called via MAUI's DI system (auto gets the service)
        {
            InitializeComponent();   // This just loads the UI
            _telemetry = telemetry; // Saves the injected service so we can use it again
            _tripHistoryPage = tripHistoryPage;
            _getTrips = getTrips;

            StartTripBtn.Clicked += StartTripBtn_Clicked;       // Register the events we want to listen for
            EndTripBtn.Clicked += EndTripBtn_Clicked;
            TripHistoryBtn.Clicked += TripHistory_Clicked;
        }
        private async void StartTripBtn_Clicked(object sender, EventArgs e) // Async because it waits inside the mainpage for its event
        {
            await _telemetry.StartAsync(); // now runs on MAIN/UI THREAD (Needed for permissions)
                                          // Await makes it so that the UI doesnt freeze while the service is finishing
            await DisplayAlert("Trip Started", $"Your trip has started at: {DateTime.UtcNow}", "Continue");
        }                               

        private async void EndTripBtn_Clicked(object sender, EventArgs e)
        {
            await _telemetry.StopAsync();
            await DisplayAlert("Trip Ended", $"Your trip has ended at: {DateTime.UtcNow}", "Continue");
        }

        private async void TripHistory_Clicked(object sender, EventArgs e)  // TODO: Implement trip history page
        {
            await Navigation.PushAsync(_tripHistoryPage); // Navigate to the main page if login is successful
        }
    }
}
