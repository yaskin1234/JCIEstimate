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

namespace JCIEstimate.Controllers
{
    public class ProjectManagerAttachmentsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ProjectManagerAttachments
        public async Task<ActionResult> Index()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            var projectManagerAttachments = db.ProjectManagerAttachments.Where(c => c.projectUid == sessionProject).Include(p => p.AspNetUser).Include(p => p.Project);
            return View(await projectManagerAttachments.ToListAsync());
        }

        // GET: ProjectManagerAttachments/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectManagerAttachment projectManagerAttachment = await db.ProjectManagerAttachments.FindAsync(id);
            if (projectManagerAttachment == null)
            {
                return HttpNotFound();
            }
            return View(projectManagerAttachment);
        }

        // GET: ProjectManagerAttachments/Create
        public ActionResult Create()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            ViewBag.aspNetUserUidAsCreated = new SelectList(db.AspNetUsers, "id", "email");
            ViewBag.projectUid = new SelectList(db.Projects.Where(c=>c.projectUid == sessionProject), "projectUid", "project1");
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetProjectManagerAttachment(Guid projectManagerAttachmentUid, string fileType)
        {
            var d = from cc in db.ProjectManagerAttachments
                    where cc.projectManagerAttachmentUid == projectManagerAttachmentUid
                    select cc.attachment;

            var docName = from cc in db.ProjectManagerAttachments
                          where cc.projectManagerAttachmentUid == projectManagerAttachmentUid
                          select cc.documentName;


            byte[] byteArray = d.FirstOrDefault();
            return File(byteArray, "application/octect-stream", docName.FirstOrDefault());
        }

        // POST: ProjectManagerAttachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "projectManagerAttachmentUid,projectUid,aspNetUserUidAsCreated,dateCreated,projectManagerAttachment1,attachment,fileType,documentName")] ProjectManagerAttachment projectManagerAttachment, HttpPostedFileBase postedFile)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            if (ModelState.IsValid)
            {
                projectManagerAttachment.projectManagerAttachmentUid = Guid.NewGuid();
                var userCreated = from cc in db.AspNetUsers
                                  where cc.UserName == System.Web.HttpContext.Current.User.Identity.Name
                                  select cc.Id;
                if (postedFile != null)
                {
                    int fileSize = postedFile.ContentLength;
                    MemoryStream target = new MemoryStream();
                    postedFile.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();
                    projectManagerAttachment.attachment = data;
                    projectManagerAttachment.fileType = Path.GetExtension(postedFile.FileName);
                    var docName = postedFile.FileName;
                    projectManagerAttachment.documentName = docName;
                    projectManagerAttachment.dateCreated = DateTime.Now;
                    projectManagerAttachment.aspNetUserUidAsCreated = userCreated.FirstOrDefault();
                }
                db.ProjectManagerAttachments.Add(projectManagerAttachment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.aspNetUserUidAsCreated = new SelectList(db.AspNetUsers, "Id", "Email", projectManagerAttachment.aspNetUserUidAsCreated);
            ViewBag.projectUid = new SelectList(db.Projects.Where(c=>c.projectUid == sessionProject), "projectUid", "project1", projectManagerAttachment.projectUid);
            return View(projectManagerAttachment);
        }

        // GET: ProjectManagerAttachments/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectManagerAttachment projectManagerAttachment = await db.ProjectManagerAttachments.FindAsync(id);
            if (projectManagerAttachment == null)
            {
                return HttpNotFound();
            }
            ViewBag.aspNetUserUidAsCreated = new SelectList(db.AspNetUsers, "Id", "AllowableContractors", projectManagerAttachment.aspNetUserUidAsCreated);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", projectManagerAttachment.projectUid);
            return View(projectManagerAttachment);
        }

        // POST: ProjectManagerAttachments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "projectManagerAttachmentUid,projectUid,aspNetUserUidAsCreated,dateCreated,projectManagerAttachment1,attachment,fileType,documentName")] ProjectManagerAttachment projectManagerAttachment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectManagerAttachment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.aspNetUserUidAsCreated = new SelectList(db.AspNetUsers, "Id", "AllowableContractors", projectManagerAttachment.aspNetUserUidAsCreated);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", projectManagerAttachment.projectUid);
            return View(projectManagerAttachment);
        }

        // GET: ProjectManagerAttachments/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectManagerAttachment projectManagerAttachment = await db.ProjectManagerAttachments.FindAsync(id);
            if (projectManagerAttachment == null)
            {
                return HttpNotFound();
            }
            return View(projectManagerAttachment);
        }

        // POST: ProjectManagerAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ProjectManagerAttachment projectManagerAttachment = await db.ProjectManagerAttachments.FindAsync(id);
            db.ProjectManagerAttachments.Remove(projectManagerAttachment);
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
