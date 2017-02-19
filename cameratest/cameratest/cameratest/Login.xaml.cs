using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace cameratest
{
    //Globals g = Globals.getInstance();
  
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        async void loggedIn(object sender, EventArgs e)
        {
            Uri uri = new Uri("http://app.tuboly-astronic.ch/app/login.php");

            var client = new System.Net.Http.HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("mail", eMail.Text));
            postData.Add(new KeyValuePair<string, string>("password", password.Text));

            Debug.WriteLine("hallooooooooooooooooooooooooooooooooooooooooo");

            var content = new System.Net.Http.FormUrlEncodedContent(postData);
            var response = await client.PostAsync(uri, content);
            Debug.WriteLine(response);


            var answer = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(answer);
            if (answer == "True")
            {
                //g.setData(eMail.Text);
                await Navigation.PushAsync(new Zwischenseite());
            }
            else if (answer == "False")
            {
                //show that wrong password or username
            }
            else
            {
                //no connection to the server
            }

            //});

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
