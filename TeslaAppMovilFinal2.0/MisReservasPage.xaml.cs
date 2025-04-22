using TeslaAppMovilFinal2._0.Models;
using TeslaAppMovilFinal2._0.Services;
using System.Net.Http.Json;

namespace TeslaAppMovilFinal2._0;

public partial class MisReservasPage : ContentPage
{
    private readonly FirebaseServiceReservas _firebaseService = new();

    public MisReservasPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarReservas();
    }

    private async void CargarReservas()
    {
        ReservasLayout.Children.Clear();
        string idUsuario = Helpers.SessionManager.UsuarioActual?.IdUsuario ?? "";

        if (string.IsNullOrEmpty(idUsuario))
        {
            await DisplayAlert("Error", "Sesión no iniciada", "OK");
            await Navigation.PopToRootAsync();
            return;
        }

        var reservas = await _firebaseService.ObtenerReservasPorUsuarioAsync(idUsuario);

        if (reservas == null || reservas.Count == 0)
        {
            ReservasLayout.Children.Add(new Label
            {
                Text = "No tienes reservas registradas.",
                FontSize = 18,
                HorizontalOptions = LayoutOptions.Center
            });
            return;
        }

        foreach (var reserva in reservas)
        {
            var frame = new Frame
            {
                BorderColor = Colors.Gray,
                CornerRadius = 12,
                Padding = 10,
                Content = new VerticalStackLayout
                {
                    Children =
                    {
                        new Label { Text = $"Modelo: {reserva.idVehiculo}", FontAttributes = FontAttributes.Bold },
                        new Label { Text = $"Color: {reserva.color}" },
                        new Label { Text = $"Aros: {reserva.aros}" },
                        new Label { Text = $"Interior: {reserva.interior}" },
                        new Label { Text = $"Nombre: {reserva.Nombre} {reserva.Apellido1} {reserva.Apellido2}" },
                        new Label { Text = $"Correo: {reserva.Email}" }
                    }
                }
            };

            ReservasLayout.Children.Add(frame);
        }
    }
}
