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
        // Diese Klasse/Seite stellt die verlinkende Seite nach dem Login dar. Man kann über Sie einen Fehler melden und später
        // Häufig gestellte Fragen nachlesen und Videos zur Problembehandlung anschauen
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
            await Navigation.PushAsync(new Settings_loggedIn());
        }
        async void openInfoPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new infoPage());
        }
        async void openFAQ(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FAQ());
        }
        async void openVideos(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Videos());
        }
    }
}
