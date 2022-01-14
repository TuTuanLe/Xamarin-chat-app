using FrontendApp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace FrontendApp.ViewModels
{
    class ProfileViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string _FullName = config.userModel.FullName;
        public string _Passwordd = config.userModel.Passwordd;
        public DateTime? _BirthDate = config.userModel.BirthDate;
        public string _Address1 = config.userModel.Address1;
        public string _Address2 = config.userModel.Address2;
        public string _Phone = config.userModel.Phone;
        public string _ImgURL = config.userModel.ImgURL;

        public string FullName { get { return _FullName; } set { _FullName = value; OnPropertyChanged(); } }
        public string Passwordd { get { return _Passwordd; } set { _Passwordd = value; OnPropertyChanged(); } }
        public DateTime? BirthDate { get { return _BirthDate; } set { _BirthDate = value; OnPropertyChanged(); } }
        public string Address1 { get { return _Address1; } set { _Address1 = value; OnPropertyChanged(); } }
        public string Address2 { get { return _Address2; } set { _Address2 = value; OnPropertyChanged(); } }
        public string Phone { get { return _Phone; } set { _Phone = value; OnPropertyChanged(); } }
        public string ImgURL { get { return _ImgURL; } set { _ImgURL = value; OnPropertyChanged(); } }


        public ProfileViewModel()
        {
            
        }



        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
