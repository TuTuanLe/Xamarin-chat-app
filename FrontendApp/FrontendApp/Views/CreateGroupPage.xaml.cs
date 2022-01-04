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
    public partial class CreateGroupPage : ContentPage
    {
        public CreateGroupPage()
        {
            BindingContext = new CreateGroupViewModel(config.homeViewModel.Friends, Navigation);

            InitializeComponent();
        }

        private void BackChatPage_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PopAsync();
        }

    }
}