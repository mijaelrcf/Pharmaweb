using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pharmaweb.Models
{
    public class VentasModels
    {
        public Int64 VentasId { get; set; }
        public int SucursalId { get; set; }
        public Int64 Factura { get; set; }
        public Int64 Nit { get; set; }

        [Display(Name = "Nombre Cliente")]
        public string NombreCliente { get; set; }

        [Required]
        [Display(Name = "Total Vendido")]
        public decimal TotalVenta { get; set; }

        [Required]
        [Display(Name = "Total Pagado")]
        public decimal TotalPagado { get; set; }

        [Required]
        [Display(Name = "Cambio")]
        public decimal Cambio { get; set; }

        [Display(Name = "Estado de la Venta")]
        public string CatEstadoVenta { get; set; }

        [Display(Name = "Laboratorio")]
        public int LaboratorioId { get; set; }
        //public List<SelectListItem> lstLaboratorio { get; set; }
        public string NombreLaboratorio { get; set; }

        [Display(Name = "Linea")]
        public int LineaId { get; set; }
        //public List<SelectListItem> lstLinea { get; set; }
        public string NombreLinea { get; set; }

        [Required]
        public int ProductoId { get; set; }
        //public List<SelectListItem> lstProducto { get; set; }

        [Display(Name = "Nombre Producto")]
        public string NombreProducto { get; set; }

        [Required]
        [Display(Name = "Precio por Paquete")]
        public decimal PrecioVentaPaquete { get; set; }

        [Required]
        [Display(Name = "Precio por Unidad")]
        public decimal PrecioVentaUnidad { get; set; }

        [Required]
        public decimal CantidadPaquete { get; set; }
        
        [Required]
        public decimal CantidadUnidad { get; set; }

        [Display(Name = "Fecha de Vencimiento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaVencimiento { get; set; }

        public int VencimientoEnDias { get; set; }

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