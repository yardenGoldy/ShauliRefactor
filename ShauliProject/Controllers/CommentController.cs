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
    public class CommentController : Controller
    {
        private BlogContext db = new BlogContext();

        // GET: Comments
        public ActionResult Index(int? id)
        {
            var comments = (IEnumerable<Comment>)TempData["Comments"] ?? db.Comments.Include(c => c.Post).ToList();
            if (id != null)
            {
                return View(comments.Where(c => c.PostId == id));
            }

            return View(comments);
        }

        [HttpGet]
        public ActionResult Search([Bind(Include = "Title,Writer,Content")] Comment Comment)
        {
            IEnumerable<Comment> Comments = db.Comments.Include(c => c.Post).ToList();

            if (!string.IsNullOrEmpty(Comment.Title))
            {
                Comments = Comments.Where(c => c.Title.ToUpper().Contains(Comment.Title.ToUpper()));
            }

            if (!string.IsNullOrEmpty(Comment.Writer))
            {
                Comments = Comments.Where(c => c.Writer.ToUpper().Contains(Comment.Writer.ToUpper()));
            }

            if (!string.IsNullOrEmpty(Comment.Content))
            {
                Comments = Comments.Where(c => c.Content.ToUpper().Contains(Comment.Content.ToUpper()));
            }

            TempData["Comments"] = Comments;

            return RedirectToAction("Index");
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        public ActionResult Create()
        {
            ViewBag.PostId = new SelectList(db.Posts, "Id", "Title");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PostId,Title,Writer,WriterWebSiteUrl,Content")] Comment comment, int id)
        {
            if (ModelState.IsValid)
            {
                var post = db.Posts.Find(id);
                comment.PostId = post.Id;
                comment.Post = post;
                db.Comments.Add(comment);
                
                if (post.Comments == null)
                {
                    post.Comments = new List<Comment>();
                    post.Comments.Add(comment);
                }
                else
                {
                    post.Comments.Add(comment);
                }

                db.Posts.Attach(post);
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Blog");
            }

            return RedirectToAction("Index", "Blog");
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.PostId = new SelectList(db.Posts, "Id", "Title", comment.PostId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PostId,Title,Writer,WriterWebSiteUrl,Content")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostId = new SelectList(db.Posts, "Id", "Title", comment.PostId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Index", "Post");
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
