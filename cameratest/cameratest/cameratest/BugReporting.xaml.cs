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
        int numPic;
        async void takePhoto(object sender, EventArgs e)
        {

            if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
            {
                if (numPic == 3)
                {
                    DisplayAlert("Fehler", "Es sind bereits 3 Bilder ausgewählt", "OK");
                    return;
                }
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

                // DisplayAlert("File Location", file.Path, "OK");
                //Aufüllen von Gridview bis 3 Bilder eingefügt sind.

                  if (numPic == 0) {
                    numPic = 1;
                    image.Source = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        file.Dispose();
                        return stream;
                    });
                 
                } else if (numPic == 1)
                {
                    numPic = 2;
                    image2.Source = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        file.Dispose();
                        return stream;
                    });
                    
                } else if (numPic == 2)
                {
                    numPic = 3;
                    image3.Source = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        file.Dispose();
                        return stream;
                    });
                }
           
            }
        }

        async void openSettings(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Settings());
        }
    }
}
