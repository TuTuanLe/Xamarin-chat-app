﻿using FrontendApp.Helpers;
using FrontendApp.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FrontendApp.ViewModels
{
  

    public class HomeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //private string _name;
        //private string _message;
        //private ObservableCollection<FriendModel> _friends;

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
        
       
        private HubConnection hubConnection;

        public Command UpdateFriendCommand { get; }
        public HomeViewModel(string userID)
        {
            Gmail = userID;
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
                    if (ms.UserId == Int32.Parse(config.UserName))
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
                            FontAttribute = ms.CountUnRead != 0 ? "Bold" : "None"

                        }) ;
                }

            });

            //hubConnection.On<FriendModel>("SeenFriend",  friend =>
            //{
            //    var listFriend = new List<FriendModel>(Friends);
            //    var index = listFriend.FindIndex(x => x.FriendId == friend.FriendId);
            //    listFriend.RemoveAt(index);
            //    listFriend.Add(friend);
            //    Friends.Clear();
            //    Friends = new ObservableCollection<FriendModel> (listFriend);
            //    //await hubConnection.InvokeAsync("GetFriend", Int32.Parse(Gmail));
            //});
            _ = Connect();
        }

        public async Task UpdateFriend(string userID)
        {
            await hubConnection.InvokeAsync("SeenFriend", config.friendModel);
            await hubConnection.InvokeAsync("GetFriend", Int32.Parse(userID));

        }

        async Task Connect()
        {
            await hubConnection.StartAsync();
            await hubConnection.InvokeAsync("GetFriend", Int32.Parse(Gmail));
            IsConnected = true;
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
