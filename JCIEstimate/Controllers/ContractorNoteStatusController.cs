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
    public class ContractorNoteStatusController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ContractorNoteStatus
        public async Task<ActionResult> Index()
        {
            return View(await db.ContractorNoteStatus.ToListAsync());
        }

        // GET: ContractorNoteStatus/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorNoteStatu contractorNoteStatu = await db.ContractorNoteStatus.FindAsync(id);
            if (contractorNoteStatu == null)
            {
                return HttpNotFound();
            }
            return View(contractorNoteStatu);
        }

        // GET: ContractorNoteStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContractorNoteStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "contractorNoteStatusUid,contractorNoteStatus,behaviorIndicator")] ContractorNoteStatu contractorNoteStatu)
        {
            if (ModelState.IsValid)
            {
                contractorNoteStatu.contractorNoteStatusUid = Guid.NewGuid();
                db.ContractorNoteStatus.Add(contractorNoteStatu);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(contractorNoteStatu);
        }

        // GET: ContractorNoteStatus/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorNoteStatu contractorNoteStatu = await db.ContractorNoteStatus.FindAsync(id);
            if (contractorNoteStatu == null)
            {
                return HttpNotFound();
            }
            return View(contractorNoteStatu);
        }

        // POST: ContractorNoteStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "contractorNoteStatusUid,contractorNoteStatus,behaviorIndicator")] ContractorNoteStatu contractorNoteStatu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contractorNoteStatu).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(contractorNoteStatu);
        }

        // GET: ContractorNoteStatus/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorNoteStatu contractorNoteStatu = await db.ContractorNoteStatus.FindAsync(id);
            if (contractorNoteStatu == null)
            {
                return HttpNotFound();
            }
            return View(contractorNoteStatu);
        }

        // POST: ContractorNoteStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ContractorNoteStatu contractorNoteStatu = await db.ContractorNoteStatus.FindAsync(id);
            db.ContractorNoteStatus.Remove(contractorNoteStatu);
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
