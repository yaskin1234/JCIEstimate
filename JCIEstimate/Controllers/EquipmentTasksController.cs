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
    public class EquipmentTasksController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: EquipmentTasks
        public async Task<ActionResult> Index()
        {
            return View(await db.EquipmentTasks.ToListAsync());
        }

        // GET: EquipmentTasks/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentTask equipmentTask = await db.EquipmentTasks.FindAsync(id);
            if (equipmentTask == null)
            {
                return HttpNotFound();
            }
            return View(equipmentTask);
        }

        // GET: EquipmentTasks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EquipmentTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "equipmentTaskUid,equipmentTask1,equipmentTaskDescription,behaviorIndicator")] EquipmentTask equipmentTask)
        {
            if (ModelState.IsValid)
            {
                equipmentTask.equipmentTaskUid = Guid.NewGuid();
                db.EquipmentTasks.Add(equipmentTask);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(equipmentTask);
        }

        // GET: EquipmentTasks/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentTask equipmentTask = await db.EquipmentTasks.FindAsync(id);
            if (equipmentTask == null)
            {
                return HttpNotFound();
            }
            return View(equipmentTask);
        }

        // POST: EquipmentTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "equipmentTaskUid,equipmentTask1,equipmentTaskDescription,behaviorIndicator")] EquipmentTask equipmentTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipmentTask).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(equipmentTask);
        }

        // GET: EquipmentTasks/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentTask equipmentTask = await db.EquipmentTasks.FindAsync(id);
            if (equipmentTask == null)
            {
                return HttpNotFound();
            }
            return View(equipmentTask);
        }

        // POST: EquipmentTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            EquipmentTask equipmentTask = await db.EquipmentTasks.FindAsync(id);
            db.EquipmentTasks.Remove(equipmentTask);
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
