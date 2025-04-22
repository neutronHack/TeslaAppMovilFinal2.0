using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<List<Reserva>> ObtenerReservasPorUsuarioAsync(string idUsuario)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<Dictionary<string, Reserva>>($"{_firebaseBaseUrl}/reservas.json");

                if (response == null)
                    return new List<Reserva>();

                return response.Values.Where(r => r.IdUsuario == idUsuario).ToList();
            }
            catch
            {
                return new List<Reserva>();
            }
        }
    }
}
