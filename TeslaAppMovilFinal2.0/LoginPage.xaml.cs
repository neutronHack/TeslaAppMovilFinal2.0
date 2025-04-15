using Microsoft.Maui.Controls;

namespace TeslaAppMovilFinal2._0;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Error", "Completa todos los campos", "OK");
            return;
        }

        // Aqu� ir�a autenticaci�n con Firebase m�s adelante
        await DisplayAlert("Bienvenido", $"Sesi�n iniciada como:\n{email}", "OK");
    }

    private async void OnRegisterRedirect(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("RegisterPage");
    }
}
