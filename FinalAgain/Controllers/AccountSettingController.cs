using FinalAgain.DAL;
using FinalAgain.Helpers;
using FinalAgain.Models;
using FinalAgain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalAgain.Controllers
{
    [Authorize]
    public class AccountSettingController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public AccountSettingController(AppDBContext context, UserManager<AppUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }
        public IActionResult Index()
        {
            var data = _context.Users.Include(x => x.Friends1).Include(x => x.Friends2).ToList();
            var currentUser = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            List<AppUser> UserList = new List<AppUser>();
            foreach (var i in data)
            {
                if (User.Identity.Name != i.UserName)
                {
                    foreach (var x in i.Friends1)
                    {
                        if ((x.User1Id == currentUser.Id) || (x.User2Id == currentUser.Id))
                        {
                            UserList.Add(i);
                        }
                    }
                    foreach (var x in i.Friends2)
                    {
                        if ((x.User1Id == currentUser.Id) || (x.User2Id == currentUser.Id))
                        {
                            UserList.Add(i);
                        }
                    }
                }
            }


            var users = _context.Users.Include(x => x.FriendRequest1).Include(x=>x.FriendRequest2).ToList();
            AppUser user = users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            List<AppUser> UserData = new List<AppUser>();
            List<ServerJoinRequest> serverJoinRequests = new List<ServerJoinRequest>();
            List<Channel> Channels = new List<Channel>();


            foreach(var item in user.FriendRequest2)
            {
                var RUser = item.FromUser;
                if(RUser!=user)
                    UserData.Add(RUser);
            }

            var ownedServers = _context.Channels.Where(x => x.UserId == user.Id).ToList();
            var servers = _context.ServerMembers.Where(x => x.UserId == currentUser.Id).ToList();



            foreach(var i in servers)
            {
                Channels.Add(_context.Channels.FirstOrDefault(x=>x.Id==i.ChannelId));
            }
            foreach(var i in ownedServers)
            {
                var request = _context.ServerJoinRequests.Where(x => x.ServerId == i.Id).Include(x=>x.Server).ToList();
                foreach(var j in request)
                {
                    serverJoinRequests.Add(j);

                }
            }

            ViewBag.Servers = Channels;
            ViewBag.OwnedServers = ownedServers;
            ViewBag.JoinRequests = serverJoinRequests;
            ViewBag.UsersData = UserData;
            ViewBag.FriendUsers = UserList;
            AccountSettingViewModel AVM = new AccountSettingViewModel()
            {
                user = user,
            };
            return View(AVM);
        }
        [HttpPost]
        public IActionResult EditProfilePhoto(AppUser user)
        {
            var CurrentUser = _userManager.FindByNameAsync(user.UserName).Result;
            if (user.UserPPFile == null)
            {
                return RedirectToAction("index");
            }
            else
            {
                FileManager.Delete(_env.WebRootPath, "uploads/users", CurrentUser.UserPP);
                if (user.UserPPFile.ContentType == "image/png" || user.UserPPFile.ContentType == "image/jpeg")
                {
                    CurrentUser.UserPP = FileManager.Save(_env.WebRootPath, "uploads/users", user.UserPPFile);
                    var result = _userManager.UpdateAsync(CurrentUser).Result;
                }
            }
            return RedirectToAction("index");
        }
        [HttpPost]
        public async Task<IActionResult> EditProfile(AppUser user)
        {
            var CurrentUser = _userManager.FindByNameAsync(User.Identity.Name).Result;
            if (user.Email != null)
                CurrentUser.Email = user.Email;
            if (user.Description != null)
            {
                CurrentUser.Description = user.Description;
            }
            var result = await _userManager.UpdateAsync(CurrentUser);
            return RedirectToAction("index");
        }
        public IActionResult AcceptServerJoin( int id)
        {
            var rqst = _context.ServerJoinRequests.FirstOrDefault(x => x.Id == id);
            ServerMember SV = new ServerMember()
            {
                ChannelId = rqst.ServerId,
                UserId = rqst.FromUserId
            };
            _context.ServerMembers.Add(SV);
            _context.ServerJoinRequests.Remove(rqst);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult RejectServerJoin(int id)
        {
            var rqst = _context.ServerJoinRequests.FirstOrDefault(x => x.Id == id);
            _context.ServerJoinRequests.Remove(rqst);
            _context.SaveChanges();
            return RedirectToAction("index");

        }
        [HttpPost]
        public IActionResult ResetPasswordOnSettings(AccountSettingViewModel VM)
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var token =  _userManager.GeneratePasswordResetTokenAsync(user).Result;
            var result = _userManager.ResetPasswordAsync(user, token, VM.SecuriryResetPassword.Password).Result;

            return RedirectToAction("index");

        }
    }
}
