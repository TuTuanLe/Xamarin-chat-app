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
                int second = 0;

                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    second++;

                    if (seconds.ToString().Length == 1)
                    {
                        entryChat.Text = "recording 0:" + second.ToString();
                    }
                    else
                    {
                        entryChat.Text = "recording 0:" + second.ToString();
                    }
                    return checkedTimeSpan;
                });

            }
            else
            {
                recordAudio.Source = ImageSource.FromFile("icons8microphone90.png");
                FrameRecord.BackgroundColor = Color.FromHex("#F2F3F4");
                checkedTimeSpan = false;
                checkedTwoClick = 1;
                gifAnimation.IsVisible = false;
                entryChat.HorizontalTextAlignment = TextAlignment.Start;
            }

       
        }
        public readonly AudioPlayer audioPlayer = new AudioPlayer();


      

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


        private AudioPlayer player;
        private Image playImage;
        private bool finishedPlay = true;
        private int seconds = 0;

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var layout = (BindableObject)sender;
            var item = (MessageModel)layout.BindingContext;
            var image = playImage = (Image)sender;
            var messageold = item.Message;
            if (finishedPlay)
            {
                finishedPlay = false;
                player = new AudioPlayer();
                player.FinishedPlaying += Player_FinishedPlaying;

                image.Source = "icons8pause90.png";
                await PlayAudio(item.AttachFilesAudio);

                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    
                    if(seconds != 0)
                        if (seconds.ToString().Length == 1)
                        {
                            item.Message = "recording 0:" + seconds.ToString();
                        }
                        else
                        {
                            item.Message = "recording 0:" + seconds.ToString();
                        }
                    seconds++;

                    return !finishedPlay;
                });
            }
            else
            {
                playImage.Source = "icons8play100.png";
                finishedPlay = true;
                player.Pause();
            }
        }

        private void Player_FinishedPlaying(object sender, EventArgs e)
        {
            playImage.Source = "icons8play100.png";
            finishedPlay = true;
            seconds = 0;
        }

        public Task PlayAudio(string audioPath)
        {
            try
            {
                player.Play(audioPath);
               
            }
            catch (Exception)
            {
            }
            return Task.CompletedTask;
        }

     
    }
}
