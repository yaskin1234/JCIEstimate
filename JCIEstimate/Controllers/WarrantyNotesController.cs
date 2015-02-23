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
    public class WarrantyNotesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: WarrantyNotes
        public async Task<ActionResult> Index()
        {
            var warrantyNotes = db.WarrantyNotes.Include(w => w.WarrantyIssue);
            return View(await warrantyNotes.ToListAsync());
        }

        // GET: WarrantyNotes/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WarrantyNote warrantyNote = await db.WarrantyNotes.FindAsync(id);
            if (warrantyNote == null)
            {
                return HttpNotFound();
            }
            return View(warrantyNote);
        }

        // GET: WarrantyNotes/Create
        public ActionResult Create()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            var warrantyIssues = from cc in db.WarrantyIssues
                                 where cc.WarrantyUnit.Location.projectUid == sessionProject
                                 select cc;

            ViewBag.warrantyIssueUid = warrantyIssues.ToSelectList(c => c.WarrantyUnit.Location.location1 + " - " + c.WarrantyUnit.warrantyUnit1 + " - " + c.warrantyIssueLocation, c => c.warrantyIssueUid.ToString(), "");
            return View();
        }

        // POST: WarrantyNotes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "warrantyNoteUid,warrantyIssueUid,warrantyNote1,date")] WarrantyNote warrantyNote)
        {
            if (ModelState.IsValid)
            {
                warrantyNote.date = DateTime.Now;
                warrantyNote.warrantyNoteUid = Guid.NewGuid();
                db.WarrantyNotes.Add(warrantyNote);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.warrantyIssueUid = new SelectList(db.WarrantyIssues, "warrantyIssueUid", "warrantyIssueLocation", warrantyNote.warrantyIssueUid);
            return View(warrantyNote);
        }

        // GET: WarrantyNotes/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WarrantyNote warrantyNote = await db.WarrantyNotes.FindAsync(id);
            if (warrantyNote == null)
            {
                return HttpNotFound();
            }
            ViewBag.warrantyIssueUid = new SelectList(db.WarrantyIssues, "warrantyIssueUid", "warrantyIssueLocation", warrantyNote.warrantyIssueUid);
            return View(warrantyNote);
        }

        // POST: WarrantyNotes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "warrantyNoteUid,warrantyIssueUid,warrantyNote1,date")] WarrantyNote warrantyNote)
        {
            if (ModelState.IsValid)
            {
                db.Entry(warrantyNote).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.warrantyIssueUid = new SelectList(db.WarrantyIssues, "warrantyIssueUid", "warrantyIssueLocation", warrantyNote.warrantyIssueUid);
            return View(warrantyNote);
        }

        // GET: WarrantyNotes/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WarrantyNote warrantyNote = await db.WarrantyNotes.FindAsync(id);
            if (warrantyNote == null)
            {
                return HttpNotFound();
            }
            return View(warrantyNote);
        }

        // POST: WarrantyNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            WarrantyNote warrantyNote = await db.WarrantyNotes.FindAsync(id);
            db.WarrantyNotes.Remove(warrantyNote);
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
