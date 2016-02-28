﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

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

            var searchRequest = SearchRequestEvaluator.Evaluate(faces, emotions);
            var dataProvider = new DoubleGisDataProvider();
            ResturantsResponse firms = dataProvider.GetResturants(searchRequest.SearchQuery, searchRequest.Criteria);
            ResturantsResponse defaultFirms = dataProvider.GetResturants("", Enumerable.Empty<string>());

            return new ResturantsResponse()
            {
                Variants = Pick(firms.Variants, 5).Union(Pick(defaultFirms.Variants, 2)).ToArray(),
                People = faces.Select(f => f.FaceAttributes).ToArray()
            };
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

        private IEnumerable<RestrauntInfo> Pick(IEnumerable<RestrauntInfo> v, int n)
        {
            var result = new List<RestrauntInfo>();
            var t = v.ToList();

            var counter = 0;
            while (counter < n && t.Count > 0)
            {
                var random = new Random();
                foreach (var resturantsResponse in t.ToArray())
                {
                    if (random.Next(1, 4) > 2)
                    {
                        result.Add(resturantsResponse);
                        t.Remove(resturantsResponse);
                        counter++;
                    }
                }
            }

            return result;
        }
    }
}