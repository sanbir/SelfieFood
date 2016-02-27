using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Newtonsoft.Json;
using SelfieFood.Dto.SelfieFood.Dto;

namespace SelfieFood.SilverlightApp
{
    public partial class Results : PhoneApplicationPage
    {
        private readonly List<ViewModel> _itemsSource = new List<ViewModel>(3);

        public Results()
        {
            InitializeComponent();


            LoadData();
            slideView.DataContext = _itemsSource;
        }

 

        private void LoadData()
        {

            ResturantsResponse  resp = null;
            string parameter;
            if (NavigationContext.QueryString.TryGetValue("d", out parameter))
            {
                resp = JsonConvert.DeserializeObject<ResturantsResponse>(parameter);
                
            }

            if (resp == null)
                return;

            var resource = Application.GetResourceStream(new Uri("FirstLookData.txt", UriKind.RelativeOrAbsolute));
            using (var reader = new StreamReader(resource.Stream))
            {
                string line;
                var index = 1;
                foreach (var item in resp.Variants)
                {
                    var model = new ViewModel
                    {
                        Image = new Uri(item.ImageUrl, UriKind.Absolute),
                        Title = item.Name,
                        Index = index,
                        Likes = item.FlampFeedbacks.Length
                    };

                    _itemsSource.Add(model);
                    index++;
                }
            }
        }

        private void OnPlayTap(object sender, GestureEventArgs e)
        {
            throw new NotImplementedException();
        }
    }

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

        public int Likes
        {
            get;
            set;
        }
    }
}