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
    public class PECTasksController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: PECTasks
        public async Task<ActionResult> Index()
        {
            return View(await db.PECTasks.ToListAsync());
        }

        // GET: PECTasks/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PECTask pECTask = await db.PECTasks.FindAsync(id);
            if (pECTask == null)
            {
                return HttpNotFound();
            }
            return View(pECTask);
        }

        // GET: PECTasks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PECTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "pecTaskUid,pecTask1,behaviorIndicator")] PECTask pECTask)
        {
            if (ModelState.IsValid)
            {
                pECTask.pecTaskUid = Guid.NewGuid();
                db.PECTasks.Add(pECTask);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(pECTask);
        }

        // GET: PECTasks/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PECTask pECTask = await db.PECTasks.FindAsync(id);
            if (pECTask == null)
            {
                return HttpNotFound();
            }
            return View(pECTask);
        }

        // POST: PECTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "pecTaskUid,pecTask1,behaviorIndicator")] PECTask pECTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pECTask).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(pECTask);
        }

        // GET: PECTasks/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PECTask pECTask = await db.PECTasks.FindAsync(id);
            if (pECTask == null)
            {
                return HttpNotFound();
            }
            return View(pECTask);
        }

        // POST: PECTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            PECTask pECTask = await db.PECTasks.FindAsync(id);
            db.PECTasks.Remove(pECTask);
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
