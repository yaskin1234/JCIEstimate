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
using System.IO;

namespace JCIEstimate.Controllers
{
    public class EquipmentAttachmentsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: EquipmentAttachments
        public async Task<ActionResult> Index()
        {
            var equipmentAttachments = db.EquipmentAttachments.Include(e => e.Equipment);
            return View(await equipmentAttachments.ToListAsync());
        }

        // GET: EquipmentAttachments/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentAttachment equipmentAttachment = await db.EquipmentAttachments.FindAsync(id);
            if (equipmentAttachment == null)
            {
                return HttpNotFound();
            }
            return View(equipmentAttachment);
        }

        // GET: EquipmentAttachments/Create
        public ActionResult Create()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            var equipments = from cc in db.Equipments
                             where cc.Location.projectUid == sessionProject
                             select cc;
            ViewBag.equipmentUid = equipments.OrderBy(c => c.jciTag).ToSelectList(d => d.jciTag + " - " + d.Location.location1, d => d.equipmentUid.ToString(), "");            

            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetEquipmentAttachment(Guid equipmentAttachmentUid, string fileType)
        {
            var d = from cc in db.EquipmentAttachments
                    where cc.equipmentAttachmentUid == equipmentAttachmentUid
                    select cc.attachment;

            var docName = from cc in db.EquipmentAttachments
                    where cc.equipmentAttachmentUid == equipmentAttachmentUid
                    select cc.documentName;
            

            byte[] byteArray = d.FirstOrDefault();
            return File(byteArray, "application/octect-stream", docName.FirstOrDefault());
        }

        // POST: EquipmentAttachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EquipmentAttachment equipmentAttachment, HttpPostedFileBase postedFile)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            if (ModelState.IsValid)
            {
                if (postedFile != null)
                {
                    int fileSize = postedFile.ContentLength;
                    MemoryStream target = new MemoryStream();
                    postedFile.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();
                    equipmentAttachment.attachment = data;
                    equipmentAttachment.fileType = Path.GetExtension(postedFile.FileName);
                }


                var docName = postedFile.FileName;
                equipmentAttachment.documentName = docName;

                equipmentAttachment.equipmentAttachmentUid = Guid.NewGuid();
                db.EquipmentAttachments.Add(equipmentAttachment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            var equipments = from cc in db.Equipments
                             where cc.Location.projectUid == sessionProject
                             select cc;
            ViewBag.equipmentUid = equipments.OrderBy(c => c.jciTag).ToSelectList(d => d.jciTag + " - " + d.Location.location1, d => d.equipmentUid.ToString(), "");    
            return View(equipmentAttachment);
        }

        // GET: EquipmentAttachments/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentAttachment equipmentAttachment = await db.EquipmentAttachments.FindAsync(id);
            if (equipmentAttachment == null)
            {
                return HttpNotFound();
            }

            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            var equipments = from cc in db.Equipments
                             where cc.Location.projectUid == sessionProject
                             select cc;
            ViewBag.equipmentUid = equipments.OrderBy(c => c.jciTag).ToSelectList(d => d.jciTag + " - " + d.Location.location1, d => d.equipmentUid.ToString(), equipmentAttachment.equipmentUid.ToString());            
            
            return View(equipmentAttachment);
        }

        // POST: EquipmentAttachments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "equipmentAttachmentUid,equipmentUid,equipmentAttachment1,attachment,fileType,documentName")] EquipmentAttachment equipmentAttachment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipmentAttachment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.equipmentUid = new SelectList(db.Equipments, "equipmentUid", "jciTag", equipmentAttachment.equipmentUid);
            return View(equipmentAttachment);
        }

        // GET: EquipmentAttachments/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentAttachment equipmentAttachment = await db.EquipmentAttachments.FindAsync(id);
            if (equipmentAttachment == null)
            {
                return HttpNotFound();
            }
            return View(equipmentAttachment);
        }

        // POST: EquipmentAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            EquipmentAttachment equipmentAttachment = await db.EquipmentAttachments.FindAsync(id);
            db.EquipmentAttachments.Remove(equipmentAttachment);
            await db.SaveChangesAsync();
            return RedirectToAction("Edit", "Equipments", new { id = equipmentAttachment.equipmentUid });
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
