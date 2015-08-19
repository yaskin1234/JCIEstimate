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
    public class EquipmentTypeTaskAssignmentsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: EquipmentTypeTaskAssignments
        public async Task<ActionResult> Index()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            var equipmentTypeTaskAssignments = db.EquipmentTypeTaskAssignments.Where(c=>c.Location.projectUid == sessionProject).Include(e => e.Contractor).Include(e => e.Contractor).Include(e => e.ECM).Include(e => e.EquipmentAttributeTypeTask).Include(e => e.Location);
            return View(await equipmentTypeTaskAssignments.ToListAsync());
        }

        // GET: EquipmentTypeTaskAssignments/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentTypeTaskAssignment equipmentTypeTaskAssignment = await db.EquipmentTypeTaskAssignments.FindAsync(id);
            if (equipmentTypeTaskAssignment == null)
            {
                return HttpNotFound();
            }
            return View(equipmentTypeTaskAssignment);
        }

        // GET: EquipmentTypeTaskAssignments/Create
        public ActionResult Create()
        {
            ViewBag.equipmentAttributeTypeUid = new SelectList(db.Contractors, "contractorUid", "contractorName");
            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName");
            ViewBag.ecmUid = new SelectList(db.ECMs, "ecmUid", "ecmDescription");
            ViewBag.equipmentAttributeTypeTaskUid = new SelectList(db.EquipmentAttributeTypeTasks, "equipmentAttributeTypeTaskUid", "equipmentAttributeTypeTask1");
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1");
            return View();
        }

        // POST: EquipmentTypeTaskAssignments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "equipmentTypeTaskAssignmentUid,ecmUid,locationUid,contractorUid,equipmentAttributeTypeUid,equipmentAttributeTypeTaskUid,amount")] EquipmentTypeTaskAssignment equipmentTypeTaskAssignment)
        {
            if (ModelState.IsValid)
            {
                equipmentTypeTaskAssignment.equipmentTypeTaskAssignmentUid = Guid.NewGuid();
                db.EquipmentTypeTaskAssignments.Add(equipmentTypeTaskAssignment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.equipmentAttributeTypeUid = new SelectList(db.Contractors, "contractorUid", "contractorName", equipmentTypeTaskAssignment.equipmentAttributeTypeUid);
            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName", equipmentTypeTaskAssignment.contractorUid);
            ViewBag.ecmUid = new SelectList(db.ECMs, "ecmUid", "ecmDescription", equipmentTypeTaskAssignment.ecmUid);
            ViewBag.equipmentAttributeTypeTaskUid = new SelectList(db.EquipmentAttributeTypeTasks, "equipmentAttributeTypeTaskUid", "equipmentAttributeTypeTask1", equipmentTypeTaskAssignment.equipmentAttributeTypeTaskUid);
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1", equipmentTypeTaskAssignment.locationUid);
            return View(equipmentTypeTaskAssignment);
        }

        // GET: EquipmentTypeTaskAssignments/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentTypeTaskAssignment equipmentTypeTaskAssignment = await db.EquipmentTypeTaskAssignments.FindAsync(id);
            if (equipmentTypeTaskAssignment == null)
            {
                return HttpNotFound();
            }
            ViewBag.equipmentAttributeTypeUid = new SelectList(db.EquipmentAttributeTypes, "equipmentAttributeTypeUid", "equipmentAttributeType1", equipmentTypeTaskAssignment.equipmentAttributeTypeUid);
            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName", equipmentTypeTaskAssignment.contractorUid);
            ViewBag.ecmUid = new SelectList(db.ECMs, "ecmUid", "ecmDescription", equipmentTypeTaskAssignment.ecmUid);
            ViewBag.equipmentAttributeTypeTaskUid = new SelectList(db.EquipmentAttributeTypeTasks, "equipmentAttributeTypeTaskUid", "equipmentAttributeTypeTask1", equipmentTypeTaskAssignment.equipmentAttributeTypeTaskUid);
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1", equipmentTypeTaskAssignment.locationUid);
            return View(equipmentTypeTaskAssignment);
        }

        // POST: EquipmentTypeTaskAssignments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "equipmentTypeTaskAssignmentUid,ecmUid,locationUid,contractorUid,equipmentAttributeTypeUid,equipmentAttributeTypeTaskUid,amount")] EquipmentTypeTaskAssignment equipmentTypeTaskAssignment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipmentTypeTaskAssignment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.equipmentAttributeTypeUid = new SelectList(db.Contractors, "contractorUid", "contractorName", equipmentTypeTaskAssignment.equipmentAttributeTypeUid);
            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName", equipmentTypeTaskAssignment.contractorUid);
            ViewBag.ecmUid = new SelectList(db.ECMs, "ecmUid", "ecmDescription", equipmentTypeTaskAssignment.ecmUid);
            ViewBag.equipmentAttributeTypeTaskUid = new SelectList(db.EquipmentAttributeTypeTasks, "equipmentAttributeTypeTaskUid", "equipmentAttributeTypeTask1", equipmentTypeTaskAssignment.equipmentAttributeTypeTaskUid);
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1", equipmentTypeTaskAssignment.locationUid);
            return View(equipmentTypeTaskAssignment);
        }

        // GET: EquipmentTypeTaskAssignments/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentTypeTaskAssignment equipmentTypeTaskAssignment = await db.EquipmentTypeTaskAssignments.FindAsync(id);
            if (equipmentTypeTaskAssignment == null)
            {
                return HttpNotFound();
            }
            return View(equipmentTypeTaskAssignment);
        }

        // POST: EquipmentTypeTaskAssignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            EquipmentTypeTaskAssignment equipmentTypeTaskAssignment = await db.EquipmentTypeTaskAssignments.FindAsync(id);
            db.EquipmentTypeTaskAssignments.Remove(equipmentTypeTaskAssignment);
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
