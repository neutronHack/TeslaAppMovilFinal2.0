using TeslaAppMovilFinal2._0.Models;
using Firebase.Database;
using Firebase.Database.Query;

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

    private async void GuardarVehiculo(object sender, EventArgs e)
    {
        if (vehiculoSeleccionado == null)
        {
            var nuevoVehiculo = new Vehiculo
            {
                Codigo = CodigoEntry.Text,
                Marca = MarcaEntry.Text,
                Modelo = ModeloEntry.Text,
                Año = AñoEntry.Text,
                Precio = int.Parse(PrecioEntry.Text),
                Kilometraje = KilometrajeEntry.Text,
                Estado = EstadoEntry.Text,
                ImagenUrl = EnlaceImagenEntry.Text
            };

            await firebaseClient.Child("vehiculos").Child(nuevoVehiculo.Codigo).PutAsync(nuevoVehiculo);
            await DisplayAlert("Éxito", "Vehículo agregado correctamente", "OK");
        }
        else
        {
            vehiculoSeleccionado.Marca = MarcaEntry.Text;
            vehiculoSeleccionado.Modelo = ModeloEntry.Text;
            vehiculoSeleccionado.Año = AñoEntry.Text;
            vehiculoSeleccionado.Precio = int.Parse(PrecioEntry.Text);
            vehiculoSeleccionado.Kilometraje = KilometrajeEntry.Text;
            vehiculoSeleccionado.Estado = EstadoEntry.Text;
            vehiculoSeleccionado.ImagenUrl = EnlaceImagenEntry.Text;

            await firebaseClient.Child("vehiculos").Child(vehiculoSeleccionado.Codigo).PutAsync(vehiculoSeleccionado);
            await DisplayAlert("Actualizado", "Vehículo modificado correctamente", "OK");
        }

        LimpiarCampos();
        CargarVehiculos();
    }

    private async void CargarVehiculos()
    {
        var vehiculos = await firebaseClient.Child("vehiculos").OnceAsync<Vehiculo>();
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
        GuardarButton.Text = "Agregar Vehículo";
        FormularioTitulo.Text = "Agregar Vehículo";
        CodigoEntry.IsEnabled = true;
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
            AñoEntry.Text = vehiculo.Año;
            PrecioEntry.Text = vehiculo.Precio.ToString();
            KilometrajeEntry.Text = vehiculo.Kilometraje;
            EstadoEntry.Text = vehiculo.Estado;
            EnlaceImagenEntry.Text = vehiculo.ImagenUrl;

            FormularioTitulo.Text = "Editar Vehículo";
            GuardarButton.Text = "Editar Vehículo";
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
                await firebaseClient.Child("vehiculos").Child(vehiculo.Codigo).DeleteAsync();
                await DisplayAlert("Eliminado", "Vehículo eliminado correctamente", "OK");
                CargarVehiculos();
            }
        }
    }

    private async void CerrarSesionClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new LoginPage();
        await DisplayAlert("Cerrar sesión", "Has cerrado sesión exitosamente.", "OK");
    }
}
