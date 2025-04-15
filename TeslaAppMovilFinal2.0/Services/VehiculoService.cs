using System.Net.Http;
using System.Text.Json;
using TeslaAppMovilFinal2._0.Models;

namespace TeslaAppMovilFinal2._0.Services
{
    public class VehiculoService
    {
        private readonly HttpClient _httpClient = new();
        private const string URL = "https://firestore.googleapis.com/v1/projects/teslaappmovil/databases/(default)/documents/vehiculos";

        public async Task<List<Vehiculo>> ObtenerVehiculosAsync()
        {
            var response = await _httpClient.GetStringAsync(URL);
            var jsonDoc = JsonDocument.Parse(response);

            var lista = new List<Vehiculo>();

            foreach (var doc in jsonDoc.RootElement.GetProperty("documents").EnumerateArray())
            {
                var fields = doc.GetProperty("fields");

                lista.Add(new Vehiculo
                {
                    Modelo = fields.GetProperty("modelo").GetProperty("stringValue").GetString(),
                    Precio = int.Parse(fields.GetProperty("precio").GetProperty("integerValue").GetString()),
                    ImagenUrl = fields.GetProperty("imagenUrl").GetProperty("stringValue").GetString(),
                    Aceleracion = fields.GetProperty("aceleracion").GetProperty("stringValue").GetString(),
                    Autonomia = fields.GetProperty("autonomia").GetProperty("stringValue").GetString(),
                    Velocidad = fields.GetProperty("velocidad").GetProperty("stringValue").GetString()
                });
            }

            return lista;
        }
    }
}
