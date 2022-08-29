using FinalAgain.DAL;
using FinalAgain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FinalAgain.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDBContext _context;

        public ChatController(UserManager<AppUser> userManager , AppDBContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public IActionResult Index(int id)
        {
            var data = _userManager.Users.ToList();
            ViewBag.Messages = _context.Messages.Include(x => x.Channel).Include(x => x.User).Where(x=>x.ChannelId==id).ToList();
            ViewBag.ChannelId = id;
            return View(data);
        }
        public IActionResult Channels()
        {
            var data = _context.Channels.ToList();
            return View(data);
        }
        public async Task<IActionResult> SetMessage([FromBody] Message messageFetch)
        {
            var name = User.Identity.Name;  
            var user = await _userManager.FindByNameAsync(name);

            Message message = new Message()
            {
                UserId = user.Id,
                ChannelId= messageFetch.ChannelId,
                Text=messageFetch.Text,
                createdAt=DateTime.Now
            };
            _context.Messages.Add(message);
            _context.SaveChanges();
            return RedirectToAction("index", "Chat");
        }
        public IActionResult SetMessageInbox([FromBody] InboxMessage messageFetch)
        {
            var user = _context.Users.FirstOrDefault(x => x.UserName == messageFetch.FromUserName);
            messageFetch.FromUserId = user.Id;
            messageFetch.createdAt = DateTime.Now;
            _context.InboxMessages.Add(messageFetch);
            _context.SaveChanges();
            var data = _context.InboxMessages.First(x => x == messageFetch);
            return Ok(data.Id);
            //return RedirectToAction("index", "Chat");
        }
        public IActionResult SetVoiceInbox([FromBody] object obj)
        {
            return Ok();
        }
    }
}
