using System.IO;

using SelfieFood.DoubleGisApi;
using SelfieFood.HowOldApi;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var doubleGisProvider = new DoubleGisDataProvider("http://catalog.api.2gis.ru/2.0/catalog/branch/search");
            var result = doubleGisProvider.GetFirms(new[] { FoodServiceAttribute.LiveMusic });

            var data = new HowOldDataProvider();

            var res = HowOldDataProvider.GetUserAge(File.ReadAllBytes(@"C:\Users\y.vasilyev\Pictures\avatar_210x210.jpg"));
        }
    }
}
