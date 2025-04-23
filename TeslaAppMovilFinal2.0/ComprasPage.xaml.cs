using MailKit.Net.Smtp;
using MimeKit;
using TeslaAppMovilFinal2._0.Models;
using TeslaAppMovilFinal2._0.Services;

namespace TeslaAppMovilFinal2._0;

public partial class ComprasPage : ContentPage
{
    public PersonalizacionVehiculo Personalizacion { get; set; }

    // Datos del cliente
    public string Nombre { get; set; }
    public string Apellido1 { get; set; }
    public string Apellido2 { get; set; }
    public string Cedula { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }

    // Datos tarjeta
    public string NumeroTarjeta { get; set; }
    public string FechaCaducidad { get; set; }
    public string CVV { get; set; }

    public ComprasPage(PersonalizacionVehiculo personalizacion)
    {
        InitializeComponent();
        Personalizacion = personalizacion;
        BindingContext = this;
    }

    private async void OnFinalizarCompraClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Nombre) || string.IsNullOrWhiteSpace(NumeroTarjeta) || string.IsNullOrWhiteSpace(CVV))
        {
            await DisplayAlert("Error", "Por favor, complete todos los campos requeridos", "OK");
            return;
        }

        // Validar que Teléfono contenga solo números
        if (!Telefono.All(char.IsDigit))
        {
            await DisplayAlert("Error", "El teléfono debe contener solo números.", "OK");
            return;
        }

        // Validar correo electrónico
        if (!System.Text.RegularExpressions.Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            await DisplayAlert("Error", "Ingrese un correo electrónico válido.", "OK");
            return;
        }

        // Validar número de tarjeta
        if (NumeroTarjeta.Length != 16 || !NumeroTarjeta.All(char.IsDigit))
        {
            await DisplayAlert("Error", "El número de tarjeta debe tener exactamente 16 dígitos numéricos.", "OK");
            return;
        }

        // Validar formato de fecha MM/YY
        if (!System.Text.RegularExpressions.Regex.IsMatch(FechaCaducidad, @"^(0[1-9]|1[0-2])\/\d{2}$"))
        {
            await DisplayAlert("Error", "La fecha de caducidad debe tener el formato MM/YY.", "OK");
            return;
        }

        // Validar CVV
        if (CVV.Length != 3 || !CVV.All(char.IsDigit))
        {
            await DisplayAlert("Error", "El CVV debe tener exactamente 3 dígitos numéricos.", "OK");
            return;
        }

        var compra = new CompraVehiculo
        {
            IdUsuario = Helpers.SessionManager.UsuarioActual?.IdUsuario ?? "",
            Nombre = this.Nombre,
            Apellido1 = this.Apellido1,
            Apellido2 = this.Apellido2,
            Cedula = this.Cedula,
            Telefono = this.Telefono,
            Email = this.Email,
            NumeroTarjeta = this.NumeroTarjeta,
            FechaCaducidad = this.FechaCaducidad,
            CVV = this.CVV,
            Modelo = Personalizacion.IdVehiculo,
            Color = Personalizacion.Color,
            Aros = Personalizacion.Aros,
            Interior = Personalizacion.Interior
        };

        var firebase = new FirebaseServiceCompras();
        bool exito = await firebase.GuardarCompraAsync(compra);
        await EnviarCorreoConfirmacion(compra.Email, compra);

        if (exito)
        {
            await DisplayAlert("¡Éxito!", "Tu compra ha sido registrada con éxito", "OK");
            await Navigation.PopToRootAsync();
        }
        else
        {
            await DisplayAlert("Error", "No se pudo guardar la compra", "OK");
        }
    }

    private async Task EnviarCorreoConfirmacion(string destinatario, CompraVehiculo compra)
    {
        var mensaje = new MimeMessage();
        mensaje.From.Add(new MailboxAddress("Tesla Costa Rica", "danielalpizarb@gmail.com"));
        mensaje.To.Add(new MailboxAddress(compra.Nombre, destinatario));
        mensaje.Subject = "Confirmación de Compra - Tesla Costa Rica";

        // Fix the issue by using MimeKit's TextPart class
        mensaje.Body = new MimeKit.TextPart("plain")
        {
            Text = $@"
            ¡Gracias por tu compra, {compra.Nombre}!

            Aquí están los detalles de tu compra:

            Modelo: {compra.Modelo}
            Color: {compra.Color}
            Aros: {compra.Aros}
            Interior: {compra.Interior}

            Nombre completo: {compra.Nombre} {compra.Apellido1} {compra.Apellido2}
            Cédula: {compra.Cedula}
            Teléfono: {compra.Telefono}
            Correo: {compra.Email}

            La compra ha sido registrada exitosamente. Pronto nos pondremos en contacto contigo.

            Saludos,
            Tesla Costa Rica
            "
        };

        using var cliente = new SmtpClient();
        try
        {
            await cliente.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await cliente.AuthenticateAsync("danielalpizarb@gmail.com", "qhrezlcxlwrayqac");
            await cliente.SendAsync(mensaje);
            await cliente.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error de correo", $"No se pudo enviar el correo: {ex.Message}", "OK");
        }
    }

}
