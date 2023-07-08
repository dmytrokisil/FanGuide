using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FanGuide.Enums
{
    public enum TrainerSort
    {
        [Display(Name = "Sort by trainer surname (A-Z)")]
        NameAsc,
        [Display(Name = "Sort by trainer surname (Z-A)")]
        NameDesc,
        [Display(Name = "Sort by team name (A-Z)")]
        TeamAsc,
        [Display(Name = "Sort by team name (Z-A)")]
        TeamDesc,
        [Display(Name = "Sort by age (new first)")]
        AgeAsc,
        [Display(Name = "Sort by age (old first)")]
        AgeDesc,
    }
}
