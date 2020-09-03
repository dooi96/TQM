using Newtonsoft.Json.Linq;
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
    public class ProjectCharterController : Controller
    {
        private TQMEntitiesModel db = new TQMEntitiesModel();
        private eLeaveEntities eleave = new eLeaveEntities();

        // GET: ProjectCharter
        public ActionResult Index()
        {
            return View(db.Project_Charter.ToList());
        }

        // GET: ProjectCharter/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project_Charter project_Charter = db.Project_Charter.Find(id);
            if (project_Charter == null)
            {
                return HttpNotFound();
            }
            return View(project_Charter);
        }

        // GET: ProjectCharter/Create
        public ActionResult Create(int id)
        {

            Project_Charter project_Charter = new Project_Charter();

            project_Charter.TQMID = id;

            ViewBag.MethodologyID = new SelectList(db.TQM_Methodology, "AutoID", "MethodologyDescription");

            ViewBag.FinanceName = new SelectList(db.TQM_Officials.Where(x=>x.RoleID == 1), "EmpName", "EmpName");

            ViewBag.EmployeeName = new SelectList(eleave.v_eLeave_EmployeeDetails, "empFullName", "empFullName");

            ViewBag.ChampionSelection = new SelectList(eleave.v_eLeave_EmployeeDetails, "empFullName", "empFullName");



            return View(project_Charter);
        }




        // POST: ProjectCharter/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TQMID,Business_Case,Opportunity,Saving,Goal,Scope,Plan,MethodologyID,CurrentStep")] Project_Charter project_Charter)
        {
            if (ModelState.IsValid)
            {
                db.Project_Charter.Add(project_Charter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project_Charter);
        }

        // GET: ProjectCharter/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project_Charter project_Charter = db.Project_Charter.Find(id);
            if (project_Charter == null)
            {
                return HttpNotFound();
            }
            return View(project_Charter);
        }

        // POST: ProjectCharter/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TQMID,Business_Case,Opportunity,Saving,Goal,Scope,Plan,MethodologyID,CurrentStep")] Project_Charter project_Charter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project_Charter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project_Charter);
        }

        // GET: ProjectCharter/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project_Charter project_Charter = db.Project_Charter.Find(id);
            if (project_Charter == null)
            {
                return HttpNotFound();
            }
            return View(project_Charter);
        }

        public ActionResult getFinanceDetail()
        {

            var empName = Request.QueryString["financeName"];

            var query = db.TQM_Officials
    .Where(x => x.EmpName == empName)
    .Select(x => new { x.EmpEmail, x.EmpNo });

            var serialize = new JavaScriptSerializer();

            var result = serialize.Serialize(query);

            return Content(result);
        }

        public ActionResult getEmpDetails()
        {

            var empEmail = Request.QueryString["empName"];


            eLeaveEntities eleave = new eLeaveEntities();
            var query = from x in eleave.v_eLeave_EmployeeDetails
                        where x.empFullName == empEmail
                        select x;

            var serialize = new JavaScriptSerializer();

            var result = serialize.Serialize(query);

            return Content(result);
        }

        public JToken MethodologyStep()
        {
            var methodologyID = Int32.Parse(Request.QueryString["methodologyID"]);

            var methodName = db.TQM_Methodology.Where(x => x.AutoID == methodologyID).FirstOrDefault();
            var list = db.TQM_MethodologyStep.Where(x => x.MethodologyID == methodologyID).ToArray();

            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("MethodSteps", list);

            var json = JToken.FromObject(dict);
            json["MethodologyName"] = methodName.MethodologyDescription;

            //json.Add("MethodologyName", methodName.MethodologyName);

            return json;
        }

        public ActionResult GetEmpName(string empNo)
        {

            eLeaveEntities eLeave = new eLeaveEntities();

            var query = from x in eLeave.v_eLeave_EmployeeDetails
                        where x.empEmployeeNo == empNo
                        select x.empFullName;

            var serialize = new JavaScriptSerializer();

            var result = serialize.Serialize(query);

            return Content(result);
        }

        public ActionResult getSponsorDetails()
        {

            var serialNo = Int32.Parse(Request.QueryString["tqmid"]);

            var query = from x in db.TQM_Details

                        join d in db.KPI_Dept

                        on x.KPI_Dept equals d.AutoID into test

                        from d in test.DefaultIfEmpty()

                        where x.SerialNo == serialNo

                        select new { x.Dept, d.Dept_Name,x.KPI_Dept, x.SerialNo };


            var serialize = new JavaScriptSerializer();

            var result = serialize.Serialize(query);


            return Content(result);
        }

        public ActionResult getDeptSponsor()
        {

            var departmentName = Request.QueryString["deptName"];

            var query = from x in db.KPI_Dept

                        where x.Dept_Name == departmentName

                        select new { x.Dept_Name, x.Dept_Mgr_Email, x.Dept_Mgr, x.Dept_Mgr_EmpID };

            var serialize = new JavaScriptSerializer();

            var result = serialize.Serialize(query);



            return Content(result);
        }


        public ActionResult getBothDeptSponsor()
        {
            var ownDeptName = Request.QueryString["deptName1"];
            var kpiDeptName = Request.QueryString["deptName2"];

            var query = from x in db.KPI_Dept

                        where x.Dept_Name == ownDeptName || x.Dept_Name == kpiDeptName

                        select new { x.Dept_Name, x.Dept_Mgr_Email, x.Dept_Mgr, x.Dept_Mgr_EmpID };

            var serialize = new JavaScriptSerializer();

            var result = serialize.Serialize(query);

            return Content(result);
        }



        // POST: ProjectCharter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project_Charter project_Charter = db.Project_Charter.Find(id);
            db.Project_Charter.Remove(project_Charter);
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