using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ShauliProject.DAL;
using ShauliProject.Models;
using System.IO;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using System.Collections;

namespace ShauliProject.Controllers
{
    public class PostController : Controller
    {
        private const string UPLOAD_IMAGES_DIR = "~/Uploads/Images";
        private const string UPLOAD_VIDEOS_DIR = "~/Uploads/Videos";
        private BlogContext db = new BlogContext();

        // GET: Post
        public ActionResult Index()
        {
            LoginController loginController = new LoginController();
            if(loginController.CheckIfUserHaveGuidOnSession(Session))
            {
                ViewBag.Selected = "Manage";
                return View(db.Posts.ToList());
            }


            return RedirectToAction("login", "Login");
        }

        // GET: Post/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }

            var postComments = from cm in db.Comments
                                where cm.PostId == id
                                select cm;
            post.Comments = postComments.ToList<Comment>();
            AddPostViewToStat((int)id);
            return View(post);
        }

        private void AddPostViewToStat(int postId)
        {
            PostStat postStat = db.PostStats.SingleOrDefault(post => post.PostId == postId);

            // If it's the first time the post is viewed
            if(postStat == null)
            {
                db.PostStats.Add(new PostStat { PostId = postId, Counter = 1 });
            }
            else
            {
                postStat.Counter = postStat.Counter + 1;
            }

            db.SaveChanges();
        }

        // GET: Post/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Search(
            DateTime? StartDate, 
            DateTime? EndDate, 
            string PostWriter, 
            string PostTitle,
            string WordsInPosts,
            string PostWriterWebsiteURL,
            int? NumOfComments)
        {
            IEnumerable<Post> posts = db.Posts.Include("Comments").ToList();

            if (StartDate != null)
            {
                posts = posts.Where(p => p.PublishDate >= StartDate);
            }

            if (EndDate != null)
            {
                posts = posts.Where(p => p.PublishDate <= EndDate);
            }

            if (!string.IsNullOrEmpty(PostWriter))
            {
                posts = posts.Where(p => p.Writer.ToUpper().Contains(PostWriter.ToUpper()));
            }

            if (!string.IsNullOrEmpty(PostTitle))
            {
                posts = posts.Where(p => p.Title.ToUpper().Contains(PostTitle.ToUpper()));
            }

            if (!string.IsNullOrEmpty(PostWriterWebsiteURL))
            {
                posts = posts.Where(p => p.WriterWebSiteUrl.ToUpper().Contains(PostWriterWebsiteURL.ToUpper()));
            }

            if (!string.IsNullOrEmpty(WordsInPosts))
            {
                posts = posts.Where(p => p.Content.ToUpper().Contains(WordsInPosts.ToUpper()));
            }

            TempData["Posts"] = posts;

            return RedirectToAction("Index", "Blog");
        }

        // POST: Post/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Writer,WriterWebSiteUrl,PublishDate,Content,Image,Video")] Post post)
        {
            if (ModelState.IsValid)
            {
                var imageFile = Request.Files[nameof(Post.Image)];
                var videoFile = Request.Files[nameof(Post.Video)];
                if (imageFile != null && imageFile.ContentLength > 0 && Request["isPostImage"].Contains(true.ToString().ToLower()))
                {
                    SetImageForPost(post, imageFile);
                }
                else
                {
                    post.Image = null;
                }

                if (videoFile != null && videoFile.ContentLength > 0 && Request["isPostVideo"].Contains(true.ToString().ToLower()))
                {
                    SetVideoForPost(post, videoFile);
                }
                else
                {
                    post.Video = null;
                }

                post.PublishDate = DateTime.Now;
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index", "Blog");
            }

            return View(post);
        }

        private void SetImageForPost(Post Post, HttpPostedFileBase ImageFile)
        {
            var imagesUploadDir = Server.MapPath(UPLOAD_IMAGES_DIR);
            if (!Directory.Exists(imagesUploadDir))
            {
                Directory.CreateDirectory(imagesUploadDir);
            }

            var imagePath = Path.Combine(imagesUploadDir, ImageFile.FileName);
            ImageFile.SaveAs(imagePath);
            Post.ConvertImageFileToPath(imagePath);
        }

        private void SetVideoForPost(Post Post, HttpPostedFileBase VideoFile)
        {
            var videosUploadDir = Server.MapPath(UPLOAD_VIDEOS_DIR);

            if (!Directory.Exists(videosUploadDir))
            {
                Directory.CreateDirectory(videosUploadDir);
            }

            var videoPath = Path.Combine(videosUploadDir, VideoFile.FileName);
            VideoFile.SaveAs(videoPath);
            Post.ConvertVideoFileToPath(videoPath);
        }

        // GET: Post/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }

            //return this.Image;
            return View(post);
        }

        // POST: Post/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Writer,WriterWebSiteUrl,PublishDate,Content,Image,Video")] Post post)
        {
            if (ModelState.IsValid)
            {
                var imageFile = Request.Files[nameof(post.Image)];
                var videoFile = Request.Files[nameof(post.Video)];

                string image = (from p in db.Posts
                                where p.Id == post.Id
                                select p.Image).Single();

                string video = (from p in db.Posts
                                where p.Id == post.Id
                                select p.Video).Single();

                if (!(Request["isPostImage"].Contains(true.ToString().ToLower())))
                {
                    post.Image = null;
                }
                else if (imageFile != null && imageFile.ContentLength > 0)
                {
                    SetImageForPost(post, imageFile);
                }
                else
                {
                    post.Image = image;
                }

                if (!(Request["isPostVideo"].Contains(true.ToString().ToLower())))
                {
                    post.Video = null;
                }
                else if (videoFile != null && videoFile.ContentLength > 0)
                {
                    SetVideoForPost(post, videoFile);
                }
                else
                {
                    post.Video = video;
                }

                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Post/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();

            if (post.Image != null && post.Image != string.Empty && System.IO.File.Exists(post.Image))
            {
                System.IO.File.Delete(post.Image);
            }

            if (post.Video != null && post.Video != string.Empty && System.IO.File.Exists(post.Video))
            {
                System.IO.File.Delete(post.Video);
            }

            return RedirectToAction("Index");
        }

        public ActionResult GetPostStats()
        {
            var stats = (from st in db.PostStats
                                join post in db.Posts
                                on st.PostId equals post.Id
                                select new { Writer = post.Writer, Counter = st.Counter, Title = post.Title }).ToArray();
            return Json(stats, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPostsCountByWriter()
        {
            var posts = (from pt in db.Posts
                         group pt by pt.Writer into grp
                         select new { Writer = grp.Key, Count = grp.Count() }).ToArray();

            return Json(posts, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTopPosts()
        {
            var topPosts = (from st in db.PostStats
                            join post in db.Posts
                            on st.PostId equals post.Id
                            orderby st.Counter descending
                            select new { id = post.Id, title = post.Title }).Take(3).ToArray();

            return Json(topPosts, JsonRequestBehavior.AllowGet);
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
