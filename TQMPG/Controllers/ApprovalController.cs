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
    public class ApprovalController : Controller
    {
        private TQMEntitiesModel db = new TQMEntitiesModel();

        // GET: Approval
        public ActionResult Index()
        {
            return View(db.TQM_Details.Where(x => x.Status == "PreDH Approval").ToList());
        }

        // GET: Approval/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TQM_Details tQM_Details = db.TQM_Details.Find(id);
            if (tQM_Details == null)
            {
                return HttpNotFound();
            }
            return View(tQM_Details);
        }

        public ActionResult Approve(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TQM_Details tQM_Details = db.TQM_Details.Find(id);
            if (tQM_Details == null)
            {
                return HttpNotFound();
            }
            return View(tQM_Details);
        }

        public ActionResult Reject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TQM_Details tQM_Details = db.TQM_Details.Find(id);
            if (tQM_Details == null)
            {
                return HttpNotFound();
            }
            return View(tQM_Details);
        }

        // GET: Approval/Create
        public ActionResult Create()
        {
        
            return View();
        }

        // POST: Approval/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AutoID,SerialNo,EmpNo,EmpName,Dept,Idea,KPI_Dept,KPI,SynergyModel,Status,TransactDate")] TQM_Details tQM_Details)
        {
            if (ModelState.IsValid)
            {
                db.TQM_Details.Add(tQM_Details);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tQM_Details);
        }

        public ActionResult ApprovalDetail()
        {

            //System.Diagnostics.Debug.WriteLine(Request.Form);
            if (ModelState.IsValid)
            {
                TQM_Approval tqm_Approval = new TQM_Approval();

                tqm_Approval.ApprovalStatus = "A";
                tqm_Approval.TQMID = Int32.Parse(Request.Form["tqmID"]);
                tqm_Approval.EmpNo = Int32.Parse(Request.Form["empNo"]);
                tqm_Approval.ApprovalComment = Request.Form["approvalComment"];
                tqm_Approval.ApprovalDatetime = DateTime.Now;
                tqm_Approval.ApprovalStep = '2';

                TQM_Details tQM_Details = db.TQM_Details.Find(Int32.Parse(Request.Form["tqmID"]));

                tQM_Details.Status = "DH PreApproved";

                db.Entry(tQM_Details).State = EntityState.Modified;

                db.TQM_Approval.Add(tqm_Approval);
                db.SaveChanges();





                return RedirectToAction("Index");
            }

            return View();
        }
        // GET: Approval/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TQM_Details tQM_Details = db.TQM_Details.Find(id);
            if (tQM_Details == null)
            {
                return HttpNotFound();
            }
            return View(tQM_Details);
        }

        // POST: Approval/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AutoID,SerialNo,EmpNo,EmpName,Dept,Idea,KPI_Dept,KPI,SynergyModel,Status,TransactDate")] TQM_Details tQM_Details)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tQM_Details).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tQM_Details);
        }

        // GET: Approval/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TQM_Details tQM_Details = db.TQM_Details.Find(id);
            if (tQM_Details == null)
            {
                return HttpNotFound();
            }
            return View(tQM_Details);
        }

        // POST: Approval/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TQM_Details tQM_Details = db.TQM_Details.Find(id);
            db.TQM_Details.Remove(tQM_Details);
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
