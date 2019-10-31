using System;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Xamarin.Essentials;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace College_Project_Version_One
{//browser email sms
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Email : Page
    {
        //List<StorageFile> attachments = new List<StorageFile>();
        public Email()
        {
            this.InitializeComponent();
            //ExperimentalFeatures.Enable(ExperimentalFeatures.EmailAttachments);
            Battery.BatteryInfoChanged += Battery_BatteryInfoChanged;
            Battery_BatteryInfoChanged1(((int)(Battery.ChargeLevel * 100)), Battery.State);
        }



        private void Battery_BatteryInfoChanged(object sender, BatteryInfoChangedEventArgs e)
        {
            Battery_BatteryInfoChanged1(((int)(e.ChargeLevel * 100)), e.State);

        }
        private void Battery_BatteryInfoChanged1(int level, BatteryState state)
        {
            //      BatteryBar.Value = (int)(Battery.ChargeLevel * 100);
            BatteryBar.Value = level;
            //            var status = Battery.EnergySaverStatus;
            //  var foreground = new SolidColorBrush();
            var color = new Color();


            //switch (state)
            //{
            //    case BatteryState.Charging:
            //        {  // Currently charging
            //            color = Colors.WhiteSmoke;
            //            break;
            //        }
            //    case BatteryState.Full:
            //        {
            //            color = Colors.White;
            //            break;
            //        }
            //    case BatteryState.NotCharging:
            //        {
            //            color = Colors.Gray;
            //            break;
            //        }
            //    case BatteryState.NotPresent:
            //        {
            //            color = Colors.Black;
            //            break;
            //        }
            //    case BatteryState.Discharging:
            //        {
            //            color = Colors.Silver;
            //            break;
            //        }

            //    case BatteryState.Unknown:
            //        {   // Unable to detect battery state
            //            color = Colors.Blue;
            //            break;
            //        }
            //}
            if (state == BatteryState.Charging)
                //   ChargeText.Foreground = new SolidColorBrush(Colors.DarkGreen);
                //       BatteryBar.Background = new SolidColorBrush(Colors.DarkGreen);
                color = Colors.DarkGreen;
            else
                // ChargeText.Foreground= new SolidColorBrush(Colors.Black);
                color = Colors.Black;
            //     BatteryBar.Background = new SolidColorBrush(Colors.Black);

            var background = new SolidColorBrush(color);
            ChargeText.Foreground = background;

            if (BatteryBar.Value <= 10)
                color = Colors.Red;
            //   BatteryBar.Foreground = new SolidColorBrush(Colors.Red);
            else if (BatteryBar.Value <= 25)
                color = Colors.Yellow;
            else if (BatteryBar.Value <= 50)
                color = Colors.LightGreen;
            else if (BatteryBar.Value <= 80)
                color = Colors.Green;
            else if (BatteryBar.Value <= 100)
                color = Colors.DarkGreen;

            var foreground = new SolidColorBrush(color);
            BatteryBar.Foreground = foreground;


            ChargeText.Text = BatteryBar.Value.ToString();
        }

        private async void EmailButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //    List<string> revipients = EmailToBox.Text.Split(",").ToList();
                var listto = EmailToBox.Text.Split(",").ToList();
                var listBcc = EmailBCCBox.Text.Split(",").ToList();
                var listCc = EmailCCBox.Text.Split(",").ToList();

                EmailMessage message = new EmailMessage
                {
                    Body = EmailBodyBox.Text,
                    Subject = EmailSubjectBox.Text,
                    BodyFormat = EmailBodyFormat.PlainText

                };
                foreach (var item in listto)
                {
                    message.To.Add(item);

                }
                foreach (var item in listBcc)
                {
                    message.Bcc.Add(item);
                }
                foreach (var item in listCc)
                {
                    message.Cc.Add(item);
                }

                //foreach (var item in attachments)
                //{
                //    if (item != null)
                //        message.Attachments.Add(new EmailAttachment(item.Path, item.ContentType));

                //}
                //var message = new Xamarin.Essentials.SmsMessage
                await Xamarin.Essentials.Email.ComposeAsync(message);

            }
            catch (FeatureNotSupportedException fbsEx)
            {
                System.ArgumentException Ex = new ArgumentException("Feature Not Supported", fbsEx);
                throw Ex;

                // Email is not supported on this device
            }
            catch (Exception ex)
            {
                var a = new ArgumentException(ex.Message, ex);
                throw a;
                // Some other exception occurred
            }

        }




        private async void SMSButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SmsMessage message = new SmsMessage(SmsBodyBox.Text, SmsBodyBox.Text);
                await Sms.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException ex)
            {
                var d = new FeatureNotEnabledException(ex.Message, ex);
                // Sms is not supported on this device.
                throw d;
            }
            catch (Exception ex)
            {
                var d = new Exception(ex.Message, ex);
                throw ex;                // Other error has occurred.
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //Battery_BatteryInfoChanged1();
            // Battery.BatteryInfoChanged += Battery_BatteryInfoChanged;
            Battery_BatteryInfoChanged1(((int)(Battery.ChargeLevel * 100)), Battery.State);
        }

        private async void NoButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string s;
                if (string.IsNullOrWhiteSpace(NumberBox.Text))
                {
                    s = "https://docs.microsoft.com/en-us/xamarin/essentials/";
                    await Browser.OpenAsync(s, BrowserLaunchMode.SystemPreferred);
                }

                else
                    await Browser.OpenAsync($"https://{NumberBox.Text}", BrowserLaunchMode.SystemPreferred);

            }
            catch (Exception)
            {
                await Browser.OpenAsync("https://docs.microsoft.com/en-us/xamarin/essentials/");
            }
            
            

            //try
            //{ 
            //  PhoneDialer.Open(NumberBox.Text);
            //}
            //catch (ArgumentNullException anEx)
            //{
            //    // Number was null or white space
            //}
            //catch (FeatureNotSupportedException ex)
            //{
            //    // Phone Dialer is not supported on this device.
            //}
            //catch (Exception ex)
            //{
            //    // Other error has occurred.
            //}
        }

        private async void ShareButton_Click(object sender, RoutedEventArgs e)
        {
            string s;
            if (string.IsNullOrWhiteSpace(NumberBox.Text))
            {
                s = "Share Text Feature using Xamarin.Essentials";

            }
            else
            {
                s = NumberBox.Text;
            }
            await Share.RequestAsync(new ShareTextRequest
            {
              //  Title = "Share Web Link",
                Text =s, Subject= "Share Text Feature using Xamarin.Essentials"
            });
        }

        private async void ShareClipButton_Click(object sender, RoutedEventArgs e)
        {
            if (Clipboard.HasText)
            {
                await Share.RequestAsync(new ShareTextRequest
                { Title = "Share From Clipboard", Text = await Clipboard.GetTextAsync() });
            }
        }

        //private async void FileButton_Click(object sender, RoutedEventArgs e)
        //{
        //    FileOpenPicker openPicker = new FileOpenPicker();
        //    openPicker.ViewMode = PickerViewMode.List;
        //    openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        //    openPicker.FileTypeFilter.Add(".pdf");
        //    //openPicker.FileTypeFilter.Add(".doc");
        //    //openPicker.FileTypeFilter.Add(".docx");
        //    //openPicker.FileTypeFilter.Add(".ppt");
        //    //openPicker.FileTypeFilter.Add(".jpeg");
        //    //openPicker.FileTypeFilter.Add(".png");
        //    //openPicker.FileTypeFilter.Add(".mp3");
        //    //openPicker.FileTypeFilter.Add(".mp4");

        //    var files = await openPicker.PickSingleFileAsync();
        //    //    foreach (var item in files)
        //    //    {
        //    //        if (item != null)
        //    //            attachments.Add(item);
        //    //    }
        //    if (files != null) attachments.Add(files);
        //}
    }
}
