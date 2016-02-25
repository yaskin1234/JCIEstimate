using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using JCIEstimate.Models;
using JCIExtensions;

namespace IdentitySample.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersAdminController : Controller
    {

        private JCIEstimateEntities db = new JCIEstimateEntities();

        public UsersAdminController()
        {
        }

        public UsersAdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        //
        // GET: /Users/
        public async Task<ActionResult> Index()
        {
            return View(await UserManager.Users.OrderBy(c=>c.Email).ToListAsync());
        }

        //
        // GET: /Users/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);

            ViewBag.RoleNames = await UserManager.GetRolesAsync(user.Id);

            return View(user);
        }

        //
        // GET: /Users/Create
        public async Task<ActionResult> Create()
        {
            //Get the list of Roles
            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
            return View();
        }

        public async Task<ActionResult> MassEmail(string filterId)
        {

            IQueryable<MassEmailViewModel> list;
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
            wf.text = "-- All --";
            wf.value = "A|" + Guid.Empty.ToString();
            wf.selected = (wf.value == filterId);
            aryFo.Add(wf);


            foreach (var item in db.Projects.GroupBy(c => c.projectUid).Select(v => v.FirstOrDefault()))
            {
                wf = new FilterOptionModel();
                wf.text = item.project1;
                wf.value = "P|" + item.projectUid.ToString();
                wf.selected = (wf.value == filterId);
                aryFo.Add(wf);
            }

            list = from cc in db.AspNetUsers
                   join ff in db.AspNetUsersExtensions on cc.Id equals ff.aspnetUserUid
                   where 1 == 0
                   select new MassEmailViewModel
                    {
                        id = cc.Id,
                        Name = cc.AspNetUsersExtensions.FirstOrDefault().name,
                        PhoneNumber = cc.PhoneNumber,
                        Email = cc.Email
                    };

            if (type == "A")
            {
                list = from cc in db.AspNetUsers
                       join ff in db.AspNetUsersExtensions on cc.Id equals ff.aspnetUserUid                       
                       select new MassEmailViewModel
                       {
                           id = cc.Id,
                           Name = cc.AspNetUsersExtensions.FirstOrDefault().name,
                           PhoneNumber = cc.PhoneNumber,
                           Email = cc.Email
                       };
            }
            else if(type == "P")
            {
                Guid val = Guid.Parse(uid);
                list = from cc in db.AspNetUsers
                join ff in db.AspNetUsersExtensions on cc.Id equals ff.aspnetUserUid
                join pp in db.ProjectUsers on cc.Id equals pp.aspNetUserUid
                where pp.projectUid == val
                select new MassEmailViewModel
                {
                    id = cc.Id,
                    Name = cc.AspNetUsersExtensions.FirstOrDefault().name,
                    PhoneNumber = cc.PhoneNumber,
                    Email = cc.Email
                };
            }

            ViewBag.list = list.OrderBy(c=>c.Email).ToList();            
            ViewBag.recipientList = aryFo.ToSelectList(c => c.text, c => c.value, wf.selected.ToString(),false);


            return View();
        }

        //
        // POST: /Users/Create
        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel userViewModel, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { 
                    UserName = userViewModel.Email, 
                    Email = userViewModel.Email,
                    AllowableContractors = userViewModel.AllowableContractors,                    
                    PhoneNumber = userViewModel.PhoneNumber
                };
                var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);

                                

                //Add User to the selected Roles 
                if (adminresult.Succeeded)
                {
                    AspNetUsersExtension newExt = new AspNetUsersExtension();
                    newExt.aspNetUsersExtensionUid = Guid.Parse(user.Id);
                    newExt.aspnetUserUid = user.Id;
                    newExt.name = userViewModel.name;
                    db.AspNetUsersExtensions.Add(newExt);
                    db.SaveChanges();

                    if (selectedRoles != null)
                    {
                        var result = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First());
                            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
                            return View();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", adminresult.Errors.First());
                    ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
                    return View();

                }
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
            return View();
        }

        //
        // GET: /Users/Edit/1
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userRoles = await UserManager.GetRolesAsync(user.Id);


            var foundName = await db.AspNetUsersExtensions.FindAsync(Guid.Parse(id));

            var name = "";
            if (foundName == null)
            {
                name = "";
            }
            else
            {
                name = foundName.name;
            }

            return View(new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                AllowableContractors = user.AllowableContractors,
                PhoneNumber = user.PhoneNumber,
                name = name,
                RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
                {
                    Selected = userRoles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                })
            });
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Email,Id,AllowableContractors,phoneNumber,name")] EditUserViewModel editUser, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(editUser.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                var ext = db.AspNetUsersExtensions.Find(Guid.Parse(editUser.Id));
                if (ext == null)
                {
                    AspNetUsersExtension newExt = new AspNetUsersExtension();
                    newExt.aspNetUsersExtensionUid = Guid.Parse(editUser.Id);
                    newExt.aspnetUserUid = editUser.Id;
                    newExt.name = editUser.name;
                    db.AspNetUsersExtensions.Add(newExt);
                }
                else
                {
                    ext.name = editUser.name;
                }
                db.SaveChanges();

                user.UserName = editUser.Email;
                user.Email = editUser.Email;
                user.AllowableContractors = editUser.AllowableContractors;                
                user.PhoneNumber = editUser.PhoneNumber;

                var userRoles = await UserManager.GetRolesAsync(user.Id);

                selectedRole = selectedRole ?? new string[] { };

                var result = await UserManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                result = await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something failed.");
            return View();
        }

        //
        // GET: /Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
