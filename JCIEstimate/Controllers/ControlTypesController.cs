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
    public class ControlTypesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ControlTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.ControlTypes.ToListAsync());
        }

        // GET: ControlTypes/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ControlType controlType = await db.ControlTypes.FindAsync(id);
            if (controlType == null)
            {
                return HttpNotFound();
            }
            return View(controlType);
        }

        // GET: ControlTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ControlTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "controlTypeUid,controlType1,controlTypeDescription,behaviorIndicator")] ControlType controlType)
        {
            if (ModelState.IsValid)
            {
                controlType.controlTypeUid = Guid.NewGuid();
                db.ControlTypes.Add(controlType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(controlType);
        }

        // GET: ControlTypes/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ControlType controlType = await db.ControlTypes.FindAsync(id);
            if (controlType == null)
            {
                return HttpNotFound();
            }
            return View(controlType);
        }

        // POST: ControlTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "controlTypeUid,controlType1,controlTypeDescription,behaviorIndicator")] ControlType controlType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(controlType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(controlType);
        }

        // GET: ControlTypes/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ControlType controlType = await db.ControlTypes.FindAsync(id);
            if (controlType == null)
            {
                return HttpNotFound();
            }
            return View(controlType);
        }

        // POST: ControlTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ControlType controlType = await db.ControlTypes.FindAsync(id);
            db.ControlTypes.Remove(controlType);
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
