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
    public class EquipmentAttributeTypesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: EquipmentAttributeTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.EquipmentAttributeTypes.OrderBy(c=>c.equipmentAttributeType1).ToListAsync());
        }

        // GET: EquipmentAttributeTypes/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentAttributeType equipmentAttributeType = await db.EquipmentAttributeTypes.FindAsync(id);
            if (equipmentAttributeType == null)
            {
                return HttpNotFound();
            }
            return View(equipmentAttributeType);
        }       

        // GET: EquipmentAttributeTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EquipmentAttributeTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "equipmentAttributeTypeUid,equipmentAttributeType1,behaviorIndicator,excludeFromDropDown")] EquipmentAttributeType equipmentAttributeType)
        {
            if (ModelState.IsValid)
            {
                equipmentAttributeType.equipmentAttributeTypeUid = Guid.NewGuid();
                db.EquipmentAttributeTypes.Add(equipmentAttributeType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(equipmentAttributeType);
        }

        // GET: EquipmentAttributeTypes/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentAttributeType equipmentAttributeType = await db.EquipmentAttributeTypes.FindAsync(id);
            if (equipmentAttributeType == null)
            {
                return HttpNotFound();
            }
            return View(equipmentAttributeType);
        }

        // POST: EquipmentAttributeTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "equipmentAttributeTypeUid,equipmentAttributeType1,behaviorIndicator,excludeFromDropDown")] EquipmentAttributeType equipmentAttributeType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipmentAttributeType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(equipmentAttributeType);
        }

        // GET: EquipmentAttributeTypes/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentAttributeType equipmentAttributeType = await db.EquipmentAttributeTypes.FindAsync(id);
            if (equipmentAttributeType == null)
            {
                return HttpNotFound();
            }
            return View(equipmentAttributeType);
        }

        // POST: EquipmentAttributeTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            EquipmentAttributeType equipmentAttributeType = await db.EquipmentAttributeTypes.FindAsync(id);
            db.EquipmentAttributeTypes.Remove(equipmentAttributeType);
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
