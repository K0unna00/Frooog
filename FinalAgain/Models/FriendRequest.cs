namespace FinalAgain.Models
{
    public class FriendRequest
    {
        public int Id { get; set; }
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public AppUser FromUser { get; set; }
        public AppUser ToUser { get; set; }
    }
}
