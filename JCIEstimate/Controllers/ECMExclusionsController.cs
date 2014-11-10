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
    public class ECMExclusionsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ECMExclusions
        public async Task<ActionResult> Index()
        {
            var eCMExclusions = db.ECMExclusions.Include(e => e.ECM);
            return View(await eCMExclusions.ToListAsync());
        }

        // GET: ECMExclusions/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ECMExclusion eCMExclusion = await db.ECMExclusions.FindAsync(id);
            if (eCMExclusion == null)
            {
                return HttpNotFound();
            }
            return View(eCMExclusion);
        }

        // GET: ECMExclusions/Create
        public ActionResult Create()
        {
            ViewBag.ecmUid = new SelectList(db.ECMs, "ecmUid", "ecmNumber");
            return View();
        }

        // POST: ECMExclusions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ecmExclusionUid,ecmExclusionID,ecmUid,ecmExclusion1")] ECMExclusion eCMExclusion)
        {
            if (ModelState.IsValid)
            {
                eCMExclusion.ecmExclusionUid = Guid.NewGuid();
                db.ECMExclusions.Add(eCMExclusion);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ecmUid = new SelectList(db.ECMs, "ecmUid", "ecmNumber", eCMExclusion.ecmUid);
            return View(eCMExclusion);
        }

        // GET: ECMExclusions/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ECMExclusion eCMExclusion = await db.ECMExclusions.FindAsync(id);
            if (eCMExclusion == null)
            {
                return HttpNotFound();
            }
            ViewBag.ecmUid = new SelectList(db.ECMs, "ecmUid", "ecmNumber", eCMExclusion.ecmUid);
            return View(eCMExclusion);
        }

        // POST: ECMExclusions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ecmExclusionUid,ecmExclusionID,ecmUid,ecmExclusion1")] ECMExclusion eCMExclusion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eCMExclusion).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ecmUid = new SelectList(db.ECMs, "ecmUid", "ecmNumber", eCMExclusion.ecmUid);
            return View(eCMExclusion);
        }

        // GET: ECMExclusions/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ECMExclusion eCMExclusion = await db.ECMExclusions.FindAsync(id);
            if (eCMExclusion == null)
            {
                return HttpNotFound();
            }
            return View(eCMExclusion);
        }

        // POST: ECMExclusions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ECMExclusion eCMExclusion = await db.ECMExclusions.FindAsync(id);
            db.ECMExclusions.Remove(eCMExclusion);
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
