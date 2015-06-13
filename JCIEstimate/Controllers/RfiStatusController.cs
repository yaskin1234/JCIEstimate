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
    public class RfiStatusController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: RfiStatus
        public async Task<ActionResult> Index()
        {
            return View(await db.RfiStatus.ToListAsync());
        }

        // GET: RfiStatus/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RfiStatu rfiStatu = await db.RfiStatus.FindAsync(id);
            if (rfiStatu == null)
            {
                return HttpNotFound();
            }
            return View(rfiStatu);
        }

        // GET: RfiStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RfiStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "rfiStatusUid,rfiStatus,behaviorIndicator,rowColor,listOrder")] RfiStatu rfiStatu)
        {
            if (ModelState.IsValid)
            {
                rfiStatu.rfiStatusUid = Guid.NewGuid();
                db.RfiStatus.Add(rfiStatu);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(rfiStatu);
        }

        // GET: RfiStatus/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RfiStatu rfiStatu = await db.RfiStatus.FindAsync(id);
            if (rfiStatu == null)
            {
                return HttpNotFound();
            }
            return View(rfiStatu);
        }

        // POST: RfiStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "rfiStatusUid,rfiStatus,behaviorIndicator,rowColor,listOrder")] RfiStatu rfiStatu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rfiStatu).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(rfiStatu);
        }

        // GET: RfiStatus/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RfiStatu rfiStatu = await db.RfiStatus.FindAsync(id);
            if (rfiStatu == null)
            {
                return HttpNotFound();
            }
            return View(rfiStatu);
        }

        // POST: RfiStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            RfiStatu rfiStatu = await db.RfiStatus.FindAsync(id);
            db.RfiStatus.Remove(rfiStatu);
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
