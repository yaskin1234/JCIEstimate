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
    public class WarrantyUnitsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: WarrantyUnits
        public async Task<ActionResult> Index()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            var warrantyUnits = db.WarrantyUnits.Where(c => c.Location.projectUid == sessionProject).OrderBy(c=>c.warrantyUnit1);

            return View(await warrantyUnits.ToListAsync());
        }

        // GET: WarrantyUnits/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WarrantyUnit warrantyUnit = await db.WarrantyUnits.FindAsync(id);
            if (warrantyUnit == null)
            {
                return HttpNotFound();
            }
            return View(warrantyUnit);
        }

        // GET: WarrantyUnits/Create
        public ActionResult Create()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            var locations = db.Locations.Where(c => c.projectUid == sessionProject);
            ViewBag.locationUid = new SelectList(locations, "locationUid", "location1");
            return View();
        }

        // POST: WarrantyUnits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "warrantyUnitUid,locationUid,warrantyUnit1,warrantyUnitDescription")] WarrantyUnit warrantyUnit)
        {
            if (ModelState.IsValid)
            {
                warrantyUnit.warrantyUnitUid = Guid.NewGuid();
                db.WarrantyUnits.Add(warrantyUnit);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(warrantyUnit);
        }

        // GET: WarrantyUnits/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WarrantyUnit warrantyUnit = await db.WarrantyUnits.FindAsync(id);
            if (warrantyUnit == null)
            {
                return HttpNotFound();
            }

            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            var locations = db.Locations.Where(c => c.projectUid == sessionProject);
            ViewBag.locationUid = new SelectList(locations, "locationUid", "location1");

            return View(warrantyUnit);
        }

        // POST: WarrantyUnits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "warrantyUnitUid,locationUid,warrantyUnit1,warrantyUnitDescription")] WarrantyUnit warrantyUnit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(warrantyUnit).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(warrantyUnit);
        }

        // GET: WarrantyUnits/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WarrantyUnit warrantyUnit = await db.WarrantyUnits.FindAsync(id);
            if (warrantyUnit == null)
            {
                return HttpNotFound();
            }
            return View(warrantyUnit);
        }

        // POST: WarrantyUnits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            WarrantyUnit warrantyUnit = await db.WarrantyUnits.FindAsync(id);
            db.WarrantyUnits.Remove(warrantyUnit);
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
