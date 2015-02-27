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
    public class LocationIssuesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: LocationIssues
        public async Task<ActionResult> Index()
        {
            var locationIssues = db.LocationIssues.Include(l => l.AspNetUser).Include(l => l.AspNetUser1).Include(l => l.Location).Include(l => l.WarrantyStatu);
            return View(await locationIssues.ToListAsync());
        }

        // GET: LocationIssues/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocationIssue locationIssue = await db.LocationIssues.FindAsync(id);
            if (locationIssue == null)
            {
                return HttpNotFound();
            }
            return View(locationIssue);
        }

        // GET: LocationIssues/Create
        public ActionResult Create()
        {
            
            ViewBag.aspNetUserUid = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1");
            ViewBag.warrantyStatusUid = new SelectList(db.WarrantyStatus.OrderBy(d => d.listOrder), "warrantyStatusUid", "warrantyStatus", "");            
            return View();
        }

        // POST: LocationIssues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "locationIssueUid,aspNetUserUidAsCreated,locationUid,warrantyStatusUid,aspNetUserUid,date,locationIssue1")] LocationIssue locationIssue)
        {
            if (ModelState.IsValid)
            {
                var user = from cc in db.AspNetUsers
                           where cc.UserName == System.Web.HttpContext.Current.User.Identity.Name
                           select cc;
                locationIssue.locationIssueUid = Guid.NewGuid();
                locationIssue.aspNetUserUidAsCreated = user.First().Id;
                locationIssue.date = DateTime.Now;
                db.LocationIssues.Add(locationIssue);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.aspNetUserUidAsCreated = new SelectList(db.AspNetUsers, "Id", "AllowableContractors", locationIssue.aspNetUserUidAsCreated);
            ViewBag.aspNetUserUid = new SelectList(db.AspNetUsers, "Id", "AllowableContractors", locationIssue.aspNetUserUid);
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1", locationIssue.locationUid);
            ViewBag.warrantyStatusUid = new SelectList(db.WarrantyStatus, "warrantyStatusUid", "warrantyStatus", locationIssue.warrantyStatusUid);
            return View(locationIssue);
        }

        // GET: LocationIssues/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocationIssue locationIssue = await db.LocationIssues.FindAsync(id);
            if (locationIssue == null)
            {
                return HttpNotFound();
            }
            ViewBag.aspNetUserUidAsCreated = new SelectList(db.AspNetUsers, "Id", "AllowableContractors", locationIssue.aspNetUserUidAsCreated);
            ViewBag.aspNetUserUid = new SelectList(db.AspNetUsers, "Id", "AllowableContractors", locationIssue.aspNetUserUid);
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1", locationIssue.locationUid);
            ViewBag.warrantyStatusUid = new SelectList(db.WarrantyStatus, "warrantyStatusUid", "warrantyStatus", locationIssue.warrantyStatusUid);
            return View(locationIssue);
        }

        // POST: LocationIssues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "locationIssueUid,aspNetUserUidAsCreated,locationUid,warrantyStatusUid,aspNetUserUid,date,locationIssue1")] LocationIssue locationIssue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(locationIssue).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.aspNetUserUidAsCreated = new SelectList(db.AspNetUsers, "Id", "AllowableContractors", locationIssue.aspNetUserUidAsCreated);
            ViewBag.aspNetUserUid = new SelectList(db.AspNetUsers, "Id", "AllowableContractors", locationIssue.aspNetUserUid);
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1", locationIssue.locationUid);
            ViewBag.warrantyStatusUid = new SelectList(db.WarrantyStatus, "warrantyStatusUid", "warrantyStatus", locationIssue.warrantyStatusUid);
            return View(locationIssue);
        }

        // GET: LocationIssues/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocationIssue locationIssue = await db.LocationIssues.FindAsync(id);
            if (locationIssue == null)
            {
                return HttpNotFound();
            }
            return View(locationIssue);
        }

        // POST: LocationIssues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            LocationIssue locationIssue = await db.LocationIssues.FindAsync(id);
            db.LocationIssues.Remove(locationIssue);
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
