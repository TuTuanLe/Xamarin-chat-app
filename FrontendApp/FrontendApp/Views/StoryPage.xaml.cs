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
    public partial class StoryPage : ContentPage
    {
        public StoryPage()
        {
            InitializeComponent();
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            test.Text = mainDisplayInfo.Width.ToString();
            test1.Text = mainDisplayInfo.Height.ToString();

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