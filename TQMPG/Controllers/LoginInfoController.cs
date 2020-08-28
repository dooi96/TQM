using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace TQMPG.Controllers
{
    public class LoginInfoController : Controller
    {
        // GET: LoginInfo
        public ActionResult Index()
        {
            System.Security.Principal.WindowsIdentity identity = System.Web.HttpContext.Current.Request.LogonUserIdentity;
            var ntid = identity.Name.Split('\\')[1];
            var details = "http://adpgintranet2.web.analog.com/webApi/api/GetUser?ntid=" + ntid ;
            System.Diagnostics.Debug.WriteLine(details);



            //IdentityReferenceCollection irc = identity.Groups;
            //foreach (IdentityReference ir in irc)
            //    System.Diagnostics.Debug.WriteLine(ir);
            return View();
        }
    }
}