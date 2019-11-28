using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace Pharmaweb.Controllers
{
    public class ReporteVencimientosController : Controller
    {
        //
        // GET: /ReporteVencimientos/
        public ActionResult Index()
        {
            return View();
        }

        XtraReport2 report = new XtraReport2();

        public ActionResult DocumentViewer1Partial()
        {
            return PartialView("_DocumentViewer1Partial", report);
        }

        public ActionResult DocumentViewer1PartialExport()
        {
            return DocumentViewerExtension.ExportTo(report, Request);
        }
	}
}