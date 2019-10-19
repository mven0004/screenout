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
using static WebApplication1.Controllers.ManageController;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Dashboard
        public ActionResult Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : "";

            // retrieve tracked data for the login user
            var userId = User.Identity.GetUserId();
            var children = db.Children.Where(child => child.UserId == userId).ToList();

            // check if the login user has any child record
            if (children.Count == 0)
            {
                ViewBag.PageStatus = "NOCHILD";
                return View();
            }

            List<ChildOverviewViewModel> childOverviewList = new List<ChildOverviewViewModel>(children.Count);
            foreach (Child child in children)
            {
                var userIdParam = new SqlParameter("@UserId", userId);
                var childIdParam = new SqlParameter("@ChildId", child.Id);
                var curDateParam = new SqlParameter("@CurDate", DateTime.Now);
                var childOverview = db.Database.SqlQuery<ChildOverviewViewModel>
                    ("GetChildOverviewData @UserId, @ChildId, @CurDate", userIdParam, childIdParam, curDateParam).First();
                childOverview.ChildId = child.Id;
                childOverview.ChildName = child.Name;
                childOverview.ScreenTimeGoal = child.ScreenTimeGoal;

                // add child to the list for displaying in the View
                childOverviewList.Add(childOverview);
            }

            bool noTrackingData = childOverviewList.All(c => c.MetGoalPercentage == -1);
            if (noTrackingData == true)
            {
                ViewBag.PageStatus = "NOTRACKING";
            }
            else
            {
                ViewBag.PageStatus = "WITHTRACKING";
            }

            return View(childOverviewList);
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