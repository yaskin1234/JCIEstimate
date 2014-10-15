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
    public class ContractorContactsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ContractorContacts
        public async Task<ActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                var contractorContacts = db.ContractorContacts.Include(c => c.Contractor);
                return View(await contractorContacts.ToListAsync());
            }
            else
            {
                var contractorContactsLimited = from cc in db.ContractorContacts
                                                join cn in db.ContractorUsers on cc.contractorUid equals cn.contractorUid
                                                join cq in db.AspNetUsers on cn.aspNetUserUid equals cq.Id
                                                where cq.UserName == System.Web.HttpContext.Current.User.Identity.Name 
                                                select cc;
                contractorContactsLimited = contractorContactsLimited.Include(c => c.Contractor);                
                return View(await contractorContactsLimited.ToListAsync());
            }
        }

        // GET: ContractorContacts/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorContact contractorContact = await db.ContractorContacts.FindAsync(id);
            if (contractorContact == null)
            {
                return HttpNotFound();
            }
            return View(contractorContact);
        }

        // GET: ContractorContacts/Create
        public ActionResult Create()
        {
            if (User.IsInRole("Admin"))
            {                
                ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName");
            }
            else
            {
                var contractorLimited = from cc in db.Contractors
                                                join cn in db.ContractorUsers on cc.contractorUid equals cn.contractorUid
                                                join cq in db.AspNetUsers on cn.aspNetUserUid equals cq.Id
                                                where cq.UserName == System.Web.HttpContext.Current.User.Identity.Name
                                                select cc;                ;
                ViewBag.contractorUid = new SelectList(contractorLimited, "contractorUid", "contractorName");
            }
            
            return View();
        }

        // POST: ContractorContacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "contractorContactUid,contractorUid,jobTitle,firstName,lastName")] ContractorContact contractorContact)
        {
            if (ModelState.IsValid)
            {
                contractorContact.contractorContactUid = Guid.NewGuid();
                db.ContractorContacts.Add(contractorContact);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            if (User.IsInRole("Admin"))
            {
                ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName");
            }
            else
            {
                var contractorLimited = from cc in db.Contractors
                                        join cn in db.ContractorUsers on cc.contractorUid equals cn.contractorUid
                                        join cq in db.AspNetUsers on cn.aspNetUserUid equals cq.Id
                                        where cq.UserName == System.Web.HttpContext.Current.User.Identity.Name
                                        select cc; ;
                ViewBag.contractorUid = new SelectList(contractorLimited, "contractorUid", "contractorName", contractorContact.contractorUid);
            }
            
            return View(contractorContact);
        }

        // GET: ContractorContacts/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorContact contractorContact = await db.ContractorContacts.FindAsync(id);
            if (contractorContact == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Admin"))
            {
                ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName");
            }
            else
            {
                var contractorLimited = from cc in db.Contractors
                                        join cn in db.ContractorUsers on cc.contractorUid equals cn.contractorUid
                                        join cq in db.AspNetUsers on cn.aspNetUserUid equals cq.Id
                                        where cq.UserName == System.Web.HttpContext.Current.User.Identity.Name
                                        select cc; ;
                ViewBag.contractorUid = new SelectList(contractorLimited, "contractorUid", "contractorName", contractorContact.contractorUid);
            }
            return View(contractorContact);
        }

        // POST: ContractorContacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "contractorContactUid,contractorUid,jobTitle,firstName,lastName")] ContractorContact contractorContact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contractorContact).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            if (User.IsInRole("Admin"))
            {
                ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName");
            }
            else
            {
                var contractorLimited = from cc in db.Contractors
                                        join cn in db.ContractorUsers on cc.contractorUid equals cn.contractorUid
                                        join cq in db.AspNetUsers on cn.aspNetUserUid equals cq.Id
                                        where cq.UserName == System.Web.HttpContext.Current.User.Identity.Name
                                        select cc; ;
                ViewBag.contractorUid = new SelectList(contractorLimited, "contractorUid", "contractorName", contractorContact.contractorUid);
            }
            return View(contractorContact);
        }

        // GET: ContractorContacts/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorContact contractorContact = await db.ContractorContacts.FindAsync(id);
            if (contractorContact == null)
            {
                return HttpNotFound();
            }
            return View(contractorContact);
        }

        // POST: ContractorContacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ContractorContact contractorContact = await db.ContractorContacts.FindAsync(id);
            db.ContractorContacts.Remove(contractorContact);
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
