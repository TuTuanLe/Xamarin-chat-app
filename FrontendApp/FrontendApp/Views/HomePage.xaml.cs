using FrontendApp.Helpers;
using FrontendApp.Models;
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
    public partial class HomePage : ContentPage
    {

        public HomePage()
        {
            config.homeViewModel = new HomeViewModel(config.UserName);
            BindingContext = config.homeViewModel;
            InitializeComponent();
        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {

            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.White;
             
            }
        }

        private async void lstFriends_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var friend = (FriendModel)lstFriends.SelectedItem;
            config.friendModel = friend;
            await Navigation.PushAsync(new MainPage(friend));
        }
    }
}