namespace Fingersture;

public partial class Home : ContentPage
{
	public Home(string nome, string cargo)
    {
        InitializeComponent();
        NomeLabel.Text = $"Name: {nome}";
        CargoLabel.Text = $"Position: {(cargo == "Nível 1" ? (cargo == "Nível 2" ? "Civil" : "Diretor de divisão")
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