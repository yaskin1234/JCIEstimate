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
using System.Linq;
using Microsoft.AspNet.Identity;
using System.IO;

namespace JCIEstimate.Controllers
{
    public class EstimatesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();


        //// GET: Estimates
        //public ActionResult Index2()
        //{
        //    return View();
        //}

        //// POST: Estimates_Read
        //[HttpPost]
        //public ActionResult Estimates_Read([DataSourceRequest] DataSourceRequest request)
        //{
        //    var est = db.Estimates.ToList().ToDataSourceResult(request);
        //    return Json(est, JsonRequestBehavior.AllowGet);
        //}

        
        // GET: Estimates
        public async Task<ActionResult> EstimateComparison(string filterId, string sort)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            var estimates = from cc in db.Estimates
                            where cc.Location.projectUid == sessionProject
                            select cc;

            return View(await estimates.ToListAsync());
        }

        // GET: Estimates
        public async Task<ActionResult> Index(string filterId, string sort)
        {
            IQueryable<Estimate> estimates;            
            List<FilterOptionModel> aryFo = new List<FilterOptionModel>();
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            estimates = from cc in db.Estimates.Where(c => c.estimateUid == Guid.Empty)
                        select cc;

            if (filterId == null)
            {
                if (Session["estimateFilterId"] != null)
                {
                    filterId = Session["estimateFilterId"].ToString();
                }
            }


            if (User.IsInRole("Admin"))
            {
                estimates = from cc in db.Estimates
                            join dd in db.Locations on cc.locationUid equals dd.locationUid
                            where dd.projectUid == sessionProject
                            select cc;
            }
            else
            {
                estimates = from cc in db.Estimates
                            join dd in db.Locations on cc.locationUid equals dd.locationUid
                            join cn in db.ContractorUsers on cc.contractorUid equals cn.contractorUid
                            join cq in db.AspNetUsers on cn.aspNetUserUid equals cq.Id
                            where dd.projectUid == sessionProject
                            && cq.UserName == System.Web.HttpContext.Current.User.Identity.Name
                            select cc;
            }     

            //Aggregates
            decimal? activeTotal = 0;
            decimal? bidTotal = 0;
            decimal? filteredActiveTotal = 0;
            decimal? filteredBidTotal = 0;

            if (estimates.Count() > 0)
            {
                activeTotal = estimates.Sum(d => d.activeAmount);
                bidTotal = estimates.Sum(d => d.amount);
            }

            //Aggregates
            ViewBag.activeTotal = String.Format("{0:C0}", activeTotal);
            ViewBag.bidTotal = String.Format("{0:C0}", bidTotal);            
            ViewBag.projectname = Session["projectName"];
            Session["activeTotal"] = activeTotal;
            Session["bidTotal"] = bidTotal;

            aryFo = buildFilterDropDown(filterId, db.Estimates.Where(c=>c.Location.projectUid == sessionProject));
            estimates = applyFilter(filterId, estimates);
            estimates = estimates.OrderBy(m => m.Location.location1).ThenBy(m => m.ECM.ecmNumber).ThenBy(m => m.Contractor.contractorName);

            if (estimates.Count() > 0)
            {
                filteredActiveTotal = estimates.Sum(d => d.activeAmount);
                filteredBidTotal = estimates.Sum(d => d.amount);
                Session["filteredActiveTotal"] = filteredActiveTotal;
                Session["filteredBidTotal"] = filteredBidTotal;
            }
            ViewBag.filteredActiveTotal = String.Format("{0:C0}", filteredActiveTotal);
            ViewBag.filteredBidTotal = String.Format("{0:C0}", filteredBidTotal);
            
            estimates = estimates.Include(e => e.Category).Include(e => e.ECM).Include(e => e.Location).Include(e => e.Contractor).Include(c=>c.ECM.Equipments).Include(e => e.EstimateStatu).OrderBy(c=>c.ECM.ecmNumber);
            ViewBag.filterList = aryFo.ToList();
            return View(await estimates.ToListAsync());
        }

        private List<FilterOptionModel> buildFilterDropDown(string filterId, IQueryable<Estimate> estimates)
        {
            //Build Drop down filter based on existing defined equipment
            List<FilterOptionModel> aryFo = new List<FilterOptionModel>();
            string[] filterPart = null;
            string type = "";
            string uid = Guid.Empty.ToString();

            if (!String.IsNullOrEmpty(filterId))
            {
                filterPart = filterId.Split('|');
                type = filterPart[0];
                uid = filterPart[1];
            }

            FilterOptionModel wf = new FilterOptionModel();
            wf.text = "-- Choose --";
            wf.value = "X|" + Guid.Empty.ToString();
            wf.selected = (wf.value == filterId || String.IsNullOrEmpty(filterId));
            aryFo.Add(wf);

            wf = new FilterOptionModel();
            wf.text = "All";
            wf.value = "A|" + Guid.Empty.ToString().Substring(0, 35) + "1";
            wf.selected = (wf.value == filterId);
            aryFo.Add(wf);


            IQueryable<Estimate> results = estimates.GroupBy(c => c.locationUid).Select(v => v.FirstOrDefault());

            foreach (var item in results.OrderBy(c => c.Location.location1))
            {
                wf = new FilterOptionModel();
                wf.text = item.Location.location1;
                wf.value = "L|" + item.locationUid.ToString();
                wf.selected = (wf.value == filterId);
                aryFo.Add(wf);
            }

            results = estimates.GroupBy(c => c.ecmUid).Select(v => v.FirstOrDefault());

            foreach (var item in results.OrderBy(c => c.ECM.ecmNumber))
            {
                wf = new FilterOptionModel();
                wf.text = item.ECM.ecmString;
                wf.value = "E|" + item.ecmUid.ToString();
                wf.selected = (wf.value == filterId);
                aryFo.Add(wf);
            }

            results = estimates.GroupBy(c => c.contractorUid).Select(v => v.FirstOrDefault());

            foreach (var item in results.OrderBy(c => c.Contractor.contractorName))
            {
                wf = new FilterOptionModel();
                wf.text = item.Contractor.contractorName;
                wf.value = "C|" + item.contractorUid.ToString();
                wf.selected = (wf.value == filterId);
                aryFo.Add(wf);
            }

            results = estimates.GroupBy(c => c.estimateStatusUid).Select(v => v.FirstOrDefault());

            foreach (var item in results.OrderBy(c => c.EstimateStatu.estimateStatus))
            {
                wf = new FilterOptionModel();
                wf.text = item.EstimateStatu.estimateStatus;
                wf.value = "S|" + item.estimateStatusUid.ToString();
                wf.selected = (wf.value == filterId);
                aryFo.Add(wf);
            }

            return aryFo;
        }

        private IQueryable<Estimate> applyFilter(string filterId, IQueryable<Estimate> estimates)
        {
            //apply filter if there is one
            string[] filterPart = null;
            string type = "";
            string uid = Guid.Empty.ToString();
            if (!String.IsNullOrEmpty(filterId))
            {
                filterPart = filterId.Split('|');
                type = filterPart[0];
                uid = filterPart[1];

                if (type == "L")
                {
                    estimates = estimates.Where(c => c.locationUid.ToString() == uid);
                }
                else if(type == "E")
                {
                    estimates = estimates.Where(c => c.ecmUid.ToString() == uid);
                }
                else if (type == "C")
                {
                    estimates = estimates.Where(c => c.contractorUid.ToString() == uid);
                }
                else if (type == "S")
                {
                    estimates = estimates.Where(c => c.estimateStatusUid.ToString() == uid);
                }
                else if (type == "X")
                {
                    estimates = estimates.Where(c => c.ecmUid == Guid.Empty);
                }
            }
            else
            {
                estimates = estimates.Where(c => c.ecmUid == Guid.Empty);
            }
            Session["estimateFilterId"] = filterId;
            
            return estimates;
        }

        // GET: EquipmentToDoes/SaveCheckedBox/5
        public async Task<ActionResult> SaveIsActive(string chkBoxName, string value)
        {
            Estimate es = db.Estimates.Find(Guid.Parse(chkBoxName));
            db.Entry(es).State = EntityState.Modified;
            if (value == "true")
            {
                es.isActive = true;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                es.isActive = false;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return View();
        }

        // GET: Estimates/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estimate estimate = await db.Estimates.FindAsync(id);
            if (estimate == null)
            {
                return HttpNotFound();
            }
            return View(estimate);
        }

        // GET: Estimates/Create
        [Authorize(Roles="Admin")]
        public ActionResult Create()
        {
            IQueryable<Location> locations;
            IQueryable<ECM> ecms;
            IQueryable<Contractor> contractors;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            ecms = from cc in db.ECMs
                   where cc.projectUid == sessionProject
                   orderby cc.ecmNumber
                   select cc;

            locations = from cc in db.Locations
                        where cc.projectUid == sessionProject
                        orderby cc.location1
                        select cc;
            
            if (!User.IsInRole("Admin"))
            {
                contractors = from cc in db.Contractors
                              join cn in db.ContractorUsers on cc.contractorUid equals cn.contractorUid
                              join cq in db.AspNetUsers on cn.aspNetUserUid equals cq.Id
                              where cq.UserName == System.Web.HttpContext.Current.User.Identity.Name
                              orderby cc.contractorName
                              select cc;
            }
            else
            {
                contractors = from cc in db.Contractors
                              orderby cc.contractorName
                              select cc;
            }
            

            ViewBag.ecmUid = ecms.ToSelectList(d => d.ecmNumber + " - " + d.ecmDescription, d => d.ecmUid.ToString(), "");
            ViewBag.locationUid = locations.ToSelectList(d => d.location1, d => d.locationUid.ToString(), "");
            ViewBag.categoryUid = db.Categories.OrderBy(m=>m.category1).ToSelectList(d => d.category1, d => d.categoryUid.ToString(), "");
            ViewBag.contractorUid = contractors.ToSelectList(d => d.contractorName, d => d.contractorUid.ToString(), "");
            ViewBag.estimateOptionUid = new SelectList(db.EstimateOptions.OrderBy(m => m.EstimateOption1), "estimateOptionUid", "EstimateOption1"); 
            ViewBag.estimateStatusUid = db.EstimateStatus.OrderBy(m => m.estimateStatus).ToSelectList(d => d.estimateStatus, d => d.estimateStatusUid.ToString(), "");
            return View();
        }

        // POST: Estimates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "estimateUid,locationUid,ecmUid,categoryUid,estimateStatusUid,isActive,amount,activeAmount,notes,contractorUid,estimateOptionUid")] Estimate estimate)
        {
            if (ModelState.IsValid)
            {
                estimate.estimateUid = Guid.NewGuid();
                db.Estimates.Add(estimate);

                //add equipment attribnute type task assignments                
                IQueryable<EquipmentAttributeTypeTask> tasks = from cc in db.Equipments
                                                               join qq in db.EquipmentAttributeTypes on cc.equipmentAttributeTypeUid equals qq.equipmentAttributeTypeUid
                                                               join dd in db.EquipmentAttributeTypeTasks on qq.equipmentAttributeTypeUid equals dd.equipmentAttributeTypeUid
                                                               where cc.ecmUid == estimate.ecmUid
                                                               select dd;

                tasks = tasks.GroupBy(c => c.equipmentAttributeTypeTaskUid).Select(grp => grp.FirstOrDefault());

                foreach (EquipmentAttributeTypeTask item in tasks)
                {
                    var exists = from cc in db.EquipmentTypeTaskAssignments
                                 where cc.ecmUid == estimate.ecmUid
                                 && cc.equipmentAttributeTypeTaskUid == item.equipmentAttributeTypeTaskUid
                                 && cc.locationUid == estimate.locationUid
                                 select cc;

                    if (exists.Count() == 0)
                    {
                        EquipmentTypeTaskAssignment et = new EquipmentTypeTaskAssignment();
                        et.equipmentTypeTaskAssignmentUid = Guid.NewGuid();
                        et.ecmUid = estimate.ecmUid;
                        et.locationUid = estimate.locationUid;
                        et.contractorUid = estimate.contractorUid;
                        et.equipmentAttributeTypeUid = item.equipmentAttributeTypeUid;
                        et.equipmentAttributeTypeTaskUid = item.equipmentAttributeTypeTaskUid;
                        db.EquipmentTypeTaskAssignments.Add(et);
                    }
                }

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.categoryUid = new SelectList(db.Categories, "categoryUid", "category1", estimate.categoryUid);
            ViewBag.ecmUid = new SelectList(db.ECMs, "ecmUid", "ecmNumber", estimate.ecmUid);
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1", estimate.locationUid);
            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName", estimate.contractorUid);
            ViewBag.estimateOptionUid = new SelectList(db.EstimateOptions.OrderBy(m => m.EstimateOption1), "estimateOptionUid", "EstimateOption1"); 
            ViewBag.estimateStatusUid = new SelectList(db.EstimateStatus, "estimateStatusUid", "estimateStatus");
            return View(estimate);
        }

       

        // GET: Estimates/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            IQueryable<Location> locations;
            IQueryable<ECM> ecms;
            IQueryable<Contractor> contractors;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estimate estimate = await db.Estimates.FindAsync(id);
            
            if (estimate == null)
            {
                return HttpNotFound();
            }            
            
            locations = from cc in db.Locations
                        where cc.projectUid == sessionProject
                        orderby cc.location1
                        select cc;


            ecms = from cc in db.ECMs
                   where cc.projectUid == sessionProject
                   orderby cc.ecmNumber
                   select cc;

            if (!User.IsInRole("Admin"))
            {
                contractors = from cc in db.Contractors
                              join cn in db.ContractorUsers on cc.contractorUid equals cn.contractorUid
                              join cq in db.AspNetUsers on cn.aspNetUserUid equals cq.Id
                              where cq.UserName == System.Web.HttpContext.Current.User.Identity.Name
                              orderby cc.contractorName
                              select cc;
            }
            else
            {
                contractors = from cc in db.Contractors
                              orderby cc.contractorName
                              select cc;
            }

            ViewBag.ecmUid = new SelectList(ecms, "ecmUid", "ecmString", estimate.ecmUid);
            ViewBag.locationUid = new SelectList(locations, "locationUid", "location1", estimate.locationUid);
            ViewBag.categoryUid = new SelectList(db.Categories.OrderBy(m=>m.category1), "categoryUid", "category1", estimate.categoryUid);
            ViewBag.contractorUid = new SelectList(contractors, "contractorUid", "contractorName", estimate.contractorUid);
            ViewBag.estimateStatusUid = new SelectList(db.EstimateStatus.OrderBy(m=>m.estimateStatus), "estimateStatusUid", "estimateStatus", estimate.estimateStatusUid);
            ViewBag.estimateOptionUid = new SelectList(db.EstimateOptions.OrderBy(m => m.EstimateOption1), "estimateOptionUid", "EstimateOption1", estimate.estimateOptionUid); 
            return View(estimate);
        }

        // POST: Estimates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "estimateUid,locationUid,ecmUid,categoryUid,estimateStatusUid,isActive,amount,activeAmount,notes,contractorUid,estimateOptionUid")] Estimate estimate, string submit)
        {
            if (ModelState.IsValid)
            {
                if (submit == "Submit")
                {
                    var submittedValue = from cc in db.EstimateStatus
                                         where cc.behaviorIndicator == "S"
                                         select cc.estimateStatusUid;
                    estimate.estimateStatusUid = submittedValue.FirstOrDefault();
                }
                db.Entry(estimate).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }            
            ViewBag.categoryUid = new SelectList(db.Categories.OrderBy(x => x.category1), "categoryUid", "category1", estimate.categoryUid);
            ViewBag.ecmUid = new SelectList(db.ECMs, "ecmUid", "ecmNumber", estimate.ecmUid);
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1", estimate.locationUid);
            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName", estimate.contractorUid);
            ViewBag.estimateStatusUid = new SelectList(db.EstimateStatus.OrderBy(m => m.estimateStatus), "estimateStatusUid", "estimateStatus", estimate.estimateStatusUid);
            ViewBag.estimateOptionUid = new SelectList(db.EstimateOptions.OrderBy(m => m.EstimateOption1), "estimateOptionUid", "EstimateOption1", estimate.estimateOptionUid); 
            return View(estimate);
        }

        // GET: Estimates/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estimate estimate = await db.Estimates.FindAsync(id);
            if (estimate == null)
            {
                return HttpNotFound();
            }
            return View(estimate);
        }

        // POST: Estimates/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Estimate estimate = await db.Estimates.FindAsync(id);
            db.Estimates.Remove(estimate);
            await db.SaveChangesAsync();
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
