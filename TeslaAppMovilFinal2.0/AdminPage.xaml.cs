using TeslaAppMovilFinal2._0.Models;
using TeslaAppMovilFinal2._0.Services;
using Firebase.Database;
using Firebase.Database.Query;


namespace TeslaAppMovilFinal2._0;

public partial class AdminPage : ContentPage
{

    private readonly FirebaseClient firebaseClient;
    private Vehiculo vehiculoSeleccionado;

    public AdminPage()
	{
		InitializeComponent();
        firebaseClient = new FirebaseClient("https://teslaappmovil-default-rtdb.firebaseio.com/");
        CargarVehiculos();

    }

    private async void AgregarVehiculo(object sender, EventArgs e)
    {
       var vehiculo = new Vehiculo();
        {
            vehiculo.Codigo = CodigoEntry.Text;
            vehiculo.Marca = MarcaEntry.Text;
            vehiculo.Modelo = PrecioEntry.Text;
            vehiculo.Año = AñoEntry.Text;
            vehiculo.Precio = int.Parse(PrecioEntry.Text);
            vehiculo.Kilometraje = KilometrajeEntry.Text;
            vehiculo.ImagenUrl = EnlaceImagenEntry.Text;
        };

        await firebaseClient
            .Child("vehiculos")
            .Child(vehiculo.Codigo)
            .PutAsync(vehiculo);

        await DisplayAlert("Éxito", "Vehículo agregado correctamente", "OK");
        LimpiarCampos();
        CargarVehiculos();
    }

    private async void CargarVehiculos()
    {
        var vehiculos = await firebaseClient
            .Child("vehiculos")
            .OnceAsync<Vehiculo>();

        VehiculosCollectionView.ItemsSource = vehiculos.Select(x => x.Object).ToList();
    }

    private void LimpiarCampos()
    {
        CodigoEntry.Text = "";
        MarcaEntry.Text = "";
        ModeloEntry.Text = "";
        AñoEntry.Text = "";
        PrecioEntry.Text = "";
        KilometrajeEntry.Text = "";
        EstadoPick.SelectedIndex = -1;
        EnlaceImagenEntry.Text = "";
        vehiculoSeleccionado = null;
    }

    private void OnEditarVehiculoClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var vehiculo = button?.BindingContext as Vehiculo;

        if (vehiculo != null)
        {
            vehiculoSeleccionado = vehiculo;
            CodigoEntry.Text = vehiculo.Codigo;
            MarcaEntry.Text = vehiculo.Marca;
            ModeloEntry.Text = vehiculo.Modelo;
            AñoEntry.Text = vehiculo.Año.ToString();
            PrecioEntry.Text = vehiculo.Precio.ToString();
            KilometrajeEntry.Text = vehiculo.Kilometraje.ToString();
            EstadoPick.SelectedItem = vehiculo.Estado;
            EnlaceImagenEntry.Text = vehiculo.ImagenUrl;
        }
    }

    private async void OnEliminarVehiculoClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var vehiculo = button?.BindingContext as Vehiculo;

        if (vehiculo != null)
        {
            var confirm = await DisplayAlert("Confirmar", $"¿Eliminar vehículo {vehiculo.Codigo}?", "Sí", "No");
            if (confirm)
            {
                await firebaseClient
                    .Child("vehiculos")
                    .Child(vehiculo.Codigo)
                    .DeleteAsync();

                await DisplayAlert("Eliminado", "Vehículo eliminado correctamente", "OK");
                CargarVehiculos();
            }
        }
    }

    //private async void OnSelectImageClicked(object sender, EventArgs e)
    //{
    //    var mediafile = await ImagenSeleccionada
    //}


}