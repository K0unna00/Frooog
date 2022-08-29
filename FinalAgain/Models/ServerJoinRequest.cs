using System;

namespace FinalAgain.Models
{
    public class ServerJoinRequest
    {
        public int Id { get; set; }
        public string FromUserId { get; set; }
        public int ServerId { get; set; }
        public DateTime SendAt { get; set; } = DateTime.Now;
        public AppUser FromUser { get; set; }
        public Channel Server { get; set; }
    }
}
