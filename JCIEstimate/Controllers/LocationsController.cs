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
    public class LocationsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: Locations
        public async Task<ActionResult> Index()
        {
            IQueryable<Location> locations;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            locations = from cc in db.Locations
                        where cc.projectUid == sessionProject
                        select cc;
                
            locations = locations.Include(l => l.Project);
            return View(await locations.ToListAsync());
        }

        // GET: Locations/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = await db.Locations.FindAsync(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // GET: Locations/Create
        public ActionResult Create()
        {
            IQueryable<Project> projects;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            projects = from cc in db.Projects
                        where cc.projectUid == sessionProject
                        select cc;

            ViewBag.projectUid = new SelectList(projects, "projectUid", "project1");
            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "locationUid,location1,locationDescription,projectUid")] Location location)
        {
            if (ModelState.IsValid)
            {
                location.locationUid = Guid.NewGuid();
                db.Locations.Add(location);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            IQueryable<Project> projects;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            projects = from cc in db.Projects
                       where cc.projectUid == sessionProject
                       select cc;

            ViewBag.projectUid = new SelectList(projects, "projectUid", "project1");

            return View(location);
        }

        // GET: Locations/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = await db.Locations.FindAsync(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            IQueryable<Project> projects;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            projects = from cc in db.Projects
                       where cc.projectUid == sessionProject
                       select cc;

            ViewBag.projectUid = new SelectList(projects, "projectUid", "project1");
            return View(location);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "locationUid,location1,locationDescription,projectUid")] Location location)
        {
            if (ModelState.IsValid)
            {
                db.Entry(location).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            IQueryable<Project> projects;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            projects = from cc in db.Projects
                       where cc.projectUid == sessionProject
                       select cc;

            ViewBag.projectUid = new SelectList(projects, "projectUid", "project1");
            return View(location);
        }

        // GET: Locations/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = await db.Locations.FindAsync(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Location location = await db.Locations.FindAsync(id);
            db.Locations.Remove(location);
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
