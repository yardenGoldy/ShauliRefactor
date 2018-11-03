using ShauliProject.DAL;
using ShauliProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShauliProject.Controllers
{
    public class LoginController : Controller
    {
        private BlogContext db = new BlogContext();
        
        public bool CheckIfUserHaveGuidOnSession(HttpSessionStateBase session)
        {
            Guid ValidatedGuid;
            bool IsValid = false;
            if (session != null && session["user"] != null && session["GUID"] != null)
            {
                IsValid = Guid.TryParse(session["GUID"].ToString(), out ValidatedGuid);
            }

            return IsValid;
        }

        [HttpGet]
        public bool IsUserManager()
        {
            bool IsUserManager = CheckIfUserHaveGuidOnSession(Session);
            TempData["IsUserManager"] = IsUserManager;
            return IsUserManager;
        }

        // GET: Login
        public ActionResult login()
        {
            if (CheckIfUserHaveGuidOnSession(Session))
            {
                return Redirect(Request.UrlReferrer.ToString());
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult login(Admin admin)
        {
            if (ModelState.IsValid)
            {
                var v = db.Admins.Where(a => a.Username.Equals(admin.Username) && a.Password.Equals(admin.Password)).FirstOrDefault();
                if (v != null)
                {
                    Session["user"] = v.Username.ToString();
                    Session["GUID"] = Guid.NewGuid().ToString();
                   
                    return RedirectToAction("Index", "Post");
                }
            }

            return View(admin);
        }
    }
}
