using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalAgain.Models
{
    public class Position
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(15)]
        public string Name { get; set; }
        [Required]
        [MaxLength(250)]
        public string Desc { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
