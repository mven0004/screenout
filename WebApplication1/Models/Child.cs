using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Child
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Child Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Screen Time Goal (hour)")]
        public double ScreenTimeGoal { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}