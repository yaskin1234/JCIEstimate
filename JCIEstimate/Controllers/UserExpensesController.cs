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
    public class UserExpensesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: UserExpenses
        public async Task<ActionResult> Index()
        {
            var userExpenses = db.UserExpenses.Include(u => u.AspNetUser).Include(u => u.UserExpenseStatu).Include(u => u.UserExpenseType).OrderBy(c=>c.AspNetUser.Email).ThenBy(c=>c.date);
            return View(await userExpenses.ToListAsync());
        }

        // GET: UserExpenses/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserExpense userExpense = await db.UserExpenses.FindAsync(id);
            if (userExpense == null)
            {
                return HttpNotFound();
            }
            return View(userExpense);
        }

        // GET: UserExpenses/Create
        public ActionResult Create()
        {
            ViewBag.aspNetUserUid = db.AspNetUsers.ToSelectList(c=>c.Email, c=>c.Id, "").OrderBy(c=>c.Text);
            ViewBag.userExpenseStatusUid = db.UserExpenseStatus.ToSelectList(c=>c.userExpenseStatus, c=>c.userExpenseStatusUid.ToString(), "").OrderBy(c=>c.Text);
            ViewBag.userExpenseTypeUid = db.UserExpenseTypes.ToSelectList(c => c.UserExpenseType1, c => c.userExpenseTypeUid.ToString(), "").OrderBy(c => c.Text);
            return View();
        }

        // POST: UserExpenses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "userExpenseUid,date,aspNetUserUid,userExpense1,userExpenseStatusUid,userExpenseTypeUid,amount")] UserExpense userExpense)
        {
            if (ModelState.IsValid)
            {
                userExpense.userExpenseUid = Guid.NewGuid();
                db.UserExpenses.Add(userExpense);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.aspNetUserUid = new SelectList(db.AspNetUsers, "Id", "Email", userExpense.aspNetUserUid);
            ViewBag.userExpenseStatusUid = new SelectList(db.UserExpenseStatus, "userExpenseStatusUid", "userExpenseStatus", userExpense.userExpenseStatusUid);
            ViewBag.userExpenseTypeUid = new SelectList(db.UserExpenseTypes, "userExpenseTypeUid", "UserExpenseType1", userExpense.userExpenseTypeUid);
            return View(userExpense);
        }

        // GET: UserExpenses/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserExpense userExpense = await db.UserExpenses.FindAsync(id);
            if (userExpense == null)
            {
                return HttpNotFound();
            }
            ViewBag.aspNetUserUid = new SelectList(db.AspNetUsers, "Id", "Email", userExpense.aspNetUserUid);
            ViewBag.userExpenseStatusUid = new SelectList(db.UserExpenseStatus, "userExpenseStatusUid", "userExpenseStatus", userExpense.userExpenseStatusUid);
            ViewBag.userExpenseTypeUid = new SelectList(db.UserExpenseTypes, "userExpenseTypeUid", "UserExpenseType1", userExpense.userExpenseTypeUid);
            return View(userExpense);
        }

        // POST: UserExpenses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "userExpenseUid,date,aspNetUserUid,userExpense1,userExpenseStatusUid,userExpenseTypeUid,amount")] UserExpense userExpense)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userExpense).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.aspNetUserUid = new SelectList(db.AspNetUsers, "Id", "Email", userExpense.aspNetUserUid);
            ViewBag.userExpenseStatusUid = new SelectList(db.UserExpenseStatus, "userExpenseStatusUid", "userExpenseStatus", userExpense.userExpenseStatusUid);
            ViewBag.userExpenseTypeUid = new SelectList(db.UserExpenseTypes, "userExpenseTypeUid", "UserExpenseType1", userExpense.userExpenseTypeUid);
            return View(userExpense);
        }

        // GET: UserExpenses/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserExpense userExpense = await db.UserExpenses.FindAsync(id);
            if (userExpense == null)
            {
                return HttpNotFound();
            }
            return View(userExpense);
        }

        // POST: UserExpenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            UserExpense userExpense = await db.UserExpenses.FindAsync(id);
            db.UserExpenses.Remove(userExpense);
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
