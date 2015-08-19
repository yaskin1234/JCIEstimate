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
    public class PECExpenseTypesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: PECExpenseTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.PECExpenseTypes.ToListAsync());
        }

        // GET: PECExpenseTypes/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PECExpenseType pECExpenseType = await db.PECExpenseTypes.FindAsync(id);
            if (pECExpenseType == null)
            {
                return HttpNotFound();
            }
            return View(pECExpenseType);
        }

        // GET: PECExpenseTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PECExpenseTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "pecExpenseTypeUid,pecExpenseType1,pecExpenseTypeDescription,behaviorIndicator")] PECExpenseType pECExpenseType)
        {
            if (ModelState.IsValid)
            {
                pECExpenseType.pecExpenseTypeUid = Guid.NewGuid();
                db.PECExpenseTypes.Add(pECExpenseType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(pECExpenseType);
        }

        // GET: PECExpenseTypes/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PECExpenseType pECExpenseType = await db.PECExpenseTypes.FindAsync(id);
            if (pECExpenseType == null)
            {
                return HttpNotFound();
            }
            return View(pECExpenseType);
        }

        // POST: PECExpenseTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "pecExpenseTypeUid,pecExpenseType1,pecExpenseTypeDescription,behaviorIndicator")] PECExpenseType pECExpenseType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pECExpenseType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(pECExpenseType);
        }

        // GET: PECExpenseTypes/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PECExpenseType pECExpenseType = await db.PECExpenseTypes.FindAsync(id);
            if (pECExpenseType == null)
            {
                return HttpNotFound();
            }
            return View(pECExpenseType);
        }

        // POST: PECExpenseTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            PECExpenseType pECExpenseType = await db.PECExpenseTypes.FindAsync(id);
            db.PECExpenseTypes.Remove(pECExpenseType);
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
