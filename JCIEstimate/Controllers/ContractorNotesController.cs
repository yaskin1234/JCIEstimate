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
    public class ContractorNotesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ContractorNotes
        public async Task<ActionResult> Index()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            IQueryable<ContractorNote> contractorNotes;            
            if (User.IsInRole("Admin"))
            {
                contractorNotes = from cc in db.ContractorNotes                            
                                  where cc.projectUid == sessionProject
                                  orderby cc.Contractor.contractorName, cc.ContractorNoteType.contractorNoteType1
                                  select cc;
            }
            else
            {
                contractorNotes = from cc in db.ContractorNotes                            
                                  join cn in db.ContractorUsers on cc.contractorUid equals cn.contractorUid
                                  join cq in db.AspNetUsers on cn.aspNetUserUid equals cq.Id
                                  where cc.projectUid == sessionProject
                                  && cc.contractorUid == cn.contractorUid
                                  && cq.UserName == System.Web.HttpContext.Current.User.Identity.Name
                                  orderby cc.ContractorNoteType.contractorNoteType1
                                  select cc;
            }

            contractorNotes = contractorNotes.Include(c => c.Contractor).Include(c => c.ContractorNoteType).Include(c => c.Project);
            return View(await contractorNotes.ToListAsync());
        }

        // GET: ContractorNotes/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorNote contractorNote = await db.ContractorNotes.FindAsync(id);
            if (contractorNote == null)
            {
                return HttpNotFound();
            }
            return View(contractorNote);
        }

        // GET: ContractorNotes/Create
        public ActionResult Create()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            IQueryable<Contractor> contractor;

            if (User.IsInRole("Admin"))
            {
                contractor = from cc in db.Contractors                                                               
                             orderby cc.contractorName
                             select cc;
            }
            else
            {
                contractor = from cc in db.Contractors
                                 join cn in db.ContractorUsers on cc.contractorUid equals cn.contractorUid
                                 join cq in db.AspNetUsers on cn.aspNetUserUid equals cq.Id
                                 where cq.UserName == System.Web.HttpContext.Current.User.Identity.Name
                                 select cc;
            }

            ViewBag.contractorUid = new SelectList(contractor, "contractorUid", "contractorName");
            ViewBag.contractorNoteTypeUid = new SelectList(db.ContractorNoteTypes, "contractorNoteTypeUid", "contractorNoteType1");
            ViewBag.contractorNoteStatusUid = new SelectList(db.ContractorNoteStatus, "contractorNoteStatusUid", "contractorNoteStatus");
            ViewBag.projectUid = new SelectList(db.Projects.Where(c=>c.projectUid == sessionProject), "projectUid", "project1");
            return View();
        }

        // POST: ContractorNotes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "contractorNoteUid,projectUid,contractorUid,contractorNoteTypeUid,contractorNote1")] ContractorNote contractorNote)
        {
            if (ModelState.IsValid)
            {
                var newNoteStatus = from cc in db.ContractorNoteStatus
                                    where cc.behaviorIndicator == "N"
                                    select cc;

                contractorNote.contractorNoteUid = Guid.NewGuid();
                contractorNote.contractorNoteStatusUid = newNoteStatus.FirstOrDefault().contractorNoteStatusUid;
                db.ContractorNotes.Add(contractorNote);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName");
            ViewBag.contractorNoteTypeUid = new SelectList(db.ContractorNoteTypes, "contractorNoteTypeUid", "contractorNoteType1");
            ViewBag.contractorNoteStatusUid = new SelectList(db.ContractorNoteStatus, "contractorNoteStatusUid", "contractorNoteStatus");
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1");
            return View(contractorNote);
        }

        // GET: ContractorNotes/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorNote contractorNote = await db.ContractorNotes.FindAsync(id);
            if (contractorNote == null)
            {
                return HttpNotFound();
            }
            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName", contractorNote.contractorUid);
            ViewBag.contractorNoteTypeUid = new SelectList(db.ContractorNoteTypes, "contractorNoteTypeUid", "contractorNoteType1", contractorNote.contractorNoteTypeUid);
            ViewBag.contractorNoteStatusUid = new SelectList(db.ContractorNoteStatus, "contractorNoteStatusUid", "contractorNoteStatus", contractorNote.contractorNoteStatusUid);
            ViewBag.projectUid = new SelectList(db.Projects.Where(c => c.projectUid == sessionProject), "projectUid", "project1", contractorNote.projectUid);
            return View(contractorNote);
        }

        // POST: ContractorNotes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "contractorNoteUid,projectUid,contractorUid,contractorNoteTypeUid, contractorNoteStatusUid,contractorNote1")] ContractorNote contractorNote, string submit)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();            

            if (ModelState.IsValid)
            {
                if (submit == "Submit")
                {
                    var submitNoteStatus = from cc in db.ContractorNoteStatus
                                           where cc.behaviorIndicator == "S"
                                           select cc;
                    contractorNote.contractorNoteStatusUid = submitNoteStatus.FirstOrDefault().contractorNoteStatusUid;
                }

                if (submit == "Reopen")
                {
                    var newNoteStatus = from cc in db.ContractorNoteStatus
                                        where cc.behaviorIndicator == "N"
                                        select cc;
                    contractorNote.contractorNoteStatusUid = newNoteStatus.FirstOrDefault().contractorNoteStatusUid;
                }
                if (submit == "Denied")
                {
                    var deniedNoteStatus = from cc in db.ContractorNoteStatus
                                        where cc.behaviorIndicator == "D"
                                        select cc;
                    contractorNote.contractorNoteStatusUid = deniedNoteStatus.FirstOrDefault().contractorNoteStatusUid;
                }

                db.Entry(contractorNote).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName", contractorNote.contractorUid);
            ViewBag.contractorNoteTypeUid = new SelectList(db.ContractorNoteTypes, "contractorNoteTypeUid", "contractorNoteType1", contractorNote.contractorNoteTypeUid);
            ViewBag.contractorNoteStatusUid = new SelectList(db.ContractorNoteStatus, "contractorNoteStatusUid", "contractorNoteStatus", contractorNote.contractorNoteStatusUid);
            ViewBag.projectUid = new SelectList(db.Projects.Where(c => c.projectUid == sessionProject), "projectUid", "project1", contractorNote.projectUid);
            return View(contractorNote);
        }

        // GET: ContractorNotes/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorNote contractorNote = await db.ContractorNotes.FindAsync(id);
            if (contractorNote == null)
            {
                return HttpNotFound();
            }
            return View(contractorNote);
        }

        // POST: ContractorNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ContractorNote contractorNote = await db.ContractorNotes.FindAsync(id);
            db.ContractorNotes.Remove(contractorNote);
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
