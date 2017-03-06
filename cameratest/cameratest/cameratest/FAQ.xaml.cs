using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace cameratest
{
    public partial class FAQ : ContentPage
    {
        public FAQ()
        {
            InitializeComponent();
        }
        async void openSettings(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Settings_loggedIn());
        }
        async void openInfoPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new infoPage());
        }
    }
}
