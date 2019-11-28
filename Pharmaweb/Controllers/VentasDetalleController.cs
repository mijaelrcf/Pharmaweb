using Data;
using Pharmaweb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Pharmaweb.Controllers
{
    [Authorize(Roles = "VentasDetalle, Administrador")]
    public class VentasDetalleController : Controller
    {
        private PharmawebEntities db = new PharmawebEntities();
        //
        // GET: /VentasDetalle/
        public ActionResult Index()
        {
            List<VentasDetalleModels> lstVentasDetalle = (from itemVentasDetalle in db.VentasDetalle.ToList()                                                          
                                                          join itemProducto in db.Producto.ToList()
                                                          on itemVentasDetalle.ProductoId equals itemProducto.ProductoId
                                                          join itemVentas in db.Ventas.ToList()
                                                          on itemVentasDetalle.VentasId equals itemVentas.VentasId
                                                          where itemVentas.CatEstadoVenta == "PENDIENTE"
                                                          && itemVentas.UsuarioCreacion == Convert.ToString(Session["UserName"])
                                                          select new VentasDetalleModels
                                                          {
                                                              VentasId = itemVentasDetalle.VentasId,
                                                              ProductoId = itemVentasDetalle.ProductoId,
                                                              NombreProducto = itemProducto.NombreProducto,
                                                              CantidadPaquete = itemVentasDetalle.CantidadPaquete,
                                                              CantidadUnidad = itemVentasDetalle.CantidadUnidad,
                                                              Subtotal = itemVentasDetalle.Subtotal
                                                          }).ToList();
            return View(lstVentasDetalle);
        }

        // GET: /VentasDetalle/Details/5
        public ActionResult Details(int? ventasId, int? productoId)
        {
            if (ventasId == null || productoId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            VentasDetalleModels ventasDetalle = (from itemVentasDetalle in db.VentasDetalle.ToList()
                                                 join itemProducto in db.Producto.ToList()
                                                 on itemVentasDetalle.ProductoId equals itemProducto.ProductoId
                                                 join itemPrecio in db.PrecioProducto.ToList()
                                                 on itemProducto.ProductoId equals itemPrecio.ProductoId
                                                 where itemPrecio.SucursalId == Convert.ToInt32(Session["Sucursal"])
                                                 && itemVentasDetalle.ProductoId == productoId
                                                 && itemVentasDetalle.VentasId == ventasId
                                                 select new VentasDetalleModels
                                                 {
                                                     VentasId = itemVentasDetalle.VentasId,
                                                     ProductoId = itemVentasDetalle.ProductoId,
                                                     NombreProducto = itemProducto.NombreProducto,
                                                     PrecioVentaPaquete = itemPrecio.PrecioVentaPaquete,
                                                     PrecioVentaUnidad = itemPrecio.PrecioVentaUnidad,
                                                     CantidadPaquete = itemVentasDetalle.CantidadPaquete,
                                                     CantidadUnidad = itemVentasDetalle.CantidadUnidad,
                                                     Subtotal = itemVentasDetalle.Subtotal,
                                                     TipoVenta = itemProducto.CatTipo
                                                 }).FirstOrDefault();

            if (ventasDetalle == null)
            {
                return HttpNotFound();
            }

            return PartialView(ventasDetalle);
        }
        
        // GET: /VentasDetalle/Edit/5
        public ActionResult Edit(int? ventasId, int? productoId)
        {
            if (ventasId == null || productoId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            VentasDetalleModels ventasDetalle = (from itemVentasDetalle in db.VentasDetalle.ToList()
                                                 join itemProducto in db.Producto.ToList()
                                                 on itemVentasDetalle.ProductoId equals itemProducto.ProductoId
                                                 join itemPrecio in db.PrecioProducto.ToList()
                                                 on itemProducto.ProductoId equals itemPrecio.ProductoId
                                                 where itemPrecio.SucursalId == Convert.ToInt32(Session["Sucursal"])
                                                 && itemVentasDetalle.ProductoId == productoId
                                                 && itemVentasDetalle.VentasId == ventasId
                                                 select new VentasDetalleModels
                                                 {
                                                     VentasId = itemVentasDetalle.VentasId,
                                                     ProductoId = itemVentasDetalle.ProductoId,
                                                     NombreProducto = itemProducto.NombreProducto,
                                                     PrecioVentaPaquete = itemPrecio.PrecioVentaPaquete,
                                                     PrecioVentaUnidad = itemPrecio.PrecioVentaUnidad,
                                                     CantidadPaquete = itemVentasDetalle.CantidadPaquete,
                                                     CantidadUnidad = itemVentasDetalle.CantidadUnidad,
                                                     Subtotal = itemVentasDetalle.Subtotal,
                                                     TipoVenta = itemProducto.CatTipo
                                                 }).FirstOrDefault();

            if (ventasDetalle == null)
            {
                return HttpNotFound();
            }

            return PartialView(ventasDetalle);
        }

        // POST: /VentasDetalle/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VentasDetalleModels ventasDetalle)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    VentasDetalle ventasDetalleDB = new VentasDetalle()
                    {
                        VentasId = ventasDetalle.VentasId,
                        ProductoId = ventasDetalle.ProductoId,
                        CantidadPaquete = ventasDetalle.CantidadPaquete,
                        CantidadUnidad = ventasDetalle.CantidadUnidad,
                        Subtotal = ventasDetalle.Subtotal
                    };

                    db.Entry(ventasDetalleDB).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, mensajeError = ex.Message });
                }
            }

            return PartialView("Edit", ventasDetalle);
        }

        // GET: /VentasDetalle/Delete/5
        public ActionResult Delete(int? ventasId, int? productoId)
        {
            if (ventasId == null || productoId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
                        
            VentasDetalleModels ventasDetalle = (from itemVentasDetalle in db.VentasDetalle.ToList()
                                                 join itemProducto in db.Producto.ToList()
                                                 on itemVentasDetalle.ProductoId equals itemProducto.ProductoId
                                                 join itemPrecio in db.PrecioProducto.ToList()
                                                 on itemProducto.ProductoId equals itemPrecio.ProductoId
                                                 where itemPrecio.SucursalId == Convert.ToInt32(Session["Sucursal"])
                                                 && itemVentasDetalle.ProductoId == productoId
                                                 && itemVentasDetalle.VentasId == ventasId
                                                 select new VentasDetalleModels
                                                 {
                                                     VentasId = itemVentasDetalle.VentasId,
                                                     ProductoId = itemVentasDetalle.ProductoId,
                                                     NombreProducto = itemProducto.NombreProducto,
                                                     PrecioVentaPaquete = itemPrecio.PrecioVentaPaquete,
                                                     PrecioVentaUnidad = itemPrecio.PrecioVentaUnidad,
                                                     CantidadPaquete = itemVentasDetalle.CantidadPaquete,
                                                     CantidadUnidad = itemVentasDetalle.CantidadUnidad,
                                                     Subtotal = itemVentasDetalle.Subtotal,
                                                     TipoVenta = itemProducto.CatTipo
                                                 }).FirstOrDefault();

            if (ventasDetalle == null)
            {
                return HttpNotFound();
            }

            return PartialView(ventasDetalle);
        }

        // POST: /Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(VentasDetalleModels ventasDetalle)
        {
            try
            {
                VentasDetalle ventasDetalleBD = db.VentasDetalle.Find(ventasDetalle.VentasId, ventasDetalle.ProductoId);
                db.VentasDetalle.Remove(ventasDetalleBD);
                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensajeError = ex.Message });
            }
        }

        // GET: /VentasDetalle/GuardarVenta/5
        public ActionResult GuardarVenta()
        {
            VentasModels ventas = (from itemVentas in db.Ventas.ToList()
                                   where itemVentas.CatEstadoVenta == "PENDIENTE"
                                   && itemVentas.UsuarioCreacion == Convert.ToString(Session["UserName"])
                                   && itemVentas.SucursalId == Convert.ToInt32(Session["Sucursal"])
                                   select new VentasModels
                                   {
                                       VentasId = itemVentas.VentasId,
                                       SucursalId = itemVentas.SucursalId,
                                       Factura = itemVentas.Factura,
                                       Nit = itemVentas.Nit,
                                       NombreCliente = itemVentas.NombreCliente,
                                       TotalVenta = itemVentas.TotalVenta,
                                       TotalPagado = itemVentas.TotalPagado, 
                                       Cambio = itemVentas.Cambio,
                                       CatEstadoVenta = itemVentas.CatEstadoVenta,
                                       UsuarioModificacion= itemVentas.UsuarioCreacion,
                                       FechaModificacion = itemVentas.FechaCreacion,
                                       UsuarioCreacion = itemVentas.UsuarioCreacion,
                                       FechaCreacion = itemVentas.FechaCreacion
                                   }).FirstOrDefault();

            ventas.Factura = (from itemVentas in db.Ventas.ToList()
                              select itemVentas.Factura).Max();
            
            return PartialView(ventas);
        }

        // POST: /VentasDetalle/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarVenta(VentasModels ventas)
        {
            try
            {
                decimal totalVenta = 0;
                
                var ventasDetalle = from itemVentasDetalle in db.VentasDetalle.ToList()
                                    where itemVentasDetalle.VentasId == ventas.VentasId
                                    select itemVentasDetalle;

                totalVenta = ventasDetalle.Sum(x => x.Subtotal);

                foreach (var item in ventasDetalle) 
                {
                    var producto = db.Producto.Find(item.ProductoId);
                    var inventario = db.Inventario.Find(ventas.SucursalId, item.ProductoId);

                    decimal totalUnidades = item.CantidadPaquete * producto.UnidadesPorPaquete + item.CantidadUnidad;
                    decimal stockUnidades = inventario.CantidadPaqueteTotal * producto.UnidadesPorPaquete + inventario.CantidadUnidadTotal;

                    int totalVendido = Convert.ToInt32(stockUnidades - totalUnidades);

                    int paquetes = totalVendido / producto.UnidadesPorPaquete;
                    int unidades = totalVendido % producto.UnidadesPorPaquete;

                    inventario.CantSalidaPaquete += item.CantidadPaquete;
                    inventario.CantSalidaUnidad += item.CantidadUnidad;

                    inventario.CantidadPaqueteTotal = paquetes;
                    inventario.CantidadUnidadTotal = unidades;

                    db.Entry(inventario).State = EntityState.Modified;
                    db.SaveChanges();
                }
               
                Ventas ventasDB = new Ventas()
                {
                    VentasId = ventas.VentasId,
                    SucursalId = ventas.SucursalId,
                    Factura = ventas.Factura + 1,
                    Nit = ventas.Nit,
                    NombreCliente = ventas.NombreCliente,
                    TotalVenta = totalVenta,
                    TotalPagado = ventas.TotalPagado,
                    Cambio = ventas.Cambio,
                    CatEstadoVenta = "CAJAS", // Se cambia el estado para enviarlo a la bandeja de CAJAS, para que este sea cobrado y facturado.
                    UsuarioModificacion = Convert.ToString(Session["UserName"]),
                    FechaModificacion = DateTime.Now,
                    UsuarioCreacion = ventas.UsuarioCreacion,
                    FechaCreacion = ventas.FechaCreacion
                };

                db.Entry(ventasDB).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensajeError = ex.Message });
            }
        }

        // GET: /VentasDetalle/GuardarVenta/5
        public ActionResult CancelarVenta()
        {
            VentasModels ventas = (from itemVentas in db.Ventas. ToList()
                                  where itemVentas.CatEstadoVenta == "PENDIENTE"
                                  && itemVentas.UsuarioCreacion == Convert.ToString(Session["UserName"])
                                  select new VentasModels 
                                  {
                                      VentasId = itemVentas.VentasId,
                                      SucursalId = itemVentas.SucursalId,
                                      Factura = itemVentas.Factura,
                                      Nit = itemVentas.Nit,
                                      NombreCliente = itemVentas.NombreCliente,
                                      TotalVenta = itemVentas.TotalVenta,
                                      TotalPagado = itemVentas.TotalPagado,
                                      Cambio = itemVentas.Cambio,
                                      CatEstadoVenta = itemVentas.CatEstadoVenta,
                                      UsuarioModificacion = itemVentas.UsuarioModificacion,
                                      FechaModificacion = itemVentas.FechaModificacion,
                                      UsuarioCreacion = itemVentas.UsuarioCreacion,
                                      FechaCreacion = itemVentas.FechaCreacion
                                  }).FirstOrDefault();

            return PartialView(ventas);
        }

        // POST: /VentasDetalle/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelarVenta(VentasModels ventas)
        {
            try
            {
                Ventas ventasDB = new Ventas()
                {
                    VentasId = ventas.VentasId,
                    SucursalId = ventas.SucursalId,
                    Factura = ventas.Factura,
                    Nit = ventas.Nit,
                    NombreCliente = ventas.NombreCliente,
                    TotalVenta = ventas.TotalVenta,
                    TotalPagado = ventas.TotalPagado,
                    Cambio = ventas.Cambio,
                    CatEstadoVenta = "CANCELADO", // Se cambia el estado a CANCELADO para cancelar la Venta.
                    UsuarioModificacion = Convert.ToString(Session["UserName"]),
                    FechaModificacion = DateTime.Now,
                    UsuarioCreacion = ventas.UsuarioCreacion,
                    FechaCreacion = ventas.FechaCreacion
                };

                db.Entry(ventasDB).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensajeError = ex.Message });
            }
        }

        public JsonResult GetNit(string term)
        {
            var algo = db.Ventas.ToList().Select(x => new { Nit = Convert.ToString(x.Nit), NombreCliente = x.NombreCliente }).ToList();

            var results = algo.Where(s => term == null || s.Nit.Contains(term)).Select(x => new { value = x.Nit, id = x.NombreCliente }).Take(5).ToList();

            return Json(results, JsonRequestBehavior.AllowGet);
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