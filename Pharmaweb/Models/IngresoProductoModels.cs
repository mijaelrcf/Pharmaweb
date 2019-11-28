using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharmaweb.Models
{
    public class IngresoProductoModels
    {
        [Required]
        public Int64 IngresoProductoId { get; set; }

        [Display(Name = "Sucursal")]
        public int SucursalId { get; set; }
        public List<SelectListItem> lstSucursal { get; set; }

        [Display(Name = "Nombre Sucursal")]
        public string NombreSucursal { get; set; }

        [Display(Name = "Tipo Ingreso")]
        public string CatTipoIngreso { get; set; }
        public List<SelectListItem> lstCatTipoIngreso { get; set; }

        [Required]
        //[RegularExpression("[0-9]+-[0-9kK]")]
        public string NroFactura { get; set; }

        public string Observacion { get; set; }

        [Display(Name = "Laboratorio")]
        public int LaboratorioId { get; set; }
        public List<SelectListItem> lstLaboratorio { get; set; }
        
        [Display(Name = "Nombre Laboratorio")]
        public string NombreLaboratorio { get; set; }

        [Display(Name = "Usuario Modificacion")]
        public string UsuarioModificacion { get; set; }

        [Display(Name = "Fecha Modificacion")]
        public DateTime FechaModificacion { get; set; }

        [Display(Name = "Usuario Creacion")]
        public string UsuarioCreacion { get; set; }

        [Display(Name = "Fecha Creacion")]
        public DateTime FechaCreacion { get; set; }


        [Required]
        public ICollection<IngresoProductoDetalleModels> Detalle { get; set; }

        public IngresoProductoModels()
        {
            this.Detalle = new HashSet<IngresoProductoDetalleModels>();
        }
    }
}