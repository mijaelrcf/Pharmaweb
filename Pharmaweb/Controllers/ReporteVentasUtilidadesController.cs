using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace Pharmaweb.Controllers
{
    public class ReporteVentasUtilidadesController : Controller
    {
        //
        // GET: /ReporteVentasUtilidades/
        public ActionResult Index()
        {
            return View();
        }

        XtraReport1 report = new XtraReport1();

        public ActionResult DocumentViewerPartial()
        {
            return PartialView("_DocumentViewerPartial", report);
        }

        public ActionResult DocumentViewerPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report, Request);
        }
	}
}