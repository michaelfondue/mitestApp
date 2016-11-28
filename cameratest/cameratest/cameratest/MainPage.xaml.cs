using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Media;

namespace cameratest
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async void loggedIn(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Zwischenseite());
        }
        async void OnCallRegistrierung(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Registrierung());
        }
        async void openSettings(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Settings());
        }
        async void openInfoPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new infoPage());
        }
    }
}