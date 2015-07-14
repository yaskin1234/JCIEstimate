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
    public class ExpenseMonthliesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ExpenseMonthlies
        public async Task<ActionResult> Index()
        {
            var expenseMonthlies = db.ExpenseMonthlies.Include(e => e.Project);
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();            
            return View(await expenseMonthlies.Where(c => c.projectUid == sessionProject).OrderBy(c => c.expenseMonthly1).ToListAsync());
        }

        // GET: ExpenseMonthlies/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpenseMonthly expenseMonthly = await db.ExpenseMonthlies.FindAsync(id);
            if (expenseMonthly == null)
            {
                return HttpNotFound();
            }
            return View(expenseMonthly);
        }

        // GET: ExpenseMonthlies/Create
        public ActionResult Create()
        {

            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            ViewBag.projectUid = new SelectList(db.Projects.Where(m => m.projectUid == sessionProject), "projectUid", "project1");
            ViewBag.expenseTypeUid = new SelectList(db.ExpenseTypes, "expenseTypeUid", "expenseType1");     
            return View();
        }

        // POST: ExpenseMonthlies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "expenseMonthlyUid,projectUid,expenseMonthly1,expenseMonthlyDescription,ratePerDay,daysPerMonth,projectDurationInMonths,total,expenseTypeUid")] ExpenseMonthly expenseMonthly)
        {
            if (ModelState.IsValid)
            {
                expenseMonthly.expenseMonthlyUid = Guid.NewGuid();
                db.ExpenseMonthlies.Add(expenseMonthly);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", expenseMonthly.projectUid);
            return View(expenseMonthly);
        }

        // GET: ExpenseMonthlies/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpenseMonthly expenseMonthly = await db.ExpenseMonthlies.FindAsync(id);
            if (expenseMonthly == null)
            {
                return HttpNotFound();
            }
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", expenseMonthly.projectUid);
            ViewBag.expenseTypeUid = new SelectList(db.ExpenseTypes, "expenseTypeUid", "expenseType1", expenseMonthly.expenseTypeUid);     
            return View(expenseMonthly);
        }

        // POST: ExpenseMonthlies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "expenseMonthlyUid,projectUid,expenseMonthly1,expenseMonthlyDescription,ratePerDay,daysPerMonth,projectDurationInMonths,total,expenseTypeUid")] ExpenseMonthly expenseMonthly)
        {
            if (ModelState.IsValid)
            {
                db.Entry(expenseMonthly).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", expenseMonthly.projectUid);
            return View(expenseMonthly);
        }

        // GET: ExpenseMonthlies/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpenseMonthly expenseMonthly = await db.ExpenseMonthlies.FindAsync(id);
            if (expenseMonthly == null)
            {
                return HttpNotFound();
            }
            return View(expenseMonthly);
        }

        // POST: ExpenseMonthlies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ExpenseMonthly expenseMonthly = await db.ExpenseMonthlies.FindAsync(id);
            db.ExpenseMonthlies.Remove(expenseMonthly);
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
