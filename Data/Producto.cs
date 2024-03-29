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
    
    public partial class Producto
    {
        public Producto()
        {
            this.IngresoProductoDetalle = new HashSet<IngresoProductoDetalle>();
            this.Inventario = new HashSet<Inventario>();
            this.PrecioProducto = new HashSet<PrecioProducto>();
            this.SalidaProducto = new HashSet<SalidaProducto>();
            this.VentasDetalle = new HashSet<VentasDetalle>();
        }
    
        public int ProductoId { get; set; }
        public int LineaId { get; set; }
        public int GeneroId { get; set; }
        public int PrincipioActivoId { get; set; }
        public string NombreProducto { get; set; }
        public string CatTipo { get; set; }
        public short UnidadesPorPaquete { get; set; }
        public string UsuarioModificacion { get; set; }
        public System.DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
    
        public virtual Genero Genero { get; set; }
        public virtual ICollection<IngresoProductoDetalle> IngresoProductoDetalle { get; set; }
        public virtual ICollection<Inventario> Inventario { get; set; }
        public virtual Linea Linea { get; set; }
        public virtual ICollection<PrecioProducto> PrecioProducto { get; set; }
        public virtual PrincipioActivo PrincipioActivo { get; set; }
        public virtual ICollection<SalidaProducto> SalidaProducto { get; set; }
        public virtual ICollection<VentasDetalle> VentasDetalle { get; set; }
    }
}
