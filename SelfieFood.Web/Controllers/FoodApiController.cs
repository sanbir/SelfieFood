using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;

using SelfieFood.Common;

namespace SelfieFood.Web.Controllers
{
    using System.Collections.Generic;

    using DoubleGisApi;

    public class FoodApiController : ApiController
    {
        [HttpGet]
        [HttpPost]
        public async Task<ResturantsResponse> PostPhoto()
        {
            var data = await this.Request.Content.ReadAsByteArrayAsync();
            var faces = GetFaces(data);
            var emotions = GetEmotions(data);

            //string lat;
            //Request.Headers.TryGetValues("Lat")
            //var geoLoc = new GeoLocationParameters() {Lat =[@"Lat"], Lon = Request.Headers["Lon"]};

            var searchRequests = SearchRequestEvaluator.Evaluate(faces, emotions);
            var dataProvider = new DoubleGisDataProvider();
            var firms = searchRequests.Select(x => dataProvider.GetResturants(x.SearchQuery, x.Criteria));
            var defaultFirms = dataProvider.GetResturants("", Enumerable.Empty<string>());

            var random = new Random();
            var pickedResults = Pick(firms.SelectMany(x => x.Variants), random.Next(4,8)).ToArray();
            var pickedResultNames = pickedResults.Select(x => x.Name);

            var defaultItems = Pick(defaultFirms.Variants.Where(x => !pickedResultNames.Contains(x.Name)), random.Next(2, 4));
            
            var result = new ResturantsResponse
            {
                Variants =
                    pickedResults.Concat(defaultItems)
                        .DistinctBy(x => x.Name)
                        .OrderByDescending(x => x.FlampOverallRating)
                        .ToArray(),
                People = faces.Select(f => f.FaceAttributes).ToArray()
            };

            return result;
        }

        private static Face[] GetFaces(byte[] image)
        {
            var faceServiceClient = new FaceServiceClient("53ea3efc28f34a549049dc91e03bac69");
            return
                Task.Run(
                    () =>
                    faceServiceClient.DetectAsync(
                        new MemoryStream(image),
                        returnFaceAttributes: new[] { FaceAttributeType.Age, FaceAttributeType.Gender, FaceAttributeType.FacialHair,  })).Result;
        }

        private static Emotion[] GetEmotions(byte[] image)
        {
            var client = new EmotionServiceClient("cb68f67d5c47415ead0fd289b051acc6");

            return Task.Run(() => client.RecognizeAsync(new MemoryStream(image))).Result;
        }

        private IEnumerable<RestrauntInfo> Pick(IEnumerable<RestrauntInfo> variants, int numberToPick)
        {
            var result = new List<RestrauntInfo>();
            var copyOfVariants = variants.ToList();

            var counter = 0;
            while (counter < numberToPick && copyOfVariants.Count > 0)
            {
                var random = new Random();
                foreach (var resturantsResponse in copyOfVariants.ToArray())
                {
                    if (random.Next(1, 5) > 3)
                    {
                        result.Add(resturantsResponse);
                        copyOfVariants.Remove(resturantsResponse);
                        counter++;
                        if (counter >= numberToPick)
                        {
                            return result;
                        }
                    }
                }
            }

            return result;
        }
    }
}