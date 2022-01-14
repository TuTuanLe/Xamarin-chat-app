using FrontendApp.Helpers;
using FrontendApp.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FrontendApp.ViewModels
{
    public class CreateGroupViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<FriendModel> _Friends;

        public ObservableCollection<FriendModel> Friends
        {
            get
            {
                return _Friends;
            }
            set
            {
                _Friends = value;
                OnPropertyChanged();
            }
        }

        public int _step = 0;
        public int Step
        {
            get
            {
                return _step;
            }
            set
            {
                _step = value;
                OnPropertyChanged();
            }
        }

        private bool _steps = true;
        public bool Steps
        {
            get
            {
                return _steps;
            }
            set
            {
                _steps = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<FriendModel> _FriendsGroup;

        public ObservableCollection<FriendModel> FriendsGroup
        {
            get
            {
                return _FriendsGroup;
            }
            set
            {
                _FriendsGroup = value;
                OnPropertyChanged();
            }
        }

        private string _ImageUrl = "icons8amera90white";
        public string ImageUrl
        {
            get
            {
                return _ImageUrl;
            }
            set
            {
                _ImageUrl = value;
                OnPropertyChanged();
            }
        }
        private string _GroupName ="";
        public string GroupName
        {
            get
            {
                return _GroupName;
            }
            set
            {
                _GroupName = value;
                OnPropertyChanged();
            }
        }


        private HubConnection hubConnection;

        public Command PreviousCommand { get; }
        
        public Command NextCommand { get; }
        
        public Command UploadCommand { get; }
        
        public INavigation Navigation { get; set; }
        
        public CreateGroupViewModel(ObservableCollection<FriendModel> friendModel, INavigation navigation)
        {
            this.Navigation = navigation;
            this.FriendsGroup = new ObservableCollection<FriendModel>();
            this.Friends = new ObservableCollection<FriendModel>();
            foreach (var f in friendModel)
            {
                if (f.FriendId != 0)
                {
                    Friends.Add(f);
                }
            }
            NextCommand = new Command(async async => { await NextSteps();});
            PreviousCommand = new Command(async async  => { await CountDownStep(); } );
            UploadCommand = new Command(async async => { await UploadImage(); });


            hubConnection = new HubConnectionBuilder()
           .WithUrl($"http://192.168.1.8:5000/ChatHub")
           .Build();
        }

        public async Task UploadImage()
        {
            var pickImage = await FilePicker.PickAsync(new PickOptions()
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Pick an Image"
            });

            if (pickImage != null)
            {
                var stream = await pickImage.OpenReadAsync();
                var image = ImageSource.FromStream(() => stream).ToString();
            }

        }

        public async Task CountDownStep()
        {
            Step--;
            if (Step == 0)
                Steps = true;
            else
                Steps = false;
            if(Step == -1)
            {
                await Navigation.PopAsync();
            }
        }

        public async Task NextSteps()
        {
            FriendsGroup.Clear();
            foreach (var f in Friends)
            {
                if (f.IsChecked == true)
                {
                    FriendsGroup.Add(f);
                }
            }
            Step++;
            if (Step == 1)
                Steps = false;
            else
                Steps = true;
            if(Step == 2)
            {
                await hubConnection.StartAsync();
                await hubConnection.InvokeAsync("AddGroupFriend",config.userModel.UserId ,ImageUrl ,GroupName ,FriendsGroup);
                await config.homeViewModel.UpdateFriend(config.userModel.UserId);
                await hubConnection.StopAsync();
                await Navigation.PopAsync();
            }

            
        }


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
