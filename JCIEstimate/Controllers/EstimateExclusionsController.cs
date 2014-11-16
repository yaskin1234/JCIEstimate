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
    public class EstimateExclusionsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: EstimateExclusions
        public async Task<ActionResult> Index()
        {
            IQueryable<EstimateExclusion> estimateExclusions;
            if (!User.IsInRole("Admin"))
            {
                estimateExclusions = from ce in db.EstimateExclusions
                                     join cc in db.Estimates on ce.estimateUid equals cc.estimateUid
                                     join cn in db.ContractorUsers on cc.contractorUid equals cn.contractorUid
                                     join cq in db.AspNetUsers on cn.aspNetUserUid equals cq.Id
                                     where cq.UserName == System.Web.HttpContext.Current.User.Identity.Name
                                     orderby ce.estimateExclusionID
                                     select ce;
            }
            else
            {
                estimateExclusions = from ce in db.EstimateExclusions
                                     select ce;
            }
            return View(await estimateExclusions.ToListAsync());
        }

        // GET: EstimateExclusions/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstimateExclusion estimateExclusion = await db.EstimateExclusions.FindAsync(id);
            if (estimateExclusion == null)
            {
                return HttpNotFound();
            }
            return View(estimateExclusion);
        }

        // GET: EstimateExclusions/Create
        public ActionResult Create()
        {
            IQueryable<Estimate> estimates;
            if (!User.IsInRole("Admin"))
            {
                estimates = from cc in db.Estimates
                            join cn in db.ContractorUsers on cc.contractorUid equals cn.contractorUid
                            join cq in db.AspNetUsers on cn.aspNetUserUid equals cq.Id
                            where cq.UserName == System.Web.HttpContext.Current.User.Identity.Name
                            orderby cn.Contractor.contractorName
                            select cc;
            }
            else
            {
                estimates = from cc in db.Estimates
                            orderby cc.Contractor.contractorName
                            select cc;
            }

            ViewBag.estimateUid = estimates.ToSelectList(d => d.Contractor.contractorName + " - " + d.ECM.ecmString, d => d.estimateUid.ToString(), "");
            return View();
        }

        // POST: EstimateExclusions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "estimateExclusionUid,estimateExclusionID,estimateUid,estimateExclusion1")] EstimateExclusion estimateExclusion)
        {
            if (ModelState.IsValid)
            {
                estimateExclusion.estimateExclusionUid = Guid.NewGuid();
                db.EstimateExclusions.Add(estimateExclusion);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            IQueryable<Estimate> estimates;
            if (!User.IsInRole("Admin"))
            {
                estimates = from cc in db.Estimates
                            join cn in db.ContractorUsers on cc.contractorUid equals cn.contractorUid
                            join cq in db.AspNetUsers on cn.aspNetUserUid equals cq.Id
                            where cq.UserName == System.Web.HttpContext.Current.User.Identity.Name
                            orderby cn.Contractor.contractorName
                            select cc;
            }
            else
            {
                estimates = from cc in db.Estimates
                            orderby cc.Contractor.contractorName
                            select cc;
            }

            ViewBag.estimateUid = estimates.ToSelectList(d => d.Contractor.contractorName + " - " + d.ECM.ecmString, d => d.estimateUid.ToString(), estimateExclusion.estimateUid.ToString());            
            return View(estimateExclusion);
        }

        // GET: EstimateExclusions/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstimateExclusion estimateExclusion = await db.EstimateExclusions.FindAsync(id);
            if (estimateExclusion == null)
            {
                return HttpNotFound();
            }
            IQueryable<Estimate> estimates;
            if (!User.IsInRole("Admin"))
            {
                estimates = from cc in db.Estimates
                            join cn in db.ContractorUsers on cc.contractorUid equals cn.contractorUid
                            join cq in db.AspNetUsers on cn.aspNetUserUid equals cq.Id
                            where cq.UserName == System.Web.HttpContext.Current.User.Identity.Name
                            orderby cn.Contractor.contractorName
                            select cc;
            }
            else
            {
                estimates = from cc in db.Estimates
                            orderby cc.Contractor.contractorName
                            select cc;
            }

            ViewBag.estimateUid = estimates.ToSelectList(d => d.Contractor.contractorName + " - " + d.ECM.ecmString, d => d.estimateUid.ToString(), estimateExclusion.estimateUid.ToString());            
            return View(estimateExclusion);
        }

        // POST: EstimateExclusions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "estimateExclusionUid,estimateExclusionID,estimateUid,estimateExclusion1")] EstimateExclusion estimateExclusion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estimateExclusion).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            IQueryable<Estimate> estimates;
            if (!User.IsInRole("Admin"))
            {
                estimates = from cc in db.Estimates
                            join cn in db.ContractorUsers on cc.contractorUid equals cn.contractorUid
                            join cq in db.AspNetUsers on cn.aspNetUserUid equals cq.Id
                            where cq.UserName == System.Web.HttpContext.Current.User.Identity.Name
                            orderby cn.Contractor.contractorName
                            select cc;
            }
            else
            {
                estimates = from cc in db.Estimates
                            orderby cc.Contractor.contractorName
                            select cc;
            }

            ViewBag.estimateUid = estimates.ToSelectList(d => d.Contractor.contractorName + " - " + d.ECM.ecmString, d => d.estimateUid.ToString(), estimateExclusion.estimateUid.ToString());            
            return View(estimateExclusion);
        }

        // GET: EstimateExclusions/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstimateExclusion estimateExclusion = await db.EstimateExclusions.FindAsync(id);
            if (estimateExclusion == null)
            {
                return HttpNotFound();
            }
            return View(estimateExclusion);
        }

        // POST: EstimateExclusions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            EstimateExclusion estimateExclusion = await db.EstimateExclusions.FindAsync(id);
            db.EstimateExclusions.Remove(estimateExclusion);
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
