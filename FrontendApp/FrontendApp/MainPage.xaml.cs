using FrontendApp.Helpers;
using FrontendApp.Models;
using FrontendApp.ViewModels;
using FrontendApp.Views;
using Plugin.AudioRecorder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FrontendApp
{
    public partial class MainPage : ContentPage
    {
        private bool checkedTimeSpan = true;
        private int checkedTwoClick = 1;

        public MainPage(FriendModel friend)
        {
            config.mainViewModel = new MainViewModel(friend);
            this.BindingContext = config.mainViewModel;
            InitializeComponent();
            Subscribe();
        }


        private void Subscribe()
        {
            MessagingCenter.Subscribe<MainViewModel>(this, "ScrollToEnd", (sender) =>
            {
                ScrollToEnd();
            });
        }

        private async void sendMessage_Clicked(object sender, EventArgs e)
        {
            entryChat.Text = "";
            
            await config.homeViewModel.UpdateFriend(config.UserName);
            var v = lstMessage.ItemsSource.Cast<object>().LastOrDefault();
          
        }


        private void ScrollToEnd(bool animated = true)
        {
            var v = lstMessage.ItemsSource.Cast<object>().LastOrDefault();
            lstMessage.ScrollTo(v, ScrollToPosition.End, animated);
        }

        private async void backToHome_Clicked(object sender, EventArgs e)
        {
            await config.homeViewModel.UpdateFriend(config.UserName);
            await Navigation.PopAsync();
        }

        private  void recordAudio_Clicked(object sender, EventArgs e)
        {
            if (checkedTwoClick == 1)
            {
                recordAudio.Source = ImageSource.FromFile("icons8start64.png");
                checkedTimeSpan = true;
                checkedTwoClick++;
                FrameRecord.BackgroundColor = Color.FromHex("#1C76D2");
                gifAnimation.IsVisible = true;
                entryChat.HorizontalTextAlignment = TextAlignment.Center;
                int seconds = 0;

                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    seconds++;

                    if (seconds.ToString().Length == 1)
                    {
                        entryChat.Text = "recording 0:" + seconds.ToString();
                    }
                    else
                    {
                        entryChat.Text = "recording 0:" + seconds.ToString();
                    }
                    return checkedTimeSpan;
                });

            }
            else
            {
                recordAudio.Source = ImageSource.FromFile("icons8microphone90.png");
                FrameRecord.BackgroundColor = Color.White;
                checkedTimeSpan = false;
                checkedTwoClick = 1;
                gifAnimation.IsVisible = false;
                entryChat.HorizontalTextAlignment = TextAlignment.Start;
            }

       
        }
        public readonly AudioPlayer audioPlayer = new AudioPlayer();


        private void lstMessage_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var message = (MessageModel)lstMessage.SelectedItem;
            if(message.checkAudioFile == true)
            {
                audioPlayer.Play(message.AttachedFiles+".wav");
            }
           

        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.White;

            }
        }

      

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InComeCallPage(config.friendModel));
        }
    }
}
