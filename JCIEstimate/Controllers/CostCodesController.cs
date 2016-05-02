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
    public class CostCodesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: /CostCodes/
        public async Task<ActionResult> Index()
        {
            return View(await db.CostCodes.OrderBy(c=>c.costCode1).ToListAsync());
        }

        // GET: /CostCodes/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CostCode costcode = await db.CostCodes.FindAsync(id);
            if (costcode == null)
            {
                return HttpNotFound();
            }
            return View(costcode);
        }

        // GET: /CostCodes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /CostCodes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="costCodeUid,costCode1,costCodeDescription,jciCodeDescription")] CostCode costcode)
        {
            if (ModelState.IsValid)
            {
                costcode.costCodeUid = Guid.NewGuid();
                db.CostCodes.Add(costcode);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(costcode);
        }

        // GET: /CostCodes/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CostCode costcode = await db.CostCodes.FindAsync(id);
            if (costcode == null)
            {
                return HttpNotFound();
            }
            return View(costcode);
        }

        // POST: /CostCodes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="costCodeUid,costCode1,costCodeDescription,jciCodeDescription")] CostCode costcode)
        {
            if (ModelState.IsValid)
            {
                db.Entry(costcode).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(costcode);
        }

        // GET: /CostCodes/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CostCode costcode = await db.CostCodes.FindAsync(id);
            if (costcode == null)
            {
                return HttpNotFound();
            }
            return View(costcode);
        }

        // POST: /CostCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            CostCode costcode = await db.CostCodes.FindAsync(id);
            db.CostCodes.Remove(costcode);
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
