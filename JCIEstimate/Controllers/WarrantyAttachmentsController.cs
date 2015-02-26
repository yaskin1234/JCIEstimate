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
    public class WarrantyAttachmentsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: WarrantyAttachments
        public async Task<ActionResult> Index()
        {
            var warrantyAttachments = db.WarrantyAttachments.Include(w => w.WarrantyIssue);
            return View(await warrantyAttachments.ToListAsync());
        }


        // GET: WarrantyAttachments/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WarrantyAttachment warrantyAttachment = await db.WarrantyAttachments.FindAsync(id);
            if (warrantyAttachment == null)
            {
                return HttpNotFound();
            }
            return View(warrantyAttachment);
        }

        // GET: WarrantyAttachments/Create
        public ActionResult Create()
        {
            ViewBag.warrantyIssueUid = new SelectList(db.WarrantyIssues, "warrantyIssueUid", "warrantyIssueLocation");
            return View();
        }

        // POST: WarrantyAttachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "warrantyAttachmentUid,warrantyIssueUid,warrantyAttachment1,document,fileType,documentName")] WarrantyAttachment warrantyAttachment)
        {
            if (ModelState.IsValid)
            {
                warrantyAttachment.warrantyAttachmentUid = Guid.NewGuid();
                db.WarrantyAttachments.Add(warrantyAttachment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.warrantyIssueUid = new SelectList(db.WarrantyIssues, "warrantyIssueUid", "warrantyIssueLocation", warrantyAttachment.warrantyIssueUid);
            return View(warrantyAttachment);
        }

        // GET: WarrantyAttachments/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WarrantyAttachment warrantyAttachment = await db.WarrantyAttachments.FindAsync(id);
            if (warrantyAttachment == null)
            {
                return HttpNotFound();
            }
            ViewBag.warrantyIssueUid = new SelectList(db.WarrantyIssues, "warrantyIssueUid", "warrantyIssueLocation", warrantyAttachment.warrantyIssueUid);
            return View(warrantyAttachment);
        }

        // POST: WarrantyAttachments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "warrantyAttachmentUid,warrantyIssueUid,warrantyAttachment1,document,fileType,documentName")] WarrantyAttachment warrantyAttachment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(warrantyAttachment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.warrantyIssueUid = new SelectList(db.WarrantyIssues, "warrantyIssueUid", "warrantyIssueLocation", warrantyAttachment.warrantyIssueUid);
            return View(warrantyAttachment);
        }

        // GET: WarrantyAttachments/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WarrantyAttachment warrantyAttachment = await db.WarrantyAttachments.FindAsync(id);
            if (warrantyAttachment == null)
            {
                return HttpNotFound();
            }
            return View(warrantyAttachment);
        }

        // POST: WarrantyAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            WarrantyAttachment warrantyAttachment = await db.WarrantyAttachments.FindAsync(id);
            db.WarrantyAttachments.Remove(warrantyAttachment);
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
