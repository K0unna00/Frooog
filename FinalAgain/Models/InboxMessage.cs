using System;

namespace FinalAgain.Models
{
    public class InboxMessage
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public string FromUserName { get; set; }
        public DateTime createdAt { get; set; }
        public AppUser FromUser { get; set; }
        public AppUser ToUser { get; set; }
    }
}
