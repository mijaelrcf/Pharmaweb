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
    [Authorize(Roles = "Laboratorio, Administrador")]
    public class LaboratorioController : Controller
    {
        private PharmawebEntities db = new PharmawebEntities();

        // GET: /Laboratorio/
        public ActionResult Index(string filter = null, int page = 1, int pageSize = 10, string sort = "NombreLaboratorio", string sortdir = "ASC")
        {
            var records = new PagedList<LaboratorioModels>();
            ViewBag.filter = filter;

            records.Content = (from item in db.Laboratorio.ToList()
                               select new LaboratorioModels
                               {
                                   LaboratorioId = item.LaboratorioId, 
                                   NombreLaboratorio = item.NombreLaboratorio
                               }).Where(x => filter == null || (x.NombreLaboratorio.Contains(filter.ToUpper())))
                                .OrderBy(sort + " " + sortdir)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            // Count
            records.TotalRecords = (from item in db.Laboratorio.ToList()
                                    select new LaboratorioModels
                                    {
                                        LaboratorioId = item.LaboratorioId,
                                        NombreLaboratorio = item.NombreLaboratorio
                                    }).Where(x => filter == null || (x.NombreLaboratorio.Contains(filter.ToUpper())))
                                    .Count();

            records.CurrentPage = page;
            records.PageSize = pageSize;

            return View(records);
        }
        
        //}
        // GET: /Laboratorio/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Laboratorio laboratorioBD = db.Laboratorio.Find(id);

            LaboratorioModels laboratorio = new LaboratorioModels()
            {
                LaboratorioId = laboratorioBD.LaboratorioId,
                NombreLaboratorio = laboratorioBD.NombreLaboratorio
            };

            if (laboratorio == null)
            {
                return HttpNotFound();
            }

            return PartialView(laboratorio);
        }

        // GET: /Laboratorio/Create
        public ActionResult Create()
        {
            LaboratorioModels laboratorio = new LaboratorioModels();

            return PartialView(laboratorio);
        }

        // POST: /Laboratorio/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LaboratorioModels laboratorio)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Laboratorio LaboratorioDB = new Laboratorio()
                    {
                        LaboratorioId = laboratorio.LaboratorioId,
                        NombreLaboratorio = laboratorio.NombreLaboratorio.ToUpper(),
                        UsuarioModificacion = Convert.ToString(Session["UserName"]),
                        FechaModificacion = DateTime.Now,
                        UsuarioCreacion = Convert.ToString(Session["UserName"]),
                        FechaCreacion = DateTime.Now
                    };

                    db.Laboratorio.Add(LaboratorioDB);
                    db.SaveChanges();

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, mensajeError = ex.Message });
                }
            }

            return Json(laboratorio, JsonRequestBehavior.AllowGet);
        }

        // GET: /Laboratorio/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Laboratorio laboratorioBD = db.Laboratorio.Find(id);

            LaboratorioModels laboratorio = new LaboratorioModels()
            {
                LaboratorioId = laboratorioBD.LaboratorioId,
                NombreLaboratorio = laboratorioBD.NombreLaboratorio,
                UsuarioModificacion = laboratorioBD.UsuarioModificacion,
                FechaModificacion = laboratorioBD.FechaModificacion,
                UsuarioCreacion = laboratorioBD.UsuarioCreacion,
                FechaCreacion = laboratorioBD.FechaCreacion
            };

            if (laboratorio == null)
            {
                return HttpNotFound();
            }

            return PartialView(laboratorio);
        }

        // POST: /Laboratorio/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Laboratorio laboratorio)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Laboratorio LaboratorioDB = new Laboratorio()
                    {
                        LaboratorioId = laboratorio.LaboratorioId,
                        NombreLaboratorio = laboratorio.NombreLaboratorio.ToUpper(),
                        UsuarioModificacion = Convert.ToString(Session["UserName"]),
                        FechaModificacion = DateTime.Now,
                        UsuarioCreacion = laboratorio.UsuarioCreacion,
                        FechaCreacion = laboratorio.FechaCreacion
                    };

                    db.Entry(LaboratorioDB).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, mensajeError = ex.Message });
                }
            }

            return PartialView(laboratorio);
        }

        // GET: /Laboratorio/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Laboratorio laboratorioBD = db.Laboratorio.Find(id);

            LaboratorioModels laboratorio = new LaboratorioModels()
            {
                LaboratorioId = laboratorioBD.LaboratorioId,
                NombreLaboratorio = laboratorioBD.NombreLaboratorio
            };

            if (laboratorio == null)
            {
                return HttpNotFound();
            }

            return PartialView(laboratorio);
        }

        // POST: /Laboratorio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(LaboratorioModels laboratorio)
        {
            try
            {
                Laboratorio laboratorioBD = db.Laboratorio.Find(laboratorio.LaboratorioId);
                db.Laboratorio.Remove(laboratorioBD);
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
