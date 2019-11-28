using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pharmaweb.Models
{
    public class VentasDetalleModels
    {
        [Required]
        public Int64 VentasId { get; set; }

        [Required]
        public int ProductoId { get; set; }
        
        [Display(Name = "Nombre Producto")]
        public string NombreProducto { get; set; }

        [Required]
        [Display(Name = "Precio por Paquete")]
        public decimal PrecioVentaPaquete { get; set; }

        [Required]
        [Display(Name = "Precio por Unidad")]
        public decimal PrecioVentaUnidad { get; set; }

        [Required]
        [Display(Name = "Cantidad de Paquetes")]
        public decimal CantidadPaquete { get; set; }

        [Required]
        [Display(Name = "Cantidad de Unidades")]
        public decimal CantidadUnidad { get; set; }

        [Required]
        [Display(Name = "Subtotal")]
        public decimal Subtotal { get; set; }

        [Display(Name = "Tipo de Venta")]
        public string TipoVenta { get; set; }

        public decimal StockPaquetes { get; set; }
        public decimal StockUnidades { get; set; }
        public decimal UnidadesPorPaquete { get; set; }
        public bool EsProductoControlado { get; set; }

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