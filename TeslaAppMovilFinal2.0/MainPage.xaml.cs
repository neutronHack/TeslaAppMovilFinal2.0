﻿using TeslaAppMovilFinal2._0.Models;
using TeslaAppMovilFinal2._0.Services;

namespace TeslaAppMovilFinal2._0
{
    public partial class MainPage : ContentPage
    {
        private readonly VehiculoService _vehiculoService = new();

        public MainPage()
        {
            InitializeComponent();
            // Verifica si hay sesión
            if (Helpers.SessionManager.UsuarioActual != null)
            {
                UserInfoBar.IsVisible = true;
                UserInfoLabel.Text = $"Usuario: {Helpers.SessionManager.UsuarioActual.Nombre} {Helpers.SessionManager.UsuarioActual.Apellido}";
            }
            CargarVehiculos();
        }

        private async void CargarVehiculos()
        {
            try
            {
                var vehiculos = await _vehiculoService.ObtenerVehiculosAsync();
                VehiculosView.ItemsSource = vehiculos;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "No se pudieron cargar los vehículos: " + ex.Message, "OK");
            }
        }

    }
}
