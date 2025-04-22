using TeslaAppMovilFinal2._0.Models;

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
}