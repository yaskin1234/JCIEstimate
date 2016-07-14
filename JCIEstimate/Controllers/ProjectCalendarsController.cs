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
    public class ProjectCalendarsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ProjectCalendars
        public async Task<ActionResult> Index()
        {
            var projectCalendars = db.ProjectCalendars.Include(p => p.AspNetUser);
            return View(await projectCalendars.ToListAsync());
        }

        // GET: ProjectCalendars/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectCalendar projectCalendar = await db.ProjectCalendars.FindAsync(id);
            if (projectCalendar == null)
            {
                return HttpNotFound();
            }
            return View(projectCalendar);
        }

        // GET: ProjectCalendars/Create
        public ActionResult Create()
        {
            ViewBag.aspNetUserUid = new SelectList(db.AspNetUsers.OrderBy(c=>c.Email), "Id", "Email");
            return View();
        }

        // POST: ProjectCalendars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "projectCalendarUid,aspNetUserUid,projectCalendar1")] ProjectCalendar projectCalendar, DateTime startDate)
        {
            if (ModelState.IsValid)
            {
                projectCalendar.projectCalendarUid = Guid.NewGuid();

                var user = from cc in db.AspNetUsers
                           where cc.Id == projectCalendar.aspNetUserUid
                           select cc;

                projectCalendar.projectCalendar1 = "Project Calendar for " + user.FirstOrDefault().Email;
                db.ProjectCalendars.Add(projectCalendar);

                DateTime dDate = startDate;
                while (dDate < startDate.AddYears(5))
                {
                    ProjectCalendarDay cd = new ProjectCalendarDay();
                    cd.projectCalendarDayUid = Guid.NewGuid();
                    cd.projectCalendarUid = projectCalendar.projectCalendarUid;
                    cd.date = dDate;
                    db.ProjectCalendarDays.Add(cd);
                    dDate = dDate.AddDays(1);
                }      

                
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.aspNetUserUid = new SelectList(db.AspNetUsers.OrderBy(c => c.Email), "Id", "Email", projectCalendar.aspNetUserUid);
            return View(projectCalendar);
        }

        public async Task<ActionResult> AddNewProjectToCalendar(Guid projectCalendarUid, Guid projectUid, DateTime startDate)
        {
            var projectCalendar = from cc in db.ProjectCalendars
                                  where cc.projectCalendarUid == projectCalendarUid
                                  select cc;
            
            var lookupDate = startDate;
            foreach (var item in db.ProjectTaskPrototypes.OrderBy(c=>c.sequence))
            {

                while (lookupDate.ToString("dddd") == "Saturday" || lookupDate.ToString("dddd") == "Sunday")
                {
                    lookupDate = lookupDate.AddDays(1);
                }

                var taskDate = from cc in db.ProjectCalendarDays
                               where cc.projectCalendarUid == projectCalendar.FirstOrDefault().projectCalendarUid
                               && cc.date == lookupDate
                               select cc.projectCalendarDayUid;                

                ProjectCalendarDayTask pcd = new ProjectCalendarDayTask();
                pcd.projectCalendarDayTaskUid = Guid.NewGuid();
                pcd.projectUid = projectUid;
                pcd.task = item.projectTaskPrototype1;
                pcd.projectCalendarDayUid = taskDate.FirstOrDefault();
                db.ProjectCalendarDayTasks.Add(pcd);

                lookupDate = lookupDate.AddDays(1);
            }
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

        // GET: ProjectCalendars/Edit/5
        public async Task<ActionResult> Edit(Guid? id, Guid? projectUid)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectCalendar projectCalendar = await db.ProjectCalendars.FindAsync(id);
            if (projectCalendar == null)
            {
                return HttpNotFound();
            }

            var standardTasks = from pc in db.ProjectCalendarDayTasks
                                where db.ProjectTaskPrototypes.Any(m => m.projectTaskPrototype1 == pc.task)
                                select pc;

            var projectUidToAdd = from cc in db.Projects
                                  where !standardTasks.Any()
                                  select cc;

            DateTime endDate = DateTime.Now.AddYears(1);
            ViewBag.aspNetUserUid = new SelectList(db.AspNetUsers.OrderBy(c => c.Email), "Id", "Email", projectCalendar.aspNetUserUid);
            ViewBag.projectUid = db.Projects.OrderBy(c => c.project1).ToSelectList(c=>c.project1, c=>c.projectUid.ToString(), "");
            ViewBag.projectUidAsAdd = db.Projects.Where(c=> !c.ProjectCalendarDayTasks.Any()).OrderBy(c => c.project1).ToSelectList(c => c.project1, c => c.projectUid.ToString(), "");
            ViewBag.calendarDays = db.ProjectCalendarDays.Where(c => c.projectCalendarUid == projectCalendar.projectCalendarUid).Where(c => c.date <= endDate).OrderBy(c => c.date);
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1");
            ViewBag.projectFilterValue = projectUid;
            return View(projectCalendar);
        }

        // POST: ProjectCalendars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "projectCalendarUid,aspNetUserUid,projectCalendar1")] ProjectCalendar projectCalendar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectCalendar).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.aspNetUserUid = new SelectList(db.AspNetUsers.OrderBy(c => c.Email), "Id", "Email", projectCalendar.aspNetUserUid);
            return View(projectCalendar);
        }

        public async Task<ActionResult> SaveProjectCalendarDayTask(string id, string value)
        {
            ProjectCalendarDayTask cds = db.ProjectCalendarDayTasks.Find(Guid.Parse(id));
            db.Entry(cds).State = EntityState.Modified;
            cds.task = value;
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


        public async Task<ActionResult> GetNewProjectCalendarTaskForm(Guid? id)
        {
            ViewBag.aspnetUserUidAsAssigned = db.AspNetUsers.OrderBy(c => c.Email).ToSelectList(c => c.Email, c => c.Id, "");
            ViewBag.locationUid = db.Locations.Where(c => c.location1 == "").ToSelectList(c => c.location1, c => c.locationUid.ToString(), "");
            ViewBag.projectUid = db.Projects.OrderBy(c => c.project1).ToSelectList(c => c.project1, c => c.projectUid.ToString(), "");
            ViewBag.id = id;
            return View();
        }

        // GET: ProjectCalendars/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectCalendar projectCalendar = await db.ProjectCalendars.FindAsync(id);
            if (projectCalendar == null)
            {
                return HttpNotFound();
            }
            return View(projectCalendar);
        }

        // POST: ProjectCalendars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ProjectCalendar projectCalendar = await db.ProjectCalendars.FindAsync(id);
            db.ProjectCalendars.Remove(projectCalendar);
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
