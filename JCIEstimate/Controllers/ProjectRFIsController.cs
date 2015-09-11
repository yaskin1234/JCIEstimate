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
using System.Xml;

namespace JCIEstimate.Controllers
{
    public class ProjectRFIsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ProjectRFIs
        public async Task<ActionResult> Index()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            var projectRFIs = db.ProjectRFIs.Include(p => p.AspNetUser).Include(p => p.AspNetUser1).Include(p => p.Contractor).Include(p => p.ECM).Include(p => p.Project).Include(p => p.RfiType).Where(c=>c.projectUid == sessionProject).Include(c=>c.RfiStatu).OrderBy(c=>c.projectRFIID);
            
            return View(await projectRFIs.ToListAsync());
        }

        // GET: ProjectRFIs/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectRFI projectRFI = await db.ProjectRFIs.FindAsync(id);
            if (projectRFI == null)
            {
                return HttpNotFound();
            }
            return View(projectRFI);
        }

        // GET: ProjectRFIs/Create
        public ActionResult Create()
        {
            IQueryable<Contractor> contractors = null;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            var ecms = from cc in db.ECMs
                       where cc.projectUid == sessionProject
                       orderby cc.ecmNumber
                       select cc;

            var projects = from cc in db.Projects
                           where cc.projectUid == sessionProject
                           select cc;

            if (User.IsInRole("Admin"))
            {
                contractors = from cc in db.Contractors
                              select cc;
            }
            else if (User.IsInRole("Contractor"))
            {
                contractors = from cc in db.Contractors
                              join cn in db.ContractorUsers on cc.contractorUid equals cn.contractorUid
                              join cq in db.AspNetUsers on cn.aspNetUserUid equals cq.Id
                              where cq.UserName == System.Web.HttpContext.Current.User.Identity.Name
                              orderby cc.contractorName
                              select cc;
            }

            var aspNetUserUidAsAssigned = from cc in db.AspNetUsers
                                          join cn in db.ProjectUsers on cc.Id equals cn.aspNetUserUid              
                                          where cn.projectUid == sessionProject
                                          select cc;

            ViewBag.aspNetUserUidAsAssigned = aspNetUserUidAsAssigned.OrderBy(c => c.Email).ToSelectList(c => c.Email, c => c.Id.ToString(), "");
            ViewBag.contractorUid = contractors.OrderBy(c=>c.contractorName).ToSelectList(c => c.contractorName, c => c.contractorUid.ToString(), "");
            ViewBag.ecmUid = ecms.ToSelectList(d => d.ecmNumber + " - " + d.ecmDescription, d => d.ecmUid.ToString(), "");
            ViewBag.projectUid = new SelectList(projects, "projectUid", "project1");
            ViewBag.rfiTypeUid = db.RfiTypes.ToSelectList(c => c.rfiType1, c => c.rfiTypeUid.ToString(), "");
            ViewBag.rfiStatusUid = db.RfiStatus.OrderBy(c=>c.listOrder).ToSelectList(c => c.rfiStatus, c => c.rfiStatusUid.ToString(), "");            
            return View();
        }

        // POST: ProjectRFIs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "projectRFIUid,projectUid,dateCreated,contractorUid,ecmUid,rfiTypeUid,aspNetUserUidAsCreated,aspNetUserUidAsAssigned,projectRFI1, rfiStatusUid")] ProjectRFI projectRFI, string rfiStatusUid)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            if (ModelState.IsValid)
            {
                var userCreated = from cc in db.AspNetUsers
                                  where cc.UserName == System.Web.HttpContext.Current.User.Identity.Name
                                  select cc.Id;
                var projectPM = from cc in db.Projects
                                 where cc.projectUid == sessionProject
                                 select cc.aspNetUserUidAsPM;
                
                projectRFI.projectRFIUid = Guid.NewGuid();
                projectRFI.projectUid = sessionProject;
                projectRFI.dateCreated = DateTime.Now;
                projectRFI.aspNetUserUidAsCreated = userCreated.First();
                projectRFI.aspNetUserUidAsAssigned = projectPM.First();
                db.ProjectRFIs.Add(projectRFI);
                await db.SaveChangesAsync();

                // if anything changed, send an email to every project user associated with this project
                SendMailRFI(projectRFI.projectRFIUid, "", "Created");
                return RedirectToAction("Index");
            }

            ViewBag.aspNetUserUidAsAssigned = new SelectList(db.AspNetUsers, "Id", "AllowableContractors", projectRFI.aspNetUserUidAsAssigned);
            ViewBag.aspNetUserUidAsCreated = new SelectList(db.AspNetUsers, "Id", "AllowableContractors", projectRFI.aspNetUserUidAsCreated);
            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName", projectRFI.contractorUid);
            ViewBag.ecmUid = new SelectList(db.ECMs, "ecmUid", "ecmNumber", projectRFI.ecmUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", projectRFI.projectUid);
            ViewBag.rfiTypeUid = new SelectList(db.RfiTypes, "rfiTypeUid", "rfiType1", projectRFI.rfiTypeUid);
            ViewBag.rfiStatusUid = new SelectList(db.RfiStatus, "rfiStatusUid", "rfiStatus1", projectRFI.rfiStatusUid);
            return View(projectRFI);
        }

        // GET: ProjectRFIs/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {

            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); 
            }
            ProjectRFI projectRFI = await db.ProjectRFIs.FindAsync(id);
            if (projectRFI == null)
            {
                return HttpNotFound();
            }

            var ecms = from cc in db.ECMs
                       where cc.projectUid == sessionProject
                       orderby cc.ecmString
                       select cc;

            var projects = from cc in db.Projects
                           where cc.projectUid == sessionProject
                           select cc;
            
            var contractors = from cc in db.Contractors
                              where db.Estimates.Any(c => c.contractorUid == cc.contractorUid)
                              select cc;


            var aspNetUserUidAsAssigned = from cc in db.AspNetUsers
                                          join cn in db.ProjectUsers on cc.Id equals cn.aspNetUserUid
                                          where cn.projectUid == sessionProject
                                          select cc;

            ViewBag.isPM = (projectRFI.Project.AspNetUser.UserName == System.Web.HttpContext.Current.User.Identity.Name) ? true : false;
            ViewBag.aspNetUserUidAsAssigned = aspNetUserUidAsAssigned.OrderBy(c => c.Email).ToSelectList(c => c.Email, c => c.Id.ToString(), projectRFI.aspNetUserUidAsAssigned.ToString());
            ViewBag.aspNetUserUidAsCreated = new SelectList(db.AspNetUsers, "Id", "Email", projectRFI.aspNetUserUidAsCreated);
            ViewBag.contractorUid = contractors.OrderBy(c => c.contractorName).ToSelectList(c => c.contractorName, c => c.contractorUid.ToString(), projectRFI.contractorUid.ToString());
            ViewBag.ecmUid = ecms.ToSelectList(d => d.ecmNumber + " - " + d.ecmDescription, d => d.ecmUid.ToString(), projectRFI.ecmUid.ToString());
            ViewBag.projectUid = new SelectList(projects, "projectUid", "project1", projectRFI.projectUid);
            ViewBag.rfiTypeUid = new SelectList(db.RfiTypes, "rfiTypeUid", "rfiType1", projectRFI.rfiTypeUid);
            ViewBag.rfiStatusUid = db.RfiStatus.ToSelectList(c=>c.rfiStatus, c=>c.rfiStatusUid.ToString(), projectRFI.rfiStatusUid.ToString());
            return View(projectRFI);
        }

        // POST: ProjectRFIs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "projectRFIUid,projectUid,dateCreated,contractorUid,ecmUid,rfiTypeUid,rfiStatusUid,aspNetUserUidAsCreated,aspNetUserUidAsAssigned,projectRFI1")] ProjectRFI projectRFI, string projectRfiResponse, string existingRFIStatusUid, string existingAssignedTo)
        {
            bool sendEmail = false;            
            if (ModelState.IsValid)
            {
                db.Entry(projectRFI).State = EntityState.Modified;                
                if (!String.IsNullOrEmpty(projectRfiResponse))
                {
                    ProjectRFIResponse pfr = new ProjectRFIResponse();
                    pfr.projectRFIResponseUid = Guid.NewGuid();                    
                    var userModified = from cc in db.AspNetUsers
                                  where cc.UserName == System.Web.HttpContext.Current.User.Identity.Name
                                  select cc.Id;
                    pfr.aspNetUserUidAsResponded = userModified.First();
                    pfr.projectRFIUid = projectRFI.projectRFIUid;
                    pfr.dateCreated = DateTime.Now;
                    pfr.projectRFIResponse1 = projectRfiResponse;
                    db.ProjectRFIResponses.Add(pfr);
                }
                await db.SaveChangesAsync();
                // if anything changed, send an email to every project user associated with this project
                if (!String.IsNullOrEmpty(projectRfiResponse) ||
                    existingRFIStatusUid != projectRFI.rfiStatusUid.ToString() ||
                    existingAssignedTo != projectRFI.aspNetUserUidAsAssigned)
                {
                    sendEmail = true;
                }

                if (sendEmail)
                {
                    SendMailRFI(projectRFI.projectRFIUid, projectRfiResponse, "Updated");
                }

                return RedirectToAction("Index");
            }
            ViewBag.aspNetUserUidAsAssigned = new SelectList(db.AspNetUsers, "Id", "AllowableContractors", projectRFI.aspNetUserUidAsAssigned);
            ViewBag.aspNetUserUidAsCreated = new SelectList(db.AspNetUsers, "Id", "AllowableContractors", projectRFI.aspNetUserUidAsCreated);
            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName", projectRFI.contractorUid);
            ViewBag.ecmUid = new SelectList(db.ECMs, "ecmUid", "ecmNumber", projectRFI.ecmUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", projectRFI.projectUid);
            ViewBag.rfiTypeUid = new SelectList(db.RfiTypes, "rfiTypeUid", "rfiType1", projectRFI.rfiTypeUid);
            return View(projectRFI);
        }

        private void SendMailRFI(Guid projectRFIUid, string projectRfiResponse, string action)
        {
            string userEdited = System.Web.HttpContext.Current.User.Identity.Name;
            var rfi = from cc in db.ProjectRFIs
                      where cc.projectRFIUid == projectRFIUid
                      select cc;

            rfi = rfi.Include(e => e.Project).Include(e => e.AspNetUser).Include(e => e.AspNetUser1).Include(e => e.RfiStatu);
            string emailPath = Server.MapPath("~/Emails/NewRFI.html");
            string subject = rfi.First().Project.project1 + " -- " + "RFI " + rfi.First().projectRFIID + " Modified";
            string emailMessage = System.IO.File.ReadAllText(emailPath);
            bool isHtml = true;
            string textMessage = "RFI " + rfi.First().projectRFIID + " Modified for project " + rfi.First().Project.project1;

            emailMessage = emailMessage.Replace("{{projectRFIID}}", rfi.First().projectRFIID.ToString());
            emailMessage = emailMessage.Replace("{{projectRFI}}", rfi.First().projectRFI1);
            emailMessage = emailMessage.Replace("{{rfiStatusUid}}", rfi.First().RfiStatu.rfiStatus.ToString());
            emailMessage = emailMessage.Replace("{{aspNetUserUidAsAssigned}}", rfi.First().AspNetUser.Email.ToString());
            emailMessage = emailMessage.Replace("{{projectRfiResponse}}", projectRfiResponse);
            emailMessage = emailMessage.Replace("{{userEdited}}", userEdited);
            emailMessage = emailMessage.Replace("{{action}}", action);            

            MCVExtensions.sendEmailToProjectUsers(db, MCVExtensions.getSessionProject(), subject, emailMessage, isHtml);
            MCVExtensions.sendTextToProjectUsers(db, MCVExtensions.getSessionProject(), subject, textMessage, isHtml);

        }

        // GET: ProjectRFIs/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectRFI projectRFI = await db.ProjectRFIs.FindAsync(id);
            if (projectRFI == null)
            {
                return HttpNotFound();
            }
            return View(projectRFI);
        }

        // POST: ProjectRFIs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ProjectRFI projectRFI = await db.ProjectRFIs.FindAsync(id);
            db.ProjectRFIs.Remove(projectRFI);
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
