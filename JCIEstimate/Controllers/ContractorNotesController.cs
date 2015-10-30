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
        public async Task<ActionResult> Index(string filterId)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            IQueryable<ContractorNote> contractorNotes;
            List<FilterOptionModel> aryFo = new List<FilterOptionModel>();


            if (filterId == null)
            {
                if (Session["notesFilterId"] != null)
                {
                    filterId = Session["notesFilterId"].ToString();
                }
            }

            aryFo = buildFilterDropDown(filterId, db.ContractorNotes.Where(c => c.projectUid == sessionProject));

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

            ViewBag.filterList = aryFo.ToList();
            contractorNotes = applyFilter(filterId, contractorNotes);
            contractorNotes = contractorNotes.OrderBy(c=>c.Contractor.contractorName).ThenBy(c => c.ContractorNoteType.contractorNoteType1).Include(c => c.Contractor).Include(c => c.ContractorNoteType).Include(c => c.Project);
            return View(await contractorNotes.ToListAsync());
        }

        private List<FilterOptionModel> buildFilterDropDown(string filterId, IQueryable<ContractorNote> contractorNotes)
        {
            //Build Drop down filter based on existing defined equipment
            List<FilterOptionModel> aryFo = new List<FilterOptionModel>();
            string[] filterPart = null;
            string type = "";
            string uid = Guid.Empty.ToString();

            if (!String.IsNullOrEmpty(filterId))
            {
                filterPart = filterId.Split('|');
                type = filterPart[0];
                uid = filterPart[1];
            }

            FilterOptionModel wf = new FilterOptionModel();
            wf.text = "-- Choose --";
            wf.value = "X|" + Guid.Empty.ToString();
            wf.selected = (wf.value == filterId || String.IsNullOrEmpty(filterId));
            aryFo.Add(wf);

            wf = new FilterOptionModel();
            wf.text = "All";
            wf.value = "A|" + Guid.Empty.ToString().Substring(0, 35) + "1";
            wf.selected = (wf.value == filterId);
            aryFo.Add(wf);


            IQueryable<ContractorNote> results = contractorNotes.GroupBy(c => c.contractorUid).Select(v => v.FirstOrDefault());

            foreach (var item in results.OrderBy(c => c.Contractor.contractorName))
            {
                wf = new FilterOptionModel();
                wf.text = item.Contractor.contractorName;
                wf.value = "C|" + item.contractorUid.ToString();
                wf.selected = (wf.value == filterId);
                aryFo.Add(wf);
            }

            results = contractorNotes.GroupBy(c => c.contractorNoteStatusUid).Select(v => v.FirstOrDefault());

            foreach (var item in results.OrderBy(c => c.ContractorNoteStatu.contractorNoteStatus))
            {
                wf = new FilterOptionModel();
                wf.text = item.ContractorNoteStatu.contractorNoteStatus;
                wf.value = "S|" + item.contractorNoteStatusUid.ToString();
                wf.selected = (wf.value == filterId);
                aryFo.Add(wf);
            }
            return aryFo;
        }

        private IQueryable<ContractorNote> applyFilter(string filterId, IQueryable<ContractorNote> contractorNotes)
        {
            //apply filter if there is one
            string[] filterPart = null;
            string type = "";
            string uid = Guid.Empty.ToString();
            if (!String.IsNullOrEmpty(filterId))
            {
                filterPart = filterId.Split('|');
                type = filterPart[0];
                uid = filterPart[1];

                if (type == "C")
                {
                    contractorNotes = contractorNotes.Where(c => c.contractorUid.ToString() == uid);
                }
                else if (type == "S")
                {
                    contractorNotes = contractorNotes.Where(c => c.contractorNoteStatusUid.ToString() == uid);
                }
                
            }
            Session["notesFilterId"] = filterId;

            return contractorNotes;
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
        public async Task<ActionResult> Create([Bind(Include = "contractorNoteUid,projectUid,contractorUid,contractorNoteTypeUid,contractorNote1,denialReason")] ContractorNote contractorNote)
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
        public async Task<ActionResult> Edit([Bind(Include = "contractorNoteUid,projectUid,contractorUid,contractorNoteTypeUid, contractorNoteStatusUid,contractorNote1,denialReason")] ContractorNote contractorNote, string submit)
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
                if (submit == "Approved")
                {
                    var approvedNoteStatus = from cc in db.ContractorNoteStatus
                                        where cc.behaviorIndicator == "A"
                                        select cc;
                    contractorNote.contractorNoteStatusUid = approvedNoteStatus.FirstOrDefault().contractorNoteStatusUid;
                }
                if (submit == "Denied")
                {
                    var deniedNoteStatus = from cc in db.ContractorNoteStatus
                                        where cc.behaviorIndicator == "D"
                                        select cc;
                    contractorNote.contractorNoteStatusUid = deniedNoteStatus.FirstOrDefault().contractorNoteStatusUid;
                    db.Entry(contractorNote).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Edit");
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
