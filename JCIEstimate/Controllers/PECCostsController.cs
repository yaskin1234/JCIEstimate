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
    public class PECCostsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: PECCosts
        public async Task<ActionResult> Index()
        {
            var pECCosts = db.PECCosts.Include(p => p.AspNetUser).Include(p => p.PECExpenseType).Include(p => p.Week).Include(p => p.PECTask).Include(p => p.Project);
            return View(await pECCosts.ToListAsync());
        }

        // GET: PECCosts/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PECCost pECCost = await db.PECCosts.FindAsync(id);
            if (pECCost == null)
            {
                return HttpNotFound();
            }
            return View(pECCost);
        }

        // GET: PECCosts/Create
        public ActionResult Create()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            ViewBag.aspNetUserUid = db.AspNetUsers.ToSelectList(c=>c.Email, c=>c.Id.ToString(), "").OrderBy(c=>c.Value);            
            ViewBag.pecExpenseTypeUid = db.PECExpenseTypes.ToSelectList(c => c.pecExpenseType1, c => c.pecExpenseTypeUid.ToString(), "").OrderBy(c => c.Text);
            ViewBag.weekUid = db.Weeks.OrderBy(c => c.startDate).ToSelectList(c => Convert.ToDateTime(c.startDate).ToString("yyyy-MM-dd") + " to " + Convert.ToDateTime(c.endDate).ToString("yyyy-MM-dd"), c => c.WeekUid.ToString(), "");
            ViewBag.pecTaskUid = db.PECTasks.ToSelectList(c => c.pecTask1, c => c.pecTaskUid.ToString(), "").OrderBy(c => c.Text);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1").Where(c => c.Value == sessionProject.ToString());
            return View();
        }

        // POST: PECCosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "pecCostUid,aspNetUserUid,projectUid,weekUid,pecTaskUid,pecExpenseTypeUid,quantity")] PECCost pECCost)
        {
            if (ModelState.IsValid)
            {
                pECCost.pecCostUid = Guid.NewGuid();
                db.PECCosts.Add(pECCost);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            ViewBag.aspNetUserUid = db.AspNetUsers.ToSelectList(c => c.Email, c => c.Id.ToString(), "").OrderBy(c => c.Value);
            ViewBag.pecExpenseTypeUid = db.PECExpenseTypes.ToSelectList(c => c.pecExpenseType1, c => c.pecExpenseTypeUid.ToString(), "").OrderBy(c => c.Text);
            ViewBag.weekUid = db.Weeks.OrderBy(c => c.startDate).ToSelectList(c => Convert.ToDateTime(c.startDate).ToString("yyyy-MM-dd") + " to " + Convert.ToDateTime(c.endDate).ToString("yyyy-MM-dd"), c => c.WeekUid.ToString(), "");
            ViewBag.pecTaskUid = db.PECTasks.ToSelectList(c => c.pecTask1, c => c.pecTaskUid.ToString(), "").OrderBy(c => c.Text);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1").Where(c => c.Value == sessionProject.ToString());
            return View(pECCost);
        }

        // GET: PECCosts/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PECCost pECCost = await db.PECCosts.FindAsync(id);
            if (pECCost == null)
            {
                return HttpNotFound();
            }
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            ViewBag.aspNetUserUid = db.AspNetUsers.ToSelectList(c => c.Email, c => c.Id.ToString(), pECCost.aspNetUserUid.ToString()).OrderBy(c => c.Value);
            ViewBag.pecExpenseTypeUid = db.PECExpenseTypes.ToSelectList(c => c.pecExpenseType1, c => c.pecExpenseTypeUid.ToString(), pECCost.pecExpenseTypeUid.ToString()).OrderBy(c => c.Text);            
            ViewBag.weekUid = db.Weeks.OrderBy(c => c.startDate).ToSelectList(c => Convert.ToDateTime(c.startDate).ToString("yyyy-MM-dd") + " to " + Convert.ToDateTime(c.endDate).ToString("yyyy-MM-dd"), c => c.WeekUid.ToString(), pECCost.weekUid.ToString());
            ViewBag.pecTaskUid = db.PECTasks.ToSelectList(c => c.pecTask1, c => c.pecTaskUid.ToString(), pECCost.pecTaskUid.ToString()).OrderBy(c => c.Text);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1").Where(c => c.Value == sessionProject.ToString());
            return View(pECCost);
        }

        // POST: PECCosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "pecCostUid,aspNetUserUid,projectUid,weekUid,pecTaskUid,pecExpenseTypeUid,quantity")] PECCost pECCost)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pECCost).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            ViewBag.aspNetUserUid = db.AspNetUsers.ToSelectList(c => c.Email, c => c.Id.ToString(), pECCost.aspNetUserUid.ToString()).OrderBy(c => c.Value);
            ViewBag.pecExpenseTypeUid = db.PECExpenseTypes.ToSelectList(c => c.pecExpenseType1, c => c.pecExpenseTypeUid.ToString(), pECCost.pecExpenseTypeUid.ToString()).OrderBy(c => c.Text);
            ViewBag.weekUid = db.Weeks.OrderBy(c => c.startDate).ToSelectList(c => Convert.ToDateTime(c.startDate).ToString("yyyy-MM-dd") + " to " + Convert.ToDateTime(c.endDate).ToString("yyyy-MM-dd"), c => c.WeekUid.ToString(), pECCost.weekUid.ToString());
            ViewBag.pecTaskUid = db.PECTasks.ToSelectList(c => c.pecTask1, c => c.pecTaskUid.ToString(), pECCost.pecTaskUid.ToString()).OrderBy(c => c.Text);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1").Where(c => c.Value == sessionProject.ToString());
            return View(pECCost);
        }

        // GET: PECCosts/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PECCost pECCost = await db.PECCosts.FindAsync(id);
            if (pECCost == null)
            {
                return HttpNotFound();
            }
            return View(pECCost);
        }

        // POST: PECCosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            PECCost pECCost = await db.PECCosts.FindAsync(id);
            db.PECCosts.Remove(pECCost);
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
