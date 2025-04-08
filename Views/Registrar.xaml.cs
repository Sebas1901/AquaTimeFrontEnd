using Newtonsoft.Json;
using System;
using System.Text;
using System.Text.RegularExpressions;
using AquaTime.Models;
namespace AquaTime.Views;

public partial class Registrar : ContentPage
{
	public Registrar()
	{
		InitializeComponent();
	}

    private async void OnRegistrarclicked(object sender, EventArgs e)
    {
        // Llamar a validarDatos de manera as�ncrona
        bool datosValidos = await validarDatos(
            regisNombre.Text,
            regisUsuario.Text,
            registelefono.Text,
            regisfechaNac.Text,
            regispeso.Text,
            regisAvatar.Text,
            regisCodPlan.Text,
            regisEmail.Text,
            regisPassword.Text
        );

        // Verificar si los datos son v�lidos
        if (datosValidos)
        {
            // Si los datos son v�lidos, ir al API
            RegistrarRequest req = new RegistrarRequest();
            req.usuario = new Usuario();
            req.usuario.usuario = regisUsuario.Text;
            req.usuario.nombre = regisNombre.Text;
            req.usuario.email = regisEmail.Text;
            req.usuario.telefono = registelefono.Text;
            req.usuario.password = regisPassword.Text;
            req.usuario.avatar = regisAvatar.Text;

            DateTime fechaNac;
            if (DateTime.TryParse(regisfechaNac.Text, out fechaNac))
            {
                req.usuario.fechaNacimiento = fechaNac;
            }
            else
            {
                // If the date format is invalid, you can handle it here
                Console.WriteLine("El formato de la fecha es inv�lido.");

                // Optionally, you can set fechaNac to a default value or handle it as needed
                req.usuario.fechaNacimiento = DateTime.MinValue; // Example default value
            }

            // Para el peso, usar TryParse para convertir a decimal
            decimal peso;
            if (decimal.TryParse(regispeso.Text, out peso))
            {
                req.usuario.peso = peso;
            }
            else
            {
                Console.WriteLine("El peso ingresado no es v�lido.");
            }

            req.usuario.avatar = regisAvatar.Text;

            // Para el tipo de plan, convertir a int
            int tipoPlan;
            if (int.TryParse(regisCodPlan.Text, out tipoPlan))
            {
                req.usuario.Cod_TipoPlan = tipoPlan;
            }
            else
            {
                Console.WriteLine("El c�digo de tipo de plan no es v�lido.");
            }

            var jsonContent = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");

            HttpResponseMessage respuestaHttp = null;

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    respuestaHttp = await httpClient.PostAsync("http://localhost:51577/api/usuario/insertar", jsonContent);

                    if (respuestaHttp.IsSuccessStatusCode)
                    {
                        var responseContent = await respuestaHttp.Content.ReadAsStringAsync();
                        registrarReponse res = JsonConvert.DeserializeObject<registrarReponse>(responseContent);

                        if (res.Resultado)
                        {
                            // Navigate to MainPage if registration is successful
                            await Navigation.PushAsync(new LoginPage());
                        }
                        else
                        {
                            // Show alert if registration is unsuccessful
                            await DisplayAlert("Registro incorrecto", "No se registr� el usuario", "Aceptar");
                        }
                    }
                    else
                    {
                        // Show alert if server response is not successful
                        await DisplayAlert("Error de conexion", "No hay respuesta del servidor", "Aceptar");
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions (e.g., network errors) and show a user-friendly message
                    await DisplayAlert("Error", "Ocurri� un error al intentar registrar al usuario. " + ex.Message, "Aceptar");
                }
            }

        }
    }



    private async Task<bool> validarDatos(string nombre, string usuario, string telefono, string fechaNac, string peso, string avatar, string codPlan, string correo, string password)
    {
        // Validar que los campos obligatorios no est�n vac�os
        if (string.IsNullOrEmpty(regisUsuario.Text) || string.IsNullOrEmpty(regisNombre.Text) ||
     string.IsNullOrEmpty(regisEmail.Text) || string.IsNullOrEmpty(registelefono.Text) ||
     string.IsNullOrEmpty(regisPassword.Text) || string.IsNullOrEmpty(regisfechaNac.Text) ||
     string.IsNullOrEmpty(regispeso.Text) || string.IsNullOrEmpty(regisAvatar.Text) ||
     string.IsNullOrEmpty(regisCodPlan.Text))
        {
            await DisplayAlert("Error", "Todos los campos son obligatorios.", "OK");
            return false;
        }

        // Validar formato del correo electr�nico
        string patronCorreo = @"^[^@\s]+@([A-Za-z0-9-]+\.)+[A-Za-z]{2,}$";
        if (!Regex.IsMatch(correo, patronCorreo))
        {
            await DisplayAlert("Error", "El correo electr�nico no es v�lido.", "OK");
            return false;
        }

        // Validar que la contrase�a tenga al menos 8 caracteres
        if (password.Length < 8)
        {
            await DisplayAlert("Error", "La contrase�a debe tener al menos 8 caracteres.", "OK");
            return false;
        }

        // Validar que el tel�fono tenga solo n�meros y al menos 8 d�gitos
        if (!Regex.IsMatch(telefono, @"^\d{8,}$"))
        {
            await DisplayAlert("Error", "El tel�fono debe tener al menos 8 d�gitos y solo contener n�meros.", "OK");
            return false;
        }
        DateTime fechaNacValida;
        if (string.IsNullOrEmpty(fechaNac) || !DateTime.TryParse(fechaNac, out fechaNacValida) || fechaNacValida > DateTime.Today)
        {
            await DisplayAlert("Error", "La fecha de nacimiento no es v�lida o es futura.", "OK");
            return false;
        }

        // Validar que el usuario tenga al menos 18 a�os
        DateTime fechaActual = DateTime.Today;
        int edad = fechaActual.Year - fechaNacValida.Year;
        if (fechaNacValida.Date > fechaActual.AddYears(-edad)) edad--; // Si no ha cumplido a�os a�n

        if (edad < 18)
        {
            await DisplayAlert("Error", "El usuario debe tener al menos 18 a�os.", "OK");
            return false;
        }

        // Si todo es v�lido
        return true;
    }

}