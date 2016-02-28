using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.Storage.Pickers;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;


namespace SelfieFood.SilverlightApp
{
    public partial class MainPage : PhoneApplicationPage
    {

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            _view = CoreApplication.GetCurrentView();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private async void viewActivated(CoreApplicationView sender, IActivatedEventArgs args1)
        {
            FileOpenPickerContinuationEventArgs args = args1 as FileOpenPickerContinuationEventArgs;

            if (args != null)
            {
                if (args.Files.Count == 0) return;

                _view.Activated -= viewActivated;
                var storageFile = args.Files[0];
                var stream = await storageFile.OpenAsync(FileAccessMode.Read);
                stream.Seek(0);

                using (var r = new BinaryReader(stream.AsStream()))
                {

                    var bytes = r.ReadBytes((int)stream.Size);

                    await ProcessImage(bytes);
                }
                //
                //                var decoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(stream);
                //                img.Source = bitmapImage;
            }
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

        private bool _clickEnabled = true;
        private CoreApplicationView _view;

        private void btnCapture_Click(object sender, RoutedEventArgs e)
        {
            if (!_clickEnabled)
                return;

            _clickEnabled = false;

            try
            {
                var camera = new CameraCaptureTask();

                camera.Completed += camera_Completed;
                camera.Show();
            }
            finally
            {
                _clickEnabled = true;
            }

        }

        private async void camera_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                //                var bitMap = new BitmapImage();
                //
                //                bitMap.SetSource(e.ChosenPhoto);
                //                // Set to Image Control  
                //                _faceImage.Stretch = System.Windows.Media.Stretch.UniformToFill;
                //                _faceImage.Source = bitMap;



                using (var r = new BinaryReader(e.ChosenPhoto))
                {
                    e.ChosenPhoto.Seek(0, SeekOrigin.Begin);

                    var bytes = r.ReadBytes((int)e.ChosenPhoto.Length);

                    await ProcessImage(bytes);
                }
            }
        }

        private async Task ProcessImage(byte[] bytes)
        {
            _busyIndicator.IsRunning = true;

            try
            {
                //var uri = new Uri("http://selfiefoodweb20160228034641.azurewebsites.net/Api/FoodApi/PostPhoto");
                            var uri = new Uri("http://uk-rnd-391:57164/Api/FoodApi/PostPhoto");

                var request = WebRequest.CreateHttp(uri);


                var data = await GetHttpPostResponse(request, bytes);

                if (data == null)
                {
                    MessageBox.Show("Ошибка при отправке запроса на сервер, попробуйте еще раз");
                    return;
                }

                NavigationService.Navigate(new Uri("/Results.xaml?d=" + data, UriKind.Relative));
            }
            catch
            {
                MessageBox.Show("Ошибка при отправке запроса на сервер, попробуйте еще раз");
            }
            finally
            {

                _busyIndicator.IsRunning = false;

            }

        }


        internal async Task<string> GetHttpPostResponse(HttpWebRequest request, byte[] requestBody)
        {
            string received = null;

            //var coord = await LocationService.GetCurrentLocation();

            //if (coord != null)
            //{
            //    //request.Headers["Lat"] = coord.Latitude.ToString(CultureInfo.InvariantCulture);
            //    //request.Headers["Lon"] = coord.Longitude.ToString(CultureInfo.InvariantCulture);
            //}

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = requestBody.Length;
            request.AllowReadStreamBuffering = true;
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
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        ;
                        // ASYNC: using StreamReader's async method to read to end, in case
                        // the stream i slarge.
                        received = await reader.ReadToEndAsync();
                    }

                }
            }
            catch (WebException we)
            {
                using (var reader = new StreamReader(we.Response.GetResponseStream()))
                {
                    var responseString = reader.ReadToEnd();
                    Debug.WriteLine(responseString);
                    return null;
                }

            }



            return received;
        }

        private void btnPredictRestraunt(object sender, RoutedEventArgs e)
        {
            var filePicker = new FileOpenPicker();
            filePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            filePicker.ViewMode = PickerViewMode.Thumbnail;

            // Filter to include a sample subset of file types
            filePicker.FileTypeFilter.Clear();
            filePicker.FileTypeFilter.Add(".bmp");
            filePicker.FileTypeFilter.Add(".png");
            filePicker.FileTypeFilter.Add(".jpeg");
            filePicker.FileTypeFilter.Add(".jpg");

            filePicker.PickSingleFileAndContinue();
            _view.Activated += viewActivated;
        }
    }
}