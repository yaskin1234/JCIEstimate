﻿using System;
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
    [Authorize]
    public class ContractorsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: Contractors
        public async Task<ActionResult> Index()
        {
            return View(await db.Contractors.OrderBy(c=>c.contractorName).ToListAsync());
        }

        // GET: Contractors/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contractor contractor = await db.Contractors.FindAsync(id);
            if (contractor == null)
            {
                return HttpNotFound();
            }
            return View(contractor);
        }

        // GET: Contractors/Create
        public ActionResult Create()
        {
            ViewBag.contractorClassificationUid = db.ContractorClassifications.ToSelectList(c=>c.contractorClassification1, c=>c.contractorClassificationUid.ToString(), "");
            ViewBag.costCodeUid = db.CostCodes.OrderBy(c => c.costCode1).ToSelectList(c => c.costCode1 + " - " + c.costCodeDescription, c => c.costCodeUid.ToString(), "");
            return View();
        }

        // POST: Contractors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "contractorUid,contractorName,isActive,engScopeCompleted,contractorSelected,contractorGroup,contractorClassificationUid,costCodeUid")] Contractor contractor)
        {
            if (ModelState.IsValid)
            {
                contractor.contractorUid = Guid.NewGuid();
                db.Contractors.Add(contractor);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(contractor);
        }

        // GET: Contractors/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contractor contractor = await db.Contractors.FindAsync(id);
            if (contractor == null)
            {
                return HttpNotFound();
            }

            ViewBag.contractorClassificationUid = db.ContractorClassifications.ToSelectList(c => c.contractorClassification1, c => c.contractorClassificationUid.ToString(), contractor.contractorClassificationUid.ToString());
            ViewBag.costCodeUid = db.CostCodes.OrderBy(c=>c.costCode1).ToSelectList(c => c.costCode1 + " - " + c.costCodeDescription, c => c.costCodeUid.ToString(), contractor.costCodeUid.ToString());
            return View(contractor);
        }

        // POST: Contractors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "contractorUid,contractorName,isActive,engScopeCompleted,contractorSelected,contractorGroup,contractorClassificationUid,costCodeUid")] Contractor contractor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contractor).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(contractor);
        }

        // GET: Contractors/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contractor contractor = await db.Contractors.FindAsync(id);
            if (contractor == null)
            {
                return HttpNotFound();
            }
            return View(contractor);
        }

        // POST: Contractors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Contractor contractor = await db.Contractors.FindAsync(id);
            db.Contractors.Remove(contractor);
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
