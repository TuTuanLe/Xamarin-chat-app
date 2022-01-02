using FrontendApp.Helpers;
using FrontendApp.Models;
using FrontendApp.Services.Interfaces;
using FrontendApp.ViewModels;
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
    public partial class InComeCallPage : ContentPage
    {
        private readonly FriendModel friend;

        private InComeCallViewModel inComeCallViewModel;
        public InComeCallPage(FriendModel friendModel)
        {
            this.friend = friendModel;
            InitializeComponent();
            var urlSource = new UrlWebViewSource();
            string baseUrl = DependencyService.Get<IWebViewService>().GetContent();
            urlSource.Url = baseUrl;
            CallWebView.Source = urlSource;
            inComeCallViewModel = new InComeCallViewModel();
            this.BindingContext = inComeCallViewModel;
            imageFriend.Source = ImageSource.FromFile(friendModel.ImgURL);
            nameFriend.Text = friendModel.Name;


        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(1000);

            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
            {
                var response = await Permissions.RequestAsync<Permissions.Camera>();
                if (response != PermissionStatus.Granted)
                {
                }
            }

            var statusMic = await Permissions.CheckStatusAsync<Permissions.Microphone>();
            if (statusMic != PermissionStatus.Granted)
            {
                var response = await Permissions.RequestAsync<Permissions.Microphone>();
                if (response != PermissionStatus.Granted)
                {
                }
            }
            await Connect();
        }
        private async Task Connect()
        {
            try
            {
                await CallWebView.EvaluateJavaScriptAsync($"init('{Int32.Parse(config.UserName) }');");
                await Task.Delay(2000);
                await CallWebView.EvaluateJavaScriptAsync($"startCall('{friend.FriendId}');");
                CallWebView.IsVisible = true;
                infomation.IsVisible = false;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
        private async void ToggleMicClick(object sender, EventArgs e)
        {
            


            inComeCallViewModel.IsAudioActive = !inComeCallViewModel.IsAudioActive;
            if (inComeCallViewModel.IsAudioActive == false)
            {
                imageMicrophone.Source = ImageSource.FromFile("icons8muteunmute90");
            }
            else
            {
                imageMicrophone.Source = ImageSource.FromFile("icons8microphone90");
            }
            await CallWebView.EvaluateJavaScriptAsync($"toggleAudio('{inComeCallViewModel.IsAudioActive.ToString().ToLower()}');");
        }

        private void EndCallClick(object sender, EventArgs e)
        {
            EndCall();
        }

        private async void ToggelCameraClick(object sender, EventArgs e)
        {
            inComeCallViewModel.IsVideoActive = !inComeCallViewModel.IsVideoActive;
            if (inComeCallViewModel.IsVideoActive == false)
            {
                imageVideoCall.Source = ImageSource.FromFile("icons8novideo90");
            }
            else
            {
                imageVideoCall.Source = ImageSource.FromFile("icons8videocall96");
            }
            await CallWebView.EvaluateJavaScriptAsync($"toggleVideo('{inComeCallViewModel.IsVideoActive.ToString().ToLower()}');");
        }

        private void EndCall()
        {
            CallWebView.Source = "about:blank";
            CallWebView.IsVisible = false;
            popup.IsVisible = true;
            infomation.IsVisible = true;
            function.IsVisible = false;
        }

        private async void btnNotNow_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}