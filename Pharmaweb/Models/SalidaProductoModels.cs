using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharmaweb.Models
{
    public class SalidaProductoModels
    {
        public Int64 SalidaProductoId { get; set; }

        [Display(Name = "Sucursal")]
        public int SucursalId { get; set; }
        public List<SelectListItem> lstSucursal { get; set; }
        public string NombreSucursal { get; set; }

        [Display(Name = "Laboratorio")]
        public int LaboratorioId { get; set; }
        public List<SelectListItem> lstLaboratorio { get; set; }
        public string NombreLaboratorio { get; set; }

        [Display(Name = "Linea")]
        public int LineaId { get; set; }
        public List<SelectListItem> lstLinea { get; set; }
        public string NombreLinea { get; set; }
        
        [Required]
        [Display(Name = "Producto")]
        public int ProductoId { get; set; }
        public List<SelectListItem> lstProducto { get; set; }

        [Display(Name = "Nombre Producto")]
        public string NombreProducto { get; set; }

        [Display(Name = "Tipo Salida")]
        public string CatTipoSalida { get; set; }
        public List<SelectListItem> lstCatTipoSalida { get; set; }

        [Required]
        [Range(0, 9999)]
        public decimal CantidadPaquetes { get; set; }

        [Required]
        [Range(0, 9999)]
        public decimal CantidadUnidades { get; set; }

        [Display(Name = "Usuario Modificacion")]
        public string UsuarioModificacion { get; set; }

        [Display(Name = "Fecha Modificacion")]
        public DateTime FechaModificacion { get; set; }

        [Display(Name = "Usuario Creacion")]
        public string UsuarioCreacion { get; set; }

        [Display(Name = "Fecha Creacion")]
        public DateTime FechaCreacion { get; set; }
    }
}