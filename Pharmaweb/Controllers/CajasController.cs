using Data;
using Pharmaweb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using System.Data.Entity;
using System.Net;
using BussinesSVC.Manager;
using BussinesSVC.Domain;
using BussinesSVC.Data;
using BussinesSVC.Manger;

namespace Pharmaweb.Controllers
{

    [Authorize(Roles = "Cajas, Administrador")]
    public class CajasController : Controller
    {
        private PharmawebEntities db = new PharmawebEntities();
        //
        // GET: /Cajas/
        public ActionResult Index(string filter = null, int page = 1, int pageSize = 20, string sort = "FechaCreacion", string sortdir = "DESC")
        {
            var records = new PagedList<VentasModels>();
            ViewBag.filter = filter;
            if (filter != null) filter = filter.ToUpper();

            records.Content = (from itemVentas in db.Ventas.ToList()
                               where itemVentas.CatEstadoVenta == "CAJAS"
                               || itemVentas.CatEstadoVenta == "VENDIDO"
                               || itemVentas.CatEstadoVenta == "REINGRESADO"
                               || itemVentas.CatEstadoVenta == "ANULADO"
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
                                   UsuarioModificacion = itemVentas.UsuarioModificacion,
                                   FechaModificacion = itemVentas.FechaModificacion,
                                   UsuarioCreacion = itemVentas.UsuarioCreacion,
                                   FechaCreacion = itemVentas.FechaCreacion
                               }).Where(x => filter == null || x.CatEstadoVenta.Contains(filter) || x.NombreCliente.Contains(filter))
                                .OrderBy(sort + " " + sortdir)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            // Count
            records.TotalRecords = (from itemVentas in db.Ventas.ToList()
                                     where itemVentas.CatEstadoVenta == "CAJAS"
                                     || itemVentas.CatEstadoVenta == "VENDIDO"
                                     || itemVentas.CatEstadoVenta == "REINGRESADO"
                                     || itemVentas.CatEstadoVenta == "ANULADO"
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
                                         UsuarioModificacion = itemVentas.UsuarioModificacion,
                                         FechaModificacion = itemVentas.FechaModificacion,
                                         UsuarioCreacion = itemVentas.UsuarioCreacion,
                                         FechaCreacion = itemVentas.FechaCreacion
                                     }).Where(x => filter == null || x.CatEstadoVenta.Contains(filter) || x.NombreCliente.Contains(filter))
                                    .Count();

            records.CurrentPage = page;
            records.PageSize = pageSize;

            return View(records);
        }

        // GET: /Cajas/Facturar/5
        public ActionResult Facturar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ventas ventasDB = db.Ventas.Find(id);

            VentasModels ventas = new VentasModels()
            {
                VentasId = ventasDB.VentasId,
                SucursalId = ventasDB.SucursalId,
                Factura = ventasDB.Factura,
                Nit = ventasDB.Nit,
                NombreCliente = ventasDB.NombreCliente,
                TotalVenta = ventasDB.TotalVenta,
                TotalPagado = ventasDB.TotalPagado,
                Cambio = ventasDB.Cambio,
                CatEstadoVenta = ventasDB.CatEstadoVenta,
                UsuarioModificacion = ventasDB.UsuarioModificacion,
                FechaModificacion = ventasDB.FechaModificacion,
                UsuarioCreacion = ventasDB.UsuarioCreacion,
                FechaCreacion = ventasDB.FechaCreacion
            };

            if (ventas == null)
            {
                return HttpNotFound();
            }

            return PartialView(ventas);
        }

        // POST: /Cajas/Facturar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Facturar(VentasModels ventas)
        {
            try
            {
                decimal totalVenta = 0;

                totalVenta = (from itemVentasDetalle in db.VentasDetalle.ToList()
                              where itemVentasDetalle.VentasId == ventas.VentasId
                              select itemVentasDetalle.Subtotal).Sum();

                Ventas ventasDB = new Ventas()
                {
                    VentasId = ventas.VentasId,
                    SucursalId = ventas.SucursalId,
                    Factura = ventas.Factura,
                    Nit = ventas.Nit,
                    NombreCliente = ventas.NombreCliente,
                    TotalVenta = totalVenta,
                    TotalPagado = ventas.TotalPagado,
                    Cambio = ventas.Cambio,
                    CatEstadoVenta = "VENDIDO",
                    UsuarioModificacion = Convert.ToString(Session["UserName"]),
                    FechaModificacion = DateTime.Now,
                    UsuarioCreacion = ventas.UsuarioCreacion,
                    FechaCreacion = ventas.FechaCreacion
                };

                db.Entry(ventasDB).State = EntityState.Modified;
                db.SaveChanges();

                Session["VentasId"] = ventas.VentasId;
                string res = "reenviar";

                return Json(new { success = true, res = res }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensajeError = ex.Message });
            }
        }

        public ActionResult Impresion()
        {
            try
            {
                int ventasId = Convert.ToInt32(Session["VentasId"]);

                if (ventasId == 0)
                {
                    //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Ventas ventasDB = db.Ventas.Find(ventasId);

                List<VentasDetalle> lstVentasDetalle = new List<VentasDetalle>();

                lstVentasDetalle = (from itemVentasDetalle in db.VentasDetalle.ToList()
                                    where itemVentasDetalle.VentasId == ventasId
                                    select itemVentasDetalle).ToList();
               
                /* GeneraFactura */
                FacturaIn facturaIn = new FacturaIn();
                facturaIn.FechaFactura = DateTime.Now;
                facturaIn.IdDosificacion = 30;
                facturaIn.MontoFactura = 50;
                facturaIn.NitCliente = ventasDB.Nit;
                facturaIn.NombreCliente = ventasDB.NombreCliente;

                List<ConceptoFactura> lstConceptos = new List<ConceptoFactura>();

                lstConceptos = (from itemVentasDetalle in lstVentasDetalle
                                join itemProducto in db.Producto.ToList()
                                on itemVentasDetalle.ProductoId equals itemProducto.ProductoId
                                join itemPrecio in db.PrecioProducto.ToList()
                                on itemVentasDetalle.ProductoId equals itemPrecio.ProductoId
                                select new ConceptoFactura
                                {
                                    Cantidad = Convert.ToInt32(itemVentasDetalle.CantidadPaquete * itemProducto.UnidadesPorPaquete + itemVentasDetalle.CantidadUnidad),
                                    Concepto = itemProducto.NombreProducto,
                                    PrecioUnidad = itemPrecio.PrecioVentaUnidad,
                                    Total = itemVentasDetalle.Subtotal
                                }).ToList();

                

                //ConceptoFactura concepto = new ConceptoFactura();

                //concepto.Cantidad = 10;
                //concepto.Concepto = "algo";
                //concepto.PrecioUnidad = 5;

                //lstConceptos.Add(concepto);

                facturaIn.Conceptos = lstConceptos;

                //facturaIn.Conceptos[0] = lstConceptos.First();

                var resul = LoginAux("rodrigo.alegre", "123");

                FacturaManager mgnrFactura = new FacturaManager();
                
                facturaIn.Conceptos.ForEach(x => x.Talla = x.Concepto);
                //facturaIn.IdDosificacion = Convert.ToInt64(Request["IdDosificacionHidden"]);

                var resul2 = mgnrFactura.GuardarFactura(facturaIn, (spLogin_Result)resul.Object);
                ViewData["Impresion"] = true;

                Session["Factura"] = facturaIn;/// cambiar es de prueba 

                return View(resul2.Object);
                //return Json(resul2.Object, JsonRequestBehavior.AllowGet);

                /* End GeneraFactura */                
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensajeError = ex.Message });
            }
        }
        private ResponseObject LoginAux(string usuario, string password)
        {
            LoginManager lgnMgr = new LoginManager();
            return (ResponseObject)lgnMgr.Login(usuario, password);
        }

        // GET: /Cajas/Reingresar/5
        public ActionResult Reingresar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ventas ventasDB = db.Ventas.Find(id);

            VentasModels ventas = new VentasModels()
            {
                VentasId = ventasDB.VentasId,
                SucursalId = ventasDB.SucursalId,
                Factura = ventasDB.Factura,
                Nit = ventasDB.Nit,
                NombreCliente = ventasDB.NombreCliente,
                TotalVenta = ventasDB.TotalVenta,
                TotalPagado = ventasDB.TotalPagado,
                Cambio = ventasDB.Cambio,
                CatEstadoVenta = ventasDB.CatEstadoVenta,
                UsuarioModificacion = ventasDB.UsuarioModificacion,
                FechaModificacion = ventasDB.FechaModificacion,
                UsuarioCreacion = ventasDB.UsuarioCreacion,
                FechaCreacion = ventasDB.FechaCreacion
            };

            if (ventas == null)
            {
                return HttpNotFound();
            }

            return PartialView(ventas);
        }

        // POST: /Cajas/Reingresar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reingresar(VentasModels ventas)
        {
            try
            {
                var ventasDetalle = from itemVentasDetalle in db.VentasDetalle.ToList()
                                    where itemVentasDetalle.VentasId == ventas.VentasId
                                    select itemVentasDetalle;

                foreach (var item in ventasDetalle)
                {
                    var inventario = db.Inventario.Find(ventas.SucursalId, item.ProductoId);

                    inventario.CantIngresoPaquete += item.CantidadPaquete;
                    inventario.CantIngresoUnidad += item.CantidadUnidad;

                    inventario.CantidadPaqueteTotal += item.CantidadPaquete;
                    inventario.CantidadUnidadTotal += item.CantidadUnidad;

                    db.Entry(inventario).State = EntityState.Modified;
                    db.SaveChanges();
                }

                Ventas ventasDB = new Ventas()
                {
                    VentasId = ventas.VentasId,
                    SucursalId = ventas.SucursalId,
                    Factura = 0,
                    Nit = ventas.Nit,
                    NombreCliente = ventas.NombreCliente,
                    TotalVenta = ventas.TotalVenta,
                    TotalPagado = ventas.TotalPagado,
                    Cambio = ventas.Cambio,
                    CatEstadoVenta = "REINGRESADO",
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

        // GET: /Cajas/Reingresar/5
        public ActionResult Anular(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ventas ventasDB = db.Ventas.Find(id);

            VentasModels ventas = new VentasModels()
            {
                VentasId = ventasDB.VentasId,
                SucursalId = ventasDB.SucursalId,
                Factura = ventasDB.Factura,
                Nit = ventasDB.Nit,
                NombreCliente = ventasDB.NombreCliente,
                TotalVenta = ventasDB.TotalVenta,
                TotalPagado = ventasDB.TotalPagado,
                Cambio = ventasDB.Cambio,
                CatEstadoVenta = ventasDB.CatEstadoVenta,
                UsuarioModificacion = ventasDB.UsuarioModificacion,
                FechaModificacion = ventasDB.FechaModificacion,
                UsuarioCreacion = ventasDB.UsuarioCreacion,
                FechaCreacion = ventasDB.FechaCreacion
            };

            if (ventas == null)
            {
                return HttpNotFound();
            }

            return PartialView(ventas);
        }

        // POST: /Cajas/Reingresar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Anular(VentasModels ventas)
        {
            try
            {
                var ventasDetalle = from itemVentasDetalle in db.VentasDetalle.ToList()
                                    where itemVentasDetalle.VentasId == ventas.VentasId
                                    select itemVentasDetalle;

                foreach (var item in ventasDetalle)
                {
                    var inventario = db.Inventario.Find(ventas.SucursalId, item.ProductoId);

                    inventario.CantIngresoPaquete += item.CantidadPaquete;
                    inventario.CantIngresoUnidad += item.CantidadUnidad;

                    inventario.CantidadPaqueteTotal += item.CantidadPaquete;
                    inventario.CantidadUnidadTotal += item.CantidadUnidad;

                    db.Entry(inventario).State = EntityState.Modified;
                    db.SaveChanges();
                }

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
                    CatEstadoVenta = "ANULADO",
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
	}
}