using System;
using TeslaAppMovilFinal2._0.Models;
namespace TeslaAppMovilFinal2._0;

public partial class AdminPage : ContentPage
{
    public AdminPage()
    {
        InitializeComponent();
    }
    private void OnEditarVehiculoClicked(object sender, EventArgs e)
    {
        
        DisplayAlert("Editar Vehículo", "Función de edición no implementada aún.", "OK");
    }

}