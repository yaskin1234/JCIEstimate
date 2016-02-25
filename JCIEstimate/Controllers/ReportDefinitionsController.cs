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
    public class ReportDefinitionsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ReportDefinitions
        public async Task<ActionResult> Index()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            return View(await db.ReportDefinitions.Where(c=>c.projectUid == sessionProject).ToListAsync());
        }

        // GET: ReportDefinitions/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportDefinition reportDefinition = await db.ReportDefinitions.FindAsync(id);
            if (reportDefinition == null)
            {
                return HttpNotFound();
            }
            return View(reportDefinition);
        }

        // GET: ReportDefinitions/Create
        public ActionResult Create()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            ViewBag.projectUid = db.Projects.Where(c => c.projectUid == sessionProject).ToSelectList(c => c.project1, c => c.projectUid.ToString(), "", false);
            return View();
        }

        // POST: ReportDefinitions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ReportDefinitionUid,projectUid,ssrsReportName,textBox1,textBox2,textBox3,textBox4,textBox5,textBox6,textBox7,textBox8,textBox9,textBox10,textBox11,textBox12,textBox13,textBox14,textBox15,textBox16,textBox17,textBox18,textBox19,textBox20,textBox21,textBox22,textBox23,textBox24,textBox25,textBox26,textBox27,textBox28,textBox29,textBox30,textBox31,textBox32,textBox33,textBox34,textBox35,textBox36,textBox37,textBox38,textBox39,textBox40,textBox41,textBox42,textBox43,textBox44,textBox45,textBox46,textBox47,textBox48,textBox49,textBox50")] ReportDefinition reportDefinition)
        {
            if (ModelState.IsValid)
            {
                Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
                reportDefinition.ReportDefinitionUid = Guid.NewGuid();
                reportDefinition.projectUid = sessionProject;
                db.ReportDefinitions.Add(reportDefinition);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(reportDefinition);
        }

        // GET: ReportDefinitions/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportDefinition reportDefinition = await db.ReportDefinitions.FindAsync(id);
            if (reportDefinition == null)
            {
                return HttpNotFound();
            }
            ViewBag.projectUid = db.Projects.Where(c => c.projectUid == sessionProject).ToSelectList(c => c.project1, c => c.projectUid.ToString(), reportDefinition.projectUid.ToString(), false);
            return View(reportDefinition);
        }

        // POST: ReportDefinitions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ReportDefinitionUid,projectUid,ssrsReportName,textBox1,textBox2,textBox3,textBox4,textBox5,textBox6,textBox7,textBox8,textBox9,textBox10,textBox11,textBox12,textBox13,textBox14,textBox15,textBox16,textBox17,textBox18,textBox19,textBox20,textBox21,textBox22,textBox23,textBox24,textBox25,textBox26,textBox27,textBox28,textBox29,textBox30,textBox31,textBox32,textBox33,textBox34,textBox35,textBox36,textBox37,textBox38,textBox39,textBox40,textBox41,textBox42,textBox43,textBox44,textBox45,textBox46,textBox47,textBox48,textBox49,textBox50")] ReportDefinition reportDefinition)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reportDefinition).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(reportDefinition);
        }

        // GET: ReportDefinitions/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportDefinition reportDefinition = await db.ReportDefinitions.FindAsync(id);
            if (reportDefinition == null)
            {
                return HttpNotFound();
            }
            return View(reportDefinition);
        }

        // POST: ReportDefinitions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ReportDefinition reportDefinition = await db.ReportDefinitions.FindAsync(id);
            db.ReportDefinitions.Remove(reportDefinition);
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
