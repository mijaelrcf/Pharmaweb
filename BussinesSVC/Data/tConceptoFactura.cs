//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BussinesSVC.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class tConceptoFactura
    {
        public long IdConceptoFactura { get; set; }
        public long id_factura_nb { get; set; }
        public int Cantidad { get; set; }
        public string Concepto { get; set; }
        public string Unidad { get; set; }
    
        public virtual tFacturas tFacturas { get; set; }
    }
}
