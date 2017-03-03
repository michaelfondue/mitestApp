using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Media;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Collections.Generic;

using Xamarin.Forms;

namespace cameratest
{
    public partial class resetPassword : ContentPage
    {
        public resetPassword()
        {
            InitializeComponent();
        }

        async void getNewPassword(object sender, EventArgs e)
        {
            if (eMail.Text == "E-Mail")
            {
                DisplayAlert("Fehler", "Bitte geben Sie Ihren Benutzernamen ein", "OK");
                return;
            }
            else if (!(eMail.Text.Contains("@") && eMail.Text.Contains(".")))
            {
                DisplayAlert("Fehler", "Bitte geben Sie eine gültige E-Mail Adresse an", "OK");
                return;
            }
            else
            {
                Uri uri = new Uri("http://app.tuboly-astronic.ch/app/forgotPassword.php");

                var client = new System.Net.Http.HttpClient();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("mail", eMail.Text));

                var content = new System.Net.Http.FormUrlEncodedContent(postData);
                var response = await client.PostAsync(uri, content);

                var answer = await response.Content.ReadAsStringAsync();
                //Debug.WriteLine("anser: "+answer);
                if (answer == "Connection established. Answer: 6")
                {
                    DisplayAlert("Erfolgreich", "Die Passwortzurücksetzung war erfolgreich, Sie werden in Kürze ein neues Passwort per E-mail erhalten", "OK");
                    await Navigation.PopAsync();
                }
                else if (answer == "Connection established. Answer: 0")
                {
                    //show that wrong password or username
                    Debug.WriteLine(answer);
                    DisplayAlert("Fehler", "Der Benutzername ist falsch", "OK");
                    
                    return;
                }
                else if (answer == "Connection established. Answer: 1")
                {
                    // not registered yet
                    Debug.WriteLine(answer);
                    DisplayAlert("Fehler", "erste query gut", "OK");
                    
                    return;
                }
                else if (answer == "Connection established. Answer: 3")
                {
                    // not registered yet
                    Debug.WriteLine(answer);
                    DisplayAlert("Fehler", "Passwort Query failed", "OK");
                    
                    return;
                }
                else if (answer == "Connection established. Answer: 2")
                {
                    // not registered yet
                    Debug.WriteLine(answer);
                    DisplayAlert("Fehler", "Query failed", "OK");
                    
                    return;
                }
                else if (answer == "Connection established. Answer: 5")
                {
                    // not registered yet
                    Debug.WriteLine(answer);
                    DisplayAlert("Fehler", "E-ail nicht versandt", "OK");
                    
                    return;
                }
                else if (answer == "Connection established. Answer: 4")
                {
                    // not registered yet
                    Debug.WriteLine(answer);
                    DisplayAlert("Fehler", "erste query gut", "OK");
                    
                    return;
                }
                else
                {
                    Debug.WriteLine(answer);
                    //no connection to the server
                    DisplayAlert("Fehler", "Keine Verbindung zum Server", "OK");
                   
                    return;
                }

                //});
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
