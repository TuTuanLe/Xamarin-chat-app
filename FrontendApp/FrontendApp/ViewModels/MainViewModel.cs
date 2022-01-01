using FrontendApp.Helpers;
using FrontendApp.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Plugin.AudioRecorder;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FrontendApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _name;
        private string _message;
        private ObservableCollection<MessageModel> _messages;
        private bool _isConnected;

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


        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }


        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MessageModel> Messages
        {
            get
            {
                return _messages;
            }
            set
            {
                _messages = value;
                OnPropertyChanged();
            }
        }
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

        public FriendModel _FriendModelS;

        public FriendModel FriendModelS
        {
            get
            {
                return _FriendModelS;
            }
            set
            {
                _FriendModelS = value;
                OnPropertyChanged();
            }
        }

        public bool _status;
        public bool status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }
        public string _AudioRecordFileUrl;

        public string AudioRecordFileUrl
        {
            get
            {
                return _AudioRecordFileUrl;
            }
            set
            {
                _AudioRecordFileUrl = value;
                OnPropertyChanged();
            }
        }

        private HubConnection hubConnection;

        public Command SendMessageCommand { get; }
        public Command ConnectCommand { get; }
        public Command DisconnectCommand { get; }
        public Command SigninCommand { get; }
        public Command PickImageCommand { get; }
        public Command AudioRecordComand { get; }

        public readonly AudioRecorderService audioRecorderService = new AudioRecorderService();
        public readonly AudioPlayer audioPlayer = new AudioPlayer();

        public MainViewModel(FriendModel friend)
        {
            this.FriendModelS = friend;
            status = friend.status;

            Messages = new ObservableCollection<MessageModel>();
            SendMessageCommand = new Command(async () => { await SendMessage(friend.UserId, Message, friend.FriendKey); });
            ConnectCommand = new Command(async () => await Connect());
            DisconnectCommand = new Command(async () => await Disconnect());
            SigninCommand = new Command(async () => await Singin());
            PickImageCommand = new Command(async () => await PickImage());
            AudioRecordComand = new Command(async () => await AudioRecord());
            IsConnected = false;

            hubConnection = new HubConnectionBuilder()
             .WithUrl($"http://192.168.1.8:5000/ChatHub")
             .Build();

            hubConnection.On<List<MessageModel>>("ReceiveOldMessage", (chatmessages) =>
            {
                Messages.Clear();
                foreach (var ms in chatmessages)
                {
                    Messages.Add(new MessageModel()
                    {
                        messsageId = ms.messsageId,
                        fromUserId = (int)ms.fromUserId,
                        toUserId = (int)ms.toUserId,
                        NameUser = ms.NameUser,
                        ToNameUser = ms.ToNameUser,
                        Message = ms.Message,
                        IsOwnMessage = ms.fromUserId == friend.UserId ? true : false,
                        IsSystemMessage = ms.IsSystemMessage,
                        DateSent = ms.DateSent,
                        DateRead = ms.DateRead,
                        AttachedFiles = ms.AttachedFiles,
                        ImgURLFromUser = ms.ImgURLFromUser,
                        ImgURLToUser = ms.ImgURLToUser,
                        checkAttachFile = String.IsNullOrEmpty(ms.AttachedFiles) ? false: (ms.AttachedFiles.Contains("tutuanle/image/upload") ? true: false),
                        checkAudioFile = String.IsNullOrEmpty(ms.AttachedFiles) ? false : (ms.AttachedFiles.Contains("tutuanle/record/upload") ? true : false),
                        AttachFilesAudio = ms.AttachedFiles
                    }); ;
                }
                MessagingCenter.Send<MainViewModel>(this, "ScrollToEnd");
            });


            hubConnection.On<string>("JoinChat", (user) =>
            {
                Messages.Add(new MessageModel() { NameUser = Name, Message = $"{user} has joined the chat", IsSystemMessage = true });
            });

            hubConnection.On<string>("LeaveChat", (user) =>
            {
                Messages.Add(new MessageModel() { NameUser = Name, Message = $"{user} has left the chat", IsSystemMessage = true });
            });

            hubConnection.On<MessageModel>("ReceiveMessage", ms =>
            {
                Messages.Add(new MessageModel()
                {
                    messsageId = ms.messsageId,
                    fromUserId = (int)ms.fromUserId,
                    toUserId = (int)ms.toUserId,
                    NameUser = ms.NameUser,
                    ToNameUser = ms.ToNameUser,
                    Message = ms.Message,
                    IsOwnMessage = ms.fromUserId == friend.UserId ? true : false,
                    IsSystemMessage = ms.IsSystemMessage,
                    DateSent = ms.DateSent,
                    DateRead = ms.DateRead,
                    AttachedFiles = ms.AttachedFiles,
                    ImgURLFromUser = ms.ImgURLFromUser,
                    ImgURLToUser = ms.ImgURLToUser,
                    checkAttachFile = String.IsNullOrEmpty(ms.AttachedFiles) ? false : (ms.AttachedFiles.Contains("tutuanle/image/upload") ? true : false),
                    checkAudioFile = String.IsNullOrEmpty(ms.AttachedFiles) ? false : (ms.AttachedFiles.Contains("tutuanle/record/upload") ? true : false),
                    AttachFilesAudio = ms.AttachedFiles
                });
                MessagingCenter.Send<MainViewModel>(this, "ScrollToEnd");
            });


            _ = Connect();
        }

        async Task AudioRecord()
        {


            var status = await Permissions.RequestAsync<Permissions.Microphone>();

            if (status != PermissionStatus.Granted)
                return;

            if (!audioRecorderService.IsRecording)
            {
                await audioRecorderService.StartRecording();
            }
            else
            {
                await audioRecorderService.StopRecording();
                AudioRecordFileUrl = "https://res.cloudinary.com/uit-information/video/upload/v1641027785/tutuanle/record/upload/" + "record" + Messages[Messages.Count -1].messsageId.ToString();
                audioPlayer.Play(audioRecorderService.GetAudioFilePath());
                var content = new MultipartFormDataContent();
                content.Add(new StreamContent(audioRecorderService.GetAudioFileStream()), "file", "record" + Messages[Messages.Count -1].messsageId.ToString());
                var httpClient = new HttpClient();
                var response = await httpClient.PostAsync("http://192.168.1.8:5000/api/message/video", content);
                string json = response.Content.ToString();
            }
        }


        async Task PickImage()
        {
            var pickImage = await FilePicker.PickAsync(new PickOptions()
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Pick an Image"
            });

            if (pickImage != null)
            {
                var stream = await pickImage.OpenReadAsync();
                var image = ImageSource.FromStream(() => stream);
                var content = new MultipartFormDataContent();
                content.Add(new StreamContent(stream), "file", pickImage.FileName);
                var httpClient = new HttpClient();
                var response = await httpClient.PostAsync("http://192.168.1.8:5000/api/message/image", content);
                string urlImage = "https://res.cloudinary.com/uit-information/image/upload/v1640925576/tutuanle/image/upload/" + pickImage.FileName;
                _ = SendMessage(FriendModelS.UserId, "Send attachedFiles Photo.", FriendModelS.FriendKey, urlImage);
            }
        }

        async Task Singin()
        {
            await hubConnection.StartAsync();
            //await hubConnection.InvokeAsync("ReceiveFriend", FriendModelS);
        }

        async Task Connect()
        {
            await hubConnection.StartAsync();
            await hubConnection.InvokeAsync("ReceiveOldMessage", FriendModelS);

            IsConnected = true;
        }

        async Task SendMessage(int userId, string message, string friendKey, string AttachedFiles = null)
        {
            if (!String.IsNullOrEmpty(AudioRecordFileUrl))
            {
                await hubConnection.InvokeAsync("SendMessage", userId, message, friendKey, AudioRecordFileUrl);
                AudioRecordFileUrl = null;
            }
            else
            {
                await hubConnection.InvokeAsync("SendMessage", userId, message, friendKey, AttachedFiles);
                
            }

            MessagingCenter.Send<MainViewModel>(this, "ScrollToEnd");
        }

        async Task Disconnect()
        {
            await hubConnection.InvokeAsync("LeaveChat", Name);
            await hubConnection.StopAsync();

            IsConnected = false;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
