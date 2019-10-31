using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;

namespace College_Project_Version_One
{

  public static class AdManager
    {
        //Receive request to remove ads
      public static  bool IfPurchaseCompleted = false;
        public static bool IfPurchaseRequested = false;
        public async static  Task GetRequestToRemoveAd(bool request)
        {  IfPurchaseRequested=request;
            // MainPage t = new MainPage();

            await Purchased();

        }
        //send request to remove ads
        public static async Task Purchased()
        {
            if (!MainPage.AppLicenseInformation.ProductLicenses["RemoveBannerAdOffer"].IsActive)
            {
                try
                {
                    // The customer doesn't own this feature, so 
                    // show the purchase dialog.

                    PurchaseResults results = await CurrentAppSimulator.RequestProductPurchaseAsync("RemoveBannerAdOffer");

                    //Check the license state to determine if the in-app purchase was successful.

                    if (results.Status == ProductPurchaseStatus.Succeeded)
                    {
                        AdManager.IfPurchaseCompleted = true;
                        AdManager.IfPurchaseRequested = false;
                        
                        //    RemoveAd();
                        // ADGRid.Visibility = Visibility.Collapsed;
                        //   MyBannerAd.Visibility = Visibility.Collapsed;
                        /// PurchaseButton.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        AdManager.IfPurchaseRequested = false;
                        AdManager.IfPurchaseCompleted = false;
                    }
                }
                catch (Exception ex)
                {
                    AdManager.IfPurchaseRequested = false;
                    AdManager.IfPurchaseCompleted = false;
                    // The in-app purchase was not completed because 
                    // an error occurred.
                    throw ex;

                }
            }
            else
            {
                AdManager.IfPurchaseCompleted = true;
                AdManager.IfPurchaseRequested = false;

                // The customer already owns this feature.
            }
        }

    }

}
