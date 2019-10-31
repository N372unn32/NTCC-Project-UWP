using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Devices.Geolocation;

namespace College_Project_Version_One
{
   public class LocationManager
    {
        public static async Task<Geoposition> GetPosition()
        {   var accessStatus = await Geolocator.RequestAccessAsync();
            if (accessStatus!=GeolocationAccessStatus.Allowed)
            {
                throw new Exception();
            }
            var geolocator = new Geolocator { DesiredAccuracyInMeters = 10000};
            Geoposition position = await geolocator.GetGeopositionAsync();
            return position;
        }
    }
}
