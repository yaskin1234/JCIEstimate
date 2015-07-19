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
using System.IO;
using JCIExtensions;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace JCIEstimate.Controllers
{
    public class ProjectAttachmentsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ProjectAttachments
        public async Task<ActionResult> Index()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            var projectAttachments = db.ProjectAttachments.Where(c=>c.projectUid == sessionProject).Include(p => p.Project).OrderBy(c=>c.projectAttachment1);
            return View(await projectAttachments.ToListAsync());
        }

        // GET: ProjectAttachments/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectAttachment projectAttachment = await db.ProjectAttachments.FindAsync(id);
            if (projectAttachment == null)
            {
                return HttpNotFound();
            }
            return View(projectAttachment);
        }

        // GET: ProjectAttachments/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1");
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetProjectAttachment(Guid projectAttachmentUid, string fileType)
        {
            var d = from cc in db.ProjectAttachments
                    where cc.projectAttachmentUid == projectAttachmentUid
                    select cc.attachment;

            var docName = from cc in db.ProjectAttachments
                          where cc.projectAttachmentUid == projectAttachmentUid
                          select cc.documentName;


            byte[] byteArray = d.FirstOrDefault();
            return File(byteArray, "application/octect-stream", docName.FirstOrDefault());
        }

        // POST: ProjectAttachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProjectAttachment projectAttachment, HttpPostedFileBase postedFile)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            
            if (ModelState.IsValid)
            {
                if (postedFile != null)
                {
                    int fileSize = postedFile.ContentLength;
                    MemoryStream target = new MemoryStream();
                    postedFile.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();
                    projectAttachment.attachment = data;
                    projectAttachment.fileType = Path.GetExtension(postedFile.FileName);
                }

                var docName = postedFile.FileName;
                projectAttachment.documentName = docName;
                projectAttachment.projectAttachmentUid = Guid.NewGuid();
                db.ProjectAttachments.Add(projectAttachment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", sessionProject);
            return View(projectAttachment);
        }

        // GET: ProjectAttachments/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(Guid? id)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectAttachment projectAttachment = await db.ProjectAttachments.FindAsync(id);
            if (projectAttachment == null)
            {
                return HttpNotFound();
            }
            var projects = from cc in db.Projects
                           where cc.projectUid == sessionProject
                           select cc;

            ViewBag.projectUid = new SelectList(projects, "projectUid", "project1", projectAttachment.projectUid);
            return View(projectAttachment);
        }

        // POST: ProjectAttachments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "projectAttachmentUid,projectUid,projectAttachment1,fileType,documentName,attachment")] ProjectAttachment projectAttachment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectAttachment).State = EntityState.Modified;
                try
                {
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}",
                                                    validationError.PropertyName,
                                                    validationError.ErrorMessage);
                        }
                    }
                }
                
                
            }
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", projectAttachment.projectUid);
            return View(projectAttachment);
        }

        [Authorize(Roles = "Admin")]
        // GET: ProjectAttachments/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectAttachment projectAttachment = await db.ProjectAttachments.FindAsync(id);
            if (projectAttachment == null)
            {
                return HttpNotFound();
            }
            return View(projectAttachment);
        }

        // POST: ProjectAttachments/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ProjectAttachment projectAttachment = await db.ProjectAttachments.FindAsync(id);
            db.ProjectAttachments.Remove(projectAttachment);
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
