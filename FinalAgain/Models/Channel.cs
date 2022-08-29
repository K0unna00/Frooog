using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalAgain.Models
{
    public class Channel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public bool IsPrivate { get; set; }
        public AppUser User { get; set; }
        public string ServerImage { get; set; }
        [NotMapped]
        public IFormFile ServerImageFile { get; set; }
        public List<ServerMember> ServerMembers { get; set; } = new List<ServerMember>();
    }
}
