using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TeslaAppMovilFinal2._0.Models;

namespace TeslaAppMovilFinal2._0.Services
{
    public class FirebaseServicePersonalizacion
    {
        private readonly HttpClient _httpClient;
        private readonly string _firebaseBaseUrl = "https://teslaappmovil-default-rtdb.firebaseio.com";

        public FirebaseServicePersonalizacion()
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> GuardarPersonalizacionAsync(PersonalizacionVehiculo personalizacion)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_firebaseBaseUrl}/personalizaciones/{personalizacion.IdPersonalizacion}.json", personalizacion);
            return response.IsSuccessStatusCode;
        }
    }
}
