using FrontendApp.Helpers;
using FrontendApp.Models;
using FrontendApp.Services.Interfaces;
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
    public partial class ChatPage : ContentPage
    {

        public ChatPage()
        {
            BindingContext = config.homeViewModel;
            InitializeComponent();
            imageURL.Source = config.userModel.ImgURL;
            NameUser.Text = config.userModel.FullName;
        }

        private async void TappedSearch_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SearchPage());
        }

        private async void btnAddGroup_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateGroupPage());
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            var layout = (BindableObject)sender;
            var item = (FriendModel)layout.BindingContext;

        }

        private async void AcceptFriend_Clicked(object sender, EventArgs e)
        {
            var layout = (BindableObject)sender;
            var item = (FriendModel)layout.BindingContext;
            config.homeViewModel.AcceptFriend(item);
            DependencyService.Get<INotification>().CreateNotification(config.userModel.FullName, "💙 Accept friend 💙 ");
            await config.homeViewModel.UpdateFriend(config.userModel.UserId);
        }

        private async void tappedItemFriend_Tapped(object sender, EventArgs e)
        {
            var layout = (BindableObject)sender;
            var item = (FriendModel)layout.BindingContext;
            if(item.AcceptFriend == true)
            {
                config.friendModel = item;
                await Navigation.PushAsync(new MainPage(item));
            }
            else
            {
                DependencyService.Get<INotification>().CreateNotification(config.userModel.FullName, $"You are accepted friend to {item.Name}");
            }
        }
    }
}