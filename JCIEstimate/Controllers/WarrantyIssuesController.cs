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
using System.IO;
using System.Data.Entity.Validation;

namespace JCIEstimate.Controllers
{
    public class WarrantyIssuesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();


        // GET: WarrantyIssues/GetWarrantyAttachment/5
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetWarrantyAttachment(Guid warrantyAttachmentUid, string fileType)
        {
            var d = from cc in db.WarrantyAttachments
                    where cc.warrantyAttachmentUid == warrantyAttachmentUid
                    select cc.document;

            var docName = from cc in db.WarrantyAttachments
                          where cc.warrantyAttachmentUid == warrantyAttachmentUid
                          select cc.documentName;

            byte[] byteArray = d.FirstOrDefault();
            return File(byteArray, "application/octect-stream", docName.FirstOrDefault());
        }

        public async Task<ActionResult> IndexPartial(string filterId, string location)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            var warrantyIssues = db.WarrantyIssues.Include(w => w.WarrantyStatu);

            //apply session project predicate
            warrantyIssues = from cc in warrantyIssues
                             where cc.WarrantyUnit.Location.projectUid == sessionProject ||
                                 cc.Location.projectUid == sessionProject
                             select cc;

            if (filterId == "A" || String.IsNullOrEmpty(filterId)) //all issues for project            
            {
                warrantyIssues = from cc in warrantyIssues
                                 select cc;

            }
            else if (filterId == "T") //my issues for project
            {
                warrantyIssues = from cc in warrantyIssues
                                 join dd in db.ProjectUsers on cc.projectUserUid equals dd.projectUserUid
                                 join pu in db.AspNetUsers on dd.aspNetUserUid equals pu.Id
                                 where pu.Email == HttpContext.User.Identity.Name
                                 select cc;
            }
            else if (filterId.Length == 36) // received GUID for locationUid
            {
                warrantyIssues = from cc in warrantyIssues
                                 where cc.WarrantyUnit.locationUid.ToString() == filterId ||
                                 cc.locationUid.ToString() == filterId
                                 select cc;
            }
            else
            {
                warrantyIssues = from cc in warrantyIssues // specific status for project
                                 where cc.WarrantyStatu.behaviorIndicator == filterId
                                 select cc;
            }

            if (location != null && location.Length > 0)
            {
                warrantyIssues = from cc in warrantyIssues
                                 where cc.Location.location1.Contains(location)
                                 select cc;

            }

            warrantyIssues = warrantyIssues.Include(w => w.Location).OrderByDescending(w => w.date);

            return PartialView(await warrantyIssues.ToListAsync());
        }

        // GET: WarrantyIssues
        public async Task<ActionResult> Index(string filterId, string location)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            var warrantyIssues = db.WarrantyIssues.Include(w => w.WarrantyStatu);


            //apply session project predicate
            warrantyIssues = from cc in warrantyIssues
                             where cc.WarrantyUnit.Location.projectUid == sessionProject ||
                                 cc.Location.projectUid == sessionProject
                             select cc;

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

            IQueryable<WarrantyIssue> results = warrantyIssues.GroupBy(c => (c.Location == null) ? c.WarrantyUnit.Location.location1 : c.Location.location1)
           .Select(v => v.FirstOrDefault());

            foreach (var item in results)
            {
                wf = new FilterOptionModel();
                WarrantyUnit wu = new WarrantyUnit();
                wu = db.WarrantyUnits.Find(item.warrantyUnitUid);
                Location loc = new Location();
                loc = db.Locations.Find(item.locationUid);

                if (wu != null)
                {
                    if (!aryFo.Exists(c => c.text == wu.Location.location1))
                    {
                        wf.text = wu.Location.location1;
                        wf.value = wu.Location.locationUid.ToString();
                        wf.selected = (wf.value == filterId);
                        aryFo.Add(wf);
                    }

                }


                if (loc != null)
                {
                    if (!aryFo.Exists(c => c.text == loc.location1))
                    {
                        wf.text = loc.location1;
                        wf.value = loc.locationUid.ToString();
                        wf.selected = (wf.value == filterId);
                        aryFo.Add(wf);
                    }
                }
            }

            results = warrantyIssues.Where(c => c.WarrantyUnit != null).GroupBy(c => c.WarrantyUnit.warrantyUnit1)
            .Select(v => v.FirstOrDefault());


            foreach (var item in results)
            {
                bool isFound = false;
                wf = new FilterOptionModel();
                WarrantyUnit wu = new WarrantyUnit();
                wu = db.WarrantyUnits.Find(item.warrantyUnitUid);

                if (wu != null)
                {
                    foreach (var m in aryFo)
                    {
                        if (m.value == item.warrantyUnitUid.ToString())
                        {
                            isFound = true;
                        }
                    }
                    if (isFound)
                        if (!aryFo.Exists(c => c.text == wu.Location.location1))
                        {
                            wf.text = wu.Location.location1;
                            wf.value = wu.Location.locationUid.ToString();
                            wf.selected = (wf.value == filterId);
                            aryFo.Add(wf);
                        }

                }
            }
            

            results = warrantyIssues.Where(c => c.WarrantyUnit != null).GroupBy(c => c.WarrantyUnit.warrantyUnit1).Select(v => v.FirstOrDefault());

            
            foreach (var item in results)
            {
                bool isFound = false;
                wf = new FilterOptionModel();
                WarrantyUnit wu = new WarrantyUnit();
                wu = db.WarrantyUnits.Find(item.warrantyUnitUid);

                if (wu != null)
                {
                    foreach (var m in aryFo)
                    {
                        if (m.value == item.warrantyUnitUid.ToString())
                        {
                            isFound = true;
                        }
                    }
                    if(isFound)
                    if (!aryFo.Exists(c => c.text == wu.Location.location1))
                    {
                        wf.text = wu.Location.location1;
                        wf.value = wu.Location.locationUid.ToString();
                        wf.selected = (wf.value == filterId);
                        aryFo.Add(wf);
                    }

                }
            }
            
          
            ViewBag.location = location;
            ViewBag.txtLocationSearch = location;
            ViewBag.filterList = aryFo.ToList();

            return View(await warrantyIssues.OrderByDescending(c => c.date).ToListAsync());
        }

        public async Task<ActionResult> GetUnits(Guid? locationUid)
        {
            var units = from cc in db.WarrantyUnits
                        where cc.locationUid == locationUid
                        || locationUid == null
                        select cc;

            return Json(
                units.Select(x => new
                {
                id = x.warrantyUnitUid,
                name = x.warrantyUnit1
            }).OrderBy(c => c.name), JsonRequestBehavior.AllowGet);

            
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
                            where true == false
                            select cc;
                        
            ViewBag.locationUid = locations.OrderBy(c=>c.location1).ToSelectList(d => d.location1, d => d.locationUid.ToString(), "");
            ViewBag.warrantyUnitUid = warrantyUnits.OrderBy(c=>c.warrantyUnit1).ToSelectList(d => d.warrantyUnit1, d => d.warrantyUnitUid.ToString(), "");
            ViewBag.warrantyStatusUid = db.WarrantyStatus.OrderBy(d => d.listOrder).ToSelectList(d => d.warrantyStatus, d => d.warrantyStatusUid.ToString(), "");                    
            return View();
        }

        // POST: WarrantyIssues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "warrantyIssueUid,warrantyUnitUid,warrantyStatusUid,warrantyIssueLocation,warrantyIssue1,date,locationUid")] WarrantyIssue warrantyIssue)
        {
            IQueryable<WarrantyUnit> warrantyUnits;
            IQueryable<Location> locations;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            locations = from cc in db.Locations
                        where cc.projectUid == sessionProject
                        select cc;

            warrantyUnits = from cc in db.WarrantyUnits
                            where true == false
                            select cc;

            ViewBag.locationUid = locations.ToSelectList(d => d.location1, d => d.locationUid.ToString(), "");
            ViewBag.warrantyStatusUid = new SelectList(db.WarrantyStatus.OrderBy(d => d.listOrder), "warrantyStatusUid", "warrantyStatus", warrantyIssue.warrantyStatusUid);
            ViewBag.warrantyUnitUid = new SelectList(db.WarrantyUnits, "warrantyUnitUid", "warrantyUnit1", warrantyIssue.warrantyUnitUid);
            
            if (ModelState.IsValid)
            {
                var user = from cc in db.AspNetUsers
                           where cc.UserName == System.Web.HttpContext.Current.User.Identity.Name
                           select cc;
                
                warrantyIssue.date = DateTime.Now;
                warrantyIssue.aspNetUserUidAsCreated = user.First().Id;
                warrantyIssue.warrantyIssueUid = Guid.NewGuid();
                if (warrantyIssue.warrantyUnitUid == JCIExtensions.MCVExtensions.pseudoNull)
                {
                    warrantyIssue.warrantyUnitUid = null;
                }

                db.WarrantyIssues.Add(warrantyIssue);
                try
                {                    
                    await db.SaveChangesAsync();
                }
                catch (DbEntityValidationException ex)
                {
                    string error = "";
                    foreach (var item in ex.EntityValidationErrors)
	                {
		                if(!item.IsValid)
                        {
                            foreach (var item1 in item.ValidationErrors)
	                        {
		                        error += "<li>" + item1.ErrorMessage + "</li>";
	                        }
                        }
	                }
                    Session["error"] = error;
                    return View();
                }
                catch (Exception ex)
                {
                    Session["error"] = ex.Message;
                    return View();
                }

                string subject = "";
                string emailMessage = "";

                if (warrantyIssue.locationUid != JCIExtensions.MCVExtensions.pseudoNull && warrantyIssue.locationUid != null)
                {
                    Location loc = new Location();
                    loc = db.Locations.Find(warrantyIssue.locationUid);
                    subject = "New Warranty Issue Created for " + loc.Project.project1;
                    

                    emailMessage += "Creator:\t" + System.Web.HttpContext.Current.User.Identity.Name + Environment.NewLine;
                    emailMessage += "Project:\t" + loc.Project.project1 + Environment.NewLine;
                    emailMessage += "Location:\t\t" + loc.location1 + Environment.NewLine;
                    emailMessage += "Room:\t\t" + warrantyIssue.warrantyIssueLocation + Environment.NewLine;
                    emailMessage += "Issue:\t\t" + warrantyIssue.warrantyIssue1 + Environment.NewLine;
                }
                else
                {
                    WarrantyUnit wi = new WarrantyUnit();
                    wi = db.WarrantyUnits.Find(warrantyIssue.warrantyUnitUid);
                    subject = "New Warranty Issue Created for " + wi.Location.Project.project1;                    

                    emailMessage += "Creator:\t" + System.Web.HttpContext.Current.User.Identity.Name + Environment.NewLine;
                    emailMessage += "Project:\t" + wi.Location.Project.project1 + " - " + wi.warrantyUnit1 + Environment.NewLine;
                    emailMessage += "Unit:\t\t" + wi.Location.location1 + " - " + wi.warrantyUnit1 + Environment.NewLine;
                    emailMessage += "Room:\t\t" + warrantyIssue.warrantyIssueLocation + Environment.NewLine;
                    emailMessage += "Issue:\t\t" + warrantyIssue.warrantyIssue1 + Environment.NewLine;
                }

                JCIExtensions.MCVExtensions.sendEmailToProjectUsers(db, JCIExtensions.MCVExtensions.getSessionProject(), subject, emailMessage, false);

                return RedirectToAction("Index");
            }
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

            IQueryable<string> location;
            IQueryable<string> warrantyUnit;
            IQueryable<ProjectUser> projectUsers;
            IQueryable<WarrantyNote> warrantyNotes;
            IQueryable<WarrantyAttachment> warrantyAttachments;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            WarrantyUnit wu = new WarrantyUnit();
            wu = db.WarrantyUnits.Find(warrantyIssue.warrantyUnitUid);

            location = from cc in db.Locations
                        where cc.locationUid == warrantyIssue.locationUid
                        select cc.location1;

            warrantyAttachments = from cc in db.WarrantyAttachments
                            where cc.warrantyIssueUid == warrantyIssue.warrantyIssueUid
                            select cc;

            warrantyNotes = from cc in db.WarrantyNotes
                            where cc.warrantyIssueUid == warrantyIssue.warrantyIssueUid
                            select cc;

            warrantyUnit = from cc in db.WarrantyUnits
                            where cc.warrantyUnitUid == warrantyIssue.warrantyUnitUid
                            select cc.warrantyUnit1;

            projectUsers = from cn in db.ProjectUsers
                           join cq in db.AspNetUsers on cn.aspNetUserUid equals cq.Id
                           where cn.projectUid == sessionProject                           
                           select cn;
            
            ViewBag.location = location.First().ToString();
            ViewBag.warrantyUnit = (warrantyUnit.Count() > 0 ? warrantyUnit.First().ToString() : "");
            ViewBag.warrantyAttachments = warrantyAttachments.OrderBy(c=>c.warrantyAttachment1).ToList();
            ViewBag.warrantyNotes = warrantyNotes.Include(c => c.AspNetUser).OrderBy(c => c.date).ToList();
            ViewBag.projectUserUid = projectUsers.ToSelectList(d => d.AspNetUser.Email, d => d.projectUserUid.ToString(), warrantyIssue.projectUserUid.ToString());            
            ViewBag.warrantyStatusUid = new SelectList(db.WarrantyStatus.OrderBy(d => d.listOrder), "warrantyStatusUid", "warrantyStatus", warrantyIssue.warrantyStatusUid);
            Session["original"] = warrantyIssue;
            return View(warrantyIssue);
        }

        // POST: WarrantyIssues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "warrantyUnitUid,warrantyIssueUid,warrantyStatusUid,warrantyIssueLocation,projectUserUid,date,addComment,postedFile, locationUid")] WarrantyIssue warrantyIssue, string addComment, HttpPostedFileBase postedFile, string warrantyAttachment1)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();           
             
            if (ModelState.IsValid)
            {
                WarrantyIssue wi = (WarrantyIssue)Session["original"];                
                //disabled field needs to be set from "original" object!!                
                warrantyIssue.warrantyIssue1 = wi.warrantyIssue1;
                warrantyIssue.date = wi.date;
                warrantyIssue.warrantyIssue1 = wi.warrantyIssue1;
                warrantyIssue.warrantyIssueLocation = wi.warrantyIssueLocation;
                warrantyIssue.warrantyUnitUid = wi.warrantyUnitUid;
                warrantyIssue.aspNetUserUidAsCreated = wi.aspNetUserUidAsCreated;
                warrantyIssue.locationUid = wi.locationUid;

                bool statusChange = (wi.warrantyStatusUid != warrantyIssue.warrantyStatusUid);
                bool assignmentChange = false;


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
                    wn.warrantyNote1 = "";
                    if (statusChange) //not nullable
                    {
                        var warrantyStatus = db.WarrantyStatus.Find(warrantyIssue.warrantyStatusUid);
                        wn.warrantyNote1 += "Status changed from " + wi.WarrantyStatu.warrantyStatus + " to " + warrantyStatus.warrantyStatus;
                        wn.warrantyNote1 += Environment.NewLine;
                    }
                    if (assignmentChange) // nullable
                    {
                        var projectUser = db.ProjectUsers.Find(warrantyIssue.projectUserUid);
                        wn.warrantyNote1 += "Assignment changed from " + ((wi.ProjectUser != null) ? wi.ProjectUser.AspNetUser.UserName : "nothing") + " to " + ((projectUser != null) ? projectUser.AspNetUser.UserName : "nothing");
                        wn.warrantyNote1 += Environment.NewLine;
                    }
                    if (!String.IsNullOrEmpty(addComment))
                    {
                        wn.warrantyNote1 += addComment;
                        wn.warrantyNote1 += Environment.NewLine;
                    }

                    string emailMessage = "";
                    string subject = "Warranty Issue Modified for project " + Session["projectName"].ToString();
                    
                    if (warrantyIssue.warrantyUnitUid != null)
                    {
                        var loc = db.Locations.Find(warrantyIssue.locationUid);
                        var wu1 = db.WarrantyUnits.Find(warrantyIssue.warrantyUnitUid);
                        emailMessage += "Modified by:\t" + System.Web.HttpContext.Current.User.Identity.Name + Environment.NewLine;
                        emailMessage += "Unit:\t\t" + loc.location1 + " - " + wu1.warrantyUnit1 + Environment.NewLine;
                        emailMessage += wn.warrantyNote1;

                    }
                    else
                    {
                        var loc = db.Locations.Find(warrantyIssue.locationUid);
                        emailMessage += "Modified by:\t" + System.Web.HttpContext.Current.User.Identity.Name + Environment.NewLine;
                        emailMessage += "Location:\t" + loc.location1 + Environment.NewLine;
                        emailMessage += wn.warrantyNote1;
                    }

                    emailMessage += "---------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
                    emailMessage += "--------------------------------------------------------------------Historical Notes------------------------------------------------------------------------" + Environment.NewLine;
                    //historical comments added to email
                    foreach (var item in db.WarrantyNotes.Where(c => c.warrantyIssueUid == warrantyIssue.warrantyIssueUid))
                    {                        
                        emailMessage += "By:\t" + item.AspNetUser.Email + Environment.NewLine;
                        emailMessage += "Date:\t" + item.date.ToString("yyyy-MM-dd HH:mm:ss") + Environment.NewLine;
                        emailMessage += item.warrantyNote1 + Environment.NewLine;
                    }

                    db.WarrantyNotes.Add(wn);
                    JCIExtensions.MCVExtensions.sendEmailToProjectUsers(db, sessionProject, subject, emailMessage, false);
                }

                if (postedFile != null)
                {
                    WarrantyAttachment wa = new WarrantyAttachment();                    
                    int fileSize = postedFile.ContentLength;
                    var docName = postedFile.FileName;
                    MemoryStream target = new MemoryStream();
                    postedFile.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();
                    wa.document = data;

                    wa.warrantyAttachmentUid = Guid.NewGuid();
                    wa.fileType = Path.GetExtension(postedFile.FileName);
                    wa.warrantyIssueUid = warrantyIssue.warrantyIssueUid;
                    wa.documentName = docName;
                    if (!String.IsNullOrEmpty(warrantyAttachment1))
                    {
                        wa.warrantyAttachment1 = warrantyAttachment1 + "; created on " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "; by " + System.Web.HttpContext.Current.User.Identity.Name;                
                    }
                    else
                    {
                        wa.warrantyAttachment1 = "attachment created on " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    db.WarrantyAttachments.Add(wa);
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
                catch (DbEntityValidationException ex)
                {
                    string error = "";
                    foreach (var item in ex.EntityValidationErrors)
                    {
                        if (!item.IsValid)
                        {
                            foreach (var item1 in item.ValidationErrors)
                            {
                                error += "<li>" + item1.ErrorMessage + "</li>";
                            }
                        }
                    }
                    Session["error"] = error;
                    return View(warrantyIssue);
                }
                catch (Exception ex)
                {
                    Session["error"] = ex.Message;
                    return View(warrantyIssue);
                }
                return RedirectToAction("Index");
            }

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
