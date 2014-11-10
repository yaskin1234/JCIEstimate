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
    public class ECMExceptionResponsesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ECMExceptionResponses
        public async Task<ActionResult> Index()
        {
            var eCMExceptionResponses = db.ECMExceptionResponses.Include(e => e.ECMException);
            return View(await eCMExceptionResponses.ToListAsync());
        }

        // GET: ECMExceptionResponses/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ECMExceptionResponse eCMExceptionResponse = await db.ECMExceptionResponses.FindAsync(id);
            if (eCMExceptionResponse == null)
            {
                return HttpNotFound();
            }
            return View(eCMExceptionResponse);
        }

        // GET: ECMExceptionResponses/Create
        public ActionResult Create()
        {
            ViewBag.ecmExceptionUid = new SelectList(db.ECMExceptions, "ecmExceptionUid", "ecmException1");
            return View();
        }

        // POST: ECMExceptionResponses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ecmExceptionResponseUid,ecmExceptionResponseID,ecmExceptionUid,ecmExceptionResponse1")] ECMExceptionResponse eCMExceptionResponse)
        {
            if (ModelState.IsValid)
            {
                eCMExceptionResponse.ecmExceptionResponseUid = Guid.NewGuid();
                db.ECMExceptionResponses.Add(eCMExceptionResponse);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ecmExceptionUid = new SelectList(db.ECMExceptions, "ecmExceptionUid", "ecmException1", eCMExceptionResponse.ecmExceptionUid);
            return View(eCMExceptionResponse);
        }

        // GET: ECMExceptionResponses/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ECMExceptionResponse eCMExceptionResponse = await db.ECMExceptionResponses.FindAsync(id);
            if (eCMExceptionResponse == null)
            {
                return HttpNotFound();
            }
            ViewBag.ecmExceptionUid = new SelectList(db.ECMExceptions, "ecmExceptionUid", "ecmException1", eCMExceptionResponse.ecmExceptionUid);
            return View(eCMExceptionResponse);
        }

        // POST: ECMExceptionResponses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ecmExceptionResponseUid,ecmExceptionResponseID,ecmExceptionUid,ecmExceptionResponse1")] ECMExceptionResponse eCMExceptionResponse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eCMExceptionResponse).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ecmExceptionUid = new SelectList(db.ECMExceptions, "ecmExceptionUid", "ecmException1", eCMExceptionResponse.ecmExceptionUid);
            return View(eCMExceptionResponse);
        }

        // GET: ECMExceptionResponses/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ECMExceptionResponse eCMExceptionResponse = await db.ECMExceptionResponses.FindAsync(id);
            if (eCMExceptionResponse == null)
            {
                return HttpNotFound();
            }
            return View(eCMExceptionResponse);
        }

        // POST: ECMExceptionResponses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ECMExceptionResponse eCMExceptionResponse = await db.ECMExceptionResponses.FindAsync(id);
            db.ECMExceptionResponses.Remove(eCMExceptionResponse);
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
