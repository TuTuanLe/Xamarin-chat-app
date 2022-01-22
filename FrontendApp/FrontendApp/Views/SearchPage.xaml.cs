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
    public partial class SearchPage : ContentPage
    {
        private SearchViewModel SearchView;
        public SearchPage()
        {
            SearchView = new SearchViewModel();
            BindingContext = SearchView;
            InitializeComponent();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var container = BindingContext as SearchViewModel;
            ListUser.BeginRefresh();
            if (String.IsNullOrWhiteSpace(e.NewTextValue))
            {
                ListUser.ItemsSource = container.Users;
            }
            else
            {
                ListUser.ItemsSource = container.Users.Where(c => c.FullName.ToLower().Contains(e.NewTextValue.ToLower()));
                
            }
            ListUser.EndRefresh();
        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.White;

            }
            var layout = (BindableObject)sender;
            var item = (UserModel)layout.BindingContext;
            SearchView.GetModel(item);
            popup.IsVisible = true;
            Action<double> callback = input => popupAnimation.TranslationY = input;
            popupAnimation.Animate("anim", callback, -550, 0, 16, 300, Easing.CubicInOut);

        }

        private void closePopup_Clicked(object sender, EventArgs e)
        {
            popup.IsVisible = false;
        }

        private async void FC_btn_Clicked(object sender, EventArgs e)
        {
            if(SearchView.SetTextBtn == "Add Friend")
            {
                SearchView.SetTextBtn = "Sent Friend Request";
                await SearchView.SentFriendRequest();
                await config.homeViewModel.UpdateFriend(config.userModel.UserId);
            }
        }

        private void imgButtonBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}