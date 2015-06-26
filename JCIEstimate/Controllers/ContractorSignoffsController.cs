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
using Microsoft.AspNet.Identity;
using System.Data.Entity.Validation;
using JCIExtensions;
using System.IO;

namespace JCIEstimate.Controllers
{
    public class ContractorSignoffsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ContractorSignoffs
        public async Task<ActionResult> Index()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            IQueryable<ContractorSignoff> contractorSignoffs;

            if (User.IsInRole("Admin"))
            {
                contractorSignoffs = from cc in db.ContractorSignoffs
                                     where cc.projectUid == sessionProject
                                     select cc;
            }
            else
            {
                contractorSignoffs = from cc in db.ContractorSignoffs
                                        join cq in db.AspNetUsers on cc.aspNetUserUidAsCreated equals cq.Id
                                        where cc.projectUid == sessionProject
                                        && cq.UserName == System.Web.HttpContext.Current.User.Identity.Name
                                        select cc;
            }
            
            
            


            contractorSignoffs = contractorSignoffs.OrderByDescending(c => c.dateCreated).Include(c => c.AspNetUser).Include(c => c.Project);
            return View(await contractorSignoffs.ToListAsync());
        }

        // GET: ContractorSignoffs/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorSignoff contractorSignoff = await db.ContractorSignoffs.FindAsync(id);
            if (contractorSignoff == null)
            {
                return HttpNotFound();
            }
            return View(contractorSignoff);
        }

        // GET: ContractorSignoffs/Create
        public ActionResult Create()
        {
            ViewBag.aspNetUserUidAsCreated = new SelectList(db.AspNetUsers, "Id", "AllowableContractors");
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1");
            return View();
        }

        // GET: ContractorSignoff/ContractorSignoff
        public async Task<ActionResult> ContractorSignoff()
        {
            IQueryable<Estimate> estimates;
            IQueryable<ProjectRFI> projectRFIs;
            IQueryable<ProjectAddendum> projectAddendums;
            IQueryable<ScopeOfWork> scopeOfWorks;
            IQueryable<AspNetUser> loggedInUser;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            estimates = from cc in db.Estimates
                        join dd in db.Locations on cc.locationUid equals dd.locationUid
                        join cn in db.ContractorUsers on cc.contractorUid equals cn.contractorUid
                        join cq in db.AspNetUsers on cn.aspNetUserUid equals cq.Id
                        where dd.projectUid == sessionProject
                        && cq.UserName == System.Web.HttpContext.Current.User.Identity.Name
                        select cc;

            loggedInUser = from cc in db.AspNetUsers
                               where cc.UserName == System.Web.HttpContext.Current.User.Identity.Name
                               select cc;

            projectRFIs = from cc in db.ProjectRFIs
                          where cc.projectUid == sessionProject
                          select cc;

            projectAddendums = from cc in db.ProjectAddendums
                               where cc.projectUid == sessionProject
                               select cc;

            scopeOfWorks = from cc in db.ScopeOfWorks
                           where cc.projectUid == sessionProject
                           select cc;

            ViewBag.scopeOfWorks = scopeOfWorks.OrderBy(c => c.scopeOfWorkDescription).ThenBy(c => c.Category.category1).ThenBy(c => c.documentName).ToList();
            ViewBag.projectRFIs = projectRFIs.OrderBy(c => c.projectRFIID).ToList();
            ViewBag.projectAddendums = projectAddendums.OrderBy(c => c.addendumId).ToList();
            ViewBag.estimates = estimates.OrderBy(c => c.ECM.ecmNumber).ToList();
            ViewBag.contractorName = estimates.First().Contractor.contractorName;
            ViewBag.projectName = estimates.First().ECM.Project.project1;
            ViewBag.loggedInUser = loggedInUser.First().Email;

            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetContractorSignoff(Guid contractorSignoffUid, string fileType)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();            

            var d = from cc in db.ContractorSignoffs
                    where cc.contractorSignoffUid == contractorSignoffUid
                    select cc.attachment;

            var docName = from cc in db.ContractorSignoffs
                          where cc.contractorSignoffUid == contractorSignoffUid
                          select cc.documentName;

            byte[] byteArray = d.FirstOrDefault();
            return File(byteArray, "application/octect-stream", docName.FirstOrDefault());
        }   

        // POST: ContractorSignoff/ContractorSignoff
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ContractorSignoff([Bind(Include = "contractorSignoffUid,projectUid,aspNetUserUidAsCreated,dateCreated,typedName,attachment,fileType,documentName")] ContractorSignoff contractorSignoff, string signedName)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(signedName))
                {

                    IQueryable<Estimate> estimates;

                    estimates = from cc in db.Estimates
                                join dd in db.Locations on cc.locationUid equals dd.locationUid
                                join cn in db.ContractorUsers on cc.contractorUid equals cn.contractorUid
                                join cq in db.AspNetUsers on cn.aspNetUserUid equals cq.Id
                                where dd.projectUid == sessionProject
                                && cq.UserName == System.Web.HttpContext.Current.User.Identity.Name
                                select cc;              
                    string directory = "JCIEstimate";
                    string reportName = "ContractorSignoff";

                    string saveFolder = "Signoffs";
                    string addressRoot = "http://localhost/ReportServer?/" + directory + "/" + reportName + "&rs:Format=word&projectUid=" + sessionProject.ToString() + "&contractorUid=" + estimates.First().contractorUid.ToString() + "&typedName=" + signedName;
                    string saveFile = "";

                    //saveFile = saveFolder + "\\Contractor Signoff" + "_" + DateTime.Now.ToFileTime() + ".xls";
                    saveFile = Server.MapPath("\\" + saveFolder) + "\\Contractor Signoff" + "_" + DateTime.Now.ToFileTime() + ".doc";
                
                    SaveFileFromURL(addressRoot, saveFile, 600000, CredentialCache.DefaultNetworkCredentials);
                    byte[] byteArray = System.IO.File.ReadAllBytes(saveFile);

                    var currentUser = IdentityExtensions.GetUserId(User.Identity);

                    DateTime dateCreated = DateTime.Now;

                    contractorSignoff.contractorUid = estimates.First().contractorUid;
                    contractorSignoff.contractorSignoffUid = Guid.NewGuid();
                    contractorSignoff.projectUid = sessionProject;
                    contractorSignoff.attachment = byteArray;
                    contractorSignoff.dateCreated = dateCreated;
                    contractorSignoff.fileType = "doc";
                    contractorSignoff.typedName = signedName;
                    contractorSignoff.documentName = estimates.First().Contractor.contractorName + "_" + String.Format("{0:yyyyMMddHHmmss}", dateCreated) + "." + contractorSignoff.fileType;
                    contractorSignoff.aspNetUserUidAsCreated = currentUser;

                    ViewBag.estimates = estimates.OrderBy(c => c.ECM.ecmNumber).ToList();                   


                }
                db.ContractorSignoffs.Add(contractorSignoff);
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbEntityValidationException ex)
                {

                    foreach (var item in ex.EntityValidationErrors)
                    {
                        foreach (var item2 in item.ValidationErrors)
                            {
                                
                            }
                        {
                            
                        }
                    }                    
                }
                
                return Redirect("/ContractorSignoffs/Index");
            }

            return View(contractorSignoff);
        }

        // POST: ContractorSignoffs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "contractorSignoffUid,projectUid,aspNetUserUidAsCreated,dateCreated,typedName,attachment,fileType,documentName")] ContractorSignoff contractorSignoff)
        {
            if (ModelState.IsValid)
            {
                contractorSignoff.contractorSignoffUid = Guid.NewGuid();
                db.ContractorSignoffs.Add(contractorSignoff);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.aspNetUserUidAsCreated = new SelectList(db.AspNetUsers, "Id", "AllowableContractors", contractorSignoff.aspNetUserUidAsCreated);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", contractorSignoff.projectUid);
            return View(contractorSignoff);
        }

        // GET: ContractorSignoffs/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorSignoff contractorSignoff = await db.ContractorSignoffs.FindAsync(id);
            if (contractorSignoff == null)
            {
                return HttpNotFound();
            }
            ViewBag.aspNetUserUidAsCreated = new SelectList(db.AspNetUsers, "Id", "AllowableContractors", contractorSignoff.aspNetUserUidAsCreated);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", contractorSignoff.projectUid);
            return View(contractorSignoff);
        }

        // POST: ContractorSignoffs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "contractorSignoffUid,projectUid,aspNetUserUidAsCreated,dateCreated,typedName,attachment,fileType,documentName")] ContractorSignoff contractorSignoff)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contractorSignoff).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.aspNetUserUidAsCreated = new SelectList(db.AspNetUsers, "Id", "AllowableContractors", contractorSignoff.aspNetUserUidAsCreated);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", contractorSignoff.projectUid);
            return View(contractorSignoff);
        }

        // GET: ContractorSignoffs/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorSignoff contractorSignoff = await db.ContractorSignoffs.FindAsync(id);
            if (contractorSignoff == null)
            {
                return HttpNotFound();
            }
            return View(contractorSignoff);
        }

        // POST: ContractorSignoffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ContractorSignoff contractorSignoff = await db.ContractorSignoffs.FindAsync(id);
            db.ContractorSignoffs.Remove(contractorSignoff);
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

        public static bool SaveFileFromURL(string url, string destinationFileName, int timeoutInSeconds, NetworkCredential nc)
        {
            
            // Create a web request to the URL
            HttpWebRequest MyRequest = (HttpWebRequest)WebRequest.Create(url);
            MyRequest.Timeout = timeoutInSeconds * 1000;
            MyRequest.Credentials = nc;
            
            
            try
            {
                // Get the web response
                HttpWebResponse MyResponse = (HttpWebResponse)MyRequest.GetResponse();                

                // Make sure the response is valid
                if (HttpStatusCode.OK == MyResponse.StatusCode)
                {
                    // Open the response stream               
                    Stream input = MyResponse.GetResponseStream();
                    // Open the destination file
                    using (FileStream MyFileStream = new FileStream(destinationFileName, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        input.CopyTo(MyFileStream);
                    }
                }
            }
                
            catch (Exception err)
            {
                Console.WriteLine("Error saving " + destinationFileName + " from URL:" + err.Message, err);
            }
            return true;
        }
    }
}
