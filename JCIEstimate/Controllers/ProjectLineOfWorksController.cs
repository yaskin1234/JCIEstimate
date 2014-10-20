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
    public class ProjectLineOfWorksController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ProjectLineOfWorks
        public async Task<ActionResult> Index()
        {
            var projectLineOfWorks = db.ProjectLineOfWorks.Include(p => p.LineOfWork).Include(p => p.Project);
            return View(await projectLineOfWorks.ToListAsync());
        }

        // GET: ProjectLineOfWorks/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectLineOfWork projectLineOfWork = await db.ProjectLineOfWorks.FindAsync(id);
            if (projectLineOfWork == null)
            {
                return HttpNotFound();
            }
            return View(projectLineOfWork);
        }

        // GET: ProjectLineOfWorks/Create
        public ActionResult Create()
        {
            ViewBag.lineOfWorkUid = new SelectList(db.LineOfWorks, "lineOfWorkUid", "lineOfWork1");
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1");
            return View();
        }

        // POST: ProjectLineOfWorks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "projectLineOfWorkUid,projectUid,lineOfWorkUid,scopeOfWork")] ProjectLineOfWork projectLineOfWork)
        {
            if (ModelState.IsValid)
            {
                projectLineOfWork.projectLineOfWorkUid = Guid.NewGuid();
                db.ProjectLineOfWorks.Add(projectLineOfWork);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.lineOfWorkUid = new SelectList(db.LineOfWorks, "lineOfWorkUid", "lineOfWork1", projectLineOfWork.lineOfWorkUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", projectLineOfWork.projectUid);
            return View(projectLineOfWork);
        }

        // GET: ProjectLineOfWorks/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectLineOfWork projectLineOfWork = await db.ProjectLineOfWorks.FindAsync(id);
            if (projectLineOfWork == null)
            {
                return HttpNotFound();
            }
            ViewBag.lineOfWorkUid = new SelectList(db.LineOfWorks, "lineOfWorkUid", "lineOfWork1", projectLineOfWork.lineOfWorkUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", projectLineOfWork.projectUid);
            return View(projectLineOfWork);
        }

        // POST: ProjectLineOfWorks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "projectLineOfWorkUid,projectUid,lineOfWorkUid,scopeOfWork")] ProjectLineOfWork projectLineOfWork)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectLineOfWork).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.lineOfWorkUid = new SelectList(db.LineOfWorks, "lineOfWorkUid", "lineOfWork1", projectLineOfWork.lineOfWorkUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", projectLineOfWork.projectUid);
            return View(projectLineOfWork);
        }

        // GET: ProjectLineOfWorks/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectLineOfWork projectLineOfWork = await db.ProjectLineOfWorks.FindAsync(id);
            if (projectLineOfWork == null)
            {
                return HttpNotFound();
            }
            return View(projectLineOfWork);
        }

        // POST: ProjectLineOfWorks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ProjectLineOfWork projectLineOfWork = await db.ProjectLineOfWorks.FindAsync(id);
            db.ProjectLineOfWorks.Remove(projectLineOfWork);
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
