using System.ComponentModel;
using TeslaAppMovilFinal2._0.Models;
using TeslaAppMovilFinal2._0.Services;
using MimeKit;
using MailKit.Net.Smtp;

namespace TeslaAppMovilFinal2._0;

public partial class ComprarReservaPage : ContentPage, INotifyPropertyChanged
{
    public Reserva Reserva { get; set; }
    public string idReserva { get; set; }
    public string NumeroTarjeta { get; set; }
    public string FechaCaducidad { get; set; }
    public string CVV { get; set; }

    public string Precio { get; set; }

    public string ReservaResumen =>
        $"Modelo: {Reserva.idVehiculo}\n" +
        $"Color: {Reserva.color}\n" +
        $"Aros: {Reserva.aros}\n" +
        $"Interior: {Reserva.interior}\n\n" +
        $"Precio: {Precio}\n\n" +
        $"Cliente: {Reserva.Nombre} {Reserva.Apellido1} {Reserva.Apellido2}\n" +
        $"Cédula: {Reserva.Cedula}\n" +
        $"Correo: {Reserva.Email}\n" +
        $"Teléfono: {Reserva.Telefono}";

    public ComprarReservaPage(Reserva reserva, string idReserva)
    {
        InitializeComponent();
        Reserva = reserva;
        this.idReserva = idReserva;
        Precio = ObtenerPrecioPorModelo(reserva.idVehiculo); 
        BindingContext = this;
    }

    public string ObtenerPrecioPorModelo(string modelo)
    {
        return modelo switch
        {
            "Tesla Model S" => "$80,000",
            "Tesla Model Y" => "$41,500",
            "Tesla Model X" => "$85,000",
            "Tesla Model 3" => "$34,500",
            _ => "$0",
        };
    }

    private async void OnFinalizarCompraClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NumeroTarjeta) || string.IsNullOrWhiteSpace(FechaCaducidad) || string.IsNullOrWhiteSpace(CVV))
        {
            await DisplayAlert("Error", "Por favor, complete todos los campos de la tarjeta.", "OK");
            return;
        }

        if (NumeroTarjeta.Length != 16 || !NumeroTarjeta.All(char.IsDigit))
        {
            await DisplayAlert("Error", "El número de tarjeta debe tener 16 dígitos numéricos.", "OK");
            return;
        }

        if (!System.Text.RegularExpressions.Regex.IsMatch(FechaCaducidad, @"^(0[1-9]|1[0-2])\/\d{2}$"))
        {
            await DisplayAlert("Error", "La fecha de caducidad debe tener el formato MM/YY.", "OK");
            return;
        }

        if (CVV.Length != 3 || !CVV.All(char.IsDigit))
        {
            await DisplayAlert("Error", "El CVV debe tener 3 dígitos numéricos.", "OK");
            return;
        }

        var compra = new CompraVehiculo
        {
            IdUsuario = Helpers.SessionManager.UsuarioActual?.IdUsuario ?? "",
            Nombre = Reserva.Nombre,
            Apellido1 = Reserva.Apellido1,
            Apellido2 = Reserva.Apellido2,
            Cedula = Reserva.Cedula,
            Telefono = Reserva.Telefono,
            Email = Reserva.Email,
            NumeroTarjeta = this.NumeroTarjeta,
            FechaCaducidad = this.FechaCaducidad,
            CVV = this.CVV,
            Modelo = Reserva.idVehiculo,
            Color = Reserva.color,
            Aros = Reserva.aros,
            Interior = Reserva.interior
        };

        var firebase = new FirebaseServiceCompras();
        bool exito = await firebase.GuardarCompraAsync(compra);
        await EnviarCorreoConfirmacion(compra.Email, compra);

        if (exito)
        {
            await DisplayAlert("¡Éxito!", "Tu compra ha sido registrada con éxito", "OK");
            FirebaseServiceReservas _firebaseService = new();
            await _firebaseService.EliminarReservaAsync(idReserva);
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

        mensaje.Body = new TextPart("plain")
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
            
            cliente.ServerCertificateValidationCallback = (s, c, h, e) => true;

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
