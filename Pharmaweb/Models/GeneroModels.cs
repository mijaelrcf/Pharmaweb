using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pharmaweb.Models
{
    public class GeneroModels
    {
        [Required]
        public int GeneroId { get; set; }
        
        [Required]
        [Display(Name = "Controlado")]
        public bool Controlado { get; set; }
        
        [Required]
        [Display(Name = "Nombre Genero")]
        public string NombreGenero { get; set; }
        
        public string UsuarioModificacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public string UsuarioCreacion { get; set; }
                
        public DateTime FechaCreacion { get; set; }
    }
}