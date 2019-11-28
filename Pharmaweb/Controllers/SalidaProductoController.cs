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
    [Authorize(Roles = "SalidaProducto, Administrador")]
    public class SalidaProductoController : Controller
    {
        private PharmawebEntities db = new PharmawebEntities();

        // GET: /SalidaProducto/
        public ActionResult Index(string filter = null, int page = 1, int pageSize = 10, string sort = "NombreProducto", string sortdir = "DESC")
        {
            var records = new PagedList<SalidaProductoModels>();
            ViewBag.filter = filter;
            if (filter != null) filter = filter.ToUpper();

            records.Content = (from itemSalidaProducto in db.SalidaProducto.ToList()
                               join itemSucursal in db.Sucursal.ToList()
                               on itemSalidaProducto.SucursalId equals itemSucursal.SucursalId
                               join itemProducto in db.Producto.ToList()
                               on itemSalidaProducto.ProductoId equals itemProducto.ProductoId                               
                               join itemLinea in db.Linea.ToList()
                               on itemProducto.LineaId equals itemLinea.LineaId
                               join itemLaboratorio in db.Laboratorio.ToList()
                               on itemLinea.LaboratorioId equals itemLaboratorio.LaboratorioId                                
                               select new SalidaProductoModels
                               {
                                   SalidaProductoId = itemSalidaProducto.SalidaProductoId,
                                   NombreSucursal = itemSucursal.NombreSucursal,
                                   NombreLaboratorio = itemLaboratorio.NombreLaboratorio,
                                   NombreLinea = itemLinea.NombreLinea,
                                   ProductoId = itemProducto.ProductoId,
                                   NombreProducto = itemProducto.NombreProducto,
                                   CatTipoSalida = itemProducto.CatTipo,
                                   CantidadPaquetes = itemSalidaProducto.CantidadPaquetes,
                                   CantidadUnidades = itemSalidaProducto.CantidadUnidades
                               }).Where(x => filter == null || x.NombreSucursal.Contains(filter) || x.NombreLaboratorio.Contains(filter) || x.NombreLinea.Contains(filter) || x.NombreProducto.Contains(filter))
                                .OrderBy(sort + " " + sortdir)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            // Count
            records.TotalRecords = (from itemSalidaProducto in db.SalidaProducto.ToList()
                                    join itemSucursal in db.Sucursal.ToList()
                                    on itemSalidaProducto.SucursalId equals itemSucursal.SucursalId
                                    join itemProducto in db.Producto.ToList()
                                    on itemSalidaProducto.ProductoId equals itemProducto.ProductoId
                                    join itemLinea in db.Linea.ToList()
                                    on itemProducto.LineaId equals itemLinea.LineaId
                                    join itemLaboratorio in db.Laboratorio.ToList()
                                    on itemLinea.LaboratorioId equals itemLaboratorio.LaboratorioId
                                    select new SalidaProductoModels
                                    {
                                        SalidaProductoId = itemSalidaProducto.SalidaProductoId,
                                        NombreSucursal = itemSucursal.NombreSucursal,
                                        NombreLaboratorio = itemLaboratorio.NombreLaboratorio,
                                        NombreLinea = itemLinea.NombreLinea,
                                        ProductoId = itemProducto.ProductoId,
                                        NombreProducto = itemProducto.NombreProducto,
                                        CatTipoSalida = itemProducto.CatTipo,
                                        CantidadPaquetes = itemSalidaProducto.CantidadPaquetes,
                                        CantidadUnidades = itemSalidaProducto.CantidadUnidades
                                    }).Where(x => filter == null || x.NombreSucursal.Contains(filter) || x.NombreLaboratorio.Contains(filter) || x.NombreLinea.Contains(filter) || x.NombreProducto.Contains(filter)).Count();

            records.CurrentPage = page;
            records.PageSize = pageSize;

            return View(records);
        }

        // GET: /SalidaProducto/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SalidaProducto salidaProductoBD = db.SalidaProducto.Find(id);
            Producto producto = db.Producto.Find(salidaProductoBD.ProductoId);
            Linea linea = db.Linea.Find(producto.LineaId);

            SalidaProductoModels salidaProducto = new SalidaProductoModels()
            {
                SucursalId = salidaProductoBD.SucursalId,
                LaboratorioId = linea.LaboratorioId,
                LineaId = producto.LineaId,
                ProductoId = salidaProductoBD.ProductoId,
                CatTipoSalida = salidaProductoBD.CatTipoSalida,
                CantidadPaquetes = salidaProductoBD.CantidadPaquetes,
                CantidadUnidades = salidaProductoBD.CantidadUnidades,

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

                lstLinea = db.Linea.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreLinea,
                    Value = x.LineaId.ToString()
                }).ToList(),

                lstProducto = db.Producto.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreProducto,
                    Value = x.ProductoId.ToString()
                }).ToList(),

                lstCatTipoSalida = db.CatalogoDetalle.Where(x => x.CatalogoId == "TIPO_SALIDA").Select(x => new SelectListItem
                {
                    Text = x.Detalle,
                    Value = x.Valor
                }).ToList()
            };

            if (salidaProducto == null)
            {
                return HttpNotFound();
            }

            return PartialView(salidaProducto);
        }

        // GET: /SalidaProducto/Create
        public ActionResult Create()
        {
            SalidaProductoModels precioProducto = new SalidaProductoModels()
            {
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

                lstLinea = db.Linea.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreLinea,
                    Value = x.LineaId.ToString()
                }).ToList(),

                lstProducto = db.Producto.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreProducto,
                    Value = x.ProductoId.ToString()
                }).ToList(),

                lstCatTipoSalida = db.CatalogoDetalle.Where(x => x.CatalogoId == "TIPO_SALIDA").Select(x => new SelectListItem
                {
                    Text = x.Detalle,
                    Value = x.Valor
                }).ToList()
            };

            return PartialView(precioProducto);
        }

        // POST: /SalidaProducto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SalidaProductoModels salidaProducto)
        {
            //if (ModelState.IsValid)
            //{
                try
                {
                    var inventario = db.Inventario.Find(salidaProducto.SucursalId, salidaProducto.ProductoId);
                    
                    inventario.CantSalidaPaquete += salidaProducto.CantidadPaquetes;
                    inventario.CantSalidaUnidad += salidaProducto.CantidadUnidades;

                    inventario.CantidadPaqueteTotal -= salidaProducto.CantidadPaquetes;
                    inventario.CantidadUnidadTotal -= salidaProducto.CantidadUnidades;

                    db.Entry(inventario).State = EntityState.Modified;
                    db.SaveChanges();

                    SalidaProducto salidaProductoDB = new SalidaProducto()
                    {
                        SucursalId = salidaProducto.SucursalId,
                        ProductoId = salidaProducto.ProductoId,
                        CatTipoSalida = salidaProducto.CatTipoSalida,
                        CantidadPaquetes = salidaProducto.CantidadPaquetes,
                        CantidadUnidades = salidaProducto.CantidadUnidades,
                        UsuarioModificacion = Convert.ToString(Session["UserName"]),
                        FechaModificacion = DateTime.Now,
                        UsuarioCreacion = Convert.ToString(Session["UserName"]),
                        FechaCreacion = DateTime.Now
                    };

                    db.SalidaProducto.Add(salidaProductoDB);
                    db.SaveChanges();

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, mensajeError = ex.Message });
                }
            //}
            //else
            //{
            //    string mensajeError = "Error de validación: " + string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            //    return Json(new { success = false, mensajeError }, JsonRequestBehavior.DenyGet);
            //}
        }

        // GET: /SalidaProducto/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SalidaProducto salidaProductoBD = db.SalidaProducto.Find(id);
            Producto producto = db.Producto.Find(salidaProductoBD.ProductoId);
            Linea linea = db.Linea.Find(producto.LineaId);

            SalidaProductoModels salidaProducto = new SalidaProductoModels()
            {
                SalidaProductoId = salidaProductoBD.SalidaProductoId,
                SucursalId = salidaProductoBD.SucursalId,
                LaboratorioId = linea.LaboratorioId,
                LineaId = producto.LineaId,
                ProductoId = salidaProductoBD.ProductoId,
                CatTipoSalida = salidaProductoBD.CatTipoSalida,
                CantidadPaquetes = salidaProductoBD.CantidadPaquetes,
                CantidadUnidades = salidaProductoBD.CantidadUnidades,
                UsuarioModificacion = salidaProductoBD.UsuarioModificacion,
                FechaModificacion = salidaProductoBD.FechaModificacion,
                UsuarioCreacion = salidaProductoBD.UsuarioCreacion,
                FechaCreacion = salidaProductoBD.FechaCreacion,

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

                lstLinea = db.Linea.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreLinea,
                    Value = x.LineaId.ToString()
                }).ToList(),

                lstProducto = db.Producto.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreProducto,
                    Value = x.ProductoId.ToString()
                }).ToList(),

                lstCatTipoSalida = db.CatalogoDetalle.Where(x => x.CatalogoId == "TIPO_SALIDA").Select(x => new SelectListItem
                {
                    Text = x.Detalle,
                    Value = x.Valor
                }).ToList()
            };

            if (salidaProducto == null)
            {
                return HttpNotFound();
            }

            return PartialView(salidaProducto);
        }

        // POST: /SalidaProducto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SalidaProductoModels salidaProducto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    SalidaProducto salidaProductoDB = new SalidaProducto()
                    {
                        SalidaProductoId = salidaProducto.SalidaProductoId,
                        SucursalId = salidaProducto.SucursalId,
                        ProductoId = salidaProducto.ProductoId,
                        CatTipoSalida = salidaProducto.CatTipoSalida,
                        CantidadPaquetes = salidaProducto.CantidadPaquetes,
                        CantidadUnidades = salidaProducto.CantidadUnidades,
                        UsuarioModificacion = Convert.ToString(Session["UserName"]),
                        FechaModificacion = DateTime.Now,
                        UsuarioCreacion = salidaProducto.UsuarioCreacion,
                        FechaCreacion = salidaProducto.FechaCreacion
                    };

                    db.Entry(salidaProductoDB).State = EntityState.Modified;
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

        // GET: /SalidaProducto/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SalidaProducto salidaProductoBD = db.SalidaProducto.Find(id);
            Producto producto = db.Producto.Find(salidaProductoBD.ProductoId);
            Linea linea = db.Linea.Find(producto.LineaId);

            SalidaProductoModels salidaProducto = new SalidaProductoModels()
            {
                SalidaProductoId = salidaProductoBD.SalidaProductoId,
                SucursalId = salidaProductoBD.SucursalId,
                LaboratorioId = linea.LaboratorioId,
                LineaId = producto.LineaId,
                ProductoId = salidaProductoBD.ProductoId,
                CatTipoSalida = salidaProductoBD.CatTipoSalida,
                CantidadPaquetes = salidaProductoBD.CantidadPaquetes,
                CantidadUnidades = salidaProductoBD.CantidadUnidades,

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

                lstLinea = db.Linea.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreLinea,
                    Value = x.LineaId.ToString()
                }).ToList(),

                lstProducto = db.Producto.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreProducto,
                    Value = x.ProductoId.ToString()
                }).ToList(),

                lstCatTipoSalida = db.CatalogoDetalle.Where(x => x.CatalogoId == "TIPO_SALIDA").Select(x => new SelectListItem
                {
                    Text = x.Detalle,
                    Value = x.Valor
                }).ToList()
            };

            if (salidaProducto == null)
            {
                return HttpNotFound();
            }

            return PartialView(salidaProducto);
        }

        // POST: /SalidaProducto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(SalidaProductoModels salidaProducto)
        {
            try
            {
                var inventario = db.Inventario.Find(salidaProducto.SucursalId, salidaProducto.ProductoId);

                inventario.CantSalidaPaquete -= salidaProducto.CantidadPaquetes;
                inventario.CantSalidaUnidad -= salidaProducto.CantidadUnidades;

                inventario.CantidadPaqueteTotal += salidaProducto.CantidadPaquetes;
                inventario.CantidadUnidadTotal += salidaProducto.CantidadUnidades;

                db.Entry(inventario).State = EntityState.Modified;
                db.SaveChanges();
                
                SalidaProducto salidaProductoDB = db.SalidaProducto.Find(salidaProducto.SalidaProductoId);
                db.SalidaProducto.Remove(salidaProductoDB);
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
