using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TQMPG.Models;

namespace TQMPG.Controllers
{
    public class KPI_DeptController : Controller
    {
        private TQMEntitiesModel db = new TQMEntitiesModel();

        // GET: KPI_Dept
        public ActionResult Index()
        {
            return View(db.KPI_Dept.ToList());
        }

        // GET: KPI_Dept/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KPI_Dept kPI_Dept = db.KPI_Dept.Find(id);
            if (kPI_Dept == null)
            {
                return HttpNotFound();
            }
            return View(kPI_Dept);
        }

        // GET: KPI_Dept/Create
        public ActionResult Create()
        {
            eLeaveEntities eleave = new eLeaveEntities();



            ViewBag.Dept_Name = new SelectList(eleave.v_eLeave_EmployeeDetails, "preDeptDesc", "preDeptDesc");

            ViewBag.Dept_Mgr = new SelectList(eleave.v_eLeave_EmployeeDetails, "empFullName", "empFullName");
            return View();
        }

        // POST: KPI_Dept/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AutoID,Dept,Dept_Mgr,Dept_Mgr_Email")] KPI_Dept kPI_Dept)
        {
            if (ModelState.IsValid)
            {
                db.KPI_Dept.Add(kPI_Dept);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kPI_Dept);
        }





        // GET: KPI_Dept/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KPI_Dept kPI_Dept = db.KPI_Dept.Find(id);
            if (kPI_Dept == null)
            {
                return HttpNotFound();
            }
            return View(kPI_Dept);
        }

        // POST: KPI_Dept/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AutoID,Dept,Dept_Mgr,Dept_Mgr_Email,Dept_Mgr_EmpID")] KPI_Dept kPI_Dept)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kPI_Dept).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kPI_Dept);
        }


        public ActionResult getMgrDetail()
        {

            var mgrName = Request.QueryString["Dept_Mgr"];

            eLeaveEntities eleave = new eLeaveEntities();

            var queryEmail = from x in eleave.v_eLeave_EmployeeDetails
                             where x.empFullName == mgrName
                             select new { x.empEmail, x.empEmployeeNo };

           

            var serialize = new JavaScriptSerializer();

            var result = serialize.Serialize(queryEmail);

            return Content(result);
        }

        // GET: KPI_Dept/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KPI_Dept kPI_Dept = db.KPI_Dept.Find(id);
            if (kPI_Dept == null)
            {
                return HttpNotFound();
            }
            return View(kPI_Dept);
        }

        // POST: KPI_Dept/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KPI_Dept kPI_Dept = db.KPI_Dept.Find(id);
            db.KPI_Dept.Remove(kPI_Dept);
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
