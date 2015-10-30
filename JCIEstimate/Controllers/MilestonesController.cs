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
    public class MilestonesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: Milestones
        public async Task<ActionResult> Index()
        {
            return View(await db.Milestones.OrderBy(c=>c.defaultListOrder).ToListAsync());
        }

        // GET: Milestones/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Milestone milestone = await db.Milestones.FindAsync(id);
            if (milestone == null)
            {
                return HttpNotFound();
            }
            return View(milestone);
        }

        // GET: Milestones/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Milestones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "milestoneUid,milestone1,milestoneDescription,defaultListOrder")] Milestone milestone)
        {
            if (ModelState.IsValid)
            {
                milestone.milestoneUid = Guid.NewGuid();
                db.Milestones.Add(milestone);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(milestone);
        }

        // GET: Milestones/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Milestone milestone = await db.Milestones.FindAsync(id);
            if (milestone == null)
            {
                return HttpNotFound();
            }
            return View(milestone);
        }

        // POST: Milestones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "milestoneUid,milestone1,milestoneDescription,defaultListOrder")] Milestone milestone)
        {
            if (ModelState.IsValid)
            {
                db.Entry(milestone).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(milestone);
        }

        // GET: Milestones/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Milestone milestone = await db.Milestones.FindAsync(id);
            if (milestone == null)
            {
                return HttpNotFound();
            }
            return View(milestone);
        }

        // POST: Milestones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Milestone milestone = await db.Milestones.FindAsync(id);
            db.Milestones.Remove(milestone);
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
