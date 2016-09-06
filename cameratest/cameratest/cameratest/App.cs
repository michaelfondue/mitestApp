using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace cameratest
{
    public class App : Application
    {
        public static List<string> PhoneNumbers { get; set; }

        public App()
        {
            MainPage = new NavigationPage(new cameratest.MainPage());
            PhoneNumbers = new List<String>();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}