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
    public class EquipmentTaskDetailsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        // GET: /EquipmentTaskDetails/
        public async Task<ActionResult> Index(string filterId)
        {
            IQueryable<EquipmentTaskDetail> etdList;
            List<FilterOptionModel> aryFo = new List<FilterOptionModel>();

            if (filterId == null)
            {
                if (Session["equipmentTaskDetailFilterId"] != null)
                {
                    filterId = Session["equipmentTaskDetailFilterId"].ToString();
                }
            }

            etdList = from cc in db.EquipmentTaskDetails                 
                      select cc;

            aryFo = buildFilterDropDown(filterId, etdList);
            etdList = applyFilter(filterId, etdList);

            ViewBag.filterList = aryFo.ToList();
            var equipmenttaskdetails = etdList.Include(e => e.EquipmentTask);

            //return View(await equipmenttaskdetails.OrderBy(c => c.EquipmentTask.EquipmentAttributeType.equipmentAttributeType1).ThenBy(c => c.EquipmentTask.equipmentTask1).ThenBy(C=>C.equipmentTaskDetail1).ToListAsync());
            return View(await etdList.OrderBy(c => c.sequence).ToListAsync());
        }



        private List<FilterOptionModel> buildFilterDropDown(string filterId, IQueryable<EquipmentTaskDetail> etdList)
        {
            //Build Drop down filter based on existing defined equipment
            List<FilterOptionModel> aryFo = new List<FilterOptionModel>();
            string[] filterPart = null;
            string type = "";
            string uid = Guid.Empty.ToString();

            if (!String.IsNullOrEmpty(filterId))
            {
                filterPart = filterId.Split('|');
                type = filterPart[0];
                uid = filterPart[1];
            }

            FilterOptionModel wf = new FilterOptionModel();
            wf.text = "-- Choose --";
            wf.value = "X|" + Guid.Empty.ToString();
            wf.selected = (wf.value == filterId || String.IsNullOrEmpty(filterId));
            aryFo.Add(wf);

            wf = new FilterOptionModel();
            wf.text = "All";
            wf.value = "A|" + Guid.Empty.ToString().Substring(0, 35) + "1";
            wf.selected = (wf.value == filterId);
            aryFo.Add(wf);


            IQueryable<EquipmentTaskDetail> results = etdList.GroupBy(c => c.EquipmentTask.equipmentAttributeTypeUid).Select(v => v.FirstOrDefault());

            foreach (var item in results.OrderBy(c => c.EquipmentTask.EquipmentAttributeType.equipmentAttributeType1))
            {
                wf = new FilterOptionModel();
                wf.text = item.EquipmentTask.EquipmentAttributeType.equipmentAttributeType1;
                wf.value = "L|" + item.EquipmentTask.equipmentAttributeTypeUid.ToString();
                wf.selected = (wf.value == filterId);
                aryFo.Add(wf);
            }

            return aryFo;
        }

        private IQueryable<EquipmentTaskDetail> applyFilter(string filterId, IQueryable<EquipmentTaskDetail> etdList)
        {
            //apply filter if there is one
            string[] filterPart = null;
            string type = "";
            string uid = Guid.Empty.ToString();
            if (!String.IsNullOrEmpty(filterId))
            {
                filterPart = filterId.Split('|');
                type = filterPart[0];
                uid = filterPart[1];

                if (type == "L")
                {
                    etdList = etdList.Where(c => c.EquipmentTask.equipmentAttributeTypeUid.ToString() == uid);
                }
                else if (type == "X")
                {
                    etdList = etdList.Where(c => c.equipmentTaskDetailUid == Guid.Empty);
                }
            }
            else
            {
                etdList = etdList.Where(c => c.equipmentTaskDetailUid == Guid.Empty);
            }
            Session["equipmentTaskDetailFilterId"] = filterId;

            return etdList;
        }

        // GET: /EquipmentTaskDetails/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentTaskDetail equipmenttaskdetail = await db.EquipmentTaskDetails.FindAsync(id);
            if (equipmenttaskdetail == null)
            {
                return HttpNotFound();
            }
            return View(equipmenttaskdetail);
        }

        // GET: /EquipmentTaskDetails/Create
        public ActionResult Create()
        {
            var tasks = from cc in db.EquipmentTasks
                        join dd in db.EquipmentAttributeTypes on cc.equipmentAttributeTypeUid equals dd.equipmentAttributeTypeUid
                        select cc;
            ViewBag.equipmentTaskUid = tasks.OrderBy(c=>c.EquipmentAttributeType.equipmentAttributeType1).ThenBy(c=>c.equipmentTask1).ToSelectList(c=>c.EquipmentAttributeType.equipmentAttributeType1 + " - " + c.equipmentTask1, c=>c.equipmentTaskUid.ToString(), "");
            return View();
        }

        // POST: /EquipmentTaskDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "equipmentTaskDetailUid,equipmentTaskUid,equipmentTaskDetail1,sequence,isScheduleList,isEquipmentExpected,isWarrantyStart")] EquipmentTaskDetail equipmenttaskdetail)
        {
            if (ModelState.IsValid)
            {
                equipmenttaskdetail.equipmentTaskDetailUid = Guid.NewGuid();
                db.EquipmentTaskDetails.Add(equipmenttaskdetail);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.equipmentTaskUid = new SelectList(db.EquipmentTasks, "equipmentTaskUid", "equipmentTask1", equipmenttaskdetail.equipmentTaskUid);
            return View(equipmenttaskdetail);
        }

        // GET: /EquipmentTaskDetails/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentTaskDetail equipmenttaskdetail = await db.EquipmentTaskDetails.FindAsync(id);
            if (equipmenttaskdetail == null)
            {
                return HttpNotFound();
            }
            var tasks = from cc in db.EquipmentTasks
                        join dd in db.EquipmentAttributeTypes on cc.equipmentAttributeTypeUid equals dd.equipmentAttributeTypeUid
                        select cc;

            ViewBag.equipmentTaskUid = tasks.OrderBy(c => c.EquipmentAttributeType.equipmentAttributeType1).ThenBy(c => c.equipmentTask1).ToSelectList(c => c.EquipmentAttributeType.equipmentAttributeType1 + " - " + c.equipmentTask1, c => c.equipmentTaskUid.ToString(), equipmenttaskdetail.equipmentTaskUid.ToString());
            ViewBag.equipmentTaskUid = new SelectList(db.EquipmentTasks, "equipmentTaskUid", "equipmentTask1", equipmenttaskdetail.equipmentTaskUid);
            return View(equipmenttaskdetail);
        }

        // POST: /EquipmentTaskDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "equipmentTaskDetailUid,equipmentTaskUid,equipmentTaskDetail1,sequence,isScheduleList,isEquipmentExpected,isWarrantyStart")] EquipmentTaskDetail equipmenttaskdetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipmenttaskdetail).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.equipmentTaskUid = new SelectList(db.EquipmentTasks, "equipmentTaskUid", "equipmentTask1", equipmenttaskdetail.equipmentTaskUid);
            return View(equipmenttaskdetail);
        }

        // GET: /EquipmentTaskDetails/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EquipmentTaskDetail equipmenttaskdetail = await db.EquipmentTaskDetails.FindAsync(id);
            if (equipmenttaskdetail == null)
            {
                return HttpNotFound();
            }
            return View(equipmenttaskdetail);
        }

        // POST: /EquipmentTaskDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            EquipmentTaskDetail equipmenttaskdetail = await db.EquipmentTaskDetails.FindAsync(id);
            db.EquipmentTaskDetails.Remove(equipmenttaskdetail);
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
