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
    public class WeeksController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: Weeks
        public async Task<ActionResult> Index()
        {
            return View(await db.Weeks.ToListAsync());
        }

        // GET: Weeks/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Week week = await db.Weeks.FindAsync(id);
            if (week == null)
            {
                return HttpNotFound();
            }
            return View(week);
        }

        // GET: Weeks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Weeks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "WeekUid,startDate,endDate")] Week week)
        {
            if (ModelState.IsValid)
            {
                week.WeekUid = Guid.NewGuid();
                db.Weeks.Add(week);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(week);
        }

        // GET: Weeks/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Week week = await db.Weeks.FindAsync(id);
            if (week == null)
            {
                return HttpNotFound();
            }
            return View(week);
        }

        // POST: Weeks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "WeekUid,startDate,endDate")] Week week)
        {
            if (ModelState.IsValid)
            {
                db.Entry(week).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(week);
        }

        // GET: Weeks/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Week week = await db.Weeks.FindAsync(id);
            if (week == null)
            {
                return HttpNotFound();
            }
            return View(week);
        }

        // POST: Weeks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Week week = await db.Weeks.FindAsync(id);
            db.Weeks.Remove(week);
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
