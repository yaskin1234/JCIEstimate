﻿using System;
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

namespace JCIEstimate.Controllers
{
    public class ScopeOfWorksController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ScopeOfWorks
        public async Task<ActionResult> Index()
        {
            var scopeOfWorks = db.ScopeOfWorks.Include(s => s.Category).Include(s => s.Project);
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
        public ActionResult Create()
        {
            ViewBag.categoryUid = new SelectList(db.Categories, "categoryUid", "category1");
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1");
            return View();
        }

        // POST: ScopeOfWorks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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

            ViewBag.categoryUid = new SelectList(db.Categories, "categoryUid", "category1", scopeOfWork.categoryUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", scopeOfWork.projectUid);
            return View(scopeOfWork);
        }

        // GET: ScopeOfWorks/Edit/5
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
            ViewBag.categoryUid = new SelectList(db.Categories, "categoryUid", "category1", scopeOfWork.categoryUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", scopeOfWork.projectUid);
            return View(scopeOfWork);            
        }

        // POST: ScopeOfWorks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "scopeOfWorkUid,projectUid,categoryUid,scopeOfWork1,scopeOfWorkDescription,document")] ScopeOfWork scopeOfWork)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scopeOfWork).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.categoryUid = new SelectList(db.Categories, "categoryUid", "category1", scopeOfWork.categoryUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", scopeOfWork.projectUid);
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
