using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class ChildrenController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Children
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            var children = db.Children.Where(child => child.UserId == userId).ToList();
            return View(children);
        }

        // GET: Children/Create
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            ViewBag.UserId = userId;
            return View();
        }

        // POST: Children/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name,ScreenTimeGoal,UserId")]Child child)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    child.UserId = User.Identity.GetUserId();
                    db.Children.Add(child);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Oop! Unable to save data. There is problem with our server. Please try again next time!");
            }
            return View(child);
        }

        // GET: Children/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userId = User.Identity.GetUserId();
            Child child = await db.Children.FindAsync(id);

            if (child == null || child.UserId != userId)
            {
                return HttpNotFound();
            }
            return View(child);
        }

        // POST: Children/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Child child)
        {
            if (ModelState.IsValid)
            {
                db.Entry(child).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(child);
        }

        // GET: Children/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // User can delete their own child record only.
            var userId = User.Identity.GetUserId();
            Child child = await db.Children.FindAsync(id);
            if (child == null || child.UserId != userId)
            {
                return HttpNotFound();
            }
            return View(child);
        }

        // POST: Children/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Child child = new Child { Id = id };
            db.Entry(child).State = EntityState.Deleted;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public ActionResult Progress(int? id)
        {
            // I need:
            //  AvgFamilyTime
            //  AvgScreenTime
            //  ScreenTimeGoal
            //  Percentage Met Goal = Count(screentime under goal) / Count(all)

            // Diplay graphs:
            //  TrackedFamilyTime - all rows
            //      >> Line graph: Date and FamilyTime on that date
            //      >> Histogram: Family Time - binsize (bucket size)
            //  ScreenTime - all rows
            //      >> Line graph: Date, ScreenTime and ScreenTimeGoal
            //      >> Histogram: Screen Time - Binsize (bucket size)
            //  Weekday vs Weekend - FamilyTime vs ScreenTime
            //  Monthly - FamilyTime vs ScreenTime
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = User.Identity.GetUserId();
            Child child = db.Children.Find(id);
            if (child == null || child.UserId != userId)
            {
                return HttpNotFound();
            }

            // retrieve tracked data for the login user
            var userIdParam = new SqlParameter("@UserId", userId);
            var childIdParam = new SqlParameter("@ChildId", child.Id);
            var childOverview = db.Database.SqlQuery<ChildOverviewViewModel>
                ("GetChildOverviewData @UserId, @ChildId", userIdParam, childIdParam).First();

            childOverview.ChildId = child.Id;
            childOverview.ChildName = child.Name;
            childOverview.ScreenTimeGoal = child.ScreenTimeGoal;

            // check if there is any tracked data for this child
            if (childOverview.AvgFamilyTime == -1)
            {
                ViewBag.PageStatus = "NOTRACKING";
                childOverview.AvgFamilyTime = 0;
                childOverview.AvgScreenTime = 0;
                childOverview.MetGoalPercent = 0;
                return View(childOverview);
            }
            else
            {
                ViewBag.PageStatus = "WITHTRACKING";
                return View(childOverview);
            }
        }

        public ActionResult GetOverviewData(int id)
        {
            // retrieve tracked data for the login user
            var userId = User.Identity.GetUserId();
            var userIdParam = new SqlParameter("@UserId", userId);
            var childIdParam = new SqlParameter("@ChildId", id);
            var childOverview = db.Database.SqlQuery<ChildOverviewViewModel>
                ("GetChildOverviewData @UserId, @ChildId", userIdParam, childIdParam).First();

            return Content(JsonConvert.SerializeObject(childOverview), "application/json");
        }
        public ActionResult GetTrackedData(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = User.Identity.GetUserId();
            Child child = db.Children.Find(id);
            if (child == null || child.User.Id != userId)
            {
                return HttpNotFound();
            }

            // retrieve tracked data for the login user
            var userIdParam = new SqlParameter("@UserId", userId);
            var childIdParam = new SqlParameter("@ChildId", child.Id);
            var trackedData = db.Database.SqlQuery<TrackedTimeViewModel>
                ("GetChildTrackedData @UserId, @ChildId", userIdParam, childIdParam).ToList();

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
                TrackedData = trackedData,
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