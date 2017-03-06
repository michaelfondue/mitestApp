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
        public ChangePassword()
        {
            InitializeComponent();
        }

        void Completed_newPasswordRepeat(object sender, EventArgs e)
        {
            if (((Entry)sender).Text != newPassword.Text)
            {
                DisplayAlert(AppResources.str_error, AppResources.str_nonMatchingPasswords, "OK");
                return;
            }
        }

        async void changedPassword(object sender, EventArgs e)
        { //last check
            if (eMail.Text == "" || oldPassword.Text == "" || newPassword.Text == "" || newPasswordRepeat.Text == "")
            {
                DisplayAlert(AppResources.str_error, AppResources.str_fillAll, "OK");
                return;
            }
            else if (!(eMail.Text.Contains("@") && eMail.Text.Contains(".")))
            {
                DisplayAlert(AppResources.str_error, AppResources.str_validMailAdress, "OK");
                return;
            }
            else if (newPassword.Text != newPasswordRepeat.Text)
            {
                DisplayAlert(AppResources.str_error, AppResources.str_nonMatchingPasswords, "OK");
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
                    DisplayAlert(AppResources.str_onSuccess, AppResources.str_changePasswordSuccess, "OK");
                    await Navigation.PushAsync(new MainPage());
                }
                else if (answer == "Connection established. Answer: 2")
                {
                    //show that wrong password or username
                    DisplayAlert(AppResources.str_error, AppResources.str_notRegistered, "OK");
                    return;
                }
                else if (answer == "Connection established. Answer: 4")
                {
                    //show that wrong password or username
                    DisplayAlert(AppResources.str_error, AppResources.str_wrongUserNameOrPassword, "OK");
                    return;
                }
                else
                {
                    //no connection to the server
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
