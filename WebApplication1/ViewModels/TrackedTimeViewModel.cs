using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.ViewModels
{
    public class TrackedTimeViewModel
    {
        public DateTime TrackedDate { get; set; }
        public string WeekName { get; set; }
        public string MonthName { get; set; }
        public double FamilyTime { get; set; }
        public double ScreenTime { get; set; }
        public double ScreenTimeGoal { get; set; }
    }
}