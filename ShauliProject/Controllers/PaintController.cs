using ShauliProject.DAL;
using System.Linq;
using System.Web.Mvc;

namespace ShauliProject.Controllers
{
    public class PaintController : Controller
    {
        private BlogContext db = new BlogContext();

        public ActionResult Index()
        {
            ViewBag.Selected = "Paint";
            return View();
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