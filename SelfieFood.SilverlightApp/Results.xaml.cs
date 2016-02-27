using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Newtonsoft.Json;
using SelfieFood.Dto.SelfieFood.Dto;
using SelfieFood.SilverlightApp.ViewModels;

namespace SelfieFood.SilverlightApp
{
    public partial class Results : PhoneApplicationPage
    {
        private readonly List<ViewModel> _itemsSource = new List<ViewModel>(3);
        private readonly PeopleViewModel _peopleViewModel = new PeopleViewModel();

        public Results()
        {
            InitializeComponent();


            LoadData();
            slideView.DataContext = _itemsSource;
            PersonIcons.DataContext = _peopleViewModel.PeopleViewModels;
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

            _peopleViewModel.PeopleViewModels =
                resp.People.Select(p => new PersonViewModel(GetStyle(p), Convert.ToInt32(p.Age))).ToList();
        }

        private void OnPlayTap(object sender, GestureEventArgs e)
        {
            throw new NotImplementedException();
        }

        private Style GetStyle(FaceAttributes faceAttributes)
        {
            Style style;

            if (faceAttributes.Age < 5)
            {
                style = (Style) Resources["Infant"];
            }
            else if (faceAttributes.Age < 14)
            {
                style = (Style) Resources["Child"];
            }
            else if (faceAttributes.Age < 65 && faceAttributes.FacialHair.Beard < 0.5 &&
                     faceAttributes.Gender.ToLowerInvariant() == "male")
            {
                style = (Style) Resources["Man"];
            }
            else if (faceAttributes.Age < 65 && faceAttributes.FacialHair.Beard >= 0.5 &&
                     faceAttributes.Gender.ToLowerInvariant() == "male")
            {
                style = (Style) Resources["BeardedMan"];
            }
            else if (faceAttributes.Age < 65 && faceAttributes.Gender.ToLowerInvariant() == "female")
            {
                style = (Style) Resources["Woman"];
            }
            else
            {
                style = (Style) Resources["OldMan"];
            }

            return style;
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