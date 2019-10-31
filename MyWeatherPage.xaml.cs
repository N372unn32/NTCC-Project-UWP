using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Xamarin.Essentials;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace College_Project_Version_One
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MyWeatherPage : Page
    {
        public MyWeatherPage()
        {
            this.InitializeComponent();
            Platform.MapServiceToken = "<<PLACE YOUR BING MAPS DEV CENTER KEY HERE>>";
            //https://www.bingmapsportal.com

        }

        private async void  Page_Loaded(object sender, RoutedEventArgs e)
        {
            
            try
            {
                MyProgressRing.IsActive = true;
                MyProgressRing.Visibility = Visibility.Visible;
                await CurrentLocationWeather();
            }
            catch
            {
                MyProgressRing.IsActive = false;
                MyProgressRing.Visibility = Visibility.Collapsed;

                DescriptionTextBlock.Text = "Unable To Get Data";
                DescriptionTextBlock.Visibility = Visibility.Visible;

                

            }


        }

        

        private async void GetDataButton_Click(object sender, RoutedEventArgs e)
        {
            MyProgressRing.IsActive = true;
            MyProgressRing.Visibility = Visibility.Visible;
            if (StaticRadioButton.IsChecked == true)
            {
                await CurrentLocationWeather();

            }
            else if (DynamicRadioButton.IsChecked == true)
            {
                if (LongitudeTextBlock.Text != "" && LatitudeTextBlock.Text != "")
                {
                    await IbrahimovicLocationWeather();

                }
            }
            else if (ByAddressRadioButton.IsChecked==true)
            {
                if (EnteredAddressTextBlock.Text != "")
                {
                    await Ronaldo();
                }
            }
                
        }

        private async void StaticRadioButton_Checked(object sender, RoutedEventArgs e)
        {   MyProgressRing.IsActive = true;
            MyProgressRing.Visibility = Visibility.Visible;
            EnteredAddressTextBlock.Visibility = Visibility.Collapsed;
            LongitudeTextBlock.Visibility = Visibility.Collapsed;
            LatitudeTextBlock.Visibility = Visibility.Collapsed;
            await CurrentLocationWeather();
        }

        private async Task CurrentLocationWeather()
        {
            MyProgressRing.IsActive = true;
            MyProgressRing.Visibility = Visibility.Visible;
            try
            {
                var position = await LocationManager.GetPosition();
                try
                {
                    await ReturnWeatherData(position.Coordinate.Point.Position.Latitude, position.Coordinate.Point.Position.Longitude);
                }

                catch (Exception e)
                {
                    MyProgressRing.IsActive = false;
                    MyProgressRing.Visibility = Visibility.Collapsed;

                    DescriptionTextBlock.Text = "Trouble Getting Current Weather Data"+ e.Message;
                    DescriptionTextBlock.Visibility = Visibility.Visible;

                }
            }

            catch
            {
                MyProgressRing.IsActive = false;
                MyProgressRing.Visibility = Visibility.Collapsed;

                DescriptionTextBlock.Text = "Trouble Getting Current Position";
                DescriptionTextBlock.Visibility = Visibility.Visible;

            }
        
        }

        private async Task IbrahimovicLocationWeather()
        {
            MyProgressRing.IsActive = true;
            MyProgressRing.Visibility = Visibility.Visible;
            if (double.TryParse(LatitudeTextBlock.Text, out var lt)&&double.TryParse(LongitudeTextBlock.Text, out var lg))
                {
                    await ReturnWeatherData(lt, lg);

                }

                //       await ReturnWeatherData(Convert.ToDouble(LatitudeTextBlock.Text), Convert.ToDouble(LongitudeTextBlock.Text));
           else
           { 
                MyProgressRing.IsActive = false;
                MyProgressRing.Visibility = Visibility.Collapsed;

                DescriptionTextBlock.Text = "Unable To parse coordinates";
                DescriptionTextBlock.Visibility = Visibility.Visible;
           }
        }
        private async void DynamicRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            MyProgressRing.IsActive = true;
            MyProgressRing.Visibility = Visibility.Visible;
            EnteredAddressTextBlock.Visibility = Visibility.Collapsed;
            LongitudeTextBlock.Visibility = Visibility.Visible;
            LatitudeTextBlock.Visibility = Visibility.Visible;
            if (LatitudeTextBlock.Text!=""&&LongitudeTextBlock.Text!="")
            {
                await IbrahimovicLocationWeather();
            } 

        }

        private async Task ReturnWeatherData(double lats, double longs)
        {
            MyProgressRing.IsActive = true;
            MyProgressRing.Visibility = Visibility.Visible;



            try
            {
                Rootobject myWeather = await OpenWeatherMapProxy.GetWeather(lats, longs);
                //string icon = String.Format("http://openweathermap.org/img/w/{0}.png",myWeather.weather[0].icon);
                string icon = String.Format("ms-appx:///Assets/Weather/{0}.png", myWeather.weather[0].icon);
                
                DataImage.Source = new BitmapImage(new Uri(icon, UriKind.Absolute));
                var t = myWeather.main.temp - 273.15;
                //   ResultTextBlock.Text = myWeather.name + "  " + myWeather.weather[0].description + "  " + myWeather.wind.speed + " " + ((int)myWeather.main.temp).ToString() + " " + myWeather.clouds.all;
                TemperatureTextBlock.Text = t.ToString() + "Celsius";
                //   TemperatureTextBlock.Text = ((int)myWeather.main.temp).ToString();
                DescriptionTextBlock.Text = myWeather.weather[0].description;
                LocationTextBlock.Text = myWeather.name;
                LocationTextBlock.Visibility = Visibility.Visible;
                TemperatureTextBlock.Visibility = Visibility.Visible;
                DescriptionTextBlock.Visibility = Visibility.Visible;

                MyProgressRing.IsActive = false;
                MyProgressRing.Visibility = Visibility.Collapsed;
                DataImage.Visibility = Visibility.Visible;
                LatitudeTextBlock.Text = lats.ToString();
                LongitudeTextBlock.Text = longs.ToString();
            }
            catch
            {
                MyProgressRing.IsActive = false;
                MyProgressRing.Visibility = Visibility.Collapsed;

                LocationTextBlock.Visibility = Visibility.Collapsed;
                TemperatureTextBlock.Visibility = Visibility.Collapsed;
                TemperatureTextBlock.Text = "Invalid Coordinates entered or data for the location not available";
                TemperatureTextBlock.Visibility = Visibility.Visible;

            }

        }

        private void LatitudeTextBlock_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
       //     args.Cancel = args.NewText.Any(c => (!char.IsDigit(c));
          //  LongitudeTextBlock.Text = sender.Text;


        }

        private async void ByAddressRadioButton_Click(object sender, RoutedEventArgs e)
        {

            LongitudeTextBlock.Visibility = Visibility.Collapsed;
            LatitudeTextBlock.Visibility = Visibility.Collapsed;
            EnteredAddressTextBlock.Visibility = Visibility.Visible;
            if (EnteredAddressTextBlock.Text != "")
            {
                await Ronaldo();
            }

        }

        private async Task Ronaldo()
        {   try
            {
                string address = EnteredAddressTextBlock.Text;
                var locations = await Geocoding.GetLocationsAsync(address);

                var location = locations?.FirstOrDefault();
                if (location != null)
                {
                    await ReturnWeatherData(location.Latitude, location.Longitude);

                }
                else
                {
                    MyProgressRing.IsActive = false;
                    MyProgressRing.Visibility = Visibility.Collapsed;

                    DescriptionTextBlock.Text = "Enter Address";
                    DescriptionTextBlock.Visibility = Visibility.Visible;
                }
            }
            catch
            {

                MyProgressRing.IsActive = false;
                MyProgressRing.Visibility = Visibility.Collapsed;

                DescriptionTextBlock.Text = "Exception Occured";
                DescriptionTextBlock.Visibility = Visibility.Visible;
            }
        }
    }
}
