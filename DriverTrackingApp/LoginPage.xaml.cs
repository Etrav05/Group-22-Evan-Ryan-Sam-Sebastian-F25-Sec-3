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
        string fakePass = "password123";

        if (user == fakeUser && pass == fakePass)
        {
            await Navigation.PushAsync(new MainPage());
        }
        else
        {
            await DisplayAlert("Login Failed", "Incorrect username or password.", "OK");
        }
    }
}