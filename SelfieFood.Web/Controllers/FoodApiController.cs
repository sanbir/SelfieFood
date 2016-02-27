﻿using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        //[HttpGet]
        //[HttpPost]
        //public async Task<ResturantsResponse> PostPhoto()
        //{
        //    var data = await this.Request.Content.ReadAsByteArrayAsync();
        //    var faces = GetFaces(data);
        //    var emotions = GetEmotions(data);

        //    var searchRequest = SearchRequestEvaluator.Evaluate(faces, emotions);
        //    var dataProvider = new DoubleGisDataProvider();
        //    var firms = dataProvider.GetResturants(searchRequest.SearchQuery, searchRequest.Criteria);

        //    // TODO: дописать АПИ
        //    return new ResturantsResponse
        //    {
        //        People = faces.Select(f=>f.FaceAttributes).ToArray(),
        //        Variants = { }
        //    };
        //}

        [HttpGet]
        [HttpPost]
        public async Task<ResturantsResponse> Test()
        {

            var searchRequest = new SearchRequest(new List<string>() { FoodServiceAttribute.KidsMenu });
            var dataProvider = new DoubleGisDataProvider();
            var firms = dataProvider.GetResturants(searchRequest.SearchQuery, searchRequest.Criteria);

            // TODO: дописать АПИ
            return new ResturantsResponse { };
        }

        private static Face[] GetFaces(byte[] image)
        {
            var faceServiceClient = new FaceServiceClient("53ea3efc28f34a549049dc91e03bac69");
            return
                Task.Run(
                    () =>
                    faceServiceClient.DetectAsync(
                        new MemoryStream(image),
                        returnFaceAttributes: new[] { FaceAttributeType.Age, FaceAttributeType.Gender, })).Result;
        }

        private static Emotion[] GetEmotions(byte[] image)
        {
            var client = new EmotionServiceClient("cb68f67d5c47415ead0fd289b051acc6");

            return Task.Run(() => client.RecognizeAsync(new MemoryStream(image))).Result;
        }
    }
}