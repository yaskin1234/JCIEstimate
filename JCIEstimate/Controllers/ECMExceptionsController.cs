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
    public class ECMExceptionsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ECMExceptions
        public async Task<ActionResult> Index()
        {
            var eCMExceptions = db.ECMExceptions.Include(e => e.ECM);
            return View(await eCMExceptions.ToListAsync());
        }

        // GET: ECMExceptions/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ECMException eCMException = await db.ECMExceptions.FindAsync(id);
            if (eCMException == null)
            {
                return HttpNotFound();
            }
            return View(eCMException);
        }

        // GET: ECMExceptions/Create
        public ActionResult Create()
        {
            ViewBag.ecmUid = new SelectList(db.ECMs, "ecmUid", "ecmNumber");
            return View();
        }

        // POST: ECMExceptions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ecmExceptionUid,ecmExceptionID,ecmUid,ecmException1")] ECMException eCMException)
        {
            if (ModelState.IsValid)
            {
                eCMException.ecmExceptionUid = Guid.NewGuid();
                db.ECMExceptions.Add(eCMException);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ecmUid = new SelectList(db.ECMs, "ecmUid", "ecmNumber", eCMException.ecmUid);
            return View(eCMException);
        }

        // GET: ECMExceptions/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ECMException eCMException = await db.ECMExceptions.FindAsync(id);
            if (eCMException == null)
            {
                return HttpNotFound();
            }
            ViewBag.ecmUid = new SelectList(db.ECMs, "ecmUid", "ecmNumber", eCMException.ecmUid);
            return View(eCMException);
        }

        // POST: ECMExceptions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ecmExceptionUid,ecmExceptionID,ecmUid,ecmException1")] ECMException eCMException)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eCMException).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ecmUid = new SelectList(db.ECMs, "ecmUid", "ecmNumber", eCMException.ecmUid);
            return View(eCMException);
        }

        // GET: ECMExceptions/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ECMException eCMException = await db.ECMExceptions.FindAsync(id);
            if (eCMException == null)
            {
                return HttpNotFound();
            }
            return View(eCMException);
        }

        // POST: ECMExceptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ECMException eCMException = await db.ECMExceptions.FindAsync(id);
            db.ECMExceptions.Remove(eCMException);
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
