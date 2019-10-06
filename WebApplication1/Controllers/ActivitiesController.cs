using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebApplication1.Models;

namespace identity.Controllers
{
    public class ActivitiesController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetEvents()
        {
            using (ApplicationDbContext dc = new ApplicationDbContext())
            {
                var userid = User.Identity.GetUserId();
                var eventdata = from n in dc.Activities
                                join m in dc.UserActivities on n.Id equals m.ActivityId
                                where m.UserId.Equals(userid)
                                select n;
                var events = eventdata.ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public JsonResult SaveEvent(Activity e)
        {
            var status = false;
            using (ApplicationDbContext dc = new ApplicationDbContext())
            {
                if (e.Id > 0)
                {
                    //Update the event
                    var v = dc.Activities.Where(a => a.Id == e.Id).FirstOrDefault();
                    if (v != null)
                    {
                        v.Subject = e.Subject;
                        v.Start = e.Start;
                        v.End = e.End;
                        v.Description = e.Description;
                        v.IsFullDay = e.IsFullDay;
                        v.ThemeColor = e.ThemeColor;
                    }
                }
                else
                {
                    dc.Activities.Add(e);
                    dc.SaveChanges();
                    var m = new UserActivity();
                    m.ActivityId = e.Id;
                    m.UserId = User.Identity.GetUserId();
                    m.CreatedDate = System.DateTime.Now;
                    dc.UserActivities.Add(m);
                }

                dc.SaveChanges();
                status = true;

            }
            return new JsonResult { Data = new { status = status } };
        }

        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;
            using (ApplicationDbContext dc = new ApplicationDbContext())
            {
                var v = dc.Activities.Where(a => a.Id == eventID).FirstOrDefault();
                if (v != null)
                {
                    dc.Activities.Remove(v);
                    dc.SaveChanges();
                    var m = dc.UserActivities.Where(a => a.ActivityId == eventID).FirstOrDefault();
                    if (m != null)
                    {
                        dc.UserActivities.Remove(m);
                        dc.SaveChanges();
                    }
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }
    }
}