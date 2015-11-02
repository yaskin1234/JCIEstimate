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
    public class SalesTeamMembersController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: SalesTeamMembers
        public async Task<ActionResult> Index()
        {
            var salesTeamMembers = db.SalesTeamMembers.OrderBy(c=>c.SalesTeam.salesTeam1).Include(s => s.AspNetUser).Include(s => s.SalesTeam);
            return View(await salesTeamMembers.ToListAsync());
        }

        // GET: SalesTeamMembers/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesTeamMember salesTeamMember = await db.SalesTeamMembers.FindAsync(id);
            if (salesTeamMember == null)
            {
                return HttpNotFound();
            }
            return View(salesTeamMember);
        }

        // GET: SalesTeamMembers/Create
        public ActionResult Create()
        {
            ViewBag.aspNetUserUid = new SelectList(db.AspNetUsers.OrderBy(c=>c.Email), "Id", "Email");
            ViewBag.salesTeamUid = new SelectList(db.SalesTeams, "salesTeamUid", "salesTeam1");
            return View();
        }

        // POST: SalesTeamMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "salesTeamMemberUid,aspNetUserUid,salesTeamUid")] SalesTeamMember salesTeamMember)
        {
            if (ModelState.IsValid)
            {
                salesTeamMember.salesTeamMemberUid = Guid.NewGuid();
                db.SalesTeamMembers.Add(salesTeamMember);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.aspNetUserUid = new SelectList(db.AspNetUsers.OrderBy(c=>c.Email), "Id", "Email", salesTeamMember.aspNetUserUid);
            ViewBag.salesTeamUid = new SelectList(db.SalesTeams, "salesTeamUid", "salesTeam1", salesTeamMember.salesTeamUid);
            return View(salesTeamMember);
        }

        // GET: SalesTeamMembers/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesTeamMember salesTeamMember = await db.SalesTeamMembers.FindAsync(id);
            if (salesTeamMember == null)
            {
                return HttpNotFound();
            }
            ViewBag.aspNetUserUid = new SelectList(db.AspNetUsers.OrderBy(c=>c.Email), "Id", "Email", salesTeamMember.aspNetUserUid);
            ViewBag.salesTeamUid = new SelectList(db.SalesTeams, "salesTeamUid", "salesTeam1", salesTeamMember.salesTeamUid);
            return View(salesTeamMember);
        }

        // POST: SalesTeamMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "salesTeamMemberUid,aspNetUserUid,salesTeamUid")] SalesTeamMember salesTeamMember)
        {
            if (ModelState.IsValid)
            {
                db.Entry(salesTeamMember).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.aspNetUserUid = new SelectList(db.AspNetUsers.OrderBy(c=>c.Email), "Id", "Email", salesTeamMember.aspNetUserUid);
            ViewBag.salesTeamUid = new SelectList(db.SalesTeams, "salesTeamUid", "salesTeam1", salesTeamMember.salesTeamUid);
            return View(salesTeamMember);
        }

        // GET: SalesTeamMembers/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesTeamMember salesTeamMember = await db.SalesTeamMembers.FindAsync(id);
            if (salesTeamMember == null)
            {
                return HttpNotFound();
            }
            return View(salesTeamMember);
        }

        // POST: SalesTeamMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            SalesTeamMember salesTeamMember = await db.SalesTeamMembers.FindAsync(id);
            db.SalesTeamMembers.Remove(salesTeamMember);
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
