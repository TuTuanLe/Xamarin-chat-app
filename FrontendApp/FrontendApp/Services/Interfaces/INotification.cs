using System;
using System.Collections.Generic;
using System.Text;

namespace FrontendApp.Services.Interfaces
{
    public interface INotification
    {
        void CreateNotification(String title, String message);
    }
}
