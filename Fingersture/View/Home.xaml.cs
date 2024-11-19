namespace Fingersture;

public partial class Home : ContentPage
{
	public Home(string nome, string cargo)
    {
        InitializeComponent();
        NomeLabel.Text = $"Name: {nome}";
        CargoLabel.Text = $"Position: {(cargo == "N�vel 1" ? (cargo == "N�vel 2" ? "Civil" : "Diretor de divis�o")
            : "Ministro do meio ambiente")}";
        NavigationPage.SetHasBackButton(this, false);
    }
    protected override bool OnBackButtonPressed()
    {
        return true;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}