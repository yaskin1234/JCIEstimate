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
    public class SalesOpportunityTasksController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: SalesOpportunityTasks
        public async Task<ActionResult> Index()
        {
            var salesOpportunityTasks = db.SalesOpportunityTasks.Include(s => s.SalesOpportunity);
            return View(await salesOpportunityTasks.ToListAsync());
        }
        

        public async Task<ActionResult> SaveIsCompleted(string id, string value)
        {

            SalesOpportunityTask sot = db.SalesOpportunityTasks.Find(Guid.Parse(id));

            if (value == "true")
            {
                sot.isCompleted = true;
                db.Entry(sot).State = EntityState.Modified;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            else
            {
                sot.isCompleted = false;
                db.Entry(sot).State = EntityState.Modified;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return View();
        }

        // GET: SalesOpportunityTasks/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesOpportunityTask salesOpportunityTask = await db.SalesOpportunityTasks.FindAsync(id);
            if (salesOpportunityTask == null)
            {
                return HttpNotFound();
            }
            return View(salesOpportunityTask);
        }

        // GET: SalesOpportunityTasks/Create
        public ActionResult Create()
        {
            ViewBag.salesOpportunityUid = new SelectList(db.SalesOpportunities, "salesOpportunityUid", "aspNetUserUid");
            return View();
        }

        // POST: SalesOpportunityTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "salesOpportunityTaskUid,salesOpportunityUid,SalesOpportunityTask1,week")] SalesOpportunityTask salesOpportunityTask)
        {
            if (ModelState.IsValid)
            {
                salesOpportunityTask.salesOpportunityTaskUid = Guid.NewGuid();
                db.SalesOpportunityTasks.Add(salesOpportunityTask);
                await db.SaveChangesAsync();
                return RedirectToAction("Edit", "SalesOpportunities", new { id = salesOpportunityTask.salesOpportunityUid });
            }

            ViewBag.salesOpportunityUid = new SelectList(db.SalesOpportunities, "salesOpportunityUid", "aspNetUserUid", salesOpportunityTask.salesOpportunityUid);
            return View(salesOpportunityTask);
        }

        // GET: SalesOpportunityTasks/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesOpportunityTask salesOpportunityTask = await db.SalesOpportunityTasks.FindAsync(id);
            if (salesOpportunityTask == null)
            {
                return HttpNotFound();
            }
            ViewBag.salesOpportunityUid = new SelectList(db.SalesOpportunities, "salesOpportunityUid", "aspNetUserUid", salesOpportunityTask.salesOpportunityUid);
            return View(salesOpportunityTask);
        }

        // POST: SalesOpportunityTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "salesOpportunityTaskUid,salesOpportunityUid,SalesOpportunityTask1,week")] SalesOpportunityTask salesOpportunityTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(salesOpportunityTask).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.salesOpportunityUid = new SelectList(db.SalesOpportunities, "salesOpportunityUid", "aspNetUserUid", salesOpportunityTask.salesOpportunityUid);
            return View(salesOpportunityTask);
        }

        // GET: SalesOpportunityTasks/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesOpportunityTask salesOpportunityTask = await db.SalesOpportunityTasks.FindAsync(id);
            if (salesOpportunityTask == null)
            {
                return HttpNotFound();
            }
            return View(salesOpportunityTask);
        }

        // POST: SalesOpportunityTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            
            SalesOpportunityTask salesOpportunityTask = await db.SalesOpportunityTasks.FindAsync(id);
            Guid salesOpGuid = salesOpportunityTask.salesOpportunityUid;
            db.SalesOpportunityTasks.Remove(salesOpportunityTask);
            await db.SaveChangesAsync();
            return RedirectToAction("Edit", "SalesOpportunities", salesOpGuid);
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
