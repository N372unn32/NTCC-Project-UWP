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
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Navigation;
using Xamarin.Essentials;
using Windows.Devices.Geolocation;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace College_Project_Version_One
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage : Page
    {
        public MapPage()
        {
            this.InitializeComponent();
            MyMap.Visibility = Visibility.Collapsed;

            Xamarin.Essentials.Platform.MapServiceToken = "<<PLACE YOUR BING MAPS DEV CENTER KEY HERE>>";

        }

        private async void CurrentButton_Click(object sender, RoutedEventArgs e)
        {
            await SetCurrentPosOnMAp();
        }
        private async Task SetCurrentPosOnMAp()
        {
          //  var something = await Xamarin.Essentials.Geolocation.GetLastKnownLocationAsync();
            var position = await LocationManager.GetPosition();
           // MyMap.Center.Position.Latitude = something.Latitude ;
            MyMap.Center = position.Coordinate.Point;
            MyMap.ZoomLevel = 17;
            MyMap.TrafficFlowVisible = true;
            LatTextBox.Text = position.Coordinate.Point.Position.Latitude.ToString();
            LongTextBox.Text = position.Coordinate.Point.Position.Longitude.ToString();
           // GetAddressTextBox.Text = position.VenueData.ToString();

        }
        private void SetPosButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Select_Terrain(object sender, RoutedEventArgs e)
        {
            if (sender == TerrainSubItem)
            {
                MyMap.Style = MapStyle.Terrain;
            }
            else if (sender == RoadSubItem)
            {
                MyMap.Style = MapStyle.Road;
            }
            else if (sender == NoneSubItem)
            {
                MyMap.Style = MapStyle.None;
            }
            else if (sender == AerialSubItem)
            {
                MyMap.Style = MapStyle.Aerial;
            }
        }

        private void ZoomSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            MyMap.ZoomLevel = ZoomSlider.Value;
        }

        private void MyMap_ZoomLevelChanged(MapControl sender, object args)
        {
            ZoomSlider.Value = MyMap.ZoomLevel;
        }

        private void MyMap_Loaded(object sender, RoutedEventArgs e)
        {
            MyMap.Visibility = Visibility.Visible;
        }

        private void MyMap_Unloaded(object sender, RoutedEventArgs e)
        {
            MyMap.Visibility = Visibility.Collapsed;
        }

        private async void AddressSubmitted_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(GetAddressTextBox.Text) && !string.IsNullOrWhiteSpace(GetAddressTextBox.Text))
                {
                    var EnteredAddress = GetAddressTextBox.Text;
                    //To get the list of address according to the Address entered
                    var PositionsFromAddresses = await Geocoding.GetLocationsAsync(EnteredAddress);
                    //to get the first address from that list 
                    var defaultPosition = PositionsFromAddresses?.FirstOrDefault();
                    if (defaultPosition != null)
                    {
                        BasicGeoposition NewCenter = new BasicGeoposition()
                        {
                            Latitude = defaultPosition.Latitude,
                            Longitude = defaultPosition.Longitude
                        };
                        Geopoint newcenter = new Geopoint(NewCenter);
                        MyMap.Center = newcenter;
                        MyMap.ZoomLevel = 15;
                    }
                    else
                    {
                        GetAddressTextBox.Text = "No Position Found";
                    }
                }
                else
                {
                    GetAddressTextBox.Text = "Error: Null/ White Space";
                }
            }
            catch (Exception)
            {
                GetAddressTextBox.Text = "Exception Occured";
            }



        }

        private void MyAppBar_Opened(object sender, object e)
        {

        }

        private void MyAppBar_Closed(object sender, object e)
        {
        }

        private async void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (double.TryParse(LatTextBox.Text, out var lt) && double.TryParse(LongTextBox.Text, out var lg))
                if (!string.IsNullOrEmpty(LongTextBox.Text) && !string.IsNullOrEmpty(LatTextBox.Text))
                {
                    if (double.TryParse(LatTextBox.Text, out var lt) && double.TryParse(LongTextBox.Text, out var lg))
                    {
                        //    var p = await Xamarin.Essentials.Geocoding.GetPlacemarksAsync(lt, lg);
                        //  var c = await Xamarin.Essentials.Geocoding.GetLocationsAsync(p.FirstOrDefault().ToString());
                        //var defaultPosition = c?.FirstOrDefault();
                        //if (defaultPosition != null)
                        if(lt>=0&&lg>=0)
                        {
                        BasicGeoposition NewCenter = new BasicGeoposition()
                        {
                            //  Latitude = defaultPosition.Latitude,
                            //Longitude = defaultPosition.Longitude
                            Latitude = lt, Longitude = lg 

                        };
                            Geopoint newcenter = new Geopoint(NewCenter);
                            MyMap.Center = newcenter;
                            MyMap.ZoomLevel = 15;
                        }
                        else
                        {
                            LatTextBox.Text = "Negative";
                            LongTextBox.Text = "Negative";
                        }
                        //MyMap.Center.Position.Latitude = lt;
                    }
                    else
                    {
                        LatTextBox.Text = "Unable to Parse";
                        LongTextBox.Text = "Unable to Parse";
                    }

                }
                else                    //Change the center of the map
                {
                    LatTextBox.Text = "Error: Null/ White Space";
                    LongTextBox.Text = "Error: Null/ White Space";
                }

            }
            catch (Exception)
            {

                LatTextBox.Text = "Exception Occured";
                LongTextBox.Text = "Exception Occured";
                await SetCurrentPosOnMAp();

            }

        }

        
        private async Task<Location> AddressToCoordinates()
        {  
            var EnteredAddress = GetAddressTextBox.Text;
            //To get the list of address according to the Address entered
            var PositionsFromAddresses = await Geocoding.GetLocationsAsync(EnteredAddress);
            //to get the first address from that list 
            var defaultPosition = PositionsFromAddresses?.FirstOrDefault();
            if (defaultPosition != null)
            {
                return defaultPosition;

                //{
                //    BasicGeoposition NewCenter = new BasicGeoposition()
                //    {
                //        Latitude = defaultPosition.Latitude,
                //        Longitude = defaultPosition.Longitude
                //    };
            }
            else
            {
                var loc = new Location();
                loc = null;
                return loc;
            }


        }

        private async void GoToDefault_Click(object sender, RoutedEventArgs e)
        {
            //Windows.UI.Popups.MessageDialog("Hello");
            if (!string.IsNullOrEmpty(GetAddressTextBox.Text) || !string.IsNullOrWhiteSpace(GetAddressTextBox.Text))
            {
                var p = await AddressToCoordinates();
                var options = new MapLaunchOptions { Name = GetAddressTextBox.Text, NavigationMode = Xamarin.Essentials.NavigationMode.Default };
                await Map.OpenAsync(p.Latitude, p.Longitude, options);
            }
            else
            {
                var p = await LocationManager.GetPosition();
                await Xamarin.Essentials.Map.OpenAsync(p.Coordinate.Point.Position.Latitude, p.Coordinate.Point.Position.Longitude);

            }

        }
    }
}
