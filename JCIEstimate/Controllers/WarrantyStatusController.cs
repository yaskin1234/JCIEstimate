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
    public class WarrantyStatusController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: WarrantyStatus
        public async Task<ActionResult> Index()
        {
            return View(await db.WarrantyStatus.ToListAsync());
        }

        // GET: WarrantyStatus/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WarrantyStatu warrantyStatu = await db.WarrantyStatus.FindAsync(id);
            if (warrantyStatu == null)
            {
                return HttpNotFound();
            }
            return View(warrantyStatu);
        }

        // GET: WarrantyStatus/Create
        public ActionResult Create()
        {          
            return View();
        }

        // POST: WarrantyStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "warrantyStatusUid,warrantyStatus,warrantyStatusDescription,behaviorIndicator,listOrder")] WarrantyStatu warrantyStatu)
        {
            if (ModelState.IsValid)
            {
                warrantyStatu.warrantyStatusUid = Guid.NewGuid();
                db.WarrantyStatus.Add(warrantyStatu);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(warrantyStatu);
        }

        // GET: WarrantyStatus/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WarrantyStatu warrantyStatu = await db.WarrantyStatus.FindAsync(id);
            if (warrantyStatu == null)
            {
                return HttpNotFound();
            }
            return View(warrantyStatu);
        }

        // POST: WarrantyStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "warrantyStatusUid,warrantyStatus,warrantyStatusDescription,behaviorIndicator,listOrder")] WarrantyStatu warrantyStatu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(warrantyStatu).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(warrantyStatu);
        }

        // GET: WarrantyStatus/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WarrantyStatu warrantyStatu = await db.WarrantyStatus.FindAsync(id);
            if (warrantyStatu == null)
            {
                return HttpNotFound();
            }
            return View(warrantyStatu);
        }

        // POST: WarrantyStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            WarrantyStatu warrantyStatu = await db.WarrantyStatus.FindAsync(id);
            db.WarrantyStatus.Remove(warrantyStatu);
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
