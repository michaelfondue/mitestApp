using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Media;
using Xamarin.Forms;
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
    public partial class Registrierung : ContentPage
    {
        // Diese Klasse/Seite ist für die Registrierung eines Users in der Datenbank zuständig.
        public Registrierung()
        {
            InitializeComponent();
        }
        void Completed_eMail(object sender, EventArgs e)
        {
            // Das Feld E-Mail wurde ausgefüllt und man hat "Ok" gedrückt, es wird überprüft ob sich Elemente wie "@" und "." darin befinden.
            if (((Entry)sender).Text != "")
            {
                if (((Entry)sender).Text.Contains("@") && ((Entry)sender).Text.Contains("."))
                {
                }
                else
                {
                    // Errormessage für ungültige E-Mailadresse
                    DisplayAlert(AppResources.str_error, AppResources.str_validMailAdress, "OK");
                    return;
                }
            }
        }
        void Completed_passwordAgain(object sender, EventArgs e)
        {
            // Das Feld Passwort Wiederholung wurde ausgefüllt und man hat "Ok" gedrückt, es wird überprüft ob die Passwörter übereinstimmen.
            if (((Entry)sender).Text != password.Text)
            {
                DisplayAlert(AppResources.str_error, AppResources.str_nonMatchingPasswords, "OK");
                return;
            }
        }

        async void registering(object sender, EventArgs e)
        {
            // Der Knopf "Registrierung" wurde gedrückt, es werden veschiedene Checks durchgeführt.
            // Wurde eines der Felder nie ausgefüllt
            if (customerCompanyName.Text == null || reporterName.Text == null || eMail.Text == null || password.Text == null || passwordAgain.Text == null || phoneNumber.Text == null)
            {
                DisplayAlert(AppResources.str_error, AppResources.str_fillAll, "OK");
                return;
            }
            // Wurde eines der Felder ausgefüllt, Ok gedrückt, der Eintrag dann aber wieder gelöscht
            else if (customerCompanyName.Text == "" || reporterName.Text == "" || eMail.Text == "" || password.Text == "" || passwordAgain.Text == "" || phoneNumber.Text == "")
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
            else if (password.Text != passwordAgain.Text)
            {
                DisplayAlert(AppResources.str_error, AppResources.str_nonMatchingPasswords, "OK");
                return;
            }
            else
            {
                // Alle Checks wurden durchgeführt, es folgt die Verbindung zum Server
                Uri uri = new Uri("http://app.tuboly-astronic.ch/app/addUser.php");

                var client = new System.Net.Http.HttpClient();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                // Daten die dem Server geschickt werden
                var postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("companyName", customerCompanyName.Text));
                postData.Add(new KeyValuePair<string, string>("name", reporterName.Text));
                postData.Add(new KeyValuePair<string, string>("mail", eMail.Text));
                postData.Add(new KeyValuePair<string, string>("password", password.Text));
                postData.Add(new KeyValuePair<string, string>("phoneNumber", phoneNumber.Text));

                var content = new System.Net.Http.FormUrlEncodedContent(postData);
                var response = await client.PostAsync(uri, content);

                // Antwort des Servers abwarten und auslesen
                var answer = await response.Content.ReadAsStringAsync();
                if (answer == "Connection established. Answer: 1")
                {
                    // Alles hat geklappt, der User wurde Registriert und kann sich nun anmelden
                    DisplayAlert(AppResources.str_onSuccess, AppResources.str_registerSuccess, "OK");
                    await Navigation.PopAsync();
                }
                else if (answer == "Connection established. Answer: 0")
                {
                    // Der Benutzername existiert bereits
                    DisplayAlert(AppResources.str_error, AppResources.str_alreadyExistingUserName, "OK");
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

