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

        public ActionResult Index()
        {
            Comment comment = new Comment();
            ViewBag.NewComment = comment;
            return View(db.Posts.Include("Comments").ToList());
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
                string json = wc.DownloadString("http://api.openweathermap.org/data/2.5/weather?q=Tel%20Aviv");

                return MvcHtmlString.Create(json);
            }
        }
    }
}