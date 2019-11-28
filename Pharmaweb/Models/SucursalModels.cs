using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharmaweb.Models
{
    public class SucursalModels
    {
        [Required]
        public int SucursalId { get; set; }
        
        [Required]
        [Display(Name = "Nombre Sucursal")]
        public string NombreSucursal { get; set; }
        
        [Required]
        [Display(Name = "Departamento")]
        public string CatDepartamento { get; set; }

        public List<SelectListItem> lstCatDepartamento { get; set; }
        
        [Required]
        [Display(Name = "Direccion")]
        public string Direccion { get; set; }
        
        [Required]
        [Display(Name = "Telefono")]
        public string Telefono { get; set; }
        
        
        [Display(Name = "Fax")]
        public string Fax { get; set; }

        [Display(Name = "Usuario Modificacion")]
        public string UsuarioModificacion { get; set; }

        [Display(Name = "Fecha Modificacion")]
        public DateTime FechaModificacion { get; set; }

        [Display(Name = "Usuario Creacion")]
        public string UsuarioCreacion { get; set; }

        [Display(Name = "Fecha Creacion")]
        public DateTime FechaCreacion { get; set; }

    }
}