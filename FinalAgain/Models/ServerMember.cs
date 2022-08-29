namespace FinalAgain.Models
{
    public class ServerMember
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ChannelId { get; set; }
        public AppUser User { get; set; }
        public Channel Channel { get; set; }
    }
}
