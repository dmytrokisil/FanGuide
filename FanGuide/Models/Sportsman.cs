using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FanGuide.Models
{
    public class Sportsman : UserBase
    {
        public int Weight { get; set; }
        public int Height { get; set; }
        public string Nationally { get; set; }
        public string Records { get; set; }
        public string Description { get; set; }
        public Team Team { get; set; }
        public int TeamId { get; set; }
    }
}
