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
    
    public partial class tUsuarios
    {
        public long id_usuario_nb { get; set; }
        public Nullable<int> id_rol_nb { get; set; }
        public string documentoDeIdentidad { get; set; }
        public string nombresVC { get; set; }
        public string apellidoPaternoVC { get; set; }
        public string apellidoMaternoVC { get; set; }
        public string usuario_vc { get; set; }
        public string password_vc { get; set; }
        public string telefonoFijoVC { get; set; }
        public string telefonoCelularVC { get; set; }
        public System.DateTime fecharVigenciaDesdeDT { get; set; }
        public Nullable<System.DateTime> fecharVigenciaHastaDT { get; set; }
        public Nullable<long> id_empresa_nb { get; set; }
    
        public virtual tRoles tRoles { get; set; }
    }
}
