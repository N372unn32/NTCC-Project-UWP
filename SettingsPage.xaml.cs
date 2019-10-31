using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Xamarin.Essentials;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace College_Project_Version_One
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {


        public SettingsPage()
        {
            this.InitializeComponent();
            //      SettingsPage settingsPage = new SettingsPage();

        }

        public async void PurchaseButton_Click(object sender, RoutedEventArgs e)
        {
            await AdManager.GetRequestToRemoveAd(true);
            //        MainPage mainPage = new MainPage();
            //  mainPage.MyBannerAd.Visibility = Visibility.Collapsed;
            CheckVisibility();
        }
        public void CheckVisibility()
        {


            if (AdManager.IfPurchaseCompleted)
            {
                PurchaseButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                PurchaseButton.Visibility = Visibility.Visible;

            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            AppNameBox.Text = Xamarin.Essentials.AppInfo.Name;


            // Package Name/Application Identifier (com.microsoft.testapp)
            PackageBox.Text = "Package Name:  " + AppInfo.PackageName;
            // Application Version (1.0.0)
            VersionBox.Text = "Version:  " + AppInfo.VersionString;
            // Application Build Number (1)
            BuildBox.Text = "Build Number:  " + AppInfo.BuildString;
        }

        private void AppSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            AppInfo.ShowSettingsUI();

        }
    }
}
