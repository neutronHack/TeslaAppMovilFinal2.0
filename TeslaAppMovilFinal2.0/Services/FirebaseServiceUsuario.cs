using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TeslaAppMovilFinal2._0.Models;

namespace TeslaAppMovilFinal2._0.Services
{
    public class FirebaseServiceUsuario
    {
        private readonly HttpClient _httpClient;
        private readonly string _firebaseBaseUrl = "https://teslaappmovil-default-rtdb.firebaseio.com/";

        public FirebaseServiceUsuario()
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> RegistrarUsuarioAsync(Usuario usuario)
        {
            try
            {
                // Crea un nuevo ID para el usuario
                string id = Guid.NewGuid().ToString();
                usuario.IdUsuario = id;

                var response = await _httpClient.PutAsJsonAsync($"{_firebaseBaseUrl}/usuarios/{id}.json", usuario);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Usuario?> AutenticarUsuarioAsync(string email, string contraseña)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<Dictionary<string, Usuario>>($"{_firebaseBaseUrl}/usuarios.json");

                if (response != null)
                {
                    foreach (var item in response)
                    {
                        if (item.Value.Email == email && item.Value.Contraseña == contraseña)
                        {
                            return item.Value;
                        }
                    }
                }
            }
            catch
            {
                // Puedes registrar errores aquí si lo deseas
            }

            return null;
        }
    }
}