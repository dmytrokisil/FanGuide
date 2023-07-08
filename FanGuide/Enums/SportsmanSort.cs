using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FanGuide.Enums
{
    public enum SportsmanSort
    {
        [Display(Name = "Sort by name (A-Z)")]
        NameAsc,
        [Display(Name = "Sort by name (Z-A)")]
        NameDesc,
        [Display(Name = "Sort by surname (A-Z)")]
        SurnameAsc,
        [Display(Name = "Sort by surname (Z-A)")]
        SurnameDesc,
        [Display(Name = "Sort by team name (A-Z)")]
        TeamAsc,
        [Display(Name = "Sort by team name (Z-A)")]
        TeamDesc,
        [Display(Name = "Sort by age (new first)")]
        AgeAsc,
        [Display(Name = "Sort by age (old first)")]
        AgeDesc,
        [Display(Name = "Sort by weight (heavy first)")]
        WeightAsc,
        [Display(Name = "Sort by weight (lungs first)")]
        WeightDesc,
    }
}
