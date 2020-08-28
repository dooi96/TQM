using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TQMPG.Models;

namespace TQMPG.Controllers
{
    public class StorySubmitController : Controller
    {
        private TQMEntitiesModel db = new TQMEntitiesModel();

        // GET: StorySubmit
        public ActionResult Index()
        {
            return View(db.TQM_Details.ToList());
        }

        // GET: StorySubmit/Details/5
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

        // GET: StorySubmit/Create
        public ActionResult Create()
        {
            ViewBag.KPI_Dept = new SelectList(db.KPI_Dept, "AutoID", "Dept");
            ViewBag.KPI = new SelectList(db.KPIs, "KPI_DeptID", "KPI1");
            return View();
        }

        // POST: StorySubmit/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "SerialNo,EmpNo,EmpName,Dept,Idea,KPI_Dept, KPI,file,Status,TransactDate")] TQM_Details tQM_Details, HttpPostedFileBase file)
        {

            //System.Diagnostics.Debug.WriteLine(Request.Form);
            //Response.Write(Request.Form);
            if (ModelState.IsValid)
            {
               
              

                

                if (file == null)
                {
                    var Html = "<script> alert('" + file + " Did not upload.'); window.location.href = 'Create'</script> ";
                    return Content(Html);
                }
                else if (file.ContentLength > 0)
                {

                    int MaxContentLength = 1024 * 1024 * 3; //3 MB
                    string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".pdf" };

                    if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                    {
                        ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
                    }

                    else if (file.ContentLength > MaxContentLength)
                    {
                        ModelState.AddModelError("File", "Your file is too large, maximum allowed size is: " + MaxContentLength + " MB");
                    }
                    else
                    {
                        //TO:DO
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/SynergyModels"), fileName);
                        file.SaveAs(path);
                        ModelState.Clear();
                        ViewBag.Message = "File uploaded successfully";

                        tQM_Details.Status = "PreDH Approval";
                        tQM_Details.TransactDate = DateTime.Now;
                        tQM_Details.SynergyModel = file.FileName;
                        db.TQM_Details.Add(tQM_Details);

                        try
                        {
                            db.SaveChanges();
                             var Html = "<script> alert('" + Request.Form["SerialNo"] + " Registered Successfully.'); window.location.href = 'Index'</script> ";
                            return Content(Html);
                         
                           
                        }
                        catch (Exception e)
                        {


                            var Html = "<script> alert('" + e.Message + " Please Try Again.'); window.location.href = 'Create'</script> ";
                            return Content(Html);
                        }
                        
                     
                    }




                }

                return RedirectToAction("Index");

            }
            return View();
    }

        // GET: StorySubmit/Edit/5
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

        // POST: StorySubmit/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AutoID,SerialNo,EmpNo,EmpName,KPI_Dept,Dept,Idea,KPI,SynergyModel,Status,TransactDate,FacilitatorID")] TQM_Details tQM_Details)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tQM_Details).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tQM_Details);
        }

        // GET: StorySubmit/Delete/5
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

        // POST: StorySubmit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TQM_Details tQM_Details = db.TQM_Details.Find(id);
            db.TQM_Details.Remove(tQM_Details);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        //GET : 
        public ActionResult GetDepartment()
        {
            var empno = Request.QueryString["empno"];


            eLeaveEntities eLeave = new eLeaveEntities();


            var data = eLeave.v_eLeave_EmployeeDetails.Where(m => m.empEmployeeNo == empno).ToList();
            
            return Content(data[0].preDeptDesc);
        }

        public ActionResult GetSerialNo()
        {
           

            var serialNo = from m in db.TQM_Details
                           select m.SerialNo;
            var serialize = new JavaScriptSerializer();

            var result = serialize.Serialize(serialNo);
            

            return Content(result);

        }

        public ActionResult GetKPI(int value)
        {


            var query = from m in db.KPIs
                        where m.KPI_DeptID == value
                        select m.KPI1;

            var serialize = new JavaScriptSerializer();

            var result = serialize.Serialize(query);

            return Content(result);
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
