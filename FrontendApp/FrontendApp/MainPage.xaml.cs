using FrontendApp.Helpers;
using FrontendApp.Models;
using FrontendApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FrontendApp
{
    public partial class MainPage : ContentPage
    {

        public MainPage(FriendModel friend)
        {
            config.mainViewModel = new MainViewModel(friend);
            this.BindingContext = config.mainViewModel;
            InitializeComponent();

            
        }

       

        private async void sendMessage_Clicked(object sender, EventArgs e)
        {
            entryChat.Text = "";
            await config.homeViewModel.UpdateFriend(config.UserName);
        }

        private void lstMessage_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            //if(config.ScrollToEnd == true)
            //{
            //    var v = lstMessage.ItemsSource.Cast<object>().LastOrDefault();
            //    lstMessage.ScrollTo(v, ScrollToPosition.End, true);
            //    config.ScrollToEnd = false;
            //}
            
        }

        private void ScrollToEnd(bool animated = true)
        {
            var v = lstMessage.ItemsSource.Cast<object>().LastOrDefault();
            lstMessage.ScrollTo(v, ScrollToPosition.End, true);
        }

        private async void backToHome_Clicked(object sender, EventArgs e)
        {
            await config.homeViewModel.UpdateFriend(config.UserName);
            await Navigation.PopAsync();
        }
    }
}
