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
    public class ContractorSchedulesController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ContractorSchedules
        public async Task<ActionResult> Index()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            IEnumerable<ContractorSchedule> contractorSchedules;
            if (User.IsInRole("Admin"))
            {
                contractorSchedules = from cc in db.ContractorSchedules                            
                                      where cc.MasterSchedule.projectUid == sessionProject
                                      select cc;
            }
            else
            {
                contractorSchedules = from cc in db.ContractorSchedules                            
                                      join cn in db.ContractorUsers on cc.contractorUid equals cn.contractorUid
                                      join cq in db.AspNetUsers on cn.aspNetUserUid equals cq.Id
                                      where cc.MasterSchedule.projectUid == sessionProject
                                      && cq.UserName == System.Web.HttpContext.Current.User.Identity.Name
                                      select cc;
            }     

            
            return View(contractorSchedules.ToList());
        }

        // GET: ContractorSchedules/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorSchedule contractorSchedule = await db.ContractorSchedules.FindAsync(id);
            if (contractorSchedule == null)
            {
                return HttpNotFound();
            }
            return View(contractorSchedule);
        }

        // GET: ContractorSchedules/Create
        public ActionResult Create()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName");
            ViewBag.masterScheduleUid = new SelectList(db.MasterSchedules.Where(c=>c.projectUid == sessionProject), "masterScheduleUid", "masterSchedule1");
            return View();
        }

        // POST: ContractorSchedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "contractorScheduleUid,masterScheduleUid,contractorUid,startDate")] ContractorSchedule contractorSchedule)
        {
            if (ModelState.IsValid)
            {
                contractorSchedule.contractorScheduleUid = Guid.NewGuid();
                db.ContractorSchedules.Add(contractorSchedule);
                foreach (var mst in db.MasterScheduleTasks.Where(c=>c.masterScheduleUid == contractorSchedule.masterScheduleUid))
                {
                    ContractorScheduleTask cst = new ContractorScheduleTask();
                    cst.contractorScheduleTaskUid = Guid.NewGuid();
                    cst.contractorScheduleUid = contractorSchedule.contractorScheduleUid;
                    cst.masterScheduleTaskUid = mst.masterScheduleTaskUid;
                    db.ContractorScheduleTasks.Add(cst);
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName", contractorSchedule.contractorUid);
            ViewBag.masterScheduleUid = new SelectList(db.MasterSchedules, "masterScheduleUid", "masterSchedule1", contractorSchedule.masterScheduleUid);
            return View(contractorSchedule);
        }

        // GET: ContractorSchedules/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorSchedule contractorSchedule = await db.ContractorSchedules.FindAsync(id);
            if (contractorSchedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName", contractorSchedule.contractorUid);
            ViewBag.masterScheduleUid = new SelectList(db.MasterSchedules.Where(c => c.projectUid == sessionProject), "masterScheduleUid", "masterSchedule1", contractorSchedule.masterScheduleUid);
            ViewBag.shiftUid = db.Shifts.OrderBy(c=>c.shift1);
            return View(contractorSchedule);
        }

        // POST: ContractorSchedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "contractorScheduleUid,masterScheduleUid,contractorUid,startDate")] ContractorSchedule contractorSchedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contractorSchedule).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName", contractorSchedule.contractorUid);
            ViewBag.masterScheduleUid = new SelectList(db.MasterSchedules, "masterScheduleUid", "masterSchedule1", contractorSchedule.masterScheduleUid);
            return View(contractorSchedule);
        }

        public async Task<ActionResult> SaveTask(string field, string identifier, string value)
        {
            try
            {                
                Guid id = new Guid(identifier);
                ContractorScheduleTask contractorScheduleTask = await db.ContractorScheduleTasks.FindAsync(id);                    
                
                if (contractorScheduleTask == null)
                {
                    return HttpNotFound();
                }

                if (field == "shift")
                {
                    Guid uid = new Guid(value);                    
                    contractorScheduleTask.shiftUid = uid;
                }
                else if(field == "taskStartDate")
                {
                    contractorScheduleTask.taskStartDate = Convert.ToDateTime(value);
                }
                else if (field == "taskEndDate")
                {
                    contractorScheduleTask.taskEndDate = Convert.ToDateTime(value);
                }
                
                db.SaveChanges();
            }
            catch (Exception e)
            {
                
                throw e;
            }
            
            
            
            return PartialView();
        }



        // GET: ContractorSchedules/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorSchedule contractorSchedule = await db.ContractorSchedules.FindAsync(id);
            if (contractorSchedule == null)
            {
                return HttpNotFound();
            }
            return View(contractorSchedule);
        }

        // POST: ContractorSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ContractorSchedule contractorSchedule = await db.ContractorSchedules.FindAsync(id);
            db.ContractorSchedules.Remove(contractorSchedule);
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
