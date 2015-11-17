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
    public class CalendarDaysController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: CalendarDays
        public async Task<ActionResult> Index()
        {
            var calendarDays = db.CalendarDays.Include(c => c.Calendar);
            return View(await calendarDays.ToListAsync());
        }

        // GET: CalendarDays/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalendarDay calendarDay = await db.CalendarDays.FindAsync(id);
            if (calendarDay == null)
            {
                return HttpNotFound();
            }
            return View(calendarDay);
        }

        // GET: CalendarDays/Create
        public ActionResult Create()
        {
            ViewBag.calendarUid = new SelectList(db.Calendars, "calendarUid", "aspNetUserUid");
            return View();
        }

        // POST: CalendarDays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "calendarDayUid,calendarUid,date")] CalendarDay calendarDay)
        {
            if (ModelState.IsValid)
            {
                calendarDay.calendarDayUid = Guid.NewGuid();
                db.CalendarDays.Add(calendarDay);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.calendarUid = new SelectList(db.Calendars, "calendarUid", "aspNetUserUid", calendarDay.calendarUid);
            return View(calendarDay);
        }

        // GET: CalendarDays/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalendarDay calendarDay = await db.CalendarDays.FindAsync(id);
            if (calendarDay == null)
            {
                return HttpNotFound();
            }
            ViewBag.calendarUid = new SelectList(db.Calendars, "calendarUid", "aspNetUserUid", calendarDay.calendarUid);
            return View(calendarDay);
        }

        // POST: CalendarDays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "calendarDayUid,calendarUid,date")] CalendarDay calendarDay)
        {
            if (ModelState.IsValid)
            {
                db.Entry(calendarDay).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.calendarUid = new SelectList(db.Calendars, "calendarUid", "aspNetUserUid", calendarDay.calendarUid);
            return View(calendarDay);
        }

        // GET: CalendarDays/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalendarDay calendarDay = await db.CalendarDays.FindAsync(id);
            if (calendarDay == null)
            {
                return HttpNotFound();
            }
            return View(calendarDay);
        }

        // POST: CalendarDays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            CalendarDay calendarDay = await db.CalendarDays.FindAsync(id);
            db.CalendarDays.Remove(calendarDay);
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
