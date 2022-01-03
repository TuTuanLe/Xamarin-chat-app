using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace FrontendApp.Models
{
    public class MessageModel: INotifyPropertyChanged
    {
        public int messsageId { get; set; }
        public int fromUserId { get; set; }
        public int toUserId { get; set; }
        public string NameUser { get; set; }
        public string ToNameUser { get; set; }

        public string _Message;
        public string Message {
            get {
                return _Message;
            }
            set
            {
                _Message = value;
                OnPropertyChanged();
            }
             }
        public bool IsOwnMessage { get; set; }
        public bool IsSystemMessage { get; set; }
        public DateTime DateSent { get; set; }
        public DateTime? DateRead { get; set; }
        public string Content { get; set; }
        public string AttachedFiles { get; set; }
        public string ImgURLFromUser { get; set; }
        public string ImgURLToUser { get; set; }
        public ImageSource image { get; set; }
        public bool checkAttachFile { get; set; }
        public bool checkAudioFile { get; set; }
        public string AttachFilesAudio { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
