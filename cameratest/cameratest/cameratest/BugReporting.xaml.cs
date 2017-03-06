using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Plugin.Media;
using Xamarin.Forms;
using System.Net;
using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Net.Http;
using Plugin.Media.Abstractions;
using System.IO;


namespace cameratest
{
    public partial class BugReporting : ContentPage
    {
        //Die Klasse ist für Seite der Fehlermeldung zuständig sowie wie den Versand der Dateiene an den Server.

        //Definition der globalen Veriablen
        public static int numPic; //Number of Pictures added
        public static int Picnum = 1; // Number of Picturecontainer
        public static int bild; // Bildwahl für Tapauswahl
        public static string zoompicpath; //Pfad für ZoomedPic
        string picpath1; //Pfad der Bilder
        string picpath2;
        string picpath3;
        public static byte[] bypic; //Bilder in Byte-Array-Form
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
                    DisplayAlert(AppResources.str_error, AppResources.str_tooLargeMachineNumber, "OK");
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
                    DisplayAlert(AppResources.str_error, AppResources.str_tooLargeMachineNumber, "OK");
                    return;
                }
            }
        }

        async void sendingReport(object sender, EventArgs e)
        {
            //Diese Methode wird beim versenden des Fehlerberichts aufgerufen. Es beinhaltet den Versand der Felder und der Bilder
            //mittels HTTP-Requests und speziell Multipartform-Requests für den Versand der Bilder.

            //URIs für die Übertragung der Daten
            Uri uri = new Uri("http://app.tuboly-astronic.ch/app/email.php");
            Uri picuri = new Uri("http://app.tuboly-astronic.ch/app/uploadpic.php");
            var client = new System.Net.Http.HttpClient();


            //Erstellen des Multipartform-Contents
            string boundary = "---8d0f01e6b3b5dafaaadada";
            var multiPartContent = new MultipartFormDataContent(boundary);

            //Überprüfung wieviele Bilder mitgesendet werden.
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



            //Auswahl und Erstellen der Datenfelder für den Versand
            var selectedValueType = machineType.Items[machineType.SelectedIndex];
            var selectedValueProblem = sortOfProblem.Items[sortOfProblem.SelectedIndex];
            Globals g = Globals.getInstance();
            var mail = g.getData();

            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("machineNumber", machineNumber.Text));
            postData.Add(new KeyValuePair<string, string>("machineType", selectedValueType));
            postData.Add(new KeyValuePair<string, string>("sortOfProblem", selectedValueProblem));
            postData.Add(new KeyValuePair<string, string>("problemBeschrieb", problembeschreibung.Text));
            postData.Add(new KeyValuePair<string, string>("eMail", mail));

            var content = new System.Net.Http.FormUrlEncodedContent(postData);
            var response = await client.PostAsync(uri, content);



            //Antwort des Servers zur Überprüfung ob die Übermittlung erfolgreich war.
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
        }


        async void OnActionPictureOption(object sender, EventArgs e, int picN)
        {
            //Diese Methode dient zur Handhabung beim Tappen eines Bildes.
            //Es wird nach Bild löschen und vergrössern unterschieden
            var action = await DisplayActionSheet("Was möchten Sie tun? ", "Cancel", null, "Bild löschen", "Bild vergrössern");
            if (action == "Cancel")
            {
                return;
            }
            //Bei Bild vergrössern wird auf die Funktion für eine neue Page verwiesen.
            if (action == "Bild vergrössern")
            {
                ZoomPic(sender, e);
            }
            if (action == "Bild löschen")
            {
                //Bei Bild löschen wird zusätzlich nach der Bildnummer gefiltert.
                if (picN == 1)
                {
                    image.Source = null;      
                    bypic = null;
                    numPic -= 1;
                    Picnum = 1;

                }
                if (picN == 2)
                {
                    image2.Source = null;
                    bypic2 = null;
                    numPic -= 1;
                    Picnum = 2;
                }
                if (picN == 3)
                {
                    image3.Source = null;
                    bypic3 = null;
                    numPic -= 1;
                    Picnum = 3;
                }
            }
        }

            async void OnActionChoosePhoto(object sender, EventArgs e)
        {
            //Bei der Auswahl für ein Photo wird zwischen Kamera und Galerie unterschieden.
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
            //set Options for picking a picture and scaling
            var pickmediaOptions = new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Custom,
                CustomPhotoSize = 20
            };

            var file = await CrossMedia.Current.PickPhotoAsync(pickmediaOptions);

            //Aufüllen von Gridview bis 3 Bilder eingefügt sind.
            if (file == null)
                return;
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
       
                    //Erstellen der Erkkennung für den Tap auf das Bild
                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += (s, x) =>
                    {
                        OnActionPictureOption(sender, e, 1);
                        zoompicpath = picpath1;
                        bild = 1;
                    };

                    image.GestureRecognizers.Add(tapGestureRecognizer);

                    //Schreiben des Bildes in den Imagestream
                    image.Source = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                    
                    //Umwandeln des Streams in ein Bytearray
                    using (var memoryStream = new MemoryStream())
                        {
                            file.GetStream().CopyTo(memoryStream);
                            bypic = memoryStream.ToArray();
                        }
                                       
                        file.Dispose();
                        return stream;
                    });
                }
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

                    //Erstellen der Erkkennung für den Tap auf das Bild
                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += (s, x) =>
                    {
                        OnActionPictureOption(sender, e, 2);
                        zoompicpath = picpath2;
                        bild = 2;
                    };
                    image2.GestureRecognizers.Add(tapGestureRecognizer);

                    //Schreiben des Bildes in den Imagestream
                    image2.Source = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        //Umwandeln des Streams in ein Bytearray
                        using (var memoryStream = new MemoryStream())
                        {
                            file.GetStream().CopyTo(memoryStream);
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

                    //Erstellen der Erkkennung für den Tap auf das Bild
                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += (s, x) =>
                    {
                        OnActionPictureOption(sender, e, 3);
                        zoompicpath = picpath3;
                        bild = 3;
                    };
                    image3.GestureRecognizers.Add(tapGestureRecognizer);

                    //Schreiben des Bildes in den Imagestream
                    image3.Source = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        //Umwandeln des Streams in ein Bytearray
                        using (var memoryStream = new MemoryStream())
                        {
                            file.GetStream().CopyTo(memoryStream);
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
            //Photo mit der Kamera erstellen
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
                    //Optionen wie das setzen des Speicherpfades und des Speichernamens
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

                        //Erstellen der Erkkennung für den Tap auf das Bild
                        var tapGestureRecognizer = new TapGestureRecognizer();
                        tapGestureRecognizer.Tapped += (s, x) =>
                        {
                            OnActionPictureOption(sender, e, 1);
                            zoompicpath = picpath1;
                            bild = 1;
                        };
                        image.GestureRecognizers.Add(tapGestureRecognizer);

                        //Schreiben des Bildes in den Imagestream
                        image.Source = ImageSource.FromStream(() =>
                        {
                            var stream = file.GetStream();
                            //Umwandeln des Streams in ein Bytearray
                            using (var memoryStream = new MemoryStream())
                            {
                                file.GetStream().CopyTo(memoryStream);
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

                        //Erstellen der Erkkennung für den Tap auf das Bild
                        var tapGestureRecognizer = new TapGestureRecognizer();
                        tapGestureRecognizer.Tapped += (s, x) =>
                        {
                            OnActionPictureOption(sender, e, 2);
                            zoompicpath = picpath2;
                            bild = 2;
                        };
                        image2.GestureRecognizers.Add(tapGestureRecognizer);

                        //Schreiben des Bildes in den Imagestream
                        image2.Source = ImageSource.FromStream(() =>
                        {
                            var stream = file.GetStream();
                            //Umwandeln des Streams in ein Bytearray
                            using (var memoryStream = new MemoryStream())
                            {
                                file.GetStream().CopyTo(memoryStream);
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

                        //Erstellen der Erkkennung für den Tap auf das Bild
                        var tapGestureRecognizer = new TapGestureRecognizer();
                        tapGestureRecognizer.Tapped += (s, x) =>
                        {
                            OnActionPictureOption(sender, e, 3);
                            zoompicpath = picpath3;
                            bild = 3;
                        };
                        image3.GestureRecognizers.Add(tapGestureRecognizer);

                        //Schreiben des Bildes in den Imagestream
                        image3.Source = ImageSource.FromStream(() =>
                        {
                            var stream = file.GetStream();
                            //Umwandeln des Streams in ein Bytearray
                            using (var memoryStream = new MemoryStream())
                            {
                                file.GetStream().CopyTo(memoryStream);
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
    }
}
