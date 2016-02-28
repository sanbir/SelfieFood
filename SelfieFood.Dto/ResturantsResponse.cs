using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

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
            public double Beard { get; set; }
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
}
