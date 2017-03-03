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
            if (eMail.Text == "E-Mail" || password.Text == "Passwort" || eMail.Text == "" || password.Text == "" || eMail.Text == null || password.Text == null)
            {
                DisplayAlert("Fehler", "Bitte geben Sie Ihre Benutzerdaten ein", "OK");
                return;
            }
            else if (!(eMail.Text.Contains("@") && eMail.Text.Contains(".")))
            {
                DisplayAlert("Fehler", "Bitte geben Sie eine gültige E-Mail Adresse an", "OK");
                return;
            }
            else
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
                    Globals g = Globals.getInstance();
                    g.setData(eMail.Text);
                    await Navigation.PushAsync(new Zwischenseite());
                    Navigation.RemovePage(this);
                }
                else if (answer == "Connection established. Answer: 0")
                {
                    //show that wrong password or username
                    DisplayAlert("Fehler", "Der Benutzername oder das Passwort ist falsch", "OK");
                    return;
                }
                else if (answer == "Connection established. Answer: 2")
                {
                    // not registered yet
                    DisplayAlert("Fehler", "Nicht registrierte Benutzerdaten", "OK");
                    return;
                }
                else
                {
                    //no connection to the server
                    DisplayAlert("Fehler", "Keine Verbindung zum Server", "OK");
                    return;
                }

                //});
            }
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
        async void forgotPassword(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new resetPassword());
        }
    }
}