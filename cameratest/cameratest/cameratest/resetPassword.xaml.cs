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
        public resetPassword()
        {
            InitializeComponent();
        }

        async void getNewPassword(object sender, EventArgs e)
        {
            if (eMail.Text == "E-Mail")
            {
                DisplayAlert(AppResources.str_error, AppResources.str_enterUsername, "OK");
                return;
            }
            else if (!(eMail.Text.Contains("@") && eMail.Text.Contains(".")))
            {
                DisplayAlert(AppResources.str_error, AppResources.str_validMailAdress, "OK");
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
                    DisplayAlert(AppResources.str_onSuccess, AppResources.str_resetPasswordSuccess, "OK");
                    await Navigation.PopAsync();
                }
                else if (answer == "Connection established. Answer: 0")
                {
                    //show that wrong password or username
                    DisplayAlert(AppResources.str_error, AppResources.str_notRegistered, "OK");
                    
                    return;
                }
                else if (answer == "Connection established. Answer: 5")
                {
                    // not registered yet
                    Debug.WriteLine(answer);
                    DisplayAlert(AppResources.str_error, AppResources.str_noMail, "OK");
                    
                    return;
                }
                else
                {
                    Debug.WriteLine(answer);
                    //no connection to the server
                    DisplayAlert(AppResources.str_error, AppResources.str_noConnection, "OK");
                   
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
