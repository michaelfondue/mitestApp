﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Plugin.Media;
using Xamarin.Forms;
using System.Net.Http.Headers;
using System.Net.Http;
using Plugin.Media.Abstractions;
using System.IO;

namespace cameratest
{
    public partial class BugReporting : ContentPage
    {
        int numPic;
        string picpath1;
        string picpath2;
        string picpath3;
        byte[] bypic;
        byte[] bypic2;
        byte[] bypic3;



        public BugReporting()
        {
            InitializeComponent();
        }

        async void sendingReport(object sender, EventArgs e)
        {
            Uri uri = new Uri("http://app.tuboly-astronic.ch/app/email.php");
            Uri picuri = new Uri("http://app.tuboly-astronic.ch/app/uploadpic.php");

            var client = new System.Net.Http.HttpClient();

            string boundary = "---8d0f01e6b3b5dafaaadada";

            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var multiPartContent = new MultipartFormDataContent(boundary);
            var selectedValueType = machineType.Items[machineType.SelectedIndex];
            var selectedValueProblem = sortOfProblem.Items[sortOfProblem.SelectedIndex];

            // content.Add(new StringContent(machineNumber.Text));
            // content.Add(new StringContent(selectedValueType));
            // content.Add(new StringContent(selectedValueProblem));
            // content.Add(new StringContent(problembeschreibung.Text));


            //byte[] fileContents = picpath1;
            //var beachImage = new Image { };
            //beachImage.Source = ImageSource.FromFile(picpath1);
            //byte[] img = GetBytes(beachImage);


            if (bypic != null)
            {
                multiPartContent.Add(new StreamContent(new MemoryStream(bypic)), "file", "upload.jpg");
            }
            if (bypic2 != null)
            {
                multiPartContent.Add(new StreamContent(new MemoryStream(bypic2)), "file2", "upload2.jpg");
            }
            if (bypic3 != null)
            {
                multiPartContent.Add(new StreamContent(new MemoryStream(bypic3)), "file3", "upload3.jpg");
            }

            HttpResponseMessage picresponse = await client.PostAsync(picuri, multiPartContent);
            //multiPartContent.Add(byteArrayContent, "file", "image" + uniqueId + ".jpg");

            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("machineNumber", machineNumber.Text));
            postData.Add(new KeyValuePair<string, string>("machineType", selectedValueType));
            postData.Add(new KeyValuePair<string, string>("sortOfProblem", selectedValueProblem));
            postData.Add(new KeyValuePair<string, string>("problemBeschrieb", problembeschreibung.Text));

            var content = new System.Net.Http.FormUrlEncodedContent(postData);

            //var content = new System.Net.Http.MultipartFormDataContent(postData);
            var response = await client.PostAsync(uri, content);
            var answer = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(answer);


            DisplayAlert("Fehlerbericht", "Es wurde eine E-Mail versendet", "OK");

            await Navigation.PushAsync(new Zwischenseite());
            //content.Add(new StreamImageSource(Image));
            //var content = new System.Net.Http.FormUrlEncodedContent(postData);
            // var content = new System.Net.Http.MultipartFormDataContent(postData);
            //var response = await client.PostAsync(uri, content);

        }

        void OnTapGestureRecognizerTapped(object sender, EventArgs args)
        { 

            var imageSender = (Image)sender;
 
            imageSender.Source = "tapped.jpg";

        }


        async void OnActionChoosePhoto(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Wählen Sie ", "Cancel", null, "Galerie", "Kamera");
            if (action == "Galerie")
            {
                pickPhoto(sender, e);
            } 
            else if (action == "Kamera") {
                takePhoto(sender, e);
            }
            else if(action == "Cancel")
            {
                return;
            }
        }
        // Photo aus Galerie auswählen
        async void pickPhoto(object sender, EventArgs e)
        {
        if (!CrossMedia.Current.IsPickPhotoSupported)
            {
            DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
            return;
            }
        if (numPic == 3)
            {
                DisplayAlert("Fehler", "Es sind bereits 3 Bilder ausgewählt", "OK");
                return;
            }
            //set Options for picking a picture
            var pickmediaOptions = new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Custom,
                CustomPhotoSize = 20
            };

            //var file = await CrossMedia.Current.PickPhotoAsync();
            var file = await CrossMedia.Current.PickPhotoAsync(pickmediaOptions);

            //Aufüllen von Gridview bis 3 Bilder eingefügt sind.
            if (file == null)
          return;
          //  await DisplayAlert("File Location", file.Path, "OK");
            if (numPic == 0)
            {
                numPic = 1;
                picpath1 = file.Path;
                image.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    //file.Dispose();

                    using (var memoryStream = new MemoryStream())
                    {
                        file.GetStream().CopyTo(memoryStream);
                        //file.Dispose();
                        bypic = memoryStream.ToArray();
                    }
                    file.Dispose();
                    return stream;
                });
                //await DisplayAlert("File Location", picpath1, "ok");
            }
            else if (numPic == 1)
            {
                numPic = 2;
                picpath2 = file.Path;
                image2.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    using (var memoryStream = new MemoryStream())
                    {
                        file.GetStream().CopyTo(memoryStream);
                        //file.Dispose();
                        bypic2 = memoryStream.ToArray();
                    }
                    file.Dispose();
                    return stream;
                });

            }
            else if (numPic == 2)
            {
                numPic = 3;
                picpath3 = file.Path;
                image3.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    using (var memoryStream = new MemoryStream())
                    {
                        file.GetStream().CopyTo(memoryStream);
                        //file.Dispose();
                        bypic3 = memoryStream.ToArray();
                    }
                    file.Dispose();
                    return stream;
                });
            }

    }

        
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
                    Name = $"{DateTime.UtcNow}.jpg",
                    SaveToAlbum = true,
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Custom,
                    CustomPhotoSize = 20
                };
                // Take a photo and save into cameratest.
                var file = await CrossMedia.Current.TakePhotoAsync(mediaOptions);
                

                if (file == null)
                    return;

                // DisplayAlert("File Location", file.Path, "OK");
                //Aufüllen von Gridview bis 3 Bilder eingefügt sind.

                  if (numPic == 0) {
                    numPic = 1;
                    picpath1 = file.Path;
                    image.Source = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        using (var memoryStream = new MemoryStream())
                        {
                            file.GetStream().CopyTo(memoryStream);
                            //file.Dispose();
                            bypic = memoryStream.ToArray();
                        }
                        file.Dispose();
                        return stream;
                    });
                 
                } else if (numPic == 1)
                {
                    numPic = 2;
                    picpath2 = file.Path;
                    image2.Source = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        using (var memoryStream = new MemoryStream())
                        {
                            file.GetStream().CopyTo(memoryStream);
                            //file.Dispose();
                            bypic2 = memoryStream.ToArray();
                        }
                        file.Dispose();
                        return stream;
                    });
                    
                } else if (numPic == 2)
                {
                    numPic = 3;
                    picpath3 = file.Path;
                    image3.Source = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        using (var memoryStream = new MemoryStream())
                        {
                            file.GetStream().CopyTo(memoryStream);
                            //file.Dispose();
                            bypic3 = memoryStream.ToArray();
                        }
                        file.Dispose();
                        return stream;
                    });
                }
           
            }
        }

        async void openSettings(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Settings());
        }
    }
}
