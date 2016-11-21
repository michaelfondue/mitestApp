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
            HttpClient client = new HttpClient();
            Uri uri = new Uri("http://app.tuboly-astronic.ch/app/addUser.php");
            bool isNewItem = false;

            //Dictionary<string, string> userInfo = new Dictionary<string, string>();
            //string companyName = customerCompanyName.Text;
            //string name = reporterName.Text;
            //string mail = eMail.Text;
            //string password = password.Text;

            //userInfo.Add("companyName", companyName);
            //userInfo.Add("name", name);
            //userInfo.Add("mail", mail);
            //userInfo.Add("password", password);

            var userInfo = new TodoItem();
            userInfo.companyName = customerCompanyName.Text;
            userInfo.name = reporterName.Text;
            userInfo.mail = eMail.Text;

            string json = JsonConvert.SerializeObject(userInfo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
          
            HttpResponseMessage response = null;
            response = await client.PostAsync(uri, content);
            Debug.WriteLine(response);
            //if (isNewItem)
            //{
            //    response = await client.PostAsync(uri, content);
            //}

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine(@"TodoItem successfully saved.");
            }
            else
            {
                Debug.WriteLine("You lose");
            }
        }

        async void openSettings(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Settings());
        }


//        public async Task registerUser(TodoItem item, bool isNewItem = false)
//        {
//            RestUrl = http://app.tuboly-astronic.ch/app/addUser.php;
//            var uri = new Uri(string.Format(Constants.RestUrl, item.ID));

  
//            var json = JsonConvert.SerializeObject(item);
//            var content = new StringContent(json, Encoding.UTF8, "application/json");

//            HttpResponseMessage response = null;
//            if (isNewItem)
//            {
//                response = await client.PostAsync(uri, content);
//            }


//              if (response.IsSuccessStatusCode)
//                        {
//                            Debug.WriteLine(@"             TodoItem successfully saved.");

//                        }
//}
    }
}

public class TodoItem
{
    public string ID { get; set; }
    public string companyName { get; set; }
    public string name { get; set; }
    public string mail { get; set; }
}

//public class RestService : IRestService
//{
//    HttpClient client;

//  public RestService()
//    {
//        client = new HttpClient();
//        client.MaxResponseContentBufferSize = 256000;
//  }

//}