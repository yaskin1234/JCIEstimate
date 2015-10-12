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
    public class ContractorNoteTypesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ContractorNoteTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.ContractorNoteTypes.ToListAsync());
        }

        // GET: ContractorNoteTypes/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorNoteType contractorNoteType = await db.ContractorNoteTypes.FindAsync(id);
            if (contractorNoteType == null)
            {
                return HttpNotFound();
            }
            return View(contractorNoteType);
        }

        // GET: ContractorNoteTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContractorNoteTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "contractorNoteTypeUid,contractorNoteType1,behaviorIndicator")] ContractorNoteType contractorNoteType)
        {
            if (ModelState.IsValid)
            {
                contractorNoteType.contractorNoteTypeUid = Guid.NewGuid();
                db.ContractorNoteTypes.Add(contractorNoteType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(contractorNoteType);
        }

        // GET: ContractorNoteTypes/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorNoteType contractorNoteType = await db.ContractorNoteTypes.FindAsync(id);
            if (contractorNoteType == null)
            {
                return HttpNotFound();
            }
            return View(contractorNoteType);
        }

        // POST: ContractorNoteTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "contractorNoteTypeUid,contractorNoteType1,behaviorIndicator")] ContractorNoteType contractorNoteType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contractorNoteType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(contractorNoteType);
        }

        // GET: ContractorNoteTypes/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorNoteType contractorNoteType = await db.ContractorNoteTypes.FindAsync(id);
            if (contractorNoteType == null)
            {
                return HttpNotFound();
            }
            return View(contractorNoteType);
        }

        // POST: ContractorNoteTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ContractorNoteType contractorNoteType = await db.ContractorNoteTypes.FindAsync(id);
            db.ContractorNoteTypes.Remove(contractorNoteType);
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
