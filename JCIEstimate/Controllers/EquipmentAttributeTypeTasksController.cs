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
using JCIExtensions;

namespace JCIEstimate.Controllers
{
    public class EquipmentAttributeTypeTasksController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: EquipmentAttributeTypeTasks
        public async Task<ActionResult> Index()
        {
            var equipmentAttributeTypeTasks = db.EquipmentAttributeTypeTasks.Include(e => e.EquipmentAttributeType);
            return View(await equipmentAttributeTypeTasks.ToListAsync());
        }

        // GET: EquipmentAttributeTypeTasks/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentAttributeTypeTask equipmentAttributeTypeTask = await db.EquipmentAttributeTypeTasks.FindAsync(id);
            if (equipmentAttributeTypeTask == null)
            {
                return HttpNotFound();
            }
            return View(equipmentAttributeTypeTask);
        }

        // GET: EquipmentAttributeTypeTasks/Create
        public ActionResult Create()
        {
            ViewBag.equipmentAttributeTypeUid = db.EquipmentAttributeTypes.OrderBy(c => c.equipmentAttributeType1).Where(c => c.excludeFromDropDown == false).ToSelectList(c => c.equipmentAttributeType1, c => c.equipmentAttributeTypeUid.ToString(), "");
            return View();
        }

        // POST: EquipmentAttributeTypeTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "equipmentAttributeTypeTaskUid,equipmentAttributeTypeUid,equipmentAttributeTypeTask1,equipmentAttributeTypeTaskDescription")] EquipmentAttributeTypeTask equipmentAttributeTypeTask)
        {
            if (ModelState.IsValid)
            {
                equipmentAttributeTypeTask.equipmentAttributeTypeTaskUid = Guid.NewGuid();
                db.EquipmentAttributeTypeTasks.Add(equipmentAttributeTypeTask);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.equipmentAttributeTypeUid = new SelectList(db.EquipmentAttributeTypes.Where(c => c.excludeFromDropDown == false), "equipmentAttributeTypeUid", "equipmentAttributeType1", equipmentAttributeTypeTask.equipmentAttributeTypeUid);
            return View(equipmentAttributeTypeTask);
        }

        // GET: EquipmentAttributeTypeTasks/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentAttributeTypeTask equipmentAttributeTypeTask = await db.EquipmentAttributeTypeTasks.FindAsync(id);
            if (equipmentAttributeTypeTask == null)
            {
                return HttpNotFound();
            }
            ViewBag.equipmentAttributeTypeUid = db.EquipmentAttributeTypes.Where(c => c.excludeFromDropDown == false).OrderBy(c => c.equipmentAttributeType1).ToSelectList(c => c.equipmentAttributeType1, c => c.equipmentAttributeTypeUid.ToString(), equipmentAttributeTypeTask.equipmentAttributeTypeUid.ToString());            
            return View(equipmentAttributeTypeTask);
        }

        // POST: EquipmentAttributeTypeTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "equipmentAttributeTypeTaskUid,equipmentAttributeTypeUid,equipmentAttributeTypeTask1,equipmentAttributeTypeTaskDescription")] EquipmentAttributeTypeTask equipmentAttributeTypeTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipmentAttributeTypeTask).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.equipmentAttributeTypeUid = new SelectList(db.EquipmentAttributeTypes.Where(c => c.excludeFromDropDown == false), "equipmentAttributeTypeUid", "equipmentAttributeType1", equipmentAttributeTypeTask.equipmentAttributeTypeUid);
            return View(equipmentAttributeTypeTask);
        }

        // GET: EquipmentAttributeTypeTasks/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentAttributeTypeTask equipmentAttributeTypeTask = await db.EquipmentAttributeTypeTasks.FindAsync(id);
            if (equipmentAttributeTypeTask == null)
            {
                return HttpNotFound();
            }
            return View(equipmentAttributeTypeTask);
        }

        // POST: EquipmentAttributeTypeTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            EquipmentAttributeTypeTask equipmentAttributeTypeTask = await db.EquipmentAttributeTypeTasks.FindAsync(id);
            db.EquipmentAttributeTypeTasks.Remove(equipmentAttributeTypeTask);
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
