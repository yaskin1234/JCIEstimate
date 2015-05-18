﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JCIExtensions;
using JCIEstimate.Models;

namespace JCIEstimate.Controllers
{
    public class EquipmentsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: Equipments
        public async Task<ActionResult> Index(string filterId)
        {
            Guid sessionProject;
            IQueryable<Equipment> equipments;

            sessionProject = Guid.Empty;

            if (Session["projectUid"] != null)
            {
                sessionProject = new System.Guid(Session["projectUid"].ToString());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            //Get full equipment list
            equipments = from cc in db.Equipments
                         where !db.Equipments.Any(c => c.equipmentUidAsReplaced == cc.equipmentUid)
                         && cc.Location.projectUid == sessionProject
                         select cc;



            //Build Drop down filter based on existing defined equipment
            List<FilterOptionModel> aryFo = new List<FilterOptionModel>();
            string[] filterPart = null;
            string type = "";
            string uid = Guid.Empty.ToString();

            if (!String.IsNullOrEmpty(filterId))
            {
                filterPart = filterId.Split('|');
                type = filterPart[0];
                uid = filterPart[1];
            }

            FilterOptionModel wf = new FilterOptionModel();
            wf.text = "All";
            wf.value = "A|" + Guid.Empty.ToString();
            wf.selected = (wf.value == filterId || String.IsNullOrEmpty(filterId));
            aryFo.Add(wf);


            IQueryable<Equipment> results = equipments.GroupBy(c=>c.equipmentAttributeTypeUid).Select(v => v.FirstOrDefault());

            foreach (var item in results)
            {
                wf = new FilterOptionModel();                                
                wf.text = item.EquipmentAttributeType.equipmentAttributeType1;
                wf.value = "E|" + item.equipmentAttributeTypeUid.ToString();
                wf.selected = (wf.value == filterId);
                aryFo.Add(wf);                
            }

            results = equipments.GroupBy(c => c.ecmUid).Select(v => v.FirstOrDefault());

            foreach (var item in results)
            {
                wf = new FilterOptionModel();
                wf.text = item.ECM.ecmString;
                wf.value = "C|" + item.ecmUid.ToString();
                wf.selected = (wf.value == filterId);
                aryFo.Add(wf);
            }

            //apply filter if there is one
            if (!String.IsNullOrEmpty(filterId))
            {
                if (type == "E")
                {
                    equipments = equipments.Where(c => c.equipmentAttributeTypeUid.ToString() == uid);
                }
                else if (type == "C")
                {
                    equipments = equipments.Where(c => c.ecmUid.ToString() == uid);
                }
            }
                        
            equipments = equipments.Include(e => e.ECM).Include(e => e.EquipmentAttributeType).Include(e => e.Location).Include(c=>c.Equipment2).OrderBy(c=>c.jciTag);                            
            ViewBag.equipmentTasks = db.EquipmentTasks;
            ViewBag.equipmentToDoes = db.EquipmentToDoes;
            ViewBag.equipmentAttributes = db.EquipmentAttributes;
            ViewBag.equipment = equipments;
            ViewBag.filterList = aryFo.ToList();
            return View(await equipments.ToListAsync());
        }

        public async Task<ActionResult> IndexPartial(string ecmUid)
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

            var equipments = db.Equipments.Where(c => c.Location.projectUid == sessionProject);

            if (!String.IsNullOrEmpty(ecmUid))
            {
                equipments = equipments.Where(c => c.ecmUid.ToString() == ecmUid);
            }
            
            equipments = equipments.Include(e => e.ECM).Include(e => e.EquipmentAttributeType).Include(e => e.Location).OrderBy(c => c.jciTag);

            ViewBag.equipmentTasks = db.EquipmentTasks;
            ViewBag.equipmentToDoes = db.EquipmentToDoes;
            ViewBag.equipmentAttributes = db.EquipmentAttributes;
            ViewBag.equipment = equipments;

            return PartialView(await equipments.ToListAsync());
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

            var equipments = db.Equipments.Where(c => c.Location.projectUid == sessionProject).Include(e => e.ECM).Include(e => e.EquipmentAttributeType).Include(e => e.Location).OrderBy(c => c.ECM.ecmNumber);

            ViewBag.equipmentTasks = db.EquipmentTasks;
            ViewBag.equipmentToDoes = db.EquipmentToDoes;
            ViewBag.equipment = equipments;

            return View(await equipments.ToListAsync());
        }

        public async Task<ActionResult> GridEditPartial(string filter)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();            

            //apply session project predicate
            var equipments = from cc in db.Equipments
                             where cc.Location.projectUid == sessionProject
                             select cc;


            if (filter != null && filter.Length > 0 && filter != "search")
            {
                equipments = from cc in equipments
                             where (cc.Location.location1 + " - " + cc.EquipmentAttributeType.equipmentAttributeType1 + " - " + cc.installDate.Value.Year + " - " + cc.ECM.ecmNumber + " - " + cc.jciTag).Contains(filter)
                             select cc;

            }

            equipments = equipments.Include(w => w.Location).OrderBy(w => w.Location.location1).ThenBy(w => w.ECM.ecmNumber);

            ViewBag.equipmentTasks = db.EquipmentTasks;
            ViewBag.equipmentToDoes = db.EquipmentToDoes;
            ViewBag.equipment = equipments;

            return PartialView(await equipments.ToListAsync());
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

            string equipmentUidAsReplaced = Request.QueryString["equipmentUidAsReplaced"];
            string ecmUid = "";
            string equipmentAttributeTypeUid = "";
            string locationUid = "";
            string jciTag = "";

            if (!String.IsNullOrEmpty(equipmentUidAsReplaced))
            {
                Equipment eq = db.Equipments.Find(Guid.Parse(equipmentUidAsReplaced));
                ecmUid = eq.ecmUid.ToString();
                equipmentAttributeTypeUid = eq.equipmentAttributeTypeUid.ToString();
                locationUid = eq.locationUid.ToString();
                jciTag = eq.jciTag;
            }           

            var replacementEqupments = db.Equipments.Where(c => c.Location.projectUid == sessionProject);
            var ecms = db.ECMs.Where(c => c.projectUid == sessionProject).OrderBy(c=>c.ecmNumber);

            ViewBag.equipmentTasks = db.EquipmentTasks;
            ViewBag.equipmentToDoes = db.EquipmentToDoes;
            ViewBag.ecms = ecms.ToSelectList(c => c.ecmString, c => c.ecmUid.ToString(), ecmUid);
            ViewBag.equipmentAttributeTypeUid = new SelectList(db.EquipmentAttributeTypes, "equipmentAttributeTypeUid", "equipmentAttributeType1", equipmentAttributeTypeUid);
            ViewBag.locationUid = new SelectList(db.Locations.Where(c => c.projectUid == sessionProject), "locationUid", "location1", locationUid);
            ViewBag.equipmentUidAsReplaced = replacementEqupments.OrderBy(c => c.jciTag).ToSelectList(d => d.jciTag + " - " + d.Location.location1, d => d.equipmentUid.ToString(), equipmentUidAsReplaced);
            ViewBag.jciTag = jciTag;
            
            return View();
        }

        // POST: Equipments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "equipmentUid,equipmentAttributeTypeUid,ecmUid,locationUid,jciTag,ownerTag,manufacturer,model,serialNumber,installDate,area,isNewToSite,useReplacement")] Equipment equipment, string ecms, string equipmentUidAsReplaced, string newTasks)
        {
            if (ModelState.IsValid)
            {
                equipment.equipmentUid = Guid.NewGuid();
                equipment.ecmUid = Guid.Parse(ecms);
                if (equipmentUidAsReplaced != Guid.Empty.ToString())
                {                    
                    equipment.equipmentUidAsReplaced = Guid.Parse(equipmentUidAsReplaced);
                }
                
                db.Equipments.Add(equipment);
                if (newTasks.Length > 0)
                {
                    foreach (var guid in newTasks.Remove(newTasks.Length - 1).Split(','))
                    {
                        EquipmentToDo eq = new EquipmentToDo();
                        eq.equipmentToDoUid = Guid.NewGuid();
                        eq.equipmentTaskUid = Guid.Parse(guid);
                        eq.equipmentUid = equipment.equipmentUid;
                        db.EquipmentToDoes.Add(eq);
                    }
                }                
                await db.SaveChangesAsync();
                return RedirectToAction(Request.QueryString["returnURL"]);
            }

            ViewBag.ecmUid = new SelectList(db.ECMs, "ecmUid", "ecmNumber", equipment.ecmUid);
            ViewBag.equipmentAttributeTypeUid = new SelectList(db.EquipmentAttributeTypes, "equipmentAttributeTypeUid", "equipmentAttributeType1", equipment.equipmentAttributeTypeUid);
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1", equipment.locationUid);
            return View(equipment);
        }

        // GET: Equipments/Edit/5
        public async Task<ActionResult> Edit(Guid? id, string returnURL)
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

            var replacementEqupments = db.Equipments.Where(c => c.Location.projectUid == sessionProject);

            ViewBag.ecmUid = new SelectList(db.ECMs.Where(c => c.projectUid == sessionProject), "ecmUid", "ecmNumber", equipment.ecmUid);
            ViewBag.equipmentAttributeTypeUid = new SelectList(db.EquipmentAttributeTypes, "equipmentAttributeTypeUid", "equipmentAttributeType1", equipment.equipmentAttributeTypeUid);
            ViewBag.locationUid = new SelectList(db.Locations.Where(c => c.projectUid == sessionProject), "locationUid", "location1", equipment.locationUid);
            ViewBag.equipmentUidAsReplaced = replacementEqupments.ToSelectList(d => d.Location.location1 + " - " + d.jciTag, d => d.equipmentUid.ToString(), equipment.equipmentUidAsReplaced.ToString());
            ViewBag.equipmentTasks = db.EquipmentTasks;
            ViewBag.equipmentToDoes = db.EquipmentToDoes;
            return View(equipment);
        }

        // POST: Equipments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "equipmentUid,equipmentAttributeTypeUid,ecmUid,locationUid,jciTag,ownerTag,manufacturer,model,serialNumber,installDate,area,equipmentUidAsReplaced,isNewToSite,useReplacement")] Equipment equipment, string ecms)
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

        // GET: EquipmentToDoes/SaveUseReplacement/5
        public async Task<ActionResult> SaveUseReplacement(string id, string value)
        {

            Equipment eq = db.Equipments.Find(Guid.Parse(id));

            if (value == "true")
            {
                eq.useReplacement = true;
                db.Entry(eq).State = EntityState.Modified;
                try
                {                    
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            else
            {
                eq.useReplacement = false;
                db.Entry(eq).State = EntityState.Modified;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public Task<string> equipments { get; set; }
    }
}
