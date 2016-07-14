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
    public class ProjectCalendarDayTasksController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ProjectCalendarDayTasks
        public async Task<ActionResult> Index()
        {
            var projectCalendarDayTasks = db.ProjectCalendarDayTasks.Include(p => p.AspNetUser).Include(p => p.Location).Include(p => p.Project).Include(p => p.ProjectCalendarDay);
            return View(await projectCalendarDayTasks.ToListAsync());
        }

        // GET: ProjectCalendarDayTasks/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectCalendarDayTask projectCalendarDayTask = await db.ProjectCalendarDayTasks.FindAsync(id);
            if (projectCalendarDayTask == null)
            {
                return HttpNotFound();
            }
            return View(projectCalendarDayTask);
        }

        public async Task<ActionResult> SaveProjectCalendarDayTaskDate(string id, string value)
        {
            ProjectCalendarDayTask cds = db.ProjectCalendarDayTasks.Find(Guid.Parse(id));
            DateTime newDate = DateTime.Parse(value);
            db.Entry(cds).State = EntityState.Modified;
            var projectCalendarUid = from cc in db.ProjectCalendarDayTasks
                                     join dd in db.ProjectCalendarDays on cc.projectCalendarDayUid equals dd.projectCalendarDayUid
                                     where cc.projectCalendarDayTaskUid == cds.projectCalendarDayTaskUid
                                     select dd.projectCalendarUid;

            var projectCalendarDayUid = from cc in db.ProjectCalendarDays
                                        where cc.date == newDate
                                        && cc.projectCalendarUid == projectCalendarUid.FirstOrDefault()
                                        select cc.projectCalendarDayUid;

            cds.projectCalendarDayUid = projectCalendarDayUid.FirstOrDefault();
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

        // GET: ProjectCalendarDayTasks/Create
        public ActionResult Create()
        {
            ViewBag.aspnetUserUidAsAssigned = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1");
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1");
            ViewBag.projectCalendarDayUid = new SelectList(db.ProjectCalendarDays, "projectCalendarDayUid", "projectCalendarDayUid");
            return View();
        }

        // POST: ProjectCalendarDayTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "projectCalendarDayTaskUid,projectCalendarDayUid,projectUid,locationUid,aspnetUserUidAsAssigned,taskStartDate,taskDuration,task")] ProjectCalendarDayTask projectCalendarDayTask)
        {
            if (ModelState.IsValid)
            {
                projectCalendarDayTask.projectCalendarDayTaskUid = Guid.NewGuid();

                var projectCalendarUid = from cc in db.ProjectCalendarDays
                                         where cc.projectCalendarDayUid == projectCalendarDayTask.projectCalendarDayUid
                                         select cc.projectCalendarUid;

                projectCalendarDayTask.projectCalendarDayTaskUid = Guid.NewGuid();
                if (projectCalendarDayTask.locationUid == Guid.Empty)
                {
                    projectCalendarDayTask.locationUid = null;
                }
                if (projectCalendarDayTask.aspnetUserUidAsAssigned == Guid.Empty.ToString())
                {
                    projectCalendarDayTask.aspnetUserUidAsAssigned = null;
                }

                db.ProjectCalendarDayTasks.Add(projectCalendarDayTask);
                await db.SaveChangesAsync();
                return RedirectToAction("Edit", "ProjectCalendars", new { @id=projectCalendarUid.FirstOrDefault() });
            }

            ViewBag.aspnetUserUidAsAssigned = new SelectList(db.AspNetUsers, "Id", "Email", projectCalendarDayTask.aspnetUserUidAsAssigned);
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1", projectCalendarDayTask.locationUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", projectCalendarDayTask.projectUid);
            ViewBag.projectCalendarDayUid = new SelectList(db.ProjectCalendarDays, "projectCalendarDayUid", "projectCalendarDayUid", projectCalendarDayTask.projectCalendarDayUid);
            return View(projectCalendarDayTask);
        }

        // GET: EquipmentToDoes/SaveCheckedBox/5
        public async Task<ActionResult> SaveProjectCalendarDayTask(string id, string task, DateTime taskStartDate, int taskDuration)
        {
            ProjectCalendarDayTask cds = db.ProjectCalendarDayTasks.Find(Guid.Parse(id));
            db.Entry(cds).State = EntityState.Modified;
            cds.task = task;
            cds.taskStartDate = taskStartDate;
            cds.taskDuration = taskDuration;
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

        // GET: ProjectCalendarDayTasks/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectCalendarDayTask projectCalendarDayTask = await db.ProjectCalendarDayTasks.FindAsync(id);
            if (projectCalendarDayTask == null)
            {
                return HttpNotFound();
            }
            ViewBag.aspnetUserUidAsAssigned = new SelectList(db.AspNetUsers, "Id", "Email", projectCalendarDayTask.aspnetUserUidAsAssigned);
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1", projectCalendarDayTask.locationUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", projectCalendarDayTask.projectUid);
            ViewBag.projectCalendarDayUid = new SelectList(db.ProjectCalendarDays, "projectCalendarDayUid", "projectCalendarDayUid", projectCalendarDayTask.projectCalendarDayUid);
            return View(projectCalendarDayTask);
        }

        // POST: ProjectCalendarDayTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "projectCalendarDayTaskUid,projectCalendarDayUid,projectUid,locationUid,aspnetUserUidAsAssigned,taskStartDate,taskDuration,task")] ProjectCalendarDayTask projectCalendarDayTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectCalendarDayTask).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.aspnetUserUidAsAssigned = new SelectList(db.AspNetUsers, "Id", "Email", projectCalendarDayTask.aspnetUserUidAsAssigned);
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1", projectCalendarDayTask.locationUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", projectCalendarDayTask.projectUid);
            ViewBag.projectCalendarDayUid = new SelectList(db.ProjectCalendarDays, "projectCalendarDayUid", "projectCalendarDayUid", projectCalendarDayTask.projectCalendarDayUid);
            return View(projectCalendarDayTask);
        }

        // GET: ProjectCalendarDayTasks/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectCalendarDayTask projectCalendarDayTask = await db.ProjectCalendarDayTasks.FindAsync(id);
            if (projectCalendarDayTask == null)
            {
                return HttpNotFound();
            }
            return View(projectCalendarDayTask);
        }

        // POST: ProjectCalendarDayTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ProjectCalendarDayTask projectCalendarDayTask = await db.ProjectCalendarDayTasks.FindAsync(id);
            var projectCalendarUid = projectCalendarDayTask.ProjectCalendarDay.projectCalendarUid;
            db.ProjectCalendarDayTasks.Remove(projectCalendarDayTask);
            await db.SaveChangesAsync();
            return RedirectToAction("Edit", "ProjectCalendars", new { @id = projectCalendarUid });
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
