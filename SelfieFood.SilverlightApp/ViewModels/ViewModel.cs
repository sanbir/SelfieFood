using System;

namespace SelfieFood.SilverlightApp
{
    public class ViewModel
    {
        public int Index
        {
            get;
            set;
        }

        public Uri Image
        {
            get;
            set;
        }

        public string DateText
        {
            get
            {
                return this.Date.ToString("MMMM dd, yyyy");
            }
        }

        public DateTime Date
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Likes
        {
            get;
            set;
        }
    }
}