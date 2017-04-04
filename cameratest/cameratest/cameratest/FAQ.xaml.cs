using System;
using Xamarin.Forms;
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
        async void openQuestion1(object sender, EventArgs e)
        {
            if (answer1.IsVisible == true)
            {
                answer1.IsVisible = false;
            }
            else
            {
                answer1.IsVisible = true;
            }
        }
        async void openQuestion2(object sender, EventArgs e)
        {
            if (answer2.IsVisible == true)
            {
                answer2.IsVisible = false;
            }
            else
            {
                answer2.IsVisible = true;
            }
        }
        async void openQuestion3(object sender, EventArgs e)
        {
            if (answer3.IsVisible == true)
            {
                answer3.IsVisible = false;
            }
            else
            {
                answer3.IsVisible = true;
            }
        }
        async void openQuestion4(object sender, EventArgs e)
        {
            if (answer4.IsVisible == true)
            {
                answer4.IsVisible = false;
            }
            else
            {
                answer4.IsVisible = true;
            }
        }
        async void openQuestion5(object sender, EventArgs e)
        {
            if (answer5.IsVisible == true)
            {
                answer5.IsVisible = false;
            }
            else
            {
                answer5.IsVisible = true;
            }
        }
        async void openQuestion6(object sender, EventArgs e)
        {
            if (answer6.IsVisible == true)
            {
                answer6.IsVisible = false;
            }
            else
            {
                answer6.IsVisible = true;
            }
        }
        async void openQuestion7(object sender, EventArgs e)
        {
            if (answer7.IsVisible == true)
            {
                answer7.IsVisible = false;
            }
            else
            {
                answer7.IsVisible = true;
            }
        }
        async void openQuestion8(object sender, EventArgs e)
        {
            if (answer8.IsVisible == true)
            {
                answer8.IsVisible = false;
            }
            else
            {
                answer8.IsVisible = true;
            }
        }
    
    }
}
