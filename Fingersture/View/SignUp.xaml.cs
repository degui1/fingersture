namespace Fingersture;

public partial class SignUp : ContentPage
{
    private readonly DatabaseService dbService;
    private string imagePath1;

    public SignUp()
    {
        InitializeComponent();
        dbService = DatabaseService.Instance;
    }

    private static async Task<string> PickImageAsync()
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Pick a fingerprint",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                return result.FullPath;
            }
            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private async void HandleImageSelection(object sender, EventArgs e)
    {
        imagePath1 = await PickImageAsync();
        if (!string.IsNullOrEmpty(imagePath1))
        {
            imagePath1 = System.IO.Path.GetFullPath(imagePath1);
            await DisplayAlert("Success", "Fingerprint successfully added!", "OK");
            ValidateForm();
        }
    }

    private void ValidateForm()
    {
        registerButton.IsEnabled = !string.IsNullOrEmpty(EntryNome.Text) &&
                                    positionPicker.SelectedItem != null &&
                                    !string.IsNullOrEmpty(imagePath1);
    }

    private async void HandleAddFingerPrint(object sender, EventArgs e)
    {
        string nome = EntryNome.Text;
        string cargo = positionPicker.SelectedItem?.ToString();

        if (string.IsNullOrEmpty(imagePath1))
        {
            await DisplayAlert("Error", "Please, select an image.", "OK");
            return;
        }

        dbService.AddFingerprint(imagePath1, nome, cargo);
        await DisplayAlert("Success", "Successfully registered.", "OK");
        await Navigation.PopToRootAsync();
    }

    private async void HandleAddImage(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}
