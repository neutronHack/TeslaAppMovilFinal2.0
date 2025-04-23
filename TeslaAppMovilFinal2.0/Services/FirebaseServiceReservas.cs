using System.Net.Http.Json;
using TeslaAppMovilFinal2._0.Models;

namespace TeslaAppMovilFinal2._0.Services
{
    class FirebaseServiceReservas
    {
        private readonly HttpClient _httpClient;
        private readonly string _firebaseBaseUrl = "https://teslaappmovil-default-rtdb.firebaseio.com";

        public FirebaseServiceReservas()
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> GuardarReservaAsync(Reserva reserva)
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                var response = await _httpClient.PutAsJsonAsync($"{_firebaseBaseUrl}/reservas/{id}.json", reserva);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
        public async Task<Dictionary<string, Reserva>?> ObtenerReservasDiccionarioAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Dictionary<string, Reserva>>($"{_firebaseBaseUrl}/reservas.json");
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> EliminarReservaAsync(string id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_firebaseBaseUrl}/reservas/{id}.json");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

    }
}
