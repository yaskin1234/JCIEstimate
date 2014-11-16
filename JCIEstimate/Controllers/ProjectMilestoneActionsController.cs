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
    public class ProjectMilestoneActionsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ProjectMilestoneActions
        public async Task<ActionResult> Index()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            var projectMilestoneActions = db.ProjectMilestoneActions.Include(p => p.ProjectMilestone).Where(d => d.ProjectMilestone.projectUid == sessionProject).OrderBy(d => d.ProjectMilestone.listOrder).ThenBy(d => d.listOrder);
            return View(await projectMilestoneActions.ToListAsync());
        }

        // GET: ProjectMilestoneActions/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectMilestoneAction projectMilestoneAction = await db.ProjectMilestoneActions.FindAsync(id);
            if (projectMilestoneAction == null)
            {
                return HttpNotFound();
            }
            return View(projectMilestoneAction);
        }

        // GET: ProjectMilestoneActions/Create
        public ActionResult Create()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            ViewBag.projectMilestoneUid = new SelectList(db.ProjectMilestones.Where(d => d.projectUid == sessionProject), "projectMilestoneUid", "projectMilestone1");
            return View();
        }

        // POST: ProjectMilestoneActions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "projectMilestoneActionUid,projectMilestoneUid,projectMilestoneAction1,projectMilestoneActionDescription,plannedStartDate,actualStartDate,duration,endDate,listOrder")] ProjectMilestoneAction projectMilestoneAction)
        {
            if (ModelState.IsValid)
            {
                projectMilestoneAction.projectMilestoneActionUid = Guid.NewGuid();
                db.ProjectMilestoneActions.Add(projectMilestoneAction);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.projectMilestoneUid = new SelectList(db.ProjectMilestones, "projectMilestoneUid", "projectMilestone1", projectMilestoneAction.projectMilestoneUid);
            return View(projectMilestoneAction);
        }

        // GET: ProjectMilestoneActions/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectMilestoneAction projectMilestoneAction = await db.ProjectMilestoneActions.FindAsync(id);
            if (projectMilestoneAction == null)
            {
                return HttpNotFound();
            }
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            ViewBag.projectMilestoneUid = new SelectList(db.ProjectMilestones.Where(d => d.projectUid == sessionProject), "projectMilestoneUid", "projectMilestone1", projectMilestoneAction.projectMilestoneUid);
            return View(projectMilestoneAction);
        }

        // POST: ProjectMilestoneActions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "projectMilestoneActionUid,projectMilestoneUid,projectMilestoneAction1,projectMilestoneActionDescription,plannedStartDate,actualStartDate,duration,endDate,listOrder")] ProjectMilestoneAction projectMilestoneAction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectMilestoneAction).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.projectMilestoneUid = new SelectList(db.ProjectMilestones, "projectMilestoneUid", "projectMilestone1", projectMilestoneAction.projectMilestoneUid);
            return View(projectMilestoneAction);
        }

        // GET: ProjectMilestoneActions/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectMilestoneAction projectMilestoneAction = await db.ProjectMilestoneActions.FindAsync(id);
            if (projectMilestoneAction == null)
            {
                return HttpNotFound();
            }
            return View(projectMilestoneAction);
        }

        // POST: ProjectMilestoneActions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ProjectMilestoneAction projectMilestoneAction = await db.ProjectMilestoneActions.FindAsync(id);
            db.ProjectMilestoneActions.Remove(projectMilestoneAction);
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
