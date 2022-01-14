using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace FrontendApp.ViewModels
{
    class RegisterViewModel: INotifyPropertyChanged
    {
        public bool _nextRegister = true;

        public bool NextRegister
        {
            get
            {
                return _nextRegister;
            }
            set
            {
                _nextRegister = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

       public RegisterViewModel()
        {

        }


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
