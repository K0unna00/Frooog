using FinalAgain.DAL;
using FinalAgain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FinalAgain.Services
{
    public class LayoutService
    {
        private readonly AppDBContext _context;
        private readonly UserManager<AppUser> _userManager;

        public LayoutService(AppDBContext context , UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public List<AppUser> GetUsers()
        {
            return _context.Users.ToList();
        }
        public List<FriendRequest> GetFriendRequests()
        {
            return _context.FriendRequests.ToList();
        }
        public bool GetServerJoinRequests(string id)
        {
            var servers = _context.Channels.Where(x => x.UserId == id);
            var requests = _context.ServerJoinRequests.ToList();
            foreach(var i in servers)
            {
                foreach(var j in requests)
                {
                    if (i.Id == j.ServerId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
