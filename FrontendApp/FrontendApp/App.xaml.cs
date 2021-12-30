using FrontendApp.ViewModels;
using FrontendApp.Views;
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

            MainPage = new NavigationPage( new SigninPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
