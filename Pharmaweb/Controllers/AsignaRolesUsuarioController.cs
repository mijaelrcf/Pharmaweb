using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Pharmaweb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharmaweb.Controllers
{
    [Authorize(Roles = "AsignaRolesUsuario, Administrador")]
    public class AsignaRolesUsuarioController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        //
        // GET: /AsignaRolesUsuario/
        public ActionResult Index()
        {
            List<Pharmaweb.Models.ApplicationUser> usuario = context.Users.ToList();  
            return View(usuario);
        }

        //
        // GET: /AsignaRolesUsuario/Create
        public ActionResult Create(string idUsuario)
        {
            var roles = context.Roles.ToList();

            AsignaRolesUsuarioModels asignaRolesUsuario = new AsignaRolesUsuarioModels();
            List<AsignaRolesUsuario> lstAsignaRolesUsuario = new List<AsignaRolesUsuario>();
            
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                        
            lstAsignaRolesUsuario = (from item in roles
                                     select new AsignaRolesUsuario
                                     { 
                                         IdRol = item.Id,
                                         Rol = item.Name,
                                         Asignado = manager.IsInRole(idUsuario, item.Name)                                         
                                     }).OrderBy(x => x.Rol).ToList();

            asignaRolesUsuario.RolesUsuarioList = lstAsignaRolesUsuario;
            asignaRolesUsuario.IdUsuario = idUsuario;

            return PartialView(asignaRolesUsuario);
        }

        //
        // POST: /AsignaRolesUsuario/Create
        [HttpPost]
        public ActionResult Create(AsignaRolesUsuarioModels asignaRolesUsuarioList)
        {
            try
            {
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                foreach(var item in asignaRolesUsuarioList.RolesUsuarioList)
                {
                    if(item.Asignado)
                        manager.AddToRole(asignaRolesUsuarioList.IdUsuario, item.Rol);                        
                    else
                        if (manager.IsInRole(asignaRolesUsuarioList.IdUsuario, item.Rol))
                            manager.RemoveFromRole(asignaRolesUsuarioList.IdUsuario, item.Rol);
                }
               
                return Json(new { success = true });
            }
            catch(Exception ex)
            {
                return Json(new { success = false, mensajeError = ex.Message});
            }
        }
    }
}
