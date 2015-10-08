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
    public class EstimateInclusionsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: EstimateInclusions
        public async Task<ActionResult> Index()
        {
            IQueryable<EstimateInclusion> estimateInclusions;
            if (!User.IsInRole("Admin"))
            {
                estimateInclusions = from ce in db.EstimateInclusions
                                     join cc in db.Estimates on ce.estimateUid equals cc.estimateUid
                                     join cn in db.ContractorUsers on cc.contractorUid equals cn.contractorUid
                                     join cq in db.AspNetUsers on cn.aspNetUserUid equals cq.Id
                                     where cq.UserName == System.Web.HttpContext.Current.User.Identity.Name
                                     && ce.Estimate.Location.projectUid == MCVExtensions.getSessionProject()
                                     orderby ce.estimateInclusionID
                                     select ce;
            }
            else
            {
                estimateInclusions = from ce in db.EstimateInclusions
                                     where ce.Estimate.Location.projectUid == MCVExtensions.getSessionProject()
                                     select ce;
            }
            return View(await estimateInclusions.ToListAsync());
        }

        // GET: EstimateInclusions/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstimateInclusion estimateInclusion = await db.EstimateInclusions.FindAsync(id);
            if (estimateInclusion == null)
            {
                return HttpNotFound();
            }
            return View(estimateInclusion);
        }

        // GET: EstimateInclusions/Create
        public ActionResult Create()
        {

            IQueryable<Estimate> estimates;
            if (!User.IsInRole("Admin"))
            {
                estimates = from cc in db.Estimates
                            join cn in db.ContractorUsers on cc.contractorUid equals cn.contractorUid
                            join cq in db.AspNetUsers on cn.aspNetUserUid equals cq.Id
                            where cq.UserName == System.Web.HttpContext.Current.User.Identity.Name
                            && cc.Location.projectUid == MCVExtensions.getSessionProject()
                            orderby cn.Contractor.contractorName
                            select cc;
            }
            else
            {
                estimates = from cc in db.Estimates
                            where cc.Location.projectUid == MCVExtensions.getSessionProject()
                            orderby cc.Contractor.contractorName
                            select cc;
            }

            ViewBag.estimateUid = estimates.ToSelectList(d => d.Contractor.contractorName + " - " + d.ECM.ecmString, d => d.estimateUid.ToString(), "");     
            return View();
        }

        // POST: EstimateInclusions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "estimateInclusionUid,estimateInclusionID,estimateUid,estimateInclusion1")] EstimateInclusion estimateInclusion)
        {
            if (ModelState.IsValid)
            {
                estimateInclusion.estimateInclusionUid = Guid.NewGuid();
                db.EstimateInclusions.Add(estimateInclusion);
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
                            && cc.Location.projectUid == MCVExtensions.getSessionProject()
                            orderby cn.Contractor.contractorName
                            select cc;
            }
            else
            {
                estimates = from cc in db.Estimates
                            where cc.Location.projectUid == MCVExtensions.getSessionProject()
                            orderby cc.Contractor.contractorName
                            select cc;
            }

            ViewBag.estimateUid = estimates.ToSelectList(d => d.Contractor.contractorName + " - " + d.ECM.ecmString, d => d.estimateUid.ToString(), estimateInclusion.estimateUid.ToString());     
            return View(estimateInclusion);
        }

        // GET: EstimateInclusions/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstimateInclusion estimateInclusion = await db.EstimateInclusions.FindAsync(id);
            if (estimateInclusion == null)
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
                            && cc.Location.projectUid == MCVExtensions.getSessionProject()
                            orderby cn.Contractor.contractorName
                            select cc;
            }
            else
            {
                estimates = from cc in db.Estimates
                            orderby cc.Contractor.contractorName
                            select cc;
            }

            ViewBag.estimateUid = estimates.ToSelectList(d => d.Contractor.contractorName + " - " + d.ECM.ecmString, d => d.estimateUid.ToString(), estimateInclusion.estimateUid.ToString());     
            return View(estimateInclusion);
        }

        // POST: EstimateInclusions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "estimateInclusionUid,estimateInclusionID,estimateUid,estimateInclusion1")] EstimateInclusion estimateInclusion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estimateInclusion).State = EntityState.Modified;
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
                            && cc.Location.projectUid == MCVExtensions.getSessionProject()
                            orderby cn.Contractor.contractorName
                            select cc;
            }
            else
            {
                estimates = from cc in db.Estimates
                            where cc.Location.projectUid == MCVExtensions.getSessionProject()
                            orderby cc.Contractor.contractorName
                            select cc;
            }

            ViewBag.estimateUid = estimates.ToSelectList(d => d.Contractor.contractorName + " - " + d.ECM.ecmString, d => d.estimateUid.ToString(), estimateInclusion.estimateUid.ToString());     
            return View(estimateInclusion);
        }

        // GET: EstimateInclusions/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstimateInclusion estimateInclusion = await db.EstimateInclusions.FindAsync(id);
            if (estimateInclusion == null)
            {
                return HttpNotFound();
            }
            return View(estimateInclusion);
        }

        // POST: EstimateInclusions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            EstimateInclusion estimateInclusion = await db.EstimateInclusions.FindAsync(id);
            db.EstimateInclusions.Remove(estimateInclusion);
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
