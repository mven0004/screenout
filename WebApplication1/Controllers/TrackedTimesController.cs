using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using Microsoft.AspNet.Identity;
using WebApplication1.ViewModels;
using Newtonsoft.Json;
using System.Data.SqlClient;
using PagedList;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class TrackedTimesController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: TrackedTimes/IndexTable
        public ActionResult Index(int? page)
        {
            var userId = User.Identity.GetUserId();
            IQueryable<Child> userChildren = db.Children.Where(child => child.UserId == userId);

            if (userChildren.Count() == 0)
            {
                ViewBag.NoChild = true;
                return View();
            }
            else
            {
                var trackedTimes = db.TrackedTimes
                    .Where(data => userChildren.Any(child => child.Id == data.ChildId))
                    .Include(t => t.Child)
                    .OrderByDescending(data => data.TrackedDate)
                    .AsQueryable();

                int pageSize = 10;
                int pageNumber = (page ?? 1);

                ViewBag.NoChild = false;
                return View(trackedTimes.ToPagedList(pageNumber, pageSize));
            }
        }

        // GET: TrackedTimes/Create
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            var userChildren = db.Children.Where(child => child.UserId == userId).ToList();
            ViewBag.ChildId = new SelectList(userChildren, "Id", "Name");
            ViewBag.ScreenTimeGoal = new SelectList(userChildren, "ScreenTimeGoal", "Id");
            return View();
        }

        // POST: TrackedTimes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ChildId,TrackedDate,FamilyTime,ScreenTime,ScreenTimeGoal")] TrackedTime trackedTime)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var selectedChild = db.Children.Find(trackedTime.ChildId);
                    trackedTime.ScreenTimeGoal = selectedChild.ScreenTimeGoal;
                    db.TrackedTimes.Add(trackedTime);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index", "Children");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Oop! Unable to save data. There is problem with our server. Please try again next time!");
            }

            var userId = User.Identity.GetUserId();
            var userChildren = db.Children.Where(child => child.UserId == userId).ToList();
            ViewBag.ChildId = new SelectList(userChildren, "Id", "Name", trackedTime.ChildId);
            return View(trackedTime);
        }

        // GET: TrackedTimes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrackedTime trackedTime = await db.TrackedTimes.FindAsync(id);
            if (trackedTime == null || trackedTime.Child.UserId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }

            List<Child> children = new List<Child> { trackedTime.Child };
            ViewBag.ChildId = new SelectList(children, "Id", "Name", trackedTime.ChildId);
            return View(trackedTime);
        }

        // POST: TrackedTimes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TrackedTime trackedTime)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trackedTime).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            List<Child> userChildren = new List<Child> { trackedTime.Child };
            ViewBag.ChildId = new SelectList(userChildren, "Id", "Name", trackedTime.ChildId);
            return View(trackedTime);
        }

        // GET: TrackedTimes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrackedTime trackedTime = await db.TrackedTimes.FindAsync(id);
            if (trackedTime == null || trackedTime.Child.UserId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }
            return View(trackedTime);
        }

        // POST: TrackedTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TrackedTime trackedTime = new TrackedTime { Id = id };
            db.Entry(trackedTime).State = EntityState.Deleted;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
