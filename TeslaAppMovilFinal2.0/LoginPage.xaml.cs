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
            // Guardar usuario en sesión
            Helpers.SessionManager.UsuarioActual = usuario;

            await DisplayAlert("Éxito", $"Bienvenido {usuario.Nombre}", "OK");

            // Si tiene rol 1 va a AppShell, sino a GestionVehiculos
            if (usuario.IdRol == 2)
            {
                Application.Current.MainPage = new AppShell(); // usuarios normales
            }
            else
            {
                Application.Current.MainPage = new GestionVehiculos(); // otros roles
            }
        }
        else
        {
            await DisplayAlert("Error", "Correo o contraseña incorrectos", "OK");
        }
    }

    private async void OnRegisterRedirect(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//RegisterPage");
    }
}
