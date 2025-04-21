using TeslaAppMovilFinal2._0.Models;
using TeslaAppMovilFinal2._0.Services;
namespace TeslaAppMovilFinal2._0;
public partial class OrdenPage : ContentPage
{
    public Vehiculo Vehiculo { get; set; }
    public string Color { get; set; } = "Stealth Grey";
    public string Aros { get; set; } = "Crossflow Negros";
    public string Interior { get; set; } = "All Black";

    private readonly FirebaseServicePersonalizacion _firebaseService = new();

    public OrdenPage(Vehiculo vehiculo)
    {
        InitializeComponent();
        Vehiculo = vehiculo;
        BindingContext = this;
    }

    private void OnColorSelected(object sender, EventArgs e)
    {
        var btn = sender as ImageButton;
        Color = btn?.CommandParameter?.ToString() ?? "";
        OnPropertyChanged(nameof(Color));
    }

    private void OnArosSelected(object sender, EventArgs e)
    {
        var btn = sender as ImageButton;
        Aros = btn?.CommandParameter?.ToString() ?? "";
        OnPropertyChanged(nameof(Aros));
    }

    private void OnInteriorSelected(object sender, EventArgs e)
    {
        var btn = sender as ImageButton;
        Interior = btn?.CommandParameter?.ToString() ?? "";
        OnPropertyChanged(nameof(Interior));
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        var personalizacion = new PersonalizacionVehiculo
        {
            IdUsuario = Helpers.SessionManager.UsuarioActual?.IdUsuario ?? "",
            IdVehiculo = Vehiculo.Modelo,
            Color = this.Color,
            Aros = this.Aros,
            Interior = this.Interior
        };

        bool exito = await _firebaseService.GuardarPersonalizacionAsync(personalizacion);
        if (exito)
            await DisplayAlert("Éxito", "Tu personalización ha sido guardada", "OK");
        else
            await DisplayAlert("Error", "Hubo un problema al guardar", "OK");
    }
}

    