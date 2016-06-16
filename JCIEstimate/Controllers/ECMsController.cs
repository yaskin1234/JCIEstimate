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
using System.Drawing;
using System.Drawing.Imaging;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Drawing;

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
        public async Task<ActionResult> Edit([Bind(Include = "ecmUid,ecmNumber,ecmDescription,ecmString,projectUid,scopeOfWorkNote,scopeOfWorkNote2,scopeOfWorkNote3,scopeOfWorkNote4,scopeOfWorkNote5,scopeOfWorkNote6,scopeOfWorkNote7,scopeOfWorkNote8,scopeOfWorkNote9,scopeOfWorkNote10,scopeOfWorkNote11,scopeOfWorkNote12,scopeOfWorkNote13,scopeOfWorkNote14,scopeOfWorkNote15")] ECM eCM, IEnumerable<HttpPostedFileBase> pics)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            //ECM currentECM = db.ECMs.AsNoTracking().Single(c => c.ecmUid == eCM.ecmUid);
            byte[] currentFile = null; //currentECM.pdfSnippet;
            //string currentEcmFileName = currentECM.pdfSnippetFileName;
            bool showOnScopeReport = true; // currentECM.showOnScopeReport;
            //currentECM = null;

            if (ModelState.IsValid)
            {                
                db.Entry(eCM).State = EntityState.Modified;
                eCM.showOnScopeReport = showOnScopeReport;
                //add pictures
                foreach (var file in pics)
                {
                    if (file != null)
                    {
                        int fileSize = file.ContentLength;                        
                        byte[] uploadedFile = new byte[file.InputStream.Length];
                        file.InputStream.Read(uploadedFile, 0, uploadedFile.Length);                                                
                        eCM.pdfSnippet = uploadedFile;
                        eCM.pdfSnippetFileName = file.FileName;
                    }
                    else
                    {
                        eCM.pdfSnippet = currentFile;
                        //eCM.pdfSnippetFileName = currentEcmFileName;
                    }
                }

                await db.SaveChangesAsync();

                if (pics != null)
                {
                    PdfDocument final = new PdfDocument();
                    foreach (var oECM in db.ECMs.Where(c=>c.projectUid == eCM.projectUid).Where(c=>c.pdfSnippet != null).Where(c=>c.showOnScopeReport).OrderBy(c=>c.ecmNumber))
                    {
                        MemoryStream ms = new MemoryStream(oECM.pdfSnippet);
                        PdfDocument from = PdfReader.Open(ms, PdfDocumentOpenMode.Import);
                        ms.Close();
                        //PdfDocument from = new PdfDocument(@"F:\Dloads\!!HealthInsurance\DentalEOB_5.2012.pdf");
                        CopyPages(from, final);
                    }

                    if (final.PageCount > 0)
                    {
                        MemoryStream finalMS = new MemoryStream();
                        final.Save(finalMS, false);
                        MemoryStream target = new MemoryStream();
                        byte[] finalPDF = new byte[finalMS.Length];
                        finalMS.Read(finalPDF, 0, finalPDF.Length);
                        finalMS.Close();
                        ProjectScope p = db.ProjectScopes.Where(c => c.projectUid == sessionProject).FirstOrDefault();
                        if (p == null)
                        {
                            p = new ProjectScope();
                            p.projectScopeUid = Guid.NewGuid();
                            p.projectUid = eCM.projectUid;
                            p.projectScopePDF = finalPDF;
                            db.ProjectScopes.Add(p);                            
                        }
                        else
                        {
                            db.Entry(p).State = EntityState.Modified;
                            p.projectScopePDF = finalPDF;                            
                        }
                        await db.SaveChangesAsync();                        
                    }
                        
                }
                
                return RedirectToAction("Index");
            }
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", eCM.projectUid);
            return View(eCM);
        }

        void CopyPages(PdfDocument from, PdfDocument to)
        {
            for (int i = 0; i < from.PageCount; i++)
            {
                to.AddPage(from.Pages[i]);
            }
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
