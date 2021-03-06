﻿using Microsoft.ProjectOxford.Face.Contract;

namespace SelfieFood.Common
{
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
}
