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
    public class ProjectTaskCategoriesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ProjectTaskCategories
        public async Task<ActionResult> Index()
        {
            return View(await db.ProjectTaskCategories.ToListAsync());
        }

        // GET: ProjectTaskCategories/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTaskCategory projectTaskCategory = await db.ProjectTaskCategories.FindAsync(id);
            if (projectTaskCategory == null)
            {
                return HttpNotFound();
            }
            return View(projectTaskCategory);
        }

        // GET: ProjectTaskCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProjectTaskCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "projectTaskCategoryUid,projectTaskCategory1")] ProjectTaskCategory projectTaskCategory)
        {
            if (ModelState.IsValid)
            {
                projectTaskCategory.projectTaskCategoryUid = Guid.NewGuid();
                db.ProjectTaskCategories.Add(projectTaskCategory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(projectTaskCategory);
        }

        // GET: ProjectTaskCategories/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTaskCategory projectTaskCategory = await db.ProjectTaskCategories.FindAsync(id);
            if (projectTaskCategory == null)
            {
                return HttpNotFound();
            }
            return View(projectTaskCategory);
        }

        // POST: ProjectTaskCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "projectTaskCategoryUid,projectTaskCategory1")] ProjectTaskCategory projectTaskCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectTaskCategory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(projectTaskCategory);
        }

        // GET: ProjectTaskCategories/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTaskCategory projectTaskCategory = await db.ProjectTaskCategories.FindAsync(id);
            if (projectTaskCategory == null)
            {
                return HttpNotFound();
            }
            return View(projectTaskCategory);
        }

        // POST: ProjectTaskCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ProjectTaskCategory projectTaskCategory = await db.ProjectTaskCategories.FindAsync(id);
            db.ProjectTaskCategories.Remove(projectTaskCategory);
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
