namespace Similarity
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            SizeChanged += HandlePageSizeChange;
        }

        private async void HandleSignIn(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Login());
        }

        private async void HandleSignUp(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Cadastro());
        }

        private void HandlePageSizeChange(object sender, EventArgs e)
        {
            cadastroButton.WidthRequest = this.Width * 0.8;
            loginButton.WidthRequest = this.Width * 0.8;
        }

    }
}