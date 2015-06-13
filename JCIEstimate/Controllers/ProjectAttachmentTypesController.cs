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

namespace JCIEstimate.Views
{
    public class ProjectAttachmentTypesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ProjectAttachmentTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.ProjectAttachmentTypes.ToListAsync());
        }

        // GET: ProjectAttachmentTypes/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectAttachmentType projectAttachmentType = await db.ProjectAttachmentTypes.FindAsync(id);
            if (projectAttachmentType == null)
            {
                return HttpNotFound();
            }
            return View(projectAttachmentType);
        }

        // GET: ProjectAttachmentTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProjectAttachmentTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "projectAttachmentTypeUid,projectAttachmentType1,behaviorIndicator")] ProjectAttachmentType projectAttachmentType)
        {
            if (ModelState.IsValid)
            {
                projectAttachmentType.projectAttachmentTypeUid = Guid.NewGuid();
                db.ProjectAttachmentTypes.Add(projectAttachmentType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(projectAttachmentType);
        }

        // GET: ProjectAttachmentTypes/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectAttachmentType projectAttachmentType = await db.ProjectAttachmentTypes.FindAsync(id);
            if (projectAttachmentType == null)
            {
                return HttpNotFound();
            }
            return View(projectAttachmentType);
        }

        // POST: ProjectAttachmentTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "projectAttachmentTypeUid,projectAttachmentType1,behaviorIndicator")] ProjectAttachmentType projectAttachmentType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectAttachmentType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(projectAttachmentType);
        }

        // GET: ProjectAttachmentTypes/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectAttachmentType projectAttachmentType = await db.ProjectAttachmentTypes.FindAsync(id);
            if (projectAttachmentType == null)
            {
                return HttpNotFound();
            }
            return View(projectAttachmentType);
        }

        // POST: ProjectAttachmentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ProjectAttachmentType projectAttachmentType = await db.ProjectAttachmentTypes.FindAsync(id);
            db.ProjectAttachmentTypes.Remove(projectAttachmentType);
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
