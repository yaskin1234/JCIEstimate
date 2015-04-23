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
    public class EquipmentAttributesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: EquipmentAttributes
        public async Task<ActionResult> Index()
        {
            var equipmentAttributes = db.EquipmentAttributes.Include(e => e.AppDataType).Include(e => e.EquipmentAttributeType);
            return View(await equipmentAttributes.ToListAsync());
        }

        // GET: EquipmentAttributes/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentAttribute equipmentAttribute = await db.EquipmentAttributes.FindAsync(id);
            if (equipmentAttribute == null)
            {
                return HttpNotFound();
            }
            return View(equipmentAttribute);
        }

        // GET: EquipmentAttributes/Create
        public ActionResult Create()
        {
            ViewBag.appDataTypeTypeUid = new SelectList(db.AppDataTypes, "appDataTypeUid", "appDataType1");
            ViewBag.equipmentAttributeTypeUid = new SelectList(db.EquipmentAttributeTypes, "equipmentAttributeTypeUid", "equipmentAttributeType1");
            return View();
        }

        // POST: EquipmentAttributes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "equipmentAttributeUid,equipmentAttributeTypeUid,appDataTypeTypeUid,equipmentAttribute1")] EquipmentAttribute equipmentAttribute)
        {
            if (ModelState.IsValid)
            {
                equipmentAttribute.equipmentAttributeUid = Guid.NewGuid();
                db.EquipmentAttributes.Add(equipmentAttribute);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.appDataTypeTypeUid = new SelectList(db.AppDataTypes, "appDataTypeUid", "appDataType1", equipmentAttribute.appDataTypeTypeUid);
            ViewBag.equipmentAttributeTypeUid = new SelectList(db.EquipmentAttributeTypes, "equipmentAttributeTypeUid", "equipmentAttributeType1", equipmentAttribute.equipmentAttributeTypeUid);
            return View(equipmentAttribute);
        }

        // GET: EquipmentAttributes/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentAttribute equipmentAttribute = await db.EquipmentAttributes.FindAsync(id);
            if (equipmentAttribute == null)
            {
                return HttpNotFound();
            }
            ViewBag.appDataTypeTypeUid = new SelectList(db.AppDataTypes, "appDataTypeUid", "appDataType1", equipmentAttribute.appDataTypeTypeUid);
            ViewBag.equipmentAttributeTypeUid = new SelectList(db.EquipmentAttributeTypes, "equipmentAttributeTypeUid", "equipmentAttributeType1", equipmentAttribute.equipmentAttributeTypeUid);
            return View(equipmentAttribute);
        }

        // POST: EquipmentAttributes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "equipmentAttributeUid,equipmentAttributeTypeUid,appDataTypeTypeUid,equipmentAttribute1")] EquipmentAttribute equipmentAttribute)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipmentAttribute).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.appDataTypeTypeUid = new SelectList(db.AppDataTypes, "appDataTypeUid", "appDataType1", equipmentAttribute.appDataTypeTypeUid);
            ViewBag.equipmentAttributeTypeUid = new SelectList(db.EquipmentAttributeTypes, "equipmentAttributeTypeUid", "equipmentAttributeType1", equipmentAttribute.equipmentAttributeTypeUid);
            return View(equipmentAttribute);
        }

        // GET: EquipmentAttributes/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentAttribute equipmentAttribute = await db.EquipmentAttributes.FindAsync(id);
            if (equipmentAttribute == null)
            {
                return HttpNotFound();
            }
            return View(equipmentAttribute);
        }

        // POST: EquipmentAttributes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            EquipmentAttribute equipmentAttribute = await db.EquipmentAttributes.FindAsync(id);
            db.EquipmentAttributes.Remove(equipmentAttribute);
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
