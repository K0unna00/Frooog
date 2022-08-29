using FinalAgain.DAL;
using FinalAgain.Helpers;
using FinalAgain.Hubs;
using FinalAgain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Threading.Tasks;

namespace FinalAgain.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDBContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHubContext<UserHub> _hubContext;
        private readonly IWebHostEnvironment _env;

        public HomeController(ILogger<HomeController> logger 
                       , AppDBContext context 
                             , UserManager<AppUser> userManager
                                   , IHubContext<UserHub> hubContext
                                        ,IWebHostEnvironment env)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _hubContext = hubContext;
            _env = env;
        }
        public IActionResult Index()
        {
            var data = _context.Users.Include(x => x.Friends1).Include(x => x.Friends2).ToList();
            var currentUser = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            List<AppUser> UserList = new List<AppUser>();
            foreach (var user in data)
            {
                if (User.Identity.Name != user.UserName)
                {
                    foreach(var x in user.Friends1)
                    {
                        if ( (x.User1Id == currentUser.Id) || (x.User2Id == currentUser.Id))
                        {
                            UserList.Add(user);
                        }
                    }
                    foreach (var x in user.Friends2)
                    {
                        if ((x.User1Id == currentUser.Id) || (x.User2Id == currentUser.Id))
                        {
                            UserList.Add(user);
                        }
                    }
                }
            }
            return View(UserList);
        }
        public IActionResult Server()
        {
            var currentUser = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var servers = _context.Channels.Include(x=>x.ServerMembers).ToList();
            List<Channel> channelList=new List<Channel>();
            foreach(var i in servers)
            {
                foreach(var j in i.ServerMembers)
                {
                    if (j.UserId == currentUser.Id)
                    {
                        channelList.Add(i);
                    }
                }
            }

            return View(channelList);
        }
        public IActionResult GetMessages([FromBody] int id)
        {
            var data = _context.Messages.Include(x => x.Channel).Include(x => x.User).Where(x => x.ChannelId == id).ToList();
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            if (user.OnServerId == id)
            {

                user.OnServerId = 0;
            }
            else
            {
                user.OnServerId = id;

            }
            var result = _userManager.UpdateAsync(user).Result;

            return Ok(data);
        }
        public IActionResult GetUsers()
        {
            var data = _context.Users.ToList();
            return Ok(data);
        }
        public IActionResult GetMessageInbox([FromBody] InboxMessage message)
        {
            var user = _context.Users.FirstOrDefault(x=> x.UserName == message.FromUserName);
            var data = _context.InboxMessages.Where(x=>x.ToUserId==message.ToUserId && x.FromUserId==user.Id).ToList();
            var data2= _context.InboxMessages.Where(x=>x.ToUserId == user.Id && x.FromUserId == message.ToUserId).ToList();
            List<InboxMessage> mList = new List<InboxMessage>();
            foreach(var i in data)
            {
                mList.Add(i);
            }
            foreach (var i in data2)
            {
                mList.Add(i);
            }
            var List = mList.OrderBy(x => x.createdAt);
            return Ok(List);
        }

        public IActionResult SetMicData([FromBody] object obj)
        {
            return Ok();
        }
        public IActionResult CreateServer()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateServer(Channel channel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (channel.ServerImageFile!=null)
            {
                if(channel.ServerImageFile.ContentType == "image/png" || channel.ServerImageFile.ContentType == "image/jpeg")
                {
                    AppUser currentUser = _userManager.FindByNameAsync(User.Identity.Name).Result;
                    channel.UserId = currentUser.Id;
                    channel.ServerImage = FileManager.Save(_env.WebRootPath, "uploads/Servers", channel.ServerImageFile);

                    _context.Channels.Add(channel);
                    _context.SaveChanges();


                    ServerMember SV = new ServerMember()
                    {
                        ChannelId = channel.Id,
                        UserId = currentUser.Id
                    };

                    _context.ServerMembers.Add(SV);
                    _context.SaveChanges();
                    return RedirectToAction("Server");
                }
                else
                {
                    ModelState.AddModelError("ServerImageFile", "Input png or jpg/jpeg");
                    return View();
                }
            }
            else
            {
                AppUser currentUser = _userManager.FindByNameAsync(User.Identity.Name).Result;
                channel.UserId = currentUser.Id;
                channel.ServerImage = "pp.png";
                _context.Channels.Add(channel);
                _context.SaveChanges();

                ServerMember SV = new ServerMember()
                {
                    ChannelId = channel.Id,
                    UserId = currentUser.Id
                };
                _context.ServerMembers.Add(SV);
                _context.SaveChanges();
                return RedirectToAction("Server");
            }
        }
        public IActionResult SearchServerOnGlobal()
        {
            var data=_context.Channels.Include(x=>x.User).Include(x=>x.ServerMembers).OrderByDescending(x => x.ServerMembers.Count).ToList();
            return View(data);
        }
        public IActionResult JoinServer([FromBody] int id)
        {
            AppUser currentUser =_userManager.FindByNameAsync(User.Identity.Name).Result;
            Channel channel = _context.Channels.FirstOrDefault(x => x.Id == id);
            List<ServerMember> serverMembers = _context.ServerMembers.Where(x => x.ChannelId == id).ToList();
            foreach(var serverM in serverMembers)
            {
                if (serverM.UserId == currentUser.Id)
                {
                    return Json(false);
                }
            }
            ServerMember SV = new ServerMember()
            {
                ChannelId=channel.Id,
                UserId=currentUser.Id
            };
            _context.ServerMembers.Add(SV);
            _context.SaveChanges();
            return Json(true);
        }
        public IActionResult SendJoinRequest([FromBody] int id)
        {
            AppUser currentUser = _userManager.FindByNameAsync(User.Identity.Name).Result;
            Channel channel = _context.Channels.FirstOrDefault(x => x.Id == id);
            List<ServerMember> serverMembers = _context.ServerMembers.Where(x => x.ChannelId == id).ToList();
            foreach (var serverM in serverMembers)
            {
                if (serverM.UserId == currentUser.Id)
                {
                    return Json(false);
                }
            }
            ServerJoinRequest request = new ServerJoinRequest()
            {
                FromUserId = currentUser.Id,
                ServerId = channel.Id,
                Server=channel
            };
            _context.ServerJoinRequests.Add(request);
            _context.SaveChanges();
            return Ok(true);
        }
        public IActionResult EditServer(int id)
        {
            var data = _context.Channels.FirstOrDefault(x => x.Id == id);
            return View(data);
        }
        [HttpPost]
        public IActionResult EditServer(Channel channel)
        {
            var currentServer = _context.Channels.FirstOrDefault(x => x.Id == channel.Id);
            if (!ModelState.IsValid)
            {
                var data = _context.Channels.FirstOrDefault(x => x.Id == currentServer.Id);
                return View(data);
            }
            currentServer.Desc = channel.Desc;
            currentServer.IsPrivate = channel.IsPrivate;
            currentServer.Name = channel.Name;
            if (channel.ServerImageFile != null)
            {
                FileManager.Delete(_env.WebRootPath, "uploads/servers", currentServer.ServerImage);
                currentServer.ServerImage=FileManager.Save(_env.WebRootPath, "uploads/servers", channel.ServerImageFile);
            }
            _context.SaveChanges();
            return RedirectToAction("index", "AccountSetting");
        }
        public IActionResult LeaveServer(int id)
        {
            var currentUser = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var data=_context.ServerMembers.FirstOrDefault(x => x.UserId == currentUser.Id && x.ChannelId == id);
            _context.ServerMembers.Remove(data);
            _context.SaveChanges();
            return RedirectToAction("index", "AccountSetting");
        }
        public IActionResult RemoveServer(int id)
        {
            var channel = _context.Channels.FirstOrDefault(x => x.Id == id);
            var messages = _context.Messages.Where(x => x.ChannelId == id).ToList();
            var serverMembers = _context.ServerMembers.Where(x => x.ChannelId == id).ToList();
            var requests = _context.ServerJoinRequests.Where(x => x.ServerId == id).ToList();
            if (messages != null)
            {
                foreach (Message i in messages)
                {
                    _context.Messages.Remove(i);
                }
            }
            if (serverMembers != null)
            {
                foreach (var i in serverMembers)
                {
                    _context.ServerMembers.Remove(i);
                }
            }
            if (requests != null)
            {
                foreach (var i in requests)
                {
                    _context.ServerJoinRequests.Remove(i);
                }
            }
            _context.Channels.Remove(channel);
            _context.SaveChanges();
            return RedirectToAction("index", "AccountSetting");
        }
        public IActionResult DeleteMessage([FromBody] int id)
        {
            var data = _context.InboxMessages.FirstOrDefault(x => x.Id == id);
            if (data!=null)
            {
                _context.InboxMessages.Remove(data);
                _context.SaveChanges();
                return Ok(true);
            }
            return Ok(false);
        }
        public IActionResult DeleteServerMessage([FromBody] int id)
        {
            var data = _context.Messages.FirstOrDefault(x => x.Id == id);
            if (data != null)
            {
                _context.Messages.Remove(data);
                _context.SaveChanges();
                return Ok(true);
            }
            return Ok(false);
        }
        public IActionResult SearchServer([FromBody] string value)
        {
            var data=_context.Channels.Where(x => x.Name.Contains(value)).ToList();
            return Ok(data);
        }
    }
}
