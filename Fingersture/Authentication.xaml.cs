namespace Fingersture
{
    public partial class Authentication : ContentPage
    {
        public Authentication()
        {
            InitializeComponent();
        }

        private async void HandleSignIn(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignIn());
        }

        private async void HandleSignUp(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUp());
        }
    }
}