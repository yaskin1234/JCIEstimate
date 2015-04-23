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
    public class EquipmentsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: Equipments
        public async Task<ActionResult> Index()
        {
            Guid sessionProject;

            sessionProject = Guid.Empty;

            if (Session["projectUid"] != null)
            {
                sessionProject = new System.Guid(Session["projectUid"].ToString());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            var equipments = db.Equipments.Where(c => c.Location.projectUid == sessionProject).Include(e => e.ECM).Include(e => e.EquipmentAttributeType).Include(e => e.Location);
            ViewBag.equipmentTasks = db.EquipmentTasks;
            ViewBag.equipmentToDoes = db.EquipmentToDoes;
            ViewBag.equipment = equipments;

            return View(await equipments.ToListAsync());
        }

        // GET: GridEdit
        public async Task<ActionResult> GridEdit()
        {
            Guid sessionProject;

            sessionProject = Guid.Empty;

            if (Session["projectUid"] != null)
            {
                sessionProject = new System.Guid(Session["projectUid"].ToString());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            var equipments = db.Equipments.Where(c => c.Location.projectUid == sessionProject).Include(e => e.ECM).Include(e => e.EquipmentAttributeType).Include(e => e.Location);

            ViewBag.equipmentTasks = db.EquipmentTasks;
            ViewBag.equipmentToDoes = db.EquipmentToDoes;
            ViewBag.equipment = equipments;

            return View(await equipments.ToListAsync());
        }

        // GET: Equipments/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipment equipment = await db.Equipments.FindAsync(id);
            if (equipment == null)
            {
                return HttpNotFound();
            }
            return View(equipment);
        }

        // GET: Equipments/Create
        public ActionResult Create()
        {
            Guid sessionProject;
            sessionProject = Guid.Empty;
            if (Session["projectUid"] != null)
            {
                sessionProject = new System.Guid(Session["projectUid"].ToString());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ecmUid = new SelectList(db.ECMs.Where(c => c.projectUid == sessionProject), "ecmUid", "ecmString");
            ViewBag.equipmentAttributeTypeUid = new SelectList(db.EquipmentAttributeTypes, "equipmentAttributeTypeUid", "equipmentAttributeType1");
            ViewBag.locationUid = new SelectList(db.Locations.Where(c => c.projectUid == sessionProject), "locationUid", "location1");
            return View();
        }

        // POST: Equipments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "equipmentUid,equipmentAttributeTypeUid,ecmUid,locationUid,jciTag,ownerTag,manufacturer,model,serialNumber,installDate,area")] Equipment equipment)
        {
            if (ModelState.IsValid)
            {
                equipment.equipmentUid = Guid.NewGuid();
                db.Equipments.Add(equipment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ecmUid = new SelectList(db.ECMs, "ecmUid", "ecmNumber", equipment.ecmUid);
            ViewBag.equipmentAttributeTypeUid = new SelectList(db.EquipmentAttributeTypes, "equipmentAttributeTypeUid", "equipmentAttributeType1", equipment.equipmentAttributeTypeUid);
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1", equipment.locationUid);
            return View(equipment);
        }

        // GET: Equipments/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipment equipment = await db.Equipments.FindAsync(id);
            if (equipment == null)
            {
                return HttpNotFound();
            }

            Guid sessionProject;
            sessionProject = Guid.Empty;
            if (Session["projectUid"] != null)
            {
                sessionProject = new System.Guid(Session["projectUid"].ToString());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ecmUid = new SelectList(db.ECMs.Where(c => c.projectUid == sessionProject), "ecmUid", "ecmNumber", equipment.ecmUid);
            ViewBag.equipmentAttributeTypeUid = new SelectList(db.EquipmentAttributeTypes, "equipmentAttributeTypeUid", "equipmentAttributeType1", equipment.equipmentAttributeTypeUid);
            ViewBag.locationUid = new SelectList(db.Locations.Where(c => c.projectUid == sessionProject), "locationUid", "location1", equipment.locationUid);
            return View(equipment);
        }

        // POST: Equipments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "equipmentUid,equipmentAttributeTypeUid,ecmUid,locationUid,jciTag,ownerTag,manufacturer,model,serialNumber,installDate,area")] Equipment equipment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ecmUid = new SelectList(db.ECMs, "ecmUid", "ecmNumber", equipment.ecmUid);
            ViewBag.equipmentAttributeTypeUid = new SelectList(db.EquipmentAttributeTypes, "equipmentAttributeTypeUid", "equipmentAttributeType1", equipment.equipmentAttributeTypeUid);
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1", equipment.locationUid);
            return View(equipment);
        }

        // GET: Equipments/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipment equipment = await db.Equipments.FindAsync(id);
            if (equipment == null)
            {
                return HttpNotFound();
            }
            return View(equipment);
        }

        // POST: Equipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Equipment equipment = await db.Equipments.FindAsync(id);
            db.Equipments.Remove(equipment);
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
