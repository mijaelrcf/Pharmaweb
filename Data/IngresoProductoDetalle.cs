//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class IngresoProductoDetalle
    {
        public long IngresoProductoDetalleId { get; set; }
        public long IngresoProductoId { get; set; }
        public int ProductoId { get; set; }
        public decimal CantidadPaquetes { get; set; }
        public decimal CantidadUnidades { get; set; }
        public System.DateTime FechaVencimiento { get; set; }
    
        public virtual IngresoProducto IngresoProducto { get; set; }
        public virtual Producto Producto { get; set; }
    }
}
