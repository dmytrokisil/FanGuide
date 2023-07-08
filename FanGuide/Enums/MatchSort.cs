using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FanGuide.Enums
{
    public enum MatchSort
    {
        [Display(Name = "Sort by first team (A-Z)")]
        HomeTeamNameAsc,
        [Display(Name = "Sort by first team (Z-A)")]
        HomeTeamNameDesc,
        [Display(Name = "Sort by second team (A-Z)")]
        VisitorTeamNameAsc,
        [Display(Name = "Sort by second team (Z-A)")]
        VisitorTeamNameDesc,
        [Display(Name = "Sort by date (new first)")]
        DateAsc,
        [Display(Name = "Sort by date (old first)")]
        DateDesc,
    }
}
