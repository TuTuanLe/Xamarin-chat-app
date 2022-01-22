using FrontendApp.Helpers;
using FrontendApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FrontendApp.ViewModels
{
    public class SearchViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<UserModel> _users;

        public ObservableCollection<UserModel> Users
        {
            get
            {
                return _users;
            }
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        private UserModel _user;
        public UserModel User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }

        public Command AddFriendCommand { get; set; }

        private string _setTextBtn = "Add Friend";
        public string SetTextBtn
        {
            get
            {
                return _setTextBtn;
            }
            set
            {
                _setTextBtn = value;
                OnPropertyChanged();
            }
        }

        private string _setTextColorBtn ;
        public string SetTextColorBtn
        {
            get
            {
                return _setTextColorBtn;
            }
            set
            {
                _setTextColorBtn = value;
                OnPropertyChanged();
            }
        }


        public SearchViewModel()
        {
            User = new UserModel();
            _ = CallApiGetAllUser();
        }

        public void GetModel(UserModel userModel)
        {
            this.User = userModel;
            if(userModel.status == null)
            {
                SetTextBtn = "Add Friend";
                SetTextColorBtn = "Gray";
            }
            else if(userModel.status == 1)
            {
                SetTextBtn = "UnFriend";
                SetTextColorBtn = "Red";
            }
            else if(userModel.status == 0)
            {
                SetTextBtn = "Sent Friend Request";
                SetTextColorBtn = "Green";
            }
            else if(userModel.status  == 2)
            {
                SetTextBtn = "Accept Friend ";
                SetTextColorBtn = "Green";
            }
        }

        


        public async Task CallApiGetAllUser()
        {
            HttpClient client = new HttpClient();
            int userId = config.userModel.UserId;
            var res =await client.GetStringAsync($"{config.UrlWebsite}/api/friend/allFriends/{userId}");
            Users = new ObservableCollection<UserModel> (JsonConvert.DeserializeObject<List<UserModel>>(res));
            foreach(var user in Users)
            {
                if(user.status == 0 &&  user.UserIdfriend == config.userModel.UserId)
                {
                    user.status = 2;
                }
            }
        }

        public async Task SentFriendRequest()
        {
            var msModel = new MessageModel()
            {
                fromUserId = config.userModel.UserId,
                toUserId = User.UserId,

            };
            var json = JsonConvert.SerializeObject(msModel);
            HttpContent httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var httpClient = new HttpClient();

            var response = await httpClient.PostAsync($"{config.UrlWebsite}/api/friend/RequestFriend", httpContent);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
