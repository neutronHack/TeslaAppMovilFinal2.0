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

        if (usuario.IdUsuario == "48143e95-8cc6-4a17-a7c5-ae9a22b3a83c")
        {
            Helpers.SessionManager.UsuarioActual = usuario;
            await DisplayAlert("Éxito", $"Bienvenido {usuario.Nombre}", "OK");

            // Redirigir a AdminPage
            Application.Current.MainPage = new AdminPage();
        }
        else
        {
            Helpers.SessionManager.UsuarioActual = usuario;
            await DisplayAlert("Éxito", $"Bienvenido {usuario.Nombre}", "OK");
            // Redirigir a MainPage
            Application.Current.MainPage = new AppShell();
        }
        //if (usuario == null)
        //{
        //    // Guardar usuario en sesión
        //    Helpers.SessionManager.UsuarioActual = usuario;

        //    await DisplayAlert("Éxito", $"Bienvenido {usuario.Nombre}", "OK");

        //    // Redirigir a MainPage
        //    Application.Current.MainPage = new AppShell();
        //}
        //else
        //{
        //    await DisplayAlert("Error", "Correo o contraseña incorrectos", "OK");
        //}
    }
    private async void OnRegisterRedirect(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//RegisterPage");
    }
}
