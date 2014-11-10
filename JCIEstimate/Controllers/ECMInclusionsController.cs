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
    public class ECMInclusionsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ECMInclusions
        public async Task<ActionResult> Index()
        {
            var eCMInclusions = db.ECMInclusions.Include(e => e.ECM);
            return View(await eCMInclusions.ToListAsync());
        }

        // GET: ECMInclusions/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ECMInclusion eCMInclusion = await db.ECMInclusions.FindAsync(id);
            if (eCMInclusion == null)
            {
                return HttpNotFound();
            }
            return View(eCMInclusion);
        }

        // GET: ECMInclusions/Create
        public ActionResult Create()
        {
            ViewBag.ecmUid = new SelectList(db.ECMs, "ecmUid", "ecmNumber");
            return View();
        }

        // POST: ECMInclusions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ecmInclusionUid,ecmInclusionID,ecmUid,ecmInclusion1")] ECMInclusion eCMInclusion)
        {
            if (ModelState.IsValid)
            {
                eCMInclusion.ecmInclusionUid = Guid.NewGuid();
                db.ECMInclusions.Add(eCMInclusion);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ecmUid = new SelectList(db.ECMs, "ecmUid", "ecmNumber", eCMInclusion.ecmUid);
            return View(eCMInclusion);
        }

        // GET: ECMInclusions/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ECMInclusion eCMInclusion = await db.ECMInclusions.FindAsync(id);
            if (eCMInclusion == null)
            {
                return HttpNotFound();
            }
            ViewBag.ecmUid = new SelectList(db.ECMs, "ecmUid", "ecmNumber", eCMInclusion.ecmUid);
            return View(eCMInclusion);
        }

        // POST: ECMInclusions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ecmInclusionUid,ecmInclusionID,ecmUid,ecmInclusion1")] ECMInclusion eCMInclusion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eCMInclusion).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ecmUid = new SelectList(db.ECMs, "ecmUid", "ecmNumber", eCMInclusion.ecmUid);
            return View(eCMInclusion);
        }

        // GET: ECMInclusions/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ECMInclusion eCMInclusion = await db.ECMInclusions.FindAsync(id);
            if (eCMInclusion == null)
            {
                return HttpNotFound();
            }
            return View(eCMInclusion);
        }

        // POST: ECMInclusions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ECMInclusion eCMInclusion = await db.ECMInclusions.FindAsync(id);
            db.ECMInclusions.Remove(eCMInclusion);
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
