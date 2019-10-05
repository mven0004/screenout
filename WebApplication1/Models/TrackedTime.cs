using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class TrackedTime
    {
        public int Id { get; set; }

        public int ChildId { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public DateTime TrackedDate { get; set; }

        [Required]
        [Display(Name = "Family Time")]
        public double FamilyTime { get; set; }

        [Required]
        [Display(Name = "Screen Time")]
        public double ScreenTime { get; set; }

        [Required]
        [Display(Name = "Screen Time Goal")]
        public double ScreenTimeGoal { get; set; }

        public virtual Child Child { get; set; }
    }
}