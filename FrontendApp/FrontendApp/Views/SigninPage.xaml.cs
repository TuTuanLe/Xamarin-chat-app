using FrontendApp.Helpers;
using FrontendApp.Models;
using FrontendApp.Services.Interfaces;
using FrontendApp.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
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

            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync($"{config.UrlWebsite}/api/user/{email.Text}&{password.Text}");
            var User = JsonConvert.DeserializeObject<UserModel>(response);

      

            if(User == null)
            {

                DependencyService.Get<INotification>().CreateNotification("Notification", $" Username or password fail !!! ");

            }
            else {
                await Xamarin.Essentials.SecureStorage.SetAsync("storageUser", JsonConvert.SerializeObject(User));

                config.userModel = new UserModel();
                config.userModel = User;
                await Navigation.PushAsync(new TabbedMessaagePage());
            }
            activityIndicator.IsRunning = false;
        }

        private void tapRegister_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegisterPage());
        }

        private async void ForgetPassword_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new  MyMobileNumberPage());
        }
    }
}