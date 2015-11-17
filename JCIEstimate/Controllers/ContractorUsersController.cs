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
    public class ContractorUsersController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ContractorUsers
        public async Task<ActionResult> Index()
        {
            var contractorUsers = db.ContractorUsers.Include(c => c.Contractor).Include(c => c.AspNetUser).OrderBy(c=>c.Contractor.contractorName).ThenBy(c=>c.AspNetUser.UserName);
            return View(await contractorUsers.ToListAsync());
        }

        // GET: ContractorUsers/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorUser contractorUser = await db.ContractorUsers.FindAsync(id);
            if (contractorUser == null)
            {
                return HttpNotFound();
            }
            return View(contractorUser);
        }

        // GET: ContractorUsers/Create
        public ActionResult Create()
        {
            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName");
            ViewBag.aspNetUserUid = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: ContractorUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "contractorUserUid,aspNetUserUid,contractorUid")] ContractorUser contractorUser)
        {
            if (ModelState.IsValid)
            {
                contractorUser.contractorUserUid = Guid.NewGuid();
                db.ContractorUsers.Add(contractorUser);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName", contractorUser.contractorUid);
            ViewBag.aspNetUserUid = new SelectList(db.AspNetUsers, "Id", "Email", contractorUser.aspNetUserUid);
            return View(contractorUser);
        }

        // GET: ContractorUsers/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorUser contractorUser = await db.ContractorUsers.FindAsync(id);
            if (contractorUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName", contractorUser.contractorUid);
            ViewBag.aspNetUserUid = new SelectList(db.AspNetUsers, "Id", "Email", contractorUser.aspNetUserUid);
            return View(contractorUser);
        }

        // POST: ContractorUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "contractorUserUid,aspNetUserUid,contractorUid")] ContractorUser contractorUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contractorUser).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName", contractorUser.contractorUid);
            ViewBag.aspNetUserUid = new SelectList(db.AspNetUsers, "Id", "Email", contractorUser.aspNetUserUid);
            return View(contractorUser);
        }

        // GET: ContractorUsers/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorUser contractorUser = await db.ContractorUsers.FindAsync(id);
            if (contractorUser == null)
            {
                return HttpNotFound();
            }
            return View(contractorUser);
        }

        // POST: ContractorUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ContractorUser contractorUser = await db.ContractorUsers.FindAsync(id);
            db.ContractorUsers.Remove(contractorUser);
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
