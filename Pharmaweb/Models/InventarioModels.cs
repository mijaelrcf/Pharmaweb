using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pharmaweb.Models
{
    public class InventarioModels
    {
        public int SucursalId { get; set; }
        
        [Display(Name = "Nombre Sucursal")]
        public string NombreSucursal { get; set; }
        public int ProductoId { get; set; }
        
        [Display(Name = "Nombre Producto")]
        public string NombreProducto { get; set; }

        [Display(Name = "Nombre Linea")]
        public string NombreLinea { get; set; }

        [Display(Name = "Nombre Laboratorio")]
        public string NombreLaboratorio { get; set; }
        public decimal CantIngresoPaquete { get; set; }
        public decimal CantIngresoUnidad { get; set; }
        public decimal CantSalidaPaquete { get; set; }
        public decimal CantSalidaUnidad { get; set; }
        public decimal CantidadPaqueteTotal { get; set; }
        public decimal CantidadUnidadTotal { get; set; }

        [Display(Name = "Usuario Modificacion")]
        public string UsuarioModificacion { get; set; }

        [Display(Name = "Fecha Modificacion")]
        public DateTime FechaModificacion { get; set; }

        [Display(Name = "Usuario Creacion")]
        public string UsuarioCreacion { get; set; }

        [Display(Name = "Fecha Modificacion")]
        public DateTime FechaCreacion { get; set; }

    }
}