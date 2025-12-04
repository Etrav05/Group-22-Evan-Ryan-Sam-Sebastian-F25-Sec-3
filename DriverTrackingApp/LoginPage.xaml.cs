using Microsoft.Maui.Controls;

namespace DriverTrackingApp;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string user = UsernameEntry.Text;
        string pass = PasswordEntry.Text;

        // Fake login values
        string fakeUser = "driver";
        string fakePass = "pass123";

        if (user == fakeUser && pass == fakePass)
        {
            var telemetry = App.Services.GetService<TelemetryService>();

            telemetry.AccountId = Guid.Parse("11111111-1111-1111-1111-111111111111"); // Hardcoded account for now

            var mainPage = App.Services.GetService<MainPage>();
            await Navigation.PushAsync(mainPage);
        }
        else
        {
            await DisplayAlert("Login Failed", "Incorrect username or password.", "OK");
        }
    }
}