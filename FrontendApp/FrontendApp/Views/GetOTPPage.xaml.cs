using FrontendApp.Helpers;
using FrontendApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FrontendApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GetOTPPage : ContentPage
    {
        public GetOTPPage(string numberphone)
        {
            InitializeComponent();
            textlb.Text = "Check your SMS messages, we have sent you the pin at " + numberphone;
        }

        private async void Loginbtn_Clicked(object sender, EventArgs e)
        {
            activityIndicator.IsRunning = true;

            string CodeOTP = K1.Text + K2.Text + K3.Text + K4.Text;
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync($"http://192.168.1.8:5000/api/user/getUser/{CodeOTP}");
            var User = JsonConvert.DeserializeObject<UserModel>(response);

            if (User == null)
            {
                await DisplayAlert("Alert", "OTP fail", "Ok");
            }
            else
            {
                config.userModel = new UserModel();
                config.userModel = User;
                await Navigation.PushAsync(new TabbedMessaagePage());
            }
            activityIndicator.IsRunning = false;
        }

        private void backbtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}