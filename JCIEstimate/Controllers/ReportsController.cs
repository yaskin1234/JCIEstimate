using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JCIEstimate.Models;

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

        public ActionResult GetContractorTotals()
        {
            return View("ContractorTotals");
        }
    }
}