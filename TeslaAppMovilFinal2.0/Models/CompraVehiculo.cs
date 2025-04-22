using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeslaAppMovilFinal2._0.Models
{
    class CompraVehiculo
    {
        public string IdCompra { get; set; } = string.Empty;
        public string IdUsuario { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido1 { get; set; } = string.Empty;
        public string Apellido2 { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string NumeroTarjeta { get; set; } = string.Empty;
        public string FechaCaducidad { get; set; } = string.Empty;
        public string CVV { get; set; } = string.Empty;

        public string Modelo { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Aros { get; set; } = string.Empty;
        public string Interior { get; set; } = string.Empty;
    }
}
