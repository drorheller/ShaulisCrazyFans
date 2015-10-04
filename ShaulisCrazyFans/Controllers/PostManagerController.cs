using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShaulisCrazyFans.Models;
using ShaulisCrazyFans.Helpers;

namespace ShaulisCrazyFans.Controllers
{
    public class PostManagerController : Controller
    {
        private CrazyFanDB db = new CrazyFanDB();

        // GET: /PoastManager/
        [AdminMembership]
        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string user, string pass)
        {
            if (user == "admin" && pass == "1234")
            {
                Session["Admin-Authentication"] = "true";
                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session["Admin-Authentication"] = null;
            Session.Remove("Admin-Authentication");
            return RedirectToAction("Index");
        }

        // GET: /PoastManager/Details/5
        [AdminMembership]
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
            return View(post);
        }

        // GET: /PoastManager/Create
        [AdminMembership]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /PoastManager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AdminMembership]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Author,AuthorSite,ReleaseDate,Content")] Post post, HttpPostedFileBase photo, HttpPostedFileBase video)
        {
            if (ModelState.IsValid)
            {
                db.Posts.Add(post);
                db.SaveChanges();
                if (photo != null && photo.ContentLength > 0) photo.SaveAs(Server.MapPath("~/Uploads/" + post.Id + ".png"));
                if (video != null && video.ContentLength > 0) video.SaveAs(Server.MapPath("~/Uploads/" + post.Id + ".mp4"));
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: /PoastManager/Edit/5
        [AdminMembership]
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
            return View(post);
        }

        // POST: /PoastManager/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AdminMembership]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Author,AuthorSite,ReleaseDate,Content")] Post post, HttpPostedFileBase photo, HttpPostedFileBase video)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                if (photo != null && photo.ContentLength > 0) photo.SaveAs(Server.MapPath("~/Uploads/" + post.Id + ".png"));
                if (video != null && video.ContentLength > 0) video.SaveAs(Server.MapPath("~/Uploads/" + post.Id + ".mp4"));
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: /PoastManager/Delete/5
        [AdminMembership]
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

        // POST: /PoastManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [AdminMembership]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            if (System.IO.File.Exists(Server.MapPath("~/Uploads/" + id + ".png")))
                System.IO.File.Delete(Server.MapPath("~/Uploads/" + id + ".png"));
            if (System.IO.File.Exists(Server.MapPath("~/Uploads/" + id + ".mp4")))
                System.IO.File.Delete(Server.MapPath("~/Uploads/" + id + ".mp4"));
            
            return RedirectToAction("Index");
        }

        public ActionResult PostsByMonthView()
        {
            string[] arrMonths = new string[] { "Jan.", "Feb.", "Mar.", "Apr.", "May", "June", "July", "Aug.", "Sept.", "Oct.", "Nov.", "Dec." };
            int[] arrPostsPerMonth = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    
            foreach (var currPost in db.Posts)
            {
                arrPostsPerMonth[currPost.ReleaseDate.Month - 1]++;
            }

            ViewBag.Months = arrMonths;
            ViewBag.PostsPerMonth = arrPostsPerMonth;

            return View();
        }

        public PartialViewResult SearchPosts(string keyword)
        {
            // System.Threading.Thread.Sleep(2000);
            var data = db.Posts.Where(f => f.Title.Contains(keyword) || f.Author.Contains(keyword)).ToList();
            return PartialView("_SearchPostsPArtial", data);
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
