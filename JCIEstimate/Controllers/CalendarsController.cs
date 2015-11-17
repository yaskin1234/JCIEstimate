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
    public class CalendarsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: Calendars
        public async Task<ActionResult> Index()
        {
            var calendars = db.Calendars.Include(c => c.AspNetUser);
            return View(await calendars.ToListAsync());
        }

        // GET: Calendars/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Calendar calendar = await db.Calendars.FindAsync(id);
            if (calendar == null)
            {
                return HttpNotFound();
            }
            return View(calendar);
        }

        // GET: Calendars/Create
        public ActionResult Create()
        {
            ViewBag.aspNetUserUid = new SelectList(db.AspNetUsers.OrderBy(c=>c.Email), "Id", "Email");
            return View();
        }

        // POST: Calendars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "calendarUid,aspNetUserUid,calendar1")] Calendar calendar)
        {
            if (ModelState.IsValid)
            {
                calendar.calendarUid = Guid.NewGuid();

                var user = from cc in db.AspNetUsers
                           where cc.Id == calendar.aspNetUserUid
                           select cc;

                calendar.calendar1 = "Calendar for " + user.FirstOrDefault().Email;
                db.Calendars.Add(calendar);                
                DateTime dDate = DateTime.Now;                
                while (dDate < DateTime.Now.AddYears(5))
                {
                    CalendarDay cd = new CalendarDay();
                    cd.calendarDayUid = Guid.NewGuid();
                    cd.calendarUid = calendar.calendarUid;
                    cd.date = dDate;
                    db.CalendarDays.Add(cd);
                    dDate = dDate.AddDays(1);
                }

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.aspNetUserUid = new SelectList(db.AspNetUsers, "Id", "AllowableContractors", calendar.aspNetUserUid);
            return View(calendar);
        }

        // GET: Calendars/Edit/5
        public async Task<ActionResult> Edit(Guid? id, Guid? projectUid)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Calendar calendar = await db.Calendars.FindAsync(id);
            if (calendar == null)
            {
                return HttpNotFound();
            }
            DateTime endDate = DateTime.Now.AddYears(1);
            ViewBag.aspNetUserUid = new SelectList(db.AspNetUsers, "Id", "AllowableContractors", calendar.aspNetUserUid);
            ViewBag.calendarDays = db.CalendarDays.Where(c => c.calendarUid == calendar.calendarUid).Where(c => c.date <= endDate).OrderBy(c => c.date);
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1");
            ViewBag.projectUid = db.Projects.OrderBy(c => c.project1).ToSelectList(c => c.project1, c => c.projectUid.ToString(), projectUid.ToString());
            ViewBag.projectFilterValue = projectUid;
            return View(calendar);
        }

        public async Task<ActionResult> GetNewCalendarTaskForm(Guid? id)
        {
            ViewBag.locationUid = db.Locations.Where(c=>c.location1 == "").ToSelectList(c=>c.location1, c=>c.locationUid.ToString(), "");
            ViewBag.projectUid = db.Projects.OrderBy(c=>c.project1).ToSelectList(c=>c.project1, c=>c.projectUid.ToString(), "");
            ViewBag.id = id;
            return View();
        }

        public async Task<ActionResult> GetLocationsForProject(Guid? id)
        {
            ViewBag.locationUid = db.Locations.OrderBy(c=>c.location1).Where(c=>c.projectUid == id).ToSelectList(c => c.location1, c => c.locationUid.ToString(), "");
            ViewBag.id = id;
            return View();
        }

        

        // POST: Calendars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "calendarUid,aspNetUserUid,calendar1")] Calendar calendar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(calendar).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.aspNetUserUid = new SelectList(db.AspNetUsers, "Id", "AllowableContractors", calendar.aspNetUserUid);
            return View(calendar);
        }

        // GET: Calendars/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Calendar calendar = await db.Calendars.FindAsync(id);
            if (calendar == null)
            {
                return HttpNotFound();
            }
            return View(calendar);
        }

        // POST: Calendars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Calendar calendar = await db.Calendars.FindAsync(id);
            db.Calendars.Remove(calendar);
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
