using Data;
using Pharmaweb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace Pharmaweb.Controllers
{
    [Authorize(Roles = "Inventario, Administrador")]
    public class InventarioController : Controller
    {
        PharmawebEntities db = new PharmawebEntities();
        //
        // GET: /Inventario/
        public ActionResult Index(string filter = null, int page = 1, int pageSize = 10, string sort = "NombreProducto", string sortdir = "DESC")
        {
            var records = new PagedList<InventarioModels>();
            ViewBag.filter = filter;
            if(filter != null) filter = filter.ToUpper();            

            records.Content = (from itemInventario in db.Inventario.ToList()
                               join itemSucursal in db.Sucursal.ToList()
                               on itemInventario.SucursalId equals itemSucursal.SucursalId
                               join itemProducto in db.Producto.ToList()
                               on itemInventario.ProductoId equals itemProducto.ProductoId
                               join itemLinea in db.Linea.ToList()
                               on itemProducto.LineaId equals itemLinea.LineaId
                               join itemLaboratorio in db.Laboratorio.ToList()
                               on itemLinea.LaboratorioId equals itemLaboratorio.LaboratorioId
                               select new InventarioModels
                               {
                                   SucursalId = itemInventario.SucursalId,
                                   ProductoId = itemInventario.ProductoId,
                                   NombreSucursal = itemSucursal.NombreSucursal,
                                   NombreProducto = itemProducto.NombreProducto,
                                   NombreLaboratorio = itemLaboratorio.NombreLaboratorio,
                                   NombreLinea = itemLinea.NombreLinea,
                                   CantIngresoPaquete = itemInventario.CantIngresoPaquete,
                                   CantIngresoUnidad = itemInventario.CantIngresoUnidad,
                                   CantSalidaPaquete = itemInventario.CantSalidaPaquete,
                                   CantSalidaUnidad = itemInventario.CantSalidaUnidad,
                                   CantidadPaqueteTotal = itemInventario.CantidadPaqueteTotal,
                                   CantidadUnidadTotal = itemInventario.CantidadUnidadTotal,
                                   UsuarioModificacion = itemInventario.UsuarioModificacion,
                                   FechaModificacion = Convert.ToDateTime(itemInventario.FechaModificacion),
                                   UsuarioCreacion = itemInventario.UsuarioCreacion,
                                   FechaCreacion = itemInventario.FechaCreacion
                               }).Where(x => filter == null || x.NombreSucursal.Contains(filter) || x.NombreLaboratorio.Contains(filter) || x.NombreLinea.Contains(filter) || x.NombreProducto.Contains(filter))
                               .OrderBy(sort + " " + sortdir)
                               .Skip((page - 1) * pageSize)
                               .Take(pageSize)
                               .ToList();

            // Count
            records.TotalRecords = (from itemInventario in db.Inventario.ToList()
                                    join itemSucursal in db.Sucursal.ToList()
                                    on itemInventario.SucursalId equals itemSucursal.SucursalId
                                    join itemProducto in db.Producto.ToList()
                                    on itemInventario.ProductoId equals itemProducto.ProductoId
                                    join itemLinea in db.Linea.ToList()
                                    on itemProducto.LineaId equals itemLinea.LineaId
                                    join itemLaboratorio in db.Laboratorio.ToList()
                                    on itemLinea.LaboratorioId equals itemLaboratorio.LaboratorioId
                                    select new InventarioModels
                                    {
                                        SucursalId = itemInventario.SucursalId,
                                        ProductoId = itemInventario.ProductoId,
                                        NombreSucursal = itemSucursal.NombreSucursal,
                                        NombreProducto = itemProducto.NombreProducto,
                                        NombreLaboratorio = itemLaboratorio.NombreLaboratorio,
                                        NombreLinea = itemLinea.NombreLinea,
                                        CantIngresoPaquete = itemInventario.CantIngresoPaquete,
                                        CantIngresoUnidad = itemInventario.CantIngresoUnidad,
                                        CantSalidaPaquete = itemInventario.CantSalidaPaquete,
                                        CantSalidaUnidad = itemInventario.CantSalidaUnidad,
                                        CantidadPaqueteTotal = itemInventario.CantidadPaqueteTotal,
                                        CantidadUnidadTotal = itemInventario.CantidadUnidadTotal,
                                        UsuarioModificacion = itemInventario.UsuarioModificacion,
                                        FechaModificacion = Convert.ToDateTime(itemInventario.FechaModificacion),
                                        UsuarioCreacion = itemInventario.UsuarioCreacion,
                                        FechaCreacion = itemInventario.FechaCreacion
                                    }).Where(x => filter == null || x.NombreSucursal.Contains(filter) || x.NombreLaboratorio.Contains(filter) || x.NombreLinea.Contains(filter) || x.NombreProducto.Contains(filter))
                                    .Count();

            records.CurrentPage = page;
            records.PageSize = pageSize;

            return View(records);
        }
	}
}