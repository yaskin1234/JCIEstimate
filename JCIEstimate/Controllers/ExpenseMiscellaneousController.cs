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
    public class ExpenseMiscellaneousController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ExpenseMiscellaneous
        public async Task<ActionResult> Index()
        {
            IQueryable<ExpenseMiscellaneou> expenseMiscellaneous;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            expenseMiscellaneous = from cc in db.ExpenseMiscellaneous
                                   where cc.projectUid == sessionProject
                                   select cc;            
            expenseMiscellaneous = expenseMiscellaneous.Include(e => e.Project);
            return View(await expenseMiscellaneous.ToListAsync());
        }

        // GET: ExpenseMiscellaneous/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpenseMiscellaneou expenseMiscellaneou = await db.ExpenseMiscellaneous.FindAsync(id);
            if (expenseMiscellaneou == null)
            {
                return HttpNotFound();
            }
            return View(expenseMiscellaneou);
        }

        // GET: ExpenseMiscellaneous/Create
        public ActionResult Create()
        {
            IQueryable<Project> projects;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            projects = from cc in db.Projects
                       where cc.projectUid == sessionProject
                       select cc;

            ViewBag.projectUid = new SelectList(projects, "projectUid", "project1");            
            return View();
        }

        // POST: ExpenseMiscellaneous/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "expenseMiscellaneousUid,projectUid,expenseMiscellaneous,expenseMiscellaneousDescription,total")] ExpenseMiscellaneou expenseMiscellaneou)
        {
            if (ModelState.IsValid)
            {
                expenseMiscellaneou.expenseMiscellaneousUid = Guid.NewGuid();
                db.ExpenseMiscellaneous.Add(expenseMiscellaneou);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", expenseMiscellaneou.projectUid);
            return View(expenseMiscellaneou);
        }

        // GET: ExpenseMiscellaneous/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpenseMiscellaneou expenseMiscellaneou = await db.ExpenseMiscellaneous.FindAsync(id);
            if (expenseMiscellaneou == null)
            {
                return HttpNotFound();
            }

            IQueryable<Project> projects;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            projects = from cc in db.Projects
                       where cc.projectUid == sessionProject
                       select cc;

            ViewBag.projectUid = new SelectList(projects, "projectUid", "project1", expenseMiscellaneou.projectUid);            
            return View(expenseMiscellaneou);
        }

        // POST: ExpenseMiscellaneous/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "expenseMiscellaneousUid,projectUid,expenseMiscellaneous,expenseMiscellaneousDescription,total")] ExpenseMiscellaneou expenseMiscellaneou)
        {
            if (ModelState.IsValid)
            {
                db.Entry(expenseMiscellaneou).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", expenseMiscellaneou.projectUid);
            return View(expenseMiscellaneou);
        }

        // GET: ExpenseMiscellaneous/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpenseMiscellaneou expenseMiscellaneou = await db.ExpenseMiscellaneous.FindAsync(id);
            if (expenseMiscellaneou == null)
            {
                return HttpNotFound();
            }
            return View(expenseMiscellaneou);
        }

        // POST: ExpenseMiscellaneous/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ExpenseMiscellaneou expenseMiscellaneou = await db.ExpenseMiscellaneous.FindAsync(id);
            db.ExpenseMiscellaneous.Remove(expenseMiscellaneou);
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
