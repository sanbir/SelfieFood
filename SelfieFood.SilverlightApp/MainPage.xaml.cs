﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Windows.Storage.Streams;
using Windows.Web.Http;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using SelfieFood.SilverlightApp.Resources;

namespace SelfieFood.SilverlightApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        //Instances  
        BitmapImage bitMap;
        Random rand;
        WriteableBitmap saveBitMap;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
        private void btnCapture_Click(object sender, RoutedEventArgs e)
        {

            CameraCaptureTask camera = new CameraCaptureTask();
            camera.Completed += camera_Completed;
            camera.Show();
        }

        private async void camera_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                bitMap = new BitmapImage();
                bitMap.SetSource(e.ChosenPhoto);
                // Set to Image Control  
                _faceImage.Stretch = System.Windows.Media.Stretch.UniformToFill;
                _faceImage.Source = bitMap;

                //e.ChosenPhoto.
                var client = new HttpClient();

                var r = new BinaryReader(e.ChosenPhoto);
                e.ChosenPhoto.Seek(0, SeekOrigin.Begin);

                var bytes = r.ReadBytes((int)e.ChosenPhoto.Length);

                var uri = new Uri("http://10.54.5.85:57164/Api/FoodApi/PostPhoto");
                
                var request = (HttpWebRequest)WebRequest.CreateHttp(uri);

                var data = await GetHttpPostResponse(request, bytes);
               
            }
        }


        internal static async Task<String> GetHttpPostResponse(HttpWebRequest request, byte[] requestBody)
        {
            string received = null;

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = requestBody.Length;
            // ASYNC: using awaitable wrapper to get request stream
            using (var postStream = await request.GetRequestStreamAsync())
            {
                // Write to the request stream.
                // ASYNC: writing to the POST stream can be slow
                await postStream.WriteAsync(requestBody, 0, requestBody.Length);
            }

            try
            {
                // ASYNC: using awaitable wrapper to get response
                var response = (HttpWebResponse)await request.GetResponseAsync();
                if (response != null)
                {
                    var reader = new StreamReader(response.GetResponseStream());
                    // ASYNC: using StreamReader's async method to read to end, in case
                    // the stream i slarge.
                    received = await reader.ReadToEndAsync();
                }
            }
            catch (WebException we)
            {
                var reader = new StreamReader(we.Response.GetResponseStream());
                string responseString = reader.ReadToEnd();
                Debug.WriteLine(responseString);
                return responseString;
            }

            return received;
        }

        private void btnPredictRestraunt(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}