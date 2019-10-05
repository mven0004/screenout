using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.ViewModels
{
    public class OverviewViewModel
    {
        public int ChildCount { get; set; }
        public double AvgFamilyTime { get; set; }
        public double AvgScreenTime { get; set; }

        private string GetHourMinuteString(double value)
        {
            var hour = Math.Floor(value);
            var minute = Math.Round((value - hour) * 60);
            if (minute == 0)
                return $"{hour}h";

            return $"{hour}h {minute}min";
        }

        public string GetAvgFamilyTimeString()
        {
            return GetHourMinuteString(AvgFamilyTime);
        }

        public string GetAvgScreenTimeString()
        {
            return GetHourMinuteString(AvgScreenTime);
        }
    }
}