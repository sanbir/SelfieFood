using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SelfieFood.Common;


namespace SelfieFood.Web.Controllers
{
    using System.IO;

    using Microsoft.ProjectOxford.Emotion;
    using Microsoft.ProjectOxford.Emotion.Contract;
    using Microsoft.ProjectOxford.Face;
    using Microsoft.ProjectOxford.Face.Contract;

    public class FoodApiController : ApiController
    {
        public ResturantsResponse PostPhoto()
        {
            return null;
        }

        private Face[] GetFaces(Byte[] image)
        {
            FaceServiceClient faceServiceClient = new FaceServiceClient("53ea3efc28f34a549049dc91e03bac69");
            using (var imageStream = new MemoryStream(image))
            {
                return
                    faceServiceClient.DetectAsync(
                        imageStream,
                        returnFaceAttributes: new FaceAttributeType[] { FaceAttributeType.Age, }).Result;
            }
        }

        private Emotion[] GetEmotions(Byte[] image)
        {
            EmotionServiceClient client = new EmotionServiceClient("cb68f67d5c47415ead0fd289b051acc6");
            using (var imageStream = new MemoryStream(image))
            {
                return client.RecognizeAsync(imageStream).Result;
            }
        }
    }
}