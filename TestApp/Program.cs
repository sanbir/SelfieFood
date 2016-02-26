using System.IO;
using SelfieFood.HowOldApi;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = new HowOldDataProvider();

            var res = HowOldDataProvider.GetUserAge(File.ReadAllBytes(@"C:\Users\y.vasilyev\Pictures\avatar_210x210.jpg"));
        }
    }
}
