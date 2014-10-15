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

namespace JCIEstimate.Controllers
{
    public class EstimatesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: Estimates
        public async Task<ActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                var estimates = db.Estimates.Include(e => e.Category).Include(e => e.ECM).Include(e => e.Location).Include(e => e.Contractor);
                return View(await estimates.ToListAsync());
            }
            else
            {
                var estimatesLimited = from cc in db.Estimates
                                        join cn in db.ContractorUsers on cc.contractorUid equals cn.contractorUid
                                        join cq in db.AspNetUsers on cn.aspNetUserUid equals cq.Id
                                        where cq.UserName == System.Web.HttpContext.Current.User.Identity.Name
                                        select cc;
                estimatesLimited = estimatesLimited.Include(e => e.Category).Include(e => e.ECM).Include(e => e.Location).Include(e => e.Contractor);
                return View(await estimatesLimited.ToListAsync());
            }
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
        public ActionResult Create()
        {
            ViewBag.categoryUid = new SelectList(db.Categories, "categoryUid", "category1");
            ViewBag.ecmUid = new SelectList(db.ECMs, "ecmUid", "ecmNumber");
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1");
            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName");
            return View();
        }

        // POST: Estimates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "estimateUid,locationUid,ecmUid,categoryUid,isActive,materialBid,laborBid,bondAmount,total,notes,deliveryWeeks,installationWeeks,contractorUid")] Estimate estimate)
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
            return View(estimate);
        }

        // GET: Estimates/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
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
            ViewBag.categoryUid = new SelectList(db.Categories, "categoryUid", "category1", estimate.categoryUid);
            ViewBag.ecmUid = new SelectList(db.ECMs, "ecmUid", "ecmNumber", estimate.ecmUid);
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1", estimate.locationUid);
            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName", estimate.contractorUid);
            return View(estimate);
        }

        // POST: Estimates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "estimateUid,locationUid,ecmUid,categoryUid,isActive,materialBid,laborBid,bondAmount,total,notes,deliveryWeeks,installationWeeks,contractorUid")] Estimate estimate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estimate).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.categoryUid = new SelectList(db.Categories, "categoryUid", "category1", estimate.categoryUid);
            ViewBag.ecmUid = new SelectList(db.ECMs, "ecmUid", "ecmNumber", estimate.ecmUid);
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1", estimate.locationUid);
            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName", estimate.contractorUid);
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
