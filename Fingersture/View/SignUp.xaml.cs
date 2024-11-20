namespace Fingersture;

public partial class SignUp : ContentPage
{
    private readonly DatabaseService dbService;
    private string fingetprintPath;

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
            return "";
        }
        catch (Exception)
        {
            return "";
        }
    }

    private async void HandleImageSelection(object sender, EventArgs e)
    {
        fingetprintPath = await PickImageAsync();
        if (!string.IsNullOrEmpty(fingetprintPath))
        {
            fingetprintPath = System.IO.Path.GetFullPath(fingetprintPath);
            await DisplayAlert("Success", "Fingerprint successfully added!", "OK");
            ValidateForm();
        }
    }

    private void ValidateForm()
    {
        Boolean isFingetPrintPathEmpty = string.IsNullOrEmpty(fingetprintPath);
        Boolean isInputNameEmpty = string.IsNullOrEmpty(InputName.Text);
        Boolean hasSelectedImage = positionPicker.SelectedItem != null;

        registerButton.IsEnabled = hasSelectedImage &&
                                    !isInputNameEmpty &&
                                    !isFingetPrintPathEmpty;
    }

    private async void HandleAddFingerPrint(object sender, EventArgs e)
    {
        string name = InputName.Text;
        string position = positionPicker.SelectedItem?.ToString();

        if (string.IsNullOrEmpty(fingetprintPath))
        {
            await DisplayAlert("Error", "Please, select an image.", "OK");
            return;
        }

        dbService.AddFingerprint(fingetprintPath, name, position);
        await DisplayAlert("Success", "Successfully registered.", "OK");
        await Navigation.PopToRootAsync();
    }

    private async void HandleAddImage(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}
