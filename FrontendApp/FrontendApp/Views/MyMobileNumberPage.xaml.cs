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
    public partial class MyMobileNumberPage : ContentPage
    {
        public MyMobileNumberPage()
        {
            InitializeComponent();
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync($"http://192.168.1.8:5000/api/user/getOTP/{numberphone.Text}");
            await Navigation.PushAsync(new GetOTPPage(numberphone.Text));
        }
    }
}