using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Microsoft.Advertising.WinRT.UI;

using Microsoft.Advertising.Ads.Requests;
using Microsoft.Advertising.MicrosoftAdvertising_XamlTypeInfo;
using static College_Project_Version_One.SettingsPage;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Store;
using Windows.Storage;
using System.Threading.Tasks;




// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace College_Project_Version_One
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
       // private readonly List<(Type page)> SaareKagaz = new List<(Type page)> { ( typeof(Home)), ( typeof(Weather)), ("Email", typeof(Email)), ("Music", typeof(Music)), ("Settings", typeof(SettingsPage)) };

        private readonly List<(string Tag, Type page)> pages = new List<(string Tag, Type page)>
         {
                ("Home",typeof(Home)),
                ("Weather",typeof(MyWeatherPage)),
                ("Email",typeof(Email)),
                ("Music",typeof(Music)),
                ("Maps",typeof(MapPage)),
                ("Settings",typeof(SettingsPage))
            };
        private readonly List<Type> Pages = new List<Type>
        {   typeof(Home),
            typeof(MyWeatherPage),
            typeof(Email),
            typeof(Music),
            typeof(MapPage)
        };
        public static bool IsRemoved = false;
        public static LicenseInformation AppLicenseInformation { get; set; }
        public MainPage()
        {
            this.InitializeComponent();
   //         ContentFrame.Navigate(typeof(Home));

            NavigationView.SelectedItem = Home;
            
            ContentGridColumnTwo.Width = new GridLength(0);
          MyBannerAd.Refresh();
        } 

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        { //NavigationView.SelectedItem
          //    NavigationView.Header = sender.SelectedItem;
          //   NavigationView.Header = NavigationView.Tag.ToString();
          //    var header = (string)sender.Tag;     
            Type lastpage = ContentFrame.CurrentSourcePageType;

            if (args.IsSettingsSelected && ContentFrame.CurrentSourcePageType != typeof(SettingsPage))
            {
                ContentFrame.Navigate(typeof(SettingsPage),null, new SlideNavigationTransitionInfo() { Effect=SlideNavigationTransitionEffect.FromLeft});
                
            }

       /*     else
            {
                foreach (var item in pages)
                {
                    if (NavigationView.SelectedItem == NavigationView.MenuItems.OfType<NavigationViewItem>().Where(p => p.Tag.Equals(item.Tag))
                        && ContentFrame.CurrentSourcePageType != item.page)
                    {
                        ContentFrame.Navigate(item.page);
                    }

                }
            }
         */
            else  if (Home.IsSelected && ContentFrame.CurrentSourcePageType != typeof(Home))
            {
                ContentFrame.Navigate(typeof(Home), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft });
              //  NavigationView.Header = "Home";
            }
            else if (Weather.IsSelected && ContentFrame.CurrentSourcePageType != typeof(MyWeatherPage))
            { 
                ContentFrame.Navigate(typeof(MyWeatherPage), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft }); //NavigationView.Header = "Weather";

            }
            else if (Music.IsSelected && ContentFrame.CurrentSourcePageType != typeof(Music))
            {
                ContentFrame.Navigate(typeof(Music), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft }); //NavigationView.Header = "Music";

            }
            else if (Email.IsSelected && ContentFrame.CurrentSourcePageType != typeof(Email))
            {
                ContentFrame.Navigate(typeof(Email), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft });
                //   NavigationView.Header = "Email";
            }
             else if(Maps.IsSelected && ContentFrame.CurrentSourcePageType != typeof(MapPage))
            {
                ContentFrame.Navigate(typeof(MapPage), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft });
            }
            
            NavigationView.Header = ((NavigationViewItem)NavigationView.SelectedItem)?.Content?.ToString();
        }
        
    /*    private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                ContentFrame.Navigate(typeof(SettingsPage));
                
            }
            else
            {
               return;
                var item = sender.MenuItems.OfType<NavigationViewItem>().First(p => (string)p.Content == (string)args.InvokedItem);
              NavigationView_Navigate(item as NavigationViewItem);
            }
        }
        
        private void NavigationView_Navigate(NavigationViewItem item)
        {
            if (item.Tag== Home)
            {
                ContentFrame.Navigate(typeof(Home));
            }
           else if (item.Tag== Weather)
            {
                ContentFrame.Navigate(typeof(Weather));
            }
            else if (item.Tag== Music)
            {
                ContentFrame.Navigate(typeof(Music));
            }
            else if (item.Tag== Email)
            {
                ContentFrame.Navigate(typeof(Email));
            }
            
        }
        */
        private void NavigationView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (!ContentFrame.CanGoBack)
            {
                return;
            }
            //        if (NavigationView.IsPaneOpen)
            //      {
            //    NavigationView.IsPaneOpen = !NavigationView.IsPaneOpen;
            //                ContentFrame.GoBack();
            //     NavigationView.SelectedItem = ContentFrame.CanGoBack;
            //}
            if (ContentFrame.CanGoBack)
            {
                Type lastpage = ContentFrame.CurrentSourcePageType;
                ContentFrame.GoBack();
                if (ContentFrame.CurrentSourcePageType==typeof(Home))
                {
                    NavigationView.SelectedItem = Home;// ContentFrame.Navigate(typeof(Home));
                    

                    // return;
                }
                else   if (ContentFrame.CurrentSourcePageType == typeof(MyWeatherPage) )
                {
                    NavigationView.SelectedItem = Weather; //ContentFrame.Navigate(typeof(Weather));

                    // return;
                }
                else if (ContentFrame.CurrentSourcePageType == typeof(Music))// && ContentFrame.CurrentSourcePageType != lastpage)
                {
                    NavigationView.SelectedItem = Music; //ContentFrame.Navigate(typeof(Music));

                    //  return;
                }
                else  if (ContentFrame.CurrentSourcePageType == typeof(Email))// && ContentFrame.CurrentSourcePageType != lastpage)
                {
                    NavigationView.SelectedItem = Email;//return;   
                  //  ContentFrame.Navigate(typeof(Email));


                }
                else if (ContentFrame.CurrentSourcePageType == typeof(MapPage))
                {
                    NavigationView.SelectedItem = Maps;
                }
                else if (ContentFrame.CurrentSourcePageType == typeof(SettingsPage) )//&& ContentFrame.CurrentSourcePageType != lastpage)
                {
                    NavigationView.SelectedItem = NavigationView.SettingsItem;//return;
                //    ContentFrame.Navigate(typeof(SettingsPage));
                
                }

                //   var item = pages.FirstOrDefault(p => p.page == ContentFrame.CurrentSourcePageType);
                //   NavigationView.SelectedItem = NavigationView.MenuItems.OfType<NavigationViewItem>().Where(p => p.Tag.Equals(item.Tag));
                //    foreach (var panna in pages)
                //  {
                //    var kagaz = ((Type)pages.Where(p => p.page==ContentFrame.CurrentSourcePageType));
                //     NavigationView.SelectedItem = kagaz;
                // }
                //   var y = NavigationView.SelectedItem;
                //   NavigationView.SelectedItem = NavigationView.MenuItems.OfType<NavigationViewItem>().Where(p => p.Tag.Equals(sender.Tag));

                //     var item= ContentFrame.CurrentSourcePageType;
                //   NavigationView.SelectedItem = item;
                //            NavigationView.SelectedItem=NavigationView.MenuItems.
                //    ContentFrame.Navigate(ContentFrame.SourcePageType);
                //     NavigationView.SelectedItem = ContentFrame.CurrentSourcePageType;
            } //ContentFrame.
        }
        
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to Load Page" + e.SourcePageType.FullName);
        }

        private void ContentFrame_NavigationStopped(object sender, NavigationEventArgs e)
        {

        }

        private  async void Page_Loaded(object sender, RoutedEventArgs e)
        {
           // Hey.Text = "Page Loaded";

            //  MyBannerAd.HasAd
            //  MyBannerAd.IsLoaded
            //MyBannerAd.Loaded
            //  MyBannerAd.AdRefreshed
            // MyBannerAd.ErrorOccurred
            
            StorageFolder proxyDataFolder = await Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFile proxyFile = await proxyDataFolder.GetFileAsync("test.xml");
            await CurrentAppSimulator.ReloadSimulatorAsync(proxyFile);
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            AppLicenseInformation = CurrentAppSimulator.LicenseInformation;

            if (AppLicenseInformation.ProductLicenses["RemoveBannerAdOffer"].IsActive)
            {
               
                //   MyBannerAd.AutoRefreshIntervalInSeconds = 0;
                RemoveAd();
               // MyBannerAd.Visibility = Visibility.Collapsed;
             //   PurchaseButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Customer can NOT access this feature.
             //   MyBannerAd.Visibility = Visibility.Visible;
           //     PurchaseButton.Visibility = Visibility.Visible;
            }

        }


        private  void MyBannerAd_AdRefreshed(object sender, RoutedEventArgs e)
        { //Hey.Text = "Refresh";
            if (AdManager.IfPurchaseCompleted)
            {//purchased
            //    Hey.Text += "Purchased";
                if (!IsRemoved)
                {//ad not removed
              //      Hey.Text += "Not yet removed";
                    RemoveAd();
                }
                else
                {
                //    Hey.Text += "removed ";
//                    ad removed

                }
            }
            else
            {//not purchased
               // Hey.Text += "NOt Purchased";
                ContentGridColumnTwo.Width = new GridLength(90);
                

            }
/*
                        Hey.Text = "Ad Refreshed";
                        if (!AdManager.IfPurchaseCompleted)
                        {//NotPurchased
                            ContentGridColumnTwo.Width = new GridLength(90);
                            var t = MyBannerAd.HasAd.ToString();
                            Hey.Text += t;
                        }

                        //   MyBannerAdHelper();
                       else
                        {
                            Hey.Text = "Purchased";
                           //await AdManager.Purchased();

                        //        RemoveAd();

                            var t = MyBannerAd.HasAd.ToString();
                            Hey.Text += t;

                        }
            */
        }

        private void MyBannerAdHelper()
        {
            if (MyBannerAd.HasAd && MyBannerAd.IsLoaded)
            {
            //    AdGrid.Width = 90;
              //  AdGrid.Visibility = Visibility.Visible;
           //    MyBannerAd.Visibility = Visibility.Visible;
                ContentGridColumnTwo.Width = new GridLength(90);
                //  ContentGridColumnTwo.Width = new GridLength(728,GridUnitType.Pixel);
                
            }
            else
            {
                //      MyBannerAd.Visibility = Visibility.Collapsed;
                ContentGridColumnTwo.Width = new GridLength(0);
                return;

                //   ContentGridColumnTwo.Width = new GridLength(0,GridUnitType.Pixel);
                //  ContentGridColumnOne.Width = new GridLength(,GridUnitType.Star) ;
                //    AdGrid.Visibility = Visibility.Collapsed;
                //  AdGrid.Width = 0;

            }
        }


        private void MyBannerAd_ErrorOccurred(object sender, Microsoft.Advertising.WinRT.UI.AdErrorEventArgs e)
        {
            ContentGridColumnTwo.Width = new GridLength(0);

            //try
            //{
                //Hey.Text = "Error";
                if (AdManager.IfPurchaseCompleted)
                {//Purchase Completed
                  //  Hey.Text += "Purchased";
                    if (!IsRemoved)
                    {//but ad not removed
                    //    Hey.Text += "Not Yet Removed";
                        RemoveAd();
                    }
                    else
                    {//and ad is removed
                      //  Hey.Text += "Removed"; 
                    }
                }
                else
                {//NOT Purchased and certainly not removed so somme error
                    //Hey.Text += "NotPurchased";
                 //   MyBannerAd.IsAutoRefreshEnabled = false;
                   // MyBannerAd.Refresh();
                   
                      //MyBannerAd.Refresh();
                }
                // MyBannerAd.Refresh();

            //}
        //    catch (Exception )
          //  {
            //    Hey.Text+= "exception";
            //    throw;
           // }
        


            /*         if (!AdManager.IfPurchaseCompleted)
                     {
                         Hey.Text = "Error";
                         var t = MyBannerAd.HasAd.ToString();

                       //  ContentGridColumnTwo.Width = new GridLength(0);
                         Hey.Text += t;

                         //return;
         //                MyBannerAd.Refresh();
                     }
                     else
                     {
                         Hey.Text = "Purchased";
                         //  await AdManager.Purchased();
                         if (AdManager.IfPurchaseCompleted)
                         {
                             RemoveAd();
                         }
                         var t = MyBannerAd.HasAd.ToString();
                         Hey.Text += t;

                     }
                     ContentGridColumnTwo.Width = new GridLength(0);
                     MyBannerAd.Refresh();
         */
        }

        /*
                private void MyBannerAd_Loaded(object sender, RoutedEventArgs e)
                {

                    Hey.Text = "Loaded";
                    if(!AdManager.IfPurchased)
                    {
                        ContentGridColumnTwo.Width = new GridLength(0);

                    }
                    else
                    {
                        Hey.Text = "Purchased";
                        RemoveAd();

                    }

                }

        */

        public  void RemoveAd()
        {
            if (!IsRemoved)
            {
                
//        Hey.Text = "RemoveAd";
                var t = MyBannerAd.HasAd;

                //    ContentGridColumnTwo.Width = new GridLength(0);
                //AdGrid.Visibility = Visibility.Collapsed;
                // MyBannerAd.Visibility = Visibility.Collapsed;
               // MyBannerAd.AutoRefreshIntervalInSeconds = 0;
                t = MyBannerAd.HasAd;
  //              Hey.Text += t;

                MyBannerAd.IsAutoRefreshEnabled = false;
                t = MyBannerAd.HasAd;
    //            Hey.Text = "RemoveAd";
      //          Hey.Text += t;
                IsRemoved = true;
                ContentGridColumnTwo.Width = new GridLength(0);
                t = MyBannerAd.HasAd;
        //        Hey.Text = "RemoveAd";
          //      Hey.Text += t;
                AdManager.IfPurchaseCompleted = true;
                AdManager.IfPurchaseRequested = false;

            }

        }
        //Purchase Checher
/*
        public void PurchaseChecker()
        {
            if (AdManager.IfPurchaseRequested)
            {
                Hey.Text = "PurchaseChecker";
                AdManager.Purchased();
              //  RemoveAd();
            }
        }
 */     


    }
}
