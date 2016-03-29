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
using System.Data.Entity.Validation;
using System.IO;

namespace JCIEstimate.Controllers
{
    public class CommissionIssuesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: CommissionIssues/GetCommissionAttachment/5
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetCommissionAttachment(Guid commissionAttachmentUid, string fileType)
        {
            var d = from cc in db.CommissionAttachments
                    where cc.commissionAttachmentUid == commissionAttachmentUid
                    select cc.document;

            var docName = from cc in db.CommissionAttachments
                          where cc.commissionAttachmentUid == commissionAttachmentUid
                          select cc.documentName;

            byte[] byteArray = d.FirstOrDefault();
            return File(byteArray, "application/octect-stream", docName.FirstOrDefault());
        }

        public async Task<ActionResult> IndexPartial(string filterId, string metasysNumber = null)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            var commissionIssues = db.CommissionIssues.Include(w => w.CommissionStatu);

            //apply session project predicate
            commissionIssues = from cc in commissionIssues
                               where cc.Equipment.Location.projectUid == sessionProject                                 
                               select cc;

            if (filterId == "A" || String.IsNullOrEmpty(filterId)) //all issues for project            
            {
                commissionIssues = from cc in commissionIssues
                                 select cc;

            }
            else if (filterId == "T") //my issues for project
            {
                commissionIssues = from cc in commissionIssues
                                 join dd in db.ProjectUsers on cc.projectUserUid equals dd.projectUserUid
                                 join pu in db.AspNetUsers on dd.aspNetUserUid equals pu.Id
                                 where pu.Email == HttpContext.User.Identity.Name
                                 select cc;
            }
            else if (filterId.Length == 36) // received GUID for locationUid
            {
                commissionIssues = from cc in commissionIssues
                                   where cc.Equipment.locationUid.ToString() == filterId
                                   select cc;
            }
            else
            {
                commissionIssues = from cc in commissionIssues // specific status for project
                                 where cc.CommissionStatu.behaviorIndicator == filterId
                                 select cc;
            }

            if (metasysNumber != null)
            {
                commissionIssues = from cc in commissionIssues
                                   where cc.Equipment.metasysNumber == metasysNumber
                                   select cc;
            }

            commissionIssues = commissionIssues.Include(w => w.Equipment.Location).OrderByDescending(w => w.date);

            return PartialView(await commissionIssues.ToListAsync());
        }

        // GET: CommissionIssues
        public async Task<ActionResult> Index(string filterId, string location)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            var commissionIssues = db.CommissionIssues.Include(w => w.CommissionStatu);


            //apply session project predicate
            commissionIssues = from cc in commissionIssues
                               where cc.Equipment.Location.projectUid == sessionProject
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

            foreach (var item in db.CommissionStatus.OrderBy(c => c.listOrder))
            {
                wf = new FilterOptionModel();
                wf.text = item.commissionStatus;
                wf.value = item.behaviorIndicator;
                wf.selected = (wf.value == filterId);
                aryFo.Add(wf);
            }

            IQueryable<CommissionIssue> results = commissionIssues.GroupBy(c => c.Equipment.Location.location1)
           .Select(v => v.FirstOrDefault());

            foreach (var item in results)
            {
                wf = new FilterOptionModel();
                Location loc = new Location();
                loc = db.Locations.Find(item.Equipment.locationUid);

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


            ViewBag.location = location;
            ViewBag.txtLocationSearch = location;
            ViewBag.filterList = aryFo.ToList();

            return View(await commissionIssues.OrderByDescending(c => c.date).ToListAsync());
        }


        public async Task<ActionResult> GetEquipments(Guid? locationUid)
        {
            var units = from cc in db.Equipments
                        where ( cc.locationUid == locationUid                        
                        || locationUid == null )
                        && cc.metasysNumber != null
                        select cc;

            return Json(
                units.Select(x => new
                {
                    id = x.equipmentUid,
                    name = x.metasysNumber + " - " + x.EquipmentAttributeType.equipmentAttributeType1
                }).OrderBy(c => c.name), JsonRequestBehavior.AllowGet);


        }

        // GET: CommissionIssues/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommissionIssue commissionIssue = await db.CommissionIssues.FindAsync(id);
            if (commissionIssue == null)
            {
                return HttpNotFound();
            }
            return View(commissionIssue);
        }
        // GET: CommissionIssues/Create
        public ActionResult Create()
        {
            IQueryable<Location> locations;
            IQueryable<Equipment> equipments;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            locations = from cc in db.Locations
                        where cc.projectUid == sessionProject
                        select cc;

            equipments = from cc in db.Equipments
                         where cc.Location.projectUid == sessionProject
                         select cc;
                        
            ViewBag.locationUid = locations.OrderBy(c => c.location1).ToSelectList(d => d.location1, d => d.locationUid.ToString(), "");
            ViewBag.equipmentUid = equipments.Where(c => c.metasysNumber != null).OrderBy(c => c.metasysNumber).ToSelectList(d => d.metasysNumber + " - " + d.Location.location1 + d.manufacturer, d => d.equipmentUid.ToString(), "");
            ViewBag.commissionStatusUid = db.CommissionStatus.OrderBy(d => d.listOrder).ToSelectList(d => d.commissionStatus, d => d.commissionStatusUid.ToString(), "");
            return View();
        }

        // POST: CommissionIssues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "commissionIssueUid,equipmentUid,commissionIssue1,date,locationUid")] CommissionIssue commissionIssue)
        {
            IQueryable<Location> locations;
            IQueryable<Equipment> equipments;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            locations = from cc in db.Locations
                        where cc.projectUid == sessionProject
                        select cc;

            equipments = from cc in db.Equipments
                         where 1 == 0
                         select cc;

            ViewBag.locationUid = locations.ToSelectList(d => d.location1, d => d.locationUid.ToString(), "");
            ViewBag.equipmentUid = equipments.Where(c => c.metasysNumber != null).OrderBy(c => c.metasysNumber).ToSelectList(d => d.metasysNumber + " - " + d.manufacturer + " - " + d.model, d => d.equipmentUid.ToString(), "");            
            
            if (ModelState.IsValid)
            {
                var user = from cc in db.AspNetUsers
                           where cc.UserName == System.Web.HttpContext.Current.User.Identity.Name
                           select cc;

                var status = from cc in db.CommissionStatus
                             where cc.behaviorIndicator == "N"
                             select cc;

                commissionIssue.date = DateTime.Now;
                commissionIssue.aspNetUserUidAsCreated = user.First().Id;
                commissionIssue.commissionIssueUid = Guid.NewGuid();
                commissionIssue.commissionStatusUid = status.FirstOrDefault().commissionStatusUid;

                db.CommissionIssues.Add(commissionIssue);
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
                    return View();
                }
                catch (Exception ex)
                {
                    Session["error"] = ex.Message;
                    return View();
                }

                string subject = "";
                string emailMessage = "";
                string textMessage = "";



                if (commissionIssue.equipmentUid != JCIExtensions.MCVExtensions.pseudoNull && commissionIssue.equipmentUid != null)
                {
                    Equipment eq = new Equipment();
                    eq = db.Equipments.Find(commissionIssue.equipmentUid);
                    subject = "New Commission Issue Created for " + eq.Location.Project.project1;

                    textMessage = "New Commission Issue Created for " + eq.Location.Project.project1;

                    string emailPath = Server.MapPath("~/Emails/CommissionIssue.html");
                    emailMessage = System.IO.File.ReadAllText(emailPath);

                    emailMessage = emailMessage.Replace("{{Creator}}", System.Web.HttpContext.Current.User.Identity.Name);
                    emailMessage = emailMessage.Replace("{{Project}}", eq.Location.Project.project1);
                    emailMessage = emailMessage.Replace("{{Location}}", eq.Location.location1);
                    emailMessage = emailMessage.Replace("{{Issue}}", commissionIssue.commissionIssue1);

                }

                JCIExtensions.MCVExtensions.sendEmailToProjectUsers(db, JCIExtensions.MCVExtensions.getSessionProject(), subject, emailMessage, true, true);
                JCIExtensions.MCVExtensions.sendTextToProjectUsers(db, JCIExtensions.MCVExtensions.getSessionProject(), subject, textMessage, false, true);

                return RedirectToAction("Index");
            }
            return View(commissionIssue);
        }

        // GET: CommissionIssues/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommissionIssue commissionIssue = await db.CommissionIssues.FindAsync(id);
            if (commissionIssue == null)
            {
                return HttpNotFound();
            }
            ViewBag.aspNetUserUidAsCreated = new SelectList(db.AspNetUsers, "Id", "AllowableContractors", commissionIssue.aspNetUserUidAsCreated);
            ViewBag.commissionStatusUid = new SelectList(db.CommissionStatus.OrderBy(c=>c.listOrder), "commissionStatusUid", "commissionStatus", commissionIssue.commissionStatusUid);
            ViewBag.equipmentUid = new SelectList(db.Equipments.Where(c => c.metasysNumber != null).OrderBy(c => c.metasysNumber), "equipmentUid", "ownerTag", commissionIssue.equipmentUid);
            ViewBag.projectUserUid = db.ProjectUsers.Where(c=>c.projectUid == sessionProject).OrderBy(c=>c.AspNetUser.Email).ToSelectList(c=>c.AspNetUser.Email, c=>c.projectUserUid.ToString(), commissionIssue.projectUserUid.ToString());
            Session["original"] = commissionIssue;
            return View(commissionIssue);
        }

        // POST: CommissionIssues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "commissionIssueUid,equipmentUid,commissionStatusUid,projectUserUid,commissionIssue1,aspNetUserUidAsCreated,date")] CommissionIssue commissionIssue, string addComment, HttpPostedFileBase postedFile, string commissionAttachment1)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            if (ModelState.IsValid)
            {
                CommissionIssue wi = (CommissionIssue)Session["original"];
                //disabled field needs to be set from "original" object!!                
                commissionIssue.commissionIssue1 = wi.commissionIssue1;
                commissionIssue.date = wi.date;
                commissionIssue.commissionIssue1 = wi.commissionIssue1;
                commissionIssue.aspNetUserUidAsCreated = wi.aspNetUserUidAsCreated;
                commissionIssue.equipmentUid = wi.equipmentUid;

                bool statusChange = (wi.commissionStatusUid != commissionIssue.commissionStatusUid);
                bool assignmentChange = false;


                if (wi.projectUserUid == null && commissionIssue.projectUserUid == JCIExtensions.MCVExtensions.pseudoNull)
                {
                    assignmentChange = false;
                }
                else if (wi.projectUserUid != commissionIssue.projectUserUid)
                {
                    assignmentChange = true;
                }

                bool addNote = (statusChange || addComment != "" || assignmentChange);
                if (addNote)
                {
                    var user = from cc in db.AspNetUsers
                               where cc.UserName == System.Web.HttpContext.Current.User.Identity.Name
                               select cc;

                    CommissionNote wn = new CommissionNote();
                    wn.commissionNoteUid = Guid.NewGuid();
                    wn.aspNetUserUidAsCreated = user.First().Id;
                    wn.date = DateTime.Now;
                    wn.commissionIssueUid = commissionIssue.commissionIssueUid;
                    wn.commissionNote1 = "";
                    if (statusChange) //not nullable
                    {
                        var commissionStatus = db.CommissionStatus.Find(commissionIssue.commissionStatusUid);
                        wn.commissionNote1 += "Status changed from " + wi.CommissionStatu.commissionStatus + " to " + commissionStatus.commissionStatus;
                        wn.commissionNote1 += Environment.NewLine;
                    }
                    if (assignmentChange) // nullable
                    {
                        var projectUser = db.ProjectUsers.Find(commissionIssue.projectUserUid);

                        var lastUser = from cc in db.CommissionIssues
                                       where cc.commissionIssueUid == commissionIssue.commissionIssueUid
                                       select cc.AspNetUser;

                        wn.commissionNote1 += Environment.NewLine + "Assignment changed from " + ((lastUser.FirstOrDefault() != null) ? lastUser.FirstOrDefault().UserName : "nothing") + " to " + ((projectUser != null) ? projectUser.AspNetUser.UserName : "nothing");
                        wn.commissionNote1 += Environment.NewLine;
                    }
                    if (!String.IsNullOrEmpty(addComment))
                    {
                        wn.commissionNote1 += Environment.NewLine + addComment;
                        wn.commissionNote1 += Environment.NewLine;
                    }

                    string emailMessage = "";
                    string subject = "Commission Issue Modified for project " + Session["projectName"].ToString();
                    string textMessage = "Commission Issue Modified for project " + Session["projectName"].ToString();

                    var eq = db.Equipments.Find(commissionIssue.equipmentUid);
                    emailMessage += "Modified by:\t" + System.Web.HttpContext.Current.User.Identity.Name + Environment.NewLine;
                    emailMessage += "Equipment:\t" + eq.metasysNumber + " - " + eq.manufacturer + " - " + eq.model + " - " + eq.serialNumber + Environment.NewLine;
                    emailMessage += Environment.NewLine + wn.commissionNote1;

                    emailMessage += "---------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
                    emailMessage += "--------------------------------------------------------------------Historical Notes------------------------------------------------------------------------" + Environment.NewLine;
                    //historical comments added to email
                    foreach (var item in db.CommissionNotes.Where(c => c.commissionIssueUid == commissionIssue.commissionIssueUid))
                    {
                        emailMessage += "By:\t" + item.AspNetUser.Email + Environment.NewLine;
                        emailMessage += "Date:\t" + item.date.ToString("yyyy-MM-dd HH:mm:ss") + Environment.NewLine;
                        emailMessage += Environment.NewLine + item.commissionNote1 + Environment.NewLine;
                    }

                    db.CommissionNotes.Add(wn);
                    JCIExtensions.MCVExtensions.sendEmailToProjectUsers(db, sessionProject, subject, emailMessage, false, true);
                    JCIExtensions.MCVExtensions.sendTextToProjectUsers(db, sessionProject, subject, textMessage, false, true);
                }

                if (postedFile != null)
                {
                    CommissionAttachment wa = new CommissionAttachment();
                    int fileSize = postedFile.ContentLength;
                    var docName = postedFile.FileName;
                    MemoryStream target = new MemoryStream();
                    postedFile.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();
                    wa.document = data;

                    wa.commissionAttachmentUid = Guid.NewGuid();
                    wa.fileType = Path.GetExtension(postedFile.FileName);
                    wa.commissionIssueUid = commissionIssue.commissionIssueUid;
                    wa.documentName = docName;
                    if (!String.IsNullOrEmpty(commissionAttachment1))
                    {
                        wa.commissionAttachment1 = commissionAttachment1 + "; created on " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "; by " + System.Web.HttpContext.Current.User.Identity.Name;
                    }
                    else
                    {
                        wa.commissionAttachment1 = "attachment created on " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    db.CommissionAttachments.Add(wa);
                }

                if (commissionIssue.projectUserUid.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    commissionIssue.projectUserUid = null;
                }
                db.Entry(commissionIssue).State = EntityState.Modified;
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
                    return View(commissionIssue);
                }
                catch (Exception ex)
                {
                    Session["error"] = ex.Message;
                    return View(commissionIssue);
                }
                return RedirectToAction("Index");
            }
            return View(commissionIssue);
        }

        // GET: CommissionIssues/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommissionIssue commissionIssue = await db.CommissionIssues.FindAsync(id);
            if (commissionIssue == null)
            {
                return HttpNotFound();
            }
            return View(commissionIssue);
        }

        // POST: CommissionIssues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            CommissionIssue commissionIssue = await db.CommissionIssues.FindAsync(id);
            db.CommissionIssues.Remove(commissionIssue);
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
