using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Media;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;

namespace cameratest
{
    public partial class MainPage : ContentPage
    {
        // Diese Klasse/Seite stellt ist die erste Seite der App und ist dazu da Benutzer einloggen und sich registrieren zu lassen
        public MainPage()
        {
            InitializeComponent();
            
        }
        
        async void loggedIn(object sender, EventArgs e)
        {
            // Der Knopf "Login" wurde gedrückt, es werden veschiedene Checks durchgeführt.
            // Wurde eines der Felder ausgefüllt, Ok gedrückt, der Eintrag dann aber wieder gelöscht
            if (eMail.Text == "E-Mail" || password.Text == "Passwort" || eMail.Text == "" || password.Text == "" || eMail.Text == null || password.Text == null)
            {
                DisplayAlert(AppResources.str_error, AppResources.str_enterUserData, "OK");
                return;
            }
            // Wurde eines der Felder nie ausgefüllt
            if (eMail.Text == null || password.Text == null || eMail.Text == null || password.Text == null)
            {
                DisplayAlert(AppResources.str_error, AppResources.str_enterUserData, "OK");
                return;
            }
            // Befindet sich im E-Mail Feld eine gültige E-Mailadresse
            //else if (!(eMail.Text.Contains("@") && eMail.Text.Contains(".")))
            //{
            //    DisplayAlert("Fehler", "Bitte geben Sie eine gültige E-Mail Adresse an", "OK");
            //    return;
            //}
            else
            {
                // Alle Checks wurden durchgeführt, es folgt die Verbindung zum Server
                Uri uri = new Uri("http://app.tuboly-astronic.ch/app/login.php");

                var client = new System.Net.Http.HttpClient();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Daten die dem Server geschickt werden
                var postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("mail", eMail.Text));
                postData.Add(new KeyValuePair<string, string>("password", password.Text));

                var content = new System.Net.Http.FormUrlEncodedContent(postData);
                var response = await client.PostAsync(uri, content);

                // Antwort des Servers abwarten und auslesen
                var answer = await response.Content.ReadAsStringAsync();
                if (answer == "Connection established. Answer: 1")
                {
                    // Die verbindung war erfolgreich, der Benutzer wird auf die Verbindungsseite geleitet und die jetztige Seite wird
                    // von der Navigation entfernt
                    // Speichern der Benutzermailadresse
                    Globals g = Globals.getInstance();
                    g.setData(eMail.Text);
                    await Navigation.PushAsync(new Zwischenseite());
                    Navigation.RemovePage(this);
                }
                else if (answer == "Connection established. Answer: 0")
                {
                    // Die Benutzerdaten sind falsch
                    DisplayAlert(AppResources.str_error, AppResources.str_wrongUserNameOrPassword, "OK");
                    return;
                }
                else if (answer == "Connection established. Answer: 2")
                {
                    // Der Benutzer ist noch gar nicht registiert
                    DisplayAlert(AppResources.str_error, AppResources.str_notRegistered, "OK");
                    return;
                }
                else
                {
                    // Keine Verbindung zum Server
                    DisplayAlert(AppResources.str_error, AppResources.str_noConnection, "OK");
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