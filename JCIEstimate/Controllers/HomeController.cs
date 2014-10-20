using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JCIEstimate.Models;
using JCIExtensions;

namespace IdentitySample.Controllers
{
    public class HomeController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();
        
        public ActionResult Index()
        {
            ViewBag.projectUid = db.Projects.ToSelectList(d => d.project1, d => d.projectUid.ToString(), "");
            //ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1");
            return View();
        }

        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        } 
    }
}
