using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShaulisCrazyFans.Models;
using System.Xml.Linq;
using System.IO;
using System.Net;

namespace ShaulisCrazyFans.Controllers
{
    public class HomeController : Controller
    {
        private CrazyFanDB db = new CrazyFanDB();
        private static Func<Post, bool> filter = (p => true);
        private static bool newFilter = false;

        public ActionResult Index()
        {
            if (!newFilter)
                filter = (p => true);
            newFilter = false;
            Comment comment = new Comment();
            ViewBag.NewComment = comment;
            return View(db.Posts.Include("Comments").ToList().Where(filter).ToList());
        }

        public ActionResult Search(DateTime? filter_date_start, DateTime? filter_date_end, string filter_writer, string filter_title, string filter_keywords, int? filter_min_comments)
        {
            filter = (p => (
                      (filter_date_start == null || (filter_date_start != null && filter_date_start <= p.ReleaseDate)) &&
                      (filter_date_end == null || (filter_date_end != null && filter_date_end >= p.ReleaseDate)) &&
                      (filter_writer == "" || (filter_writer != "" && filter_writer == p.Author)) &&
                      (filter_title == "" || (filter_title != "" && filter_title == p.Title)) &&
                      (filter_keywords == "" || (filter_keywords != "" && p.Content.Contains(filter_keywords))) &&
                      (filter_min_comments == null || (filter_min_comments != null && filter_min_comments <= p.Comments.Count)) ));
            newFilter = true;
            return RedirectToAction("Index");
        }

        public ActionResult Statistics()
        {
            return View();
        }

        // POST: /Comment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Prefix="c", Include = "Id,PostId,Title,Author,AuthorSite,Content")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        public MvcHtmlString GetWeatherReport()
        {
            using (WebClient wc = new WebClient())
            {
                string json = wc.DownloadString("http://api.openweathermap.org/data/2.5/weather?q=Tel%20Aviv&appid=bd82977b86bf27fb59a04b61b657fb6f");

                return MvcHtmlString.Create(json);
            }
        }
    }
}