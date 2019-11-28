using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharmaweb.Models
{
    public class ProductoModels
    {
        public int ProductoId { get; set; }

        [Display(Name = "Laboratorio")]
        public int LaboratorioId { get; set; }
        public List<SelectListItem> lstLaboratorio { get; set; }
        public string NombreLaboratorio { get; set; }

        [Display(Name = "Linea")]
        public int LineaId { get; set; }
        public List<SelectListItem> lstLinea { get; set; }
        public string NombreLinea { get; set; }

        [Display(Name = "Genero")]
        public int GeneroId { get; set; }
        public List<SelectListItem> lstGenero { get; set; }
        public string NombreGenero { get; set; }

        [Display(Name = "Principio Activo")]
        public int PrincipioActivoId { get; set; }
        public List<SelectListItem> lstPrincipioActivo { get; set; }
        public string NombrePrincipioActivo { get; set; }

        [Required]
        [Display(Name = "Tipo Venta")]
        public string CatTipo { get; set; }
        public List<SelectListItem> lstCatTipo { get; set; }

        [Required]
        [Display(Name = "Minimo")]
        public short Minimo { get; set; }
        
        [Required]
        [Display(Name = "Unidades X Paquete")]
        public short UnidadesPorPaquete { get; set; }

        [Required]
        [Display(Name = "Nombre Producto")]
        public string NombreProducto { get; set; }

        public string UsuarioModificacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public string UsuarioCreacion { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}