using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace cameratest
{
    public partial class ZoomPic : ContentPage
    {
        public ZoomPic()
        {
            InitializeComponent();
            zoomedPic.Source = BugReporting.picpath1;
        }

        
        BugReporting Bug = new BugReporting();
      

        async void OnDel(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Bild wirklich löschen?", "Nein", "Ja");
            if (action == "Nein")
            {
                return;
            }
            if (action == "Ja")
            {
                Bug.OnActionPictureOption(sender, e, 1);
                await Navigation.PushAsync(new BugReporting());
            }
         } 
      }
}
