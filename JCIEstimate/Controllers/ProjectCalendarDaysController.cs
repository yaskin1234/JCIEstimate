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
    public class ProjectCalendarDaysController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ProjectCalendarDays
        public async Task<ActionResult> Index()
        {
            var projectCalendarDays = db.ProjectCalendarDays.Include(p => p.ProjectCalendar);
            return View(await projectCalendarDays.ToListAsync());
        }

        // GET: ProjectCalendarDays/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectCalendarDay projectCalendarDay = await db.ProjectCalendarDays.FindAsync(id);
            if (projectCalendarDay == null)
            {
                return HttpNotFound();
            }
            return View(projectCalendarDay);
        }

        // GET: ProjectCalendarDays/Create
        public ActionResult Create()
        {
            ViewBag.projectCalendarUid = new SelectList(db.ProjectCalendars, "projectCalendarUid", "aspNetUserUid");
            return View();
        }

        // POST: ProjectCalendarDays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "projectCalendarDayUid,projectCalendarUid,date")] ProjectCalendarDay projectCalendarDay)
        {
            if (ModelState.IsValid)
            {
                projectCalendarDay.projectCalendarDayUid = Guid.NewGuid();
                db.ProjectCalendarDays.Add(projectCalendarDay);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.projectCalendarUid = new SelectList(db.ProjectCalendars, "projectCalendarUid", "aspNetUserUid", projectCalendarDay.projectCalendarUid);
            return View(projectCalendarDay);
        }

        // GET: ProjectCalendarDays/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectCalendarDay projectCalendarDay = await db.ProjectCalendarDays.FindAsync(id);
            if (projectCalendarDay == null)
            {
                return HttpNotFound();
            }
            ViewBag.projectCalendarUid = new SelectList(db.ProjectCalendars, "projectCalendarUid", "aspNetUserUid", projectCalendarDay.projectCalendarUid);
            return View(projectCalendarDay);
        }

        // POST: ProjectCalendarDays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "projectCalendarDayUid,projectCalendarUid,date")] ProjectCalendarDay projectCalendarDay)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectCalendarDay).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.projectCalendarUid = new SelectList(db.ProjectCalendars, "projectCalendarUid", "aspNetUserUid", projectCalendarDay.projectCalendarUid);
            return View(projectCalendarDay);
        }

        // GET: ProjectCalendarDays/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectCalendarDay projectCalendarDay = await db.ProjectCalendarDays.FindAsync(id);
            if (projectCalendarDay == null)
            {
                return HttpNotFound();
            }
            return View(projectCalendarDay);
        }

        // POST: ProjectCalendarDays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ProjectCalendarDay projectCalendarDay = await db.ProjectCalendarDays.FindAsync(id);
            db.ProjectCalendarDays.Remove(projectCalendarDay);
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
