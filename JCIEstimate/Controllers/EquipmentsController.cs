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
    public class EquipmentsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: Equipments
        public async Task<ActionResult> Index()
        {
            IQueryable<Equipment> equipments;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            equipments = from cc in db.Equipments                         
                         where cc.ECM.projectUid == sessionProject
                         select cc;

            equipments = equipments.Include(e => e.ECM).Include(e => e.EquipmentType).Include(e => e.Location);
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
            IQueryable<ECM> ecms;
            IQueryable<Location> locations;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            ecms = from cc in db.ECMs
                    where cc.projectUid == sessionProject
                    select cc;

            locations = from cc in db.Locations
                   where cc.projectUid == sessionProject
                   select cc;

            ViewBag.ecmUid = new SelectList(ecms, "ecmUid", "ecmNumber");
            ViewBag.equipmentTypeUid = new SelectList(db.EquipmentTypes, "equipmentTypeUid", "equipmentType1");
            ViewBag.equipmentTaskUid = new SelectList(db.EquipmentTasks, "equipmentTaskUid", "equipmentTask1");
            ViewBag.locationUid = new SelectList(locations, "locationUid", "location1");
            return View();
        }

        // POST: Equipments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "equipmentUid,ecmUid,locationUid,room,equipmentTypeUid,equipment1,jciCode,barCode")] Equipment equipment, params string[] selectedTasks)
        {

            EquipmentToDo myEQ;

            if (ModelState.IsValid)
            {
                //Add equipment
                equipment.equipmentUid = Guid.NewGuid();
                db.Equipments.Add(equipment);
                await db.SaveChangesAsync();

                if (selectedTasks != null)
                {
                    //add each selected task to EquipmentToDo
                    foreach (var item in selectedTasks)
                    {                        
                        myEQ = new EquipmentToDo();
                        myEQ.equipmentToDoUid = Guid.NewGuid();
                        myEQ.equipmentTaskUid = new Guid(item);
                        myEQ.equipmentUid = equipment.equipmentUid;
                        db.EquipmentToDoes.Add(myEQ);
                        await db.SaveChangesAsync();                        
                    }                    
                }
                return RedirectToAction("Index");
            }
            
            ViewBag.ecmUid = new SelectList(db.ECMs, "ecmUid", "ecmNumber", equipment.ecmUid);
            ViewBag.equipmentTypeUid = new SelectList(db.EquipmentTypes, "equipmentTypeUid", "equipmentType1", equipment.equipmentTypeUid);
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

            IQueryable<ECM> ecms;
            IQueryable<Location> locations;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            var selectedTasks = from cc in db.EquipmentToDoes
                                where cc.equipmentUid == id
                                select cc.equipmentTaskUid.ToString();

            ecms = from cc in db.ECMs
                   where cc.projectUid == sessionProject
                   select cc;

            locations = from cc in db.Locations
                        where cc.projectUid == sessionProject
                        select cc;

            var equipmentList = db.EquipmentTasks.ToList().Select(x => new SelectListItem()
            {
                Selected = selectedTasks.Contains(x.equipmentTaskUid.ToString()),
                Text = x.equipmentTask1,
                Value = x.equipmentTaskUid.ToString()
            });

            ViewBag.ecmUid = new SelectList(ecms, "ecmUid", "ecmNumber", equipment.ecmUid);
            ViewBag.equipmentTypeUid = new SelectList(db.EquipmentTypes, "equipmentTypeUid", "equipmentType1", equipment.equipmentTypeUid);
            ViewBag.locationUid = new SelectList(locations, "locationUid", "location1", equipment.locationUid);
            ViewBag.equipmentList = new SelectList(equipmentList, "Value", "Text", selectedTasks.ToList());            
            return View(equipment);
        }

        // POST: Equipments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "equipmentUid,ecmUid,locationUid,room,equipmentTypeUid,equipment1,jciCode,barCode")] Equipment equipment, params string[] selectedTasks)
        {
            EquipmentToDo myEQ;

            if (ModelState.IsValid)
            {
                if (selectedTasks != null)
                {

                    IQueryable<EquipmentToDo> equipmentToDosToDelete;
                    IQueryable<EquipmentTask> equipmentToDosToInsert;

                    equipmentToDosToDelete = from cc in db.EquipmentToDoes
                                             where !(selectedTasks.Any(v => cc.equipmentTaskUid.ToString().Contains(v)))
                                             && cc.equipmentUid == equipment.equipmentUid
                                             select cc;

                    equipmentToDosToInsert = from cc in db.EquipmentTasks
                                             where selectedTasks.Any(v => cc.equipmentTaskUid.ToString().Contains(v))                                     
                                             select cc;                    

                    //Delete
                    foreach (var item in equipmentToDosToDelete.ToList())
                    {
                        myEQ = db.EquipmentToDoes.Find(item.equipmentToDoUid);
                        db.EquipmentToDoes.Remove(myEQ);
                        await db.SaveChangesAsync();
                    }

                    //Insert
                    foreach (var item in equipmentToDosToInsert.ToList())
                    {                           
                        myEQ = new EquipmentToDo();                        
                        myEQ.equipmentTaskUid = item.equipmentTaskUid;
                        myEQ.equipmentUid = equipment.equipmentUid;
                        JCIExtensions.MCVExtensions.InsertOrUpdate(db, myEQ);                                                
                    }                    
                }
            }    

            if (ModelState.IsValid)
            {
                db.Entry(equipment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ecmUid = new SelectList(db.ECMs, "ecmUid", "ecmNumber", equipment.ecmUid);
            ViewBag.equipmentTypeUid = new SelectList(db.EquipmentTypes, "equipmentTypeUid", "equipmentType1", equipment.equipmentTypeUid);
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
