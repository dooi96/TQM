using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TQMPG.Models;

namespace TQMPG.Controllers
{
    public class KPIController : Controller
    {
        private TQMEntitiesModel db = new TQMEntitiesModel();

        // GET: KPI
        public ActionResult Index()
        {
            return View(db.KPIs.ToList());
        }

        // GET: KPI/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KPI kPI = db.KPIs.Find(id);
            if (kPI == null)
            {
                return HttpNotFound();
            }
            return View(kPI);
        }

        // GET: KPI/Create
        public ActionResult Create()
        {

            ViewBag.KPI_DeptID = new SelectList(db.KPI_Dept, "AutoID", "Dept");
            return View();
        }

        // POST: KPI/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AutoID,KPI_DeptID,KPI1")] KPI kPI)
        {
            if (ModelState.IsValid)
            {
                //kPI.KPI_DeptID = Dept;
                db.KPIs.Add(kPI);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kPI);
        }

        // GET: KPI/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KPI kPI = db.KPIs.Find(id);
            if (kPI == null)
            {
                return HttpNotFound();
            }

            return View(kPI);
        }

        // POST: KPI/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AutoID,KPI_DeptID,KPI1")] KPI kPI)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kPI).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kPI);
        }

        // GET: KPI/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KPI kPI = db.KPIs.Find(id);
            if (kPI == null)
            {
                return HttpNotFound();
            }
            return View(kPI);
        }

        // POST: KPI/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KPI kPI = db.KPIs.Find(id);
            db.KPIs.Remove(kPI);
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
