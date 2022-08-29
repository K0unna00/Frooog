using FinalAgain.DAL;
using FinalAgain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalAgain.Controllers
{
	public class Friendship_FriendRequestController : Controller
	{
        private readonly AppDBContext _context;
        private readonly UserManager<AppUser> _userManager;

        public Friendship_FriendRequestController(AppDBContext context , UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public bool GetFriends(AppUser user1  , AppUser user2)
        {
            var data1= _context.Friends.FirstOrDefault(x => x.User1Id == user1.Id && x.User2Id == user2.Id);
            var data2= _context.Friends.FirstOrDefault(x => x.User1Id == user2.Id && x.User2Id == user1.Id);
            if (data1 != null || data2 != null)
            {
                return true;
            }
            return false;
        }
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult AddFriend()
		{
			return View();
		}
		public async Task<IActionResult> SearchUser([FromBody] string userName)
        {
            var  searchedUser = await _userManager.FindByNameAsync(userName);
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var data1 = _context.Friends.FirstOrDefault(x => x.User1Id == searchedUser.Id && x.User2Id == currentUser.Id);
            var data2 = _context.Friends.FirstOrDefault(x => x.User1Id == currentUser.Id && x.User2Id == searchedUser.Id);
            var data3 = _context.FriendRequests.FirstOrDefault(x =>x.FromUserId==currentUser.Id && x.ToUserId==searchedUser.Id);
            var data4 = _context.FriendRequests.FirstOrDefault(x =>x.FromUserId== searchedUser.Id && x.ToUserId== currentUser.Id);
            if (data1 != null || data2 != null || data3!=null || data4!=null)
            {
                return Json(null);
            }
            if (currentUser.UserName != userName)
            {
                return Json(searchedUser);
            }
            return Json(null);
        }
        public IActionResult SendRequest([FromBody]  string id)
        {
            var currentUser = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var ToUser = _context.Users.FirstOrDefault(x => x.Id == id);
            FriendRequest request = new FriendRequest()
            {
                FromUserId = currentUser.Id,
                ToUserId = ToUser.Id,
                FromUser = currentUser,
                ToUser = ToUser
            };
            _context.FriendRequests.Add(request);
            _context.SaveChanges();
            return Ok(true);
        }
        public IActionResult AcceptRequest(string id)
        {
            var User1 = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var User2 = _context.Users.FirstOrDefault(x => x.Id == id);

            Friendship friendship = new Friendship()
            {
                User1 = User1,
                User2 = User2,
                User1Id = User1.Id,
                User2Id = User2.Id
            };

            _context.Friends.Add(friendship);
            var data = _context.FriendRequests.FirstOrDefault(x => x.ToUser == User1 && x.FromUser == User2);
            _context.FriendRequests.Remove(data);
            _context.SaveChanges();

            return RedirectToAction("index","AccountSetting");
        }
        public IActionResult RejectRequest(string id)
        {
            var User1 = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var User2 = _context.Users.FirstOrDefault(x => x.Id == id);
            var data = _context.FriendRequests.FirstOrDefault(x => x.ToUser == User1 && x.FromUser == User2);
            _context.FriendRequests.Remove(data);
            _context.SaveChanges();
            return RedirectToAction("index", "AccountSetting");

        }
        public IActionResult RemoveFriend(string id)
        {
            var user1 = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var user2 = _userManager.FindByIdAsync(id).Result;
            var friendship = _context.Friends.FirstOrDefault(x => (x.User1Id == user1.Id && x.User2Id == user2.Id) || (x.User2Id == user1.Id && x.User1Id == user2.Id));
            if (friendship != null)
            {
                _context.Friends.Remove(friendship);
            }
            _context.SaveChanges();
            return RedirectToAction("index", "AccountSetting");

        }
    }
}
