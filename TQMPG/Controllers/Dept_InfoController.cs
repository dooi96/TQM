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
    public class Dept_InfoController : Controller
    {
        private TQMEntitiesModel db = new TQMEntitiesModel();

        // GET: Dept_Info
        public ActionResult Index()
        {
            return View(db.Dept_Info.ToList());
        }

        // GET: Dept_Info/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dept_Info dept_Info = db.Dept_Info.Find(id);
            if (dept_Info == null)
            {
                return HttpNotFound();
            }
            return View(dept_Info);
        }

        // GET: Dept_Info/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dept_Info/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AutoID,Department,Dept_Mgr,Dept_Mgr_Email")] Dept_Info dept_Info)
        {
            if (ModelState.IsValid)
            {
                db.Dept_Info.Add(dept_Info);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dept_Info);
        }

        // GET: Dept_Info/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dept_Info dept_Info = db.Dept_Info.Find(id);
            if (dept_Info == null)
            {
                return HttpNotFound();
            }
            return View(dept_Info);
        }

        // POST: Dept_Info/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AutoID,Department,Dept_Mgr,Dept_Mgr_Email")] Dept_Info dept_Info)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dept_Info).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dept_Info);
        }

        // GET: Dept_Info/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dept_Info dept_Info = db.Dept_Info.Find(id);
            if (dept_Info == null)
            {
                return HttpNotFound();
            }
            return View(dept_Info);
        }

        // POST: Dept_Info/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dept_Info dept_Info = db.Dept_Info.Find(id);
            db.Dept_Info.Remove(dept_Info);
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
