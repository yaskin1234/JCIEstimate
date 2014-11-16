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
    public class ProjectMilestonesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ProjectMilestones
        public async Task<ActionResult> Index()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            var projectMilestones = db.ProjectMilestones.Include(p => p.Project).Where(d=>d.projectUid == sessionProject);
            return View(await projectMilestones.ToListAsync());
        }

        // GET: ProjectMilestones/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectMilestone projectMilestone = await db.ProjectMilestones.FindAsync(id);
            if (projectMilestone == null)
            {
                return HttpNotFound();
            }
            return View(projectMilestone);
        }

        // GET: ProjectMilestones/Create
        public ActionResult Create()
        {
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1");
            return View();
        }

        // POST: ProjectMilestones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "projectMilestoneUid,projectUid,projectMilestone1,projectMilestoneDescription,listOrder")] ProjectMilestone projectMilestone)
        {
            if (ModelState.IsValid)
            {
                projectMilestone.projectMilestoneUid = Guid.NewGuid();
                db.ProjectMilestones.Add(projectMilestone);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            ViewBag.projectUid = new SelectList(db.Projects.Where(d => d.projectUid == sessionProject), "projectUid", "project1", projectMilestone.projectUid);
            return View(projectMilestone);
        }

        // GET: ProjectMilestones/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectMilestone projectMilestone = await db.ProjectMilestones.FindAsync(id);
            if (projectMilestone == null)
            {
                return HttpNotFound();
            }
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            ViewBag.projectUid = new SelectList(db.Projects.Where(d => d.projectUid == sessionProject), "projectUid", "project1", projectMilestone.projectUid);
            return View(projectMilestone);
        }

        // POST: ProjectMilestones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "projectMilestoneUid,projectUid,projectMilestone1,projectMilestoneDescription,listOrder")] ProjectMilestone projectMilestone)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectMilestone).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", projectMilestone.projectUid);
            return View(projectMilestone);
        }

        // GET: ProjectMilestones/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectMilestone projectMilestone = await db.ProjectMilestones.FindAsync(id);
            if (projectMilestone == null)
            {
                return HttpNotFound();
            }
            return View(projectMilestone);
        }

        // POST: ProjectMilestones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ProjectMilestone projectMilestone = await db.ProjectMilestones.FindAsync(id);
            db.ProjectMilestones.Remove(projectMilestone);
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
