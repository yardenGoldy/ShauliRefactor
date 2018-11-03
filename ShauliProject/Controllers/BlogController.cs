using ShauliProject.DAL;
using ShauliProject.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ShauliProject.Controllers
{
    public class BlogController : Controller
    {
        private BlogContext db = new BlogContext();

        public ActionResult Index()
        {
            ViewBag.Selected = "Blog";

            IEnumerable<Post> postsToShow = (IEnumerable<Post>)TempData["Posts"] ?? db.Posts.Include("Comments").ToList();

            return View(postsToShow);
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