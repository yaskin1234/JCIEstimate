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
    public class UserExpenseTypesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: UserExpenseTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.UserExpenseTypes.ToListAsync());
        }

        // GET: UserExpenseTypes/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserExpenseType userExpenseType = await db.UserExpenseTypes.FindAsync(id);
            if (userExpenseType == null)
            {
                return HttpNotFound();
            }
            return View(userExpenseType);
        }

        // GET: UserExpenseTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserExpenseTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "userExpenseTypeUid,UserExpenseType1,behaviorIndicator")] UserExpenseType userExpenseType)
        {
            if (ModelState.IsValid)
            {
                userExpenseType.userExpenseTypeUid = Guid.NewGuid();
                db.UserExpenseTypes.Add(userExpenseType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(userExpenseType);
        }

        // GET: UserExpenseTypes/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserExpenseType userExpenseType = await db.UserExpenseTypes.FindAsync(id);
            if (userExpenseType == null)
            {
                return HttpNotFound();
            }
            return View(userExpenseType);
        }

        // POST: UserExpenseTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "userExpenseTypeUid,UserExpenseType1,behaviorIndicator")] UserExpenseType userExpenseType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userExpenseType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(userExpenseType);
        }

        // GET: UserExpenseTypes/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserExpenseType userExpenseType = await db.UserExpenseTypes.FindAsync(id);
            if (userExpenseType == null)
            {
                return HttpNotFound();
            }
            return View(userExpenseType);
        }

        // POST: UserExpenseTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            UserExpenseType userExpenseType = await db.UserExpenseTypes.FindAsync(id);
            db.UserExpenseTypes.Remove(userExpenseType);
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
