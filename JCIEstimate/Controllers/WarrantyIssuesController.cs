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
    public class WarrantyIssuesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: WarrantyIssues
        public async Task<ActionResult> Index()
        {
            var warrantyIssues = db.WarrantyIssues.Include(w => w.WarrantyStatu).Include(w => w.WarrantyUnit);            
            return View(await warrantyIssues.ToListAsync());
        }

        // GET: WarrantyIssues/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WarrantyIssue warrantyIssue = await db.WarrantyIssues.FindAsync(id);
            if (warrantyIssue == null)
            {
                return HttpNotFound();
            }
            return View(warrantyIssue);
        }

        // GET: WarrantyIssues/Create
        public ActionResult Create()
        {
            IQueryable<WarrantyUnit> warrantyUnits ;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            warrantyUnits = from cc in db.WarrantyUnits
                            where cc.Location.projectUid == sessionProject
                            select cc;

            ViewBag.warrantyUnitUid = warrantyUnits.ToSelectList(d => d.Location.location1 + " - " + d.warrantyUnit1, d => d.warrantyUnitUid.ToString(), "");
            ViewBag.warrantyStatusUid = db.WarrantyStatus.ToSelectList(d=>d.warrantyStatus, d=>d.warrantyStatusUid.ToString(), "New" );                    
            return View();
        }

        // POST: WarrantyIssues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "warrantyIssueUid,warrantyUnitUid,warrantyStatusUid,warrantyIssueLocation,warrantyIssue1")] WarrantyIssue warrantyIssue)
        {
            if (ModelState.IsValid)
            {
                warrantyIssue.warrantyIssueUid = Guid.NewGuid();
                db.WarrantyIssues.Add(warrantyIssue);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.warrantyStatusUid = new SelectList(db.WarrantyStatus, "warrantyStatusUid", "warrantyStatus", warrantyIssue.warrantyStatusUid);
            ViewBag.warrantyUnitUid = new SelectList(db.WarrantyUnits, "warrantyUnitUid", "warrantyUnit1", warrantyIssue.warrantyUnitUid);
            return View(warrantyIssue);
        }

        // GET: WarrantyIssues/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WarrantyIssue warrantyIssue = await db.WarrantyIssues.FindAsync(id);
            if (warrantyIssue == null)
            {
                return HttpNotFound();
            }

            IQueryable<WarrantyUnit> warrantyUnits;
            IQueryable<ProjectUser> projectUsers;
            IQueryable<WarrantyNote> warrantyNotes;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            warrantyNotes = from cc in db.WarrantyNotes
                            where cc.warrantyIssueUid == warrantyIssue.warrantyIssueUid
                            select cc;

            warrantyUnits = from cc in db.WarrantyUnits
                            where cc.warrantyUnitUid == warrantyIssue.warrantyUnitUid
                            select cc;

            projectUsers = from cn in db.ProjectUsers
                           join cq in db.AspNetUsers on cn.aspNetUserUid equals cq.Id
                           where cn.projectUid == sessionProject                           
                           select cn;
            
            ViewBag.projectUserUid = projectUsers.ToSelectList(d => d.AspNetUser.Email, d => d.projectUserUid.ToString(), warrantyIssue.projectUserUid.ToString());
            ViewBag.warrantyUnitUid = warrantyUnits.ToSelectList(d => d.Location.location1 + " - " + d.warrantyUnit1, d => d.warrantyUnitUid.ToString(), warrantyIssue.warrantyUnitUid.ToString(), false);            
            ViewBag.warrantyStatusUid = new SelectList(db.WarrantyStatus, "warrantyStatusUid", "warrantyStatus", warrantyIssue.warrantyStatusUid);            
            return View(warrantyIssue);
        }

        // POST: WarrantyIssues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "warrantyUnitUid,warrantyIssueUid,warrantyStatusUid,warrantyIssueLocation,warrantyIssue1,projectUserUid")] WarrantyIssue warrantyIssue)
        {
            if (ModelState.IsValid)
            {
                if (warrantyIssue.projectUserUid.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    warrantyIssue.projectUserUid = null;
                }
                db.Entry(warrantyIssue).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            IQueryable<WarrantyUnit> warrantyUnits;
            IQueryable<ProjectUser> projectUsers;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            warrantyUnits = from cc in db.WarrantyUnits
                            where cc.warrantyUnitUid == warrantyIssue.warrantyUnitUid
                            select cc;

            projectUsers = from cn in db.ProjectUsers
                           join cq in db.AspNetUsers on cn.aspNetUserUid equals cq.Id
                           where cn.projectUid == sessionProject
                           select cn;

            ViewBag.projectUserUid = projectUsers.ToSelectList(d => d.AspNetUser.Email, d => d.projectUserUid.ToString(), warrantyIssue.projectUserUid.ToString());
            ViewBag.warrantyUnitUid = warrantyUnits.ToSelectList(d => d.Location.location1 + " - " + d.warrantyUnit1, d => d.warrantyUnitUid.ToString(), warrantyIssue.warrantyUnitUid.ToString());
            ViewBag.warrantyStatusUid = new SelectList(db.WarrantyStatus, "warrantyStatusUid", "warrantyStatus", warrantyIssue.warrantyStatusUid);
            return View(warrantyIssue);
        }

        // GET: WarrantyIssues/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WarrantyIssue warrantyIssue = await db.WarrantyIssues.FindAsync(id);
            if (warrantyIssue == null)
            {
                return HttpNotFound();
            }
            return View(warrantyIssue);
        }

        // POST: WarrantyIssues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            WarrantyIssue warrantyIssue = await db.WarrantyIssues.FindAsync(id);
            db.WarrantyIssues.Remove(warrantyIssue);
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
