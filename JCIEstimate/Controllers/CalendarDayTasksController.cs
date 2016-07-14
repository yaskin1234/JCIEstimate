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
    public class CalendarDayTasksController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: CalendarDayTasks
        public async Task<ActionResult> Index()
        {
            var calendarDayTasks = db.CalendarDayTasks.Include(c => c.CalendarDay).Include(c => c.Location).Include(c => c.Project);
            return View(await calendarDayTasks.ToListAsync());
        }

        // GET: CalendarDayTasks/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalendarDayTask calendarDayTask = await db.CalendarDayTasks.FindAsync(id);
            if (calendarDayTask == null)
            {
                return HttpNotFound();
            }
            return View(calendarDayTask);
        }

        // GET: EquipmentToDoes/SaveCheckedBox/5
        public async Task<ActionResult> SaveCalendarDayTask(string id, string value)
        {
            CalendarDayTask cds = db.CalendarDayTasks.Find(Guid.Parse(id));
            db.Entry(cds).State = EntityState.Modified;
            cds.task = value;
            await db.SaveChangesAsync();
            return View();
        }

        // GET: EquipmentToDoes/SaveCheckedBox/5
        public async Task<ActionResult> SaveCalendarDayTaskDate(string id, string value)
        {
            CalendarDayTask cds = db.CalendarDayTasks.Find(Guid.Parse(id));            
            DateTime newDate = DateTime.Parse(value);
            db.Entry(cds).State = EntityState.Modified;
            var calendarUid = from cc in db.CalendarDayTasks
                              join dd in db.CalendarDays on cc.calendarDayUid equals dd.calendarDayUid
                              where cc.calendarDayTaskUid == cds.calendarDayTaskUid
                              select dd.calendarUid;                              

            var calendarDayUid = from cc in db.CalendarDays
                                 where cc.date == newDate             
                                 && cc.calendarUid == calendarUid.FirstOrDefault()
                                 select cc.calendarDayUid;

            cds.calendarDayUid = calendarDayUid.FirstOrDefault();
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                return Json("error: " + ex.Message);
            }
            
            return Json("success");
        }


        // GET: CalendarDayTasks/Create
        public ActionResult Create()
        {
            ViewBag.calendarDayUid = new SelectList(db.CalendarDays, "calendarDayUid", "calendarDayUid");
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1");
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1");
            return View();
        }

        // POST: CalendarDayTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "calendarDayTaskUid,calendarDayUid,projectUid,locationUid,task")] CalendarDayTask calendarDayTask)
        {
            if (ModelState.IsValid)
            {
                var calendarUid = from cc in db.CalendarDays
                                  where cc.calendarDayUid == calendarDayTask.calendarDayUid
                                  select cc.calendarUid;

                calendarDayTask.calendarDayTaskUid = Guid.NewGuid();
                if (calendarDayTask.locationUid == Guid.Empty)
                {
                    calendarDayTask.locationUid = null;
                }
                if (calendarDayTask.projectUid == Guid.Empty)
                {
                    calendarDayTask.projectUid = null;
                }
                db.CalendarDayTasks.Add(calendarDayTask);
                await db.SaveChangesAsync();
                return RedirectToAction("Edit", "Calendars", new { @id = calendarUid.FirstOrDefault() });
            }

            ViewBag.calendarDayUid = new SelectList(db.CalendarDays, "calendarDayUid", "calendarDayUid", calendarDayTask.calendarDayUid);
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1", calendarDayTask.locationUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", calendarDayTask.projectUid);
            return View(calendarDayTask);
        }

        // GET: CalendarDayTasks/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalendarDayTask calendarDayTask = await db.CalendarDayTasks.FindAsync(id);
            if (calendarDayTask == null)
            {
                return HttpNotFound();
            }
            ViewBag.calendarDayUid = new SelectList(db.CalendarDays, "calendarDayUid", "calendarDayUid", calendarDayTask.calendarDayUid);
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1", calendarDayTask.locationUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", calendarDayTask.projectUid);
            return View(calendarDayTask);
        }

        // POST: CalendarDayTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "calendarDayTaskUid,calendarDayUid,projectUid,locationUid,task")] CalendarDayTask calendarDayTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(calendarDayTask).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.calendarDayUid = new SelectList(db.CalendarDays, "calendarDayUid", "calendarDayUid", calendarDayTask.calendarDayUid);
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1", calendarDayTask.locationUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", calendarDayTask.projectUid);
            return View(calendarDayTask);
        }

        // GET: CalendarDayTasks/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalendarDayTask calendarDayTask = await db.CalendarDayTasks.FindAsync(id);
            if (calendarDayTask == null)
            {
                return HttpNotFound();
            }
            return View(calendarDayTask);
        }

        // POST: CalendarDayTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            CalendarDayTask calendarDayTask = await db.CalendarDayTasks.FindAsync(id);
            var calendarUid = from cc in db.CalendarDays
                              where cc.calendarDayUid == calendarDayTask.calendarDayUid
                              select cc.calendarUid;
                        
            db.CalendarDayTasks.Remove(calendarDayTask);
            await db.SaveChangesAsync();
            return RedirectToAction("Edit", "Calendars", new { @id = calendarUid.FirstOrDefault() });
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
