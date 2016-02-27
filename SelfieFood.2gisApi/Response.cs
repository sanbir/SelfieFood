using System;
using Newtonsoft.Json;

namespace SelfieFood.DoubleGisApi
{
    public class Firm
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [JsonProperty(PropertyName = "address_name")]
        public string AddressName { get; set; }
        public Reviews Reviews { get; set; }
        [JsonProperty(PropertyName = "external_content")]
        public PhotoAlbum[] Album { get; set; }
    }
    
    public class Reviews
    {
        public float Rating { get; set; }
        public string Count { get; set; }
    }
    
    public class PhotoAlbum
    {
        [JsonProperty(PropertyName = "main_photo_url")]
        public Uri MainPhotoUrl { get; set; }
    }
}