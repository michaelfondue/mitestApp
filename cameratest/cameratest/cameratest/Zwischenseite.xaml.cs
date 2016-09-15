using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace cameratest
{
    public partial class Zwischenseite : ContentPage
    {
        public Zwischenseite()
        {
            InitializeComponent();
        }

        async void reportingBug(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BugReporting());
        }
        async void openSettings(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Settings());
        }
    }
}
