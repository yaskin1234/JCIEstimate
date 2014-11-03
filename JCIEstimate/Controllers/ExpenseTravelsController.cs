﻿using System;
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
    public class ExpenseTravelsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ExpenseTravels
        public async Task<ActionResult> Index()
        {
            var expenseTravels = db.ExpenseTravels.Include(e => e.Project);
            return View(await expenseTravels.ToListAsync());
        }

        // GET: ExpenseTravels/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpenseTravel expenseTravel = await db.ExpenseTravels.FindAsync(id);
            if (expenseTravel == null)
            {
                return HttpNotFound();
            }
            return View(expenseTravel);
        }

        // GET: ExpenseTravels/Create
        public ActionResult Create()
        {
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1");
            return View();
        }

        // POST: ExpenseTravels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "expenseTravelUid,projectUid,expenseTravel1,expenseTravelDescription,ratePerDay,daysPerMonth,projectDurationInMonths,total")] ExpenseTravel expenseTravel)
        {
            if (ModelState.IsValid)
            {
                expenseTravel.expenseTravelUid = Guid.NewGuid();
                db.ExpenseTravels.Add(expenseTravel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", expenseTravel.projectUid);
            return View(expenseTravel);
        }

        // GET: ExpenseTravels/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpenseTravel expenseTravel = await db.ExpenseTravels.FindAsync(id);
            if (expenseTravel == null)
            {
                return HttpNotFound();
            }
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", expenseTravel.projectUid);
            return View(expenseTravel);
        }

        // POST: ExpenseTravels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "expenseTravelUid,projectUid,expenseTravel1,expenseTravelDescription,ratePerDay,daysPerMonth,projectDurationInMonths,total")] ExpenseTravel expenseTravel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(expenseTravel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", expenseTravel.projectUid);
            return View(expenseTravel);
        }

        // GET: ExpenseTravels/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpenseTravel expenseTravel = await db.ExpenseTravels.FindAsync(id);
            if (expenseTravel == null)
            {
                return HttpNotFound();
            }
            return View(expenseTravel);
        }

        // POST: ExpenseTravels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ExpenseTravel expenseTravel = await db.ExpenseTravels.FindAsync(id);
            db.ExpenseTravels.Remove(expenseTravel);
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