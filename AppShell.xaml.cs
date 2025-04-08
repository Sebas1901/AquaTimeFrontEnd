namespace AquaTime
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {

            bool cerrarSesion = await DisplayAlert("Cerrar sesión", "¿Desea cerrar sesión?", "Sí", "No");
            if (cerrarSesion)
            {
                // Aquí ejecuta la lógica para cerrar sesión.
                // Por ejemplo, navegar a la página de Login:
                await Shell.Current.GoToAsync("//LoginPage");

            }
        }
    }
}
