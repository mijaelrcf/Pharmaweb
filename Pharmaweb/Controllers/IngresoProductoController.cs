using Data;
using Pharmaweb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using System.Net;
using System.Data.Entity;

namespace Pharmaweb.Controllers
{
    [Authorize(Roles = "IngresoProducto, Administrador")]
    public class IngresoProductoController : Controller
    {
        PharmawebEntities db = new PharmawebEntities();
        //
        // GET: /IngresoProducto/
        public ActionResult Index(string filter = null, int page = 1, int pageSize = 10, string sort = "NroFactura", string sortdir = "DESC")
        {
            var records = new PagedList<IngresoProductoModels>();
            ViewBag.filter = filter;
            if(filter != null) filter = filter.ToUpper();

            records.Content = (from itemIngresoProducto in db.IngresoProducto.ToList()
                               join itemLaboratorio in db.Laboratorio.ToList()
                               on itemIngresoProducto.LaboratorioId equals itemLaboratorio.LaboratorioId
                               join itemSucursal in db.Sucursal.ToList()
                               on itemIngresoProducto.SucursalId equals itemSucursal.SucursalId
                               select new IngresoProductoModels
                               {
                                   IngresoProductoId = itemIngresoProducto.IngresoProductoId,
                                   NombreSucursal = itemSucursal.NombreSucursal,
                                   NombreLaboratorio = itemLaboratorio.NombreLaboratorio,
                                   CatTipoIngreso = itemIngresoProducto.CatTipoIngreso,
                                   NroFactura = itemIngresoProducto.NroFactura,
                                   Observacion = itemIngresoProducto.Observacion,                                   
                                   FechaCreacion = itemIngresoProducto.FechaCreacion
                               }).Where(x => filter == null || x.NombreSucursal.Contains(filter) || x.NombreLaboratorio.Contains(filter) || x.NroFactura.Contains(filter) || x.CatTipoIngreso.Contains(filter))
                                .OrderBy(sort + " " + sortdir)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();
            
            // Count
            records.TotalRecords = (from itemIngresoProducto in db.IngresoProducto.ToList()
                                    join itemLaboratorio in db.Laboratorio.ToList()
                                    on itemIngresoProducto.LaboratorioId equals itemLaboratorio.LaboratorioId
                                    join itemSucursal in db.Sucursal.ToList()
                                    on itemIngresoProducto.SucursalId equals itemSucursal.SucursalId
                                    select new IngresoProductoModels
                                    {
                                        IngresoProductoId = itemIngresoProducto.IngresoProductoId,
                                        NombreSucursal = itemSucursal.NombreSucursal,
                                        NombreLaboratorio = itemLaboratorio.NombreLaboratorio,
                                        CatTipoIngreso = itemIngresoProducto.CatTipoIngreso,
                                        NroFactura = itemIngresoProducto.NroFactura,
                                        Observacion = itemIngresoProducto.Observacion,
                                        FechaCreacion = itemIngresoProducto.FechaCreacion
                                    }).Where(x => filter == null || x.NombreSucursal.Contains(filter) || x.NombreLaboratorio.Contains(filter) || x.NroFactura.Contains(filter) || x.CatTipoIngreso.Contains(filter))
                                    .Count();

            records.CurrentPage = page;
            records.PageSize = pageSize;

            return View(records);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ICollection<IngresoProductoDetalle> ingresoProductoDetalleBD = db.IngresoProductoDetalle.Where(x => x.IngresoProductoId == id).ToList();

            var ingresoProductoDetalle = (from item in ingresoProductoDetalleBD
                                          join itemProducto in db.Producto.ToList()
                                          on item.ProductoId equals itemProducto.ProductoId
                                          select new IngresoProductoDetalleModels
                                          {
                                              IngresoProductoDetalleId = item.IngresoProductoDetalleId,
                                              IngresoProductoId = item.IngresoProductoId,
                                              ProductoId = item.ProductoId,
                                              NombreProducto = itemProducto.NombreProducto,
                                              CantidadPaquetes = Convert.ToDecimal(item.CantidadPaquetes),
                                              CantidadUnidades = Convert.ToDecimal(item.CantidadUnidades),
                                              FechaVencimiento = Convert.ToDateTime(item.FechaVencimiento)
                                          }).ToList();

            IngresoProducto ingresoProductoBD = db.IngresoProducto.Find(id);
            IngresoProductoModels ingresoProducto = new IngresoProductoModels()
            {
                IngresoProductoId = ingresoProductoBD.IngresoProductoId,
                SucursalId = ingresoProductoBD.SucursalId,
                LaboratorioId = ingresoProductoBD.LaboratorioId,
                CatTipoIngreso = ingresoProductoBD.CatTipoIngreso,
                NroFactura = ingresoProductoBD.NroFactura,
                Observacion = ingresoProductoBD.Observacion,
                FechaCreacion = ingresoProductoBD.FechaCreacion,
                Detalle = ingresoProductoDetalle,
                lstSucursal = db.Sucursal.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreSucursal,
                    Value = x.SucursalId.ToString()
                }).ToList(),
                lstLaboratorio = db.Laboratorio.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreLaboratorio,
                    Value = x.LaboratorioId.ToString()
                }).ToList(),
                lstCatTipoIngreso = db.CatalogoDetalle.Where(x => x.CatalogoId == "TIPO_INGRESO").Select(x => new SelectListItem
                {
                    Text = x.Detalle,
                    Value = x.Valor
                }).ToList(),
            };

            if (ingresoProducto == null)
            {
                return HttpNotFound();
            }

            return PartialView(ingresoProducto);
        }
        public ActionResult Create()
        {
            List<ProductoModels> lstProducto = new List<ProductoModels>();

            lstProducto = (from item in db.Producto.ToList()
                           select new ProductoModels
                           {
                               ProductoId = item.ProductoId, 
                               NombreProducto = item.NombreProducto, 
                           }).ToList();

            IngresoProductoModels ingresoProducto = new IngresoProductoModels()
            {
                lstSucursal = db.Sucursal.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreSucursal,
                    Value = x.SucursalId.ToString()
                }).ToList(),
                
                lstCatTipoIngreso = db.CatalogoDetalle.Where(x => x.CatalogoId == "TIPO_INGRESO").Select(x => new SelectListItem
                {
                    Text = x.Detalle,
                    Value = x.Valor
                }).ToList(),

                lstLaboratorio = db.Laboratorio.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreLaboratorio,
                    Value = x.LaboratorioId.ToString()
                }).ToList()
            };

            ViewBag.Productos = lstProducto;
            return PartialView(ingresoProducto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IngresoProductoModels modelo, string operacion = null)
        {
            if (ModelState.IsValid)
            {
                if (modelo.Detalle == null || modelo.Detalle.Count == 0)
                {
                    ModelState.AddModelError("Detalle", "Debe agregar al menos un detalle para la factura");
                }
                else
                {
                    IngresoProducto ingresoProductoDB = new IngresoProducto()
                    {
                        IngresoProductoId = modelo.IngresoProductoId,
                        SucursalId = modelo.SucursalId,
                        LaboratorioId = modelo.LaboratorioId,
                        CatTipoIngreso = modelo.CatTipoIngreso,
                        NroFactura = modelo.NroFactura,
                        Observacion = modelo.Observacion,
                        UsuarioModificacion = Convert.ToString(Session["UserName"]),
                        FechaModificacion = DateTime.Now,
                        UsuarioCreacion = Convert.ToString(Session["UserName"]),
                        FechaCreacion = DateTime.Now
                    };

                    db.IngresoProducto.Add(ingresoProductoDB);
                    db.SaveChanges();

                    Int64 ingresoProductoId = ingresoProductoDB.IngresoProductoId; // recupera el id generado
                    
                    //detalle
                    foreach(var item in modelo.Detalle)
                    {
                        IngresoProductoDetalle ingresoProductoDetalleDB = new IngresoProductoDetalle()
                        {
                            IngresoProductoDetalleId = item.IngresoProductoDetalleId,
                            IngresoProductoId = ingresoProductoId, 
                            ProductoId = item.ProductoId,
                            CantidadPaquetes = item.CantidadPaquetes,
                            CantidadUnidades = item.CantidadUnidades, 
                            FechaVencimiento = item.FechaVencimiento                        
                        };

                        db.IngresoProductoDetalle.Add(ingresoProductoDetalleDB);
                        db.SaveChanges();

                        var existeInventarioDB = (from itemInventario in db.Inventario.ToList()
                                                  where itemInventario.ProductoId == item.ProductoId && itemInventario.SucursalId == modelo.SucursalId
                                                  select itemInventario).FirstOrDefault();

                        if (existeInventarioDB == null)
                        {
                            Inventario inventarioDB = new Inventario()
                            {
                                SucursalId = modelo.SucursalId,
                                ProductoId = item.ProductoId,
                                CantIngresoPaquete = item.CantidadPaquetes,
                                CantIngresoUnidad = item.CantidadUnidades,
                                CantSalidaPaquete = 0,
                                CantSalidaUnidad = 0,
                                CantidadPaqueteTotal = item.CantidadPaquetes,
                                CantidadUnidadTotal = item.CantidadUnidades,
                                UsuarioModificacion = Convert.ToString(Session["UserName"]),
                                FechaModificacion = DateTime.Now,
                                UsuarioCreacion = Convert.ToString(Session["UserName"]),
                                FechaCreacion = DateTime.Now
                            };

                            db.Inventario.Add(inventarioDB);
                            db.SaveChanges();
                        }
                        else 
                        {
                            existeInventarioDB.CantIngresoPaquete += item.CantidadPaquetes;
                            existeInventarioDB.CantIngresoUnidad += item.CantidadUnidades;
                            existeInventarioDB.CantidadPaqueteTotal += item.CantidadPaquetes;
                            existeInventarioDB.CantidadUnidadTotal += item.CantidadUnidades;
                            existeInventarioDB.UsuarioModificacion = Convert.ToString(Session["UserName"]);
                            existeInventarioDB.FechaModificacion = DateTime.Now;

                            db.Entry(existeInventarioDB).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }

                return Json(new { success = true });
            }
            else
            {
                string mensajeError = "Error de validación: " + string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return Json(new { success = false, mensajeError }, JsonRequestBehavior.DenyGet);
            }
        }

        //
        // GET: /IngresoProducto/Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ICollection<IngresoProductoDetalle> ingresoProductoDetalleBD = db.IngresoProductoDetalle.Where(x => x.IngresoProductoId == id).ToList();

            var ingresoProductoDetalle = (from item in ingresoProductoDetalleBD
                                          join itemProducto in db.Producto.ToList()
                                          on item.ProductoId equals itemProducto.ProductoId
                                          select new IngresoProductoDetalleModels
                                          {
                                              IngresoProductoDetalleId = item.IngresoProductoDetalleId,
                                              IngresoProductoId = item.IngresoProductoId,
                                              ProductoId = item.ProductoId,
                                              NombreProducto = itemProducto.NombreProducto,
                                              CantidadPaquetes = Convert.ToDecimal(item.CantidadPaquetes),
                                              CantidadUnidades = Convert.ToDecimal(item.CantidadUnidades),
                                              FechaVencimiento = Convert.ToDateTime(item.FechaVencimiento)
                                          }).ToList();

            IngresoProducto ingresoProductoBD = db.IngresoProducto.Find(id);

            IngresoProductoModels ingresoProducto = new IngresoProductoModels()
            {
                IngresoProductoId = ingresoProductoBD.IngresoProductoId,
                SucursalId = ingresoProductoBD.SucursalId,
                LaboratorioId = ingresoProductoBD.LaboratorioId,
                CatTipoIngreso = ingresoProductoBD.CatTipoIngreso,
                NroFactura = ingresoProductoBD.NroFactura,
                Observacion = ingresoProductoBD.Observacion,
                FechaCreacion = ingresoProductoBD.FechaCreacion,
                Detalle = ingresoProductoDetalle,
                lstSucursal = db.Sucursal.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreSucursal,
                    Value = x.SucursalId.ToString()
                }).ToList(),
                lstLaboratorio = db.Laboratorio.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreLaboratorio,
                    Value = x.LaboratorioId.ToString()
                }).ToList(),
                lstCatTipoIngreso = db.CatalogoDetalle.Where(x => x.CatalogoId == "TIPO_INGRESO").Select(x => new SelectListItem
                {
                    Text = x.Detalle,
                    Value = x.Valor
                }).ToList(),
            };

            if (ingresoProducto == null)
            {
                return HttpNotFound();
            }

            return PartialView(ingresoProducto);


            //var roles = context.Roles.ToList();

            //AsignaRolesUsuarioModels asignaRolesUsuario = new AsignaRolesUsuarioModels();
            //List<AsignaRolesUsuario> lstAsignaRolesUsuario = new List<AsignaRolesUsuario>();

            //var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            //lstAsignaRolesUsuario = (from item in roles
            //                         select new AsignaRolesUsuario
            //                         {
            //                             IdRol = item.Id,
            //                             Rol = item.Name,
            //                             Asignado = manager.IsInRole(idUsuario, item.Name)
            //                         }).OrderBy(x => x.Rol).ToList();

            //asignaRolesUsuario.RolesUsuarioList = lstAsignaRolesUsuario;
            //asignaRolesUsuario.IdUsuario = idUsuario;

            //return PartialView(asignaRolesUsuario);
        }

        //
        // POST: /IngresoProducto/Edit
        [HttpPost]
        public ActionResult Edit(IngresoProductoModels ingresoProductoModels)
        {
            try
            {
                foreach (var item in ingresoProductoModels.Detalle)
                {
                    var ingresoProductoDetalle = db.IngresoProductoDetalle.Find(item.IngresoProductoDetalleId);
                    ingresoProductoDetalle.FechaVencimiento = item.FechaVencimiento;

                    db.Entry(ingresoProductoDetalle).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensajeError = ex.Message });
            }
        }
                
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            ICollection<IngresoProductoDetalle> ingresoProductoDetalleBD = db.IngresoProductoDetalle.Where(x => x.IngresoProductoId == id).ToList();

            var ingresoProductoDetalle = (from item in ingresoProductoDetalleBD
                                          join itemProducto in db.Producto.ToList()
                                          on item.ProductoId equals itemProducto.ProductoId
                                          select new IngresoProductoDetalleModels
                                          {
                                              IngresoProductoDetalleId = item.IngresoProductoDetalleId,
                                              IngresoProductoId = item.IngresoProductoId,
                                              ProductoId = item.ProductoId,
                                              NombreProducto = itemProducto.NombreProducto,
                                              CantidadPaquetes = Convert.ToDecimal(item.CantidadPaquetes),
                                              FechaVencimiento = Convert.ToDateTime(item.FechaVencimiento)
                                          }).ToList();
            
            IngresoProducto ingresoProductoBD = db.IngresoProducto.Find(id);
            IngresoProductoModels ingresoProducto = new IngresoProductoModels()
            {
                IngresoProductoId = ingresoProductoBD.IngresoProductoId,
                SucursalId = ingresoProductoBD.SucursalId,
                LaboratorioId = ingresoProductoBD.LaboratorioId,
                CatTipoIngreso = ingresoProductoBD.CatTipoIngreso,
                NroFactura = ingresoProductoBD.NroFactura,
                Observacion = ingresoProductoBD.Observacion,
                FechaCreacion = ingresoProductoBD.FechaCreacion,
                Detalle = ingresoProductoDetalle,
                lstSucursal = db.Sucursal.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreSucursal,
                    Value = x.SucursalId.ToString()
                }).ToList(),
                lstLaboratorio = db.Laboratorio.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreLaboratorio,
                    Value = x.LaboratorioId.ToString()
                }).ToList(),
                lstCatTipoIngreso = db.CatalogoDetalle.Where(x => x.CatalogoId == "TIPO_INGRESO").Select(x => new SelectListItem
                {
                    Text = x.Detalle,
                    Value = x.Valor
                }).ToList(),
            };

            if (ingresoProducto == null)
            {
                return HttpNotFound();
            }

            return PartialView(ingresoProducto);
        }

        // POST: /Laboratorio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(IngresoProductoModels ingresoProducto)
        {
            try
            {
                var detalle = from item in db.IngresoProductoDetalle
                              where item.IngresoProductoId == ingresoProducto.IngresoProductoId
                              select item;
                
                IngresoProducto ingresoProductoBD = db.IngresoProducto.Find(ingresoProducto.IngresoProductoId);

                foreach (var item in detalle)
                {
                    db.IngresoProductoDetalle.Remove(item);
                    db.SaveChanges();


                    var existeInventarioDB = (from itemInventario in db.Inventario.ToList()
                                              where itemInventario.ProductoId == item.ProductoId && itemInventario.SucursalId == ingresoProductoBD.SucursalId
                                              select itemInventario).FirstOrDefault();

                    existeInventarioDB.CantIngresoPaquete -= Convert.ToDecimal(item.CantidadPaquetes);
                    existeInventarioDB.CantIngresoUnidad -= Convert.ToDecimal(item.CantidadUnidades);
                    existeInventarioDB.CantidadPaqueteTotal -= Convert.ToDecimal(item.CantidadPaquetes);
                    existeInventarioDB.CantidadUnidadTotal -= Convert.ToDecimal(item.CantidadUnidades);
                    existeInventarioDB.UsuarioModificacion = Convert.ToString(Session["UserName"]);
                    existeInventarioDB.FechaModificacion = DateTime.Now;

                    db.Entry(existeInventarioDB).State = EntityState.Modified;
                    db.SaveChanges();
                }
                
                db.IngresoProducto.Remove(ingresoProductoBD);
                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensajeError = ex.Message });
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetProductos(int id)
        {
            List<ProductoModels> lstProducto = new List<ProductoModels>();

            lstProducto = (from item in db.Producto.ToList()
                           join itemLinea in db.Linea.ToList()
                           on item.LineaId equals itemLinea.LineaId
                           where itemLinea.LaboratorioId == id
                           select new ProductoModels
                           {
                               ProductoId = item.ProductoId,
                               NombreProducto = item.NombreProducto,
                           }).ToList();

            //SelectListItem itemList = new SelectListItem() { Text = "-- Seleccionar --", Value = "" };
            //lstProducto.Insert(0, itemList);

            return Json(lstProducto, JsonRequestBehavior.AllowGet);
            
            //var lstProducto = db.Producto.ToList().Where(x => x.LineaId == id).Select(x => new SelectListItem
            //{
            //    Text = x.NombreProducto,
            //    Value = x.ProductoId.ToString()
            //}).ToList();

            //SelectListItem item = new SelectListItem() { Text = "-- Seleccionar --", Value = "" };
            //lstProducto.Insert(0, item);

            //return Json(lstProducto, JsonRequestBehavior.AllowGet);
        }

	}
}