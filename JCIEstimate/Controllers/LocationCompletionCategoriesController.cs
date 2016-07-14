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
    public class LocationCompletionCategoriesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: LocationCompletionCategories
        public async Task<ActionResult> Index()
        {
            var locationCompletionCategories = db.LocationCompletionCategories.Include(l => l.CompletionCategory).Include(l => l.Location).Include(l => l.Project);
            return View(await locationCompletionCategories.ToListAsync());
        }

        // GET: LocationCompletionCategories/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocationCompletionCategory locationCompletionCategory = await db.LocationCompletionCategories.FindAsync(id);
            if (locationCompletionCategory == null)
            {
                return HttpNotFound();
            }
            return View(locationCompletionCategory);
        }

        // GET: LocationCompletionCategories/Create
        public ActionResult Create()
        {
            ViewBag.completionCategoryUid = new SelectList(db.CompletionCategories, "completionCategoryUid", "completionCategory1");
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1");
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1");
            return View();
        }

        // POST: LocationCompletionCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "locationCompletionCategoryUid,projectUid,locationUid,completionCategoryUid")] LocationCompletionCategory locationCompletionCategory)
        {
            if (ModelState.IsValid)
            {
                locationCompletionCategory.locationCompletionCategoryUid = Guid.NewGuid();
                db.LocationCompletionCategories.Add(locationCompletionCategory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.completionCategoryUid = new SelectList(db.CompletionCategories, "completionCategoryUid", "completionCategory1", locationCompletionCategory.completionCategoryUid);
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1", locationCompletionCategory.locationUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", locationCompletionCategory.projectUid);
            return View(locationCompletionCategory);
        }


        // GET: LocationCompletionCategories/SaveCheckedBox/5
        public async Task<ActionResult> SaveCheckedBox(string chkBoxName, string value)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            string[] incomingValues;
            incomingValues = chkBoxName.Split('_');
            if (value == "true")
            {
                LocationCompletionCategory newLCC = new LocationCompletionCategory();
                newLCC.locationUid = new Guid(incomingValues[0]);
                newLCC.completionCategoryUid = new Guid(incomingValues[1]);
                newLCC.locationCompletionCategoryUid = Guid.NewGuid();
                newLCC.projectUid = sessionProject;
                try
                {
                    db.LocationCompletionCategories.Add(newLCC);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    return Json("error: " + ex.Message);
                }
            }
            else
            {
                Guid locationUid = new Guid(incomingValues[0].ToString());
                Guid completionCategoryUid = new Guid(incomingValues[1].ToString());
                var id = from cc in db.LocationCompletionCategories
                         where cc.locationUid == locationUid
                         && cc.completionCategoryUid == completionCategoryUid
                         select cc.locationCompletionCategoryUid;
                try
                {
                    LocationCompletionCategory locationCompletionCategory = await db.LocationCompletionCategories.FindAsync(id.First());
                    db.LocationCompletionCategories.Remove(locationCompletionCategory);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return Json("error: " + ex.Message);
                }

            }
            return Json("success");
        }

        // GET: LocationCompletionCategories/Edit/5
        public async Task<ActionResult> Edit()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            ViewBag.locations = db.Locations.Where(c => c.projectUid == sessionProject).OrderBy(c => c.location1);
            ViewBag.completionCategories = db.CompletionCategories.OrderBy(c => c.behaviorIndicator);
            ViewBag.locationCompletionCategories = db.LocationCompletionCategories.Where(c => c.projectUid == sessionProject);

            return View();
        }

        // POST: LocationCompletionCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "locationCompletionCategoryUid,projectUid,locationUid,completionCategoryUid")] LocationCompletionCategory locationCompletionCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(locationCompletionCategory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.completionCategoryUid = new SelectList(db.CompletionCategories, "completionCategoryUid", "completionCategory1", locationCompletionCategory.completionCategoryUid);
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1", locationCompletionCategory.locationUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", locationCompletionCategory.projectUid);
            return View(locationCompletionCategory);
        }

        // GET: LocationCompletionCategories/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocationCompletionCategory locationCompletionCategory = await db.LocationCompletionCategories.FindAsync(id);
            if (locationCompletionCategory == null)
            {
                return HttpNotFound();
            }
            return View(locationCompletionCategory);
        }

        // POST: LocationCompletionCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            LocationCompletionCategory locationCompletionCategory = await db.LocationCompletionCategories.FindAsync(id);
            db.LocationCompletionCategories.Remove(locationCompletionCategory);
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
