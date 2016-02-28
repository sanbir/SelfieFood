using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SelfieFood.Common;

namespace SelfieFood.DoubleGisApi
{
    public class DoubleGisDataProvider
    {
        private const string ApiUri = "http://catalog.api.2gis.ru/2.0/catalog/branch/search";
        private const string CardDoubleGisTemplate = "https://2gis.ru/novosibirsk/search/{0}/firm/{1}";
        private const string CardFlampTemplate = "http://novosibirsk.flamp.ru/firm/{0}";
        private const string UserKey = "ruffzo9376";
        private const string DefaultsSearchString = "Поесть";
        private const string DefaultRegionId = "1";
        private const string PageSize  = "10";

        private readonly string[] _fields = { "items.reviews", "items.external_content" };

        public ResturantsResponse GetResturants(string searchString, IEnumerable<string> criteries,
            GeoLocationParameters geoLocationParameters = null)
        {
            var uri = CreateUri(searchString, criteries, geoLocationParameters);

            var resturants = MakeRequest(uri);

            var resturantsResponse = new ResturantsResponse()
            {
                Variants = resturants.ToArray()
            };

            //var firms = new List<RestrauntInfo>();
            //foreach (var item in response.Result.Items)
            //{
            //    var firmId = GetFirmId(item.Id);
            //    firms.Add(new RestrauntInfo()
            //    {
            //        Name = item.Name,
            //        DoubleGisCardUrl = GetCardDoubleGisUrl(firmId),
            //        CardFlampUrl = GetCardFlampUrl(firmId),
            //        ImageUrl =
            //            item.Album.Select(x => x.MainPhotoUrl != null ? x.MainPhotoUrl.ToString() : null)
            //                .FirstOrDefault(),
            //        FlampOverallRating = item.Reviews.Rating,
            //        Address = item.AddressName,
            //    });
            //}

            return resturantsResponse;
        }

        private Uri CreateUri(string searchString, IEnumerable<string> criteries, GeoLocationParameters geoLocationParameters)
        {
            var uriBuilder = new UriBuilder(ApiUri);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            query["q"] = string.IsNullOrWhiteSpace(searchString) ? DefaultsSearchString : searchString;
            query["region_id"] = DefaultRegionId;
            query["key"] = UserKey;

            query["fields"] = string.Join(",", _fields);
            query["sort"] = "flamp_rating";
            query["work_time"] = "now";
            query["page_size"] = PageSize;

            if (geoLocationParameters != null)
            {
                query["point"] = string.Format("{0},{1}", geoLocationParameters.Lon, geoLocationParameters.Lat);
                query["radius"] = geoLocationParameters.Radius.ToString();
            }

            foreach (var critery in criteries)
            {
                query.Add(string.Format("attr[{0}]", critery), true.ToString());
            }

            //TODO: Исправить проблемы с кодировкой
            uriBuilder.Query = HttpUtility.UrlDecode(query.ToString());

            return uriBuilder.Uri;
        }

        private static IEnumerable<RestrauntInfo> MakeRequest(Uri uri)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);

            using (var response = request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
        {
                    if (stream == null)
                        return new RestrauntInfo[] {};

                    var reader = new StreamReader(stream);

                    var str = reader.ReadToEnd();

                    return Parse(str);
                }
            }
        }

        private static IEnumerable<RestrauntInfo> Parse(string str)
        {
            var googleSearch = JObject.Parse(str);
            var results = googleSearch["result"]["items"].Children().ToList();

            return (from result in results
                select JsonConvert.DeserializeObject<Firm>(result.ToString())
                into searchResult
                let firmId = GetFirmId(searchResult.Id)
                select new RestrauntInfo()
            {
                    Name = searchResult.Name,
                    DoubleGisCardUrl = GetCardDoubleGisUrl(firmId),
                    CardFlampUrl = GetCardFlampUrl(firmId),
                    ImageUrl = searchResult.Album.Select(x => x.MainPhotoUrl != null? x.MainPhotoUrl.ToString() : null).FirstOrDefault(),
                    FlampOverallRating = searchResult.Reviews.Rating,
                    Address = searchResult.AddressName
                }).ToList();
        }

        private static string GetFirmId(string id)
                {
            return id.Substring(0, id.IndexOf("_", StringComparison.Ordinal));
                }

        private static string GetCardDoubleGisUrl(string id)
        {
            // NOTE: Нужно обязательно указать любой запрос
            return string.Format(CardDoubleGisTemplate, "SelfieFood", id);
            }

        private static string GetCardFlampUrl(string id)
        {
            return string.Format(CardFlampTemplate, id);
        }
    }
}