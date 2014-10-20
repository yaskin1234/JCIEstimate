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
    public class LineOfWorksController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: LineOfWorks
        public async Task<ActionResult> Index()
        {
            return View(await db.LineOfWorks.ToListAsync());
        }

        // GET: LineOfWorks/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LineOfWork lineOfWork = await db.LineOfWorks.FindAsync(id);
            if (lineOfWork == null)
            {
                return HttpNotFound();
            }
            return View(lineOfWork);
        }

        // GET: LineOfWorks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LineOfWorks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "lineOfWorkUid,lineOfWork1,lineOfWorkDescription,behaviorIndicator")] LineOfWork lineOfWork)
        {
            if (ModelState.IsValid)
            {
                lineOfWork.lineOfWorkUid = Guid.NewGuid();
                db.LineOfWorks.Add(lineOfWork);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(lineOfWork);
        }

        // GET: LineOfWorks/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LineOfWork lineOfWork = await db.LineOfWorks.FindAsync(id);
            if (lineOfWork == null)
            {
                return HttpNotFound();
            }
            return View(lineOfWork);
        }

        // POST: LineOfWorks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "lineOfWorkUid,lineOfWork1,lineOfWorkDescription,behaviorIndicator")] LineOfWork lineOfWork)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lineOfWork).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(lineOfWork);
        }

        // GET: LineOfWorks/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LineOfWork lineOfWork = await db.LineOfWorks.FindAsync(id);
            if (lineOfWork == null)
            {
                return HttpNotFound();
            }
            return View(lineOfWork);
        }

        // POST: LineOfWorks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            LineOfWork lineOfWork = await db.LineOfWorks.FindAsync(id);
            db.LineOfWorks.Remove(lineOfWork);
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
