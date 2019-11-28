using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pharmaweb.Models
{
    public class LaboratorioModels
    {
        [Required]
        public int LaboratorioId { get; set; }

        [Required]
        [Display(Name = "Nombre Laboratorio")]
        public string NombreLaboratorio { get; set; }
        public string UsuarioModificacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public string UsuarioCreacion { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}