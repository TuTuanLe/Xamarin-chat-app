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
            Messaging ms = new Messaging();
            if (friend != null)
            {
                ms.FromUserId = userId;
                ms.DateSent = DateTime.Now;
                ms.DateRead = null;
                ms.Content = message;
                ms.ToUserId = friend.UserIdfriend;
                ms.FriendId = friend.FriendKey;
                ms.AttachedFiles = AttachedFiles;
            
                messengeChat.Add(ms);
                messengeChat.SaveChanges();
            }
            else
            {
                ms.FromUserId = userId;
                ms.DateSent = DateTime.Now;
                ms.DateRead = null;
                ms.Content = message;
                ms.ToUserId = 0;
                ms.FriendId = friendKey;
                ms.AttachedFiles = AttachedFiles;
                
                messengeChat.Add(ms);
                messengeChat.SaveChanges();
            }
            


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
                IsOwnMessage = ms.FromUserId == userId ? true : false,
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
            var accceptFriend = (from f in messengeChat.Friends
                                 join u in messengeChat.Users
                                 on f.UserId equals u.UserId
                                 where f.status == 0
                                 select new FriendModel
                                 {
                                     AcceptFriend = false,
                                     FriendId = (int)f.UserIdfriend,
                                     UserId = u.UserId,
                                     status = (from us in messengeChat.Users
                                               where us.UserId == f.UserId
                                               select us.Active
                                            ).SingleOrDefault() == 1 ? true : false,
                                     FriendKey = f.FriendKey,
                                     Name = (from us in messengeChat.Users
                                             where us.UserId == f.UserId
                                             select us.FirstName + " " + us.LastName).SingleOrDefault(),
                                     ImgURL = (from us in messengeChat.Users
                                               where us.UserId == f.UserId
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
                                 }).ToList();


            var modelFriend = (from f in messengeChat.Friends
                              join u in messengeChat.Users
                              on f.UserId equals u.UserId
                              where f.status == 1
                               select new FriendModel
                              {
                                  AcceptFriend = true,
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
                               }).ToList();


            var modelGroupFriend = (from f in messengeChat.DetailUserGroups
                               join u in messengeChat.Users
                               on f.AddUserId equals u.UserId
                               select new FriendModel
                               {
                                   AcceptFriend = true,
                                   FriendId = 0,
                                   UserId = u.UserId,
                                   status = (from us in messengeChat.Users
                                             where us.UserId == f.AddUserId
                                             select us.Active
                                            ).SingleOrDefault() == 1 ? true : false,
                                   FriendKey = f.FriendKey,
                                   Name = (from userGroup in messengeChat.UserGroups 
                                          where userGroup.UserGroupId == f.UserGroupId
                                          select userGroup.NickNameowner).FirstOrDefault(),
                                   ImgURL = (from us in messengeChat.Users
                                             where us.UserId == f.AddUserId
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
                               }).ToList();

            modelFriend.AddRange(modelGroupFriend);
            modelFriend.AddRange(accceptFriend);
            await Groups.AddToGroupAsync(Context.ConnectionId,"123");

            await Clients.Group("123").SendAsync("GetFriend", modelFriend.OrderByDescending(x => x.sortDate));
        }

        public async Task CallFriendAsync(int userID, string friendKey)
        {
            await Clients.Group(friendKey).SendAsync("ReceivePrivateVideoCall", userID, friendKey);
        }

        public async Task AddGroupFriend(int userId,string ImageUrl, string GroupName,List<FriendModel> friendModels)
        {
          
            var friendKey = generateUniqueID();
            
            messengeChat.Add(new UserGroup()
            {
                UserId = userId,
                IsAdmin = 1,
                NickNameowner = GroupName,
            });
            messengeChat.SaveChanges();

            var UserGroupId = messengeChat
                                    .UserGroups
                                    .Where(c => c.UserId == userId
                                                && c.NickNameowner.Contains(GroupName) == true)
                                    .Select(c => c.UserGroupId)
                                    .FirstOrDefault();

            messengeChat.Add(new DetailUserGroup()
            {
                UserGroupId = UserGroupId,
                AddUserId = userId,
                NickNameGuest = messengeChat
                                    .Users
                                    .Where(c => c.UserId == userId )
                                    .Select(c => c.FirstName +" "+ c.LastName)
                                    .FirstOrDefault(),
                FriendKey = friendKey
            });
            messengeChat.SaveChanges();

            foreach(var friend in friendModels)
            {
                messengeChat.Add(new DetailUserGroup()
                {
                    UserGroupId = UserGroupId,
                    AddUserId = friend.FriendId,
                    NickNameGuest = friend.Name,
                    FriendKey = friendKey
                });
                messengeChat.SaveChanges();
            }
            

            messengeChat.Add(new Messaging()
            {
                FromUserId = userId,
                DateSent = DateTime.Now,
                DateRead = null,
                Content = "Add success Group",
                ToUserId = 0,
                FriendId = friendKey,
                AttachedFiles= null
            });
            messengeChat.SaveChanges();

            await Clients.Caller.SendAsync("AddGroupSuccess");
        }

        private string generateUniqueID(int _characterLength = 11)
        {
            System.Text.StringBuilder _builder = new System.Text.StringBuilder();
            Enumerable
                .Range(65, 26)
                .Select(e => ((char)e).ToString())
                .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
                .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
                .OrderBy(e => Guid.NewGuid())
                .Take(_characterLength)
                .ToList().ForEach(e => _builder.Append(e));
            return _builder.ToString();
        }
        //public async Task AddFriend()
        //{

        //}
    }
}
