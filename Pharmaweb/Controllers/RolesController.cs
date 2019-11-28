using Pharmaweb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
//using System.Linq.Dynamic;

namespace Pharmaweb.Controllers
{
    [Authorize(Roles = "Roles, Administrador")]
    public class RolesController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        //
        // GET: /Roles/
        public ActionResult Index()
        {
            var roles = context.Roles.OrderBy(x => x.Name).ToList();
            return View(roles);
        }

        //
        // GET: /Roles/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var thisRole = context.Roles.Find(id);

            if (thisRole == null)
            {
                return HttpNotFound();
            }

            return PartialView(thisRole);
        }

        //
        // GET: /Roles/Create
        public ActionResult Create()
        {
            //var rol = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();

            return PartialView();
        }

        //
        // POST: /Roles/Create
        [HttpPost]
        public JsonResult Create(Microsoft.AspNet.Identity.EntityFramework.IdentityRole collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    context.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole()
                    {
                        Name = collection.Name
                    });
                    context.SaveChanges();

                    return Json(new { success = true });
                }
                catch(Exception ex)
                {
                    return Json(new { success = false, mensajeError = ex.Message});
                }
            }

            return Json(collection, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Roles/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var thisRole = context.Roles.Find(id);

            if (thisRole == null)
            {
                return HttpNotFound();
            }

            return PartialView(thisRole);
        }

        //
        // POST: /Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Microsoft.AspNet.Identity.EntityFramework.IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    context.Entry(role).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, mensajeError = ex.Message });
                }
            }

            return PartialView("Edit", role);
        }

        //
        // GET: /Roles/Edit/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var thisRole = context.Roles.Find(id);

            if (thisRole == null)
            {
                return HttpNotFound();
            }

            return PartialView(thisRole);
        }

        //
        // POST: /Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Microsoft.AspNet.Identity.EntityFramework.IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var thisRole = context.Roles.Find(role.Id);

                    context.Roles.Remove(thisRole);
                    context.SaveChanges();

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, mensajeError = ex.Message });
                }
            }

            return PartialView("Edit", role);
        }
	}
}