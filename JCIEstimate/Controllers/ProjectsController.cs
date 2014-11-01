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
    public class ProjectsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: Projects
        public async Task<ActionResult> Index()
        {
            return View(await db.Projects.ToListAsync());        
        }

        // GET: ChooseProject
        public async Task<ActionResult> ChooseProject()
        {
            return View(await db.Projects.ToListAsync());
        }

        // POST: Projects/SetProject
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetProject([Bind(Include = "projectUid, project1")] Project project)
        {
            if (ModelState.IsValid)
            {
                Session["projectUid"] = project.projectUid;                           
                Session["projectName"] = db.Projects.First(d => d.projectUid == project.projectUid).project1.ToString();
                
                return RedirectToAction("Index", "Estimates");
            }

            return View(project); //test comment
        }

        // GET: Projects/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            ViewBag.miscExpenseList = new SelectList(db.ExpenseMiscellaneous, "expenseMiscellaneousUid", "expenseMiscellaneous");
            return View();
        }       

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "projectUid,project1,projectDescription")] Project project, params string[] selectedExpenses)
        {
            ExpenseMiscellaneousProject myExpense;            

            if (ModelState.IsValid)
            {

                project.projectUid = Guid.NewGuid();
                db.Projects.Add(project);
                await db.SaveChangesAsync();

                if (selectedExpenses != null)
                {
                    //add each selected task to EquipmentToDo
                    foreach (var item in selectedExpenses)
                    {
                        myExpense = new ExpenseMiscellaneousProject();
                        myExpense.expenseMiscellaneousProjectUid = Guid.NewGuid();
                        myExpense.projectUid = project.projectUid;
                        myExpense.expenseMiscellaneousUid = new Guid(item);
                        db.ExpenseMiscellaneousProjects.Add(myExpense);
                        await db.SaveChangesAsync();
                    }
                }
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            var selectedExpenses = from cc in db.ExpenseMiscellaneousProjects
                                where cc.projectUid == id
                                select cc.expenseMiscellaneousUid.ToString();
            var expenseList = db.ExpenseMiscellaneous.ToList().Select(x => new SelectListItem()
            {
                Selected = selectedExpenses.Contains(x.expenseMiscellaneousUid.ToString()),
                Text = x.expenseMiscellaneous,
                Value = x.expenseMiscellaneousUid.ToString()
            });
            ViewBag.expenseList = new SelectList(expenseList, "Value", "Text", selectedExpenses.ToList());            
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "projectUid,project1,projectDescription")] Project project, params string[] selectedExpenses)
        {
            ExpenseMiscellaneousProject myExpense;

            if (ModelState.IsValid)
            {
                if (selectedExpenses != null)
                {

                    IQueryable<ExpenseMiscellaneousProject> expensesToDelete;
                    IQueryable<ExpenseMiscellaneou> expensesToInsert;

                    expensesToDelete = from cc in db.ExpenseMiscellaneousProjects
                                        where !(selectedExpenses.Any(v => cc.expenseMiscellaneousUid.ToString().Contains(v)))
                                        && cc.projectUid == project.projectUid
                                        select cc;

                    expensesToInsert = from cc in db.ExpenseMiscellaneous
                                       where selectedExpenses.Any(v => cc.expenseMiscellaneousUid.ToString().Contains(v))
                                       select cc;

                    //Delete
                    foreach (var item in expensesToDelete.ToList())
                    {
                        myExpense = db.ExpenseMiscellaneousProjects.Find(item.expenseMiscellaneousProjectUid);
                        db.ExpenseMiscellaneousProjects.Remove(myExpense);
                        await db.SaveChangesAsync();
                    }

                    //Insert
                    foreach (var item in expensesToInsert.ToList())
                    {
                        myExpense = new ExpenseMiscellaneousProject();
                        myExpense.expenseMiscellaneousUid = item.expenseMiscellaneousUid;
                        myExpense.projectUid= project.projectUid;
                        JCIExtensions.MCVExtensions.InsertOrUpdate(db, myExpense);
                    }
                }
            }    


            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Project project = await db.Projects.FindAsync(id);
            db.Projects.Remove(project);
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
