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
    [Authorize(Roles = "Genero, Administrador")]
    public class GeneroController : Controller
    {
        private PharmawebEntities db = new PharmawebEntities();

        // GET: /Genero/
        public ActionResult Index(string filter = null, int page = 1, int pageSize = 10, string sort = "NombreGenero", string sortdir = "ASC")
        {
            var records = new PagedList<GeneroModels>();
            ViewBag.filter = filter;

            records.Content = (from item in db.Genero.ToList()
                               select new GeneroModels
                               {
                                   Controlado = item.Controlado,
                                   GeneroId = item.GeneroId,
                                   NombreGenero = item.NombreGenero
                               }).Where(x => filter == null || x.NombreGenero.Contains(filter.ToUpper()))
                                .OrderBy(sort + " " + sortdir)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            // Count
            records.TotalRecords = (from item in db.Genero.ToList()
                                    select new GeneroModels
                                    {
                                        Controlado = item.Controlado,
                                        GeneroId = item.GeneroId,
                                        NombreGenero = item.NombreGenero
                                    }).Where(x => filter == null || x.NombreGenero.Contains(filter.ToUpper()))
                                    .Count();

            records.CurrentPage = page;
            records.PageSize = pageSize;

            return View(records);
        }

        // GET: /Genero/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var generoBD = db.Genero.Find(id);

            GeneroModels genero = new GeneroModels()
            {
                Controlado = generoBD.Controlado,
                GeneroId = generoBD.GeneroId,
                NombreGenero = generoBD.NombreGenero
            };

            if (genero == null)
            {
                return HttpNotFound();
            }
            return PartialView(genero);
        }

        // GET: /Genero/Create
        public ActionResult Create()
        {
            GeneroModels genero = new GeneroModels();
            return PartialView(genero);
        }

        // POST: /Genero/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(GeneroModels genero)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Genero generoDB = new Genero()
                    {
                        Controlado = genero.Controlado,
                        GeneroId = genero.GeneroId, 
                        NombreGenero = genero.NombreGenero.ToUpper(),
                        UsuarioModificacion = Convert.ToString(Session["UserName"]),
                        FechaModificacion = DateTime.Now,
                        UsuarioCreacion = Convert.ToString(Session["UserName"]),
                        FechaCreacion = DateTime.Now
                    };

                    db.Genero.Add(generoDB);
                    db.SaveChanges();

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, mensajeError = ex.Message });
                }
            }

            return Json(genero, JsonRequestBehavior.AllowGet);
        }

        // GET: /Genero/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var generoBD = db.Genero.Find(id);

            GeneroModels genero = new GeneroModels()
            {
                Controlado = generoBD.Controlado, 
                NombreGenero = generoBD.NombreGenero,
                GeneroId = generoBD.GeneroId,
                UsuarioModificacion = generoBD.UsuarioModificacion,
                FechaModificacion = generoBD.FechaModificacion,
                UsuarioCreacion = generoBD.UsuarioCreacion,
                FechaCreacion = generoBD.FechaCreacion
            };

            if (genero == null)
            {
                return HttpNotFound();
            }
            return PartialView(genero);
        }

        // POST: /Genero/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GeneroModels genero)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Genero generoDB = new Genero()
                    {
                        Controlado = genero.Controlado,
                        GeneroId = genero.GeneroId,
                        NombreGenero = genero.NombreGenero.ToUpper(),
                        UsuarioModificacion = Convert.ToString(Session["UserName"]),
                        FechaModificacion = DateTime.Now,
                        UsuarioCreacion = genero.UsuarioCreacion,
                        FechaCreacion = genero.FechaCreacion
                    };

                    db.Entry(generoDB).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, mensajeError = ex.Message });
                }
            }

            return PartialView("Edit", genero);
        }

        // GET: /Genero/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var generoBD = db.Genero.Find(id);

            GeneroModels genero = new GeneroModels()
            {
                Controlado = generoBD.Controlado,
                NombreGenero = generoBD.NombreGenero,
                GeneroId = generoBD.GeneroId
            };

            if (genero == null)
            {
                return HttpNotFound();
            }
            return PartialView(genero);
        }

        // POST: /Genero/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(GeneroModels genero)
        {
            try
            {
                Genero generoBD = db.Genero.Find(genero.GeneroId);
                db.Genero.Remove(generoBD);
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
