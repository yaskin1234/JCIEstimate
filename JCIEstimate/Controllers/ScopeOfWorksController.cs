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
using System.IO;
using System.Data.Entity.Validation;

namespace JCIEstimate.Controllers
{
    public class ScopeOfWorksController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ScopeOfWorks
        public async Task<ActionResult> Index()
        {
            IQueryable<ScopeOfWork> scopeOfWorks;
            
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            scopeOfWorks = from cc in db.ScopeOfWorks
                   where cc.projectUid == sessionProject
                   orderby cc.Category.category1, cc.documentName
                   select cc;

            scopeOfWorks = scopeOfWorks.Include(s => s.Category).Include(s => s.Project).OrderBy(c=>c.scopeOfWork1).ThenBy(c=>c.scopeOfWorkDescription);
            return View(await scopeOfWorks.ToListAsync());
        }

        // GET: ScopeOfWorks/getScopeDocument/5
        public async Task<ActionResult> GetScopeDocument(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScopeOfWork scopeOfWork = await db.ScopeOfWorks.FindAsync(id);
            if (scopeOfWork == null)
            {
                return HttpNotFound();
            }
            return View(scopeOfWork);
        }

        // GET: ScopeOfWorks/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScopeOfWork scopeOfWork = await db.ScopeOfWorks.FindAsync(id);
            if (scopeOfWork == null)
            {
                return HttpNotFound();
            }
            return View(scopeOfWork);
        }

        // GET: ScopeOfWorks/Create
        [Authorize(Roles="Admin")]
        public ActionResult Create()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            ViewBag.projectUid = new SelectList(db.Projects.Where(m => m.projectUid == sessionProject), "projectUid", "project1");
            ViewBag.categoryUid = new SelectList(db.Categories, "categoryUid", "category1");
            return View();
        }

        // POST: ScopeOfWorks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(ScopeOfWork scopeOfWork, HttpPostedFileBase postedFile)
        {
            if (ModelState.IsValid)
            {
                if (postedFile != null)
                {
                    int fileSize = postedFile.ContentLength;
                    MemoryStream target = new MemoryStream();
                    postedFile.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();
                    scopeOfWork.document = data;
                    scopeOfWork.fileType = Path.GetExtension(postedFile.FileName);
                }


                var docName = postedFile.FileName;

                scopeOfWork.documentName = docName;

                scopeOfWork.scopeOfWorkUid = Guid.NewGuid();
                db.ScopeOfWorks.Add(scopeOfWork);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            ViewBag.categoryUid = new SelectList(db.Categories, "categoryUid", "category1", scopeOfWork.categoryUid);
            ViewBag.projectUid = new SelectList(db.Projects.Where(m => m.projectUid == sessionProject), "projectUid", "project1", scopeOfWork.projectUid);
            return View(scopeOfWork);
        }

        // GET: ScopeOfWorks/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScopeOfWork scopeOfWork = await db.ScopeOfWorks.FindAsync(id);
            if (scopeOfWork == null)
            {
                return HttpNotFound();
            }            

            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();            
            ViewBag.projectUid = new SelectList(db.Projects.Where(m => m.projectUid == sessionProject), "projectUid", "project1", scopeOfWork.projectUid);
            ViewBag.categoryUid = new SelectList(db.Categories, "categoryUid", "category1", scopeOfWork.categoryUid);
            return View(scopeOfWork);            
        }

        // POST: ScopeOfWorks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "scopeOfWorkUid,projectUid,categoryUid,scopeOfWork1,scopeOfWorkDescription,document,fileType")] ScopeOfWork scopeOfWork)
        {
            if (ModelState.IsValid)
            {                
                db.Entry(scopeOfWork).State = EntityState.Modified;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbEntityValidationException ex)
                {

                    foreach (var eve in ex.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                }
                
                return RedirectToAction("Index");
            }
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            ViewBag.projectUid = new SelectList(db.Projects.Where(m => m.projectUid == sessionProject), "projectUid", "project1", scopeOfWork.projectUid);
            ViewBag.categoryUid = new SelectList(db.Categories, "categoryUid", "category1", scopeOfWork.categoryUid);            
            return View(scopeOfWork);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetScopeDocument(Guid scopeOfWorkUid, string fileType)
        {
            var d = from cc in db.ScopeOfWorks
                    where cc.scopeOfWorkUid == scopeOfWorkUid
                    select cc.document;

            var docName = from cc in db.ScopeOfWorks
                          where cc.scopeOfWorkUid == scopeOfWorkUid
                          select cc.documentName;

            byte[] byteArray = d.FirstOrDefault();
            return File(byteArray, "application/octect-stream", docName.FirstOrDefault());
        }

        
        // GET: ScopeOfWorks/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScopeOfWork scopeOfWork = await db.ScopeOfWorks.FindAsync(id);
            if (scopeOfWork == null)
            {
                return HttpNotFound();
            }
            return View(scopeOfWork);
        }

        // POST: ScopeOfWorks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Admin")]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ScopeOfWork scopeOfWork = await db.ScopeOfWorks.FindAsync(id);
            db.ScopeOfWorks.Remove(scopeOfWork);
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
