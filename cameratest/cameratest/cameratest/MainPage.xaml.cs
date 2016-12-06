using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Media;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Collections.Generic;

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
            Uri uri = new Uri("http://app.tuboly-astronic.ch/app/login.php");

            var client = new System.Net.Http.HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("mail", eMail.Text));
            postData.Add(new KeyValuePair<string, string>("password", password.Text));

            var content = new System.Net.Http.FormUrlEncodedContent(postData);
            var response = await client.PostAsync(uri, content);

            var answer = await response.Content.ReadAsStringAsync();
            //Debug.WriteLine("anser: "+answer);
            if (answer == "Connection established. Answer: 1")
            {
                await Navigation.PushAsync(new Zwischenseite());
            }
            else if (answer == "Connection established. Answer: 0")
            {
                //show that wrong password or username
            }
            else if(answer == "Connection established. Answer: 2")
            {
                // not registered yet
            }
            else
            {
                //no connection to the server
            }

            //});

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