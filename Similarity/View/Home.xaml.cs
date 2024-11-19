namespace Fingersture;

public partial class Home : ContentPage
{
	public Home(string nome, string cargo)
    {
        InitializeComponent();
        NomeLabel.Text = $"Nome: {nome}";
        CargoLabel.Text = $"Cargo: {(cargo == "N�vel 1" ? (cargo == "N�vel 2" ? "Civil" : "Diretor de divis�o")
            : "Ministro do meio ambiente")}";
        NavigationPage.SetHasBackButton(this, false);
        this.SizeChanged += OnPageSizeChanged;
    }
    protected override bool OnBackButtonPressed()
    {
        return true;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }

    private void OnPageSizeChanged(object sender, EventArgs e)
    {
        labelAcesso.WidthRequest = this.Width * 0.8;
        NomeLabel.WidthRequest = this.Width * 0.8;
        CargoLabel.WidthRequest = this.Width * 0.8;
        buttonBack.WidthRequest = this.Width * 0.8;
    }
}