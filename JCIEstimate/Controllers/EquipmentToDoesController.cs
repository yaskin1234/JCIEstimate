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
    public class EquipmentToDoesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: EquipmentToDoes
        public async Task<ActionResult> Index()
        {
            IQueryable<EquipmentToDo> equipmentToDos;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            equipmentToDos = from cc in db.EquipmentToDoes
                         where cc.Equipment.Location.projectUid == sessionProject
                         select cc;

            equipmentToDos = equipmentToDos.Include(e => e.Equipment).Include(e => e.EquipmentTask);
            return View(await equipmentToDos.ToListAsync());
        }

        // GET: EquipmentToDoes/SaveCheckedBox/5
        public async Task<ActionResult> SaveCheckedBox(string chkBoxName, string value)
        {
            string[] incomingValues;
            incomingValues = chkBoxName.Split('_');
            if (value == "true")
            {
                EquipmentToDo newEQ = new EquipmentToDo();
                newEQ.equipmentUid = new Guid(incomingValues[0]);
                newEQ.equipmentTaskUid = new Guid(incomingValues[1]);
                newEQ.equipmentToDoUid = Guid.NewGuid();
                try
                {
                    db.EquipmentToDoes.Add(newEQ);
                    await db.SaveChangesAsync();                             
                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }                
            }
            else
            {
                Guid equipUid = new Guid(incomingValues[0].ToString());
                Guid taskUid = new Guid(incomingValues[1].ToString());
                var id = from cc in db.EquipmentToDoes
                         where cc.equipmentUid == equipUid
                         && cc.equipmentTaskUid == taskUid
                         select cc.equipmentToDoUid;                
                try
                {
                    EquipmentToDo equipmentToDo = await db.EquipmentToDoes.FindAsync(id.First());
                    db.EquipmentToDoes.Remove(equipmentToDo);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {                    
                    throw ex;
                }
                
            }
            return View();
        }

        // GET: EquipmentToDoes/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentToDo equipmentToDo = await db.EquipmentToDoes.FindAsync(id);
            if (equipmentToDo == null)
            {
                return HttpNotFound();
            }
            return View(equipmentToDo);
        }

        // GET: EquipmentToDoes/Create
        public ActionResult Create()
        {
            IQueryable<Equipment> equipments;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            equipments = from cc in db.Equipments
                         where cc.Location.projectUid == sessionProject
                         select cc;
            ViewBag.equipmentUid = new SelectList(equipments, "equipmentUid", "equipment1");
            ViewBag.equipmentTaskUid = new SelectList(db.EquipmentTasks, "equipmentTaskUid", "equipmentTask1");
            return View();
        }

        // POST: EquipmentToDoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "equipmentToDoUid,equipmentUid,daysToComplete")] EquipmentToDo equipmentToDo, params string[] selectedTasks)
        {
            if (ModelState.IsValid)
            {
                equipmentToDo.equipmentToDoUid = Guid.NewGuid();
                db.EquipmentToDoes.Add(equipmentToDo);
                await db.SaveChangesAsync();                                    
                return RedirectToAction("Index");
            }

            ViewBag.equipmentUid = new SelectList(db.Equipments, "equipmentUid", "equipment1", equipmentToDo.equipmentUid);
            ViewBag.equipmentTaskUid = new SelectList(db.EquipmentTasks, "equipmentTaskUid", "equipmentTask1", equipmentToDo.equipmentTaskUid);
            return View(equipmentToDo);
        }

        public ActionResult GridEdit()
        {
            
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            var equipment = from cc in db.Equipments
                            where cc.Location.projectUid == sessionProject
                            select cc;

            var equipmentToDoes = from cc in db.EquipmentToDoes
                                  where cc.Equipment.Location.projectUid == sessionProject
                                  select cc;

            var selectedTasks = from cc in db.EquipmentToDoes                                
                                select cc.equipmentTaskUid.ToString();

            var equipmentTasks = db.EquipmentTasks.ToList().Select(x => new SelectListItem()
            {
                Selected = selectedTasks.Contains(x.equipmentTaskUid.ToString()),
                Text = x.equipmentTask1,
                Value = x.equipmentTaskUid.ToString()
            });

            //ViewBag.equipment = equipment.ToSelectList(d => d.Location.location1 + " " + d.equipment1, d => d.equipmentUid.ToString(), "");
            ViewBag.equipmentToDoes = equipmentToDoes;
            ViewBag.equipment = equipment.OrderBy(c=>c.Location.location1).ThenBy(c => c.equipment1);
            ViewBag.equipmentTasks = new SelectList(equipmentTasks, "Value", "Text", selectedTasks.ToList());
            return View();
        }

        // GET: EquipmentToDoes/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentToDo equipmentToDo = await db.EquipmentToDoes.FindAsync(id);
            if (equipmentToDo == null)
            {
                return HttpNotFound();
            }
            IQueryable<Equipment> equipments;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            equipments = from cc in db.Equipments
                         where cc.Location.projectUid == sessionProject
                         select cc;

            ViewBag.equipmentUid = new SelectList(equipments, "equipmentUid", "equipment1", equipmentToDo.equipmentUid);
            ViewBag.equipmentTaskUid = new SelectList(db.EquipmentTasks, "equipmentTaskUid", "equipmentTask1", equipmentToDo.equipmentTaskUid);
            return View(equipmentToDo);
        }

        // POST: EquipmentToDoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "equipmentToDoUid,equipmentUid,equipmentTaskUid,daysToComplete")] EquipmentToDo equipmentToDo, params string[] selectedTasks)
        {
           

            IQueryable<Equipment> equipments;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            equipments = from cc in db.Equipments
                         where cc.Location.projectUid == sessionProject
                         select cc;
            ViewBag.equipmentUid = new SelectList(equipments, "equipmentUid", "equipment1", equipmentToDo.equipmentUid);
            ViewBag.equipmentTaskUid = new SelectList(db.EquipmentTasks, "equipmentTaskUid", "equipmentTask1", equipmentToDo.equipmentTaskUid);
            return View(equipmentToDo);
        }

        // GET: EquipmentToDoes/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentToDo equipmentToDo = await db.EquipmentToDoes.FindAsync(id);
            if (equipmentToDo == null)
            {
                return HttpNotFound();
            }
            return View(equipmentToDo);
        }

        // POST: EquipmentToDoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            EquipmentToDo equipmentToDo = await db.EquipmentToDoes.FindAsync(id);
            db.EquipmentToDoes.Remove(equipmentToDo);
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
