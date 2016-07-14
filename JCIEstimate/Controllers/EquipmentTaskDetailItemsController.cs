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
    public class EquipmentTaskDetailItemsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: /EquipmentTaskDetailItems/
        public async Task<ActionResult> Index(string filterId)
        {
            
            var equipmenttaskdetailitems = db.EquipmentTaskDetailItems.Include(e => e.Contractor).Include(e => e.EquipmentTaskDetail).Include(e => e.EquipmentToDo).OrderBy(c=>c.EquipmentTaskDetail.sequence);
            return View(await equipmenttaskdetailitems.ToListAsync());
        }

        // GET: /EquipmentTaskDetailItems/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentTaskDetailItem equipmenttaskdetailitem = await db.EquipmentTaskDetailItems.FindAsync(id);
            if (equipmenttaskdetailitem == null)
            {
                return HttpNotFound();
            }
            return View(equipmenttaskdetailitem);
        }

        // GET: /EquipmentTaskDetailItems/Create
        public ActionResult Create()
        {
            ViewBag.contractorUidAsAssigned = new SelectList(db.Contractors, "contractorUid", "contractorName");
            ViewBag.equipmentTaskDetailUid = new SelectList(db.EquipmentTaskDetails, "equipmentTaskDetailUid", "equipmentTaskDetail1");
            ViewBag.equipmentToDoUid = new SelectList(db.EquipmentToDoes, "equipmentToDoUid", "equipmentToDoUid");
            return View();
        }

        // POST: /EquipmentTaskDetailItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="equipmentTaskDetailItemUid,equipmentTaskDetailUid,equipmentToDoUid,contractorUidAsAssigned,startDate,endDate")] EquipmentTaskDetailItem equipmenttaskdetailitem)
        {
            if (ModelState.IsValid)
            {
                equipmenttaskdetailitem.equipmentTaskDetailItemUid = Guid.NewGuid();
                db.EquipmentTaskDetailItems.Add(equipmenttaskdetailitem);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.contractorUidAsAssigned = new SelectList(db.Contractors, "contractorUid", "contractorName", equipmenttaskdetailitem.contractorUidAsAssigned);
            ViewBag.equipmentTaskDetailUid = new SelectList(db.EquipmentTaskDetails, "equipmentTaskDetailUid", "equipmentTaskDetail1", equipmenttaskdetailitem.equipmentTaskDetailUid);
            ViewBag.equipmentToDoUid = new SelectList(db.EquipmentToDoes, "equipmentToDoUid", "equipmentToDoUid", equipmenttaskdetailitem.equipmentToDoUid);
            return View(equipmenttaskdetailitem);
        }

        // GET: /EquipmentTaskDetailItems/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentTaskDetailItem equipmenttaskdetailitem = await db.EquipmentTaskDetailItems.FindAsync(id);
            if (equipmenttaskdetailitem == null)
            {
                return HttpNotFound();
            }
            ViewBag.contractorUidAsAssigned = new SelectList(db.Contractors, "contractorUid", "contractorName", equipmenttaskdetailitem.contractorUidAsAssigned);
            ViewBag.equipmentTaskDetailUid = new SelectList(db.EquipmentTaskDetails, "equipmentTaskDetailUid", "equipmentTaskDetail1", equipmenttaskdetailitem.equipmentTaskDetailUid);
            ViewBag.equipmentToDoUid = new SelectList(db.EquipmentToDoes, "equipmentToDoUid", "equipmentToDoUid", equipmenttaskdetailitem.equipmentToDoUid);
            return View(equipmenttaskdetailitem);
        }

        public async Task<ActionResult> SaveItem(string type, string id, string value)
        {
            EquipmentTaskDetailItem equipmenttaskdetailitem = await db.EquipmentTaskDetailItems.FindAsync(Guid.Parse(id));
            db.Entry(equipmenttaskdetailitem).State = EntityState.Modified;
            if (type == "contractor")
            {
                equipmenttaskdetailitem.contractorUidAsAssigned = Guid.Parse(value);
            }
            else if (type == "startDate")
            {
                equipmenttaskdetailitem.startDate = DateTime.Parse(value);
            }
            else if (type == "endDate")
            {
                equipmenttaskdetailitem.endDate = DateTime.Parse(value);
            }

            db.SaveChanges();

            return Json("success");
        }


        // GET: /EquipmentTaskDetailItems/Edit/5
        public async Task<ActionResult> EditList(string id)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            Guid equipmentUid = Guid.Parse(id.Split('_')[0]);
            Guid equipmentTaskUid = Guid.Parse(id.Split('_')[1]);
            Guid? equipmentToDoUid;
            var etdUid = db.EquipmentToDoes.Where(c => c.equipmentTaskUid == equipmentTaskUid).Where(c => c.equipmentUid == equipmentUid).FirstOrDefault();
            if (etdUid != null)
            {
                equipmentToDoUid = db.EquipmentToDoes.Where(c => c.equipmentTaskUid == equipmentTaskUid).Where(c => c.equipmentUid == equipmentUid).FirstOrDefault().equipmentToDoUid;
            }
            else
            {
                equipmentToDoUid = null;
            }
            
            var items = db.EquipmentTaskDetailItems.Where(c => c.EquipmentToDo.equipmentToDoUid == equipmentToDoUid);

            var contractors = db.Estimates.Where(c=>c.Location.projectUid == sessionProject).GroupBy(c => c.contractorUid).Select(v => v.FirstOrDefault());
            ViewBag.contractorUidAsAssigned = contractors.ToSelectList(c => c.Contractor.contractorName, c => c.contractorUid.ToString(), "");

            return View(items);
        }

        // POST: /EquipmentTaskDetailItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "equipmentTaskDetailItemUid,equipmentTaskDetailUid,equipmentToDoUid,contractorUidAsAssigned,startDate,endDate")] EquipmentTaskDetailItem equipmenttaskdetailitem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipmenttaskdetailitem).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.contractorUidAsAssigned = new SelectList(db.Contractors, "contractorUid", "contractorName", equipmenttaskdetailitem.contractorUidAsAssigned);
            ViewBag.equipmentTaskDetailUid = new SelectList(db.EquipmentTaskDetails, "equipmentTaskDetailUid", "equipmentTaskDetail1", equipmenttaskdetailitem.equipmentTaskDetailUid);
            ViewBag.equipmentToDoUid = new SelectList(db.EquipmentToDoes, "equipmentToDoUid", "equipmentToDoUid", equipmenttaskdetailitem.equipmentToDoUid);
            return View(equipmenttaskdetailitem);
        }

        // GET: /EquipmentTaskDetailItems/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentTaskDetailItem equipmenttaskdetailitem = await db.EquipmentTaskDetailItems.FindAsync(id);
            if (equipmenttaskdetailitem == null)
            {
                return HttpNotFound();
            }
            return View(equipmenttaskdetailitem);
        }

        // POST: /EquipmentTaskDetailItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            EquipmentTaskDetailItem equipmenttaskdetailitem = await db.EquipmentTaskDetailItems.FindAsync(id);
            db.EquipmentTaskDetailItems.Remove(equipmenttaskdetailitem);
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
