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
    public class ExpenseConstructionDrawsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ExpenseConstructionDraws
        public async Task<ActionResult> Index()
        {
            var expenseConstructionDraws = db.ExpenseConstructionDraws.Include(e => e.ExpenseType).Include(e => e.Project);
            return View(await expenseConstructionDraws.ToListAsync());
        }

        // GET: ExpenseConstructionDraws/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpenseConstructionDraw expenseConstructionDraw = await db.ExpenseConstructionDraws.FindAsync(id);
            if (expenseConstructionDraw == null)
            {
                return HttpNotFound();
            }
            return View(expenseConstructionDraw);
        }

        // GET: ExpenseConstructionDraws/Create
        public ActionResult Create()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            var projects = from cc in db.Projects
                           where cc.projectUid == sessionProject
                           select cc;

            ViewBag.expenseTypeUid = new SelectList(db.ExpenseTypes.OrderBy(c=>c.expenseType1), "expenseTypeUid", "expenseType1");
            ViewBag.projectUid = new SelectList(projects, "projectUid", "project1");
            return View();
        }

        // POST: ExpenseConstructionDraws/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "expenseConstructionDrawUid,projectUid,expenseTypeUid,dateCreated")] ExpenseConstructionDraw expenseConstructionDraw)
        {
            if (ModelState.IsValid)
            {
                Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
                expenseConstructionDraw.expenseConstructionDrawUid = Guid.NewGuid();
                expenseConstructionDraw.dateCreated = DateTime.Now;
                db.ExpenseConstructionDraws.Add(expenseConstructionDraw);

                int count = 31;
                for (int i = 0; i < count; i++)
                {
                    ExpenseConstructionDrawSchedule drawSchedule = new ExpenseConstructionDrawSchedule();
                    drawSchedule.expenseConstructionDrawScheduleUid = Guid.NewGuid();
                    drawSchedule.expenseConstructionDrawUid = expenseConstructionDraw.expenseConstructionDrawUid;
                    drawSchedule.drawPeriod = i;
                    db.ExpenseConstructionDrawSchedules.Add(drawSchedule);
                }


                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.expenseTypeUid = new SelectList(db.ExpenseTypes, "expenseTypeUid", "expenseType1", expenseConstructionDraw.expenseTypeUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", expenseConstructionDraw.projectUid);
            return View(expenseConstructionDraw);
        }

        // GET: EquipmentToDoes/SaveCheckedBox/5
        public async Task<ActionResult> SaveExpenseConstructionDrawScheduleAmount(string id, string value)
        {
            ExpenseConstructionDrawSchedule cds = db.ExpenseConstructionDrawSchedules.Find(Guid.Parse(id));
            db.Entry(cds).State = EntityState.Modified;
            int amount;
            if (value == "")
            {
                value = "0";
            }
            if (int.TryParse(value, out amount))
            {
                cds.amount = amount;
                await db.SaveChangesAsync();
            }

            string returnString = String.Format("{0:C0}", cds.ExpenseConstructionDraw.ExpenseType.ExpenseConstructions.Sum(c => c.total)) + "|" + String.Format("{0:C0}", cds.ExpenseConstructionDraw.ExpenseConstructionDrawSchedules.Sum(c => c.amount)) + "|" + String.Format("{0:C0}", cds.ExpenseConstructionDraw.ExpenseType.ExpenseConstructions.Sum(c => c.total) - cds.ExpenseConstructionDraw.ExpenseConstructionDrawSchedules.Sum(c => c.amount));

            return Json(returnString);
        }

        // GET: ExpenseConstructionDraws/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            var projects = from cc in db.Projects
                           where cc.projectUid == sessionProject
                           select cc;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpenseConstructionDraw expenseConstructionDraw = await db.ExpenseConstructionDraws.FindAsync(id);
            if (expenseConstructionDraw == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.expenseAmount = expenseConstructionDraw.ExpenseType.ExpenseConstructions.Sum(c => c.total);
            ViewBag.runningTotal = expenseConstructionDraw.ExpenseConstructionDrawSchedules.Sum(c => c.amount);
            ViewBag.drawSchedules = expenseConstructionDraw.ExpenseConstructionDrawSchedules.OrderBy(c => c.drawPeriod);
            ViewBag.expenseTypeUid = new SelectList(db.ExpenseTypes, "expenseTypeUid", "expenseType1", expenseConstructionDraw.expenseTypeUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", expenseConstructionDraw.projectUid);
            ViewBag.startDate = expenseConstructionDraw.Project.startDate;
            return View(expenseConstructionDraw);
        }

        // POST: ExpenseConstructionDraws/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "expenseConstructionDrawUid,projectUid,expenseTypeUid,dateCreated")] ExpenseConstructionDraw expenseConstructionDraw)
        {
            if (ModelState.IsValid)
            {
                db.Entry(expenseConstructionDraw).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.expenseTypeUid = new SelectList(db.ExpenseTypes, "expenseTypeUid", "expenseConstruction1", expenseConstructionDraw.expenseTypeUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", expenseConstructionDraw.projectUid);
            return View(expenseConstructionDraw);
        }

        // GET: ExpenseConstructionDraws/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpenseConstructionDraw expenseConstructionDraw = await db.ExpenseConstructionDraws.FindAsync(id);
            if (expenseConstructionDraw == null)
            {
                return HttpNotFound();
            }
            return View(expenseConstructionDraw);
        }

        // POST: ExpenseConstructionDraws/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ExpenseConstructionDraw expenseConstructionDraw = await db.ExpenseConstructionDraws.FindAsync(id);
            db.ExpenseConstructionDraws.Remove(expenseConstructionDraw);
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
