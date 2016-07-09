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
    public class UserExpenseStatusController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: UserExpenseStatus
        public async Task<ActionResult> Index()
        {
            return View(await db.UserExpenseStatus.ToListAsync());
        }

        // GET: UserExpenseStatus/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserExpenseStatu userExpenseStatu = await db.UserExpenseStatus.FindAsync(id);
            if (userExpenseStatu == null)
            {
                return HttpNotFound();
            }
            return View(userExpenseStatu);
        }

        // GET: UserExpenseStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserExpenseStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "userExpenseStatusUid,userExpenseStatus,behaviorIndicator")] UserExpenseStatu userExpenseStatu)
        {
            if (ModelState.IsValid)
            {
                userExpenseStatu.userExpenseStatusUid = Guid.NewGuid();
                db.UserExpenseStatus.Add(userExpenseStatu);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(userExpenseStatu);
        }

        // GET: UserExpenseStatus/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserExpenseStatu userExpenseStatu = await db.UserExpenseStatus.FindAsync(id);
            if (userExpenseStatu == null)
            {
                return HttpNotFound();
            }
            return View(userExpenseStatu);
        }

        // POST: UserExpenseStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "userExpenseStatusUid,userExpenseStatus,behaviorIndicator")] UserExpenseStatu userExpenseStatu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userExpenseStatu).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(userExpenseStatu);
        }

        // GET: UserExpenseStatus/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserExpenseStatu userExpenseStatu = await db.UserExpenseStatus.FindAsync(id);
            if (userExpenseStatu == null)
            {
                return HttpNotFound();
            }
            return View(userExpenseStatu);
        }

        // POST: UserExpenseStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            UserExpenseStatu userExpenseStatu = await db.UserExpenseStatus.FindAsync(id);
            db.UserExpenseStatus.Remove(userExpenseStatu);
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
