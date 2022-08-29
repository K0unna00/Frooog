using System;

namespace FinalAgain.Models
{
    public class FooormAnswers
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Text { get; set; }
        public int FooormId { get; set; }
        public bool IsUseful { get; set; }
        public DateTime CreatedAt { get; set; }
        public Fooorm Fooorm { get; set; }
        public AppUser User { get; set; }
    }
}
