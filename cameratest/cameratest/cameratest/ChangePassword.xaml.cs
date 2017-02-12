using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Media;
using System.Net;
using System.Net.Http;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.IO;
using System.Globalization;
using System.Net.Http.Headers;

namespace cameratest
{
    public partial class ChangePassword : ContentPage
    {
        public ChangePassword()
        {
            InitializeComponent();
        }

        void Completed_newPasswordRepeat(object sender, EventArgs e)
        {
            if (((Entry)sender).Text != newPassword.Text)
            {
                DisplayAlert("Fehler", "Die Passwörter stimmen nicht überein", "OK");
                return;
            }
        }

        async void changedPassword(object sender, EventArgs e)
        { //last check
            if (eMail.Text == "" || oldPassword.Text == "" || newPassword.Text == "" || newPasswordRepeat.Text == "")
            {
                DisplayAlert("Fehler", "Bitte füllen Sie alle Felder aus", "OK");
                return;
            }
            else if (!(eMail.Text.Contains("@") && eMail.Text.Contains(".")))
            {
                DisplayAlert("Fehler", "Bitte geben Sie eine gültige E-Mail Adresse an", "OK");
                return;
            }
            else if (newPassword.Text != newPasswordRepeat.Text)
            {
                DisplayAlert("Fehler", "Die Passwörter stimmen nicht überein", "OK");
                return;
            }
            else
            {

                Uri uri = new Uri("http://app.tuboly-astronic.ch/app/changePassword.php");

                var client = new System.Net.Http.HttpClient();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("newPassword", newPassword.Text));
                postData.Add(new KeyValuePair<string, string>("mail", eMail.Text));
                postData.Add(new KeyValuePair<string, string>("password", oldPassword.Text));

                var content = new System.Net.Http.FormUrlEncodedContent(postData);
                //var content = new System.Net.Http.MultipartFormDataContent(postData);
                var response = await client.PostAsync(uri, content);

                var answer = await response.Content.ReadAsStringAsync();
                if (answer == "Connection established. Answer: 1")
                {
                    DisplayAlert("Erfolgreich", "Sie haben Ihr Passwort erfolgreich geändert", "OK");
                    await Navigation.PushAsync(new MainPage());
                }
                else if (answer == "Connection established. Answer: 2")
                {
                    //show that wrong password or username
                    DisplayAlert("Fehler", "query failed, not registered yet", "OK");
                    return;
                }
                else if (answer == "Connection established. Answer: 3")
                {
                    //show that wrong password or username
                    DisplayAlert("Fehler", "zweite query failed", "OK");
                    return;
                }
                else if (answer == "Connection established. Answer: 4")
                {
                    //show that wrong password or username
                    DisplayAlert("Fehler", "pw falsch", "OK");
                    return;
                }
                else
                {
                    //no connection to the server
                    DisplayAlert("Fehler", "Keine Verbindung zum Server", "OK");
                    return;
                }

            }
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
