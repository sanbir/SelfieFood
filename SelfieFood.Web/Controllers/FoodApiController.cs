﻿using System;
using System.IO;
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
    using SelfieFood.DoubleGisApi;

    public class FoodApiController : ApiController
    {
        [HttpGet]
        [HttpPost]
        public async Task<ResturantsResponse> PostPhoto()
        {
           
            var data = await this.Request.Content.ReadAsByteArrayAsync();
            var faces = GetFaces(data);
            var emotions = GetEmotions(data);


            var searchRequest = SearchRequestEvaluator.Evaluate(faces, emotions);
            var dataProvider = new DoubleGisDataProvider("2gis.ru");
            var firms = dataProvider.GetFirms(searchRequest.Criteria);

            return new ResturantsResponse { Name = firms.Result.Items.First().Name };
        }

        private Face[] GetFaces(Byte[] image)
        {
            FaceServiceClient faceServiceClient = new FaceServiceClient("53ea3efc28f34a549049dc91e03bac69");
            return
                Task.Run(
                    () =>
                    faceServiceClient.DetectAsync(
                        new MemoryStream(image),
                        returnFaceAttributes: new FaceAttributeType[] { FaceAttributeType.Age, FaceAttributeType.Gender,  })).Result;
        }

        private Emotion[] GetEmotions(Byte[] image)
        {
            EmotionServiceClient client = new EmotionServiceClient("cb68f67d5c47415ead0fd289b051acc6");

            return Task.Run(() => client.RecognizeAsync(new MemoryStream(image))).Result;
        }
    }
}