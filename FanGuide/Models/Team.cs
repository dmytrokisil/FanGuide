using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FanGuide.Models
{
    public class Team
    {
        public int Id { get; set; }
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The string length must be between 3 and 50 characters")]
        public string Name { get; set; }
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The string length must be between 3 and 50 characters")]
        public string City { get; set; }
        public DateTime CreateDate { get; set; }
        public List<Trainer> Trainers { get; set; } = new();
        public List<Sportsman> Sportsmen { get; set; } = new();
        public List<Match> HomeMatches { get; set; } = new();
        public List<Match> VisitorMatches { get; set; } = new();
        public Sport Sport { get; set; }
        public int SportId { get; set; }
    }
}
