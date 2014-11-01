using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JCIEstimate.Models;
using Microsoft.AspNet.Identity;

namespace JCIEstimate.Controllers
{
    public class ReportsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCategoryECMByContractor()
        {
            if (Session["projectUid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("rptCategoryECMByContractor");
            }
        }

        public ActionResult GetEstimateSummary()
        {
            if (Session["projectUid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("rptEstimateSummary");
            }
        }

        public ActionResult GetBidSummary()
        {
            if (Session["projectUid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("rptBidSummary");
            }
        }
        public ActionResult GetContractorSummary()
        {
            if (Session["projectUid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Session["userUid"] = IdentityExtensions.GetUserId(User.Identity);                
                return View("rptContractorSummary");
            }
        }  
    }
}