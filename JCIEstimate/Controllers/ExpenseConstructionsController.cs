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
    public class ExpenseConstructionsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ExpenseConstructions
        public async Task<ActionResult> Index()
        {
            IQueryable<ExpenseConstruction> expenseConstructions;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            
            expenseConstructions = from cc in db.ExpenseConstructions
                   where cc.projectUid == sessionProject
                   select cc;
            expenseConstructions = expenseConstructions.Include(e => e.Interval).Include(e => e.Project);            
            return View(await expenseConstructions.ToListAsync());
        }

        // GET: ExpenseConstructions/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpenseConstruction expenseConstruction = await db.ExpenseConstructions.FindAsync(id);
            if (expenseConstruction == null)
            {
                return HttpNotFound();
            }
            return View(expenseConstruction);
        }

        // GET: ExpenseConstructions/Create
        public ActionResult Create()
        {

            IQueryable<Project> projects;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            projects = from cc in db.Projects
                        where cc.projectUid == sessionProject
                        select cc;

            ViewBag.projectUid = new SelectList(projects, "projectUid", "project1");
            ViewBag.intervalUid = new SelectList(db.Intervals, "intervalUid", "interval1");
            ViewBag.expenseTypeUid = new SelectList(db.ExpenseTypes, "expenseTypeUid", "expenseType1");            
            return View();
        }

        // POST: ExpenseConstructions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "expenseConstructionUid,projectUid,expenseConstruction1,expenseConstructionDescription,rate,intervalUid,quantity,total,expenseTypeUid")] ExpenseConstruction expenseConstruction)
        {
            if (ModelState.IsValid)
            {
                expenseConstruction.expenseConstructionUid = Guid.NewGuid();
                db.ExpenseConstructions.Add(expenseConstruction);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.intervalUid = new SelectList(db.Intervals, "intervalUid", "interval1", expenseConstruction.intervalUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", expenseConstruction.projectUid);
            return View(expenseConstruction);
        }

        // GET: ExpenseConstructions/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpenseConstruction expenseConstruction = await db.ExpenseConstructions.FindAsync(id);
            if (expenseConstruction == null)
            {
                return HttpNotFound();
            }

            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            var projects = from cc in db.Projects
                       where cc.projectUid == sessionProject
                       select cc;

            ViewBag.projectUid = new SelectList(projects, "projectUid", "project1", expenseConstruction.projectUid);
            ViewBag.intervalUid = new SelectList(db.Intervals, "intervalUid", "interval1", expenseConstruction.intervalUid);
            ViewBag.expenseTypeUid = new SelectList(db.ExpenseTypes, "expenseTypeUid", "expenseType1", expenseConstruction.expenseTypeUid);            
            return View(expenseConstruction);
        }

        // POST: ExpenseConstructions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "expenseConstructionUid,projectUid,expenseConstruction1,expenseConstructionDescription,rate,intervalUid,quantity,total,expenseTypeUid")] ExpenseConstruction expenseConstruction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(expenseConstruction).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.intervalUid = new SelectList(db.Intervals, "intervalUid", "interval1", expenseConstruction.intervalUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", expenseConstruction.projectUid);
            return View(expenseConstruction);
        }

        // GET: ExpenseConstructions/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpenseConstruction expenseConstruction = await db.ExpenseConstructions.FindAsync(id);
            if (expenseConstruction == null)
            {
                return HttpNotFound();
            }
            return View(expenseConstruction);
        }

        // POST: ExpenseConstructions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ExpenseConstruction expenseConstruction = await db.ExpenseConstructions.FindAsync(id);
            db.ExpenseConstructions.Remove(expenseConstruction);
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
