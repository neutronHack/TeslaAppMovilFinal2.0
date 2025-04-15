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

        var firebase = new Services.FirebaseServiceUsuario();
        var usuario = await firebase.AutenticarUsuarioAsync(email, password);

        if (usuario != null)
        {
            // Guardar usuario en sesi�n
            Helpers.SessionManager.UsuarioActual = usuario;

            await DisplayAlert("�xito", $"Bienvenido {usuario.Nombre}", "OK");

            // Redirigir a MainPage
            Application.Current.MainPage = new AppShell();
        }
        else
        {
            await DisplayAlert("Error", "Correo o contrase�a incorrectos", "OK");
        }
    }
    private async void OnRegisterRedirect(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//RegisterPage");
    }
}
