using MVCEmail.Models;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // check if user already logged in
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.IsLoggedIn = true;
            }
            else
            {
                ViewBag.IsLoggedIn = false;
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Credits()
        {
            return View();
        }

        public ActionResult Cause()
        {
            ViewBag.Message = "Your casuse page.";

            return View();
        }

        public ActionResult ScreenTime()
        {
            return View();
        }

        public ActionResult GameTime()
        {
            return View();
        }

        public ActionResult StepsOfSolve()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult QuizTest()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Education()
        {
            return View();
        }

        public ActionResult MediaPlan()
        {
            // check if user already logged in
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.IsLoggedIn = true;
            }
            else
            {
                ViewBag.IsLoggedIn = false;
            }
            return View();
        }

        public ActionResult Plan()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailFormModel model)
        {
            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress(model.FromEmail));
                message.From = new MailAddress("Gaway@team.team");
                message.Subject = "Your email subject";
                message.Body = string.Format(body, model.CustomerName, model.FromEmail,model.PhoneNum, model.Message);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "apikey",// replace with valid value
                        Password = "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"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.sendgrid.net";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("Sent");
                }
            }
            return View(model);
        }

        public ActionResult Sent()
        {
            return View();
        }
    }
}
   