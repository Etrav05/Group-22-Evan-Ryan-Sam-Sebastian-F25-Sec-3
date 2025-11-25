using System;
using Microsoft.Maui.Controls;

namespace DriverTrackingApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            try
            {
                InitializeComponent();
            }
            catch (InvalidOperationException ex) when (ex.Message?.Contains("VisualStateGroup Names must be unique") == true)
            {
                // Log helpful details, then rethrow so you still get the exception in debugger
                System.Diagnostics.Debug.WriteLine("Duplicate VisualStateGroup name detected in MainPage.xaml. Search for 'VisualStateGroup' and ensure each group's x:Name is unique within the same element.");
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw;
            }
        }

        private void OnStartTripClicked(object sender, EventArgs e) {
            // Fake click action
            DisplayAlert("Start Trip", $"Start Trip clicked 1 times!", "OK");
        }

        private void OnEndTripClicked(object sender, EventArgs e) {
            // Fake click action
            DisplayAlert("End Trip", $"End Trip clicked 1 times!", "OK");
        }

        private void OnTripHistoryClicked(object sender, EventArgs e) {
            // Fake click action
            DisplayAlert("Trip History", $"Trip History clicked 1 times!", "OK");
        }
    }
}
