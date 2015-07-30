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
    public class CompletionCategoriesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: CompletionCategories
        public async Task<ActionResult> Index()
        {
            return View(await db.CompletionCategories.ToListAsync());
        }

        // GET: CompletionCategories/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompletionCategory completionCategory = await db.CompletionCategories.FindAsync(id);
            if (completionCategory == null)
            {
                return HttpNotFound();
            }
            return View(completionCategory);
        }

        // GET: CompletionCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CompletionCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "completionCategoryUid,completionCategory1,behaviorIndicator")] CompletionCategory completionCategory)
        {
            if (ModelState.IsValid)
            {
                completionCategory.completionCategoryUid = Guid.NewGuid();
                db.CompletionCategories.Add(completionCategory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(completionCategory);
        }

        // GET: CompletionCategories/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompletionCategory completionCategory = await db.CompletionCategories.FindAsync(id);
            if (completionCategory == null)
            {
                return HttpNotFound();
            }
            return View(completionCategory);
        }

        // POST: CompletionCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "completionCategoryUid,completionCategory1,behaviorIndicator")] CompletionCategory completionCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(completionCategory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(completionCategory);
        }

        // GET: CompletionCategories/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompletionCategory completionCategory = await db.CompletionCategories.FindAsync(id);
            if (completionCategory == null)
            {
                return HttpNotFound();
            }
            return View(completionCategory);
        }

        // POST: CompletionCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            CompletionCategory completionCategory = await db.CompletionCategories.FindAsync(id);
            db.CompletionCategories.Remove(completionCategory);
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
