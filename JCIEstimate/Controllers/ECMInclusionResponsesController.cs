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
    public class ECMInclusionResponsesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ECMInclusionResponses
        public async Task<ActionResult> Index()
        {
            var eCMInclusionResponses = db.ECMInclusionResponses.Include(e => e.ECMInclusion);
            return View(await eCMInclusionResponses.ToListAsync());
        }

        // GET: ECMInclusionResponses/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ECMInclusionResponse eCMInclusionResponse = await db.ECMInclusionResponses.FindAsync(id);
            if (eCMInclusionResponse == null)
            {
                return HttpNotFound();
            }
            return View(eCMInclusionResponse);
        }

        // GET: ECMInclusionResponses/Create
        public ActionResult Create()
        {
            ViewBag.ecmInclusionUid = new SelectList(db.ECMInclusions, "ecmInclusionUid", "ecmInclusion1");
            return View();
        }

        // POST: ECMInclusionResponses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ecmInclusionResponseUid,ecmInclusionResponseID,ecmInclusionUid,ecmInclusionResponse1")] ECMInclusionResponse eCMInclusionResponse)
        {
            if (ModelState.IsValid)
            {
                eCMInclusionResponse.ecmInclusionResponseUid = Guid.NewGuid();
                db.ECMInclusionResponses.Add(eCMInclusionResponse);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ecmInclusionUid = new SelectList(db.ECMInclusions, "ecmInclusionUid", "ecmInclusion1", eCMInclusionResponse.ecmInclusionUid);
            return View(eCMInclusionResponse);
        }

        // GET: ECMInclusionResponses/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ECMInclusionResponse eCMInclusionResponse = await db.ECMInclusionResponses.FindAsync(id);
            if (eCMInclusionResponse == null)
            {
                return HttpNotFound();
            }
            ViewBag.ecmInclusionUid = new SelectList(db.ECMInclusions, "ecmInclusionUid", "ecmInclusion1", eCMInclusionResponse.ecmInclusionUid);
            return View(eCMInclusionResponse);
        }

        // POST: ECMInclusionResponses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ecmInclusionResponseUid,ecmInclusionResponseID,ecmInclusionUid,ecmInclusionResponse1")] ECMInclusionResponse eCMInclusionResponse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eCMInclusionResponse).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ecmInclusionUid = new SelectList(db.ECMInclusions, "ecmInclusionUid", "ecmInclusion1", eCMInclusionResponse.ecmInclusionUid);
            return View(eCMInclusionResponse);
        }

        // GET: ECMInclusionResponses/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ECMInclusionResponse eCMInclusionResponse = await db.ECMInclusionResponses.FindAsync(id);
            if (eCMInclusionResponse == null)
            {
                return HttpNotFound();
            }
            return View(eCMInclusionResponse);
        }

        // POST: ECMInclusionResponses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ECMInclusionResponse eCMInclusionResponse = await db.ECMInclusionResponses.FindAsync(id);
            db.ECMInclusionResponses.Remove(eCMInclusionResponse);
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
