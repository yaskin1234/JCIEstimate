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
    public class ContractorClassificationsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: /ContractorClassifications/
        public async Task<ActionResult> Index()
        {
            return View(await db.ContractorClassifications.ToListAsync());
        }

        // GET: /ContractorClassifications/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorClassification contractorclassification = await db.ContractorClassifications.FindAsync(id);
            if (contractorclassification == null)
            {
                return HttpNotFound();
            }
            return View(contractorclassification);
        }

        // GET: /ContractorClassifications/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /ContractorClassifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="contractorClassificationUid,contractorClassification1,contractorClassificationDescription")] ContractorClassification contractorclassification)
        {
            if (ModelState.IsValid)
            {
                contractorclassification.contractorClassificationUid = Guid.NewGuid();
                db.ContractorClassifications.Add(contractorclassification);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(contractorclassification);
        }

        // GET: /ContractorClassifications/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorClassification contractorclassification = await db.ContractorClassifications.FindAsync(id);
            if (contractorclassification == null)
            {
                return HttpNotFound();
            }
            return View(contractorclassification);
        }

        // POST: /ContractorClassifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="contractorClassificationUid,contractorClassification1,contractorClassificationDescription")] ContractorClassification contractorclassification)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contractorclassification).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(contractorclassification);
        }

        // GET: /ContractorClassifications/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorClassification contractorclassification = await db.ContractorClassifications.FindAsync(id);
            if (contractorclassification == null)
            {
                return HttpNotFound();
            }
            return View(contractorclassification);
        }

        // POST: /ContractorClassifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ContractorClassification contractorclassification = await db.ContractorClassifications.FindAsync(id);
            db.ContractorClassifications.Remove(contractorclassification);
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
