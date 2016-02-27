using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Web;

namespace SelfieFood.DoubleGisApi
{
    public class DoubleGisDataProvider
    {
        private readonly string _ApiUri = "http://catalog.api.2gis.ru/2.0/catalog/branch/search";
        private readonly string _userKey = "ruffzo9376";
        private readonly string[] _fields = { "items.reviews", "items.external_content" };

        public Response GetFirms(string searchString, IEnumerable<string> criteries)
        {
            var uriBuilder = new UriBuilder(_ApiUri);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            query["q"] = string.IsNullOrWhiteSpace(searchString) ? "Поесть" : searchString;

            query["region_id"] = 1.ToString();
            query["fields"] = string.Join(",", _fields);
            query["key"] = _userKey;
            query["sort"] = "flamp_rating";

            foreach (var critery in criteries)
            {
                query.Add(string.Format("attr[{0}]", critery), true.ToString());
            }

            //TODO: Исправить проблемы с кодировкой
            uriBuilder.Query = HttpUtility.UrlDecode(query.ToString());

            return MakeRequest(uriBuilder.Uri);
        }

        private static Response MakeRequest(Uri uri)
        {
            var request = WebRequest.Create(uri) as HttpWebRequest;
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(string.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));
                }
                var jsonSerializer = new DataContractJsonSerializer(typeof(Response));
                var objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                var jsonResponse = objResponse as Response;
                return jsonResponse;
            }
        }
    }
}