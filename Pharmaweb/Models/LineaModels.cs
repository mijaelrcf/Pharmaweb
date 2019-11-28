using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharmaweb.Models
{
    public class LineaModels
    {
        [Required]
        public int LineaId { get; set; }

        [Required]
        [Display(Name = "Nombre Linea")]
        public string NombreLinea { get; set; }

        [Required]
        [Display(Name = "Laboratorio")]
        public int LaboratorioId { get; set; }

        public List<SelectListItem> lstLaboratorio { get; set; }

        [Display(Name = "Nombre Laboratorio")]
        public string NombreLaboratorio { get; set; }

        [Required]
        [Display(Name = "Procedencia")]
        public string CatProcedencia { get; set; }

        public List<SelectListItem> lstCatProcedencia { get; set; }

        [Required]
        [Range(0, 100)]
        [Display(Name = "Margen de Ganancia")]
        public decimal Margen { get; set; }

        public string UsuarioModificacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public string UsuarioCreacion { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}