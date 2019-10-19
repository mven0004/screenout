using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.ViewModels
{
    public class ChildOverviewViewModel
    {
        public int ChildId { get; set; }
        public string ChildName { get; set; }
        public double AvgFamilyTime { get; set; }
        public double AvgScreenTime { get; set; }
        public double CurAvgFamilyTime { get; set; }
        public double CurAvgScreenTime { get; set; }
        public double PreAvgFamilyTime { get; set; }
        public double PreAvgScreenTime { get; set; }
        public double MoMAvgFamilyTime { get; set; }
        public double MoMAvgScreenTime { get; set; }
        public double MetGoalPercentage { get; set; }
        public double ScreenTimeGoal { get; set; }
        public DateTime CurDate { get; set; }
        public DateTime CurStartDate { get; set; }
        public DateTime PreDate { get; set; }
        public DateTime PreStartDate { get; set; }

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
            if (AvgFamilyTime == -1)
            {
                return "No Data";
            }
            return GetHourMinuteString(AvgFamilyTime);
        }

        public string GetAvgScreenTimeString()
        {
            if (AvgScreenTime == -1)
            {
                return "No Data";
            }
            return GetHourMinuteString(AvgScreenTime);
        }

        public string GetCurAvgFamilyTimeString()
        {
            if (CurAvgFamilyTime == -1)
            {
                return "No Data";
            }
            return GetHourMinuteString(CurAvgFamilyTime);
        }

        public string GetCurAvgScreenTimeString()
        {
            if (CurAvgScreenTime == -1)
            {
                return "No Data";
            }
            return GetHourMinuteString(CurAvgScreenTime);
        }

        public string GetPreAvgFamilyTimeString()
        {
            if (PreAvgFamilyTime == -1)
            {
                return "No Data";
            }
            return GetHourMinuteString(PreAvgFamilyTime);
        }

        public string GetPreAvgScreenTimeString()
        {
            if (PreAvgScreenTime == -1)
            {
                return "No Data";
            }
            return GetHourMinuteString(PreAvgScreenTime);
        }

        public string GetScreenTimeGoalString()
        {
            return GetHourMinuteString(ScreenTimeGoal);
        }

        public string GetGoalPercentString()
        {
            if (MetGoalPercentage == -1)
            {
                return "No Data";
            }
            return MetGoalPercentage.ToString("p0");
        }

        public string GetMoMAvgFamilyTimeString()
        {
            if (PreAvgFamilyTime == -1)
            {
                return "No Data";
            }
            return MoMAvgFamilyTime.ToString("p0");
        }

        public string GetMoMAvgScreenTimeString()
        {
            if (PreAvgScreenTime == -1)
            {
                return "No Data";
            }
            return MoMAvgScreenTime.ToString("p0");
        }
    }
}