using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JCIExtensions;
using JCIEstimate.Models;
using System.IO;

namespace JCIEstimate.Controllers
{
    public class EquipmentsController : Controller
    {
        private JCIEstimateEntities db = new JCIEstimateEntities();

        public async Task<ActionResult> GetAttributesForType(Guid equipmentAttributeTypeUid)
        {

            var attributes = from cc in db.EquipmentAttributes
                             where cc.equipmentAttributeTypeUid == equipmentAttributeTypeUid
                             select cc;
            
            return PartialView(await attributes.ToListAsync());
        }

        // GET: Equipments
        public async Task<ActionResult> Index(string filterId, string sort)
        {            
            IQueryable<Equipment> equipments;
            List<FilterOptionModel> aryFo = new List<FilterOptionModel>();
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            if (filterId == null)
            {
                if (Session["equipmentFilterId"] != null)
                {
                    filterId = Session["equipmentFilterId"].ToString();
                }
            }

            ViewBag.filterId = filterId;

            //Get full equipment list
            equipments = from cc in db.Equipments
                         where !db.Equipments.Any(c => c.equipmentUidAsReplaced == cc.equipmentUid)
                         && cc.Location.projectUid == sessionProject
                         select cc;            
            
            aryFo = buildFilterDropDown(filterId, equipments);            
            equipments = applyFilter(filterId, equipments);
            equipments = applySorts(sort, equipments);
                        
            equipments = equipments.Include(e => e.ECM).Include(e => e.EquipmentAttributeType).Include(e => e.Location).Include(c=>c.Equipment2).OrderBy(c=>c.Location.location1).ThenBy(c=>c.EquipmentAttributeType.equipmentAttributeType1).ThenBy(c=>c.jciTag);
            ViewBag.equipmentTasks = db.EquipmentTasks;
            ViewBag.equipmentToDoes = db.EquipmentToDoes;
            ViewBag.equipmentAttributes = db.EquipmentAttributes.OrderBy(c=>c.equipmentAttribute1);
            ViewBag.equipment = equipments;
            ViewBag.filterList = aryFo.ToList();
            return View(await equipments.ToListAsync());
        }

        private IQueryable<Equipment> applyFilter(string filterId, IQueryable<Equipment> equipments)
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

                if (type == "E")
                {
                    equipments = equipments.Where(c => c.equipmentAttributeTypeUid.ToString() == uid);
                }
                else if (type == "C")
                {
                    equipments = equipments.Where(c => c.ecmUid.ToString() == uid);
                }
                else if (type == "X")
                {
                    equipments = equipments.Where(c=>c.equipmentUid == null);
                }
            }
            else
            {
                equipments = equipments.Where(c => c.equipmentUid == null);
            }
            Session["equipmentfilterId"] = filterId;
            return equipments;
        }

        private IQueryable<Equipment> applySorts(string sort, IQueryable<Equipment> equipments)
        {
            //apply sort
            if (!String.IsNullOrEmpty(sort))
            {
                string[] sortParts = sort.Split('|');
                string col = sortParts[0];
                string order = sortParts[1];

                if (col == "ECM")
                {
                    if (order == "desc")
                    {
                        equipments = equipments.OrderByDescending(c => c.ECM.ecmNumber);
                    }
                    else
                    {
                        equipments = equipments.OrderBy(c => c.ECM.ecmNumber);
                    }

                }

                if (col == "Location")
                {
                    if (order == "desc")
                    {
                        equipments = equipments.OrderByDescending(c => c.Location.location1);
                    }
                    else
                    {
                        equipments = equipments.OrderBy(c => c.Location.location1);
                    }

                }

                if (col == "jciTag")
                {
                    if (order == "desc")
                    {
                        equipments = equipments.OrderByDescending(c => c.jciTag);
                    }
                    else
                    {
                        equipments = equipments.OrderBy(c => c.jciTag);
                    }

                }
            }     
            return equipments;
        }

        private List<FilterOptionModel> buildFilterDropDown(string filterId, IQueryable<Equipment> equipments)
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


            IQueryable<Equipment> results = equipments.GroupBy(c => c.equipmentAttributeTypeUid).Select(v => v.FirstOrDefault());

            foreach (var item in results.OrderBy(c => c.EquipmentAttributeType.equipmentAttributeType1))
            {
                wf = new FilterOptionModel();
                wf.text = item.EquipmentAttributeType.equipmentAttributeType1;
                wf.value = "E|" + item.equipmentAttributeTypeUid.ToString();
                wf.selected = (wf.value == filterId);
                aryFo.Add(wf);
            }

            results = equipments.GroupBy(c => c.ecmUid).Select(v => v.FirstOrDefault());

            foreach (var item in results.Where(c=>c.ecmUid != null).OrderBy(c => c.ECM.ecmNumber))
            {
                wf = new FilterOptionModel();
                wf.text = item.ECM.ecmString;
                wf.value = "C|" + item.ecmUid.ToString();
                wf.selected = (wf.value == filterId);
                aryFo.Add(wf);
            }
            return aryFo;
        }

        public async Task<ActionResult> IndexPartial(string ecmUid)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            var equipments = db.Equipments.Where(c => c.Location.projectUid == sessionProject);

            if (!String.IsNullOrEmpty(ecmUid))
            {
                equipments = equipments.Where(c => c.ecmUid.ToString() == ecmUid);
            }
            
            equipments = equipments.Include(e => e.ECM).Include(e => e.EquipmentAttributeType).Include(e => e.Location).OrderBy(c => c.jciTag);            
            ViewBag.equipmentTasks = db.EquipmentTasks;
            ViewBag.equipmentToDoes = db.EquipmentToDoes;
            ViewBag.equipmentAttributes = db.EquipmentAttributes;
            ViewBag.equipment = equipments;

            return PartialView(await equipments.ToListAsync());
        }

        // GET: Equipments/EngineerEdit/5
        public async Task<ActionResult> EngineerEdit(Guid? id, string returnURL)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipment equipment = await db.Equipments.FindAsync(id);
            if (equipment == null)
            {
                return HttpNotFound();
            }

            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            var replacementEqupments = db.Equipments.Where(c => c.Location.projectUid == sessionProject);

            var equipmentAttributeValues = from cc in db.EquipmentAttributeValues
                                           where cc.equipmentUid == equipment.equipmentUid
                                           select cc;

            ViewBag.equipmentAttributeValues = equipmentAttributeValues;
            ViewBag.equipmentAttributeTypeUid = new SelectList(db.EquipmentAttributeTypes, "equipmentAttributeTypeUid", "equipmentAttributeType1", equipment.equipmentAttributeTypeUid);
            ViewBag.locationUid = new SelectList(db.Locations.Where(c => c.projectUid == sessionProject).OrderBy(c=>c.location1), "locationUid", "location1", equipment.locationUid);
            ViewBag.heatTypeUid = new SelectList(db.HeatTypes, "heatTypeUid", "heatType1", equipment.heatTypeUid);
            ViewBag.controlTypeUid = new SelectList(db.ControlTypes, "controlTypeUid", "controlType1", equipment.controlTypeUid);            
            return View(equipment);
        }

        // POST: Equipments/EngineerEdit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EngineerEdit([Bind(Include = "equipmentUid,jciTag")] Equipment equipment, IEnumerable<HttpPostedFileBase> pics)
        {
            Guid sessionProject = MCVExtensions.getSessionProject();

            if (ModelState.IsValid)
            {
                //add pictures
                foreach (var file in pics)
                {
                    if (file != null)
                    {
                        EquipmentAttachment ea = new EquipmentAttachment();
                        int fileSize = file.ContentLength;
                        MemoryStream target = new MemoryStream();
                        file.InputStream.CopyTo(target);
                        byte[] data = target.ToArray();
                        ea.attachment = data;
                        ea.fileType = Path.GetExtension(file.FileName);
                        var docName = MCVExtensions.MakeValidFileName(file.FileName);
                        ea.documentName = docName;
                        ea.equipmentAttachmentUid = Guid.NewGuid();
                        ea.equipmentUid = equipment.equipmentUid;
                        ea.equipmentAttachment1 = Path.GetFileNameWithoutExtension(docName);
                        db.EquipmentAttachments.Add(ea);
                    }
                }
                try
                {
                    await db.SaveChangesAsync();
                    ViewBag.result = "Picture added successfully";
                    var jciTag = from cc in db.Equipments
                                 where cc.equipmentUid == equipment.equipmentUid
                                 select cc.jciTag;

                    Response.Redirect("/Equipments/EngineerEdit?id=" + equipment.equipmentUid.ToString());
                }
                catch (Exception ex)
                {
                    ViewBag.result = "Tag " + equipment.jciTag.ToString() + " failed with error:" + ex.Message;
                }
            }
            return View(equipment);
        }



        // GET: Equipments/EngineerCreate        
        public ActionResult EngineerCreate()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();                        
            decimal? jciTag = null;

            ViewBag.equipmentAttributes = db.EquipmentAttributes.OrderBy(c => c.equipmentAttribute1);            
            ViewBag.equipmentAttributeTypeUid = db.EquipmentAttributeTypes.OrderBy(c=>c.equipmentAttributeType1).ToSelectList(c => c.equipmentAttributeType1, c => c.equipmentAttributeTypeUid.ToString(), "");

            ViewBag.locationUid = db.Locations.Where(c => c.projectUid == sessionProject).OrderBy(c=>c.location1).ToSelectList(c => c.location1, c => c.locationUid.ToString(), "");
            ViewBag.heatTypeUid = db.HeatTypes.ToSelectList(c => c.heatType1, c => c.heatTypeUid.ToString(), "");
            ViewBag.controlTypeUid = db.ControlTypes.ToSelectList(c => c.controlType1, c => c.controlTypeUid.ToString(), "");
            ViewBag.equipmentConditionUid = db.EquipmentConditions.ToSelectList(c => c.equipmentCondition1, c => c.equipmentConditionUid.ToString(), "");
            ViewBag.jciTag = jciTag;

            return View();
        }

        // POST: Equipments/EngineerCreate
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EngineerCreate([Bind(Include = "equipmentUid,equipmentAttributeTypeUid,locationUid,jciTag,heatTypeUid,controlTypeUid,ownerTag,model,serialNumber,equipmentConditionUid")] Equipment equipment, IEnumerable<HttpPostedFileBase> pics)
        {
            Guid sessionProject = MCVExtensions.getSessionProject();



            var existingJCITag = from cc in db.Equipments
                                 where cc.Location.projectUid == sessionProject
                                 && cc.jciTag == equipment.jciTag
                                 select cc;
            if (existingJCITag.Count() == 0)
            {
                if (ModelState.IsValid)
                {
                    equipment.equipmentUid = Guid.NewGuid();
                    db.Equipments.Add(equipment);

                    //add attributes
                    foreach (var f in Request.Form)
                    {
                        if (f.ToString().Length == 36 && Request[f.ToString()].ToString() != "")
                        {
                            EquipmentAttributeValue eav = new EquipmentAttributeValue();
                            eav.equipmentAttributeUid = Guid.Parse(f.ToString());
                            eav.equipmentAttributeValue1 = Request[f.ToString()];
                            eav.equipmentUid = equipment.equipmentUid;
                            eav.equipmentAttributeValueUid = Guid.NewGuid();
                            db.EquipmentAttributeValues.Add(eav);
                        }

                    }

                    //add pictures
                    foreach (var file in pics)
                    {
                        if (file != null)
                        {
                            EquipmentAttachment ea = new EquipmentAttachment();
                            int fileSize = file.ContentLength;
                            MemoryStream target = new MemoryStream();
                            file.InputStream.CopyTo(target);
                            byte[] data = target.ToArray();
                            ea.attachment = data;
                            ea.fileType = Path.GetExtension(file.FileName);
                            var docName = MCVExtensions.MakeValidFileName(file.FileName);
                            ea.documentName = docName;
                            ea.equipmentAttachmentUid = Guid.NewGuid();
                            ea.equipmentUid = equipment.equipmentUid;
                            ea.equipmentAttachment1 = Path.GetFileNameWithoutExtension(docName);
                            db.EquipmentAttachments.Add(ea);
                        }
                    }
                    try
                    {
                        await db.SaveChangesAsync();
                        ViewBag.result = "Tag " + equipment.jciTag.ToString() + " added successfully";
                        Response.Redirect("/Equipments/EngineerEdit?id=" + equipment.equipmentUid.ToString() + "&returnURL=EngineerCreate");                        
                    }
                    catch (Exception ex)
                    {
                        ViewBag.result = "Tag " + equipment.jciTag.ToString() + " failed with error:" + ex.Message;
                    }
                }
            }
            else
            {
                var nextJCITag = from cc in db.Equipments
                                 where cc.Location.projectUid == sessionProject
                                 select cc.jciTag;
                
                ViewBag.result = "JCI Tag already exists. Please choose another";
            }

            ViewBag.ecmUid = db.ECMs.Where(c => c.projectUid == sessionProject).ToSelectList(c => c.ecmString, c => c.ecmUid.ToString(), equipment.ecmUid.ToString());
            ViewBag.equipmentAttributeTypeUid = new SelectList(db.EquipmentAttributeTypes, "equipmentAttributeTypeUid", "equipmentAttributeType1", equipment.equipmentAttributeTypeUid);
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1", equipment.locationUid);
            ViewBag.heatTypeUid = new SelectList(db.HeatTypes, "heatTypeUid", "heatType1",equipment.heatTypeUid);
            ViewBag.equipmentConditionUid = db.EquipmentConditions.ToSelectList(c => c.equipmentCondition1, c => c.equipmentConditionUid.ToString(), "");
            ViewBag.controlTypeUid = new SelectList(db.ControlTypes, "controlTypeUid", "controlType1", equipment.controlTypeUid);           
            
            return View(equipment);
        }

        // POST: Equipments/AddPicturesForEquipment
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPicturesForEquipment([Bind(Include = "equipmentUid,jciTag")] Equipment equipment, IEnumerable<HttpPostedFileBase> pics)
        {
            Guid sessionProject = MCVExtensions.getSessionProject();
          
            if (ModelState.IsValid)
            {
                //add pictures
                foreach (var file in pics)
                {
                    if (file != null)
                    {
                        EquipmentAttachment ea = new EquipmentAttachment();                        
                        int fileSize = file.ContentLength;
                        MemoryStream target = new MemoryStream();
                        file.InputStream.CopyTo(target);
                        byte[] data = target.ToArray();
                        ea.attachment = data;
                        ea.fileType = Path.GetExtension(file.FileName);
                        var docName = MCVExtensions.MakeValidFileName(file.FileName);
                        ea.documentName = docName;
                        ea.equipmentAttachmentUid = Guid.NewGuid();
                        ea.equipmentUid = equipment.equipmentUid;
                        ea.equipmentAttachment1 = Path.GetFileNameWithoutExtension(docName);
                        db.EquipmentAttachments.Add(ea);
                    }
                }
                try
                {
                    await db.SaveChangesAsync();
                    ViewBag.result = "Picture added successfully";
                    var jciTag = from cc in db.Equipments
                                 where cc.equipmentUid == equipment.equipmentUid
                                 select cc.jciTag;

                    Response.Redirect("/Equipments/AddPicturesForEquipment?jciTag=" + Convert.ToInt16(jciTag.FirstOrDefault()));
                }
                catch (Exception ex)
                {
                    ViewBag.result = "Tag " + equipment.jciTag.ToString() + " failed with error:" + ex.Message;
                }
            }
            
            

            return View(equipment);
        }


        public ActionResult showImage(string id)
        {
            var image = from cc in db.EquipmentAttachments
                        where cc.equipmentAttachmentUid.ToString() == id
                        select cc;

            var stream = new MemoryStream(image.FirstOrDefault().attachment.ToArray());
            return new FileStreamResult(stream, "image/jpeg");
        }
        

        // GET: GridEdit
        public async Task<ActionResult> GridEdit(string filter, string filterId, string sort)
        {
            IQueryable<Equipment> equipments;
            List<FilterOptionModel> aryFo = new List<FilterOptionModel>();
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            if (filterId == null)
            {
                if (Session["equipmentFilterId"] != null)
                {
                    filterId = Session["equipmentFilterId"].ToString();
                }
            }

            ViewBag.filterId = filterId;

            //Get full equipment list
            equipments = from cc in db.Equipments
                         where !db.Equipments.Any(c => c.equipmentUidAsReplaced == cc.equipmentUid)
                         && cc.Location.projectUid == sessionProject
                         select cc;

            aryFo = buildFilterDropDown(filterId, equipments);
            equipments = applyFilter(filterId, equipments);
            equipments = applySorts(sort, equipments);
            
            ViewBag.filterList = aryFo.ToList();
            ViewBag.equipmentTasks = db.EquipmentTasks;
            ViewBag.equipmentToDoes = db.EquipmentToDoes;
            ViewBag.equipment = equipments.OrderBy(c=>c.jciTag);

            return View(await equipments.OrderBy(c => c.jciTag).ToListAsync());
        }

        public async Task<ActionResult> AddPicturesForEquipment(int? jciTag)
        {
            Equipment equipment;
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            if (jciTag != null)
            {
                var equipments = from cc in db.Equipments
                                 where cc.jciTag.ToString().StartsWith(jciTag.ToString())
                                 && cc.Location.projectUid == sessionProject
                                 orderby cc.jciTag
                                 select cc;

                if (equipments.Count() > 0)
                {
                    equipment = equipments.First();
                }
                else
                {
                    equipment = null;
                }
            }
            else
            {
                equipment = null;
            }
            return View(equipment);
        }
        
        public async Task<ActionResult> GridEditPartial(string filter, string filterId, string sort)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            List<FilterOptionModel> aryFo = new List<FilterOptionModel>();

            //apply session project predicate
            var equipments = from cc in db.Equipments
                             where cc.Location.projectUid == sessionProject
                             select cc;


            equipments = applyFilter(filterId, equipments);
            equipments = equipments.Include(w => w.Location).OrderBy(n => n.Location.location1).ThenBy(n=>n.jciTag);

            aryFo = buildFilterDropDown(filterId, equipments);

            ViewBag.filterList = aryFo.ToList();
            ViewBag.equipmentTasks = db.EquipmentTasks;
            ViewBag.equipmentToDoes = db.EquipmentToDoes;
            ViewBag.equipment = equipments.OrderBy(c => c.jciTag);

            return PartialView(await equipments.OrderBy(c => c.jciTag).ToListAsync());
        }

        // GET: Equipments/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipment equipment = await db.Equipments.FindAsync(id);
            if (equipment == null)
            {
                return HttpNotFound();
            }
            return View(equipment);
        }

        // GET: Equipments/Create
        public ActionResult Create()
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            string equipmentUidAsReplaced = Request.QueryString["equipmentUidAsReplaced"];
            string ecmUid = "";
            string equipmentAttributeTypeUid = "";
            string locationUid = "";
            decimal? jciTag = null;

            if (!String.IsNullOrEmpty(equipmentUidAsReplaced))
            {
                Equipment eq = db.Equipments.Find(Guid.Parse(equipmentUidAsReplaced));
                ecmUid = eq.ecmUid.ToString();
                equipmentAttributeTypeUid = eq.equipmentAttributeTypeUid.ToString();
                locationUid = eq.locationUid.ToString();
                jciTag = eq.jciTag;
            }           

            var replacementEqupments = db.Equipments.Where(c => c.Location.projectUid == sessionProject);
            var ecms = db.ECMs.Where(c => c.projectUid == sessionProject).OrderBy(c=>c.ecmNumber);

            ViewBag.equipmentTasks = db.EquipmentTasks;
            ViewBag.equipmentToDoes = db.EquipmentToDoes;
            ViewBag.ecms = ecms.ToSelectList(c => c.ecmString, c => c.ecmUid.ToString(), ecmUid);
            ViewBag.equipmentAttributeTypeUid = new SelectList(db.EquipmentAttributeTypes, "equipmentAttributeTypeUid", "equipmentAttributeType1", equipmentAttributeTypeUid);
            ViewBag.locationUid = new SelectList(db.Locations.Where(c => c.projectUid == sessionProject), "locationUid", "location1", locationUid);
            ViewBag.equipmentUidAsReplaced = replacementEqupments.OrderBy(c => c.jciTag).ToSelectList(d => d.jciTag + " - " + d.Location.location1, d => d.equipmentUid.ToString(), equipmentUidAsReplaced);
            ViewBag.jciTag = jciTag;
            
            return View();
        }

        // POST: Equipments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "equipmentUid,equipmentAttributeTypeUid,ecmUid,locationUid,jciTag,ownerTag,manufacturer,model,serialNumber,installDate,area,isNewToSite,useReplacement,price,equipmentConditionUid")] Equipment equipment, string ecms, string equipmentUidAsReplaced, string newTasks)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();
            if (ModelState.IsValid)
            {
                equipment.equipmentUid = Guid.NewGuid();                
                if (ecms == Guid.Empty.ToString())
                {
                    equipment.ecmUid = null;
                }
                else
	            {
                    equipment.ecmUid = Guid.Parse(ecms);
	            }                
                
                if (equipmentUidAsReplaced != Guid.Empty.ToString())
                {                    
                    equipment.equipmentUidAsReplaced = Guid.Parse(equipmentUidAsReplaced);
                }
               
                db.Equipments.Add(equipment);
                if (newTasks.Length > 0)
                {
                    foreach (var guid in newTasks.Remove(newTasks.Length - 1).Split(','))
                    {
                        EquipmentToDo eq = new EquipmentToDo();
                        eq.equipmentToDoUid = Guid.NewGuid();
                        eq.equipmentTaskUid = Guid.Parse(guid);
                        eq.equipmentUid = equipment.equipmentUid;
                        db.EquipmentToDoes.Add(eq);
                    }
                }
                await db.SaveChangesAsync();               
                
                return RedirectToAction(Request.QueryString["returnURL"]);
            }

            ViewBag.ecmUid = db.ECMs.Where(c => c.projectUid == sessionProject).ToSelectList(c => c.ecmString, c => c.ecmUid.ToString(), "");
            ViewBag.equipmentAttributeTypeUid = new SelectList(db.EquipmentAttributeTypes, "equipmentAttributeTypeUid", "equipmentAttributeType1", equipment.equipmentAttributeTypeUid);
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1", equipment.locationUid);
            return View(equipment);
        }

        // GET: Equipments/Edit/5
        public async Task<ActionResult> Edit(Guid? id, string returnURL)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipment equipment = await db.Equipments.FindAsync(id);
            if (equipment == null)
            {
                return HttpNotFound();
            }

            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            
            var replacementEqupments = db.Equipments.Where(c => c.Location.projectUid == sessionProject);

            var equipmentAttributeValues = from cc in db.EquipmentAttributeValues
                                           where cc.equipmentUid == equipment.equipmentUid
                                           select cc;

            ViewBag.equipmentAttributeValues = equipmentAttributeValues;
            ViewBag.ecmUid = db.ECMs.Where(c => c.projectUid == sessionProject).ToSelectList(c=>c.ecmString, c=>c.ecmUid.ToString() , equipment.ecmUid.ToString());
            ViewBag.equipmentAttributeTypeUid = new SelectList(db.EquipmentAttributeTypes, "equipmentAttributeTypeUid", "equipmentAttributeType1", equipment.equipmentAttributeTypeUid);
            ViewBag.locationUid = new SelectList(db.Locations.Where(c => c.projectUid == sessionProject), "locationUid", "location1", equipment.locationUid);
            ViewBag.equipmentUidAsReplaced = replacementEqupments.OrderBy(c=>c.jciTag).ToSelectList(d => d.Location.location1 + " - " + d.jciTag, d => d.equipmentUid.ToString(), equipment.equipmentUidAsReplaced.ToString());
            ViewBag.equipmentConditionUid = new SelectList(db.EquipmentConditions, "equipmentConditionUid", "equipmentCondition1", equipment.equipmentConditionUid);
            ViewBag.equipmentTasks = db.EquipmentTasks;
            ViewBag.equipmentToDoes = db.EquipmentToDoes;
            return View(equipment);
        }

        // POST: Equipments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "equipmentUid,equipmentAttributeTypeUid,ecmUid,locationUid,jciTag,ownerTag,manufacturer,model,serialNumber,installDate,area,equipmentUidAsReplaced,isNewToSite,price,useReplacement,equipmentConditionUid")] Equipment equipment, string ecms, string equipmentUidAsReplaced)
        {
            Guid sessionProject = JCIExtensions.MCVExtensions.getSessionProject();

            if (ModelState.IsValid)
            {
                db.Entry(equipment).State = EntityState.Modified;
                if (equipmentUidAsReplaced == Guid.Empty.ToString())
                {
                    equipment.equipmentUidAsReplaced = null;
                }
                if (equipment.ecmUid.ToString() == Guid.Empty.ToString())
                {
                    equipment.ecmUid = null;
                }

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ecmUid = db.ECMs.Where(c => c.projectUid == sessionProject).ToSelectList(c => c.ecmString, c => c.ecmUid.ToString(), equipment.ecmUid.ToString());
            ViewBag.equipmentAttributeTypeUid = new SelectList(db.EquipmentAttributeTypes, "equipmentAttributeTypeUid", "equipmentAttributeType1", equipment.equipmentAttributeTypeUid);
            ViewBag.equipmentConditionUid = new SelectList(db.EquipmentConditions, "equipmentConditionUid", "equipmentCondition1", equipment.equipmentConditionUid);
            ViewBag.locationUid = new SelectList(db.Locations, "locationUid", "location1", equipment.locationUid);
            return View(equipment);
        }

        // GET: Equipments/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipment equipment = await db.Equipments.FindAsync(id);
            if (equipment == null)
            {
                return HttpNotFound();
            }
            return View(equipment);
        }

        // POST: Equipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Equipment equipment = await db.Equipments.FindAsync(id);
            db.Equipments.Remove(equipment);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: EquipmentToDoes/SaveUseReplacement/5
        public async Task<ActionResult> SaveUseReplacement(string id, string value)
        {

            Equipment eq = db.Equipments.Find(Guid.Parse(id));

            if (value == "true")
            {
                eq.useReplacement = true;
                db.Entry(eq).State = EntityState.Modified;
                try
                {                    
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            else
            {
                eq.useReplacement = false;
                db.Entry(eq).State = EntityState.Modified;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public Task<string> equipments { get; set; }
    }
}
