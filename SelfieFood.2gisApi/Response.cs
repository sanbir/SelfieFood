using System;
using System.Runtime.Serialization;

namespace SelfieFood.DoubleGisApi
{
    [DataContract]
    public class Response
    {
        [DataMember(Name = "result")]
        public Result Result { get; set; }
    }

    [DataContract]
    public class Result
    {
        [DataMember(Name = "items")]
        public Firm[] Items { get; set; }
    }

    [DataContract]
    public class Firm
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "address_name")]
        public string AddressName { get; set; }
        [DataMember(Name = "reviews")]
        public Reviews Reviews { get; set; }
        [DataMember(Name = "external_content")]
        public PhotoAlbum[] Album { get; set; }
    }

    [DataContract]
    public class Reviews
    {
        [DataMember(Name = "rating")]
        public float Rating { get; set; }
        [DataMember(Name = "recommendation_count")]
        public string Count { get; set; }
    }

    [DataContract]
    public class PhotoAlbum
    {
        [DataMember(Name = "main_photo_url")]
        public Uri MainPhotoUrl { get; set; }
    }
}