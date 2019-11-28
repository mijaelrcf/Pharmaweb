using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharmaweb.Models
{
    public class PrecioProductoModels
    {
        [Display(Name = "Sucursal")]
        public int SucursalId { get; set; }
        public List<SelectListItem> lstSucursal { get; set; }
        public string NombreSucursal { get; set; }

        //[Display(Name = "Laboratorio")]
        public int LaboratorioId { get; set; }
        public List<SelectListItem> lstLaboratorio { get; set; }
        public string NombreLaboratorio { get; set; }

        //[Display(Name = "Linea")]
        public int LineaId { get; set; }
        public List<SelectListItem> lstLinea { get; set; }
        public string NombreLinea { get; set; }

        [Required]
        public int ProductoId { get; set; }
        public List<SelectListItem> lstProducto { get; set; }
        
        [Display(Name = "Nombre Producto")]
        public string NombreProducto { get; set; }

        [Required]
        [Display(Name = "Costo")]                
        public decimal Costo { get; set; }

        [Required]
        public int Base { get; set; }
        
        [Required]
        public int Bonificacion { get; set; }

        [Range(0, 100)]
        public decimal Descuento1 { get; set; }
        
        [Range(0, 100)]
        public decimal Descuento2 { get; set; }
        
        [Range(0, 100)]
        public decimal Descuento3 { get; set; }
        
        [Range(0, 100)]
        public decimal Descuento4 { get; set; }
        public decimal Margen { get; set; }
        public decimal CostoTotal { get; set; }
        public decimal PrecioVentaPaquete { get; set; }
        public decimal PrecioVentaUnidad { get; set; }
        public short UnidadPaquete { get; set; }
        
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