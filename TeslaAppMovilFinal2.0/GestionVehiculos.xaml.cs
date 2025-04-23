using TeslaAppMovilFinal2._0.Models;
using TeslaAppMovilFinal2._0.Services;
using Firebase.Database;
using Firebase.Database.Query;
using Plugin.Media;
using Plugin.Media.Abstractions;


namespace TeslaAppMovilFinal2._0;

public partial class GestionVehiculos : ContentPage
{

    private readonly FirebaseClient firebaseClient;
    private Vehiculo vehiculoSeleccionado;

    public GestionVehiculos()
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
            vehiculo.Estado = EstadoEntry.Text;
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
        EstadoEntry.Text = "";
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
            EstadoEntry.Text = vehiculo.Estado;
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

    //private string imagenLocalPath = string.Empty;
    //private async void OnSelectImageClicked(object sender, EventArgs e)
    //{
    //    var resultado = await FilePicker.PickAsync(new PickOptions
    //    {
    //        PickerTitle = "Seleccionar imagen",
    //        FileTypes = FilePickerFileType.Images
    //    });

    //    if (resultado != null) 
    //    {
    //        var fileName = Path.GetFileName(resultado.FullPath);
    //        var destino = Path.Combine(FileSystem.AppDataDirectory, fileName);

    //        using var stream = await resultado.OpenReadAsync();
    //        using var newStream = File.OpenWrite(destino);
    //        await stream.CopyToAsync(newStream);


    //        imagenLocalPath = destino;
    //        ImagenSeleccionada.Source = imagenLocalPath; // Si tienes un <Image x:Name="ImagenPreview"/>

    //    }

    private async void CerrarSesionClicked(object sender, EventArgs e)
    {
        //// Implementación para cerrar sesión
        Application.Current.MainPage = new LoginPage();
        DisplayAlert("Cerrar sesión", "Has cerrado sesión exitosamente.", "OK");
    }


}




