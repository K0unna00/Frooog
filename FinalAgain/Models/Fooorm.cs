using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalAgain.Models
{
    public class Fooorm
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [MaxLength(45)]
        public string Header { get; set; }
        [MaxLength(245)]
        public string Text { get; set; }

        public int ViewCount { get; set; }
        public int AnswerCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<FooormAnswers> FooormAnswers { get; set; } = new List<FooormAnswers>();
        public AppUser User { get; set; }

    }
}
