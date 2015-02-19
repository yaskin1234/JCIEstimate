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
using System.IO;  

namespace IdentitySample.Controllers
{
    public class HomeController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase file1) // OR IEnumerable<HttpPostedFileBase> files
        {
            file1 = Request.Files["file1"];

            if (file1 != null) // Same for file2, file3, file4
            {
                // Check and Save the file1 // Same for file2, file3, file4
                if (file1.ContentLength > 0)
                {
                    string filePath = Path.Combine(HttpContext.Server.MapPath(@"..\Context\ScopeDocuments"),
                                                   Path.GetFileName(file1.FileName));
                    file1.SaveAs(filePath);
                }
            }
            return View();
        }
        
        public ActionResult Index()
        {
            Session["projectUid"] = null;
            Session["projectName"] = null;
            
            IQueryable<Project> projects;

            if (User.IsInRole("Admin") || User.IsInRole("Sales"))
            {
                projects = from cc in db.Projects
                           select cc;
            }
            else if (User.IsInRole("Warranty"))
            {
                projects = from cc in db.Projects
                           join pu in db.ProjectUsers on cc.projectUid equals pu.projectUid
                           join pq in db.AspNetUsers on pu.aspNetUserUid equals pq.Id
                           where pq.UserName == System.Web.HttpContext.Current.User.Identity.Name
                           select cc;
            }
            else
            {
                projects = from cc in db.Projects
                           join dd in db.Locations on cc.projectUid equals dd.projectUid
                           join ee in db.Estimates on dd.locationUid equals ee.locationUid
                           join c in db.Contractors on ee.contractorUid equals c.contractorUid
                           join cn in db.ContractorUsers on c.contractorUid equals cn.contractorUid
                           join cq in db.AspNetUsers on cn.aspNetUserUid equals cq.Id
                           where cq.UserName == System.Web.HttpContext.Current.User.Identity.Name
                           select cc;
                projects = projects.Distinct();
            }


            ViewBag.projectUid = projects.ToSelectList(d => d.project1, d => d.projectUid.ToString(), "");            
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
