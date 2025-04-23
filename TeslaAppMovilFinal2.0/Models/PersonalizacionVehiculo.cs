namespace TeslaAppMovilFinal2._0.Models
{
    public class PersonalizacionVehiculo
    {
        public string IdPersonalizacion { get; set; } = Guid.NewGuid().ToString();
        public string IdUsuario { get; set; } = string.Empty;
        public string IdVehiculo { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Aros { get; set; } = string.Empty;
        public string Interior { get; set; } = string.Empty;
        public DateTime FechaPersonalizacion { get; set; } = DateTime.Now;
    }
}
