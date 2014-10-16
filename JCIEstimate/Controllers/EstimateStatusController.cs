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
    public class EstimateStatusController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: EstimateStatus
        public async Task<ActionResult> Index()
        {
            return View(await db.EstimateStatus.ToListAsync());
        }

        // GET: EstimateStatus/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstimateStatu estimateStatu = await db.EstimateStatus.FindAsync(id);
            if (estimateStatu == null)
            {
                return HttpNotFound();
            }
            return View(estimateStatu);
        }

        // GET: EstimateStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EstimateStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "estimateStatusUid,estimateStatus,estimateStatusDescription,behaviorIndicator")] EstimateStatu estimateStatu)
        {
            if (ModelState.IsValid)
            {
                estimateStatu.estimateStatusUid = Guid.NewGuid();
                db.EstimateStatus.Add(estimateStatu);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(estimateStatu);
        }

        // GET: EstimateStatus/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstimateStatu estimateStatu = await db.EstimateStatus.FindAsync(id);
            if (estimateStatu == null)
            {
                return HttpNotFound();
            }
            return View(estimateStatu);
        }

        // POST: EstimateStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "estimateStatusUid,estimateStatus,estimateStatusDescription,behaviorIndicator")] EstimateStatu estimateStatu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estimateStatu).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(estimateStatu);
        }

        // GET: EstimateStatus/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstimateStatu estimateStatu = await db.EstimateStatus.FindAsync(id);
            if (estimateStatu == null)
            {
                return HttpNotFound();
            }
            return View(estimateStatu);
        }

        // POST: EstimateStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            EstimateStatu estimateStatu = await db.EstimateStatus.FindAsync(id);
            db.EstimateStatus.Remove(estimateStatu);
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
