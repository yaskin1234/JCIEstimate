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
    public class ECMExclusionResponsesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ECMExclusionResponses
        public async Task<ActionResult> Index()
        {
            var eCMExclusionResponses = db.ECMExclusionResponses.Include(e => e.ECMExclusion);
            return View(await eCMExclusionResponses.ToListAsync());
        }

        // GET: ECMExclusionResponses/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ECMExclusionResponse eCMExclusionResponse = await db.ECMExclusionResponses.FindAsync(id);
            if (eCMExclusionResponse == null)
            {
                return HttpNotFound();
            }
            return View(eCMExclusionResponse);
        }

        // GET: ECMExclusionResponses/Create
        public ActionResult Create()
        {
            ViewBag.ecmExclusionUid = new SelectList(db.ECMExclusions, "ecmExclusionUid", "ecmExclusion1");
            return View();
        }

        // POST: ECMExclusionResponses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ecmExclusionResponseUid,ecmExclusionResponseID,ecmExclusionUid,ecmExclusionResponse1")] ECMExclusionResponse eCMExclusionResponse)
        {
            if (ModelState.IsValid)
            {
                eCMExclusionResponse.ecmExclusionResponseUid = Guid.NewGuid();
                db.ECMExclusionResponses.Add(eCMExclusionResponse);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ecmExclusionUid = new SelectList(db.ECMExclusions, "ecmExclusionUid", "ecmExclusion1", eCMExclusionResponse.ecmExclusionUid);
            return View(eCMExclusionResponse);
        }

        // GET: ECMExclusionResponses/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ECMExclusionResponse eCMExclusionResponse = await db.ECMExclusionResponses.FindAsync(id);
            if (eCMExclusionResponse == null)
            {
                return HttpNotFound();
            }
            ViewBag.ecmExclusionUid = new SelectList(db.ECMExclusions, "ecmExclusionUid", "ecmExclusion1", eCMExclusionResponse.ecmExclusionUid);
            return View(eCMExclusionResponse);
        }

        // POST: ECMExclusionResponses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ecmExclusionResponseUid,ecmExclusionResponseID,ecmExclusionUid,ecmExclusionResponse1")] ECMExclusionResponse eCMExclusionResponse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eCMExclusionResponse).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ecmExclusionUid = new SelectList(db.ECMExclusions, "ecmExclusionUid", "ecmExclusion1", eCMExclusionResponse.ecmExclusionUid);
            return View(eCMExclusionResponse);
        }

        // GET: ECMExclusionResponses/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ECMExclusionResponse eCMExclusionResponse = await db.ECMExclusionResponses.FindAsync(id);
            if (eCMExclusionResponse == null)
            {
                return HttpNotFound();
            }
            return View(eCMExclusionResponse);
        }

        // POST: ECMExclusionResponses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ECMExclusionResponse eCMExclusionResponse = await db.ECMExclusionResponses.FindAsync(id);
            db.ECMExclusionResponses.Remove(eCMExclusionResponse);
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
