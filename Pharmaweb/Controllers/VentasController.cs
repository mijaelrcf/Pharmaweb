using Data;
using Pharmaweb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using System.Net;

namespace Pharmaweb.Controllers
{
    [Authorize(Roles = "Ventas, Administrador")]
    public class VentasController : Controller
    {
        private PharmawebEntities db = new PharmawebEntities();
        
        //
        // GET: /Ventas/
        public ActionResult Index(string filter = null, int page = 1, int pageSize = 8, string sort = "NombreProducto", string sortdir = "ASC")
        {
            var records = new PagedList<VentasModels>();
            ViewBag.filter = filter;
            if (filter != null) filter = filter.ToUpper();

            var IngresoProductoConVencimiento = from itemIngresoProductoDetalle in db.IngresoProductoDetalle.ToList()
                                                group itemIngresoProductoDetalle by itemIngresoProductoDetalle.ProductoId into grupo
                                                select new
                                                {
                                                    grupo.Key,
                                                    FechaVencimientoMinima = grupo.Min(p => p.FechaVencimiento)
                                                };
            
            records.Content = (from itemLaboratorio in db.Laboratorio.ToList()
                               join itemLinea in db.Linea.ToList()
                               on itemLaboratorio.LaboratorioId equals itemLinea.LaboratorioId
                               join itemProducto in db.Producto.ToList()
                               on itemLinea.LineaId equals itemProducto.LineaId
                               join itemPrecio in db.PrecioProducto.ToList()
                               on itemProducto.ProductoId equals itemPrecio.ProductoId
                               join itemProdVencimiento in IngresoProductoConVencimiento
                               on itemProducto.ProductoId equals itemProdVencimiento.Key
                               join itemInventario in db.Inventario.ToList()                               
                               on itemProducto.ProductoId equals itemInventario.ProductoId into joined
                               from itemLeftJoin in joined.DefaultIfEmpty()
                               where itemPrecio.SucursalId == Convert.ToInt32(Session["Sucursal"])
                               && itemLeftJoin.SucursalId == Convert.ToInt32(Session["Sucursal"])
                               select new VentasModels
                               {
                                   NombreLaboratorio = itemLaboratorio.NombreLaboratorio,
                                   NombreLinea = itemLinea.NombreLinea,
                                   ProductoId = itemProducto.ProductoId,
                                   NombreProducto = itemProducto.NombreProducto,
                                   PrecioVentaPaquete = itemPrecio.PrecioVentaPaquete,
                                   PrecioVentaUnidad = itemPrecio.PrecioVentaUnidad, 
                                   CantidadPaquete = itemLeftJoin == null ? 0 : itemLeftJoin.CantidadPaqueteTotal,
                                   CantidadUnidad = itemLeftJoin == null ? 0 : itemLeftJoin.CantidadUnidadTotal,
                                   FechaVencimiento = itemProdVencimiento.FechaVencimientoMinima,
                                   VencimientoEnDias = CalculaVencimientoEnDias(itemProdVencimiento.FechaVencimientoMinima)
                               }).Where(x => filter == null || x.NombreLaboratorio.Contains(filter) || x.NombreLinea.Contains(filter) || x.NombreProducto.Contains(filter))
                                .OrderBy(sort + " " + sortdir)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            // Count
            records.TotalRecords = (from itemLaboratorio in db.Laboratorio.ToList()
                                    join itemLinea in db.Linea.ToList()
                                    on itemLaboratorio.LaboratorioId equals itemLinea.LaboratorioId
                                    join itemProducto in db.Producto.ToList()
                                    on itemLinea.LineaId equals itemProducto.LineaId
                                    join itemPrecio in db.PrecioProducto.ToList()
                                    on itemProducto.ProductoId equals itemPrecio.ProductoId
                                    join itemProdVencimiento in IngresoProductoConVencimiento
                                    on itemProducto.ProductoId equals itemProdVencimiento.Key
                                    join itemInventario in db.Inventario.ToList()
                                    on itemProducto.ProductoId equals itemInventario.ProductoId into joined
                                    from itemLeftJoin in joined.DefaultIfEmpty()
                                    where itemPrecio.SucursalId == Convert.ToInt32(Session["Sucursal"])
                                    && itemLeftJoin.SucursalId == Convert.ToInt32(Session["Sucursal"])
                                    select new VentasModels
                                    {
                                        NombreLaboratorio = itemLaboratorio.NombreLaboratorio,
                                        NombreLinea = itemLinea.NombreLinea,
                                        ProductoId = itemProducto.ProductoId,
                                        NombreProducto = itemProducto.NombreProducto,
                                        PrecioVentaPaquete = itemPrecio.PrecioVentaPaquete,
                                        PrecioVentaUnidad = itemPrecio.PrecioVentaUnidad,
                                        CantidadPaquete = itemLeftJoin == null ? 0 : itemLeftJoin.CantidadPaqueteTotal,
                                        CantidadUnidad = itemLeftJoin == null ? 0 : itemLeftJoin.CantidadUnidadTotal,
                                        FechaVencimiento = itemProdVencimiento.FechaVencimientoMinima,
                                        VencimientoEnDias = CalculaVencimientoEnDias(itemProdVencimiento.FechaVencimientoMinima)
                                    }).Where(x => filter == null || x.NombreLaboratorio.Contains(filter) || x.NombreLinea.Contains(filter) || x.NombreProducto.Contains(filter))
                                    .Count();

            records.CurrentPage = page;
            records.PageSize = pageSize;

            return View(records);
        }

        // GET: /Ventas/AddProducto/5
        public ActionResult AddProducto(int? productoId)
        {
            if (productoId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            VentasDetalleModels ventasDetalle = (from itemProducto in db.Producto.ToList()
                                                join itemPrecio in db.PrecioProducto.ToList()
                                                on itemProducto.ProductoId equals itemPrecio.ProductoId
                                                join itemInventario in db.Inventario.ToList()
                                                on itemProducto.ProductoId equals itemInventario.ProductoId
                                                join itemGenero in db.Genero.ToList()
                                                on itemProducto.GeneroId equals itemGenero.GeneroId
                                                where itemPrecio.SucursalId == Convert.ToInt32(Session["Sucursal"]) 
                                                && itemProducto.ProductoId == productoId 
                                                select new VentasDetalleModels 
                                                {
                                                     ProductoId = itemProducto.ProductoId,
                                                     NombreProducto = itemProducto.NombreProducto,
                                                     PrecioVentaPaquete = itemPrecio.PrecioVentaPaquete,
                                                     PrecioVentaUnidad = itemPrecio.PrecioVentaUnidad,
                                                     CantidadPaquete = 0,
                                                     CantidadUnidad = 0,
                                                     TipoVenta = itemProducto.CatTipo,
                                                     StockPaquetes = itemInventario.CantidadPaqueteTotal,
                                                     StockUnidades = itemInventario.CantidadUnidadTotal,
                                                     UnidadesPorPaquete = itemProducto.UnidadesPorPaquete,
                                                     EsProductoControlado = itemGenero.Controlado,                                                     
                                                }).FirstOrDefault();

            if (ventasDetalle == null)
            {
                return HttpNotFound();
            }

            return PartialView(ventasDetalle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProducto(VentasDetalleModels ventasDetalle)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Int64 ventasId = 0;

                    var ventaPendiente = from item in db.Ventas.ToList()
                                         where item.CatEstadoVenta == "PENDIENTE"
                                         && item.UsuarioCreacion == Convert.ToString(Session["UserName"])
                                         && item.SucursalId == Convert.ToInt32(Session["Sucursal"]) 
                                         select item;

                    if (ventaPendiente == null || ventaPendiente.Count() == 0)
                    {
                        Ventas ventasDB = new Ventas()
                        {
                            SucursalId = Convert.ToInt32(Session["Sucursal"]),
                            Factura = 0,
                            Nit = 0,
                            NombreCliente = "Sin Nombre",
                            TotalVenta = 0,
                            TotalPagado = 0,
                            Cambio = 0,
                            CatEstadoVenta = "PENDIENTE",
                            UsuarioModificacion = Convert.ToString(Session["UserName"]), 
                            FechaModificacion = DateTime.Now, 
                            UsuarioCreacion = Convert.ToString(Session["UserName"]),
                            FechaCreacion = DateTime.Now
                        };

                        db.Ventas.Add(ventasDB);
                        db.SaveChanges();

                        ventasId = ventasDB.VentasId; // recupera el id generado                        
                    }
                    else
                    {
                        ventasId = ventaPendiente.FirstOrDefault().VentasId;
                    }

                    var producto = db.Producto.Find(ventasDetalle.ProductoId);
                    decimal totalUnidades = ventasDetalle.CantidadPaquete * producto.UnidadesPorPaquete + ventasDetalle.CantidadUnidad;

                    int paquetes = Convert.ToInt32(totalUnidades / producto.UnidadesPorPaquete);
                    int unidades = Convert.ToInt32(totalUnidades % producto.UnidadesPorPaquete);

                    VentasDetalle ventasDetalleDB = new VentasDetalle()
                    {
                        VentasId = ventasId,
                        ProductoId = ventasDetalle.ProductoId,
                        CantidadPaquete = paquetes,
                        CantidadUnidad = unidades,
                        Subtotal = ventasDetalle.Subtotal
                    };

                    db.VentasDetalle.Add(ventasDetalleDB);
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

        private int CalculaVencimientoEnDias(DateTime fechaVencimiento)
        {
            int diferencia = 0;
            TimeSpan ts = fechaVencimiento - DateTime.Today;

            diferencia = ts.Days;
            
            return diferencia;
        }
	}
}