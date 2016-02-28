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
using Microsoft.Phone.Tasks;
using Newtonsoft.Json;
using SelfieFood.Dto.SelfieFood.Dto;
using SelfieFood.SilverlightApp.ViewModels;

namespace SelfieFood.SilverlightApp
{
    public partial class Results : PhoneApplicationPage
    {
        private readonly List<ViewModel> _itemsSource = new List<ViewModel>(3);
        private readonly PeopleViewModel _peopleViewModel = new PeopleViewModel();

        public int ItemsCount { get { return _itemsSource.Count; } }

        public Results()
        {
            InitializeComponent();

            Loaded += Results_Loaded;


        }

        void Results_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
            slideView.DataContext = _itemsSource;
            PersonIcons.ItemsSource = _peopleViewModel.PeopleViewModels;
        }


        private void LoadData()
        {

            ResturantsResponse resp = null;
            string parameter;
            if (NavigationContext.QueryString.TryGetValue("d", out parameter))
            {
                resp = JsonConvert.DeserializeObject<ResturantsResponse>(parameter);

            }

            if (resp == null)
                return;

            var index = 1;
            foreach (var item in resp.Variants.Where(x => !string.IsNullOrWhiteSpace(x.ImageUrl)))
            {
                var model = new ViewModel
                {
                    Image = new Uri(item.ImageUrl, UriKind.Absolute),
                    Title = item.Name,
                    Index = index,
                    Likes = Convert.ToInt32(item.FlampOverallRating),
                    Url = item.DoubleGisCardUrl,
                    FlampUrl = item.CardFlampUrl,
                    Total = resp.Variants.Length
                };

                _itemsSource.Add(model);
                index++;
            }


            _peopleViewModel.PeopleViewModels =
                resp.People.Select(p => new PersonViewModel(GetStyle(p), Convert.ToInt32(p.Age))).ToList();
            AgeDiapasonText.Text = _peopleViewModel.AgeDiapason;
        }

        private void OnPlayTap(object sender, GestureEventArgs e)
        {
            var namewhatevz = new WebBrowserTask
            {
                Uri = new Uri(((ViewModel) slideView.SelectedItem).Url, UriKind.Absolute)
            };
            namewhatevz.Show();
        }

        private Style GetStyle(FaceAttributes faceAttributes)
        {
            Style style;

            faceAttributes.FacialHair = faceAttributes.FacialHair ?? new FacialHair();

            if (faceAttributes.Age < 5)
            {
                style = (Style)Resources["Infant"];
            }
            else if (faceAttributes.Age < 14)
            {
                style = (Style)Resources["Child"];
            }
            else if (faceAttributes.Age < 65 && faceAttributes.FacialHair.Beard < 0.5 &&
                     faceAttributes.Gender.ToLowerInvariant() == "male")
            {
                style = (Style)Resources["Man"];
            }
            else if (faceAttributes.Age < 65
                &&
                    (faceAttributes.FacialHair.Beard >= 0.5 || faceAttributes.FacialHair.Moustache >= 0.5 || faceAttributes.FacialHair.Sideburns >= 0.5)
                &&
                     faceAttributes.Gender.ToLowerInvariant() == "male")
            {
                style = (Style)Resources["BeardedMan"];
            }
            else if (faceAttributes.Age < 65 && faceAttributes.Gender.ToLowerInvariant() == "female")
            {
                style = (Style)Resources["Woman"];
            }
            else
            {
                style = (Style)Resources["OldMan"];
            }

            return style;
        }

        private void UIElement_OnTap(object sender, GestureEventArgs e)
        {
            var namewhatevz = new WebBrowserTask
            {
                Uri = new Uri(((ViewModel)slideView.SelectedItem).FlampUrl, UriKind.Absolute)
            };
            namewhatevz.Show();
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

        public string Url { get; set; }
        public string FlampUrl { get; set; }
        public int Total { get; set; }
    }
}