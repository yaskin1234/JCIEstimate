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
        public async Task<ActionResult> Index(string filterId)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            var warrantyIssues = db.WarrantyIssues.Include(w => w.WarrantyStatu);

            warrantyIssues = warrantyIssues.Where(c => c.WarrantyUnit.Location.projectUid == sessionProject).Include(w => w.WarrantyStatu).Include(w => w.WarrantyUnit);

            //Build Drop down filter
            List<FilterOptionModel> aryFo = new List<FilterOptionModel>();

            FilterOptionModel wf = new FilterOptionModel();
            wf.text = "All";
            wf.value = "A";
            wf.selected = (wf.value == filterId || String.IsNullOrEmpty(filterId));
            aryFo.Add(wf);

            wf = new FilterOptionModel();
            wf.text = "My assigned tasks";
            wf.value = "T";
            wf.selected = (wf.value == filterId);
            aryFo.Add(wf);

            foreach (var item in db.WarrantyStatus.OrderBy(c => c.listOrder))
            {
                wf = new FilterOptionModel();
                wf.text = item.warrantyStatus;
                wf.value = item.behaviorIndicator;
                wf.selected = (wf.value == filterId);
                aryFo.Add(wf);
            }

            var results = warrantyIssues.GroupBy(c => c.WarrantyUnit.Location.location1)
            .Select(v => v.FirstOrDefault());            

            foreach (var item in results)
            {
                wf = new FilterOptionModel();
                wf.text = item.WarrantyUnit.Location.location1;
                wf.value = item.WarrantyUnit.locationUid.ToString();
                wf.selected = (wf.value == filterId);
                aryFo.Add(wf);
            }

            
            if (filterId == "A" || String.IsNullOrEmpty(filterId)) //all issues for project
            {
                warrantyIssues = from cc in warrantyIssues
                                 where cc.WarrantyUnit.Location.projectUid == sessionProject
                                 select cc;

            }
            else if (filterId == "T") //my issues for project
            {
                warrantyIssues = from cc in warrantyIssues
                                 join dd in db.ProjectUsers on cc.projectUserUid equals dd.projectUserUid
                                 join pu in db.AspNetUsers on dd.aspNetUserUid equals pu.Id
                                 where pu.Email == HttpContext.User.Identity.Name 
                                 && cc.WarrantyUnit.Location.projectUid == sessionProject
                                 select cc;
            }
            else if (filterId.Length == 36) // received GUID for locationUid
            {
                warrantyIssues = from cc in warrantyIssues
                                 where cc.WarrantyUnit.Location.projectUid == sessionProject
                                 && cc.WarrantyUnit.locationUid.ToString() == filterId
                                 select cc;
            }
            else 
            {
                warrantyIssues = from cc in warrantyIssues // specific status for project
                                 where cc.WarrantyStatu.behaviorIndicator == filterId
                                 && cc.WarrantyUnit.Location.projectUid == sessionProject
                                 select cc;
            }

            ViewBag.filterList = aryFo.ToList();
            warrantyIssues = warrantyIssues.OrderBy(w => w.WarrantyUnit.Location.location1).ThenBy(w => w.WarrantyUnit.warrantyUnit1).ThenBy(w => w.WarrantyStatu.listOrder);
            
            return View(await warrantyIssues.ToListAsync());
        }

        public async Task<ActionResult> GetUnits(Guid locationUid)
        {
            var units = from cc in db.WarrantyUnits
                        where cc.locationUid == locationUid
                        select cc;

            return View(await units.ToListAsync());
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
            IQueryable<Location> locations;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            locations = from cc in db.Locations
                        where cc.projectUid == sessionProject
                        select cc;

            warrantyUnits = from cc in db.WarrantyUnits
                            where cc.Location.projectUid == sessionProject
                            select cc;

            ViewBag.locationUid = locations.ToSelectList(d => d.location1, d => d.locationUid.ToString(), "");
            ViewBag.warrantyUnitUid = warrantyUnits.ToSelectList(d => d.Location.location1 + " - " + d.warrantyUnit1, d => d.warrantyUnitUid.ToString(), "");
            ViewBag.warrantyStatusUid = db.WarrantyStatus.OrderBy(d => d.listOrder).ToSelectList(d => d.warrantyStatus, d => d.warrantyStatusUid.ToString(), "");                    
            return View();
        }

        // POST: WarrantyIssues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "warrantyIssueUid,warrantyUnitUid,warrantyStatusUid,warrantyIssueLocation,warrantyIssue1,date")] WarrantyIssue warrantyIssue)
        {
            if (ModelState.IsValid)
            {
                var user = from cc in db.AspNetUsers
                           where cc.UserName == System.Web.HttpContext.Current.User.Identity.Name
                           select cc;
                
                warrantyIssue.date = DateTime.Now;
                warrantyIssue.aspNetUserUidAsCreated = user.First().Id;
                warrantyIssue.warrantyIssueUid = Guid.NewGuid();
                db.WarrantyIssues.Add(warrantyIssue);
                await db.SaveChangesAsync();

                WarrantyUnit wi = new WarrantyUnit();
                wi = db.WarrantyUnits.Find(warrantyIssue.warrantyUnitUid);
                string subject = "New Warranty Issue Created for " + wi.Location.Project.project1;
                string emailMessage = "";

                emailMessage += "Creator: " + System.Web.HttpContext.Current.User.Identity.Name + "\n";
                emailMessage += "Project: " + wi.Location.Project.project1 + " - " + wi.warrantyUnit1 + "\n";
                emailMessage += "Unit: " + wi.Location.location1 + " - " + wi.warrantyUnit1 + "\n";
                emailMessage += "Room: " + warrantyIssue.warrantyIssueLocation + "\n";
                emailMessage += "Issue: " + warrantyIssue.warrantyIssue1 + "\n";
                JCIExtensions.MCVExtensions.sendEmailToProjectUsers(db, wi.Location.projectUid, subject, emailMessage, false);

                return RedirectToAction("Index");
            }

            ViewBag.warrantyStatusUid = new SelectList(db.WarrantyStatus.OrderBy(d => d.listOrder), "warrantyStatusUid", "warrantyStatus", warrantyIssue.warrantyStatusUid);
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

            
            ViewBag.warrantyNotes = warrantyNotes.Include(c => c.AspNetUser).OrderBy(c => c.date).ToList();
            ViewBag.projectUserUid = projectUsers.ToSelectList(d => d.AspNetUser.Email, d => d.projectUserUid.ToString(), warrantyIssue.projectUserUid.ToString());
            ViewBag.warrantyUnitUid = warrantyUnits.ToSelectList(d => d.Location.location1 + " - " + d.warrantyUnit1, d => d.warrantyUnitUid.ToString(), warrantyIssue.warrantyUnitUid.ToString(), false);            
            ViewBag.warrantyStatusUid = new SelectList(db.WarrantyStatus.OrderBy(d => d.listOrder), "warrantyStatusUid", "warrantyStatus", warrantyIssue.warrantyStatusUid);
            Session["original"] = warrantyIssue;
            return View(warrantyIssue);
        }

        // POST: WarrantyIssues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "warrantyUnitUid,warrantyIssueUid,warrantyStatusUid,warrantyIssueLocation,projectUserUid,date,addComment")] WarrantyIssue warrantyIssue, string addComment)
        {
            if (ModelState.IsValid)
            {
                WarrantyIssue wi = (WarrantyIssue)Session["original"];
                bool statusChange = (wi.warrantyStatusUid != warrantyIssue.warrantyStatusUid);
                bool assignmentChange = false;

                //disabled field needs to be set from "original" object!!
                warrantyIssue.warrantyIssue1 = wi.warrantyIssue1;
                warrantyIssue.date = wi.date;
                warrantyIssue.warrantyIssue1 = wi.warrantyIssue1;
                warrantyIssue.warrantyIssueLocation = wi.warrantyIssueLocation;
                warrantyIssue.warrantyUnitUid = wi.warrantyUnitUid;
                warrantyIssue.aspNetUserUidAsCreated = wi.aspNetUserUidAsCreated;

                if (wi.projectUserUid == null && warrantyIssue.projectUserUid == JCIExtensions.MCVExtensions.pseudoNull)
                {
                    assignmentChange = false;    
                }
                else if (wi.projectUserUid != warrantyIssue.projectUserUid)
                {
                    assignmentChange = true;
                }
                
                bool addNote = (statusChange || addComment != "" || assignmentChange);                
                if (addNote)
                {
                    var user = from cc in db.AspNetUsers
                               where cc.UserName == System.Web.HttpContext.Current.User.Identity.Name
                               select cc;

                    WarrantyNote wn = new WarrantyNote();
                    wn.warrantyNoteUid = Guid.NewGuid();
                    wn.aspNetUserUidAsCreated = user.First().Id;
                    wn.date = DateTime.Now;
                    wn.warrantyIssueUid = warrantyIssue.warrantyIssueUid;                    
                    wn.warrantyNote1 = addComment;
                    if (statusChange)
                    {
                        var warrantyStatus = db.WarrantyStatus.Find(warrantyIssue.warrantyStatusUid);
                        wn.warrantyNote1 += "\n\n Status changed to " + warrantyStatus.warrantyStatus;
                    }
                    if (assignmentChange)
                    {
                        var projectUser = db.ProjectUsers.Find(warrantyIssue.projectUserUid);
                        wn.warrantyNote1 += "\n\n Assignment changed from " + ((wi.ProjectUser != null) ? wi.ProjectUser.AspNetUser.UserName : "nothing") + " to " + ((projectUser != null) ? projectUser.AspNetUser.UserName : "nothing");
                    }
                    db.WarrantyNotes.Add(wn);

                    string emailMessage = "";
                    emailMessage += "Modified by: " + System.Web.HttpContext.Current.User.Identity.Name + "\n";
                    emailMessage += wi.WarrantyUnit.Location.location1 + " - " + wi.WarrantyUnit.warrantyUnit1 + "  has been modified: " + wn.warrantyNote1 + "\n";
                    
                    string subject = "Warranty Issue Modified for project " + wn.WarrantyIssue.WarrantyUnit.Location.Project.project1;
                    JCIExtensions.MCVExtensions.sendEmailToProjectUsers(db, wi.WarrantyUnit.Location.projectUid, subject, emailMessage, false);
                }                

                if (warrantyIssue.projectUserUid.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    warrantyIssue.projectUserUid = null;                    
                }
                db.Entry(warrantyIssue).State = EntityState.Modified;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            WarrantyIssue warrantyIssue = await db.WarrantyIssues.FindAsync(id);
            db.WarrantyIssues.Remove(warrantyIssue);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            
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
