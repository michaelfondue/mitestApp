using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Media;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Collections.Generic;


namespace cameratest
{
    public partial class resetPassword : ContentPage
    {
        // Diese Klasse/Seite ist für den Erhalt eines neues Passwortes zuständig, wenn man sein altes vergessen hat
        public resetPassword()
        {
            InitializeComponent();
        }

        async void getNewPassword(object sender, EventArgs e)
        {
            // Checks des Formulars
            // Wurde eine E-Mailadresse angegeben
            if (eMail.Text == "E-Mail")
            {
                DisplayAlert(AppResources.str_error, AppResources.str_enterUsername, "OK");
                return;
            }
            // Ist es eine gültige E-Mailadresse
            else if (!(eMail.Text.Contains("@") && eMail.Text.Contains(".")))
            {
                DisplayAlert(AppResources.str_error, AppResources.str_validMailAdress, "OK");
                return;
            }
            else
            {
                // Alles war richtig, es folt die Verbindung zum Server
                Uri uri = new Uri("http://app.tuboly-astronic.ch/app/forgotPassword.php");

                var client = new System.Net.Http.HttpClient();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Daten, die dem Server gesendet werden
                var postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("mail", eMail.Text));

                var content = new System.Net.Http.FormUrlEncodedContent(postData);
                var response = await client.PostAsync(uri, content);

                // Die Antwort des Servers und seine Auswertung
                var answer = await response.Content.ReadAsStringAsync();

                if (answer == "Connection established. Answer: 6")
                {
                    // Das Passwort wurde zurückgesetzt und der User erhält ein neues Passwort per E-Mail
                    DisplayAlert(AppResources.str_onSuccess, AppResources.str_resetPasswordSuccess, "OK");
                    await Navigation.PopAsync();
                }
                else if (answer == "Connection established. Answer: 0")
                {
                    // Der Benutzername war noch gar nicht registriert
                    DisplayAlert(AppResources.str_error, AppResources.str_notRegistered, "OK");
                    
                    return;
                }
                else if (answer == "Connection established. Answer: 5")
                {
                    // Es gab ein Fehler auf dem Server und die E-Mail mit dem neuen Passwort konnte nicht versendet werden
                    DisplayAlert(AppResources.str_error, AppResources.str_noMail, "OK");
                    
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
