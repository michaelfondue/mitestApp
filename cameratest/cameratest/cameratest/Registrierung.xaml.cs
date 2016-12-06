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
using Newtonsoft.Json;
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

        async void registering(object sender, EventArgs e)
        {

            Uri uri = new Uri("http://app.tuboly-astronic.ch/app/addUser.php");

            var client = new System.Net.Http.HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("companyName", customerCompanyName.Text));
            postData.Add(new KeyValuePair<string, string>("name", reporterName.Text));
            postData.Add(new KeyValuePair<string, string>("mail", eMail.Text));
            postData.Add(new KeyValuePair<string, string>("password", password.Text));

            var content = new System.Net.Http.FormUrlEncodedContent(postData);
            //var content = new System.Net.Http.MultipartFormDataContent(postData);
            var response = await client.PostAsync(uri, content);
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

