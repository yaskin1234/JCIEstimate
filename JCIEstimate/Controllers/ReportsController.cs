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

        public ActionResult GetNewEquipment()
        {
            if (Session["projectUid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("rptNewEquipment");
            }
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

        public ActionResult GetContractorSignOff()
        {
            if (Session["projectUid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var currentUser = IdentityExtensions.GetUserId(User.Identity);
                var contractorUid = from cc in db.ContractorUsers
                                    where cc.aspNetUserUid == currentUser
                                    select cc.contractorUid;

                Session["userUid"] = IdentityExtensions.GetUserId(User.Identity);                

                if (contractorUid.FirstOrDefault() != Guid.Empty)
                {
                    ViewBag.contractorUid = contractorUid.FirstOrDefault();
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
                

                return View("rptContractorSignOff");
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

        public ActionResult GetEquipmentForECM()
        {
            if (Session["projectUid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Session["userUid"] = IdentityExtensions.GetUserId(User.Identity);
                return View("rptEquipmentForECM");
            }
        }

        public ActionResult GetEquipmentForECMToPurchase()
        {
            if (Session["projectUid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Session["userUid"] = IdentityExtensions.GetUserId(User.Identity);
                return View("rptEquipmentForECMToPurchase");
            }
        }

        public ActionResult GetProjectGrandTotal()
        {
            if (Session["projectUid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Session["userUid"] = IdentityExtensions.GetUserId(User.Identity);

                return View("rptProjectGrandTotal");
            }
        }

        public ActionResult GetBarGraphSummary()
        {
            if (Session["projectUid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Session["userUid"] = IdentityExtensions.GetUserId(User.Identity);

                return View("rptBarGraphSummary");
            }
        }


        public ActionResult GetProjectMilestoneActions()
        {
            if (Session["projectUid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Session["userUid"] = IdentityExtensions.GetUserId(User.Identity);

                return View("rptProjectMilestoneActions");
            }
        }
    }
}