using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Data;
using Pharmaweb.Models;
using System.Linq.Dynamic;

namespace Pharmaweb.Controllers
{
    [Authorize(Roles = "Sucursal, Administrador")]
    public class SucursalController : Controller
    {
        private PharmawebEntities db = new PharmawebEntities();

        // GET: /Sucursal/
        public ActionResult Index(string filter = null, int page = 1, int pageSize = 10, string sort = "NombreSucursal", string sortdir = "DESC")
        {
            var records = new PagedList<SucursalModels>();
            ViewBag.filter = filter;

            records.Content = (from item in db.Sucursal.ToList()
                               select new SucursalModels
                               {
                                   CatDepartamento = item.CatDepartamento,
                                   Direccion = item.Direccion,
                                   Fax = item.Fax,
                                   FechaCreacion = item.FechaCreacion,
                                   FechaModificacion = Convert.ToDateTime(item.FechaModificacion),
                                   NombreSucursal = item.NombreSucursal,
                                   SucursalId = item.SucursalId,
                                   Telefono = item.Telefono,
                                   UsuarioCreacion = item.UsuarioCreacion,
                                   UsuarioModificacion = item.UsuarioModificacion
                               }).Where(x => filter == null || (x.NombreSucursal.Contains(filter.ToUpper())))
                                .OrderBy(sort + " " + sortdir)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            // Count
            records.TotalRecords = (from item in db.Sucursal.ToList()
                                    select new SucursalModels
                                    {
                                        CatDepartamento = item.CatDepartamento,
                                        Direccion = item.Direccion,
                                        Fax = item.Fax,
                                        FechaCreacion = item.FechaCreacion,
                                        FechaModificacion = Convert.ToDateTime(item.FechaModificacion),
                                        NombreSucursal = item.NombreSucursal,
                                        SucursalId = item.SucursalId,
                                        Telefono = item.Telefono,
                                        UsuarioCreacion = item.UsuarioCreacion,
                                        UsuarioModificacion = item.UsuarioModificacion
                                    }).Where(x => filter == null || (x.NombreSucursal.Contains(filter.ToUpper())))
                                    .Count();

            records.CurrentPage = page;
            records.PageSize = pageSize;

            return View(records);
        }

        // GET: /Sucursal/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var sucursalBD = db.Sucursal.Find(id);

            SucursalModels sucursal = new SucursalModels()
            {
                CatDepartamento = sucursalBD.CatDepartamento,
                Direccion = sucursalBD.Direccion,
                Fax = sucursalBD.Fax,
                FechaCreacion = sucursalBD.FechaCreacion,
                FechaModificacion = Convert.ToDateTime(sucursalBD.FechaModificacion),
                NombreSucursal = sucursalBD.NombreSucursal,
                SucursalId = sucursalBD.SucursalId,
                Telefono = sucursalBD.Telefono,
                UsuarioCreacion = sucursalBD.UsuarioCreacion,
                UsuarioModificacion = sucursalBD.UsuarioModificacion,

                lstCatDepartamento = db.CatalogoDetalle.Where(x => x.CatalogoId == "DEPARTAMENTO").Select(x => new SelectListItem
                {
                    Text = x.Detalle,
                    Value = x.Valor
                }).ToList(),
            };

            if (sucursal == null)
            {
                return HttpNotFound();
            }

            return PartialView(sucursal);
        }

        // GET: /Sucursal/Create
        public ActionResult Create()
        {
            SucursalModels sucursal = new SucursalModels()
            {
                lstCatDepartamento = db.CatalogoDetalle.Where(x => x.CatalogoId == "DEPARTAMENTO").Select(x => new SelectListItem
                {
                    Text = x.Detalle,
                    Value = x.Valor
                }).ToList(),
            };
            return PartialView(sucursal);
        }

        // POST: /Sucursal/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SucursalModels sucursal)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Sucursal sucursalDB = new Sucursal()
                    {
                        CatDepartamento = sucursal.CatDepartamento,
                        Direccion = sucursal.Direccion.ToUpper(),
                        Fax = sucursal.Fax,
                        NombreSucursal = sucursal.NombreSucursal.ToUpper(),
                        SucursalId = sucursal.SucursalId,
                        Telefono = sucursal.Telefono,
                        UsuarioModificacion = Convert.ToString(Session["UserName"]),
                        FechaModificacion = DateTime.Now,
                        UsuarioCreacion = Convert.ToString(Session["UserName"]),
                        FechaCreacion = DateTime.Now
                    };

                    db.Sucursal.Add(sucursalDB);
                    db.SaveChanges();

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

        // GET: /Sucursal/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var sucursalBD = db.Sucursal.Find(id);

            SucursalModels sucursal = new SucursalModels()
            {
                CatDepartamento = sucursalBD.CatDepartamento,
                Direccion = sucursalBD.Direccion,
                Fax = sucursalBD.Fax,
                FechaCreacion = sucursalBD.FechaCreacion,
                FechaModificacion = sucursalBD.FechaModificacion,
                NombreSucursal = sucursalBD.NombreSucursal,
                SucursalId = sucursalBD.SucursalId,
                Telefono = sucursalBD.Telefono,
                UsuarioCreacion = sucursalBD.UsuarioCreacion,
                UsuarioModificacion = sucursalBD.UsuarioModificacion,
                lstCatDepartamento = db.CatalogoDetalle.Where(x => x.CatalogoId == "DEPARTAMENTO").Select(x => new SelectListItem
                {
                    Text = x.Detalle,
                    Value = x.Valor
                }).ToList(),
            };

            if (sucursal == null)
            {
                return HttpNotFound();
            }

            return PartialView(sucursal);
        }

        // POST: /Sucursal/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SucursalModels sucursal)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Sucursal sucursalDB = new Sucursal()
                    {
                        CatDepartamento = sucursal.CatDepartamento,
                        Direccion = sucursal.Direccion.ToUpper(),
                        Fax = sucursal.Fax,
                        NombreSucursal = sucursal.NombreSucursal.ToUpper(),
                        SucursalId = sucursal.SucursalId,
                        Telefono = sucursal.Telefono,
                        UsuarioModificacion = Convert.ToString(Session["UserName"]),
                        FechaModificacion = DateTime.Now,
                        UsuarioCreacion = sucursal.UsuarioCreacion,
                        FechaCreacion = sucursal.FechaCreacion
                    };

                    db.Entry(sucursalDB).State = EntityState.Modified;
                    db.SaveChanges();

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

        // GET: /Sucursal/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var sucursalBD = db.Sucursal.Find(id);

            SucursalModels sucursal = new SucursalModels()
            {
                CatDepartamento = sucursalBD.CatDepartamento,
                Direccion = sucursalBD.Direccion,
                Fax = sucursalBD.Fax,
                FechaCreacion = sucursalBD.FechaCreacion,
                FechaModificacion = Convert.ToDateTime(sucursalBD.FechaModificacion),
                NombreSucursal = sucursalBD.NombreSucursal,
                SucursalId = sucursalBD.SucursalId,
                Telefono = sucursalBD.Telefono,
                UsuarioCreacion = sucursalBD.UsuarioCreacion,
                UsuarioModificacion = sucursalBD.UsuarioModificacion,
                lstCatDepartamento = db.CatalogoDetalle.Where(x => x.CatalogoId == "DEPARTAMENTO").Select(x => new SelectListItem
                {
                    Text = x.Detalle,
                    Value = x.Valor
                }).ToList(),
            };

            if (sucursal == null)
            {
                return HttpNotFound();
            }

            return PartialView(sucursal);
        }

        // POST: /Sucursal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(SucursalModels sucursal)
        {
            try
            {
                Sucursal sucursalBD = db.Sucursal.Find(sucursal.SucursalId);
                db.Sucursal.Remove(sucursalBD);
                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensajeError = ex.Message });
            }
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
