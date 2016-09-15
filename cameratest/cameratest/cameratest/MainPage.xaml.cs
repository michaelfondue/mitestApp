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

        async void OnCallLogin(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Login());
        }
        async void OnCallRegistrierung(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Registrierung());
        }
        async void openSettings(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Settings());
        }

    }
}