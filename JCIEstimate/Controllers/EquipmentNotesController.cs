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
    public class EquipmentNotesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: EquipmentNotes
        public async Task<ActionResult> Index()
        {
            var equipmentNotes = db.EquipmentNotes.Include(e => e.Equipment).Include(e => e.EquipmentNoteType);
            return View(await equipmentNotes.ToListAsync());
        }

        // GET: EquipmentNotes/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentNote equipmentNote = await db.EquipmentNotes.FindAsync(id);
            if (equipmentNote == null)
            {
                return HttpNotFound();
            }
            return View(equipmentNote);
        }

        // GET: EquipmentNotes/Create
        public ActionResult Create()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            var equipments = db.Equipments.Where(c => c.Location.projectUid == sessionProject).OrderBy(c=>c.jciTag);
            equipments = equipments.Include(e => e.ECM).Include(e => e.EquipmentAttributeType).Include(e => e.Location).OrderBy(c => c.jciTag);

            ViewBag.equipmentUid = equipments.ToSelectList(c => c.Location.location1 + "-" + c.jciTag + "-" + c.EquipmentAttributeType.equipmentAttributeType1 + "-" + Convert.ToDateTime(c.installDate).Year + "-" + c.ECM.ecmNumber, c => c.equipmentUid.ToString(), "");
            ViewBag.equipmentNoteTypeUid = new SelectList(db.EquipmentNoteTypes, "equipmentNoteTypeUid", "equipmentNoteType1");
            return View();
        }

        // POST: EquipmentNotes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "equipmentNoteUid,equipmentUid,equipmentNoteTypeUid,equipmentNote1,date,aspNetUserUidAsCreated")] EquipmentNote equipmentNote)
        {
            if (ModelState.IsValid)
            {
                equipmentNote.equipmentNoteUid = Guid.NewGuid();
                var user = from cc in db.AspNetUsers
                           where cc.UserName == System.Web.HttpContext.Current.User.Identity.Name
                           select cc.Id;
                equipmentNote.aspNetUserUidAsCreated = user.First();
                equipmentNote.date = DateTime.Now;
                db.EquipmentNotes.Add(equipmentNote);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.equipmentUid = new SelectList(db.Equipments, "equipmentUid", "ownerTag", equipmentNote.equipmentUid);
            ViewBag.equipmentNoteTypeUid = new SelectList(db.EquipmentNoteTypes, "equipmentNoteTypeUid", "equipmentNoteType1", equipmentNote.equipmentNoteTypeUid);
            return View(equipmentNote);
        }

        // GET: EquipmentNotes/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentNote equipmentNote = await db.EquipmentNotes.FindAsync(id);
            if (equipmentNote == null)
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
            var equipments = db.Equipments.Where(c => c.Location.projectUid == sessionProject);
            equipments = equipments.Include(e => e.ECM).Include(e => e.EquipmentAttributeType).Include(e => e.Location).OrderBy(c => c.jciTag);

            ViewBag.equipmentUid = equipments.ToSelectList(c => c.Location.location1 + "-" + c.jciTag + "-" + c.EquipmentAttributeType.equipmentAttributeType1 + "-" + Convert.ToDateTime(c.installDate).Year + "-" + c.ECM.ecmNumber, c => c.equipmentUid.ToString(), equipmentNote.equipmentUid.ToString());
            ViewBag.equipmentNoteTypeUid = new SelectList(db.EquipmentNoteTypes, "equipmentNoteTypeUid", "equipmentNoteType1", equipmentNote.equipmentNoteTypeUid);
            return View(equipmentNote);
        }

        // POST: EquipmentNotes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "equipmentNoteUid,equipmentUid,equipmentNoteTypeUid,equipmentNote1,date,aspNetUserUidAsCreated")] EquipmentNote equipmentNote)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipmentNote).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.equipmentUid = new SelectList(db.Equipments, "equipmentUid", "ownerTag", equipmentNote.equipmentUid);
            ViewBag.equipmentNoteTypeUid = new SelectList(db.EquipmentNoteTypes, "equipmentNoteTypeUid", "equipmentNoteType1", equipmentNote.equipmentNoteTypeUid);
            return View(equipmentNote);
        }

        // GET: EquipmentNotes/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentNote equipmentNote = await db.EquipmentNotes.FindAsync(id);
            if (equipmentNote == null)
            {
                return HttpNotFound();
            }
            return View(equipmentNote);
        }

        // POST: EquipmentNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            EquipmentNote equipmentNote = await db.EquipmentNotes.FindAsync(id);
            db.EquipmentNotes.Remove(equipmentNote);
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
