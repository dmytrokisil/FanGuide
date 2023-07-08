using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FanGuide.Models
{
    public abstract class UserBase
    {
        public int Id { get; set; }
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The string length must be between 3 and 50 characters")]
        public string Name { get; set; }
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The string length must be between 3 and 50 characters")]
        public string Surname { get; set; }
        [Range(1, 110, ErrorMessage = "Invalid age")]
        public int Age { get; set; }
    }
}
