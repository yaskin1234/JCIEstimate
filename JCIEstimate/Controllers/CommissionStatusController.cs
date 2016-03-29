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
    public class CommissionStatusController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: CommissionStatus
        public async Task<ActionResult> Index()
        {
            return View(await db.CommissionStatus.ToListAsync());
        }

        // GET: CommissionStatus/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommissionStatu commissionStatu = await db.CommissionStatus.FindAsync(id);
            if (commissionStatu == null)
            {
                return HttpNotFound();
            }
            return View(commissionStatu);
        }

        // GET: CommissionStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CommissionStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "commissionStatusUid,commissionStatus,commissionStatusDescription,behaviorIndicator,listOrder")] CommissionStatu commissionStatu)
        {
            if (ModelState.IsValid)
            {
                commissionStatu.commissionStatusUid = Guid.NewGuid();
                db.CommissionStatus.Add(commissionStatu);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(commissionStatu);
        }

        // GET: CommissionStatus/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommissionStatu commissionStatu = await db.CommissionStatus.FindAsync(id);
            if (commissionStatu == null)
            {
                return HttpNotFound();
            }
            return View(commissionStatu);
        }

        // POST: CommissionStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "commissionStatusUid,commissionStatus,commissionStatusDescription,behaviorIndicator,listOrder")] CommissionStatu commissionStatu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(commissionStatu).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(commissionStatu);
        }

        // GET: CommissionStatus/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommissionStatu commissionStatu = await db.CommissionStatus.FindAsync(id);
            if (commissionStatu == null)
            {
                return HttpNotFound();
            }
            return View(commissionStatu);
        }

        // POST: CommissionStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            CommissionStatu commissionStatu = await db.CommissionStatus.FindAsync(id);
            db.CommissionStatus.Remove(commissionStatu);
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
