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
    public class ContractorDrawsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ContractorDraws
        public async Task<ActionResult> Index()
        {            
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            IQueryable<ContractorDraw> contractorDraws;
            if (User.IsInRole("Admin"))
            {
                contractorDraws = from cc in db.ContractorDraws
                                  where cc.projectUid == sessionProject
                                  select cc;
            }
            else
            {
                contractorDraws = from cc in db.ContractorDraws
                                  join cn in db.ContractorUsers on cc.contractorUid equals cn.contractorUid
                                  join cq in db.AspNetUsers on cn.aspNetUserUid equals cq.Id
                                  where cc.projectUid == sessionProject
                                  && cq.UserName == System.Web.HttpContext.Current.User.Identity.Name                                  
                                  select cc;
            }
            
                                      
            contractorDraws = contractorDraws.Include(c => c.Contractor).Include(c => c.Project);
            return View(await contractorDraws.ToListAsync());
        }

        // GET: ContractorDraws/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorDraw contractorDraw = await db.ContractorDraws.FindAsync(id);
            if (contractorDraw == null)
            {
                return HttpNotFound();
            }
            return View(contractorDraw);
        }

        // GET: ContractorDraws/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            var projects = from cc in db.Projects
                           where cc.projectUid == sessionProject
                           select cc;

            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName");
            ViewBag.projectUid = new SelectList(projects, "projectUid", "project1");
            return View();
        }

        // POST: ContractorDraws/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "contractorDrawUid,projectUid,contractorUid,dateCreated")] ContractorDraw contractorDraw)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            if (ModelState.IsValid)
            {
                contractorDraw.contractorDrawUid = Guid.NewGuid();
                contractorDraw.dateCreated = DateTime.Now;
                db.ContractorDraws.Add(contractorDraw);
                
                
                int count = 31;
                for (int i = 0; i < count; i++)
                {
                    ContractorDrawSchedule drawSchedule = new ContractorDrawSchedule();
                    drawSchedule.contractorDrawScheduleUid = Guid.NewGuid();
                    drawSchedule.contractorDrawUid = contractorDraw.contractorDrawUid;
                    drawSchedule.drawPeriod = i;
                    db.ContractorDrawSchedules.Add(drawSchedule);
                }
                
                

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName", contractorDraw.contractorUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", contractorDraw.projectUid);
            return View(contractorDraw);
        }

        // GET: EquipmentToDoes/SaveCheckedBox/5
        public async Task<ActionResult> SaveDrawScheduleAmount(string id, string value)
        {
            ContractorDrawSchedule cds = db.ContractorDrawSchedules.Find(Guid.Parse(id));
            db.Entry(cds).State = EntityState.Modified;
            int amount;
            if (int.TryParse(value, out amount))
            {
                cds.amount = amount;
                await db.SaveChangesAsync();
            }

            return Json(String.Format("{0:C0}", cds.ContractorDraw.ContractorDrawSchedules.Sum(c => c.amount)));
        }

        // GET: ContractorDraws/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            var projects = from cc in db.Projects
                           where cc.projectUid == sessionProject
                           select cc;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorDraw contractorDraw = await db.ContractorDraws.FindAsync(id);
            if (contractorDraw == null)
            {
                return HttpNotFound();
            }
            ViewBag.runningTotal = contractorDraw.ContractorDrawSchedules.Sum(c => c.amount);
            ViewBag.drawSchedules = contractorDraw.ContractorDrawSchedules.OrderBy(c => c.drawPeriod);
            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName", contractorDraw.contractorUid);
            ViewBag.projectUid = new SelectList(projects, "projectUid", "project1", contractorDraw.projectUid);
            return View(contractorDraw);
        }

        // POST: ContractorDraws/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "contractorDrawUid,projectUid,contractorUid,dateCreated")] ContractorDraw contractorDraw)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contractorDraw).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName", contractorDraw.contractorUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", contractorDraw.projectUid);
            return View(contractorDraw);
        }

        // GET: ContractorDraws/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorDraw contractorDraw = await db.ContractorDraws.FindAsync(id);
            if (contractorDraw == null)
            {
                return HttpNotFound();
            }
            return View(contractorDraw);
        }

        // POST: ContractorDraws/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ContractorDraw contractorDraw = await db.ContractorDraws.FindAsync(id);
            db.ContractorDraws.Remove(contractorDraw);
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
