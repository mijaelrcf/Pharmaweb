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
    [Authorize(Roles = "PrincipioActivo, Administrador")]
    public class PrincipioActivoController : Controller
    {
        private PharmawebEntities db = new PharmawebEntities();

        // GET: /PrincipioActivo/
        public ActionResult Index(string filter = null, int page = 1, int pageSize = 10, string sort = "NombrePrincipioActivo", string sortdir = "ASC")
        {
            var records = new PagedList<PrincipioActivoModels>();
            ViewBag.filter = filter;
            
            records.Content = (from item in db.PrincipioActivo.ToList()
                               select new PrincipioActivoModels
                               {
                                   PrincipioActivoId = item.PrincipioActivoId,
                                   NombrePrincipioActivo = item.NombrePrincipioActivo
                               }).Where(x => filter == null || x.NombrePrincipioActivo.Contains(filter.ToUpper()))
                                .OrderBy(sort + " " + sortdir)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            // Count
            records.TotalRecords = (from item in db.PrincipioActivo.ToList()
                                    select new PrincipioActivoModels
                                    {
                                        PrincipioActivoId = item.PrincipioActivoId,
                                        NombrePrincipioActivo = item.NombrePrincipioActivo
                                    }).Where(x => filter == null || x.NombrePrincipioActivo.Contains(filter.ToUpper()))
                                    .Count(); 

            records.CurrentPage = page;
            records.PageSize = pageSize;

            return View(records);
        }

        // GET: /PrincipioActivo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var principioBD = db.PrincipioActivo.Find(id);
            PrincipioActivoModels principioActivo = new PrincipioActivoModels()
            {
                NombrePrincipioActivo = principioBD.NombrePrincipioActivo,
                PrincipioActivoId = principioBD.PrincipioActivoId
            };

            if (principioActivo == null)
            {
                return HttpNotFound();
            }
            return PartialView(principioActivo);
        }

        // GET: /PrincipioActivo/Create
        public ActionResult Create()
        {
            PrincipioActivoModels principioActivo = new PrincipioActivoModels();
            return PartialView(principioActivo);
        }

        // POST: /PrincipioActivo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(PrincipioActivoModels principioActivo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    PrincipioActivo principioActivoDB = new PrincipioActivo()
                    {
                        PrincipioActivoId = principioActivo.PrincipioActivoId,
                        NombrePrincipioActivo = principioActivo.NombrePrincipioActivo.ToUpper(),
                        UsuarioModificacion = Convert.ToString(Session["UserName"]),
                        FechaModificacion = DateTime.Now,
                        UsuarioCreacion = Convert.ToString(Session["UserName"]),
                        FechaCreacion = DateTime.Now
                    };

                    db.PrincipioActivo.Add(principioActivoDB);
                    db.SaveChanges();

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, mensajeError = ex.Message });
                }
            }

            return Json(principioActivo, JsonRequestBehavior.AllowGet);
        }

        // GET: /PrincipioActivo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var principioBD = db.PrincipioActivo.Find(id);

            PrincipioActivoModels principioActivo = new PrincipioActivoModels()
            {
                NombrePrincipioActivo = principioBD.NombrePrincipioActivo,
                PrincipioActivoId = principioBD.PrincipioActivoId,
                UsuarioModificacion = principioBD.UsuarioModificacion,
                FechaModificacion = principioBD.FechaModificacion,
                UsuarioCreacion = principioBD.UsuarioCreacion,
                FechaCreacion = principioBD.FechaCreacion
            };

            if (principioActivo == null)
            {
                return HttpNotFound();
            }

            return PartialView(principioActivo);
        }

        // POST: /PrincipioActivo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PrincipioActivoModels principioActivo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    PrincipioActivo principioActivoDB = new PrincipioActivo()
                    {
                        PrincipioActivoId = principioActivo.PrincipioActivoId,
                        NombrePrincipioActivo = principioActivo.NombrePrincipioActivo.ToUpper(),
                        UsuarioModificacion = Convert.ToString(Session["UserName"]),
                        FechaModificacion = DateTime.Now,
                        UsuarioCreacion = principioActivo.UsuarioCreacion,
                        FechaCreacion = principioActivo.FechaCreacion
                    };

                    db.Entry(principioActivoDB).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, mensajeError = ex.Message });
                }
            }

            return PartialView("Edit", principioActivo);
        }

        // GET: /PrincipioActivo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var principioBD = db.PrincipioActivo.Find(id);

            PrincipioActivoModels principioActivo = new PrincipioActivoModels()
            {
                NombrePrincipioActivo = principioBD.NombrePrincipioActivo,
                PrincipioActivoId = principioBD.PrincipioActivoId
            };

            if (principioActivo == null)
            {
                return HttpNotFound();
            }
            return PartialView(principioActivo);
        }

        // POST: /PrincipioActivo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(PrincipioActivoModels principioActivo)
        {
            try
            {
                PrincipioActivo principioactivoDB = db.PrincipioActivo.Find(principioActivo.PrincipioActivoId);
                db.PrincipioActivo.Remove(principioactivoDB);
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
