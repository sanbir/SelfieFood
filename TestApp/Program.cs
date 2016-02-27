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
            var restaurants = doubleGisProvider.GetFirms("Поесть", new[] { FoodServiceAttribute.LiveMusic });
            var bars = doubleGisProvider.GetFirms("Бары / Пабы", new string[] { });
            var sushiRestaurant = doubleGisProvider.GetFirms("суши-бары / рестораны", new string[] { });

            var data = new HowOldDataProvider();

            var res = HowOldDataProvider.GetUserAge(File.ReadAllBytes(@"C:\Users\y.vasilyev\Pictures\avatar_210x210.jpg"));
        }
    }
}
