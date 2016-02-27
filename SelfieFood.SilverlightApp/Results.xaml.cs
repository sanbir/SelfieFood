using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Phone.Controls;

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
            var resource = Application.GetResourceStream(new Uri("FirstLookData.txt", UriKind.RelativeOrAbsolute));
            using (var reader = new StreamReader(resource.Stream))
            {
                string line;
                var index = 1;
                while (!string.IsNullOrEmpty(line = reader.ReadLine()))
                {
                    var values = line.Split(';');
                    var model = new ViewModel
                    {
                        Image = new Uri(values[0], UriKind.Absolute),
                        Title = values[1],
                        Date = DateTime.Parse(values[2], CultureInfo.InvariantCulture),
                        Index = index,
                        Likes = values[3]
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
}