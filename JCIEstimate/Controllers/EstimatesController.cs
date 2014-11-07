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

namespace JCIEstimate.Controllers
{
    public class EstimatesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: Estimates
        public async Task<ActionResult> Index(int page = 1, string sort = "", string sortDir = "ASC", 
            string location = null, string ecm = null, string category = null, string contractor = null, string status = null, string active = null,
            string filterSubmit = null
            )
        {
            IQueryable<Estimate> estimates;            
            Guid sessionProject;

            sessionProject = Guid.Empty;

            if (Session["projectUid"] != null)
            {
                sessionProject = new System.Guid(Session["projectUid"].ToString());
            }
            else
            {
                return RedirectToAction("Index", "Home");
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

            #region="filter"            
            if (sort == "Contractor")
            {                
                if (sortDir == "ASC")
                {
                    estimates = from ee in estimates
                                orderby ee.Contractor.contractorName ascending
                                select ee;
                }
                else
                {
                    estimates = from ee in estimates
                                orderby ee.Contractor.contractorName descending
                                select ee;
                }
                
            }
            else if (sort == "Location")
            {
                if (sortDir == "ASC")
                {
                    estimates = from ee in estimates
                                orderby ee.Location.location1 ascending
                                select ee;
                }
                else
                {
                    estimates = from ee in estimates
                                orderby ee.Location.location1 descending
                                select ee;
                }

            }
            else if (sort == "Category")
            {
                if (sortDir == "ASC")
                {
                    estimates = from ee in estimates
                                orderby ee.Category.category1 ascending
                                select ee;
                }
                else
                {
                    estimates = from ee in estimates
                                orderby ee.Category.category1 descending
                                select ee;
                }

            }
            else if (sort == "ECM")
            {
                if (sortDir == "ASC")
                {
                    estimates = from ee in estimates
                                orderby ee.ECM.ecmNumber ascending
                                select ee;
                }
                else
                {
                    estimates = from ee in estimates
                                orderby ee.ECM.ecmNumber descending
                                select ee;
                }

            }
            else if (sort == "Active?")
            {
                if (sortDir == "ASC")
                {
                    estimates = from ee in estimates
                                orderby ee.isActive ascending
                                select ee;
                }
                else
                {
                    estimates = from ee in estimates
                                orderby ee.isActive descending
                                select ee;
                }

            }
            else if (sort == "Amount")
            {
                if (sortDir == "ASC")
                {
                    estimates = from ee in estimates
                                orderby ee.amount ascending
                                select ee;
                }
                else
                {
                    estimates = from ee in estimates
                                orderby ee.amount descending
                                select ee;
                }

            }
            else if (sort == "Active Amount")
            {
                if (sortDir == "ASC")
                {
                    estimates = from ee in estimates
                                orderby ee.activeAmount ascending
                                select ee;
                }
                else
                {
                    estimates = from ee in estimates
                                orderby ee.activeAmount descending
                                select ee;
                }

            }
            else
            {
                estimates = estimates.OrderBy(m => m.Location.location1).ThenBy(m => m.ECM.ecmString).ThenBy(m => m.Contractor.contractorName);
            }
            #endregion

            if (filterSubmit != "clear")            
            {
                if (location != null)
                {
                    estimates = estimates.Where(m => m.Location.location1.Contains(location));
                }

                if (ecm != null)
                {
                    estimates = estimates.Where(m => m.ECM.ecmString.Contains(ecm));
                }

                if (contractor != null)
                {
                    estimates = estimates.Where(m => m.Contractor.contractorName.Contains(contractor));
                }

                if (category != null)
                {
                    estimates = estimates.Where(m => m.Category.category1.Contains(category));
                }

                if (status != null)
                {
                    estimates = estimates.Where(m => m.EstimateStatu.estimateStatus.Contains(status));
                }

                if (active != null)
                {
                    estimates = estimates.Where(m => m.isActive == true);
                }
            }

            if (estimates.Count() > 0)
            {
                filteredActiveTotal = estimates.Sum(d => d.activeAmount);
                filteredBidTotal = estimates.Sum(d => d.amount);
                Session["filteredActiveTotal"] = filteredActiveTotal;
                Session["filteredBidTotal"] = filteredBidTotal;
            }
            ViewBag.filteredActiveTotal = String.Format("{0:C0}", filteredActiveTotal);
            ViewBag.filteredBidTotal = String.Format("{0:C0}", filteredBidTotal);
            
            estimates = estimates.Include(e => e.Category).Include(e => e.ECM).Include(e => e.Location).Include(e => e.Contractor).Include(e => e.EstimateStatu);            
            return View(await estimates.ToListAsync());
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
            Guid sessionProject;

            sessionProject = Guid.Empty;

            if (Session["projectUid"] != null)
            {
                sessionProject = new System.Guid(Session["projectUid"].ToString());
            }
            else
            {
                Url.Action("Index", "Home");
            }           

            ecms = from cc in db.ECMs
                   where cc.projectUid == sessionProject
                   orderby cc.ecmString
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
            ViewBag.estimateStatusUid = db.EstimateStatus.OrderBy(m => m.estimateStatus).ToSelectList(d => d.estimateStatus, d => d.estimateStatusUid.ToString(), "");
            return View();
        }

        // POST: Estimates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "estimateUid,locationUid,ecmUid,categoryUid,estimateStatusUid,isActive,amount,activeAmount,notes,contractorUid")] Estimate estimate)
        {
            if (ModelState.IsValid)
            {
                estimate.estimateUid = Guid.NewGuid();
                db.Estimates.Add(estimate);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.categoryUid = new SelectList(db.Categories, "categoryUid", "category1", estimate.categoryUid);
            ViewBag.ecmUid = new SelectList(db.ECMs, "ecmUid", "ecmNumber", estimate.ecmUid);
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1", estimate.locationUid);
            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName", estimate.contractorUid);
            ViewBag.estimateStatusUid = new SelectList(db.EstimateStatus, "estimateStatusUid", "estimateStatus");
            return View(estimate);
        }

        // GET: Estimates/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            IQueryable<Location> locations;
            IQueryable<ECM> ecms;
            IQueryable<Contractor> contractors;
            Guid sessionProject;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estimate estimate = await db.Estimates.FindAsync(id);
            if (estimate == null)
            {
                return HttpNotFound();
            }

            if (Session["projectUid"] != null)
            {
                sessionProject = new System.Guid(Session["projectUid"].ToString());
            }
            else
            {
                sessionProject = new System.Guid(DBNull.Value.ToString());
            }
            
            locations = from cc in db.Locations
                        where cc.projectUid == sessionProject
                        orderby cc.location1
                        select cc;


            ecms = from cc in db.ECMs
                   where cc.projectUid == sessionProject
                   orderby cc.ecmString
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
            ViewBag.estimateStatusUid = new SelectList(db.EstimateStatus.OrderBy(m=>m.estimateStatus), "estimateStatusUid", "estimateStatus");
            return View(estimate);
        }

        // POST: Estimates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "estimateUid,locationUid,ecmUid,categoryUid,estimateStatusUid,isActive,amount,activeAmount,notes,contractorUid")] Estimate estimate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estimate).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }            
            ViewBag.categoryUid = new SelectList(db.Categories.OrderBy(x => x.category1), "categoryUid", "category1", estimate.categoryUid);
            ViewBag.ecmUid = new SelectList(db.ECMs, "ecmUid", "ecmNumber", estimate.ecmUid);
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1", estimate.locationUid);
            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName", estimate.contractorUid);
            ViewBag.estimateStatusUid = new SelectList(db.EstimateStatus, "estimateStatusUid", "estimateStatus");
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
