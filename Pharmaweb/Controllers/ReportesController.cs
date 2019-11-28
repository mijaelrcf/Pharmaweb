using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using DevExpress.XtraReports.UI;
using System.Drawing;
using Pharmaweb.Models;

namespace Pharmaweb.Controllers
{
    public class ReportesController : Controller
    {
        //
        // GET: /Reportes/
        public ActionResult Index()
        {
            
            ViewData["Options"] = new CheckListDemoOptions();
            return View();

            //return View();
        }

        [HttpPost]
        public ActionResult Index([Bind]CheckBoxListExtension options)
        {
            //var algo = DevExpress.RadioButtonListExtension.GetValue("RadioButtonList1");

            return View();
        }
        //XtraReport3 report = new XtraReport3();

        //public ActionResult DocumentViewerPartial()
        //{
        //    List<Data.ReporteCierreCaja_Result> lista = null;
        //    using (Data.PharmawebEntities contexto = new Data.PharmawebEntities())
        //    {
        //        //DateTime fecha = Convert.ToDateTime(report.Parameters["parameter1"].Value);

        //        DateTime fecha = Convert.ToDateTime("2017-03-31 19:59:24.533");
        //        lista = contexto.ReporteCierreCaja(fecha).ToList();
        //        report.DataSource = lista;
        //        report.FillDataSource();
        //    }
            
        //    return PartialView("_DocumentViewerPartial", report);
        //}

        //public ActionResult DocumentViewerPartialExport()
        //{
        //    return DocumentViewerExtension.ExportTo(report, Request);
        //}

        XtraReport1 report = new XtraReport1();

        public ActionResult DocumentViewerPartial()
        {
            
            return PartialView("_DocumentViewerPartial", report);
        }

        public ActionResult DocumentViewerPartialExport()
        {
            return DocumentViewerExtension.ExportTo(report, Request);
        }

        //public WinControlContainer CreateWinControl()
        //{
        //    // Create a GroupBox object.
        //    GroupBox groupBox = new GroupBox();

        //    // Set its text.
        //    groupBox.Text = "Windows.Forms Group Box";

        //    // Create a WinControlContainer object.
        //    WinControlContainer winContainer = new WinControlContainer();

        //    // Set its location and size.
        //    winContainer.LocationF = new PointF(250F, 40F);
        //    winContainer.SizeF = new SizeF(200F, 100F);

        //    // Set the groupBox as a wrapped object.
        //    winContainer.WinControl = groupBox;

        //    return winContainer;
        //}
	}
}