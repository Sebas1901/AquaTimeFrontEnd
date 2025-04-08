using AquaTime.Services;
using AquaTime.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;


namespace AquaTime.Views;

public partial class PlanHidratacionPage : ContentPage
{

    private List<PlanHidratacion> planesHidratacion = new List<PlanHidratacion>();
    private int selectedPlanId;

    public PlanHidratacionPage()
	{
		InitializeComponent();
        ObtenerPlanesHidratacion();
    }
   

    // Método para cargar los planes de hidratación desde el endpoint
    public async Task ObtenerPlanesHidratacion()
    {
        try
        {
            var httpClient = new HttpClient();
            var url = "http://localhost:51577/api/hidratacion/obtenerPlanHidratacion";

            var jsonContent = new StringContent("{}", Encoding.UTF8, "application/json");
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url)
            {
                Content = jsonContent
            };

            var response = await httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var planResponse = JsonConvert.DeserializeObject<PlanHidratacionResponse>(responseJson);

                planesHidratacion = planResponse.Planes;

                Device.BeginInvokeOnMainThread(() =>
                {
                    PlanPicker.ItemsSource = planesHidratacion;
                    PlanPicker.ItemDisplayBinding = new Binding("NombrePlan");
                });
            }
            else
            {
                await DisplayAlert("Error", $"No se pudieron obtener los planes. Código: {response.StatusCode}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Ocurrió un problema al obtener los planes: " + ex.Message, "OK");
        }
    }

    // Evento cuando el usuario selecciona un plan
    private async void OnPlanSelectedChanged(object sender, EventArgs e)
    {
               
        foreach (var plan in planesHidratacion)
        {
            Console.WriteLine($"Plan: {plan.NombrePlan}, ID: {plan.Cod_TipoPlan}");
        }

        if (PlanPicker.SelectedItem is PlanHidratacion planSeleccionado)
        {
            selectedPlanId = planSeleccionado.Cod_TipoPlan;

            // Cambiar fondo según el código del plan
            switch (planSeleccionado.Cod_TipoPlan)
            {
                case 1: // Sedentario
                    this.BackgroundImageSource = "sedentario.jpg";
                    break;
                case 2: // Activo
                    this.BackgroundImageSource = "activo.jpg";
                    break;
                case 3: // Deportivo
                    this.BackgroundImageSource = "deportista.jpg";
                    break;
                default:
                    this.BackgroundImageSource = null;
                    break;
            }
        }


    }


    // Evento cuando el usuario hace clic en el botón "Cambiar"
    private async void OnCambiarClicked(object sender, EventArgs e)
    {
        var usuario = SessionService.Instance.Usuario;

        if (selectedPlanId == 0)
        {
            await DisplayAlert("Atención", "Debe seleccionar un plan de hidratación", "OK");
            return;
        }

        // Confirmación antes de enviar la solicitud
        bool confirmar = await DisplayAlert("Confirmación", "¿Está seguro de que desea cambiar su plan de hidratación?", "Sí", "No");
        if (!confirmar) return;

        try
        {
            var httpClient = new HttpClient();
            var url = "http://localhost:51577/api/usuario/modificarTipoPlan";

            var requestData = new
            {
                usuario = usuario,
                tipoPlan = selectedPlanId
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Éxito", "Plan de hidratación cambiado correctamente.", "OK");
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Error", $"No se pudo cambiar el plan. {errorMessage}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Ocurrió un problema al intentar cambiar el plan: " + ex.Message, "OK");
        }
    }

    // Evento cuando el usuario hace clic en el botón "Cancelar"
    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///MainPage");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Validar si el usuario está logueado
        if (string.IsNullOrEmpty(SessionService.Instance.Usuario))
        {

            // Redirigir a la página de login
            Shell.Current.GoToAsync("//LoginPage"); // Cambia a la ruta correcta de la página de login
        }
    }


}