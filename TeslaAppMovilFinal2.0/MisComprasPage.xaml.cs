using TeslaAppMovilFinal2._0.Models;
using TeslaAppMovilFinal2._0.Services;
using System.Net.Http.Json;

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
                        new Label { Text = $"Correo: {compra.Email}" }
                    }
                }
            };

            ComprasLayout.Children.Add(frame);
        }
    }
}
