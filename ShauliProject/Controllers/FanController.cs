using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShauliProject.DAL;
using ShauliProject.Models;

namespace ShauliProject.Controllers
{
    public class FanController : Controller
    {
        private BlogContext db = new BlogContext();

        // GET: Fan
        public ActionResult Index()
        {
            ViewBag.Selected = "Fan";

            IEnumerable<Fan> fans = (IEnumerable<Fan>)TempData["Fans"] ?? db.Fans.ToList();

            return View(fans);
        }

        public ActionResult FindPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            IEnumerable<Post> Posts = db.Posts.ToList();

            Fan fan = db.Fans.Find(id);
            if (fan == null)
            {
                return HttpNotFound();
            }

            if (!string.IsNullOrEmpty(fan.FirstName))
            {
                Posts = (from p in db.Posts join f in db.Fans on p.Writer equals (f.FirstName + " " + f.LastName) where f.Id == id  select p);
            }
            return View(Posts);
        }

        // GET: Fan/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fan fan = db.Fans.Find(id);
            if (fan == null)
            {
                return HttpNotFound();
            }
            return View(fan);
        }

        // GET: Fan/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Fan/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Gender,DateOfBirth,YearsOfSeniority,Address")] Fan fan)
        {
            if (ModelState.IsValid)
            {
                db.Fans.Add(fan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fan);
        }

        [HttpGet]
        public ActionResult Search([Bind(Include = "FirstName,LastName,DateOfBirth,Gender,Address")] Fan fan)
        {
            IEnumerable<Fan> fans = db.Fans.ToList();

            if (!string.IsNullOrEmpty(fan.FirstName))
            {
                fans = fans.Where(f => f.FirstName.ToUpper().Contains(fan.FirstName.ToUpper()));
            }

            if (!string.IsNullOrEmpty(fan.LastName))
            {
                fans = fans.Where(f => f.LastName.ToUpper().Contains(fan.LastName.ToUpper()));
            }

            if (fan.DateOfBirth != null)
            {
                fans = fans.Where(f => f.DateOfBirth == fan.DateOfBirth);
            }

            if (fan.Gender != null)
            {
                fans = fans.Where(f => f.Gender == fan.Gender);
            }

            if (!string.IsNullOrEmpty(fan.Address))
            {
                fans = fans.Where(f => f.Address.ToUpper().Contains(fan.Address.ToUpper()));
            }

            TempData["Fans"] = fans;

            return RedirectToAction("Index", "Fan");
        }

        // GET: Fan/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fan fan = db.Fans.Find(id);
            if (fan == null)
            {
                return HttpNotFound();
            }
            return View(fan);
        }

        // POST: Fan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Gender,DateOfBirth,YearsOfSeniority,Address")] Fan fan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fan);
        }

        // GET: Fan/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fan fan = db.Fans.Find(id);
            if (fan == null)
            {
                return HttpNotFound();
            }
            return View(fan);
        }

        // POST: Fan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fan fan = db.Fans.Find(id);
            db.Fans.Remove(fan);
            db.SaveChanges();
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
