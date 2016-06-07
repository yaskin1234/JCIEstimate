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
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            IQueryable<ContractorSignoffFinal> contractorSignoffFinals;
            if (User.IsInRole("Admin"))
            {
                contractorSignoffFinals = db.ContractorSignoffFinals.Where(c => c.projectUid == sessionProject).Include(c => c.AspNetUser).Include(c => c.Contractor).Include(c => c.Project).OrderByDescending(c => c.dateCreated);
            }
            else
            {
                contractorSignoffFinals = from cc in db.ContractorSignoffFinals                            
                                          join cn in db.ContractorUsers on cc.contractorUid equals cn.contractorUid
                                          join cq in db.AspNetUsers on cn.aspNetUserUid equals cq.Id
                                          where cc.projectUid == sessionProject
                                          && cq.UserName == System.Web.HttpContext.Current.User.Identity.Name
                                          select cc;
                contractorSignoffFinals = contractorSignoffFinals.Include(c => c.AspNetUser).Include(c => c.Contractor).Include(c => c.Project).OrderByDescending(c => c.dateCreated);
            }
            
            return View(await contractorSignoffFinals.ToListAsync());
        }

        // GET: GenerateSignoff
        public async Task<ActionResult> GenerateSignoff()
        {

            var contractors = db.Estimates.GroupBy(c => c.contractorUid)
                .Select(grp => grp.FirstOrDefault()).Include(c=>c.Contractor).OrderBy(c=>c.Contractor.contractorName);

            ViewBag.contractorUid = contractors.ToSelectList(c=>c.Contractor.contractorName, c=>c.contractorUid.ToString(), "");            

            return View();
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
                        && ( cq.UserName == System.Web.HttpContext.Current.User.Identity.Name || User.IsInRole("Admin") )
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


        private void CreateFinalSignoff(string contractorUid, bool isActiveOnly)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            ContractorSignoffFinal contractorSignoffFinal = new ContractorSignoffFinal();
            ContractorSignoff cf = new ContractorSignoff();
            byte[] byteArray;

            IEnumerable<Contractor> contractor;
            IEnumerable<Estimate> estimates;
            IEnumerable<AspNetUser> user;
            var currentUser = IdentityExtensions.GetUserId(User.Identity);

            user = from cc in db.AspNetUsers
                   where cc.Id == currentUser
                   select cc;

            string typedName = user.FirstOrDefault().AspNetUsersExtensions.FirstOrDefault().name;

            contractor = from cc in db.Contractors
                         where cc.contractorUid.ToString() == contractorUid
                         select cc;

            estimates = from cc in db.Estimates
                        join dd in db.Locations on cc.locationUid equals dd.locationUid
                        where dd.projectUid == sessionProject
                        && cc.contractorUid.ToString() == contractorUid
                        where cc.isActive == true
                        select cc;
            

            string directory = "JCIEstimate";
            string reportName = "ContractorSignoff";

            string saveFolder = "Signoffs";
            string addressRoot = "http://localhost/ReportServer?/" + directory + "/" + reportName + "&rs:Format=pdf&projectUid=" + sessionProject.ToString() + "&contractorUid=" + contractorUid.ToString() + "&typedName=" + typedName + "&isActive=" + ((isActiveOnly) ? "1" : "0");
            string saveFile = "";

            //saveFile = saveFolder + "\\Contractor Signoff" + "_" + DateTime.Now.ToFileTime() + ".xls";
            saveFile = Server.MapPath("\\" + saveFolder) + "\\Contractor Signoff" + "_" + DateTime.Now.ToFileTime() + ".pdf";

            SaveFileFromURL(addressRoot, saveFile, 600000, CredentialCache.DefaultNetworkCredentials);

            if (!System.IO.File.Exists(saveFile))
            {
                ViewBag.message = "file failed to create";
            }
            else
            {
                byteArray = System.IO.File.ReadAllBytes(saveFile);
                DateTime dateCreated = DateTime.Now;

                if (isActiveOnly)
                {
                    contractorSignoffFinal.contractorUid = Guid.Parse(contractorUid);
                    contractorSignoffFinal.contractorSignoffFinalUid = Guid.NewGuid();
                    contractorSignoffFinal.projectUid = sessionProject;
                    contractorSignoffFinal.attachment = byteArray;
                    contractorSignoffFinal.dateCreated = dateCreated;
                    contractorSignoffFinal.fileType = "pdf";
                    contractorSignoffFinal.typedName = "N/A";
                    contractorSignoffFinal.documentName = contractor.First().contractorName + "_" + String.Format("{0:yyyyMMddHHmmss}", dateCreated) + "." + contractorSignoffFinal.fileType;
                    contractorSignoffFinal.aspNetUserUidAsCreated = currentUser;
                    db.ContractorSignoffFinals.Add(contractorSignoffFinal);
                }
                else
                {
                    cf.contractorUid = Guid.Parse(contractorUid);
                    cf.contractorSignoffUid = Guid.NewGuid();
                    cf.projectUid = sessionProject;
                    cf.attachment = byteArray;
                    cf.dateCreated = dateCreated;
                    cf.fileType = "pdf";
                    cf.typedName = user.FirstOrDefault().AspNetUsersExtensions.FirstOrDefault().name;
                    cf.documentName = contractor.First().contractorName + "_" + String.Format("{0:yyyyMMddHHmmss}", dateCreated) + "." + cf.fileType;
                    cf.aspNetUserUidAsCreated = currentUser;
                    db.ContractorSignoffs.Add(cf);
                }
    
                try
                {
                    db.SaveChanges();
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
            }
        }
        
        // POST: ContractorSignoff/ContractorSignoffFinal
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GenerateSignoff(ContractorSignoffFinal contractorSignoffFinal, string contractorUid, string mode)
        {
            if (mode == "proposed")
            {
                CreateFinalSignoff(contractorUid, false);
            }
            else
            {
                CreateFinalSignoff(contractorUid, true);
            }
            

            var contractors = db.Estimates.GroupBy(c => c.contractorUid)
                .Select(grp => grp.FirstOrDefault()).Include(c => c.Contractor).OrderBy(c => c.Contractor.contractorName);

            ViewBag.contractorUid = contractors.ToSelectList(c => c.Contractor.contractorName, c => c.contractorUid.ToString(), "");

            var cName = from cc in db.Contractors
                           where cc.contractorUid.ToString() == contractorUid
                           select cc.contractorName;

            ViewBag.message = cName.FirstOrDefault() + " -- " + mode + " generated successfully.";
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
                throw new Exception("Error generating report " + destinationFileName + ".  Error: " + err.Message, err.InnerException);
            }
            return true;
        }
    }
}
