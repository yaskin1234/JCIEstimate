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
    public class ContractorSignoffFinalsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: ContractorSignoffFinals
        public async Task<ActionResult> Index()
        {
            var contractorSignoffFinals = db.ContractorSignoffFinals.Include(c => c.AspNetUser).Include(c => c.Contractor).Include(c => c.Project);
            return View(await contractorSignoffFinals.ToListAsync());
        }

        // GET: ContractorSignoff/ContractorSignoff
        public async Task<ActionResult> ContractorSignoffFinal()
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
                        where cc.isActive == true
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

        // POST: ContractorSignoff/ContractorSignoffFinal
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ContractorSignoffFinal([Bind(Include = "ContractorSignoffFinalUid,projectUid,aspNetUserUidAsCreated,dateCreated,typedName,attachment,fileType,documentName")] ContractorSignoffFinal contractorSignoffFinal, string signedName)
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
                                where cc.isActive == true
                                select cc;

                    string directory = "JCIEstimate";
                    string reportName = "ContractorSignoff";

                    string saveFolder = "Signoffs";
                    string addressRoot = "http://localhost/ReportServer?/" + directory + "/" + reportName + "&rs:Format=word&projectUid=" + sessionProject.ToString() + "&contractorUid=" + estimates.First().contractorUid.ToString() + "&typedName=" + signedName + "&isActive=1";
                    string saveFile = "";

                    //saveFile = saveFolder + "\\Contractor Signoff" + "_" + DateTime.Now.ToFileTime() + ".xls";
                    saveFile = Server.MapPath("\\" + saveFolder) + "\\Contractor Signoff" + "_" + DateTime.Now.ToFileTime() + ".doc";

                    SaveFileFromURL(addressRoot, saveFile, 600000, CredentialCache.DefaultNetworkCredentials);
                    byte[] byteArray = System.IO.File.ReadAllBytes(saveFile);

                    var currentUser = IdentityExtensions.GetUserId(User.Identity);

                    DateTime dateCreated = DateTime.Now;

                    contractorSignoffFinal.contractorUid = estimates.First().contractorUid;
                    contractorSignoffFinal.contractorSignoffFinalUid = Guid.NewGuid();
                    contractorSignoffFinal.projectUid = sessionProject;
                    contractorSignoffFinal.attachment = byteArray;
                    contractorSignoffFinal.dateCreated = dateCreated;
                    contractorSignoffFinal.fileType = "doc";
                    contractorSignoffFinal.typedName = signedName;
                    contractorSignoffFinal.documentName = estimates.First().Contractor.contractorName + "_" + String.Format("{0:yyyyMMddHHmmss}", dateCreated) + "." + contractorSignoffFinal.fileType;
                    contractorSignoffFinal.aspNetUserUidAsCreated = currentUser;

                    ViewBag.estimates = estimates.OrderBy(c => c.ECM.ecmNumber).ToList();


                }
                db.ContractorSignoffFinals.Add(contractorSignoffFinal);
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

                return Redirect("/ContractorSignoffFinals/Index");
            }

            return View(contractorSignoffFinal);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetContractorSignoffFinal(Guid contractorSignoffFinalUid, string fileType)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            var d = from cc in db.ContractorSignoffFinals
                    where cc.contractorSignoffFinalUid == contractorSignoffFinalUid
                    select cc.attachment;

            var docName = from cc in db.ContractorSignoffFinals
                          where cc.contractorSignoffFinalUid == contractorSignoffFinalUid
                          select cc.documentName;

            byte[] byteArray = d.FirstOrDefault();
            return File(byteArray, "application/octect-stream", docName.FirstOrDefault());
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetContractorSignoffFinalAttachment(Guid contractorSignoffFinalAttachmentUid, string fileType)
        {
            var d = from cc in db.ContractorSignoffFinalAttachments
                    where cc.contractorSignoffFinalAttachmentUid == contractorSignoffFinalAttachmentUid
                    select cc.attachment;

            var docName = from cc in db.ContractorSignoffFinalAttachments
                          where cc.contractorSignoffFinalAttachmentUid == contractorSignoffFinalAttachmentUid
                          select cc.documentName;

            byte[] byteArray = d.FirstOrDefault();
            return File(byteArray, "application/octect-stream", docName.FirstOrDefault());
        }

        // GET: ContractorSignoffFinals/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorSignoffFinal contractorSignoffFinal = await db.ContractorSignoffFinals.FindAsync(id);
            if (contractorSignoffFinal == null)
            {
                return HttpNotFound();
            }
            return View(contractorSignoffFinal);
        }

        // GET: ContractorSignoffFinals/Create
        public ActionResult Create()
        {
            ViewBag.aspNetUserUidAsCreated = new SelectList(db.AspNetUsers, "Id", "AllowableContractors");
            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName");
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1");
            return View();
        }

        // POST: ContractorSignoffFinals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "contractorSignoffFinalUid,projectUid,aspNetUserUidAsCreated,contractorUid,dateCreated,typedName,attachment,fileType,documentName")] ContractorSignoffFinal contractorSignoffFinal)
        {
            if (ModelState.IsValid)
            {
                contractorSignoffFinal.contractorSignoffFinalUid = Guid.NewGuid();
                db.ContractorSignoffFinals.Add(contractorSignoffFinal);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.aspNetUserUidAsCreated = new SelectList(db.AspNetUsers, "Id", "AllowableContractors", contractorSignoffFinal.aspNetUserUidAsCreated);
            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName", contractorSignoffFinal.contractorUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", contractorSignoffFinal.projectUid);
            return View(contractorSignoffFinal);
        }


       
        // GET: ContractorSignoffFinals/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorSignoffFinal contractorSignoffFinal = await db.ContractorSignoffFinals.FindAsync(id);
            if (contractorSignoffFinal == null)
            {
                return HttpNotFound();
            }

            ViewBag.signoffFinalAttachments = contractorSignoffFinal.ContractorSignoffFinalAttachments.OrderByDescending(c => c.dateCreated);
            ViewBag.aspNetUserUidAsCreated = new SelectList(db.AspNetUsers, "Id", "AllowableContractors", contractorSignoffFinal.aspNetUserUidAsCreated);
            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName", contractorSignoffFinal.contractorUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", contractorSignoffFinal.projectUid);
            return View(contractorSignoffFinal);
        }

        // POST: ContractorSignoffFinals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "contractorSignoffFinalUid,projectUid,aspNetUserUidAsCreated,contractorUid,dateCreated,typedName,attachment,fileType,documentName")] ContractorSignoffFinal contractorSignoffFinal, HttpPostedFileBase postedFile, string attachmentDescription)
        {
            if (ModelState.IsValid)
            {
                if (postedFile != null)
                {
                    var userUid = from cc in db.AspNetUsers
                                  where cc.UserName == System.Web.HttpContext.Current.User.Identity.Name
                                  select cc.Id;
                    ContractorSignoffFinalAttachment csa = new ContractorSignoffFinalAttachment();
                    csa.contractorSignoffFinalAttachmentUid = Guid.NewGuid();

                    int fileSize = postedFile.ContentLength;
                    MemoryStream target = new MemoryStream();
                    postedFile.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();
                    csa.attachment = data;

                    csa.aspNetUserUidAsCreated = userUid.First();
                    csa.contractorSignoffFinalUid = contractorSignoffFinal.contractorSignoffFinalUid;
                    csa.dateCreated = DateTime.Now;
                    csa.fileType = Path.GetExtension(postedFile.FileName);
                    csa.documentName = postedFile.FileName;
                    csa.contractorSignoffFinalAttachment1 = attachmentDescription;
                    db.ContractorSignoffFinalAttachments.Add(csa);
                }

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.aspNetUserUidAsCreated = new SelectList(db.AspNetUsers, "Id", "AllowableContractors", contractorSignoffFinal.aspNetUserUidAsCreated);
            ViewBag.contractorUid = new SelectList(db.Contractors, "contractorUid", "contractorName", contractorSignoffFinal.contractorUid);
            ViewBag.projectUid = new SelectList(db.Projects, "projectUid", "project1", contractorSignoffFinal.projectUid);
            return View(contractorSignoffFinal);
        }

        // GET: ContractorSignoffFinals/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContractorSignoffFinal contractorSignoffFinal = await db.ContractorSignoffFinals.FindAsync(id);
            if (contractorSignoffFinal == null)
            {
                return HttpNotFound();
            }
            return View(contractorSignoffFinal);
        }

        // POST: ContractorSignoffFinals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            ContractorSignoffFinal contractorSignoffFinal = await db.ContractorSignoffFinals.FindAsync(id);
            db.ContractorSignoffFinals.Remove(contractorSignoffFinal);
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
