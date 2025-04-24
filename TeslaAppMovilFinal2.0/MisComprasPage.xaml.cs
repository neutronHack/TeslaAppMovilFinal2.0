using TeslaAppMovilFinal2._0.Services;

namespace TeslaAppMovilFinal2._0;

public partial class MisComprasPage : ContentPage
{
    private readonly FirebaseServiceCompras _firebaseService = new();

    public MisComprasPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarCompras();
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

    private async void CargarCompras()
    {
        ComprasLayout.Children.Clear();
        string idUsuario = Helpers.SessionManager.UsuarioActual?.IdUsuario ?? "";

        if (string.IsNullOrEmpty(idUsuario))
        {
            await DisplayAlert("Error", "Sesión no iniciada", "OK");
            await Navigation.PopToRootAsync();
            return;
        }

        var compras = await _firebaseService.ObtenerComprasPorUsuarioAsync(idUsuario);

        if (compras == null || compras.Count == 0)
        {
            ComprasLayout.Children.Add(new Label
            {
                Text = "No tienes compras registradas.",
                FontSize = 18,
                HorizontalOptions = LayoutOptions.Center
            });
            return;
        }

        foreach (var compra in compras)
        {
            string precio = ObtenerPrecioPorModelo(compra.Modelo);
            var frame = new Frame
            {
                BorderColor = Colors.Gray,
                CornerRadius = 12,
                Padding = 10,
                Content = new VerticalStackLayout
                {
                    Children =
                    {
                        new Label { Text = $"Modelo: {compra.Modelo}", FontAttributes = FontAttributes.Bold },
                        new Label { Text = $"Color: {compra.Color}" },
                        new Label { Text = $"Aros: {compra.Aros}" },
                        new Label { Text = $"Interior: {compra.Interior}" },
                        new Label { Text = $"Correo: {compra.Email}" },
                        new Label { Text = $"Precio: {precio}" }
                    }
                }
            };

            ComprasLayout.Children.Add(frame);
        }
    }
}
