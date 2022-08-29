namespace FinalAgain.Models
{
    public class Friendship
    {
        public int Id { get; set; }
        public string User1Id { get; set; }
        public string User2Id { get; set; }
        public AppUser User1 { get; set; }
        public AppUser User2 { get; set; }
    }
}
