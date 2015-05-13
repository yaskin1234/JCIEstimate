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
    public class EquipmentAttributeValuesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: EquipmentAttributeValues
        public async Task<ActionResult> Index()
        {
            var equipmentAttributeValues = db.EquipmentAttributeValues.Include(e => e.Equipment).Include(e => e.EquipmentAttribute);
            return View(await equipmentAttributeValues.ToListAsync());
        }

        // GET: EquipmentAttributeValues/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentAttributeValue equipmentAttributeValue = await db.EquipmentAttributeValues.FindAsync(id);
            if (equipmentAttributeValue == null)
            {
                return HttpNotFound();
            }
            return View(equipmentAttributeValue);
        }


        public async Task<ActionResult> SaveValue(string identifiers, string value)
        {
            string[] ids;                
            ids = identifiers.Split('|');
            Guid equipmentAttributeUid = new Guid(ids[0]);
            Guid equipmentUid = new Guid(ids[1]);
            if (db.EquipmentAttributeValues.Where(c => c.equipmentAttributeUid == equipmentAttributeUid && c.equipmentUid == equipmentUid).Count() == 0)
            { // create new
                EquipmentAttributeValue eq = new EquipmentAttributeValue();
                eq.equipmentAttributeValueUid = Guid.NewGuid();
                eq.equipmentUid = equipmentUid;
                eq.equipmentAttributeUid = equipmentAttributeUid;
                eq.equipmentAttributeValue1 = value;
                db.EquipmentAttributeValues.Add(eq);
                db.SaveChanges();
            }
            else // epdate existing
            {
                EquipmentAttributeValue eq = new EquipmentAttributeValue();
                eq = db.EquipmentAttributeValues.Where(c => c.equipmentAttributeUid == equipmentAttributeUid && c.equipmentUid == equipmentUid).First();
                eq.equipmentAttributeValue1 = value;
                db.SaveChanges();
            }
            return PartialView();
        }


        // GET: EquipmentAttributeValues/Create
        public ActionResult Create()
        {
            ViewBag.equipmentUid = new SelectList(db.Equipments, "equipmentUid", "jciTag");
            ViewBag.equipmentAttributeUid = new SelectList(db.EquipmentAttributes, "equipmentAttributeUid", "equipmentAttribute1");
            return View();
        }

        // POST: EquipmentAttributeValues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "equipmentAttributeValueUid,equipmentAttributeUid,equipmentUid,equipmentAttributeValue1")] EquipmentAttributeValue equipmentAttributeValue)
        {
            if (ModelState.IsValid)
            {
                equipmentAttributeValue.equipmentAttributeValueUid = Guid.NewGuid();
                db.EquipmentAttributeValues.Add(equipmentAttributeValue);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.equipmentUid = new SelectList(db.Equipments, "equipmentUid", "jciTag", equipmentAttributeValue.equipmentUid);
            ViewBag.equipmentAttributeUid = new SelectList(db.EquipmentAttributes, "equipmentAttributeUid", "equipmentAttribute1", equipmentAttributeValue.equipmentAttributeUid);
            return View(equipmentAttributeValue);
        }

        // GET: EquipmentAttributeValues/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentAttributeValue equipmentAttributeValue = await db.EquipmentAttributeValues.FindAsync(id);
            if (equipmentAttributeValue == null)
            {
                return HttpNotFound();
            }
            ViewBag.equipmentUid = new SelectList(db.Equipments, "equipmentUid", "jciTag", equipmentAttributeValue.equipmentUid);
            ViewBag.equipmentAttributeUid = new SelectList(db.EquipmentAttributes, "equipmentAttributeUid", "equipmentAttribute1", equipmentAttributeValue.equipmentAttributeUid);
            return View(equipmentAttributeValue);
        }

        // POST: EquipmentAttributeValues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "equipmentAttributeValueUid,equipmentAttributeUid,equipmentUid,equipmentAttributeValue1")] EquipmentAttributeValue equipmentAttributeValue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipmentAttributeValue).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.equipmentUid = new SelectList(db.Equipments, "equipmentUid", "jciTag", equipmentAttributeValue.equipmentUid);
            ViewBag.equipmentAttributeUid = new SelectList(db.EquipmentAttributes, "equipmentAttributeUid", "equipmentAttribute1", equipmentAttributeValue.equipmentAttributeUid);
            return View(equipmentAttributeValue);
        }

        // GET: EquipmentAttributeValues/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentAttributeValue equipmentAttributeValue = await db.EquipmentAttributeValues.FindAsync(id);
            if (equipmentAttributeValue == null)
            {
                return HttpNotFound();
            }
            return View(equipmentAttributeValue);
        }

        // POST: EquipmentAttributeValues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            EquipmentAttributeValue equipmentAttributeValue = await db.EquipmentAttributeValues.FindAsync(id);
            db.EquipmentAttributeValues.Remove(equipmentAttributeValue);
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
