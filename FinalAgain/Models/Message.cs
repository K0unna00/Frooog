using System;

namespace FinalAgain.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public int ChannelId { get; set; }
        public DateTime createdAt { get; set; }
        public AppUser User { get; set; }
        public Channel Channel { get; set; }
    }
}
