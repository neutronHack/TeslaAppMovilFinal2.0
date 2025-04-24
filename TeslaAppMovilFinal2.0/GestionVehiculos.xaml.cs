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
                A�o = A�oEntry.Text,
                Precio = int.Parse(PrecioEntry.Text),
                Kilometraje = KilometrajeEntry.Text,
                Estado = EstadoEntry.Text,
                ImagenUrl = EnlaceImagenEntry.Text
            };

            await firebaseClient.Child("vehiculos").Child(nuevoVehiculo.Codigo).PutAsync(nuevoVehiculo);
            await DisplayAlert("�xito", "Veh�culo agregado correctamente", "OK");
        }
        else
        {
            vehiculoSeleccionado.Marca = MarcaEntry.Text;
            vehiculoSeleccionado.Modelo = ModeloEntry.Text;
            vehiculoSeleccionado.A�o = A�oEntry.Text;
            vehiculoSeleccionado.Precio = int.Parse(PrecioEntry.Text);
            vehiculoSeleccionado.Kilometraje = KilometrajeEntry.Text;
            vehiculoSeleccionado.Estado = EstadoEntry.Text;
            vehiculoSeleccionado.ImagenUrl = EnlaceImagenEntry.Text;

            await firebaseClient.Child("vehiculos").Child(vehiculoSeleccionado.Codigo).PutAsync(vehiculoSeleccionado);
            await DisplayAlert("Actualizado", "Veh�culo modificado correctamente", "OK");
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
        A�oEntry.Text = "";
        PrecioEntry.Text = "";
        KilometrajeEntry.Text = "";
        EstadoEntry.Text = "";
        EnlaceImagenEntry.Text = "";
        vehiculoSeleccionado = null;
        GuardarButton.Text = "Agregar Veh�culo";
        FormularioTitulo.Text = "Agregar Veh�culo";
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
            A�oEntry.Text = vehiculo.A�o;
            PrecioEntry.Text = vehiculo.Precio.ToString();
            KilometrajeEntry.Text = vehiculo.Kilometraje;
            EstadoEntry.Text = vehiculo.Estado;
            EnlaceImagenEntry.Text = vehiculo.ImagenUrl;

            FormularioTitulo.Text = "Editar Veh�culo";
            GuardarButton.Text = "Editar Veh�culo";
        }
    }

    private async void OnEliminarVehiculoClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var vehiculo = button?.BindingContext as Vehiculo;

        if (vehiculo != null)
        {
            var confirm = await DisplayAlert("Confirmar", $"�Eliminar veh�culo {vehiculo.Codigo}?", "S�", "No");
            if (confirm)
            {
                await firebaseClient.Child("vehiculos").Child(vehiculo.Codigo).DeleteAsync();
                await DisplayAlert("Eliminado", "Veh�culo eliminado correctamente", "OK");
                CargarVehiculos();
            }
        }
    }

    private async void CerrarSesionClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new LoginPage();
        await DisplayAlert("Cerrar sesi�n", "Has cerrado sesi�n exitosamente.", "OK");
    }
}
