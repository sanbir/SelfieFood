using System.Collections.Generic;
using System.Linq;

using Microsoft.ProjectOxford.Emotion.Contract;
using Microsoft.ProjectOxford.Face.Contract;

namespace SelfieFood.DoubleGisApi
{
    public static class SearchRequestEvaluator
    {
        private const string Boy = "male";
        private const string Girl = "female";

        private static class Criteria
        {
            public static string[] ForDate = new string[]
                                                 {
                                                     FoodServiceAttribute.TableReservation,
                                                     FoodServiceAttribute.LiveMusic
                                                 };

            public static string[] ForGirlsCompany = new string[]
                                                 {
                                                     FoodServiceAttribute.VegetarianMenu,
                                                     FoodServiceAttribute.WineList
                                                 };

            public static string[] ForBoysCompany = new string[]
                                                 {
                                                     FoodServiceAttribute.SportOnTv,
                                                 };

            public static string[] ForKids = new string[]
                                                 {
                                                     FoodServiceAttribute.KidsMenu,
                                                 };

        }

        public static SearchRequest Evaluate(IReadOnlyCollection<Face> faces, IReadOnlyCollection<Emotion> emotions)
        {
            if (faces.Count > 2)
            {
                return ForGroup(faces, emotions);
            }
            else if (faces.Count == 2)
            {
                return ForPair(faces, emotions);
            }
            else if (faces.Count == 1)
            {
                return ForPerson(faces, emotions);
            }

            return new SearchRequest();
        }

        private static SearchRequest ForGroup(IReadOnlyCollection<Face> faces, IReadOnlyCollection<Emotion> emotions)
        {
            var result = new List<string>();

            var minAge = faces.Min(x => x.FaceAttributes.Age);
            if (minAge < 9)
            {
                result.AddRange(new[] { FoodServiceAttribute.KidsRoom });
            }

            if (minAge < 14)
            {
                result.AddRange(new[] { FoodServiceAttribute.KidsMenu });
            }

            if (faces.Any(x => x.FaceAttributes.Age > 10 && x.FaceAttributes.Age < 35))
            {
                result.Add(FoodServiceAttribute.Wifi);
            }

            return new SearchRequest(result);
        }

        private static SearchRequest ForPair(IReadOnlyCollection<Face> faces, IReadOnlyCollection<Emotion> emotions)
        {
            var result = new List<string>();

            if (faces.AnyKids())
            {
                result.AddRange(Criteria.ForKids);
            }

            if (faces.Any(x => x.FaceAttributes.Age > 10 && x.FaceAttributes.Age < 35))
            {
                result.Add(FoodServiceAttribute.Wifi);
            }

            return new SearchRequest();
        }

        private static SearchRequest ForPerson(IReadOnlyCollection<Face> faces, IReadOnlyCollection<Emotion> emotions)
        {
            var result = new List<string>();
            var q = string.Empty;

            var age = faces.First().FaceAttributes.Age;
            if (age < 14)
            {
                result.AddRange(new[] { FoodServiceAttribute.KidsMenu, FoodServiceAttribute.KidsRoom });
            }

            if (age > 10 && age <= 20)
            {
                result.Add(FoodServiceAttribute.Wifi);
            }

            if (faces.First().FaceAttributes.FacialHair.Beard + faces.First().FaceAttributes.FacialHair.Moustache +
                faces.First().FaceAttributes.FacialHair.Sideburns > 0.6)
            {
                q = "Пивной ресторан";
            }
            else if (faces.First().FaceAttributes.Gender.ToLower() == Boy && faces.First().FaceAttributes.Age > 30)
            {
                q = "гриль-бар";
            }
            else if (faces.First().FaceAttributes.Gender.ToLower() == Boy && faces.First().FaceAttributes.Age > 20)
            {
                q = "кофейни";
            }

            return new SearchRequest(result, q);
        }

        private static bool AnyKids(this IReadOnlyCollection<Face> faces)
        {
            return faces.Any(x => x.FaceAttributes.Age < 14);
        }

        private static double AgeDifference(this IReadOnlyCollection<Face> faces)
        {
            return faces.Max(x => x.FaceAttributes.Age) - faces.Min(x => x.FaceAttributes.Age);
        }

        private static bool OfSameGender(this IReadOnlyCollection<Face> faces)
        {
            return faces.All(x => x.FaceAttributes.Gender.ToLower() == Girl) || faces.All(x => x.FaceAttributes.Gender.ToLower() == Boy);
        }

        private static bool GirlsOnly(this IReadOnlyCollection<Face> faces)
        {
            return faces.All(x => x.FaceAttributes.Gender.ToLower() == Girl);
        }

        private static bool BoysOnly(this IReadOnlyCollection<Face> faces)
        {
            return faces.All(x => x.FaceAttributes.Gender.ToLower() == Boy);
        }
    }

    public class SearchRequest
    {
        public SearchRequest(IReadOnlyCollection<string> criteria = null, string searchQuery = "")
        {
            Criteria = criteria;
            SearchQuery = searchQuery;
        }

        public IReadOnlyCollection<string> Criteria { get; private set; }
        public string SearchQuery { get; private set; }
    }
}
