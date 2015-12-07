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
    public class ProjectTaskListsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ProjectTaskLists
        public async Task<ActionResult> Index()
        {
            var projectTaskLists = db.ProjectTaskLists.Include(p => p.Project).Include(p => p.ProjectTaskList2).Include(p => p.ProjectTaskList3);
            return View(await projectTaskLists.OrderBy(c=>c.projectTaskSequence).ToListAsync());
        }

        public async Task<ActionResult> SaveTask(Guid projectTaskListUid, DateTime startDate, int duration, int predecessor)
        {
            ProjectTaskList ptl = db.ProjectTaskLists.Find(projectTaskListUid);
            db.Entry(ptl).State = EntityState.Modified;
            ptl.projectTaskStartDate = startDate;
            ptl.projectTaskDuration = duration;
            if (predecessor != 0)
            {
                var predecessorUid = from cc in db.ProjectTaskLists
                                     where cc.projectTaskSequence == predecessor
                                     select cc.projectTaskListUid;
                ptl.projectTaskListUidAsPredecessor = predecessorUid.FirstOrDefault();
            }
            else
            {
                ptl.projectTaskListUidAsPredecessor = null;
            }

            await db.SaveChangesAsync();
            return View();
        }

        // GET: Projects/AddTasksForProject
        public ActionResult AddTasksForProject()
        {
            Guid sessionProject = MCVExtensions.getSessionProject();
            int taskSequence = 100;
            string currentCategory = "";
            string previousCategory = "";
            Guid? projectTaskUidAsParent = null;
            foreach (var item in db.ProjectTaskPrototypes.OrderBy(c=>c.sequence))
            {
                currentCategory = item.ProjectTaskCategory.projectTaskCategory1;                

                if (currentCategory != previousCategory)
                {
                    ProjectTaskList ptl = new ProjectTaskList();
                    
                    ptl.projectTaskListUid = Guid.NewGuid();
                    ptl.projectTaskSequence = taskSequence;
                    ptl.projectTask = currentCategory;
                    ptl.projectUid = sessionProject;
                    projectTaskUidAsParent = ptl.projectTaskListUid;

                    previousCategory = currentCategory;
                    db.ProjectTaskLists.Add(ptl);
                    taskSequence += 100;
                }

                ProjectTaskList ptl1 = new ProjectTaskList();
                ptl1.projectTaskListUid = Guid.NewGuid();
                ptl1.projectTaskSequence = taskSequence;
                ptl1.projectTask = item.projectTaskPrototype1;
                ptl1.projectUid = sessionProject;
                ptl1.projectTaskListUidAsParent = projectTaskUidAsParent;
                previousCategory = currentCategory;
                db.ProjectTaskLists.Add(ptl1);
                taskSequence += 100;
            }
            db.SaveChanges();
            RedirectToAction("Index", "ProjectTaskLists");
            return View();
        }        

        // GET: ProjectTaskLists/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTaskList projectTaskList = await db.ProjectTaskLists.FindAsync(id);
            if (projectTaskList == null)
            {
                return HttpNotFound();
            }
            return View(projectTaskList);
        }

        // GET: ProjectTaskLists/Create
        public ActionResult Create()
        {
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1");
            ViewBag.projectTaskListUidAsParent = new SelectList(db.ProjectTaskLists, "projectTaskListUid", "projectTask");
            ViewBag.projectTaskListUidAsPredecessor = new SelectList(db.ProjectTaskLists, "projectTaskListUid", "projectTask");
            return View();
        }

        // POST: ProjectTaskLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "projectTaskListUid,projectUid,projectTaskListUidAsParent,projectTaskListUidAsPredecessor,projectTask,projectTaskSequence,projectTaskStartDate,projectTaskDuration")] ProjectTaskList projectTaskList)
        {
            if (ModelState.IsValid)
            {
                projectTaskList.projectTaskListUid = Guid.NewGuid();
                db.ProjectTaskLists.Add(projectTaskList);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", projectTaskList.projectUid);
            ViewBag.projectTaskListUidAsParent = new SelectList(db.ProjectTaskLists, "projectTaskListUid", "projectTask", projectTaskList.projectTaskListUidAsParent);
            ViewBag.projectTaskListUidAsPredecessor = new SelectList(db.ProjectTaskLists, "projectTaskListUid", "projectTask", projectTaskList.projectTaskListUidAsPredecessor);
            return View(projectTaskList);
        }

        // GET: ProjectTaskLists/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTaskList projectTaskList = await db.ProjectTaskLists.FindAsync(id);
            if (projectTaskList == null)
            {
                return HttpNotFound();
            }
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", projectTaskList.projectUid);
            ViewBag.projectTaskListUidAsParent = new SelectList(db.ProjectTaskLists, "projectTaskListUid", "projectTask", projectTaskList.projectTaskListUidAsParent);
            ViewBag.projectTaskListUidAsPredecessor = new SelectList(db.ProjectTaskLists, "projectTaskListUid", "projectTask", projectTaskList.projectTaskListUidAsPredecessor);
            return View(projectTaskList);
        }

        // POST: ProjectTaskLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "projectTaskListUid,projectUid,projectTaskListUidAsParent,projectTaskListUidAsPredecessor,projectTask,projectTaskSequence,projectTaskStartDate,projectTaskDuration")] ProjectTaskList projectTaskList)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectTaskList).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", projectTaskList.projectUid);
            ViewBag.projectTaskListUidAsParent = new SelectList(db.ProjectTaskLists, "projectTaskListUid", "projectTask", projectTaskList.projectTaskListUidAsParent);
            ViewBag.projectTaskListUidAsPredecessor = new SelectList(db.ProjectTaskLists, "projectTaskListUid", "projectTask", projectTaskList.projectTaskListUidAsPredecessor);
            return View(projectTaskList);
        }

        // GET: ProjectTaskLists/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectTaskList projectTaskList = await db.ProjectTaskLists.FindAsync(id);
            if (projectTaskList == null)
            {
                return HttpNotFound();
            }
            return View(projectTaskList);
        }

        // POST: ProjectTaskLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ProjectTaskList projectTaskList = await db.ProjectTaskLists.FindAsync(id);
            db.ProjectTaskLists.Remove(projectTaskList);
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
