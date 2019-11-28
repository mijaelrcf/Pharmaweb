using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pharmaweb.Models
{
    public class IngresoProductoDetalleModels
    {
        public Int64 IngresoProductoDetalleId { get; set; }

        public Int64 IngresoProductoId { get; set; }
        public int ProductoId { get; set; }

        public string NombreProducto { get; set; }

        [Required]
        [Range(0, 9999)]
        public decimal CantidadPaquetes { get; set; }

        [Required]
        [Range(0, 9999)]
        public decimal CantidadUnidades { get; set; }
        
        
        [DisplayName("Fecha de Nacimiento"), Required(ErrorMessage = "Debe ingresar la fecha de vencimiento.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaVencimiento { get; set; }

        
        public DateTime Birthday { get; set; }

    }
}