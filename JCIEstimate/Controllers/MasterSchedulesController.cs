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
    public class MasterSchedulesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: MasterSchedules
        public async Task<ActionResult> Index()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            var masterSchedules = db.MasterSchedules.Where(c=>c.projectUid == sessionProject).Include(m => m.Location).Include(m => m.Project);
            ViewBag.masterScheduleTaskUidAsPredecessor = db.MasterScheduleTasks.Where(c => c.MasterSchedule.projectUid == sessionProject).OrderBy(c => c.taskSequence).ToSelectList(c => c.taskName, c => c.masterScheduleTaskUid.ToString(), "");
            ViewBag.MasterScheduleTasks = db.MasterScheduleTasks.Where(c => c.MasterSchedule.projectUid == sessionProject).OrderBy(c=>c.taskSequence).ToList();
            return View(await masterSchedules.ToListAsync());
        }

        // GET: MasterSchedules/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MasterSchedule masterSchedule = await db.MasterSchedules.FindAsync(id);
            if (masterSchedule == null)
            {
                return HttpNotFound();
            }
            return View(masterSchedule);
        }

        public async Task<ActionResult> SaveMasterTask(string field, string identifier, string value)
        {
            try
            {
                Guid id = new Guid(identifier);
                MasterScheduleTask masterScheduleTask = await db.MasterScheduleTasks.FindAsync(id);

                if (masterScheduleTask == null)
                {
                    return HttpNotFound();
                }

                if (field == "taskSequence")
                {
                    masterScheduleTask.taskSequence = Convert.ToInt16(value);
                }
                else if (field == "taskName")
                {
                    masterScheduleTask.taskName = value;
                }
                else if (field == "masterScheduleIdAsPredecessors")
                {
                    masterScheduleTask.masterScheduleIdAsPredecessors = value;
                }

                db.SaveChanges();
            }
            catch (Exception e)
            {

                throw e;
            }



            return PartialView();
        }

        // GET: MasterSchedules/Create
        public ActionResult Create()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            ViewBag.locationUid = new SelectList(db.Locations.OrderBy(c=>c.location1).Where(c => c.projectUid == sessionProject), "locationUid", "location1");
            ViewBag.projectUid = new SelectList(db.Projects.Where(c => c.projectUid == sessionProject), "projectUid", "project1");
            return View();
        }

        // POST: MasterSchedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "masterScheduleUid,projectUid,locationUid,masterSchedule1,room,description")] MasterSchedule masterSchedule)
        {
            if (ModelState.IsValid)
            {
                masterSchedule.masterScheduleUid = Guid.NewGuid();
                db.MasterSchedules.Add(masterSchedule);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1", masterSchedule.locationUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", masterSchedule.projectUid);
            return View(masterSchedule);
        }

        // GET: MasterSchedules/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MasterSchedule masterSchedule = await db.MasterSchedules.FindAsync(id);
            if (masterSchedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.locationUid = new SelectList(db.Locations.OrderBy(c => c.location1).Where(c => c.projectUid == sessionProject), "locationUid", "location1", masterSchedule.locationUid);
            ViewBag.projectUid = new SelectList(db.Projects.Where(c => c.projectUid == sessionProject), "projectUid", "project1", masterSchedule.projectUid);
            return View(masterSchedule);
        }

        // POST: MasterSchedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "masterScheduleUid,projectUid,locationUid,masterSchedule1,room,description")] MasterSchedule masterSchedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(masterSchedule).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1", masterSchedule.locationUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", masterSchedule.projectUid);
            return View(masterSchedule);
        }

        // GET: MasterSchedules/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MasterSchedule masterSchedule = await db.MasterSchedules.FindAsync(id);
            if (masterSchedule == null)
            {
                return HttpNotFound();
            }
            return View(masterSchedule);
        }

        // POST: MasterSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            MasterSchedule masterSchedule = await db.MasterSchedules.FindAsync(id);
            db.MasterSchedules.Remove(masterSchedule);
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
