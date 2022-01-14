using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace FrontendApp.ViewModels
{
    public class InComeCallViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        private bool _isAudioActive;

        public bool IsAudioActive
        {
            get
            {
                return _isAudioActive;
            }
            set
            {
                _isAudioActive = value;
                OnPropertyChanged(); 
            }
        }

        private bool _isVideoActive;
        public bool IsVideoActive
        {
            get
            {
                return _isVideoActive;
            }
            set
            {
                _isVideoActive = value;
                OnPropertyChanged();
            }
        }

        public InComeCallViewModel()
        {
            IsAudioActive = true;
            IsVideoActive = true;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
