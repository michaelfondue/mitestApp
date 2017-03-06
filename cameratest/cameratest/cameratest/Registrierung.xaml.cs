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
        public Registrierung()
        {
            InitializeComponent();
        }
        void Completed_eMail(object sender, EventArgs e)
        {
            if (((Entry)sender).Text != "")
            {
                if (((Entry)sender).Text.Contains("@") && ((Entry)sender).Text.Contains("."))
                {
                }
                else
                {
                    DisplayAlert(AppResources.str_error, AppResources.str_validMailAdress, "OK");
                    return;
                }
            }
        }
        void Changed_eMail(object sender, EventArgs e)
        {
            if (((Entry)sender).Text != "")
            {
            }
        }
        void Completed_password(object sender, EventArgs e)
        {
            if (((Entry)sender).Text != "")
            {
            }
        }
        void Changed_password(object sender, EventArgs e)
        {
            if (((Entry)sender).Text != "")
            {
            }
        }
        void Completed_passwordAgain(object sender, EventArgs e)
        {
            if (((Entry)sender).Text != password.Text)
            {
                DisplayAlert(AppResources.str_error, AppResources.str_nonMatchingPasswords, "OK");
                return;
            }
        }
        void Changed_passwordAgain(object sender, EventArgs e)
        {
            if (((Entry)sender).Text != "")
            {
            }
        }

        async void registering(object sender, EventArgs e)
        {
            //last check
            if (customerCompanyName.Text == "" || reporterName.Text == "" || eMail.Text == "" || password.Text == "" || passwordAgain.Text == "" || phoneNumber.Text == "")
            {
                DisplayAlert(AppResources.str_error, AppResources.str_fillAll, "OK");
                return;
            }
            else if (!(eMail.Text.Contains("@") && eMail.Text.Contains(".")))
            {
                DisplayAlert(AppResources.str_error, AppResources.str_validMailAdress, "OK");
                return;
            }
            else if (password.Text != passwordAgain.Text)
            {
                DisplayAlert(AppResources.str_error, AppResources.str_nonMatchingPasswords, "OK");
                return;
            }
            else
            {
                Uri uri = new Uri("http://app.tuboly-astronic.ch/app/addUser.php");

                var client = new System.Net.Http.HttpClient();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("companyName", customerCompanyName.Text));
                postData.Add(new KeyValuePair<string, string>("name", reporterName.Text));
                postData.Add(new KeyValuePair<string, string>("mail", eMail.Text));
                postData.Add(new KeyValuePair<string, string>("password", password.Text));
                postData.Add(new KeyValuePair<string, string>("phoneNumber", phoneNumber.Text));

                var content = new System.Net.Http.FormUrlEncodedContent(postData);
                //var content = new System.Net.Http.MultipartFormDataContent(postData);
                var response = await client.PostAsync(uri, content);

                var answer = await response.Content.ReadAsStringAsync();
                if (answer == "Connection established. Answer: 1")
                {
                    await DisplayAlert(AppResources.str_onSuccess, AppResources.str_registerSuccess, "OK");
                    await Navigation.PopAsync();
                }
                else if (answer == "Connection established. Answer: 0")
                {
                    //show that wrong password or username
                    DisplayAlert(AppResources.str_error, AppResources.str_alreadyExistingUserName, "OK");
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

