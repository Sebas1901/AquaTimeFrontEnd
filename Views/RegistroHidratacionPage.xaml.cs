using AquaTime.Services;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using AquaTime.Services;


namespace AquaTime.Views;

public partial class RegistroHidratacionPage : ContentPage
{
	public RegistroHidratacionPage()
	{
		InitializeComponent();
	}

    private async void OnRegistrarClicked(object sender, EventArgs e)
    {
        // Obtener el valor ingresado en el Entry
        if (double.TryParse(txtCantidad.Text, out double cantidad) && cantidad > 0 && cantidad < 10000)
        {

            var registro = new
            {
                registrohidratacion = new
                {
                    usuario = SessionService.Instance.Usuario,
                    cantidadML = txtCantidad.Text
                }
            };
            string json = JsonConvert.SerializeObject(registro);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            try
            {
                var response = await client.PostAsync("http://localhost:51577/api/hidratacion/insertar", content);
                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("�xito", "Registro guardado correctamente.", "OK");
                    await Shell.Current.GoToAsync("//MainPage"); // Redirige a MainPage
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo registrar la hidrataci�n.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al conectar con el servidor: {ex.Message}", "OK");
            }

        }
        else
        {
            // Mostrar mensaje de error si no es un n�mero v�lido
            await DisplayAlert("Error", "Ingrese una cantidad v�lida en mililitros, entre 1 y 10.0000.", "OK");
            return;

        }
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Validar si el usuario est� logueado
        if (string.IsNullOrEmpty(SessionService.Instance.Usuario))
        {

            // Redirigir a la p�gina de login
            Shell.Current.GoToAsync("//LoginPage"); // Cambia a la ruta correcta de la p�gina de login
        }
    }


    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        // Procesas la cantidad de hidrataci�n que se ingres�, por ejemplo:
        var cantidad = txtCantidad.Text;
        // Aqu� ir�a la l�gica para usar esa cantidad...

        // Limpia el campo (y otros controles si es necesario)
        txtCantidad.Text = string.Empty;
        // Si tienes m�s controles, resetea sus valores

        // Si deseas, puedes tambi�n forzar que la p�gina se "refresque" volviendo a navegar a ella
        await Shell.Current.GoToAsync("///MainPage");
    }



}