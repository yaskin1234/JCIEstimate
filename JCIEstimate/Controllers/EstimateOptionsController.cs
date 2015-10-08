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
    public class EstimateOptionsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: EstimateOptions
        public async Task<ActionResult> Index()
        {
            return View(await db.EstimateOptions.ToListAsync());
        }

        // GET: EstimateOptions/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstimateOption estimateOption = await db.EstimateOptions.FindAsync(id);
            if (estimateOption == null)
            {
                return HttpNotFound();
            }
            return View(estimateOption);
        }

        // GET: EstimateOptions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EstimateOptions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "estimateOptionUid,EstimateOption1,behaviorIndicator")] EstimateOption estimateOption)
        {
            if (ModelState.IsValid)
            {
                estimateOption.estimateOptionUid = Guid.NewGuid();
                db.EstimateOptions.Add(estimateOption);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(estimateOption);
        }

        // GET: EstimateOptions/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstimateOption estimateOption = await db.EstimateOptions.FindAsync(id);
            if (estimateOption == null)
            {
                return HttpNotFound();
            }
            return View(estimateOption);
        }

        // POST: EstimateOptions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "estimateOptionUid,EstimateOption1,behaviorIndicator")] EstimateOption estimateOption)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estimateOption).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(estimateOption);
        }

        // GET: EstimateOptions/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstimateOption estimateOption = await db.EstimateOptions.FindAsync(id);
            if (estimateOption == null)
            {
                return HttpNotFound();
            }
            return View(estimateOption);
        }

        // POST: EstimateOptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            EstimateOption estimateOption = await db.EstimateOptions.FindAsync(id);
            db.EstimateOptions.Remove(estimateOption);
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
