using FrontendApp.Helpers;
using FrontendApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FrontendApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SigninPage : ContentPage
    {
        public SigninPage()
        {
            
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }

        private async void signin_Clicked(object sender, EventArgs e)
        {

            activityIndicator.IsRunning = true;
            config.UserName = email.Text;
            config.password = password.Text;
            await Navigation.PushAsync(new TabbedMessaagePage());
            activityIndicator.IsRunning = false;
        }
    }
}