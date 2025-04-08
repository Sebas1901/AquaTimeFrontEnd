using AquaTime.Services;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace AquaTime.Views;

public partial class PlanPersonalizadoPage : ContentPage
{
    public PlanPersonalizadoPage()
    {
        InitializeComponent();
    }

    private async void ProcesarRegistro(object sender, EventArgs e)
    {
        string usuario = SessionService.Instance.Usuario;
        if (!int.TryParse(metaDiariaEntry.Text, out int metaDiaria))
        {
            errorLabel.Text = "Ingrese una meta diaria válida";
            errorLabel.IsVisible = true;
            return;
        }

        var registro = new
        {
            registro = new
            {
                Usuario = usuario,
                Meta_Diaria = metaDiaria,
                Fecha_Inicio = fechaInicioPicker.Date.ToString("yyyy-MM-dd"),
                Fecha_Fin = fechaFinPicker.Date.ToString("yyyy-MM-dd")
            }
        };

        string json = JsonSerializer.Serialize(registro);
        HttpClient client = new HttpClient();
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync("http://localhost:51577/api/hidratacion/registrarPlanPersonalizado", content);
        string responseBody = await response.Content.ReadAsStringAsync();

        using JsonDocument doc = JsonDocument.Parse(responseBody);
        JsonElement root = doc.RootElement;

        bool resultado = root.GetProperty("resultado").GetBoolean();

        if (!resultado)
        {
            string mensajeError = root.GetProperty("error")[0].GetProperty("Message").GetString();
            errorLabel.Text = mensajeError;
            errorLabel.IsVisible = true;
        }
        else
        {
            await DisplayAlert("Éxito", "Plan personalizado registrado", "OK");
            await Navigation.PopAsync();
        }
    }

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