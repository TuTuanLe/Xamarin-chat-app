using FrontendApp.Helpers;
using FrontendApp.Models;
using FrontendApp.ViewModels;
using FrontendApp.Views;
using Newtonsoft.Json;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FrontendApp
{
    public partial class App : Application
    {
        

        public App()
        {
            InitializeComponent();
           
           
        }

        protected async override void OnStart()
        {
            string jsonUserModel = await Xamarin.Essentials.SecureStorage.GetAsync("storageUser");
            if(jsonUserModel != null)
            {
                config.userModel = JsonConvert.DeserializeObject<UserModel>(jsonUserModel);
                MainPage = new NavigationPage(new TabbedMessaagePage());
            }
            else
            {
                MainPage = new NavigationPage(new SigninPage());
            } 
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
