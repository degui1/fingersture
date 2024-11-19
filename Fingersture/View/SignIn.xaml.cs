using Microsoft.Maui.Controls.Shapes;
using OpenCvSharp;
using OpenCvSharp.Features2D;
using Size = OpenCvSharp.Size;


namespace Fingersture;

public partial class SignIn : ContentPage
{
    private readonly DatabaseService dbService;
    private string imagePath1;
    private bool isLoading = true;

    public SignIn()
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

    async static Task<Mat> CreateMatchImage(Mat img1, Mat img2, List<DMatch> goodMatches, KeyPoint[] keypoints1, KeyPoint[] keypoints2)
    {
        Mat matchImage = new();
        Cv2.DrawMatches(img1, keypoints1, img2, keypoints2, goodMatches, matchImage, flags: DrawMatchesFlags.NotDrawSinglePoints);
        return matchImage;
    }

    async void HandleSelectImage(object sender, EventArgs e)
    {
        imagePath1 = await PickImageAsync();
        if (!string.IsNullOrEmpty(imagePath1))
        {
            imagePath1 = System.IO.Path.GetFullPath(imagePath1);
            selectedImageLabel.Text = $"Image: {System.IO.Path.GetFileName(imagePath1)}";
            ButtonCompare.IsEnabled = true;
        }
    }

    async Task<bool> CompareFingerprintsAndShowMatches(string imagePath1, string imagePath2)
    {
        try
        {
            Mat img1 = Cv2.ImRead(imagePath1, ImreadModes.Grayscale);
            Mat img2 = Cv2.ImRead(imagePath2, ImreadModes.Grayscale);

            if (img1.Empty() || img2.Empty())
            {
                return false;
            }

            Cv2.GaussianBlur(img1, img1, new Size(5, 5), 0);
            Cv2.GaussianBlur(img2, img2, new Size(5, 5), 0);
            Cv2.EqualizeHist(img1, img1);
            Cv2.EqualizeHist(img2, img2);

            Cv2.Threshold(img1, img1, 0, 255, ThresholdTypes.Otsu);
            Cv2.Threshold(img2, img2, 0, 255, ThresholdTypes.Otsu);

            Cv2.Canny(img1, img1, 100, 200);
            Cv2.Canny(img2, img2, 100, 200);

            var contours1 = Cv2.FindContoursAsMat(img1, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            var contours2 = Cv2.FindContoursAsMat(img2, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            Cv2.DrawContours(img1, contours1, -1, new Scalar(255, 0, 0), 2);
            Cv2.DrawContours(img2, contours2, -1, new Scalar(255, 0, 0), 2);

            var sift = SIFT.Create();
            Mat descriptors1 = new(), descriptors2 = new();
            sift.DetectAndCompute(img1, null, out KeyPoint[] keypoints1, descriptors1);
            sift.DetectAndCompute(img2, null, out KeyPoint[] keypoints2, descriptors2);

            var bf = new BFMatcher(NormTypes.L2, crossCheck: false);
            var matches = bf.KnnMatch(descriptors1, descriptors2, k: 2);

            List<DMatch> goodMatches = [];
            foreach (var match in matches)
            {
                if (match[0].Distance < 0.75 * match[1].Distance)
                {
                    goodMatches.Add(match[0]);
                }
            }

            double matchPercentage = goodMatches.Count / (double)keypoints1.Length;
            if (matchPercentage > 0.15)
            {
                Mat matchImage = await CreateMatchImage(img1, img2, goodMatches, keypoints1, keypoints2);
                string matchImagePath = System.IO.Path.Combine(FileSystem.CacheDirectory, "matchImage.jpg");
                matchImage.SaveImage(matchImagePath);

                FullScreenImage.Source = ImageSource.FromFile(matchImagePath);
                FullScreenImage.IsVisible = true;
                Form.IsVisible = false;

                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to compare fingerprints: {ex.Message}", "OK");
            return false;
        }
    }

    private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        ValidateForm();
    }

    private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        ValidateForm();
    }

    private void ValidateForm()
    {
        ButtonCompare.IsEnabled = !string.IsNullOrEmpty(EntryNome.Text) && !string.IsNullOrEmpty(imagePath1);
    }

    async void OnCompareFingerprintsClicked(object sender, EventArgs e)
    {
        this.isLoading = true;
        Loading();
        await Task.Delay(1000);
        if (string.IsNullOrEmpty(EntryNome.Text))
        {
            this.isLoading = false;
            Loading();
            await DisplayAlert("Error", "Please, type your name.", "OK");
            return;
        }

        if (!string.IsNullOrEmpty(imagePath1))
        {
            var fingerprints = dbService.GetAllFingerprints();
            bool accessGranted = false;

            foreach (var (ImagePath, Nome, Cargo) in fingerprints)
            {
                if (Nome == EntryNome.Text)
                {
                    bool isMatch = await CompareFingerprintsAndShowMatches(imagePath1, ImagePath);
                    if (isMatch)
                    {
                        accessGranted = true;
                        await Task.Delay(2000);
                        await Navigation.PushAsync(new Home(EntryNome.Text, Cargo));
                        this.isLoading = false;
                        Loading();
                        return;
                    }
                }
            }

            if (!accessGranted)
            {
                this.isLoading = false;
                Loading();
                await DisplayAlert("Access denied!", "Fingerprint or username not found in the database", "OK");
                FullScreenImage.IsVisible = false;
            }
        }
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    void Loading()
    {
        if (isLoading)
        {
            loadingView.IsVisible = true;
            Form.IsVisible = false;
            StartSpinnerAnimation();
        } else
        {
            loadingView.IsVisible = false;
            Form.IsVisible = true;
        }
    }

    private void StartSpinnerAnimation()
    {
        var rotationAnimation = new Animation(v => loaderCircle.Rotation = v, 0, 360);

        rotationAnimation.Commit(this, "LoaderRotation", length: 1000, repeat: () => true, finished: (v, c) => { });
    }





}
