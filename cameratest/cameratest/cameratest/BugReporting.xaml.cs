﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Plugin.Media;
using Xamarin.Forms;
using System.Net;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http;
using Plugin.Media.Abstractions;
using System.IO;


namespace cameratest
{
    public partial class BugReporting : ContentPage
    {
        public static int numPic; //Number of Pictures added
        public static int Picnum = 1; // Number of Picturecontainer
        public static int bild; // Bildwahl für Tapauswahl
        public static string zoompicpath; //Pfad für ZoomedPic
        public static string picpath1;
        string picpath2;
        string picpath3;
        public static byte[] bypic;
        public static byte[] bypic2;
        public static byte[] bypic3;
        




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
            //image4.Source = ImageSource.FromFile(picpath1);
            //byte[] img = GetBytes(beachImage);


            if (bypic != null)
            {
                multiPartContent.Add(new StreamContent(new MemoryStream(bypic)), "file", "reportpicture1.jpg");
            }
            if (bypic2 != null)
            {
                multiPartContent.Add(new StreamContent(new MemoryStream(bypic2)), "file2", "reportpicture2.jpg");
            }
            if (bypic3 != null)
            {
                multiPartContent.Add(new StreamContent(new MemoryStream(bypic3)), "file3", "reportpicture3.jpg");
            }

            HttpResponseMessage picresponse = await client.PostAsync(picuri, multiPartContent);
            //multiPartContent.Add(byteArrayContent, "file", "image" + uniqueId + ".jpg");
            Globals g = Globals.getInstance();
            var mail = g.getData();

            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("machineNumber", machineNumber.Text));
            postData.Add(new KeyValuePair<string, string>("machineType", selectedValueType));
            postData.Add(new KeyValuePair<string, string>("sortOfProblem", selectedValueProblem));
            postData.Add(new KeyValuePair<string, string>("problemBeschrieb", problembeschreibung.Text));
            postData.Add(new KeyValuePair<string, string>("eMail", mail));

            var content = new System.Net.Http.FormUrlEncodedContent(postData);

            //var content = new System.Net.Http.MultipartFormDataContent(postData);
            var response = await client.PostAsync(uri, content);


            var answer = await response.Content.ReadAsStringAsync();
            if (answer == "Message has been sent")
            {
                await DisplayAlert("Erfolgreich", "E-Mail erfolgreich versendet.", "OK");
                await Navigation.PopAsync();
            }
            else if (answer == "Message could not be sent.")
            {
                //E-Mail could not be sent
                DisplayAlert("Fehler", "E-Mail konnte nicht versendet werden.", "OK");
                return;
            }
            else
            {
                //no connection to the server
                Debug.WriteLine(answer);
                DisplayAlert("Fehler", "Keine Verbindung zum Server", "OK");
                return;
            }

            //await Navigation.PopAsync();
            //content.Add(new StreamImageSource(Image));
            //var content = new System.Net.Http.FormUrlEncodedContent(postData);
            // var content = new System.Net.Http.MultipartFormDataContent(postData);
            //var response = await client.PostAsync(uri, content);

        }


        async void OnActionPictureOption(object sender, EventArgs e, int picN)
        {
            var action = await DisplayActionSheet("Was möchten Sie tun? ", "Cancel", null, "Bild löschen", "Bild vergrössern");
            if (action == "Cancel")
            {
                return;
            }
            if (action == "Bild vergrössern")
            {
                ZoomPic(sender, e);
            }
            if (action == "Bild löschen")
            {

                //BugReporting Bug =  cameratest.BugReporting;
                //Debug.WriteLine(picN);
                if (picN == 1)
                {
                    //  Debug.WriteLine("ich bin pic 1");
                    image.Source = null;      
                    bypic = null;
                    numPic -= 1;
                    Picnum = 1;

                }
                if (picN == 2)
                {
                    //  Debug.WriteLine("ich bin pic 2");
                    image2.Source = null;
                    bypic2 = null;
                    numPic -= 1;
                    Picnum = 2;
                }
                if (picN == 3)
                {
                    //Debug.WriteLine("ich bin pic 3");
                    image3.Source = null;
                    bypic3 = null;
                    numPic -= 1;
                    Picnum = 3;
                }
            }
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
            if (Picnum == 1 && numPic < 3)
            {
                if (image.Source != null)
                {
                    Picnum = 2;
                }
                else
                {
                    numPic += 1;
                    Picnum = 2;
                    picpath1 = file.Path;
                    Debug.WriteLine(picpath1);
                    

                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += (s, x) =>
                    {

                        //DisplayAlert("OMFG", "ROFL", "OK");
                        //image.Scale = 4;
                        //DisplayAlert("Bildoptionen", "", "OK");
                        //bild = 1;
                        OnActionPictureOption(sender, e, 1);
                        zoompicpath = picpath1;
                        bild = 1;
                        //ZoomPic(sender, e); // gross Bildfunktion

                    };
                    image.GestureRecognizers.Add(tapGestureRecognizer);

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
                }
                //await DisplayAlert("File Location", picpath1, "ok");
            }
            else if (Picnum == 2 && numPic < 3)
            {
                if (image2.Source != null)
                {
                    Picnum = 3;
                }
                else
                {
                    numPic += 1;
                    Picnum = 3;
                    picpath2 = file.Path;
                    

                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += (s, x) =>
                    {

                        //DisplayAlert("OMFG", "ROFL", "OK");
                        //image.Scale = 4;
                        //DisplayAlert("Bildoptionen", "", "OK");
                        OnActionPictureOption(sender, e, 2);
                        zoompicpath = picpath2;
                        bild = 2;
                        //ZoomPic(sender, e);

                    };
                    image2.GestureRecognizers.Add(tapGestureRecognizer);

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
            }
            else if (Picnum == 3 && numPic < 3)
            {
                
                if (image3.Source != null)
                {
                    Picnum = 1;
                }
                else
                {
                    numPic += 1;
                    Picnum = 1;
                    picpath3 = file.Path;

                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += (s, x) =>
                    {

                        //DisplayAlert("OMFG", "ROFL", "OK");
                        //image.Scale = 4;
                        //DisplayAlert("Bildoptionen", "", "OK");
                        OnActionPictureOption(sender, e, 3);
                        zoompicpath = picpath3;
                        bild = 3;
                        //ZoomPic(sender, e);

                    };
                    image3.GestureRecognizers.Add(tapGestureRecognizer);

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

                  if (Picnum == 1 && numPic < 3) {

                    if (image.Source != null)
                    {
                        Picnum = 2;
                    }
                    else
                    {
                        numPic += 1;
                        Picnum = 2;
                        picpath1 = file.Path;

                        var tapGestureRecognizer = new TapGestureRecognizer();
                        tapGestureRecognizer.Tapped += (s, x) =>
                        {

                            //DisplayAlert("OMFG", "ROFL", "OK");
                            //image.Scale = 4;
                            //DisplayAlert("Bildoptionen", "", "OK");
                            OnActionPictureOption(sender, e, 1);
                            zoompicpath = picpath1;
                            bild = 1;
                            //ZoomPic(sender, e);

                        };
                        image.GestureRecognizers.Add(tapGestureRecognizer);

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
                    }
                 
                } else if (Picnum == 2 && numPic < 3)
                {
                    if (image2.Source != null)
                    {
                        Picnum = 3;
                    }
                    else
                    {
                        numPic += 1;
                        Picnum = 3;
                        picpath2 = file.Path;

                        var tapGestureRecognizer = new TapGestureRecognizer();
                        tapGestureRecognizer.Tapped += (s, x) =>
                        {

                            //DisplayAlert("OMFG", "ROFL", "OK");
                            //image.Scale = 4;
                            //DisplayAlert("Bildoptionen", "", "OK");
                            OnActionPictureOption(sender, e, 2);
                            zoompicpath = picpath2;
                            bild = 2;
                            //ZoomPic(sender, e);

                        };
                        image2.GestureRecognizers.Add(tapGestureRecognizer);

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
                    
                } else if (Picnum == 3 && numPic < 3)
                {
                    if (image3.Source != null)
                    {
                        Picnum = 1;
                    }
                    else
                    {
                        numPic += 1;
                        Picnum = 1;
                        picpath3 = file.Path;
                        

                        var tapGestureRecognizer = new TapGestureRecognizer();
                        tapGestureRecognizer.Tapped += (s, x) =>
                        {

                            //DisplayAlert("OMFG", "ROFL", "OK");
                            //image.Scale = 4;
                            //DisplayAlert("Bildoptionen", "", "OK");
                            OnActionPictureOption(sender, e, 3);
                            zoompicpath = picpath3;
                            bild = 3;
                            //ZoomPic(sender, e);

                        };
                        image3.GestureRecognizers.Add(tapGestureRecognizer);

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
        }

        async void openSettings(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Settings_loggedIn());
        }

        async void openInfoPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new infoPage());
        }

        async void ZoomPic(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ZoomPic());
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
