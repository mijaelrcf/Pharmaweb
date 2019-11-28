using Microsoft.AspNet.Identity.EntityFramework;
using Pharmaweb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using System.Net;
using Microsoft.AspNet.Identity;

namespace Pharmaweb.Controllers
{
    [Authorize(Roles = "Usuarios, Administrador")]
    public class UsuariosController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        //
        // GET: /Usuarios/
        public ActionResult Index(string filter = null, int page = 1, int pageSize = 10, string sort = "UserName", string sortdir = "ASC")
        {
            var records = new PagedList<IdentityUser>();
            ViewBag.filter = filter;            

            records.Content = (from itemUsuario in context.Users.ToList()
                               select new IdentityUser
                               {
                                   Id = itemUsuario.Id,
                                   UserName = itemUsuario.UserName,
                                   PasswordHash = itemUsuario.PasswordHash
                               }).Where(x => filter == null || x.UserName.Contains(filter))
                                .OrderBy(sort + " " + sortdir)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            // Count
            records.TotalRecords = (from itemUsuario in context.Users.ToList()
                                    select new IdentityUser
                                    {
                                        Id = itemUsuario.Id,
                                        UserName = itemUsuario.UserName,
                                        PasswordHash = itemUsuario.PasswordHash
                                    }).Where(x => filter == null || x.UserName.Contains(filter))
                                    .Count();

            records.CurrentPage = page;
            records.PageSize = pageSize;

            return View(records);
        }

        //
        // GET: /Usuarios/Details
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var thisUser = context.Users.Find(id);
            
            if (thisUser == null)
            {
                return HttpNotFound();
            }

            return PartialView(thisUser);
        }

        //
        // GET: /Usuarios/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        //
        // POST: /Usuarios/Create
        [HttpPost]
        public JsonResult Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

                    var user = new ApplicationUser() { UserName = model.UserName };
                    var result = manager.Create(user, model.Password);
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, mensajeError = ex.Message });
                }
            }
            else
            {
                string mensajeError = "Error de validación: " + string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return Json(new { success = false, mensajeError }, JsonRequestBehavior.DenyGet);
            }
        }

        //
        // GET: /Usuarios/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var thisUser = context.Users.Find(id);

            RegisterViewModel usuario = new RegisterViewModel() 
            {
                IdUsuario = thisUser.Id,
                UserName = thisUser.UserName,
                Password = thisUser.PasswordHash,
                ConfirmPassword = thisUser.PasswordHash
            };

            if (thisUser == null)
            {
                return HttpNotFound();
            }

            return PartialView(usuario);
        }

        //
        // POST: /Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

                    var result = manager.RemovePassword(model.IdUsuario);
                    
                    if (result.Succeeded)
                    {
                        var result2 = manager.AddPassword(model.IdUsuario, model.Password);
                        
                        if(result2.Succeeded)
                            return Json(new { success = true });
                        else
                            return Json(new { success = false, result2.Errors }, JsonRequestBehavior.DenyGet);
                    }
                    else
                    {
                        return Json(new { success = false, result.Errors }, JsonRequestBehavior.DenyGet);
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, mensajeError = ex.Message });
                }
            }
            else
            {
                string mensajeError = "Error de validación: " + string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return Json(new { success = false, mensajeError }, JsonRequestBehavior.DenyGet);
            }
        }

        //
        // GET: /Usuarios/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var thisUser = context.Users.Find(id);

            RegisterViewModel usuario = new RegisterViewModel()
            {
                IdUsuario = thisUser.Id,
                UserName = thisUser.UserName,
                Password = thisUser.PasswordHash
            };

            if (thisUser == null)
            {
                return HttpNotFound();
            }

            return PartialView(usuario);
        }

        //
        // POST: /Usuarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(RegisterViewModel model)
        {
            try
            {
                var thisUser = context.Users.Find(model.IdUsuario);

                context.Users.Remove(thisUser);
                context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensajeError = ex.Message });
            }
        }
	}
}