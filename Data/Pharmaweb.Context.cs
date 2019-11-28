﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class PharmawebEntities : DbContext
    {
        public PharmawebEntities()
            : base("name=PharmawebEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Catalogo> Catalogo { get; set; }
        public virtual DbSet<CatalogoDetalle> CatalogoDetalle { get; set; }
        public virtual DbSet<Genero> Genero { get; set; }
        public virtual DbSet<IngresoProducto> IngresoProducto { get; set; }
        public virtual DbSet<IngresoProductoDetalle> IngresoProductoDetalle { get; set; }
        public virtual DbSet<Inventario> Inventario { get; set; }
        public virtual DbSet<Laboratorio> Laboratorio { get; set; }
        public virtual DbSet<Linea> Linea { get; set; }
        public virtual DbSet<PrecioProducto> PrecioProducto { get; set; }
        public virtual DbSet<PrincipioActivo> PrincipioActivo { get; set; }
        public virtual DbSet<Producto> Producto { get; set; }
        public virtual DbSet<SalidaProducto> SalidaProducto { get; set; }
        public virtual DbSet<Sucursal> Sucursal { get; set; }
        public virtual DbSet<Ventas> Ventas { get; set; }
        public virtual DbSet<VentasDetalle> VentasDetalle { get; set; }
    
        public virtual ObjectResult<ReporteVencimientos_Result> ReporteVencimientos(string anio, string mes)
        {
            var anioParameter = anio != null ?
                new ObjectParameter("anio", anio) :
                new ObjectParameter("anio", typeof(string));
    
            var mesParameter = mes != null ?
                new ObjectParameter("mes", mes) :
                new ObjectParameter("mes", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ReporteVencimientos_Result>("ReporteVencimientos", anioParameter, mesParameter);
        }
    
        public virtual ObjectResult<ReporteCierreCaja_Result1> ReporteCierreCaja(Nullable<System.DateTime> fechaInicio, Nullable<System.DateTime> fechaFin)
        {
            var fechaInicioParameter = fechaInicio.HasValue ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ReporteCierreCaja_Result1>("ReporteCierreCaja", fechaInicioParameter, fechaFinParameter);
        }
    }
}