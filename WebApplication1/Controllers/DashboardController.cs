using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Dashboard
        public ActionResult Index()
        {
            // retrieve tracked data for the login user
            var userId = User.Identity.GetUserId();
            var userIdParam = new SqlParameter("@UserId", userId);
            var overview = db.Database.SqlQuery<OverviewViewModel>("GetOverviewData @UserId", userIdParam).First();

            // check if the login user has any child record
            if (overview.ChildCount == 0)
            {
                ViewBag.PageStatus = "NOCHILD";
                return View();
            }
            else if (overview.AvgFamilyTime == -1)
            {
                ViewBag.PageStatus = "NOTRACKING";
                overview.AvgFamilyTime = 0;
                overview.AvgScreenTime = 0;
                return View(overview);
            }
            else
            {
                ViewBag.PageStatus = "WITHTRACKING";
                return View(overview);
            }
        }

        public ActionResult GetTrackedDataSummary()
        {
            // retrieve tracked data for this login user
            // we created a stored procedure on SQL Server for this task.
            var userId = new SqlParameter("@UserId", User.Identity.GetUserId());
            var trackedData = db.Database.SqlQuery<TrackedTimeViewModel>("GetTrackedData @UserId", userId).ToList();

            // we grouped the tracked data by WeekName and get Avg Family and Screen Time
            var weekSummary = trackedData
                .GroupBy(d => d.WeekName)
                .Select(g => new
                {
                    WeekName = g.Key,
                    AvgFamilyTime = g.Average(d => d.FamilyTime),
                    AvgScreenTime = g.Average(d => d.ScreenTime)
                })
                .OrderBy(d => d.WeekName);

            // we grouped the tracked data by MonthName and get Avg Family and Screen Time
            var monthSummary = trackedData
                .GroupBy(d => d.MonthName)
                .Select(g => new
                {
                    MonthName = g.Key,
                    AvgFamilyTime = g.Average(d => d.FamilyTime),
                    AvgScreenTime = g.Average(d => d.ScreenTime)
                });

            // we create anonymous object to be returned as Json file
            var result = new
            {
                WeekSummary = weekSummary,
                MonthSummary = monthSummary
            };

            // return the Json file. This will be read by ajax call.
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}