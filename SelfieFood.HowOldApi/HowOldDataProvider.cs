using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace SelfieFood.HowOldApi
{
    public enum Sex
    {
        Unknown = 0,
        Male = 1,
        Female = 2
    }

    public class HowOldResult
    {
        public Face[] Faces { get; set; }
    }

    public class Face
    {
        public FaceAttribute Attributes { get; set; }
    }

    public class FaceAttribute
    {
        public Sex Gender { get; set; }
        public float Age { get; set; }
    }

    public class HowOldDataProvider
    {

        private static readonly HowOldResult DefaultResult = new HowOldResult();

        public static HowOldResult GetUserAge(byte[] photo)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://how-old.net/Home/Analyze?isTest=False&source=&version=how-old.net");

            request.ContentType = "application/octet-stream";
            request.Accept = "*/*";
            request.Method = "POST";
            request.Proxy = WebRequest.DefaultWebProxy;
            request.Proxy.Credentials = CredentialCache.DefaultNetworkCredentials;
            request.UseDefaultCredentials = true;
            request.Credentials = CredentialCache.DefaultNetworkCredentials;

            request.ContentLength = photo.Length;

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(photo, 0, photo.Length);
            }

            using (var response = request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    if (stream == null)
                        return DefaultResult;

                    var reader = new StreamReader(stream);

                    var  str = reader.ReadToEnd();

                    return Parse(str);
                }
            }
        }

        private static HowOldResult Parse(string str)
        {
            var s = JsonConvert.DeserializeObject<string>(str);
            return JsonConvert.DeserializeObject<HowOldResult>(s);            
        }
    }

}
