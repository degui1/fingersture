using Fingersture;

namespace Fingersture
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Authentication());
            SQLitePCL.Batteries_V2.Init();

        }
    }
}
