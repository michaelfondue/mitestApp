using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Media;
using Xamarin.Forms;

namespace cameratest
{
    public partial class BugReporting : ContentPage
    {
        public BugReporting()
        {
            InitializeComponent();
        }
        //async void sendingReport(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new BugReporting());
        //}
        async void takePhoto(object sender, EventArgs e)
        {

            if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
            {
                // Supply media options for saving our photo after it's taken.
                var mediaOptions = new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "cameratest",
                    Name = $"{DateTime.UtcNow}.jpg"
                };

                // Take a photo and save into cameratest.
                var file = await CrossMedia.Current.TakePhotoAsync(mediaOptions);

                if (file == null)
                    return;

                DisplayAlert("File Location", file.Path, "OK");

                image.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });

            }
        }
    }
}
