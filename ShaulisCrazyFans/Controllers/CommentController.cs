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
    public class CommentController : Controller
    {
        private CrazyFanDB db = new CrazyFanDB();

        // GET: /Comment/
        [AdminMembership]
        public ActionResult Index(int id)
        {
            var comments = db.Comments.Include(c => c.Post);
            return View(comments.ToList().Where(c => c.PostId == id));
        }

        // GET: /Comment/Details/5
        [AdminMembership]
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

        // GET: /Comment/Create
        public ActionResult Create(int id)
        {
           // ViewBag.PostId = new SelectList(db.Posts, "Id", "Title");
            ViewBag.PostId = id;
            return PartialView();
        }

        // POST: /Comment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,PostId,Title,Author,AuthorSite,Content")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                ViewBag.PostId = comment.PostId;
                return RedirectToAction("Index", new { id = comment.PostId });
            }

            ViewBag.PostId = comment.PostId;//new SelectList(db.Posts, "Id", "Title", comment.PostId);
            return View(comment);
        }

        // GET: /Comment/Edit/5
        [AdminMembership]
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

        // POST: /Comment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AdminMembership]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,PostId,Title,Author,AuthorSite,Content")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = comment.PostId });
            }
            ViewBag.PostId = new SelectList(db.Posts, "Id", "Title", comment.PostId);
            return View(comment);
        }

        // GET: /Comment/Delete/5
        [AdminMembership]
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

        // POST: /Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        [AdminMembership]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = comment.PostId });
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
