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
    public class SalesTeamsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: SalesTeams
        public async Task<ActionResult> Index()
        {
            return View(await db.SalesTeams.OrderBy(c=>c.salesTeam1).ToListAsync());
        }

        // GET: SalesTeams/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesTeam salesTeam = await db.SalesTeams.FindAsync(id);
            if (salesTeam == null)
            {
                return HttpNotFound();
            }
            return View(salesTeam);
        }

        // GET: SalesTeams/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SalesTeams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "salesTeamUid,salesTeam1,behaviorIndicator")] SalesTeam salesTeam)
        {
            if (ModelState.IsValid)
            {
                salesTeam.salesTeamUid = Guid.NewGuid();
                db.SalesTeams.Add(salesTeam);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(salesTeam);
        }

        // GET: SalesTeams/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesTeam salesTeam = await db.SalesTeams.FindAsync(id);
            if (salesTeam == null)
            {
                return HttpNotFound();
            }
            return View(salesTeam);
        }

        // POST: SalesTeams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "salesTeamUid,salesTeam1,behaviorIndicator")] SalesTeam salesTeam)
        {
            if (ModelState.IsValid)
            {
                db.Entry(salesTeam).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(salesTeam);
        }

        // GET: SalesTeams/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesTeam salesTeam = await db.SalesTeams.FindAsync(id);
            if (salesTeam == null)
            {
                return HttpNotFound();
            }
            return View(salesTeam);
        }

        // POST: SalesTeams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            SalesTeam salesTeam = await db.SalesTeams.FindAsync(id);
            db.SalesTeams.Remove(salesTeam);
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
