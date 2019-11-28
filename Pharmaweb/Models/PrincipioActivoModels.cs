using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pharmaweb.Models
{
    public class PrincipioActivoModels
    {
        [Required]
        public int PrincipioActivoId { get; set; }
        [Required]
        [Display(Name = "Nombre Principio Activo")]
        public string NombrePrincipioActivo { get; set; }

        public string UsuarioModificacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public string UsuarioCreacion { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}