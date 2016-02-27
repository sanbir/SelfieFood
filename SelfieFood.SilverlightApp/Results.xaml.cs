using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace SelfieFood.SilverlightApp
{
    public partial class Results : PhoneApplicationPage
    {
        private List<ViewModel> itemsSource = new List<ViewModel>(3);
       

        public Results()
        {
            InitializeComponent();

            LoadData();


            this.slideView.DataContext = this.itemsSource;

        }

        private void LoadData()
        {
            StreamResourceInfo resource = Application.GetResourceStream(new Uri("FirstLookData.txt", UriKind.RelativeOrAbsolute));
            using (StreamReader reader = new StreamReader(resource.Stream))
            {
                string line = string.Empty;
                int index = 1;
                while (!string.IsNullOrEmpty(line = reader.ReadLine()))
                {
                    string[] values = line.Split(';');
                    ViewModel model = new ViewModel();

                    model.Image = new Uri(values[0], UriKind.Absolute); ;
                    model.Title = values[1];
                    model.Date = DateTime.Parse(values[2], CultureInfo.InvariantCulture);
                    model.Index = index;
                    model.Likes = values[3];

                    this.itemsSource.Add(model);
                    index++;
                }
            }
        }

        void bmi_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            
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

        public string Likes
        {
            get;
            set;
        }
    }
}