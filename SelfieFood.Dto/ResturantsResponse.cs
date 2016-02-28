namespace SelfieFood.Dto
{
    namespace SelfieFood.Dto
    {

        public class FaceAttributes
        {
            public double Age { get; set; }

            public string Gender { get; set; }

            public FacialHair FacialHair { get; set; }
        }

        public class FacialHair
        {
            public double Moustache { get; set; }

            public double Beard { get; set; }

            public double Sideburns { get; set; }
        }

        public class ResturantsResponse
        {
            public FaceAttributes[] People { get; set; }

            public RestrauntInfo[] Variants { get; set; }
        }

        public class RestrauntInfo
        {
            public string Name { get; set; }

            public string Address { get; set; }

            public string DoubleGisCardUrl { get; set; }

            public string ImageUrl { get; set; }

            public float FlampOverallRating { get; set; }

            public string CardFlampUrl { get; set; }
        }

        public class FlampFeedback
        {
            public float Rating { get; set; }
            public string Text { get; set; }
        }
    }
}
