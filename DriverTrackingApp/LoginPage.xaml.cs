using Microsoft.Maui.Controls;

namespace DriverTrackingApp;

public partial class LoginPage : ContentPage
{
    private readonly TelemetryService _telemetry;
    private readonly MainPage _mainPage;
    public LoginPage(TelemetryService telemetry, MainPage mainPage)
    {
        InitializeComponent(); // Set up the UI components

        _telemetry = telemetry;
        _mainPage = mainPage;

        // The login page is set up to already have the event wired in XAML so no need to do it here
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string user = UsernameEntry.Text; // Creates both the users entries as variables
        string pass = PasswordEntry.Text;

        // Fake login values
        string fakeUser = "driver";
        string fakePass = "pass123";

        if (user == fakeUser && pass == fakePass) // Checks them against the hardcoded values
        {
            _telemetry.AccountId = Guid.Parse("11111111-1111-1111-1111-111111111111"); // Hardcoded account for now

            Application.Current.MainPage = new NavigationPage(_mainPage);
        }
        else
        {
            await DisplayAlert("Login Failed", "Incorrect username or password.", "OK");
        }
    }
}