using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Media;
using Xamarin.Forms;

namespace cameratest
{
    public partial class feteli : ContentPage
    {

        public feteli()
        {
            InitializeComponent();
        }

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

       async void choosePhoto(object sender, EventArgs e)
        {


          if  (CrossMedia.Current.IsPickPhotoSupported)
            {
                // Display photo gallery
                var pic = await CrossMedia.Current.PickPhotoAsync();

               await DisplayAlert("File Location", pic.Path, "OK");

                if (pic == null)
                    return;

                /* var chosenPic = new Image { Aspect = Aspect.AspectFit };
                 String picpath = pic.Path;
                 chosenPic.Source = ImageSource.FromFile(picpath);
                 */
                   image.Source = ImageSource.FromStream(() =>
                {
                    var stream = pic.GetStream();
                    pic.Dispose();
                    return stream;
                });
                
            }

        }

    }
}


