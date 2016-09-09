using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace cameratest
{
    public partial class Registrierung : ContentPage
    {
        public Registrierung()
        {
            InitializeComponent();
        }

        async void registered(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Zwischenseite());
            //    //if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
            //    //{
            //    //    // Take a photo and save into cameratest.
            //    //    var file = await CrossMedia.Current.TakePhotoAsync(mediaOptions);
            //    //}
        }
    }
}
