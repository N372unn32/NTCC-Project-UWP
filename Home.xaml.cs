using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Xamarin.Essentials;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace College_Project_Version_One
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Home : Page
    {
        public Home()
        {
            this.InitializeComponent();
            DocView.NavigationFailed += DocView_NavigationFailed;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;


        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.Internet)
            {

                WelcomeTextBlock.Text = "Welcome: Connected";
                DocView.Visibility = Visibility.Visible;

                //    DocView.Source = new Uri("https://sway.office.com/EwLiN5p0i4jE91BX?ref=Link");
                DocView.Navigate(new Uri("https://sway.office.com/EwLiN5p0i4jE91BX?ref=Link"));


            }
            else
            {
                WelcomeTextBlock.Text = "Welcome:  No Internet Connection";
                if (!DocView.IsLoaded)
                DocView.Visibility = Visibility.Collapsed;
                else DocView.Visibility = Visibility.Visible;

            }
        }

        private void DocView_Loaded(object sender, RoutedEventArgs e)
        {
        //    DocView.Visibility = Visibility.Visible;
        }

        private void DocView_Unloaded(object sender, RoutedEventArgs e)
        {
            DocView.Visibility = Visibility.Collapsed;

        }

        

        private void DocView_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            DocView.Visibility = Visibility.Collapsed;

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Xamarin.Essentials.Connectivity.NetworkAccess!=NetworkAccess.Internet)
            {
                WelcomeTextBlock.Text = "Welcome: No Internet Connection";
                DocView.Visibility = Visibility.Collapsed;
            }
            else
            {
                WelcomeTextBlock.Text = "Welcome";
                DocView.Visibility = Visibility.Visible;

            }
        }
    }
}
