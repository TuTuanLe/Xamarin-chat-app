using FrontendApp.Helpers;
using FrontendApp.Models;
using FrontendApp.Services.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FrontendApp.ViewModels
{
  

    public class HomeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _gmail;

        private string _password;

        public string Gmail
        {
            get
            {
                return _gmail;
            }
            set
            {
                _gmail = value;
                OnPropertyChanged();
            }
        }

        private bool _isLoading = true;

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Story> _Storys;

        public ObservableCollection<Story> Storys
        {
            get
            {
                return _Storys;
            }
            set
            {
                _Storys = value;
                OnPropertyChanged();
            }
        }

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
      
        public bool _isConnected;

        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }
            set
            {
                _isConnected = value;
                OnPropertyChanged();
            }
        }
        DisplayInfo mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
        private int _PeekAreaInsets ; 

        public int PeekAreaInsets
        {
            get
            {
                return _PeekAreaInsets;
            }
            set
            {
                _PeekAreaInsets = value;
                OnPropertyChanged();
            }
        }

        private HubConnection hubConnection;

        public Command UpdateFriendCommand { get; }
        public HomeViewModel(int userID)
        {
            PeekAreaInsets = (int)mainDisplayInfo.Width;
            Storys = new ObservableCollection<Story>();
            Friends = new ObservableCollection<FriendModel>();
            UpdateFriendCommand = new Command(async async => { await UpdateFriend(userID); });
            TestData();

            IsConnected = false;
            hubConnection = new HubConnectionBuilder()
             .WithUrl($"http://192.168.1.8:5000/ChatHub")
             .Build();

            hubConnection.On<List<FriendModel>>("GetFriend", (getfriend) =>
            {
                Friends.Clear();
                foreach (var ms in getfriend)
                {
                    if (ms.UserId == config.userModel.UserId && ms.AcceptFriend == true)
                    {
                        Friends.Add(new FriendModel()
                        {
                            FriendId = ms.FriendId,
                            UserId = ms.UserId,
                            status = ms.status,
                            Name = ms.Name,
                            FriendKey = ms.FriendKey,
                            ImgURL = ms.ImgURL,
                            CountUnRead = ms.CountUnRead,
                            DateSend = ms.DateSend,
                            IdMessageNew = ms.IdMessageNew,
                            MessageNew = ms.MessageNew,
                            IsSeen = ms.CountUnRead == 0 ? false : true,
                            sortDate = ms.sortDate,
                            ColorSeen = ms.ColorSeen,
                            FontAttribute = ms.CountUnRead != 0 ? "Bold" : "None",
                            IsChecked = false,
                            AcceptFriend = ms.AcceptFriend
                        });


                    }  
                    else if(ms.FriendId == config.userModel.UserId && ms.AcceptFriend == false)
                    {
                        Friends.Add(new FriendModel()
                        {
                            FriendId = ms.FriendId,
                            UserId = ms.UserId,
                            status = ms.status,
                            Name = ms.Name,
                            FriendKey = ms.FriendKey,
                            ImgURL = ms.ImgURL,
                            CountUnRead = ms.CountUnRead,
                            DateSend = ms.DateSend,
                            IdMessageNew = ms.IdMessageNew,
                            MessageNew = ms.MessageNew,
                            IsSeen = ms.CountUnRead == 0 ? false : true,
                            sortDate = ms.sortDate,
                            ColorSeen = ms.ColorSeen,
                            FontAttribute = ms.CountUnRead != 0 ? "Bold" : "None",
                            IsChecked = false,
                            AcceptFriend = ms.AcceptFriend
                        });

                        if(DateTime.Now.ToString("H:mm", CultureInfo.InvariantCulture) == ms.DateSend)
                            DependencyService.Get<INotification>().CreateNotification(ms.Name, $"❤️ Sent request to {config.userModel.FullName} ❤️ ");


                    }
                }

            });

    
            _ = Connect();
        }

        public async Task UpdateFriend(int userID)
        {
            if (config.friendModel != null)
                await hubConnection.InvokeAsync("SeenFriend", config.friendModel);
            await hubConnection.InvokeAsync("GetFriend", userID);
        }

        async Task Connect()
        {
            await hubConnection.StartAsync();
            await hubConnection.InvokeAsync("GetFriend", config.userModel.UserId);
            IsConnected = true;
            IsLoading = false;
        }

        public async void AcceptFriend(FriendModel friendModel)
        {
            var json = JsonConvert.SerializeObject(friendModel);
            HttpContent httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var httpClient = new HttpClient();

            var response = await httpClient.PutAsync("http://192.168.1.8:5000/api/friend/AcceptFriend", httpContent);
        }


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void TestData()
        {
            Storys.Add(new Story()
            {
                id = 1,
                ImgURl = "https://kenh14cdn.com/thumb_w/660/203336854389633024/2021/12/8/d04735780ac715fd325abffee4627f11-16389696872851695463984.jpg",
                Name = "ltt"
            });
            Storys.Add(new Story()
            {
                id = 3,
                ImgURl = "https://thegioidienanh.vn/stores/news_dataimages/anhvu/032020/06/14/5624_01.jpg",
                Name = "ltt"
            });
            Storys.Add(new Story()
            {
                id = 2,
                ImgURl = "https://baotuoitre.net/wp-content/uploads/2019/06/tieu-su-rose-blackpink-1.jpg",
                Name = "ltt"
            });

        }

    }
}
