using AquaTime.Services;
using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace AquaTime.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly LoginService _loginService;

        public LoginPage()
        {
            InitializeComponent();
            _loginService = new LoginService();
        }

        private void OnForgotPassword(object sender, EventArgs e)
        {
            DisplayAlert("Recuperar Contraseña", "Función no implementada aún.", "OK");
        }

        //private void OnForgotUser(object sender, EventArgs e)
        //{
        //    DisplayAlert("Recuperar Usuario", "Función no implementada aún.", "OK");
        //}

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Usuario y contraseña son requeridos", "OK");
                return;
            }

            var response = await _loginService.LoginAsync(username, password);

            if (response.Resultado)
            {
                // Almacena usuario
                SessionService.Instance.Usuario = username;


                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                string errorMessage = response != null && response.Error != null && response.Error.Count > 0
                    ? response.Error[0].Message
    :               "Error desconocido";

                    await DisplayAlert("Error", errorMessage, "OK");


            }



        }


        private async void OnRequestUser(object sender, EventArgs e){
            await Navigation.PushAsync(new Registrar());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SessionService.Instance.Usuario = null;
            UsernameEntry.Text = string.Empty;
            PasswordEntry.Text = string.Empty;
        }

        
    }
}
