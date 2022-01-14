using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("/api/user")]
    public class UserController : ControllerBase
    {
        private readonly MessengeChatTestContext messengeChat;
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _environment;
        private static string numberPhoneS;
        private static string CodeOTPS;
        public UserController(MessengeChatTestContext messenge, ILogger<HomeController> logger,
            IWebHostEnvironment environment)
        {
            this.messengeChat = messenge;
            this._logger = logger;
            this._environment = environment ?? throw new ArgumentNullException(nameof(environment));

        }

        [HttpGet("{gmail}&{password}")]

        public ActionResult GetUser(string gmail, string password)
        {
            var user = (from u in messengeChat.Users
                        where u.Gmail == gmail && u.Passwordd == password
                        select
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
                            UserIdfriend = 0,
                            status = 0,
                            FriendKey = "",
                        }
                       ).FirstOrDefault();


            return Ok(user);
        }

        [Route("getOTP")]
        [HttpGet("getOTP/{numberphone}")]
        public IActionResult GetUserFromNumberPhone(string numberphone)
        {
            numberPhoneS = numberphone;
            string numberPhoneConvert = "+84" + numberphone.Substring(1, numberphone.Length - 1);

            string accountSid = "ACdfd4a57cdcef99989542588a049c3b30";
            string authToken = "cdbf5fa43fba3ba1ba779a7f61a726d6";

            TwilioClient.Init(accountSid, authToken);

            Random generator = new Random();
            string randomOTP = generator.Next(0, 9999).ToString("D4");
            CodeOTPS = randomOTP;
            var message = MessageResource.Create(
                body: "Your new OPT is ? " + randomOTP,
                from: new Twilio.Types.PhoneNumber("+17373771201"),
                to: new Twilio.Types.PhoneNumber(numberPhoneConvert)
            );
            return Ok(message.Sid);
        }


        [HttpGet("getUser/{CodeOTP}")]
        [Route("getUser")]
        public IActionResult GetUserToOTP(string CodeOTP)
        {
            if(CodeOTPS == CodeOTP)
            {
                var user = (from u in messengeChat.Users
                            where u.Phone == numberPhoneS
                            select
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
                                UserIdfriend = 0,
                                status = 0,
                                FriendKey = "",
                            }
                     ).FirstOrDefault();
                return Ok(user);
            }

           

            return Ok(null);
        }

        [HttpPost]
        [Route("register")]
        public IActionResult PostUser(User user)
        {
            var query = (from u in messengeChat.Users
                         where u.Gmail == user.Gmail || u.Phone == user.Phone
                         select u).FirstOrDefault();
            if (query == null)
            {
                var us = new User()
                {
                    UserIdtype = 1,
                    Active = 1,
                    ActivatedDate = DateTime.Now,
                    CompanyName = "UIT",
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Passwordd = user.Passwordd,
                    BirthDate = user.BirthDate,
                    Address1 = user.Address1,
                    Address2 = user.Address2,
                    City = "Vn",
                    Phone = user.Phone,
                    Gmail = user.Gmail,
                    ImgURL = user.ImgURL
                };

                messengeChat.Add(us);
                messengeChat.SaveChanges();
                return Ok(us);
            }

            return BadRequest();
        }
    
    }
}
