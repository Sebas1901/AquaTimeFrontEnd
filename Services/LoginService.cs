using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AquaTime.Models;

namespace AquaTime.Services
{
    public class LoginService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:51577/api/usuario/loguear";

        public LoginService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<UsuarioLoginResponse> LoginAsync(string username, string password)
        {
            try
            {
                var request = new UsuarioLoginRequest(username, password);

                string json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(BaseUrl, content);
                string responseBody = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<UsuarioLoginResponse>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result == null || result.Error == null)
                {
                    return new UsuarioLoginResponse
                    {
                        Resultado = false,
                        Error = new List<ErrorDetail> { new ErrorDetail { Message = "Error desconocido" } }
                    };
                }

                return result;
            }
            catch (Exception ex)
            {
                return new UsuarioLoginResponse
                {
                    Resultado = false,
                    Error = new List<ErrorDetail> { new ErrorDetail { Message = $"Error: {ex.Message}" } }
                };
            }
        }


    }
}
