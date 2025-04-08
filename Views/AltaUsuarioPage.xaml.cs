namespace AquaTime.Views;

public partial class AltaUsuarioPage : ContentPage
{
	public AltaUsuarioPage()
	{
		InitializeComponent();
	}

    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///LoginPage");
    }

    private async void OnCrearUsuarioClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("");
    }

}