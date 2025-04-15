namespace TeslaAppMovilFinal2._0;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
	}
    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        string nombre = NombreEntry.Text;
        string apellido = ApellidoEntry.Text;
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;
        string telefono = TelefonoEntry.Text;

        if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellido) ||
            string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(telefono))
        {
            await DisplayAlert("Error", "Completa todos los campos", "OK");
            return;
        }

        var usuario = new Models.Usuario
        {
            Nombre = nombre,
            Apellido = apellido,
            Email = email,
            Contraseña = password,
            Telefono = telefono,
            IdRol = 2 // Por ejemplo, 2 = Usuario común
        };

        var firebase = new Services.FirebaseServiceUsuario();
        bool exito = await firebase.RegistrarUsuarioAsync(usuario);

        if (exito)
        {
            await DisplayAlert("Éxito", "Registro completado", "OK");
            await Shell.Current.GoToAsync("//LoginPage");
        }
        else
        {
            await DisplayAlert("Error", "No se pudo registrar el usuario", "OK");
        }
    }
    private async void OnLoginRedirect(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }

}