using BussinesSVC.Data;
using BussinesSVC.Domain;
using BussinesSVC.Manager;
using BussinesSVC.Manger;
using CerberusSVF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Pharmaweb.Controllers
{
    public class ReporteLibroVentasController : Controller
    {
        //
        // GET: /ReporteLibroDeVentas/
        public ActionResult Index()
        {
            return View(new LibroVentasModel());
        }

        [HttpPost]
        public ActionResult Index(LibroVentasModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Error"] = "Por favor corrija los errores!";
            }

            var resul = LoginAux("rodrigo.alegre", "123");
            FacturaManager mngr = new FacturaManager();

            long IdEmpresa = Convert.ToInt64(((spLogin_Result)resul.Object).id_empresa);
            model.Items = mngr.ObtenerLibroVentas(IdEmpresa, model.Mes, model.Gestion);

            return View(model);
        }


        [HttpPost]
        public ActionResult ExportarVentas(int Mes, int Gestion)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Error"] = "Por favor corrija los errores!";
            }

            var resul = LoginAux("rodrigo.alegre", "123");
            long IdEmpresa = Convert.ToInt64(((spLogin_Result)resul.Object).id_empresa);
            FacturaManager mngr = new FacturaManager();

            string pathFile = Server.MapPath("~/LibroVentas") + "\\";
            var resulFile = mngr.ExportarLibroVentas(IdEmpresa, Mes, Gestion, pathFile, (spLogin_Result)resul.Object);

            return File(Encoding.UTF8.GetBytes(resulFile.Object.GetData()), "text/plain", resulFile.Object.NameFile.Split('\\')[resulFile.Object.NameFile.Split('\\').Length - 1]);
        }

        private ResponseObject LoginAux(string usuario, string password)
        {
            LoginManager lgnMgr = new LoginManager();
            return (ResponseObject)lgnMgr.Login(usuario, password);
        }
	}
}