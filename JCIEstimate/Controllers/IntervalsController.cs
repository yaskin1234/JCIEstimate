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
    public class IntervalsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: Intervals
        public async Task<ActionResult> Index()
        {
            return View(await db.Intervals.ToListAsync());
        }

        // GET: Intervals/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interval interval = await db.Intervals.FindAsync(id);
            if (interval == null)
            {
                return HttpNotFound();
            }
            return View(interval);
        }

        // GET: Intervals/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Intervals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "intervalUid,interval1,intervalDescription,behaviorIndicator")] Interval interval)
        {
            if (ModelState.IsValid)
            {
                interval.intervalUid = Guid.NewGuid();
                db.Intervals.Add(interval);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(interval);
        }

        // GET: Intervals/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interval interval = await db.Intervals.FindAsync(id);
            if (interval == null)
            {
                return HttpNotFound();
            }
            return View(interval);
        }

        // POST: Intervals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "intervalUid,interval1,intervalDescription,behaviorIndicator")] Interval interval)
        {
            if (ModelState.IsValid)
            {
                db.Entry(interval).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(interval);
        }

        // GET: Intervals/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interval interval = await db.Intervals.FindAsync(id);
            if (interval == null)
            {
                return HttpNotFound();
            }
            return View(interval);
        }

        // POST: Intervals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Interval interval = await db.Intervals.FindAsync(id);
            db.Intervals.Remove(interval);
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
