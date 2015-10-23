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
using JCIExtensions;

namespace JCIEstimate.Controllers
{
    public class MasterScheduleTasksController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: MasterScheduleTasks
        public async Task<ActionResult> Index()
        {
            var masterScheduleTasks = db.MasterScheduleTasks.Include(m => m.MasterSchedule);
            return View(await masterScheduleTasks.ToListAsync());
        }

        // GET: MasterScheduleTasks/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MasterScheduleTask masterScheduleTask = await db.MasterScheduleTasks.FindAsync(id);
            if (masterScheduleTask == null)
            {
                return HttpNotFound();
            }
            return View(masterScheduleTask);
        }

        // GET: MasterScheduleTasks/Create
        public ActionResult Create()
        {
            ViewBag.masterScheduleUid = new SelectList(db.MasterSchedules, "masterScheduleUid", "masterSchedule1");            
            return View();
        }

        // POST: MasterScheduleTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "masterScheduleTaskUid,masterScheduleUid,taskName,taskSequence,masterScheduleIdAsPredecessors")] MasterScheduleTask masterScheduleTask)
        {
            if (ModelState.IsValid)
            {                
                masterScheduleTask.masterScheduleTaskUid = Guid.NewGuid();
                db.MasterScheduleTasks.Add(masterScheduleTask);
                foreach (var cs in db.ContractorSchedules.Where(c=>c.masterScheduleUid == masterScheduleTask.masterScheduleUid))
                {
                    ContractorScheduleTask cst = new ContractorScheduleTask();
                    cst.contractorScheduleTaskUid = Guid.NewGuid();
                    cst.contractorScheduleUid = cs.contractorScheduleUid;
                    cst.masterScheduleTaskUid = masterScheduleTask.masterScheduleTaskUid;
                    db.ContractorScheduleTasks.Add(cst);
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Edit", "MasterSchedules", new { id = masterScheduleTask.masterScheduleUid });
            }

            ViewBag.masterScheduleUid = new SelectList(db.MasterSchedules, "masterScheduleUid", "masterSchedule1", masterScheduleTask.masterScheduleUid);            
            return View(masterScheduleTask);
        }

        // GET: MasterScheduleTasks/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MasterScheduleTask masterScheduleTask = await db.MasterScheduleTasks.FindAsync(id);
            if (masterScheduleTask == null)
            {
                return HttpNotFound();
            }
            ViewBag.masterScheduleUid = new SelectList(db.MasterSchedules, "masterScheduleUid", "masterSchedule1", masterScheduleTask.masterScheduleUid);            
            return View(masterScheduleTask);
        }

        // POST: MasterScheduleTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "masterScheduleTaskUid,masterScheduleUid,taskName,taskSequence,masterScheduleIdAsPredecessors")] MasterScheduleTask masterScheduleTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(masterScheduleTask).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.masterScheduleUid = new SelectList(db.MasterSchedules, "masterScheduleUid", "masterSchedule1", masterScheduleTask.masterScheduleUid);            
            return View(masterScheduleTask);
        }

        // GET: MasterScheduleTasks/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MasterScheduleTask masterScheduleTask = await db.MasterScheduleTasks.FindAsync(id);
            if (masterScheduleTask == null)
            {
                return HttpNotFound();
            }
            return View(masterScheduleTask);
        }

        // POST: MasterScheduleTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            MasterScheduleTask masterScheduleTask = await db.MasterScheduleTasks.FindAsync(id);
            db.MasterScheduleTasks.Remove(masterScheduleTask);
            await db.SaveChangesAsync();
            return RedirectToAction("Edit", "MasterSchedules", new { id = masterScheduleTask.masterScheduleUid });
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
