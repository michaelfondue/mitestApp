﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace cameratest
{
    public partial class Settings_loggedIn : ContentPage
    {
        public Settings_loggedIn()
        {
            InitializeComponent();
        }

        async void wantToChangePassword(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChangePassword());

        }
        async void openSettings(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Settings());
        }
        async void openInfoPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new infoPage());
        }
        async void loggingOut(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
            //TODO: Nach dem Ausloggen sollte der User nirgends zurück können!!!
        }
    }
}
