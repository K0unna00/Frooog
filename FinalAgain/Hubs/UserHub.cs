using FinalAgain.DAL;
using FinalAgain.Helpers;
using FinalAgain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalAgain.Hubs
{
    public class UserHub:Hub
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDBContext _context;
        private readonly IWebHostEnvironment _env;

        public UserHub(UserManager<AppUser> userManager , AppDBContext context , IWebHostEnvironment env)
        {
            _userManager = userManager;
            _context = context;
            _env = env;
        }
        public override Task OnConnectedAsync()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                AppUser user = _userManager.FindByNameAsync(Context.User.Identity.Name).Result;
                user.ConnectionId = Context.ConnectionId;
                user.OnServerId = 0;
                var result = _userManager.UpdateAsync(user).Result;
                var ress = Clients.Client(user.ConnectionId).SendAsync("AddUsersOnServer", user);
            }
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                AppUser user = _userManager.FindByNameAsync(Context.User.Identity.Name).Result;
                user.ConnectionId = null;
                user.OnServerId = 0;
                user.CameraOn = false;
                var result = _userManager.UpdateAsync(user).Result;
                var res = Clients.All.SendAsync("CloseCameraFromAllUserWhenDisconnect", user.UserName);
                var ress = Clients.All.SendAsync("DeleteUserOnServer", user);
            }
            return base.OnDisconnectedAsync(exception);
        }
        public async Task SendMessage(string text , int channelId)
        {
            var name = Context.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(name);
            var date = DateTime.Now;
            await Clients.All.SendAsync("ReceiveMessage", user, text , channelId, date);
        }
        public async Task SendMessageInbox(string text , string FUserName , string TUserId, int id )
        {
            var TUser = await _userManager.FindByIdAsync(TUserId);
            var FUser = await _userManager.FindByNameAsync(FUserName);
            var sentAt = DateTime.Now;
            await Clients.Clients(TUser.ConnectionId , FUser.ConnectionId).SendAsync("ReceiveMessage", FUserName, text , sentAt , FUser.Id,id);
        }
        public async Task SendMessageFooorm(string text , int fooormId)
        {
            var user = await _userManager.FindByNameAsync(Context.User.Identity.Name);
            await Clients.All.SendAsync("ReceiveMessageFooorm", text, fooormId ,user , DateTime.Now);
        }
        public async Task ShowCameraData()
        {
            var user = await _userManager.FindByNameAsync(Context.User.Identity.Name);
            user.CameraOn = true;
            var result = _userManager.UpdateAsync(user).Result;
            await Clients.All.SendAsync("ReceiveCameraData", user);
        }
        public async Task CloseCamera(int i)
        {
            var user = await _userManager.FindByNameAsync(Context.User.Identity.Name);
            user.CameraOn = false;
            var result = _userManager.UpdateAsync(user).Result;
            await Clients.All.SendAsync("CloseCameraFromAllUser", i);
        }
        public async Task AddUserOnServer(int id)
        {
            var user = await _userManager.FindByNameAsync(Context.User.Identity.Name);

            await Clients.All.SendAsync("AddUserOnServer", id , user);
        }
        public async Task ShowOthersCameras(int id)
        {
            var user = await _userManager.FindByNameAsync(Context.User.Identity.Name);
            List<string> connectionStringList = new List<string>();
            foreach(var userr in _userManager.Users.ToList())
            {
                if(userr.CameraOn && userr.OnServerId == id)
                {
                    connectionStringList.Add(userr.ConnectionId);
                }
            }
            await Clients.Clients(connectionStringList).SendAsync("ShowCameraToNewUser");
        }
        public async Task SendVoiceMessage(object data)
        {
            FileManager.SaveAudio(_env.WebRootPath, "uploads/Audios", data);
            await Clients.All.SendAsync("receiveVoiceMessage",data);
        }
    }
}
