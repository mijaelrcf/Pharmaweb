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

namespace Pharmaweb.Controllers
{
    [Authorize(Roles = "Linea, Administrador")]
    public class LineaController : Controller
    {
        private Data.PharmawebEntities db = new Data.PharmawebEntities();

        // GET: /Linea/
        public ActionResult Index(string filter = null, int page = 1, int pageSize = 10, string sort = "NombreLinea", string sortdir = "ASC")
        {
            var records = new PagedList<LineaModels>();
            ViewBag.filter = filter;

            records.Content = (from item in db.Linea.ToList()
                               join itemLaboratorio in db.Laboratorio.ToList()
                               on item.LaboratorioId equals itemLaboratorio.LaboratorioId
                               join itemCatalogoDetalle in db.CatalogoDetalle 
                               on item.CatProcedencia equals itemCatalogoDetalle.Valor
                               select new LineaModels
                               {
                                   LineaId = item.LineaId,
                                   NombreLinea = item.NombreLinea,
                                   LaboratorioId = item.LaboratorioId,
                                   NombreLaboratorio = itemLaboratorio.NombreLaboratorio,
                                   CatProcedencia = itemCatalogoDetalle.Detalle,
                                   Margen = item.Margen
                               }).Where(x => filter == null || x.NombreLaboratorio.Contains(filter.ToUpper()) || x.NombreLinea.Contains(filter.ToUpper()))
                                .OrderBy(sort + " " + sortdir)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();
            // Count
            records.TotalRecords = (from item in db.Linea.ToList()
                                    join itemLaboratorio in db.Laboratorio.ToList()
                                    on item.LaboratorioId equals itemLaboratorio.LaboratorioId
                                    join itemCatalogoDetalle in db.CatalogoDetalle
                                    on item.CatProcedencia equals itemCatalogoDetalle.Valor
                                    select new LineaModels
                                    {
                                        LineaId = item.LineaId,
                                        NombreLinea = item.NombreLinea,
                                        LaboratorioId = item.LaboratorioId,
                                        NombreLaboratorio = itemLaboratorio.NombreLaboratorio,
                                        CatProcedencia = itemCatalogoDetalle.Detalle,
                                        Margen = item.Margen
                                    }).Where(x => filter == null || x.NombreLaboratorio.Contains(filter.ToUpper()) || x.NombreLinea.Contains(filter.ToUpper()))
                                    .Count();

            records.CurrentPage = page;
            records.PageSize = pageSize;

            return View(records);
        }

        // GET: /Linea/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var lineaBD = db.Linea.Find(id);

            LineaModels linea = new LineaModels()
            {
                LineaId = lineaBD.LineaId,
                LaboratorioId = lineaBD.LaboratorioId,
                NombreLinea = lineaBD.NombreLinea,
                CatProcedencia = lineaBD.CatProcedencia,
                Margen = lineaBD.Margen,

                lstLaboratorio = db.Laboratorio.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreLaboratorio,
                    Value = x.LaboratorioId.ToString()
                }).ToList(),

                lstCatProcedencia = db.CatalogoDetalle.Where(x => x.CatalogoId == "PAIS").Select(x => new SelectListItem
                {
                    Text = x.Detalle,
                    Value = x.Valor
                }).ToList()
            };

            if (linea == null)
            {
                return HttpNotFound();
            }
            return PartialView(linea);
        }

        // GET: /Linea/Create
        public ActionResult Create()
        {
            LineaModels linea = new LineaModels()
            {
                lstLaboratorio = db.Laboratorio.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreLaboratorio,
                    Value = x.LaboratorioId.ToString()
                }).OrderBy(x => x.Text).ToList(),

                lstCatProcedencia = db.CatalogoDetalle.Where(x=> x.CatalogoId == "PAIS").Select(x => new SelectListItem
                {
                    Text = x.Detalle,
                    Value = x.Valor
                }).OrderBy(x => x.Text).ToList()
            };
            return PartialView(linea);
        }

        // POST: /Linea/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LineaModels linea)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Data.Linea lineaDB = new Data.Linea()
                    {
                        LaboratorioId = linea.LaboratorioId,
                        CatProcedencia = linea.CatProcedencia,
                        NombreLinea = linea.NombreLinea.ToUpper(),
                        LineaId = linea.LineaId,
                        Margen = linea.Margen,
                        UsuarioModificacion = Convert.ToString(Session["UserName"]),
                        FechaModificacion = DateTime.Now,
                        UsuarioCreacion = Convert.ToString(Session["UserName"]),
                        FechaCreacion = DateTime.Now
                    };

                    db.Linea.Add(lineaDB);
                    db.SaveChanges();

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, mensajeError = ex.Message });
                }
            }

            return Json(linea, JsonRequestBehavior.AllowGet);
        }

        // GET: /Linea/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var lineaBD = db.Linea.Find(id);

            LineaModels linea = new LineaModels()
            {
                LineaId = lineaBD.LineaId,
                LaboratorioId = lineaBD.LaboratorioId,
                NombreLinea = lineaBD.NombreLinea,
                CatProcedencia = lineaBD.CatProcedencia,
                Margen = lineaBD.Margen,
                UsuarioModificacion = lineaBD.UsuarioModificacion,
                FechaModificacion = lineaBD.FechaModificacion,
                UsuarioCreacion = lineaBD.UsuarioCreacion,
                FechaCreacion = lineaBD.FechaCreacion,
                
                lstLaboratorio = db.Laboratorio.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreLaboratorio,
                    Value = x.LaboratorioId.ToString()
                }).OrderBy(x => x.Text).ToList(),

                lstCatProcedencia = db.CatalogoDetalle.Where(x => x.CatalogoId == "PAIS").Select(x => new SelectListItem
                {
                    Text = x.Detalle,
                    Value = x.Valor
                }).OrderBy(x => x.Text).ToList()
            };

            if (linea == null)
            {
                return HttpNotFound();
            }
            return PartialView(linea);
        }

        // POST: /Linea/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LineaModels linea)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Data.Linea lineaDB = new Data.Linea()
                    {
                        LineaId = linea.LineaId,
                        LaboratorioId = linea.LaboratorioId,
                        CatProcedencia = linea.CatProcedencia,
                        NombreLinea = linea.NombreLinea.ToUpper(),
                        Margen = linea.Margen,
                        UsuarioModificacion = Convert.ToString(Session["UserName"]),
                        FechaModificacion = DateTime.Now,
                        UsuarioCreacion = linea.UsuarioCreacion,
                        FechaCreacion = linea.FechaCreacion
                    };

                    db.Entry(lineaDB).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, mensajeError = ex.Message });
                }
            }

            return PartialView("Edit", linea);
        }

        // GET: /Linea/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var lineaBD = db.Linea.Find(id);

            LineaModels linea = new LineaModels()
            {
                LineaId = lineaBD.LineaId,
                LaboratorioId = lineaBD.LaboratorioId,
                NombreLinea = lineaBD.NombreLinea,
                CatProcedencia = lineaBD.CatProcedencia,
                Margen = lineaBD.Margen,

                lstLaboratorio = db.Laboratorio.ToList().Select(x => new SelectListItem
                {
                    Text = x.NombreLaboratorio,
                    Value = x.LaboratorioId.ToString()
                }).ToList(),

                lstCatProcedencia = db.CatalogoDetalle.Where(x => x.CatalogoId == "PAIS").Select(x => new SelectListItem
                {
                    Text = x.Detalle,
                    Value = x.Valor
                }).ToList()
            };

            if (linea == null)
            {
                return HttpNotFound();
            }
            return PartialView(linea);
        }

        // POST: /Linea/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(LineaModels linea)
        {
            try
            {
                Data.Linea lineaDB = db.Linea.Find(linea.LineaId);
                db.Linea.Remove(lineaDB);
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
