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
    public class ContractorSignoffAttachmentsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ContractorSignoffAttachments
        public async Task<ActionResult> Index()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            IQueryable<ContractorSignoffAttachment> contractorSignoffAttachments;
            if (User.IsInRole("Admin"))
            {
                contractorSignoffAttachments = from cc in db.ContractorSignoffAttachments
                            where cc.ContractorSignoff.projectUid == sessionProject
                            select cc;
            }
            else
            {
                contractorSignoffAttachments = from cc in db.ContractorSignoffAttachments                            
                            join cn in db.ContractorUsers on cc.ContractorSignoff.contractorUid equals cn.contractorUid
                            join cq in db.AspNetUsers on cn.aspNetUserUid equals cq.Id
                            where cc.ContractorSignoff.projectUid == sessionProject
                            && cq.UserName == System.Web.HttpContext.Current.User.Identity.Name
                            select cc;
            }
            contractorSignoffAttachments = contractorSignoffAttachments.OrderByDescending(c => c.dateCreated).Include(c => c.AspNetUser).Include(c => c.ContractorSignoff);
            return View(await contractorSignoffAttachments.ToListAsync());
        }

        // GET: ContractorSignoffAttachments/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorSignoffAttachment contractorSignoffAttachment = await db.ContractorSignoffAttachments.FindAsync(id);
            if (contractorSignoffAttachment == null)
            {
                return HttpNotFound();
            }
            return View(contractorSignoffAttachment);
        }

        // GET: ContractorSignoffAttachments/Create
        public ActionResult Create()
        {
            ViewBag.aspNetUserUidAsCreated = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.contractorSignoffUid = new SelectList(db.ContractorSignoffs, "contractorSignoffUid", "aspNetUserUidAsCreated");
            return View();
        }        

        // POST: ContractorSignoffAttachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "contractorSignoffAttachmentUid,contractorSignoffUid,aspNetUserUidAsCreated,dateCreated,contractorSignoffAttachment1,attachment,fileType,documentName")] ContractorSignoffAttachment contractorSignoffAttachment)
        {
            if (ModelState.IsValid)
            {
                contractorSignoffAttachment.contractorSignoffAttachmentUid = Guid.NewGuid();
                db.ContractorSignoffAttachments.Add(contractorSignoffAttachment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.aspNetUserUidAsCreated = new SelectList(db.AspNetUsers, "Id", "AllowableContractors", contractorSignoffAttachment.aspNetUserUidAsCreated);
            ViewBag.contractorSignoffUid = new SelectList(db.ContractorSignoffs, "contractorSignoffUid", "aspNetUserUidAsCreated", contractorSignoffAttachment.contractorSignoffUid);
            return View(contractorSignoffAttachment);
        }

        // GET: ContractorSignoffAttachments/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorSignoffAttachment contractorSignoffAttachment = await db.ContractorSignoffAttachments.FindAsync(id);
            if (contractorSignoffAttachment == null)
            {
                return HttpNotFound();
            }
            ViewBag.aspNetUserUidAsCreated = new SelectList(db.AspNetUsers, "Id", "AllowableContractors", contractorSignoffAttachment.aspNetUserUidAsCreated);
            ViewBag.contractorSignoffUid = new SelectList(db.ContractorSignoffs, "contractorSignoffUid", "aspNetUserUidAsCreated", contractorSignoffAttachment.contractorSignoffUid);
            return View(contractorSignoffAttachment);
        }

        // POST: ContractorSignoffAttachments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "contractorSignoffAttachmentUid,contractorSignoffUid,aspNetUserUidAsCreated,dateCreated,contractorSignoffAttachment1,attachment,fileType,documentName")] ContractorSignoffAttachment contractorSignoffAttachment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contractorSignoffAttachment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.aspNetUserUidAsCreated = new SelectList(db.AspNetUsers, "Id", "AllowableContractors", contractorSignoffAttachment.aspNetUserUidAsCreated);
            ViewBag.contractorSignoffUid = new SelectList(db.ContractorSignoffs, "contractorSignoffUid", "aspNetUserUidAsCreated", contractorSignoffAttachment.contractorSignoffUid);
            return View(contractorSignoffAttachment);
        }

        // GET: ContractorSignoffAttachments/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorSignoffAttachment contractorSignoffAttachment = await db.ContractorSignoffAttachments.FindAsync(id);
            if (contractorSignoffAttachment == null)
            {
                return HttpNotFound();
            }
            return View(contractorSignoffAttachment);
        }

        // POST: ContractorSignoffAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ContractorSignoffAttachment contractorSignoffAttachment = await db.ContractorSignoffAttachments.FindAsync(id);
            db.ContractorSignoffAttachments.Remove(contractorSignoffAttachment);
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
