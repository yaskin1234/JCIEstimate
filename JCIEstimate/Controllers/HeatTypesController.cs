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
    public class HeatTypesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: HeatTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.HeatTypes.ToListAsync());
        }

        // GET: HeatTypes/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HeatType heatType = await db.HeatTypes.FindAsync(id);
            if (heatType == null)
            {
                return HttpNotFound();
            }
            return View(heatType);
        }

        // GET: HeatTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HeatTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "heatTypeUid,heatType1,heatTypeDescription,behaviorIndicator")] HeatType heatType)
        {
            if (ModelState.IsValid)
            {
                heatType.heatTypeUid = Guid.NewGuid();
                db.HeatTypes.Add(heatType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(heatType);
        }

        // GET: HeatTypes/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HeatType heatType = await db.HeatTypes.FindAsync(id);
            if (heatType == null)
            {
                return HttpNotFound();
            }
            return View(heatType);
        }

        // POST: HeatTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "heatTypeUid,heatType1,heatTypeDescription,behaviorIndicator")] HeatType heatType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(heatType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(heatType);
        }

        // GET: HeatTypes/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HeatType heatType = await db.HeatTypes.FindAsync(id);
            if (heatType == null)
            {
                return HttpNotFound();
            }
            return View(heatType);
        }

        // POST: HeatTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            HeatType heatType = await db.HeatTypes.FindAsync(id);
            db.HeatTypes.Remove(heatType);
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
