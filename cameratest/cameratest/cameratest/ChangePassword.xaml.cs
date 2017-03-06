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
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.IO;
using System.Globalization;
using System.Net.Http.Headers;

namespace cameratest
{
    public partial class ChangePassword : ContentPage
    {
        // Diese Klasse/Seite stellt ist für das Ändern des Passwortes zuständig
        public ChangePassword()
        {
            InitializeComponent();
        }

        // Das wiederholte neue Passwort wird überprüft, ob es mit dem anderen neune übereinstimmt
        void Completed_newPasswordRepeat(object sender, EventArgs e)
        {
            if (((Entry)sender).Text != newPassword.Text)
            {
                DisplayAlert(AppResources.str_error, AppResources.str_nonMatchingPasswords, "OK");
                return;
            }
        }

        async void changedPassword(object sender, EventArgs e)
        {
            // Der Knopf "Passwort ändern" wurde gedrückt, es werden veschiedene Checks durchgeführt.
            // Wurde eines der Felder ausgefüllt, Ok gedrückt, der Eintrag dann aber wieder gelöscht
            if (eMail.Text == "" || oldPassword.Text == "" || newPassword.Text == "" || newPasswordRepeat.Text == "")
            {
                DisplayAlert(AppResources.str_error, AppResources.str_fillAll, "OK");
                return;
            }
            // Wurde eines der Felder nie ausgefüllt
            else if (eMail.Text == null || oldPassword.Text == null || newPassword.Text == null || newPasswordRepeat.Text == null)
            {
                DisplayAlert(AppResources.str_error, AppResources.str_fillAll, "OK");
                return;
            }
            // Befindet sich im E-Mail Feld eine gültige E-Mailadresse
            else if (!(eMail.Text.Contains("@") && eMail.Text.Contains(".")))
            {
                DisplayAlert(AppResources.str_error, AppResources.str_validMailAdress, "OK");
                return;
            }
            // Stimmen die Passwörter überein
            else if (newPassword.Text != newPasswordRepeat.Text)
            {
                DisplayAlert(AppResources.str_error, AppResources.str_nonMatchingPasswords, "OK");
                return;
            }
            else
            {
                // Alle Checks wurden durchgeführt, es folgt die Verbindung zum Server
                Uri uri = new Uri("http://app.tuboly-astronic.ch/app/changePassword.php");

                var client = new System.Net.Http.HttpClient();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Daten die dem Server geschickt werden
                var postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("newPassword", newPassword.Text));
                postData.Add(new KeyValuePair<string, string>("mail", eMail.Text));
                postData.Add(new KeyValuePair<string, string>("password", oldPassword.Text));

                var content = new System.Net.Http.FormUrlEncodedContent(postData);
                var response = await client.PostAsync(uri, content);

                // Antwort des Servers abwarten und auslesen
                var answer = await response.Content.ReadAsStringAsync();
                if (answer == "Connection established. Answer: 1")
                {
                    // Alles hat geklappt, das Passwort wurde geändert
                    DisplayAlert(AppResources.str_onSuccess, AppResources.str_changePasswordSuccess, "OK");
                    await Navigation.PushAsync(new MainPage());
                }
                else if (answer == "Connection established. Answer: 2")
                {
                    // Der Benutzer ist noch gar nicht registiert
                    DisplayAlert(AppResources.str_error, AppResources.str_notRegistered, "OK");
                    return;
                }
                else if (answer == "Connection established. Answer: 4")
                {
                    // Die Benutzerdaten sind falsch
                    DisplayAlert(AppResources.str_error, AppResources.str_wrongUserNameOrPassword, "OK");
                    return;
                }
                else
                {
                    // Keine Verbindung zum Server
                    DisplayAlert(AppResources.str_error, AppResources.str_noConnection, "OK");
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
