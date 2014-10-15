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
    public class EddressesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: Eddresses
        public async Task<ActionResult> Index()
        {
            var eddresses = db.Eddresses.Include(e => e.ContractorContact);
            return View(await eddresses.ToListAsync());
        }

        // GET: Eddresses/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eddress eddress = await db.Eddresses.FindAsync(id);
            if (eddress == null)
            {
                return HttpNotFound();
            }
            return View(eddress);
        }

        // GET: Eddresses/Create
        public ActionResult Create()
        {
            var contractorContactSelectList = from cc in db.ContractorContacts
                                              join cn in db.Contractors on cc.contractorUid equals cn.contractorUid
                                              select new { Value = cc.contractorContactUid.ToString(), Text = cn.contractorName + " - " + cc.jobTitle };
            ViewBag.contractorContactUid = new SelectList(contractorContactSelectList, "value", "text");

            return View();
        }

        // POST: Eddresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "eddressUid,contractorContactUid,eddress1,isPrimary")] Eddress eddress)
        {
            if (ModelState.IsValid)
            {
                eddress.eddressUid = Guid.NewGuid();
                db.Eddresses.Add(eddress);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            var contractorContactSelectList = from cc in db.ContractorContacts
                                              join cn in db.Contractors on cc.contractorUid equals cn.contractorUid
                                              select new { Value = cc.contractorContactUid.ToString(), Text = cn.contractorName + " - " + cc.jobTitle };
            ViewBag.contractorContactUid = new SelectList(contractorContactSelectList, "value", "text", eddress.contractorContactUid);
            return View(eddress);
        }

        // GET: Eddresses/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eddress eddress = await db.Eddresses.FindAsync(id);
            if (eddress == null)
            {
                return HttpNotFound();
            }
            var contractorContactSelectList = from cc in db.ContractorContacts
                                              join cn in db.Contractors on cc.contractorUid equals cn.contractorUid
                                              select new { Value = cc.contractorContactUid.ToString(), Text = cn.contractorName + " - " + cc.jobTitle };
            ViewBag.contractorContactUid = new SelectList(contractorContactSelectList, "value", "text", eddress.contractorContactUid);
            return View(eddress);
        }

        // POST: Eddresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "eddressUid,contractorContactUid,eddress1,isPrimary")] Eddress eddress)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eddress).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            var contractorContactSelectList = from cc in db.ContractorContacts
                                              join cn in db.Contractors on cc.contractorUid equals cn.contractorUid
                                              select new { Value = cc.contractorContactUid.ToString(), Text = cn.contractorName + " - " + cc.jobTitle };
            ViewBag.contractorContactUid = new SelectList(contractorContactSelectList, "value", "text", eddress.contractorContactUid);
            return View(eddress);
        }

        // GET: Eddresses/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eddress eddress = await db.Eddresses.FindAsync(id);
            if (eddress == null)
            {
                return HttpNotFound();
            }
            return View(eddress);
        }

        // POST: Eddresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Eddress eddress = await db.Eddresses.FindAsync(id);
            db.Eddresses.Remove(eddress);
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
