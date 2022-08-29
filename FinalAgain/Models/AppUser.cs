using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalAgain.Models
{
    public class AppUser:IdentityUser
    {
        public string ConnectionId { get; set; }
        public int OnServerId { get; set; }
        public bool CameraOn { get; set; }
        public string UserPP { get; set; }
        public string Description { get; set; }
        [NotMapped]
        public IFormFile UserPPFile { get; set; }
        [InverseProperty("User1")]
        public List<Friendship> Friends1 { get; set; } = new List<Friendship>();
        [InverseProperty("User2")]
        public List<Friendship> Friends2 { get; set; } = new List<Friendship>();
        [InverseProperty("FromUser")]
        public List<FriendRequest> FriendRequest1 { get; set; } = new List<FriendRequest>();
        [InverseProperty("ToUser")]
        public List<FriendRequest> FriendRequest2 { get; set; } = new List<FriendRequest>();
        public List<Fooorm> Fooorms { get; set; } = new List<Fooorm>();
        public List<ServerMember> ServerMembers { get; set; } = new List<ServerMember>();
    }
}
