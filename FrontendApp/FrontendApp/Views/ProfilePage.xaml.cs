using FrontendApp.Services.Interfaces;
using FrontendApp.ViewModels;
using Plugin.LocalNotification;
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
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            BindingContext = new ProfileViewModel();
            InitializeComponent();
        }

     

        private void Button_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<INotification>().CreateNotification("Notification", "Update success");
            //var notification = new NotificationRequest
            //{
            //    BadgeNumber = 1,
            //    Description = "test description",
            //    Title = "Notification",
            //    ReturningData = "Dumy data",
            //    NotificationId = 1337,

            //};
            //NotificationCenter.Current.Show(notification);
        }
    }
}