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
    public class ToDoStatusController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ToDoStatus
        public async Task<ActionResult> Index()
        {
            return View(await db.ToDoStatus.ToListAsync());
        }

        // GET: ToDoStatus/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDoStatu toDoStatu = await db.ToDoStatus.FindAsync(id);
            if (toDoStatu == null)
            {
                return HttpNotFound();
            }
            return View(toDoStatu);
        }

        // GET: ToDoStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ToDoStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "toDoStatusUid,toDoStatus,toDoStatusDescription,behaviorIndicator")] ToDoStatu toDoStatu)
        {
            if (ModelState.IsValid)
            {
                toDoStatu.toDoStatusUid = Guid.NewGuid();
                db.ToDoStatus.Add(toDoStatu);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(toDoStatu);
        }

        // GET: ToDoStatus/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDoStatu toDoStatu = await db.ToDoStatus.FindAsync(id);
            if (toDoStatu == null)
            {
                return HttpNotFound();
            }
            return View(toDoStatu);
        }

        // POST: ToDoStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "toDoStatusUid,toDoStatus,toDoStatusDescription,behaviorIndicator")] ToDoStatu toDoStatu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(toDoStatu).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(toDoStatu);
        }

        // GET: ToDoStatus/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDoStatu toDoStatu = await db.ToDoStatus.FindAsync(id);
            if (toDoStatu == null)
            {
                return HttpNotFound();
            }
            return View(toDoStatu);
        }

        // POST: ToDoStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ToDoStatu toDoStatu = await db.ToDoStatus.FindAsync(id);
            db.ToDoStatus.Remove(toDoStatu);
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
