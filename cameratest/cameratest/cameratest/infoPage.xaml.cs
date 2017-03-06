using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace cameratest
{
    public partial class infoPage : ContentPage
    {
        // Diese Klasse/Seite ist zur reinen Information da
        public infoPage()
        {
            InitializeComponent();
        }
        async void openSettings(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Settings());
        }
    }
}
