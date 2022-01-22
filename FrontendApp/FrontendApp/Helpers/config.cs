using FrontendApp.Models;
using FrontendApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FrontendApp.Helpers
{
    public static class config
    {

        public static UserModel userModel { get; set; }

        public static MainViewModel mainViewModel;

        public static HomeViewModel homeViewModel;

        public static FriendModel friendModel;

        public static bool ScrollToEnd;

        public static string UrlWebsite = "http://www.messagingchat.somee.com";
    }
}
