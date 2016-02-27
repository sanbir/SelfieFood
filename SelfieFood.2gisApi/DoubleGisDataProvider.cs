using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Web;
using SelfieFood.Common;

namespace SelfieFood.DoubleGisApi
{
    public class DoubleGisDataProvider
    {
        private const string ApiUri = "http://catalog.api.2gis.ru/2.0/catalog/branch/search";
        private const string CardDoubleGisTemplate = "https://2gis.ru/novosibirsk/search/{0}/firm/{1}";
        private const string CardFlampTemplate = "http://novosibirsk.flamp.ru/firm/{0}";
        private const string UserKey = "ruffzo9376";
        private readonly string[] _fields = { "items.reviews", "items.external_content" };

        public ResturantsResponse GetResturants(string searchString, IEnumerable<string> criteries)
        {
            var uri = CreateUri(searchString, criteries);
            var response = MakeRequest(uri);

            var resturants = new ResturantsResponse();
            var firms = new List<RestrauntInfo>();

            foreach (var item in response.Result.Items)
            {
                var firmId = GetFirmId(item.Id);
                firms.Add(new RestrauntInfo()
                {
                    Name = item.Name,
                    Url = GetCardDoubleGisUrl(firmId),
                    ImageUrl = item.Album.Select(x => x.MainPhotoUrl?.ToString()).FirstOrDefault(),
                    FlampOverallRating = item.Reviews.Rating,
                    Address = item.AddressName,
                });
            }

            resturants.Variants = firms.ToArray();

            return resturants;
        }

        private static string GetFirmId(string id)
        {
            return id.Substring(0, id.IndexOf("_", StringComparison.Ordinal));
        }

        private Uri CreateUri(string searchString, IEnumerable<string> criteries)
        {
            var uriBuilder = new UriBuilder(ApiUri);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            query["q"] = searchString;

            query["region_id"] = 1.ToString();
            query["fields"] = string.Join(",", _fields);
            query["key"] = UserKey;
            query["sort"] = "flamp_rating";

            foreach (var critery in criteries)
            {
                query.Add($"attr[{critery}]", true.ToString());
            }

            //TODO: Исправить проблемы с кодировкой
            uriBuilder.Query = HttpUtility.UrlDecode(query.ToString());

            return uriBuilder.Uri;
        }

        private string GetCardDoubleGisUrl(string id)
        {
            // NOTE: Нужно обязательно указать запрос
            return string.Format(CardDoubleGisTemplate, "Поесть", id);
        }

        private string GetCardFlampUrl(string id)
        {
            return string.Format(CardFlampTemplate, id);
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