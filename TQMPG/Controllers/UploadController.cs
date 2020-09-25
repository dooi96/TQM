using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TQMPG.Models;

namespace TQMPG.Controllers
{
    public class UploadController : Controller
    {
        TQMEntitiesModel db = new TQMEntitiesModel();
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]

        public ActionResult downloadFile()
        {
            return View(db.TQM_UploadCompletedStory.ToList());
        }


        public ActionResult UploadFile()
        {
            ViewBag.MethodologyID = new SelectList(db.TQM_Methodology, "AutoID", "MethodologyDescription");
            return View();
        }

        public ActionResult getPath()
        {
            string filename = Request.Form["id"];
            string path = Server.MapPath("~/UploadedFiles/");


            return Content(path+filename);
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            ViewBag.MethodologyID = new SelectList(db.TQM_Methodology, "AutoID", "MethodologyDescription");
            try
            {
                if (file.ContentLength > 0)
                {
                    string filename = Request.Form["projectTitle"];
                    //System.Diagnostics.Debug.WriteLine(filename);
                    string _FileName = filename + Path.GetExtension(file.FileName);
                    //System.Diagnostics.Debug.WriteLine(_FileName);
                    string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
                    //System.Diagnostics.Debug.WriteLine(_path);
                    file.SaveAs(_path);


                    TQM_UploadCompletedStory uploadCompletedStory = new TQM_UploadCompletedStory();
                    uploadCompletedStory.ProjectTitle = Request.Form["projectTitle"];
                    uploadCompletedStory.Category = Request.Form["category"];
                    uploadCompletedStory.Methodology = Int32.Parse(Request.Form["MethodologyID"]);
                    uploadCompletedStory.FY = Request.Form["fy"];
                    uploadCompletedStory.filename = _FileName;
                    db.TQM_UploadCompletedStory.Add(uploadCompletedStory);
             
                    db.SaveChanges();
                    
                  
                     
                }
                ViewBag.Message = "File Uploaded Successfully!!";
                return View();
            }
            catch(Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }
    }

}
