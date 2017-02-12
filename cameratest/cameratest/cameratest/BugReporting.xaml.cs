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

namespace cameratest
{
    public partial class BugReporting : ContentPage
    {
        public BugReporting()
        {
            InitializeComponent();
        }

        void Completed_MachineNumber(object sender, EventArgs e)
        {
            if (((Entry)sender).Text != "")
            {
                string stringNumber = ((Entry)sender).Text;
                int number = Convert.ToInt32(stringNumber);
                if (number > 2000)
                {
                    DisplayAlert("Fehler", "Die Maschinennummer ist zu gross", "OK");
                    return;
                }
            }
        }
        void Changed_MachineNumber(object sender, EventArgs e)
        {
            if (((Entry)sender).Text != "") 
            {
                string stringNumber = ((Entry)sender).Text;
                int number = Convert.ToInt32(stringNumber);
                if (number > 2000)
                {
                    DisplayAlert("Fehler", "Die Maschinennummer ist zu gross", "OK");
                    return;
                }
            }
        }
        //async void sendingReport(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new BugReporting());
        //}
        int numPic;
        async void takePhoto(object sender, EventArgs e)
        {

            if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
            {
                if (numPic == 3)
                {
                    DisplayAlert("Fehler", "Es sind bereits 3 Bilder ausgewählt", "OK");
                    return;
                }
                // Supply media options for saving our photo after it's taken.
                var mediaOptions = new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "cameratest",
                    Name = $"{DateTime.UtcNow}.jpg"
                };

                // Take a photo and save into cameratest.
                var file = await CrossMedia.Current.TakePhotoAsync(mediaOptions);

                if (file == null)
                    return;

                // DisplayAlert("File Location", file.Path, "OK");
                //Aufüllen von Gridview bis 3 Bilder eingefügt sind.

                  if (numPic == 0) {
                    numPic = 1;
                    image.Source = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        file.Dispose();
                        return stream;
                    });
                 
                } else if (numPic == 1)
                {
                    numPic = 2;
                    image2.Source = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        file.Dispose();
                        return stream;
                    });
                    
                } else if (numPic == 2)
                {
                    numPic = 3;
                    image3.Source = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        file.Dispose();
                        return stream;
                    });
                }
           
            }
        }

        async void openSettings(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Settings_loggedIn());
        }

        async void openInfoPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new infoPage());
        }
        //async void sendingReport(object sender, EventArgs e)
        //{
        //    //var client = new System.Net.WebClient();
        //    //Webclient client = new WebClient();
        //    HttpClient client = new HttpClient();
        //    Uri uri = new Uri("http://10.2.128.58/addUser.php");
        //    bool isNewItem = false;

        //    Dictionary<String, String> userInfo = new Dictionary<String, String>();
        //    userInfo.Add("companyName", "Apple");
        //    userInfo.Add("name", "name");
        //    userInfo.Add("mail", "mail");
        //    userInfo.Add("password", "password");

        //    string json = JsonConvert.SerializeObject(userInfo);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");
        //    HttpResponseMessage response = null;
        //    if (isNewItem)
        //    {
        //        response = await client.PostAsync(uri, content);
        //    }

        //    if (response.IsSuccessStatusCode)
        //    {
        //        Debug.WriteLine(@"TodoItem successfully saved.");
        //    }
        //    //client.UploadValuesCompleted += client_UploadValuesCompleted;

        //    //await Navigation.PushAsync(new Settings());
        //}

        //void client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
