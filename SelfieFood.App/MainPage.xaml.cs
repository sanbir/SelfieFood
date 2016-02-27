using System;
using Windows.Devices.Sensors;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
//For MediaCapture
//For Encoding Image in JPEG format
//For storing Capture Image in App storage or in Picture Library

//For BitmapImage. for showing image on screen we need BitmapImage format.

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace SelfieFood.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            Start_Capture_Preview_Click();
        }
       
        //Declare MediaCapture object globally
        
       
        async private void Start_Capture_Preview_Click()
        {
            var captureManager = App.CaptureManager;        //Define MediaCapture object            
            await captureManager.InitializeAsync();     //Initialize MediaCapture and 
            capturePreview.Source = captureManager;     //Start preiving on CaptureElement
            captureManager.SetPreviewRotation(VideoRotation.Clockwise90Degrees);

            await captureManager.StartPreviewAsync();   //Start camera capturing 

            SimpleOrientationSensor sensor = SimpleOrientationSensor.GetDefault();
            sensor.OrientationChanged += (s, arg) =>
            {
                switch (arg.Orientation)
                {
                    case SimpleOrientation.Rotated90DegreesCounterclockwise:
                        captureManager.SetPreviewRotation(VideoRotation.None);
                        break;
                    case SimpleOrientation.Rotated180DegreesCounterclockwise:
                    case SimpleOrientation.Rotated270DegreesCounterclockwise:
                        captureManager.SetPreviewRotation(VideoRotation.Clockwise180Degrees);
                        break;
                    default:
                        captureManager.SetPreviewRotation(VideoRotation.Clockwise90Degrees);
                        break;
                }
            };

        }

        async private void Capture_Photo_Click(object sender, RoutedEventArgs e)
        {
            //Create JPEG image Encoding format for storing image in JPEG type
            ImageEncodingProperties imgFormat = ImageEncodingProperties.CreateJpeg();
            
            // create storage file in local app storage
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("Photo.jpg",CreationCollisionOption.ReplaceExisting);

            // take photo and store it on file location.
            await App.CaptureManager.CapturePhotoToStorageFileAsync(imgFormat, file);

            //// create storage file in Picture Library
            //StorageFile file = await KnownFolders.PicturesLibrary.CreateFileAsync("Photo.jpg",CreationCollisionOption.GenerateUniqueName);
                        
            // Get photo as a BitmapImage using storage file path.
             BitmapImage bmpImage = new BitmapImage(new Uri(file.Path));


        }

    }
}
