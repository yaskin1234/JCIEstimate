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

namespace JCIEstimate.Controllers
{
    public class ProjectTaskPrototypesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ProjectTaskPrototypes
        public async Task<ActionResult> Index()
        {
            return View(await db.ProjectTaskPrototypes.OrderByDescending(c=>c.sequence).ToListAsync());
        }

        // GET: ProjectTaskPrototypes/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTaskPrototype projectTaskPrototype = await db.ProjectTaskPrototypes.FindAsync(id);
            if (projectTaskPrototype == null)
            {
                return HttpNotFound();
            }
            return View(projectTaskPrototype);
        }

        // GET: ProjectTaskPrototypes/Create
        public ActionResult Create()
        {
            ViewBag.projectTaskCategoryUid = db.ProjectTaskCategories.OrderBy(c => c.projectTaskCategory1).ToSelectList(c => c.projectTaskCategory1, c => c.projectTaskCategoryUid.ToString(), "");
            return View();
        }

        // POST: ProjectTaskPrototypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "projectTaskPrototypeUid,projectTaskPrototype1,sequence,projectTaskCategoryUid")] ProjectTaskPrototype projectTaskPrototype)
        {
            if (ModelState.IsValid)
            {
                projectTaskPrototype.projectTaskPrototypeUid = Guid.NewGuid();
                db.ProjectTaskPrototypes.Add(projectTaskPrototype);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(projectTaskPrototype);
        }

        // GET: ProjectTaskPrototypes/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTaskPrototype projectTaskPrototype = await db.ProjectTaskPrototypes.FindAsync(id);
            if (projectTaskPrototype == null)
            {
                return HttpNotFound();
            }

            ViewBag.projectTaskCategoryUid = db.ProjectTaskCategories.OrderBy(c => c.projectTaskCategory1).ToSelectList(c => c.projectTaskCategory1, c => c.projectTaskCategoryUid.ToString(), projectTaskPrototype.projectTaskCategoryUid.ToString());
            return View(projectTaskPrototype);
        }

        // POST: ProjectTaskPrototypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "projectTaskPrototypeUid,projectTaskPrototype1,sequence,projectTaskCategoryUid")] ProjectTaskPrototype projectTaskPrototype)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectTaskPrototype).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(projectTaskPrototype);
        }

        // GET: ProjectTaskPrototypes/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTaskPrototype projectTaskPrototype = await db.ProjectTaskPrototypes.FindAsync(id);
            if (projectTaskPrototype == null)
            {
                return HttpNotFound();
            }
            return View(projectTaskPrototype);
        }

        // POST: ProjectTaskPrototypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ProjectTaskPrototype projectTaskPrototype = await db.ProjectTaskPrototypes.FindAsync(id);
            db.ProjectTaskPrototypes.Remove(projectTaskPrototype);
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
