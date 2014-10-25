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
    public class ExpensePercentagesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ExpensePercentages
        public async Task<ActionResult> Index()        
        {
            IQueryable<ExpensePercentage> expensePercentages;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();            

            expensePercentages = from cc in db.ExpensePercentages
                                   where cc.projectUid == sessionProject
                                   select cc;
            var projectTotal = db.Estimates.Sum(e => e.laborBid + e.materialBid + e.bondAmount);
            expensePercentages = expensePercentages.Include(e => e.Project);
            ViewBag.projectTotal = projectTotal;
            return View(await expensePercentages.ToListAsync());
        }

        // GET: ExpensePercentages/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpensePercentage expensePercentage = await db.ExpensePercentages.FindAsync(id);
            if (expensePercentage == null)
            {
                return HttpNotFound();
            }
            return View(expensePercentage);
        }

        // GET: ExpensePercentages/Create
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

        // POST: ExpensePercentages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "expensePercentageUid,projectUid,expensePercentage1,expensePercentageDescription,percentage,isForActiveOnly")] ExpensePercentage expensePercentage)
        {
            if (ModelState.IsValid)
            {
                expensePercentage.expensePercentageUid = Guid.NewGuid();
                db.ExpensePercentages.Add(expensePercentage);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            IQueryable<Project> projects;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            projects = from cc in db.Projects
                       where cc.projectUid == sessionProject
                       select cc;

            ViewBag.projectUid = new SelectList(projects, "projectUid", "project1", expensePercentage.projectUid);            
            return View(expensePercentage);
        }

        // GET: ExpensePercentages/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpensePercentage expensePercentage = await db.ExpensePercentages.FindAsync(id);
            if (expensePercentage == null)
            {
                return HttpNotFound();
            }
            IQueryable<Project> projects;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            projects = from cc in db.Projects
                       where cc.projectUid == sessionProject
                       select cc;

            ViewBag.projectUid = new SelectList(projects, "projectUid", "project1");            
            return View(expensePercentage);
        }

        // POST: ExpensePercentages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "expensePercentageUid,projectUid,expensePercentage1,expensePercentageDescription,percentage,isForActiveOnly")] ExpensePercentage expensePercentage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(expensePercentage).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", expensePercentage.projectUid);
            return View(expensePercentage);
        }

        // GET: ExpensePercentages/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpensePercentage expensePercentage = await db.ExpensePercentages.FindAsync(id);
            if (expensePercentage == null)
            {
                return HttpNotFound();
            }
            return View(expensePercentage);
        }

        // POST: ExpensePercentages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ExpensePercentage expensePercentage = await db.ExpensePercentages.FindAsync(id);
            db.ExpensePercentages.Remove(expensePercentage);
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
