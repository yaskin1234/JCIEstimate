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
    public class RfiTypesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: RfiTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.RfiTypes.ToListAsync());
        }

        // GET: RfiTypes/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RfiType rfiType = await db.RfiTypes.FindAsync(id);
            if (rfiType == null)
            {
                return HttpNotFound();
            }
            return View(rfiType);
        }

        // GET: RfiTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RfiTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "rfiTypeUid,rfiType1,behaviorIndicator")] RfiType rfiType)
        {
            if (ModelState.IsValid)
            {
                rfiType.rfiTypeUid = Guid.NewGuid();
                db.RfiTypes.Add(rfiType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(rfiType);
        }

        // GET: RfiTypes/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RfiType rfiType = await db.RfiTypes.FindAsync(id);
            if (rfiType == null)
            {
                return HttpNotFound();
            }
            return View(rfiType);
        }

        // POST: RfiTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "rfiTypeUid,rfiType1,behaviorIndicator")] RfiType rfiType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rfiType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(rfiType);
        }

        // GET: RfiTypes/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RfiType rfiType = await db.RfiTypes.FindAsync(id);
            if (rfiType == null)
            {
                return HttpNotFound();
            }
            return View(rfiType);
        }

        // POST: RfiTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            RfiType rfiType = await db.RfiTypes.FindAsync(id);
            db.RfiTypes.Remove(rfiType);
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
