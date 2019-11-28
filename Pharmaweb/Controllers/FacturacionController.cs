using BussinesSVC.Data;
using BussinesSVC.Domain;
using BussinesSVC.Manager;
using CerberusSVF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;


namespace Pharmaweb.Controllers
{
    public class FacturacionController : Controller
    {
        //
        // GET: /Facturacion/
        /// <summary>
        /// Interfaz de ingreso a datos
        /// </summary>
        /// <returns></returns>

        public ActionResult Index()
        {
            FacturaIn factura = new FacturaIn();
            FacturaManager manager = new FacturaManager();
            Session["Factura"] = factura;
            var resul = LoginAux();
            long IdEmpresa = Convert.ToInt64(((spLogin_Result)resul.Object).id_empresa);

            Session["Dosificaciones"] = manager.ObtenerDosificaciones(IdEmpresa);
            return View(factura);
        }



        [HttpPost]
        public ActionResult Impresion(FacturaIn facturaIn)
        {
            facturaIn.Conceptos = ((FacturaIn)Session["Factura"]).Conceptos;
            if (facturaIn.Conceptos == null || facturaIn.Conceptos.Count == 0)
            {
                ViewBag.Error = "Debe incluir conceptos a facturar!";
                facturaIn.Conceptos = new List<ConceptoFactura>();
                return View("Index", facturaIn);
            }

            var resul = LoginAux();

            FacturaManager mgnrFactura = new FacturaManager();
            facturaIn.Conceptos.ForEach(x => x.Talla = x.Concepto);
            facturaIn.IdDosificacion = Convert.ToInt64(Request["IdDosificacionHidden"]);
            var resul2 = mgnrFactura.GuardarFactura(facturaIn, (spLogin_Result)resul.Object);
            ViewData["Impresion"] = true;
            return View(resul2.Object);

        }

        private ResponseObject LoginAux()
        {
            /*
            LoginManager mnger = new LoginManager();
            var resul = mnger.Login("rchalco", "123");*/
            return (ResponseObject)Session["resulLogin"];
        }

        public ActionResult LibroVentas()
        {

            return View(new LibroVentasModel());
        }

        [HttpPost]
        public ActionResult LibroVentas(LibroVentasModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Error"] = "Por favor corrija los errores!";
            }

            var resul = LoginAux();
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

            var resul = LoginAux();
            long IdEmpresa = Convert.ToInt64(((spLogin_Result)resul.Object).id_empresa);
            FacturaManager mngr = new FacturaManager();

            string pathFile = Server.MapPath("~/LibroVentas") + "\\";
            var resulFile = mngr.ExportarLibroVentas(IdEmpresa, Mes, Gestion, pathFile, (spLogin_Result)resul.Object);

            return File(Encoding.UTF8.GetBytes(resulFile.Object.GetData()), "text/plain", resulFile.Object.NameFile.Split('\\')[resulFile.Object.NameFile.Split('\\').Length - 1]);
        }


        public ActionResult AdminsitrarDosificacion()
        {
            FacturaManager mngr = new FacturaManager();
            return View(mngr.ObtenerDosificaciones(1));
        }

        [ValidateInput(false)]
        public ActionResult AdminsitrarDosificacionPartial()
        {
            var resul = LoginAux();
            FacturaManager mngr = new FacturaManager();
            long IdEmpresa = Convert.ToInt64(((spLogin_Result)resul.Object).id_empresa);
            return PartialView("AdminsitrarDosificacionPartial", mngr.ObtenerDosificaciones(IdEmpresa));
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EdithDosificacionPartial(Dosificacion dosificacion)
        {
            FacturaManager mngr = new FacturaManager();
            var resul = LoginAux();
            long IdEmpresa = Convert.ToInt64(((spLogin_Result)resul.Object).id_empresa);
            mngr.GuardarDosificacion(dosificacion, (spLogin_Result)resul.Object);
            return PartialView("AdminsitrarDosificacionPartial", mngr.ObtenerDosificaciones(IdEmpresa));
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteDosificacionPartial(Dosificacion dosificacion)
        {
            FacturaManager mngr = new FacturaManager();
            var resul = LoginAux();
            mngr.EliminarDosificacion(dosificacion, (spLogin_Result)resul.Object);
            return PartialView("AdminsitrarDosificacionPartial", mngr.ObtenerDosificaciones(1));
        }


        public ActionResult AdminsitrarEmpresa()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult ConceptosPartial()
        {
            FacturaIn factura = new FacturaIn();
            if (Session["Factura"] != null)
            {
                factura = (FacturaIn)Session["Factura"];
            }
            return PartialView("ConceptosPartial", factura.Conceptos);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult AddConceptoPartial(ConceptoFactura conceptoFactura)
        {

            FacturaIn factura = new FacturaIn();
            if (Session["Factura"] != null)
            {
                factura = (FacturaIn)Session["Factura"];
            }

            if (ModelState.IsValid)
            {
                try
                {
                    conceptoFactura.IdConceptoFactura = factura.Conceptos.Count + 1;
                    conceptoFactura.Total = conceptoFactura.PrecioUnidad * conceptoFactura.Cantidad;
                    factura.Conceptos.Add(conceptoFactura);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = "Por favor corrija los errores!";
                ViewData["EditableconceptoFactura"] = conceptoFactura;
            }

            Session["Factura"] = factura;
            return PartialView("ConceptosPartial", factura.Conceptos);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteConceptoPartial(int IdConceptoFactura)
        {

            FacturaIn factura = new FacturaIn();
            if (Session["Factura"] != null)
            {
                factura = (FacturaIn)Session["Factura"];
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var Concepto = factura.Conceptos.First(x => x.IdConceptoFactura == IdConceptoFactura);
                    if (Concepto != null)
                    {
                        factura.Conceptos.Remove(Concepto);
                    }

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = "Por favor corrija los errores!";
                ViewData["EditableconceptoFactura"] = null;
            }
            Session["Factura"] = factura;
            return PartialView("ConceptosPartial", factura.Conceptos);
        }



        public ActionResult FacturasPeriodo()
        {
            FacturaPeriodoModel model = new FacturaPeriodoModel();
            Session["FacturaPeriodoModel"] = model;
            return View(model);
        }

        [HttpPost]
        public ActionResult FacturasPeriodo(FacturaPeriodoModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var resul = LoginAux();
            long IdEmpresa = Convert.ToInt64(((spLogin_Result)resul.Object).id_empresa);
            FacturaManager mngr = new FacturaManager();
            model.Items = mngr.ObtenerFacturasPeriodo(IdEmpresa, model.Mes, model.Gestion);
            Session["FacturaPeriodoModel"] = model;
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult FacturasPeriodoPartial()
        {
            var resul = LoginAux();
            long IdEmpresa = Convert.ToInt64(((spLogin_Result)resul.Object).id_empresa);
            FacturaManager mngr = new FacturaManager();
            FacturaPeriodoModel model = (FacturaPeriodoModel)Session["FacturaPeriodoModel"];
            model.Items = mngr.ObtenerFacturasPeriodo(IdEmpresa, model.Mes, model.Gestion);
            Session["FacturaPeriodoModel"] = model;
            return PartialView("FacturasPeriodoPartial", model.Items);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AnularFacturasPeriodoPartial(spConsultaFacturasPeriodo_Result factura)
        {
            var resul = LoginAux();
            long IdEmpresa = Convert.ToInt64(((spLogin_Result)resul.Object).id_empresa);
            FacturaManager mngr = new FacturaManager();
            mngr.AnularFactura(factura.idFactura);
            FacturaPeriodoModel model = (FacturaPeriodoModel)Session["FacturaPeriodoModel"];
            model.Items = mngr.ObtenerFacturasPeriodo(IdEmpresa, model.Mes, model.Gestion);
            Session["FacturaPeriodoModel"] = model;
            return PartialView("FacturasPeriodoPartial", model.Items);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial()
        {
            var model = new object[0];
            return PartialView("_GridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] System.Linq.Dynamic.ParseException item)
        {
            var model = new object[0];
            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to insert the new item in your model
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_GridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] System.Linq.Dynamic.ParseException item)
        {
            var model = new object[0];
            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to update the item in your model
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_GridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialDelete(System.Int32 Position)
        {
            var model = new object[0];
            if (Position >= 0)
            {
                try
                {
                    // Insert here a code to delete the item from your model
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_GridViewPartial", model);
        }
    }
}