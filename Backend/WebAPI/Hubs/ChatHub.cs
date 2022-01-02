using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using WebAPI.Models;

namespace WebAPI.Hubs
{
    public class ChatHub : Hub
    {
        private readonly MessengeChatTestContext messengeChat;
        public ChatHub(MessengeChatTestContext messenge)
        {
            this.messengeChat = messenge;
        }
        public async Task JoinChat(string user)
        {
            await Clients.All.SendAsync("JoinChat", user);
        }

        public async Task LeaveChat(string user)
        {
            await Clients.All.SendAsync("LeaveChat", user);
        }

        public async Task SendMessage(int userId, string message, string friendKey, string AttachedFiles = null)
        {

            var friend =  messengeChat.Friends.Where(c => c.FriendKey == friendKey && c.UserIdfriend != userId).SingleOrDefault();
            Messaging ms = new Messaging()
            {
                FromUserId = userId,
                DateSent = DateTime.Now,
                DateRead = null,
                Content = message,
                ToUserId = friend.UserIdfriend,
                FriendId = friend.FriendKey,
                AttachedFiles = AttachedFiles
            };
            messengeChat.Add(ms);
            messengeChat.SaveChanges();


            MessageModel messageModel = new MessageModel()
            {
                messsageId = messengeChat.Messagings.Count(),
                fromUserId = userId,
                toUserId = (int)ms.ToUserId,
                NameUser = (from u in messengeChat.Users
                            where u.UserId == userId
                            select u.FirstName + " " + u.LastName).SingleOrDefault(),
                ToNameUser = (from u in messengeChat.Users
                              where u.UserId == ms.ToUserId
                              select u.FirstName + " " + u.LastName).SingleOrDefault(),
                Message = ms.Content,
                IsOwnMessage = ms.FromUserId == friend.UserId ? true : false,
                IsSystemMessage = false,
                DateSent = ms.DateSent,
                DateRead = ms.DateRead,
                AttachedFiles = ms.AttachedFiles,
                ImgURLFromUser = (from u in messengeChat.Users
                                  where u.UserId == ms.FromUserId
                                  select u.ImgURL).SingleOrDefault(),

                ImgURLToUser = (from u in messengeChat.Users
                                where u.UserId == ms.ToUserId
                                select u.ImgURL).SingleOrDefault(),
            };
            
            await Clients.Group(friendKey).SendAsync("ReceiveMessage", messageModel);
           
        }

        public async Task ReceiveOldMessage(FriendModel friend )
        {
            var test = (from m in messengeChat.Messagings
             
                        where m.FriendId == friend.FriendKey
                       select m).ToList();
            List<MessageModel> msModel = new List<MessageModel>();
            foreach(var ms in test)
            {
                msModel.Add(new MessageModel()
                {
                    messsageId = messengeChat.Messagings.Count(),
                    fromUserId = (int)ms.FromUserId,
                    toUserId = (int)ms.ToUserId,
                    NameUser= (from u in messengeChat.Users
                              where u.UserId == ms.FromUserId
                              select u.FirstName +" " + u.LastName ).SingleOrDefault(),
                    ToNameUser= (from u in messengeChat.Users
                                 where u.UserId == ms.ToUserId
                                 select u.FirstName + " " + u.LastName).SingleOrDefault(),
                    Message = ms.Content,
                    IsOwnMessage = ms.FromUserId == friend.UserId ? true : false,
                    IsSystemMessage = false,
                    DateSent = ms.DateSent,
                    DateRead = ms.DateRead,
                    AttachedFiles = ms.AttachedFiles,
                    ImgURLFromUser = (from u in messengeChat.Users
                                     where u.UserId ==ms.FromUserId
                                     select u.ImgURL).SingleOrDefault(),

                    ImgURLToUser = (from u in messengeChat.Users
                                      where u.UserId == ms.ToUserId
                                      select u.ImgURL).SingleOrDefault(),
                });
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, friend.FriendKey);
            await Clients.Caller.SendAsync("ReceiveOldMessage", msModel.ToList());

        }

        public void SeenFriend(FriendModel friendModel)
        {
            var fm = friendModel;
            if (friendModel.IdMessageNew.Count != 0)
            {
                foreach (var index in friendModel.IdMessageNew)
                {
                    var query = (from ms in messengeChat.Messagings
                                 where ms.MessageId == index
                                 select ms).SingleOrDefault();

                    query.DateRead = DateTime.Now;
                    messengeChat.SaveChanges();
                }

            }
            //fm.DateSend = DateTime.Now.ToString("H:mm", CultureInfo.InvariantCulture);
            //fm.CountUnRead = 0;
            //fm.IdMessageNew = (from ms in messengeChat.Messagings
            //                            where ms.FriendId == friendModel.FriendKey && ms.DateRead == null && ms.FromUserId != friendModel.UserId
            //                            select ms.MessageId).ToList();

            //fm.MessageNew = (from m in messengeChat.Messagings
            //                 where m.FriendId == friendModel.FriendKey
            //                 orderby m.DateSent descending
            //                 select m.Content).FirstOrDefault();
            //fm.ColorSeen = "#5C5B5B";
            //fm.IsSeen = false;

            //await Groups.AddToGroupAsync(Context.ConnectionId, friendModel.FriendKey);
            //await Clients.Group(friendModel.FriendKey).SendAsync("SeenFriend", fm);
        }

        public async Task GetFriend(int userId)
        {
            var modelFriend = (from f in messengeChat.Friends
                              join u in messengeChat.Users
                              on f.UserId equals u.UserId
                              //where u.UserId == userId
                               select new FriendModel
                              {
                                  FriendId = (int)f.UserIdfriend,
                                  UserId = u.UserId,
                                  status = (from us in messengeChat.Users 
                                            where us.UserId == f.UserIdfriend
                                            select us.Active
                                            ).SingleOrDefault() == 1 ? true : false,
                                  FriendKey = f.FriendKey,
                                  Name = (from us in messengeChat.Users
                                          where us.UserId == f.UserIdfriend
                                          select us.FirstName + " " + us.LastName).SingleOrDefault(),
                                  ImgURL = (from us in messengeChat.Users
                                            where us.UserId == f.UserIdfriend
                                            select us.ImgURL).SingleOrDefault(),
                                  DateSend = (from ms in messengeChat.Messagings
                                              where ms.FriendId == f.FriendKey
                                              orderby ms.DateSent descending
                                              select ms.DateSent
                                                ).SingleOrDefault().ToString("H:mm", CultureInfo.InvariantCulture),
                                  CountUnRead = (from ms in messengeChat.Messagings
                                                 where ms.FriendId == f.FriendKey && ms.DateRead == null && ms.FromUserId != u.UserId
                                                 select ms
                                                ).ToList().Count,
                                   IdMessageNew = (from ms in messengeChat.Messagings
                                                   where ms.FriendId == f.FriendKey && ms.DateRead == null && ms.FromUserId != u.UserId
                                                   select ms.MessageId
                                                ).ToList(),
                                   MessageNew = (from ms in messengeChat.Messagings
                                                where ms.FriendId == f.FriendKey
                                                orderby ms.DateSent descending
                                                select ms.Content
                                                ).SingleOrDefault(),

                                   sortDate = (from ms in messengeChat.Messagings
                                               where ms.FriendId == f.FriendKey
                                               orderby ms.DateSent descending
                                               select ms.DateSent
                                                ).SingleOrDefault(),
                                   ColorSeen = (from ms in messengeChat.Messagings
                                                where ms.FriendId == f.FriendKey && ms.DateRead == null && ms.FromUserId != u.UserId
                                                select ms
                                                ).ToList().Count == 0 ? "#5B5A5A" : "#000000",
                               });

            await Groups.AddToGroupAsync(Context.ConnectionId,"123");

            await Clients.Group("123").SendAsync("GetFriend", modelFriend.OrderByDescending(x => x.sortDate));
        }


        public async Task CallFriendAsync(int userID, string friendKey)
        {
            await Clients.Group(friendKey).SendAsync("ReceivePrivateVideoCall", userID, friendKey);
        }



    }
}
