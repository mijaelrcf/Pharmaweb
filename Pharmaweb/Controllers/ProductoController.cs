using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Pharmaweb.Models;
using System.Linq.Dynamic;
using Data;


namespace Pharmaweb.Controllers
{
    [Authorize(Roles = "Producto, Administrador")]
    public class ProductoController : Controller
    {
        private PharmawebEntities db = new PharmawebEntities();

        // GET: /Producto/
        public ActionResult Index(string filter = null, int page = 1, int pageSize = 10, string sort = "NombreProducto", string sortdir = "ASC")
        {
            var records = new PagedList<ProductoModels>();
            ViewBag.filter = filter;
            if (filter != null) filter = filter.ToUpper();

            records.Content = (from itemProducto in db.Producto.ToList()
                               join itemLinea in db.Linea.ToList()
                               on itemProducto.LineaId equals itemLinea.LineaId
                               join itemLaboratorio in db.Laboratorio.ToList()
                               on itemLinea.LaboratorioId equals itemLaboratorio.LaboratorioId 
                               join itemGenero in db.Genero.ToList()
                               on itemProducto.GeneroId equals itemGenero.GeneroId
                               join itemPrincipioActivo in db.PrincipioActivo.ToList()
                               on itemProducto.PrincipioActivoId equals itemPrincipioActivo.PrincipioActivoId
                               select new ProductoModels
                               {
                                   NombreLaboratorio = itemLaboratorio.NombreLaboratorio,
                                   LineaId = itemProducto.LineaId,
                                   NombreLinea = itemLinea.NombreLinea,
                                   ProductoId = itemProducto.ProductoId,
                                   NombreProducto = itemProducto.NombreProducto,                                   
                                   CatTipo = itemProducto.CatTipo,
                                   GeneroId = itemProducto.GeneroId, 
                                   NombreGenero = itemGenero.NombreGenero,
                                   PrincipioActivoId = itemProducto.PrincipioActivoId,
                                   NombrePrincipioActivo = itemPrincipioActivo.NombrePrincipioActivo,                                   
                                   UnidadesPorPaquete = itemProducto.UnidadesPorPaquete
                               }).Where(x => filter == null || x.NombreLaboratorio.Contains(filter) || x.NombreLinea.Contains(filter) || x.NombreProducto.Contains(filter) || x.NombrePrincipioActivo.Contains(filter))
                                .OrderBy(sort + " " + sortdir)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            // Count
            records.TotalRecords = (from itemProducto in db.Producto.ToList()
                                    join itemLinea in db.Linea.ToList()
                                    on itemProducto.LineaId equals itemLinea.LineaId
                                    join itemLaboratorio in db.Laboratorio.ToList()
                                    on itemLinea.LaboratorioId equals itemLaboratorio.LaboratorioId
                                    join itemGenero in db.Genero.ToList()
                                    on itemProducto.GeneroId equals itemGenero.GeneroId
                                    join itemPrincipioActivo in db.PrincipioActivo.ToList()
                                    on itemProducto.PrincipioActivoId equals itemPrincipioActivo.PrincipioActivoId
                                    select new ProductoModels
                                    {
                                        NombreLaboratorio = itemLaboratorio.NombreLaboratorio,
                                        LineaId = itemProducto.LineaId,
                                        NombreLinea = itemLinea.NombreLinea,
                                        ProductoId = itemProducto.ProductoId,
                                        NombreProducto = itemProducto.NombreProducto,
                                        CatTipo = itemProducto.CatTipo,
                                        GeneroId = itemProducto.GeneroId,
                                        NombreGenero = itemGenero.NombreGenero,
                                        PrincipioActivoId = itemProducto.PrincipioActivoId,
                                        NombrePrincipioActivo = itemPrincipioActivo.NombrePrincipioActivo,
                                        UnidadesPorPaquete = itemProducto.UnidadesPorPaquete
                                    }).Where(x => filter == null || x.NombreLaboratorio.Contains(filter) || x.NombreLinea.Contains(filter) || x.NombreProducto.Contains(filter) || x.NombrePrincipioActivo.Contains(filter))
                                   .Count();
           
            records.CurrentPage = page;
            records.PageSize = pageSize;

            return View(records);
        }

        // GET: /Producto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var productoBD = db.Producto.Find(id);
            var lineaBD = db.Linea.Where(x => x.LineaId == productoBD.LineaId).FirstOrDefault();

            ProductoModels producto = new ProductoModels()
            {
                CatTipo = productoBD.CatTipo,
                GeneroId = productoBD.GeneroId,
                LineaId = productoBD.LineaId,                
                NombreProducto = productoBD.NombreProducto,
                PrincipioActivoId = productoBD.PrincipioActivoId,
                ProductoId = productoBD.ProductoId,
                UnidadesPorPaquete = productoBD.UnidadesPorPaquete,
                LaboratorioId = lineaBD.LaboratorioId,

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

                lstCatTipo = db.CatalogoDetalle.Where(x => x.CatalogoId == "TIPO_PRODUCTO").Select(x => new SelectListItem
                {
                    Text = x.Detalle,
                    Value = x.Valor
                }).ToList(),

                lstGenero = db.Genero.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreGenero,
                    Value = x.GeneroId.ToString()
                }).ToList(),
                
                lstPrincipioActivo = db.PrincipioActivo.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombrePrincipioActivo,
                    Value = x.PrincipioActivoId.ToString()
                }).ToList()
            };

            if (producto == null)
            {
                return HttpNotFound();
            }

            return PartialView(producto);
        }

        // GET: /Producto/Create
        public ActionResult Create()
        {
            ProductoModels producto = new ProductoModels()
            {
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

                lstCatTipo = db.CatalogoDetalle.Where(x => x.CatalogoId == "TIPO_PRODUCTO").Select(x => new SelectListItem
                {
                    Text = x.Detalle,
                    Value = x.Valor
                }).OrderBy(x => x.Text).ToList(),

                lstGenero = db.Genero.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreGenero,
                    Value = x.GeneroId.ToString()
                }).OrderBy(x => x.Text).ToList(),

                lstPrincipioActivo = db.PrincipioActivo.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombrePrincipioActivo,
                    Value = x.PrincipioActivoId.ToString()
                }).OrderBy(x => x.Text).ToList()                
            };

            return PartialView(producto);
        }

        // POST: /Producto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductoModels producto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Producto productoDB = new Producto()
                    {
                        CatTipo = producto.CatTipo,
                        GeneroId = producto.GeneroId,
                        LineaId = producto.LineaId,                        
                        NombreProducto = producto.NombreProducto.ToUpper(),
                        PrincipioActivoId = producto.PrincipioActivoId,
                        ProductoId = producto.ProductoId,
                        UnidadesPorPaquete = producto.UnidadesPorPaquete,
                        UsuarioModificacion = Convert.ToString(Session["UserName"]),
                        FechaModificacion = DateTime.Now,
                        UsuarioCreacion = Convert.ToString(Session["UserName"]),
                        FechaCreacion = DateTime.Now
                    };

                    db.Producto.Add(productoDB);
                    db.SaveChanges();

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, mensajeError = ex.Message });
                }
            }

            return Json(producto, JsonRequestBehavior.AllowGet);
        }

        // GET: /Producto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var productoBD = db.Producto.Find(id);
            var lineaBD = db.Linea.Where(x => x.LineaId == productoBD.LineaId).FirstOrDefault();

            ProductoModels producto = new ProductoModels()
            {
                CatTipo = productoBD.CatTipo,
                GeneroId = productoBD.GeneroId,
                LineaId = productoBD.LineaId,                
                NombreProducto = productoBD.NombreProducto,
                PrincipioActivoId = productoBD.PrincipioActivoId,
                ProductoId = productoBD.ProductoId,
                UnidadesPorPaquete = productoBD.UnidadesPorPaquete,
                LaboratorioId = lineaBD.LaboratorioId,
                UsuarioModificacion = productoBD.UsuarioModificacion,
                FechaModificacion = productoBD.FechaModificacion,
                UsuarioCreacion = productoBD.UsuarioCreacion,
                FechaCreacion = productoBD.FechaCreacion,

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

                lstCatTipo = db.CatalogoDetalle.Where(x => x.CatalogoId == "TIPO_PRODUCTO").Select(x => new SelectListItem
                {
                    Text = x.Detalle,
                    Value = x.Valor
                }).OrderBy(x => x.Text).ToList(),

                lstGenero = db.Genero.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreGenero,
                    Value = x.GeneroId.ToString()
                }).OrderBy(x => x.Text).ToList(),

                lstPrincipioActivo = db.PrincipioActivo.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombrePrincipioActivo,
                    Value = x.PrincipioActivoId.ToString()
                }).OrderBy(x => x.Text).ToList()  
            };

            if (producto == null)
            {
                return HttpNotFound();
            }

            return PartialView(producto);
        }

        // POST: /Producto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductoModels producto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Producto productoDB = new Producto()
                    {
                        CatTipo = producto.CatTipo,
                        GeneroId = producto.GeneroId,
                        LineaId = producto.LineaId,                        
                        NombreProducto = producto.NombreProducto.ToUpper(),
                        PrincipioActivoId = producto.PrincipioActivoId,
                        ProductoId = producto.ProductoId,
                        UnidadesPorPaquete = producto.UnidadesPorPaquete,
                        UsuarioModificacion = Convert.ToString(Session["UserName"]),
                        FechaModificacion = DateTime.Now,
                        UsuarioCreacion = producto.UsuarioCreacion,
                        FechaCreacion = producto.FechaCreacion
                    };

                    db.Entry(productoDB).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, mensajeError = ex.Message });
                }
            }

            return PartialView("Edit", producto);
        }

        // GET: /Producto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var productoBD = db.Producto.Find(id);
            var lineaBD = db.Linea.Where(x => x.LineaId == productoBD.LineaId).FirstOrDefault();

            ProductoModels producto = new ProductoModels()
            {
                CatTipo = productoBD.CatTipo,
                GeneroId = productoBD.GeneroId,
                LineaId = productoBD.LineaId,                
                NombreProducto = productoBD.NombreProducto,
                PrincipioActivoId = productoBD.PrincipioActivoId,
                ProductoId = productoBD.ProductoId,
                UnidadesPorPaquete = productoBD.UnidadesPorPaquete,
                LaboratorioId = lineaBD.LaboratorioId,

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

                lstCatTipo = db.CatalogoDetalle.Where(x => x.CatalogoId == "TIPO_PRODUCTO").Select(x => new SelectListItem
                {
                    Text = x.Detalle,
                    Value = x.Valor
                }).ToList(),

                lstGenero = db.Genero.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreGenero,
                    Value = x.GeneroId.ToString()
                }).ToList(),

                lstPrincipioActivo = db.PrincipioActivo.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombrePrincipioActivo,
                    Value = x.PrincipioActivoId.ToString()
                }).ToList()  
            };

            if (producto == null)
            {
                return HttpNotFound();
            }

            return PartialView(producto);
        }

        // POST: /Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ProductoModels producto)
        {
            try
            {
                Producto productoBD = db.Producto.Find(producto.ProductoId);
                db.Producto.Remove(productoBD);
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

        public JsonResult GetNombreProducto(string term)
        {
            var results = db.Producto.Where(s => term == null || s.NombreProducto.ToLower().Contains(term.ToLower())).Select(x => new { id = x.ProductoId, value = x.NombreProducto }).Take(5).OrderBy(x => x.value).ToList();

            return Json(results, JsonRequestBehavior.AllowGet);
        }
    }
}
