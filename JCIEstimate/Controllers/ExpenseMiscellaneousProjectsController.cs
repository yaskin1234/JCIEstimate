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
    public class ExpenseMiscellaneousProjectsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ExpenseMiscellaneousProjects
        public async Task<ActionResult> Index()
        {
            IQueryable<ExpenseMiscellaneousProject> expenseMiscellaneousProjects;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            expenseMiscellaneousProjects = from cc in db.ExpenseMiscellaneousProjects
                         where cc.projectUid == sessionProject
                         select cc;
            expenseMiscellaneousProjects = expenseMiscellaneousProjects.Include(e => e.ExpenseMiscellaneou).Include(e => e.Project);
            return View(await expenseMiscellaneousProjects.ToListAsync());
        }

        // GET: ExpenseMiscellaneousProjects/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpenseMiscellaneousProject expenseMiscellaneousProject = await db.ExpenseMiscellaneousProjects.FindAsync(id);
            if (expenseMiscellaneousProject == null)
            {
                return HttpNotFound();
            }
            return View(expenseMiscellaneousProject);
        }

        // GET: ExpenseMiscellaneousProjects/Create
        public ActionResult Create()
        {
            IQueryable<Project> projects;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            projects = from cc in db.Projects
                        where cc.projectUid == sessionProject
                        select cc;
            ViewBag.expenseMiscellaneousUid = new SelectList(db.ExpenseMiscellaneous, "expenseMiscellaneousUid", "expenseMiscellaneous");
            ViewBag.projectUid = new SelectList(projects, "projectUid", "project1");
            return View();
        }

        // POST: ExpenseMiscellaneousProjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "expenseMiscellaneousProjectUid,expenseMiscellaneousUid,projectUid,total")] ExpenseMiscellaneousProject expenseMiscellaneousProject)
        {
            if (ModelState.IsValid)
            {
                expenseMiscellaneousProject.expenseMiscellaneousProjectUid = Guid.NewGuid();
                db.ExpenseMiscellaneousProjects.Add(expenseMiscellaneousProject);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.expenseMiscellaneousUid = new SelectList(db.ExpenseMiscellaneous, "expenseMiscellaneousUid", "expenseMiscellaneous", expenseMiscellaneousProject.expenseMiscellaneousUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", expenseMiscellaneousProject.projectUid);
            return View(expenseMiscellaneousProject);
        }

        // GET: ExpenseMiscellaneousProjects/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpenseMiscellaneousProject expenseMiscellaneousProject = await db.ExpenseMiscellaneousProjects.FindAsync(id);
            if (expenseMiscellaneousProject == null)
            {
                return HttpNotFound();
            }
            IQueryable<Project> projects;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            projects = from cc in db.Projects
                       where cc.projectUid == sessionProject
                       select cc;
            ViewBag.expenseMiscellaneousUid = new SelectList(db.ExpenseMiscellaneous, "expenseMiscellaneousUid", "expenseMiscellaneous", expenseMiscellaneousProject.expenseMiscellaneousUid);
            ViewBag.projectUid = new SelectList(projects, "projectUid", "project1", expenseMiscellaneousProject.projectUid);
            return View(expenseMiscellaneousProject);
        }

        // POST: ExpenseMiscellaneousProjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "expenseMiscellaneousProjectUid,expenseMiscellaneousUid,projectUid,total")] ExpenseMiscellaneousProject expenseMiscellaneousProject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(expenseMiscellaneousProject).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.expenseMiscellaneousUid = new SelectList(db.ExpenseMiscellaneous, "expenseMiscellaneousUid", "expenseMiscellaneous", expenseMiscellaneousProject.expenseMiscellaneousUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", expenseMiscellaneousProject.projectUid);
            return View(expenseMiscellaneousProject);
        }

        // GET: ExpenseMiscellaneousProjects/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpenseMiscellaneousProject expenseMiscellaneousProject = await db.ExpenseMiscellaneousProjects.FindAsync(id);
            if (expenseMiscellaneousProject == null)
            {
                return HttpNotFound();
            }
            return View(expenseMiscellaneousProject);
        }

        // POST: ExpenseMiscellaneousProjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ExpenseMiscellaneousProject expenseMiscellaneousProject = await db.ExpenseMiscellaneousProjects.FindAsync(id);
            db.ExpenseMiscellaneousProjects.Remove(expenseMiscellaneousProject);
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
