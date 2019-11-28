using BussinesSVC.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BussinesSVC.Domain
{
    [DataContract]
    [Serializable]
    public class FacturaIn
    {
        public FacturaIn()
        {
            Conceptos = new List<ConceptoFactura>();
            FechaFactura = DateTime.Now;
        }

        [Required]
        [Display(Name = "Fecha")]
        [DataMember]
        public DateTime FechaFactura { get; set; }

        [Required]
        [Display(Name = "Monto")]
        [DataMember]
        public decimal MontoFactura { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]

        [Display(Name = "Nombre")]
        [DataMember]
        public string NombreCliente { get; set; }

        [Required]
        [Display(Name = "NIT/CI")]
        [DataMember]
        public long NitCliente { get; set; }

        [Required]
        [Display(Name = "Descuento")]
        [DataMember]
        public decimal Descuento { get; set; }

        [Required]
        [DataMember]
        public long IdDosificacion { get; set; }

        [DataMember]
        public List<ConceptoFactura> Conceptos { get; set; }
    }
}
