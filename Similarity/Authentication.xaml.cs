namespace Fingersture
{
    public partial class Authentication : ContentPage
    {
        public Authentication()
        {
            InitializeComponent();
            SizeChanged += HandlePageSizeChange;
        }

        private async void HandleSignIn(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignIn());
        }

        private async void HandleSignUp(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUp());
        }

        private void HandlePageSizeChange(object sender, EventArgs e)
        {
            signInButton.WidthRequest = this.Width * 0.8;
            signUpButton.WidthRequest = this.Width * 0.8;
        }

    }
}