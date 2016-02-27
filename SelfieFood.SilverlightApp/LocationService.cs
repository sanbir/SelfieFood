using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace SelfieFood.SilverlightApp
{
    internal class LocationService
    {
        public static async Task<Geocoordinate> GetCurrentLocation()
        {

            var geolocator = new Geolocator
            {
                DesiredAccuracyInMeters = 50
            };

            try
            {
                var geoposition = await geolocator.GetGeopositionAsync(TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(10));

                return geoposition.Coordinate;
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x80004004)
                {
                    // the application does not have the right capability or the location master switch is off
                }
                //else
                {
                    // something else happened acquring the location
                }
            }

            return null;
        }
    }
}
