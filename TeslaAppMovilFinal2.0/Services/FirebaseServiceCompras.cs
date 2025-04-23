using System.Net.Http.Json;
using TeslaAppMovilFinal2._0.Models;

namespace TeslaAppMovilFinal2._0.Services
{
    class FirebaseServiceCompras
    {
        private readonly HttpClient _httpClient;
        private readonly string _firebaseBaseUrl = "https://teslaappmovil-default-rtdb.firebaseio.com";

        public FirebaseServiceCompras()
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> GuardarCompraAsync(CompraVehiculo compra)
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                compra.IdCompra = id;

                var response = await _httpClient.PutAsJsonAsync($"{_firebaseBaseUrl}/compras/{id}.json", compra);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<CompraVehiculo>> ObtenerComprasPorUsuarioAsync(string idUsuario)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<Dictionary<string, CompraVehiculo>>($"{_firebaseBaseUrl}/compras.json");

                if (response == null)
                    return new List<CompraVehiculo>();

                return response.Values.Where(c => c.IdUsuario == idUsuario).ToList();
            }
            catch
            {
                return new List<CompraVehiculo>();
            }
        }

    }
}
