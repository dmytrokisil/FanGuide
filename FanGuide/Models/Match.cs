using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FanGuide.Models
{
    public class Match
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Team HomeTeam { get; set; }
        [Required(ErrorMessage = "Type something")]
        public int HomeTeamId { get; set; }
        public Team VisitorTeam { get; set; }
        [Required(ErrorMessage = "Type something")]
        public int VisitorTeamId { get; set; }
        [Range(0, 1000, ErrorMessage = "Invalid score")]
        public int HomeTeamScore { get; set; }
        [Range(0, 1000, ErrorMessage = "Invalid score")]
        public int VisitorTeamScore { get; set; }
    }
}
