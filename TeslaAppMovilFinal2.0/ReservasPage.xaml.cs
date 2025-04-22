using MailKit.Net.Smtp;
using MimeKit;
using TeslaAppMovilFinal2._0.Models;
using TeslaAppMovilFinal2._0.Services;

namespace TeslaAppMovilFinal2._0;

public partial class ReservasPage : ContentPage
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

    public ReservasPage(PersonalizacionVehiculo personalizacion)
    {
        InitializeComponent();
        Personalizacion = personalizacion;
        BindingContext = this;
    }

    private async void OnFinalizarReservaClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Nombre) || string.IsNullOrWhiteSpace(Apellido1) ||
            string.IsNullOrWhiteSpace(Apellido2) || string.IsNullOrWhiteSpace(Cedula) ||
            string.IsNullOrWhiteSpace(Telefono) || string.IsNullOrWhiteSpace(Email))
        {
            await DisplayAlert("Error", "Por favor, complete todos los campos requeridos", "OK");
            return;
        }

        if (!Telefono.All(char.IsDigit))
        {
            await DisplayAlert("Error", "El teléfono debe contener solo números.", "OK");
            return;
        }

        if (!System.Text.RegularExpressions.Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            await DisplayAlert("Error", "Ingrese un correo electrónico válido.", "OK");
            return;
        }

        var reserva = new Reserva
        {
            IdUsuario = Helpers.SessionManager.UsuarioActual?.IdUsuario ?? "",
            Nombre = this.Nombre,
            Apellido1 = this.Apellido1,
            Apellido2 = this.Apellido2,
            Cedula = this.Cedula,
            Telefono = this.Telefono,
            Email = this.Email,
            idVehiculo = Personalizacion.IdVehiculo,
            color = Personalizacion.Color,
            aros = Personalizacion.Aros,
            interior = Personalizacion.Interior
        };

        var firebase = new FirebaseServiceReservas();
        bool exito = await firebase.GuardarReservaAsync(reserva);
        await EnviarCorreoConfirmacionReserva(reserva.Email, reserva);

        if (exito)
        {
            await DisplayAlert("¡Reserva Exitosa!", "Tu reserva ha sido registrada con éxito", "OK");
            await Navigation.PopToRootAsync();
        }
        else
        {
            await DisplayAlert("Error", "No se pudo guardar la reserva", "OK");
        }
    }

    private async Task EnviarCorreoConfirmacionReserva(string destinatario, Reserva reserva)
    {
        var mensaje = new MimeMessage();
        mensaje.From.Add(new MailboxAddress("Tesla Costa Rica", "danielalpizarb@gmail.com"));
        mensaje.To.Add(new MailboxAddress(reserva.Nombre, destinatario));
        mensaje.Subject = "Confirmación de Reserva - Tesla Costa Rica";

        mensaje.Body = new TextPart("plain")
        {
            Text = $@"
        ¡Gracias por tu reserva, {reserva.Nombre}!

        Aquí están los detalles de tu reserva:

        Modelo: {reserva.idVehiculo}
        Color: {reserva.color}
        Aros: {reserva.aros}
        Interior: {reserva.interior}

        Nombre completo: {reserva.Nombre} {reserva.Apellido1} {reserva.Apellido2}
        Cédula: {reserva.Cedula}
        Teléfono: {reserva.Telefono}
        Correo: {reserva.Email}

        La reserva ha sido registrada exitosamente. Pronto nos pondremos en contacto contigo.

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