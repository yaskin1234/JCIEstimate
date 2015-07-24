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
    public class EquipmentConditionsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: EquipmentConditions
        public async Task<ActionResult> Index()
        {
            return View(await db.EquipmentConditions.ToListAsync());
        }

        // GET: EquipmentConditions/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentCondition equipmentCondition = await db.EquipmentConditions.FindAsync(id);
            if (equipmentCondition == null)
            {
                return HttpNotFound();
            }
            return View(equipmentCondition);
        }

        // GET: EquipmentConditions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EquipmentConditions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "equipmentConditionUid,equipmentCondition1,behaviorIndicator")] EquipmentCondition equipmentCondition)
        {
            if (ModelState.IsValid)
            {
                equipmentCondition.equipmentConditionUid = Guid.NewGuid();
                db.EquipmentConditions.Add(equipmentCondition);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(equipmentCondition);
        }

        // GET: EquipmentConditions/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentCondition equipmentCondition = await db.EquipmentConditions.FindAsync(id);
            if (equipmentCondition == null)
            {
                return HttpNotFound();
            }
            return View(equipmentCondition);
        }

        // POST: EquipmentConditions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "equipmentConditionUid,equipmentCondition1,behaviorIndicator")] EquipmentCondition equipmentCondition)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipmentCondition).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(equipmentCondition);
        }

        // GET: EquipmentConditions/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentCondition equipmentCondition = await db.EquipmentConditions.FindAsync(id);
            if (equipmentCondition == null)
            {
                return HttpNotFound();
            }
            return View(equipmentCondition);
        }

        // POST: EquipmentConditions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            EquipmentCondition equipmentCondition = await db.EquipmentConditions.FindAsync(id);
            db.EquipmentConditions.Remove(equipmentCondition);
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
