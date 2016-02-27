namespace SelfieFood.Common
{
    public class ResturantsResponse
    {
        public RestrauntInfo[] Variants { get; set; }
    }

    public class RestrauntInfo
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string Url { get; set; }

        public string ImageUrl { get; set; }

        public float FlampOverallRating { get; set; }

        public FlampFeedback[] FlampFeedbacks { get; set; }
    }

    public class FlampFeedback
    {
        public float Rating { get; set; }
        public string Text { get; set; }
    }
}