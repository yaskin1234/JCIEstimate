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
    public class ProjectUsersController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ProjectUsers
        public async Task<ActionResult> Index()
        {
            var projectUsers = db.ProjectUsers.Include(p => p.AspNetUser).Include(p => p.Project);
            return View(await projectUsers.ToListAsync());
        }

        // GET: ProjectUsers/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectUser projectUser = await db.ProjectUsers.FindAsync(id);
            if (projectUser == null)
            {
                return HttpNotFound();
            }
            return View(projectUser);
        }

        // GET: ProjectUsers/Create
        public ActionResult Create()
        {
            ViewBag.aspNetUserUid = new SelectList(db.AspNetUsers, "Id", "UserName");
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1");
            return View();
        }

        // POST: ProjectUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "projectUserUid,aspNetUserUid,projectUid")] ProjectUser projectUser)
        {
            if (ModelState.IsValid)
            {
                projectUser.projectUserUid = Guid.NewGuid();
                db.ProjectUsers.Add(projectUser);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.aspNetUserUid = new SelectList(db.AspNetUsers, "Id", "UserName", projectUser.aspNetUserUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", projectUser.projectUid);
            return View(projectUser);
        }

        // GET: ProjectUsers/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectUser projectUser = await db.ProjectUsers.FindAsync(id);
            if (projectUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.aspNetUserUid = new SelectList(db.AspNetUsers, "Id", "UserName", projectUser.aspNetUserUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", projectUser.projectUid);
            return View(projectUser);
        }

        // POST: ProjectUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "projectUserUid,aspNetUserUid,projectUid")] ProjectUser projectUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectUser).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.aspNetUserUid = new SelectList(db.AspNetUsers, "Id", "UserName", projectUser.aspNetUserUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", projectUser.projectUid);
            return View(projectUser);
        }

        // GET: ProjectUsers/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectUser projectUser = await db.ProjectUsers.FindAsync(id);
            if (projectUser == null)
            {
                return HttpNotFound();
            }
            return View(projectUser);
        }

        // POST: ProjectUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ProjectUser projectUser = await db.ProjectUsers.FindAsync(id);
            db.ProjectUsers.Remove(projectUser);
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
