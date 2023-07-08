using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FanGuide.Models
{
    public class Trainer : UserBase
    {
        public Team Team { get; set; }
        public int TeamId { get; set; }
    }
}
