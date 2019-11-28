using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pharmaweb.Models
{
    public class AsignaRolesUsuarioModels
    {
        public string IdUsuario { get; set; }
        public List<AsignaRolesUsuario> RolesUsuarioList { get; set; }
    }
    public class AsignaRolesUsuario
    {
        public string IdRol { get; set; }
        public string Rol { get; set; }
        public bool Asignado { get; set; }        
    }
}