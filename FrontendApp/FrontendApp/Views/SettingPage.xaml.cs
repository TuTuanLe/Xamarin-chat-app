using FrontendApp.Helpers;
using FrontendApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FrontendApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingPage : ContentPage
    {
        public SettingPage()
        {
            
            InitializeComponent();
            imagebtnURL.Source = config.userModel.ImgURL;
            txtUser.Text = config.userModel.FullName;
        }

        private async void Logout_Clicked(object sender, EventArgs e)
        {
            SecureStorage.Remove("storageUser");
            await Navigation.PushAsync(new SigninPage());
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<INotification>().CreateNotification("TUTUANLE", "Hello :v");

        }
    }
}