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
    public class ProjectToDoesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ProjectToDoes
        public async Task<ActionResult> Index()
        {
            var projectToDoes = db.ProjectToDoes.Include(p => p.ToDoStatu);
            return View(await projectToDoes.ToListAsync());
        }

        // GET: ProjectToDoes/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectToDo projectToDo = await db.ProjectToDoes.FindAsync(id);
            if (projectToDo == null)
            {
                return HttpNotFound();
            }
            return View(projectToDo);
        }

        // GET: ProjectToDoes/Create
        public ActionResult Create()
        {
            ViewBag.toDoStatusUid = new SelectList(db.ToDoStatus, "toDoStatusUid", "toDoStatus");
            return View();
        }

        // POST: ProjectToDoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "projectToDoUid,projectToDo1,projectToDoDescription,toDoStatusUid,dateCreated,dateResolved")] ProjectToDo projectToDo)
        {
            if (ModelState.IsValid)
            {
                projectToDo.dateCreated = DateTime.Now;
                projectToDo.projectToDoUid = Guid.NewGuid();
                db.ProjectToDoes.Add(projectToDo);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            

            ViewBag.toDoStatusUid = new SelectList(db.ToDoStatus, "toDoStatusUid", "toDoStatus", projectToDo.toDoStatusUid);
            return View(projectToDo);
        }

        // GET: ProjectToDoes/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectToDo projectToDo = await db.ProjectToDoes.FindAsync(id);
            if (projectToDo == null)
            {
                return HttpNotFound();
            }
            ViewBag.toDoStatusUid = new SelectList(db.ToDoStatus, "toDoStatusUid", "toDoStatus", projectToDo.toDoStatusUid);
            return View(projectToDo);
        }

        // POST: ProjectToDoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "projectToDoUid,projectToDo1,projectToDoDescription,toDoStatusUid,dateCreated,dateResolved")] ProjectToDo projectToDo)
        {
            var pToDo = from cc in db.ToDoStatus
                        where projectToDo.toDoStatusUid == cc.toDoStatusUid
                        select cc.behaviorIndicator;


            if (pToDo.FirstOrDefault() == "C")
            {
                //JCIExtensions.MCVExtensions.sendMail("To do task :" + projectToDo.projectToDo1 + " \r\n has been marked as ready completed and is ready for validation", "To Do Status change", "admin@jciestimate.com", "brian@ld0designs.net");
            }

            if (ModelState.IsValid)
            {
                db.Entry(projectToDo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }


            ViewBag.toDoStatusUid = new SelectList(db.ToDoStatus, "toDoStatusUid", "toDoStatus", projectToDo.toDoStatusUid);
            return View(projectToDo);
        }

        // GET: ProjectToDoes/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectToDo projectToDo = await db.ProjectToDoes.FindAsync(id);
            if (projectToDo == null)
            {
                return HttpNotFound();
            }
            return View(projectToDo);
        }

        // POST: ProjectToDoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ProjectToDo projectToDo = await db.ProjectToDoes.FindAsync(id);
            db.ProjectToDoes.Remove(projectToDo);
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
