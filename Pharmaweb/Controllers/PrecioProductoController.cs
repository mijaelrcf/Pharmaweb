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
    [Authorize(Roles = "PrecioProducto, Administrador")]
    public class PrecioProductoController : Controller
    {
        private PharmawebEntities db = new PharmawebEntities();

        // GET: /PrecioProducto/
        public ActionResult Index(string filter = null, int page = 1, int pageSize = 10, string sort = "NombreProducto", string sortdir = "DESC")
        {
            var records = new PagedList<PrecioProductoModels>();
            ViewBag.filter = filter;
            if (filter != null) filter = filter.ToUpper();

            records.Content = (from itemPrecio in db.PrecioProducto.ToList()
                               join itemSucursal in db.Sucursal.ToList()
                               on itemPrecio.SucursalId equals itemSucursal.SucursalId
                               join itemProducto in db.Producto.ToList()
                               on itemPrecio.ProductoId equals itemProducto.ProductoId
                               join itemLinea in db.Linea.ToList()
                               on itemProducto.LineaId equals itemLinea.LineaId
                               join itemLaboratorio in db.Laboratorio.ToList()
                               on itemLinea.LaboratorioId equals itemLaboratorio.LaboratorioId
                               select new PrecioProductoModels
                               {
                                   SucursalId = itemPrecio.SucursalId,
                                   NombreSucursal = itemSucursal.NombreSucursal,
                                   ProductoId = itemPrecio.ProductoId,
                                   NombreProducto = itemProducto.NombreProducto,
                                   NombreLaboratorio = itemLaboratorio.NombreLaboratorio,
                                   NombreLinea = itemLinea.NombreLinea,
                                   Costo = itemPrecio.Costo,
                                   Base = itemPrecio.Base,
                                   Bonificacion = itemPrecio.Bonificacion,
                                   Descuento1 = itemPrecio.Descuento1,
                                   Descuento2 = itemPrecio.Descuento2,
                                   Descuento3 = itemPrecio.Descuento3,
                                   Descuento4 = itemPrecio.Descuento4,
                                   CostoTotal = itemPrecio.CostoTotal,
                                   PrecioVentaPaquete = itemPrecio.PrecioVentaPaquete,
                                   PrecioVentaUnidad = itemPrecio.PrecioVentaUnidad,
                               }).Where(x => filter == null || x.NombreLaboratorio.Contains(filter) || x.NombreLinea.Contains(filter) || x.NombreProducto.Contains(filter))
                                .OrderBy(sort + " " + sortdir)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            // Count
            records.TotalRecords = (from itemPrecio in db.PrecioProducto.ToList()
                                    join itemSucursal in db.Sucursal.ToList()
                                    on itemPrecio.SucursalId equals itemSucursal.SucursalId
                                    join itemProducto in db.Producto.ToList()
                                    on itemPrecio.ProductoId equals itemProducto.ProductoId
                                    join itemLinea in db.Linea.ToList()
                                    on itemProducto.LineaId equals itemLinea.LineaId
                                    join itemLaboratorio in db.Laboratorio.ToList()
                                    on itemLinea.LaboratorioId equals itemLaboratorio.LaboratorioId
                                    select new PrecioProductoModels
                                    {
                                        SucursalId = itemPrecio.SucursalId,
                                        NombreSucursal = itemSucursal.NombreSucursal,
                                        ProductoId = itemPrecio.ProductoId,
                                        NombreProducto = itemProducto.NombreProducto,
                                        NombreLaboratorio = itemLaboratorio.NombreLaboratorio,
                                        NombreLinea = itemLinea.NombreLinea,
                                        Costo = itemPrecio.Costo,
                                        Base = itemPrecio.Base,
                                        Bonificacion = itemPrecio.Bonificacion,
                                        Descuento1 = itemPrecio.Descuento1,
                                        Descuento2 = itemPrecio.Descuento2,
                                        Descuento3 = itemPrecio.Descuento3,
                                        Descuento4 = itemPrecio.Descuento4,
                                        CostoTotal = itemPrecio.CostoTotal,
                                        PrecioVentaPaquete = itemPrecio.PrecioVentaPaquete,
                                        PrecioVentaUnidad = itemPrecio.PrecioVentaUnidad,
                                    }).Where(x => filter == null || x.NombreLaboratorio.Contains(filter) || x.NombreLinea.Contains(filter) || x.NombreProducto.Contains(filter))
                                    .Count();

            records.CurrentPage = page;
            records.PageSize = pageSize;

            return View(records);
        }

        // GET: /PrecioProducto/Details/5
        public ActionResult Details(int? sucursalId, int? productoId)
        {
            if (sucursalId == null && productoId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PrecioProducto precioProductoBD = db.PrecioProducto.Find(sucursalId, productoId);

            PrecioProductoModels precioProducto = new PrecioProductoModels()
            {
                SucursalId = precioProductoBD.SucursalId,
                ProductoId = precioProductoBD.ProductoId,
                Costo = precioProductoBD.Costo,
                Base = precioProductoBD.Base,
                Bonificacion = precioProductoBD.Bonificacion,
                Descuento1 = precioProductoBD.Descuento1,
                Descuento2 = precioProductoBD.Descuento2,
                Descuento3 = precioProductoBD.Descuento3,
                Descuento4 = precioProductoBD.Descuento4,
                CostoTotal = precioProductoBD.CostoTotal,
                PrecioVentaPaquete = precioProductoBD.PrecioVentaPaquete,
                PrecioVentaUnidad = precioProductoBD.PrecioVentaUnidad,

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
                }).ToList()
            };

            if (precioProducto == null)
            {
                return HttpNotFound();
            }

            return PartialView(precioProducto);
        }

        // GET: /PrecioProducto/Create
        public ActionResult Create()
        {
            PrecioProductoModels precioProducto = new PrecioProductoModels()
            {
                lstSucursal = db.Sucursal.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreSucursal,
                    Value = x.SucursalId.ToString()
                }).OrderBy(x => x.Text).ToList(),

                lstLaboratorio = db.Laboratorio.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreLaboratorio,
                    Value = x.LaboratorioId.ToString()
                }).OrderBy(x => x.Text).ToList(),

                lstLinea = db.Linea.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreLinea,
                    Value = x.LineaId.ToString()
                }).OrderBy(x => x.Text).ToList(),

                lstProducto = db.Producto.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreProducto,
                    Value = x.ProductoId.ToString()
                }).OrderBy(x => x.Text).ToList()
            };

            return PartialView(precioProducto);
        }

        // POST: /PrecioProducto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PrecioProductoModels precioProducto)
        {
            //if (ModelState.IsValid)
            //{
                try
                {
                    PrecioProducto precioProductoDB = new PrecioProducto()
                    {
                        SucursalId = precioProducto.SucursalId,
                        ProductoId = precioProducto.ProductoId,
                        Costo = precioProducto.Costo,
                        Base = precioProducto.Base,
                        Bonificacion = precioProducto.Bonificacion,
                        Descuento1 = precioProducto.Descuento1,
                        Descuento2 = precioProducto.Descuento2,
                        Descuento3 = precioProducto.Descuento3,
                        Descuento4 = precioProducto.Descuento4,
                        CostoTotal = precioProducto.CostoTotal,
                        PrecioVentaPaquete = precioProducto.PrecioVentaPaquete,
                        PrecioVentaUnidad = precioProducto.PrecioVentaUnidad,
                        UsuarioModificacion = Convert.ToString(Session["UserName"]),
                        FechaModificacion = DateTime.Now,
                        UsuarioCreacion = Convert.ToString(Session["UserName"]),
                        FechaCreacion = DateTime.Now
                    };

                    db.PrecioProducto.Add(precioProductoDB);
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

        // GET: /PrecioProducto/Edit/5
        public ActionResult Edit(int? sucursalId, int? productoId)
        {
            if (sucursalId == null && productoId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PrecioProducto precioProductoBD = db.PrecioProducto.Find(sucursalId, productoId);

            var productoLinea = (from itemProducto in db.Producto.ToList()
                                join itemLinea in db.Linea.ToList()
                                on itemProducto.LineaId equals itemLinea.LineaId
                                where itemProducto.ProductoId == productoId
                                select new { itemLinea.Margen, itemLinea.LineaId, itemLinea.LaboratorioId, itemProducto.UnidadesPorPaquete }).FirstOrDefault();

            PrecioProductoModels precioProducto = new PrecioProductoModels()
            {
                SucursalId = precioProductoBD.SucursalId,
                ProductoId = precioProductoBD.ProductoId,
                LaboratorioId = productoLinea.LaboratorioId,
                LineaId = productoLinea.LineaId,
                Margen = productoLinea.Margen,
                UnidadPaquete = productoLinea.UnidadesPorPaquete,
                Costo = precioProductoBD.Costo,
                Base = precioProductoBD.Base,
                Bonificacion = precioProductoBD.Bonificacion,
                Descuento1 = precioProductoBD.Descuento1,
                Descuento2 = precioProductoBD.Descuento2,
                Descuento3 = precioProductoBD.Descuento3,
                Descuento4 = precioProductoBD.Descuento4,
                CostoTotal = precioProductoBD.CostoTotal,
                PrecioVentaPaquete = precioProductoBD.PrecioVentaPaquete,
                PrecioVentaUnidad = precioProductoBD.PrecioVentaUnidad,
                UsuarioModificacion = precioProductoBD.UsuarioModificacion,
                FechaModificacion = precioProductoBD.FechaModificacion,
                UsuarioCreacion = precioProductoBD.UsuarioCreacion,
                FechaCreacion = precioProductoBD.FechaCreacion,

                lstSucursal = db.Sucursal.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreSucursal,
                    Value = x.SucursalId.ToString()
                }).OrderBy(x => x.Text).ToList(),

                lstLaboratorio = db.Laboratorio.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreLaboratorio,
                    Value = x.LaboratorioId.ToString()
                }).OrderBy(x => x.Text).ToList(),

                lstLinea = db.Linea.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreLinea,
                    Value = x.LineaId.ToString()
                }).OrderBy(x => x.Text).ToList(),

                lstProducto = db.Producto.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreProducto,
                    Value = x.ProductoId.ToString()
                }).OrderBy(x => x.Text).ToList()
            };
            
            if (precioProducto == null)
            {
                return HttpNotFound();
            }

            return PartialView(precioProducto);
        }

        // POST: /PrecioProducto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PrecioProductoModels precioProducto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    PrecioProducto precioProductoDB = new PrecioProducto()
                    {
                        SucursalId = precioProducto.SucursalId,
                        ProductoId = precioProducto.ProductoId,
                        Costo = precioProducto.Costo,
                        Base = precioProducto.Base,
                        Bonificacion = precioProducto.Bonificacion,
                        Descuento1 = precioProducto.Descuento1,
                        Descuento2 = precioProducto.Descuento2,
                        Descuento3 = precioProducto.Descuento3,
                        Descuento4 = precioProducto.Descuento4,
                        CostoTotal = precioProducto.CostoTotal,
                        PrecioVentaPaquete = precioProducto.PrecioVentaPaquete,
                        PrecioVentaUnidad = precioProducto.PrecioVentaUnidad,
                        UsuarioModificacion = Convert.ToString(Session["UserName"]),
                        FechaModificacion = DateTime.Now,
                        UsuarioCreacion = precioProducto.UsuarioCreacion,
                        FechaCreacion = precioProducto.FechaCreacion
                    };

                    db.Entry(precioProductoDB).State = EntityState.Modified;
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

        // GET: /PrecioProducto/Delete/5
        public ActionResult Delete(int? sucursalId, int? productoId)
        {
            if (sucursalId == null && productoId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PrecioProducto precioProductoBD = db.PrecioProducto.Find(sucursalId, productoId);

            PrecioProductoModels precioProducto = new PrecioProductoModels()
            {
                SucursalId = precioProductoBD.SucursalId,
                ProductoId = precioProductoBD.ProductoId,
                Costo = precioProductoBD.Costo,
                Base = precioProductoBD.Base,
                Bonificacion = precioProductoBD.Bonificacion,
                Descuento1 = precioProductoBD.Descuento1,
                Descuento2 = precioProductoBD.Descuento2,
                Descuento3 = precioProductoBD.Descuento3,
                Descuento4 = precioProductoBD.Descuento4,
                CostoTotal = precioProductoBD.CostoTotal,
                PrecioVentaPaquete = precioProductoBD.PrecioVentaPaquete,
                PrecioVentaUnidad = precioProductoBD.PrecioVentaUnidad,
                UsuarioModificacion = precioProductoBD.UsuarioModificacion,
                FechaModificacion = precioProductoBD.FechaModificacion,
                UsuarioCreacion = precioProductoBD.UsuarioCreacion,
                FechaCreacion = precioProductoBD.FechaCreacion,

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
                }).ToList()
            };

            if (precioProducto == null)
            {
                return HttpNotFound();
            }

            return PartialView(precioProducto);
        }

        // POST: /PrecioProducto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(PrecioProductoModels precioProducto)
        {
            try
            {
                PrecioProducto precioProductoDB = db.PrecioProducto.Find(precioProducto.SucursalId, precioProducto.ProductoId);
                db.PrecioProducto.Remove(precioProductoDB);
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

        /**
         * Metodos Auxiliares
         **/
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetLineas(int id)
        {
            var lstLinea = db.Linea.ToList().Where(x => x.LaboratorioId == id).Select(x => new SelectListItem
            {
                Text = x.NombreLinea,
                Value = x.LineaId.ToString()
            }).OrderBy(x => x.Text).ToList();
            
            SelectListItem item = new SelectListItem() { Text = "-- Seleccionar --", Value = "" };
            lstLinea.Insert(0, item);

            return Json(lstLinea, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetProductos(int id)
        {
            var lstProducto = db.Producto.ToList().Where(x => x.LineaId == id).Select(x => new SelectListItem
            {
                Text = x.NombreProducto,
                Value = x.ProductoId.ToString()
            }).OrderBy(x => x.Text).ToList();

            SelectListItem item = new SelectListItem() { Text = "-- Seleccionar --", Value = "" };
            lstProducto.Insert(0, item);

            return Json(lstProducto, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetPorcentajeGanancia(int id)
        {
            decimal margen = (from itemProducto in db.Producto.ToList()
                             join itemLinea in db.Linea.ToList()
                             on itemProducto.LineaId equals itemLinea.LineaId
                             where itemProducto.ProductoId == id
                             select itemLinea.Margen).FirstOrDefault();

            short unidadesXpaquete = db.Producto.Find(id).UnidadesPorPaquete;

            return Json(new { margen, unidadesXpaquete }, JsonRequestBehavior.AllowGet);
        }
    }
}
