using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("/api/message")]
    public class HomeController : ControllerBase
    {
        private readonly MessengeChatTestContext messengeChat;
        public HomeController(MessengeChatTestContext messenge)
        {
            this.messengeChat = messenge;
        }
        [HttpGet("{userId}")]

        public IActionResult GetUser(int userId)
        {
            var modelFriend = (from f in messengeChat.Friends
                               join u in messengeChat.Users
                               on f.UserId equals u.UserId
                               select new FriendModel
                               {
                                   FriendId = f.FriendId,
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
                                                ).ToList().Count == 0 ? "#C7C7C7" : "#000000"
                               });


            return Ok(modelFriend.ToList());
        }

        [HttpGet]
        public IActionResult GetFriend()
        {
            int userId = 1;
            //var test = from u in messengeChat.Users
            //           select u.Active== 1? true: false;
            //return Ok(test);

            var MessageNew = (from ms in messengeChat.Messagings
                              where ms.FriendId == "1221"
                              orderby ms.DateSent descending
                              select ms.Content);
            return Ok(MessageNew.FirstOrDefault());
        }

    }
}
