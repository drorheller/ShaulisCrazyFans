using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShaulisCrazyFans.Models;
using System.IO;
using System.Xml.Linq;
using System.Collections;

namespace ShaulisCrazyFans.Controllers
{
    public class FanClubController : Controller
    {
        private CrazyFanDB db = new CrazyFanDB();

        // GET: /FanClub/
        public ActionResult Index()
        {
            return View(db.CrazyFans.ToList());
        }

        // GET: /FanClub/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CrazyFan crazyfan = db.CrazyFans.Find(id);
            if (crazyfan == null)
            {
                return HttpNotFound();
            }
            return View(crazyfan);
        }

        // GET: /FanClub/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /FanClub/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,FirstName,LastName,Gender,Birthday,TimeInClub")] CrazyFan crazyfan)
        {
            if (ModelState.IsValid)
            {
                db.CrazyFans.Add(crazyfan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(crazyfan);
        }

        // GET: /FanClub/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CrazyFan crazyfan = db.CrazyFans.Find(id);
            if (crazyfan == null)
            {
                return HttpNotFound();
            }
            return View(crazyfan);
        }

        // POST: /FanClub/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,FirstName,LastName,Gender,Birthday,TimeInClub")] CrazyFan crazyfan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crazyfan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(crazyfan);
        }

        // GET: /FanClub/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CrazyFan crazyfan = db.CrazyFans.Find(id);
            if (crazyfan == null)
            {
                return HttpNotFound();
            }
            return View(crazyfan);
        }

        // POST: /FanClub/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CrazyFan crazyfan = db.CrazyFans.Find(id);
            db.CrazyFans.Remove(crazyfan);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult FansByAgeView()
        {
            int groupA = 0, groupB = 0, groupC = 0;
            
            foreach (var currFan in db.CrazyFans)
            {
                int nFanAge = DateTime.Now.Year - currFan.Birthday.Year;

                // Group A = 0-20
                if (nFanAge < 20)
                {
                    groupA++;
                }
                // Group B = 20-40
                else if (nFanAge >= 20 && nFanAge < 40)
                {
                    groupB++;
                }
                // Group C = 40+
                else
                {
                    groupC++;
                }
            }

            string data = "age,population\n0-20," + groupA + "\n20-40," + groupB + "\n40+," + groupC;

            ViewBag.GraphData = data;

            return View();
        }

        public ActionResult FansMap()
        {
            List<string> lstFansLocations = new List<string>();

            foreach (var currFan in db.CrazyFans)
            {
                lstFansLocations.Add("CityName" + "," + currFan.FirstName + " " + currFan.LastName);
            }

            //ViewBag.Locations = lstFansLocations.ToArray();
            ViewBag.Locations = new string[] { "Ashkelon,Ori David", "Rishon Letziyon,Dror Heller", "Gan Yavne,Itai Litov" };
            return View();
        }

        public MvcHtmlString GetCoordinatesByCityName(string city)
        {
            string url = "http://maps.google.com/maps/api/geocode/xml?address=" + city;
            string xmlString = GetUrl(url);

            XDocument xd = XDocument.Load(url);
            XElement Coordinates = xd.Element("GeocodeResponse").Element("result").Element("geometry").Element("location");

            return MvcHtmlString.Create(Coordinates.Element("lat").Value + "," + Coordinates.Element("lng").Value);
        }

        private static string GetUrl(string url)
        {
            string result = string.Empty;
            System.Net.WebClient Client = new WebClient();
            using (Stream strm = Client.OpenRead(url))
            {
                StreamReader sr = new StreamReader(strm);
                result = sr.ReadToEnd();
            }

            return result;
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
