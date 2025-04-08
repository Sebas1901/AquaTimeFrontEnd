using AquaTime.Services;
using AquaTime.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace AquaTime.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();


            // Verifica si el usuario está vacío o nulo
            if (string.IsNullOrEmpty(SessionService.Instance.Usuario))
            {
                UsuarioLabel.IsVisible = false;
                ConsumidoLabel.IsVisible = false;
                MetaDiariaBar.IsVisible = false;
                ConsumidoBar.IsVisible = false;
            }
            else
            {
                UsuarioLabel.IsVisible = true;
                ConsumidoLabel.IsVisible = true;
                MetaDiariaBar.IsVisible = true;
                ConsumidoBar.IsVisible = true;
                UsuarioLabel.Text = SessionService.Instance.Usuario;  // Asigna el nombre de usuario
            }



            if (SessionService.Instance.Usuario != null)
            {
                UsuarioLabel.Text = "Usuario: " + SessionService.Instance.Usuario.ToUpper();

                // Construir el objeto de solicitud correctamente
                var request = new HidratacionRequest
                {
                    Usuario = SessionService.Instance.Usuario,
                    Fecha = DateTime.Now.ToString("yyyy-MM-dd")
                };

                // Hacer la solicitud a la API
                var response = await GetHidratacionDataAsync(request);

                if (response != null && response.Resultado)
                {
                    UpdateGraphAsync(response);
                }
                else
                {
                    Console.WriteLine("Error al obtener los datos de hidratación.");
                }
            }
            else
            {
                UsuarioLabel.Text = "";
            }
        }

        private async Task<HidratacionResponse> GetHidratacionDataAsync(HidratacionRequest request)
        {
            try
            {
                string url = "http://localhost:51577/api/hidratacion/calcular";
                string json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseJson = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<HidratacionResponse>(responseJson);
                }
                else
                {
                    // Aquí debes usar la clase correcta de error
                    return new HidratacionResponse
                    {
                        Resultado = false,
                        Error = new List<HidratacionErrorDetail> // Cambié ErrorDetail a HidratacionErrorDetail
                        {
                            new HidratacionErrorDetail { Message = "Error al obtener datos" }
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en la solicitud: {ex.Message}");
                // Aquí también cambié a HidratacionErrorDetail
                return new HidratacionResponse
                {
                    Resultado = false,
                    Error = new List<HidratacionErrorDetail> // Cambié ErrorDetail a HidratacionErrorDetail
                    {
                        new HidratacionErrorDetail { Message = "Error inesperado" }
                    }
                };
            }
        }


        private void UpdateGraphAsync(HidratacionResponse response)
        {
            double metaDiaria = response.MetaDiaria;  // Meta total (1000, 2000, 3000, etc.)
            double consumido = response.Consumido;  // Lo que ha consumido el usuario

            // **Altura real de la imagen de la botella en la UI** (ajustar si es necesario)
            double maxHeight = 220;

            // **Calcular altura proporcional del consumo**
            double heightConsumidoProporcionado = (consumido / metaDiaria) * maxHeight;

            // **Asegurar que la altura del consumo no sea menor que 1 (para que siempre se vea)**
            if (heightConsumidoProporcionado < 1)
            {
                heightConsumidoProporcionado = 1;
            }

            // **Reducir el ancho de las barras**
            double originalWidth = 100;
            double reducedWidth = originalWidth * 0.6;

            MetaDiariaBar.WidthRequest = reducedWidth;
            ConsumidoBar.WidthRequest = reducedWidth;

            MetaDiariaBar.HeightRequest = maxHeight;
            ConsumidoBar.HeightRequest = heightConsumidoProporcionado;

            // **Posicionar las barras desde la base**
            MetaDiariaBar.TranslationY = 0;  // Colocar el recuadro gris en la base de la botella

            // **Ajuste dinámico del desplazamiento basado en la meta diaria**
            double baseOffsetConsumido = 0;

            // Ajustar según el valor de la meta diaria
            if (metaDiaria == 3000 || metaDiaria == 6000)
            {
                baseOffsetConsumido = maxHeight * 0.3;  // 30% para metas grandes
            }
            else if (metaDiaria == 2000)
            {
                baseOffsetConsumido = maxHeight * 0.4;  // 20% para metas medianas
            }
            else if (metaDiaria == 1000)
            {
                baseOffsetConsumido = maxHeight * 0.1;  // 15% para metas pequeñas
            }

            // **Posicionar la barra azul (ConsumidoBar) desde la base ajustada**
            ConsumidoBar.TranslationY = baseOffsetConsumido;

            ConsumidoLabel.Text = "Consumido :" + consumido.ToString() + " ml";
            MetaLabel.Text = "Meta Diaria :" + metaDiaria.ToString() + " ml";

            // 🔹 Verificar si alcanzó o superó la meta diaria
            if (response.Consumido >= response.MetaDiaria)
            {
                 MostrarFelicitacion(response.MetaDiaria);
            }

        }

        private async Task MostrarFelicitacion(double metaDiaria)
        {
            await DisplayAlert("🎉 ¡Felicitaciones! 🎉",
                $"¡Ya has alcanzado tu meta de {metaDiaria} ml por hoy! Sigue hidratándote. 💧",
                "OK");
        }



















        private async void OnSolicitarUsuarioTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AltaUsuarioPage());
        }
    }
}
