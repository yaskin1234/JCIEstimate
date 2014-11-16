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
    public class MilestoneActionsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: MilestoneActions
        public async Task<ActionResult> Index()
        {
            var milestoneActions = db.MilestoneActions.Include(m => m.Milestone);
            return View(await milestoneActions.OrderBy(d => d.Milestone.defaultListOrder).ThenBy(d=>d.defaultListOrder).ToListAsync());
        }

        // GET: MilestoneActions/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MilestoneAction milestoneAction = await db.MilestoneActions.FindAsync(id);
            if (milestoneAction == null)
            {
                return HttpNotFound();
            }
            return View(milestoneAction);
        }

        // GET: MilestoneActions/Create
        public ActionResult Create()
        {
            ViewBag.milestoneUid = new SelectList(db.Milestones, "milestoneUid", "milestone1");
            return View();
        }

        // POST: MilestoneActions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "milestoneActionUid,milestoneUid,milestoneAction1,milestoneActionDescription,defaultListOrder")] MilestoneAction milestoneAction)
        {
            if (ModelState.IsValid)
            {
                milestoneAction.milestoneActionUid = Guid.NewGuid();
                db.MilestoneActions.Add(milestoneAction);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.milestoneUid = new SelectList(db.Milestones, "milestoneUid", "milestone1", milestoneAction.milestoneUid);
            return View(milestoneAction);
        }

        // GET: MilestoneActions/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MilestoneAction milestoneAction = await db.MilestoneActions.FindAsync(id);
            if (milestoneAction == null)
            {
                return HttpNotFound();
            }
            ViewBag.milestoneUid = new SelectList(db.Milestones, "milestoneUid", "milestone1", milestoneAction.milestoneUid);
            return View(milestoneAction);
        }

        // POST: MilestoneActions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "milestoneActionUid,milestoneUid,milestoneAction1,milestoneActionDescription,defaultListOrder")] MilestoneAction milestoneAction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(milestoneAction).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.milestoneUid = new SelectList(db.Milestones, "milestoneUid", "milestone1", milestoneAction.milestoneUid);
            return View(milestoneAction);
        }

        // GET: MilestoneActions/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MilestoneAction milestoneAction = await db.MilestoneActions.FindAsync(id);
            if (milestoneAction == null)
            {
                return HttpNotFound();
            }
            return View(milestoneAction);
        }

        // POST: MilestoneActions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            MilestoneAction milestoneAction = await db.MilestoneActions.FindAsync(id);
            db.MilestoneActions.Remove(milestoneAction);
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
