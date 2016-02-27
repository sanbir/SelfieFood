using System.IO;

using SelfieFood.DoubleGisApi;
using SelfieFood.HowOldApi;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var doubleGisProvider = new DoubleGisDataProvider();
            var restaurants = doubleGisProvider.GetResturants("Поесть", new[] { FoodServiceAttribute.LiveMusic });
            var bars = doubleGisProvider.GetResturants("Бары / Пабы", new string[] { }, new GeoLocationParameters()
            {
                Lat = 82,
                Lon = 55,
                Radius = 1000
            });
            var sushiRestaurant = doubleGisProvider.GetResturants("суши-бары / рестораны", new string[] { });

            var data = new HowOldDataProvider();

            var res = HowOldDataProvider.GetUserAge(File.ReadAllBytes(@"C:\Users\y.vasilyev\Pictures\avatar_210x210.jpg"));
        }
    }
}
