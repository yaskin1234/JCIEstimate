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
    public class SalesOpportunityMilestonesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: SalesOpportunityMilestones
        public async Task<ActionResult> Index()
        {
            var salesOpportunityMilestones = db.SalesOpportunityMilestones.Include(s => s.SalesOpportunity).Include(s => s.Milestone);
            return View(await salesOpportunityMilestones.ToListAsync());
        }

        // GET: SalesOpportunityMilestones/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesOpportunityMilestone salesOpportunityMilestone = await db.SalesOpportunityMilestones.FindAsync(id);
            if (salesOpportunityMilestone == null)
            {
                return HttpNotFound();
            }
            return View(salesOpportunityMilestone);
        }

        public async Task<ActionResult> SaveIsCompleted(string id, string value)
        {

            SalesOpportunityMilestone som = db.SalesOpportunityMilestones.Find(Guid.Parse(id));

            if (value == "true")
            {
                som.isCompleted = true;
                db.Entry(som).State = EntityState.Modified;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    return Json("error: " + ex.Message);
                }
            }
            else
            {
                som.isCompleted = false;
                db.Entry(som).State = EntityState.Modified;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return Json("error: " + ex.Message);
                }

            }
            return View();
        }

        public async Task<ActionResult> SaveDateCompleted(string id, string value)
        {

            SalesOpportunityMilestone som = db.SalesOpportunityMilestones.Find(Guid.Parse(id));
            DateTime parsedDate;
            if (DateTime.TryParse(value, out parsedDate))
            {
                som.dateCompleted = parsedDate;
            }
            else
            {
                som.dateCompleted = null;
            }
            
            db.Entry(som).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Json("error: " + ex.Message);
            }

            return Json("success");
        }

        // GET: SalesOpportunityMilestones/Create
        public ActionResult Create()
        {
            ViewBag.salesOpportunityUid = new SelectList(db.SalesOpportunities, "salesOpportunityUid", "salesTeam1");
            ViewBag.salesOpportunityUid = db.SalesOpportunities.ToSelectList(c => c.Opportunity.opportunity1 + "-" + c.SalesTeam.salesTeam1, c => c.salesOpportunityUid.ToString(), "");
            ViewBag.milestoneUid = new SelectList(db.Milestones, "milestoneUid", "milestone1");
            return View();
        }

        // POST: SalesOpportunityMilestones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "salesOpportunityMilestoneUid,salesOpportunityUid,milestoneUid,isCompleted,dateCompleted")] SalesOpportunityMilestone salesOpportunityMilestone)
        {
            if (ModelState.IsValid)
            {
                salesOpportunityMilestone.salesOpportunityMilestoneUid = Guid.NewGuid();
                db.SalesOpportunityMilestones.Add(salesOpportunityMilestone);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.salesOpportunityUid = new SelectList(db.SalesOpportunities, "salesOpportunityUid", "salesTeam1", salesOpportunityMilestone.salesOpportunityUid);
            ViewBag.milestoneUid = new SelectList(db.Milestones, "milestoneUid", "milestone1", salesOpportunityMilestone.milestoneUid);
            return View(salesOpportunityMilestone);
        }

        // GET: SalesOpportunityMilestones/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesOpportunityMilestone salesOpportunityMilestone = await db.SalesOpportunityMilestones.FindAsync(id);
            if (salesOpportunityMilestone == null)
            {
                return HttpNotFound();
            }
            ViewBag.salesOpportunityUid = db.SalesOpportunities.ToSelectList(c => c.Opportunity.opportunity1 + "-" + c.SalesTeam.salesTeam1, c => c.salesOpportunityUid.ToString(), salesOpportunityMilestone.salesOpportunityUid.ToString());            
            ViewBag.milestoneUid = new SelectList(db.Milestones, "milestoneUid", "milestone1", salesOpportunityMilestone.milestoneUid);
            return View(salesOpportunityMilestone);
        }

        // POST: SalesOpportunityMilestones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "salesOpportunityMilestoneUid,salesOpportunityUid,milestoneUid,isCompleted,dateCompleted")] SalesOpportunityMilestone salesOpportunityMilestone)
        {
            if (ModelState.IsValid)
            {
                db.Entry(salesOpportunityMilestone).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.salesOpportunityUid = new SelectList(db.SalesOpportunities, "salesOpportunityUid", "aspNetUserUid", salesOpportunityMilestone.salesOpportunityUid);
            ViewBag.milestoneUid = new SelectList(db.Milestones, "milestoneUid", "milestone1", salesOpportunityMilestone.milestoneUid);
            return View(salesOpportunityMilestone);
        }

        // GET: SalesOpportunityMilestones/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesOpportunityMilestone salesOpportunityMilestone = await db.SalesOpportunityMilestones.FindAsync(id);
            if (salesOpportunityMilestone == null)
            {
                return HttpNotFound();
            }
            return View(salesOpportunityMilestone);
        }

        // POST: SalesOpportunityMilestones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            SalesOpportunityMilestone salesOpportunityMilestone = await db.SalesOpportunityMilestones.FindAsync(id);
            db.SalesOpportunityMilestones.Remove(salesOpportunityMilestone);
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
