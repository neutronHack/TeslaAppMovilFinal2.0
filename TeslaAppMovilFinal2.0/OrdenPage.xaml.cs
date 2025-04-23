using TeslaAppMovilFinal2._0.Models;
using TeslaAppMovilFinal2._0.Services;
namespace TeslaAppMovilFinal2._0;
public partial class OrdenPage : ContentPage
{
    public Vehiculo Vehiculo { get; set; }
    public string Color { get; set; } = "Stealth Grey";
    public string Aros { get; set; } = "Crossflow Negros";
    public string Interior { get; set; } = "All Black";
    public string ImagenPersonalizada { get; set; }


    private readonly FirebaseServicePersonalizacion _firebaseService = new();

    public OrdenPage(Vehiculo vehiculo)
    {
        InitializeComponent();
        Vehiculo = vehiculo;
        Color = "Stealth Grey";
        ImagenPersonalizada = ObtenerImagenPorColor(vehiculo.Modelo, Color);
        BindingContext = this;
    }

    private void OnColorSelected(object sender, EventArgs e)
    {
        var btn = sender as ImageButton;
        Color = btn?.CommandParameter?.ToString() ?? "";

        ImagenPersonalizada = ObtenerImagenPorColor(Vehiculo.Modelo, Color); // 👈 aquí actualiza la imagen
        OnPropertyChanged(nameof(Color));
        OnPropertyChanged(nameof(ImagenPersonalizada)); // 👈 asegura refresco en XAML

        System.Diagnostics.Debug.WriteLine($"🔍 Modelo: {Vehiculo.Modelo} | Color: {Color}");
        System.Diagnostics.Debug.WriteLine($"🖼 Imagen esperada: {ImagenPersonalizada}");


    }

    private string ObtenerImagenPorColor(string modelo, string color)
    {
   
        string modeloKey = modelo.Replace("Tesla ", "").Replace(" ", "").ToLower();

        string colorKey = color switch
        {
            "Stealth Grey" => "stealthgrey",
            "Pearl White" => "white",
            "Deep Blue" => "blue",
            "Solid Black" => "black",
            "Red Multi-Coat" => "red",
            "Silver" => "quicksilver",
            _ => "stealthgrey"
        };

        return $"{modeloKey}_{colorKey}.png"; // Ej: modely_stealthgrey.png
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

    private async void OnIrAComprasClicked(object sender, EventArgs e)
    {
        var personalizacion = new PersonalizacionVehiculo
        {
            IdUsuario = Helpers.SessionManager.UsuarioActual?.IdUsuario ?? "",
            IdVehiculo = Vehiculo.Modelo,
            Color = this.Color,
            Aros = this.Aros,
            Interior = this.Interior
        };

        await Navigation.PushAsync(new ComprasPage(personalizacion));
    }

    private async void OnIrAReservasClicked(object sender, EventArgs e)
    {
        var personalizacion = new PersonalizacionVehiculo
        {
            IdUsuario = Helpers.SessionManager.UsuarioActual?.IdUsuario ?? "",
            IdVehiculo = Vehiculo.Modelo,
            Color = this.Color,
            Aros = this.Aros,
            Interior = this.Interior
        };

        await Navigation.PushAsync(new ReservasPage(personalizacion));
    }

}

