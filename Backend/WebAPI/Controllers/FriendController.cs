using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("/api/friend")]
    public class FriendController : ControllerBase
    {
        private readonly MessengeChatTestContext messengeChat;
        private readonly ILogger<FriendController> _logger;
        private readonly IWebHostEnvironment _environment;
        public FriendController(MessengeChatTestContext messenge, ILogger<FriendController> logger,
            IWebHostEnvironment environment)
        {
            this.messengeChat = messenge;
            this._logger = logger;
            this._environment = environment ?? throw new ArgumentNullException(nameof(environment));

        }

        [HttpGet("AllFriend")]
        public ActionResult GetAllFriend()
        {
            var getAllUser = messengeChat.Users.ToList();
            return Ok(getAllUser);
        }


        [HttpGet("AllFriends/{UserId}")]
        public IActionResult GetAllFriend(int UserId)
        {
            var getAllUser = (from u in messengeChat.Users
                              where u.UserId != UserId
                              select (
                                 new UserModel()
                                 {
                                     UserId = u.UserId,
                                     UserIdtype = u.UserIdtype,
                                     Active = u.Active,
                                     CompanyName = u.CompanyName,
                                     FullName = u.FirstName + " " + u.LastName,
                                     Passwordd = u.Passwordd,
                                     BirthDate = u.BirthDate,
                                     Address1 = u.Address1,
                                     Address2 = u.Address2,
                                     City = u.City,
                                     Phone = u.Phone,
                                     ImgURL = u.ImgURL,
                                     UserIdfriend = (from f in messengeChat.Friends
                                                     where (f.UserId == UserId && f.UserIdfriend == u.UserId)
                                                     || (((f.UserIdfriend == UserId) && (f.status == 0)) && f.UserId == u.UserId)
                                                     select f.UserIdfriend
                                                    ).FirstOrDefault(),
                                     status = (from f in messengeChat.Friends
                                               where (f.UserId == UserId && f.UserIdfriend == u.UserId)
                                                     || (((f.UserIdfriend == UserId) && (f.status == 0)) && f.UserId == u.UserId)
                                               select f.status
                                                    ).FirstOrDefault(),
                                     FriendKey = (from f in messengeChat.Friends
                                                  where (f.UserId == UserId && f.UserIdfriend == u.UserId)
                                                     || (((f.UserIdfriend == UserId) && (f.status == 0)) && f.UserId == u.UserId)
                                                  select f.FriendKey
                                                    ).FirstOrDefault(),
                                 }


                              )).ToList();

            return Ok(getAllUser);
        }


        [HttpGet("TestAllFriend/{UserId}")]
        public IActionResult GetFriend(int UserId)
        {
            var accceptFriend = (from f in messengeChat.Friends
                                 join u in messengeChat.Users
                                 on f.UserId equals u.UserId
                                 where f.status == 0 && f.UserIdfriend == UserId
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
                                 }).ToList();

            var modelFriend = (from f in messengeChat.Friends
                               join u in messengeChat.Users
                               on f.UserId equals u.UserId
                               where f.status == 1 && u.UserId == UserId
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
                               }).ToList();


            var modelGroupFriend = (from f in messengeChat.DetailUserGroups
                                    join u in messengeChat.Users
                                    on f.AddUserId equals u.UserId
                                    where u.UserId == UserId
                                    select new FriendModel
                                    {
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
            return Ok(modelFriend);
        }
    
        [HttpPut]
        [Route("AcceptFriend")]
        public IActionResult PostAcceptFriend(FriendModel friendModel)
        {
            var updateF = (from f in messengeChat.Friends
                           where f.FriendKey == friendModel.FriendKey
                           select f).FirstOrDefault();
            updateF.status = 1;
            messengeChat.Update(updateF);
            messengeChat.SaveChanges();



            var friend = new Friend()
            {
                UserId = friendModel.FriendId,
                UserIdfriend = friendModel.UserId,
                status = 1,
                FriendKey = friendModel.FriendKey,
            };

            messengeChat.Add(friend);
            messengeChat.SaveChanges();



            

            return Ok();
        }

        [HttpPost]
        [Route("RequestFriend")]
        public IActionResult PostRequestFriend(MessageModel messageModel)
        {
            string friendKey = generateUniqueID();

            var friend = new Friend()
            {
                UserId = messageModel.fromUserId,
                UserIdfriend = messageModel.toUserId,
                status = 0,
                FriendKey = friendKey,
            };

            messengeChat.Add(friend);
            messengeChat.SaveChanges();

            var msFirst = new Messaging()
            {
                FromUserId = messageModel.fromUserId,
                DateSent = DateTime.Now,
                DateRead = DateTime.Now,
                Content = "Sent request friend",
                AttachedFiles = null,
                ToUserId = messageModel.toUserId,
                FriendId = friendKey
            };
            messengeChat.Add(msFirst);
            messengeChat.SaveChanges();

            return Ok();
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
    }
}
