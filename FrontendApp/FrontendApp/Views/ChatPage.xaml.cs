using FrontendApp.Helpers;
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
        }

        private async void TappedSearch_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SearchPage());
        }

        private async void btnAddGroup_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateGroupPage());
        }
    }
}