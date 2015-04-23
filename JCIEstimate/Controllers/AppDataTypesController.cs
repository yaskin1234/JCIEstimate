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
    public class AppDataTypesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: AppDataTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.AppDataTypes.ToListAsync());
        }

        // GET: AppDataTypes/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppDataType appDataType = await db.AppDataTypes.FindAsync(id);
            if (appDataType == null)
            {
                return HttpNotFound();
            }
            return View(appDataType);
        }

        // GET: AppDataTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AppDataTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "appDataTypeUid,appDataType1")] AppDataType appDataType)
        {
            if (ModelState.IsValid)
            {
                appDataType.appDataTypeUid = Guid.NewGuid();
                db.AppDataTypes.Add(appDataType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(appDataType);
        }

        // GET: AppDataTypes/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppDataType appDataType = await db.AppDataTypes.FindAsync(id);
            if (appDataType == null)
            {
                return HttpNotFound();
            }
            return View(appDataType);
        }

        // POST: AppDataTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "appDataTypeUid,appDataType1")] AppDataType appDataType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appDataType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(appDataType);
        }

        // GET: AppDataTypes/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppDataType appDataType = await db.AppDataTypes.FindAsync(id);
            if (appDataType == null)
            {
                return HttpNotFound();
            }
            return View(appDataType);
        }

        // POST: AppDataTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            AppDataType appDataType = await db.AppDataTypes.FindAsync(id);
            db.AppDataTypes.Remove(appDataType);
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
