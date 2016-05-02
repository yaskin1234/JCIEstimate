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

        public ActionResult GetEstimateComparison()
        {
            if (Session["projectUid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("rptEstimateComparison");
            }
        }

        public ActionResult GetCommissionIssueSummary()
        {
            if (Session["projectUid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("rptCommissionIssueSummary");
            }
        }

        public ActionResult GetEstimateComparisonActive()
        {
            if (Session["projectUid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("rptEstimateComparisonActive");
            }
        }

        public ActionResult GetBidsByClassification()
        {
            if (Session["projectUid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("rptBidsByClassification");
            }
        }

        public ActionResult GetBidsByClassificationAndJCICode()
        {
            if (Session["projectUid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("rptBidsByClassificationAndJCICode");
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

        public ActionResult GetECMList()
        {
            if (Session["projectUid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("rptECMList");
            }
        }

        public ActionResult GetJCIWorksiteExport()
        {
            if (Session["projectUid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("rptJCIWorksiteExport");
            }
        }

        public ActionResult GetScopeOfWorkMaster()
        {
            if (Session["projectUid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("rptScopeOfWorkMaster");
            }
        }

        public ActionResult GetLocationList()
        {
            if (Session["projectUid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("rptLocationList");
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

        public ActionResult GetEquipmentDetailMaster()
        {
            if (Session["projectUid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Session["userUid"] = IdentityExtensions.GetUserId(User.Identity);
                return View("rptEquipmentDetailMaster");
            }
        }

        public ActionResult GetDrawSchedules()
        {
            if (Session["projectUid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Session["userUid"] = IdentityExtensions.GetUserId(User.Identity);
                return View("rptDrawSchedules");
            }
        }


        public ActionResult GetActiveEstimates()
        {
            if (Session["projectUid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Session["userUid"] = IdentityExtensions.GetUserId(User.Identity);
                return View("rptActiveEstimates");
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

        public ActionResult GetEquipmentExport()
        {
            if (Session["projectUid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("rptEquipmentExport");
            }
        }
    }
}