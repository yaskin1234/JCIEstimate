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
using Microsoft.AspNet.Identity;
using JCIExtensions;

namespace JCIEstimate.Controllers
{
    public class ProjectAddendumsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ProjectAddendums
        public async Task<ActionResult> Index()
        {
            
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            var projectAddendums = from cc in db.ProjectAddendums
                                   where cc.projectUid == sessionProject
                                   select cc;

            
            projectAddendums = db.ProjectAddendums.Include(p => p.Project);
            return View(await projectAddendums.ToListAsync());
        }

        // GET: ProjectAddendums/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectAddendum projectAddendum = await db.ProjectAddendums.FindAsync(id);
            if (projectAddendum == null)
            {
                return HttpNotFound();
            }
            return View(projectAddendum);
        }

        // GET: ProjectAddendums/Create
        public ActionResult Create()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            var projects = from cc in db.Projects
                           where cc.projectUid == sessionProject
                           select cc;

            ViewBag.projectUid = new SelectList(projects , "projectUid", "project1");
            return View();
        }

        // POST: ProjectAddendums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "projectAddendumUid,projectUid,dateCreated,aspNetUserUidAsCreated,projectAddendum1,addendumId")] ProjectAddendum projectAddendum)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            if (ModelState.IsValid)
            {                
                projectAddendum.projectAddendumUid = Guid.NewGuid();
                projectAddendum.dateCreated = DateTime.Now;
                projectAddendum.projectUid = sessionProject;
                projectAddendum.aspNetUserUidAsCreated = IdentityExtensions.GetUserId(User.Identity);
                db.ProjectAddendums.Add(projectAddendum);
                await db.SaveChangesAsync();
                SendMailAddendum(projectAddendum.projectAddendumUid);
                return RedirectToAction("Index");
            }

            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", projectAddendum.projectUid);
            return View(projectAddendum);
        }

        // GET: ProjectAddendums/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectAddendum projectAddendum = await db.ProjectAddendums.FindAsync(id);
            if (projectAddendum == null)
            {
                return HttpNotFound();
            }
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", projectAddendum.projectUid);
            return View(projectAddendum);
        }

        // POST: ProjectAddendums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "projectAddendumUid,projectUid,dateCreated,aspNetUserUidAsCreated,projectAddendum1,addendumId")] ProjectAddendum projectAddendum)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectAddendum).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", projectAddendum.projectUid);
            return View(projectAddendum);
        }

        // GET: ProjectAddendums/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectAddendum projectAddendum = await db.ProjectAddendums.FindAsync(id);
            if (projectAddendum == null)
            {
                return HttpNotFound();
            }
            return View(projectAddendum);
        }

        // POST: ProjectAddendums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ProjectAddendum projectAddendum = await db.ProjectAddendums.FindAsync(id);
            db.ProjectAddendums.Remove(projectAddendum);
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

        private void SendMailAddendum(Guid projectAddendumUid)
        {
            string userEdited = System.Web.HttpContext.Current.User.Identity.Name;
            var addendum = from cc in db.ProjectAddendums
                           where cc.projectAddendumUid == projectAddendumUid
                           select cc;

            addendum = addendum.Include(e => e.Project);
            string emailPath = Server.MapPath("~/Emails/NewAddendum.html");
            string subject = addendum.First().Project.project1 + " -- " + "Addendum " + addendum.First().addendumId + " Modified";
            string emailMessage = System.IO.File.ReadAllText(emailPath);
            bool isHtml = true;

            emailMessage = emailMessage.Replace("{{addendumId}}", addendum.First().addendumId.ToString());
            emailMessage = emailMessage.Replace("{{projectAddendum1}}", addendum.First().projectAddendum1);                                    
            emailMessage = emailMessage.Replace("{{userEdited}}", userEdited);            

            MCVExtensions.sendEmailToProjectUsers(db, MCVExtensions.getSessionProject(), subject, emailMessage, isHtml, false);
            //MCVExtensions.sendTextToProjectUsers(db, MCVExtensions.getSessionProject(), subject, emailMessage, isHtml, false);
        }
    }
}
