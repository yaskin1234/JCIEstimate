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
    public class ECMsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();        

        // GET: ECMs
        public async Task<ActionResult> Index()
        {
            IQueryable<ECM> ecms;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            ecms = from cc in db.ECMs
                        where cc.projectUid == sessionProject
                        orderby cc.ecmNumber
                        select cc;
            var eCMs = ecms.Include(e => e.Project);
            return View(await eCMs.ToListAsync());
        }

        public async Task<ActionResult> ToggleEquipmentForScope()
        {
            IQueryable<ECM> ecms;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            ecms = from cc in db.ECMs                   
                   where cc.projectUid == sessionProject                        
                   orderby cc.ecmNumber
                   select cc;
            var eCMs = ecms.Include(e => e.Project);
            return View(await eCMs.ToListAsync());
        }

        

        // GET: ECMs/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ECM eCM = await db.ECMs.FindAsync(id);
            if (eCM == null)
            {
                return HttpNotFound();
            }
            return View(eCM);
        }

        // GET: ECMs/Create
        public ActionResult Create()
        {
            IQueryable<Project> projects;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            projects = from cc in db.Projects
                   where cc.projectUid == sessionProject
                   select cc;
            ViewBag.projectUid = new SelectList(projects, "projectUid", "project1");
            return View();
        }

        // POST: ECMs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ecmUid,ecmNumber,ecmDescription,ecmString,projectUid,scopeOfWorkNote,scopeOfWorkNote2,scopeOfWorkNote3,scopeOfWorkNote4,scopeOfWorkNote5,scopeOfWorkNote6,scopeOfWorkNote7,scopeOfWorkNote8,scopeOfWorkNote9,scopeOfWorkNote10,scopeOfWorkNote11,scopeOfWorkNote12,scopeOfWorkNote13,scopeOfWorkNote14,scopeOfWorkNote15")] ECM eCM)
        {
            if (ModelState.IsValid)
            {
                eCM.ecmUid = Guid.NewGuid();
                db.ECMs.Add(eCM);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", eCM.projectUid);
            return View(eCM);
        }

        // GET: ECMs/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ECM eCM = await db.ECMs.FindAsync(id);
            if (eCM == null)
            {
                return HttpNotFound();
            }

            IQueryable<Project> projects;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            projects = from cc in db.Projects
                       where cc.projectUid == sessionProject
                       select cc;
            ViewBag.projectUid = new SelectList(projects, "projectUid", "project1", eCM.projectUid);            
            return View(eCM);
        }

        // POST: ECMs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ecmUid,ecmNumber,ecmDescription,ecmString,projectUid,scopeOfWorkNote,scopeOfWorkNote2,scopeOfWorkNote3,scopeOfWorkNote4,scopeOfWorkNote5,scopeOfWorkNote6,scopeOfWorkNote7,scopeOfWorkNote8,scopeOfWorkNote9,scopeOfWorkNote10,scopeOfWorkNote11,scopeOfWorkNote12,scopeOfWorkNote13,scopeOfWorkNote14,scopeOfWorkNote15")] ECM eCM)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eCM).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", eCM.projectUid);
            return View(eCM);
        }

        // GET: ECMs/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ECM eCM = await db.ECMs.FindAsync(id);
            if (eCM == null)
            {
                return HttpNotFound();
            }
            return View(eCM);
        }

        // POST: ECMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ECM eCM = await db.ECMs.FindAsync(id);
            db.ECMs.Remove(eCM);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> GetEquipmentForECM(Guid ecmUid)
        {
            ECM eCM = await db.ECMs.FindAsync(ecmUid);
            ViewBag.ecms = eCM;
            ViewBag.equipments = eCM.Equipments;

            return PartialView(eCM.Equipments);
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
