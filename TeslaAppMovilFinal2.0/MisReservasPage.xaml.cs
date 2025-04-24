using TeslaAppMovilFinal2._0.Services;

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

    private async Task CargarReservas()
    {
        ReservasLayout.Children.Clear();
        string idUsuario = Helpers.SessionManager.UsuarioActual?.IdUsuario ?? "";

        if (string.IsNullOrEmpty(idUsuario))
        {
            await DisplayAlert("Error", "Sesión no iniciada", "OK");
            await Navigation.PopToRootAsync();
            return;
        }

        var reservasDic = await _firebaseService.ObtenerReservasDiccionarioAsync();

        if (reservasDic == null || reservasDic.Count == 0)
        {
            ReservasLayout.Children.Add(new Label
            {
                Text = "No tienes reservas registradas.",
                FontSize = 18,
                HorizontalOptions = LayoutOptions.Center
            });
            return;
        }

        foreach (var reservas in reservasDic)
        {
            var idReserva = reservas.Key;
            var reserva = reservas.Value;

            if (reserva.IdUsuario != idUsuario)
                continue;

            string precio = ObtenerPrecioPorModelo(reserva.idVehiculo);
            var frame = new Frame
            {
                BorderColor = Colors.Gray,
                CornerRadius = 12,
                Padding = 10,
                Content = new VerticalStackLayout
                {
                    Spacing = 8,
                    Children =
                    {
                        new Label { Text = $"Modelo: {reserva.idVehiculo}", FontAttributes = FontAttributes.Bold },
                        new Label { Text = $"Color: {reserva.color}" },
                        new Label { Text = $"Aros: {reserva.aros}" },
                        new Label { Text = $"Interior: {reserva.interior}" },
                        new Label { Text = $"Nombre: {reserva.Nombre} {reserva.Apellido1} {reserva.Apellido2}" },
                        new Label { Text = $"Correo: {reserva.Email}" },
                        new Label { Text = $"Precio: {precio}" },

                        new HorizontalStackLayout
                        {
                            Spacing = 10,
                            Children =
                            {
                                new Button
                                {
                                    Text = "Eliminar",
                                    BackgroundColor = Colors.Red,
                                    TextColor = Colors.White,
                                    Command = new Command(async () => await EliminarReserva(idReserva))
                                },
                                new Button
                                {
                                    Text = "Comprar",
                                    BackgroundColor = Colors.Green,
                                    TextColor = Colors.White,
                                    Command = new Command(async () => await Navigation.PushAsync(new ComprarReservaPage(reserva,idReserva)))
                                }
                            }
                        }
                    }
                }
            };

            ReservasLayout.Children.Add(frame);
        }
    }

    private async Task EliminarReserva(string idReserva)
    {
        bool confirmado = await DisplayAlert("Confirmar", "¿Deseas eliminar esta reserva?", "Sí", "No");
        if (confirmado)
        {
            bool exito = await _firebaseService.EliminarReservaAsync(idReserva);
            if (exito)
                await CargarReservas();
            else
                await DisplayAlert("Error", "No se pudo eliminar la reserva", "OK");
        }
    }
}
