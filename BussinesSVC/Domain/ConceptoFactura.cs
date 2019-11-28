using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BussinesSVC.Domain
{
    [Serializable]
    [DataContract]
    public class ConceptoFactura
    {
        [Display(Name = "Nro")]
        [DataMember]
        public int IdConceptoFactura { get; set; }

        [Required]
        [DataMember]
        public int Cantidad { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
        [DataMember]
        public string Concepto { get; set; }

        [DataMember]
        public string Talla { get; set; }

        [Required]
        [Range(1, Double.MaxValue)]
        [Display(Name = "Precio Unidad")]
        [DataMember]
        public decimal PrecioUnidad { get; set; }

        [DataMember]
        public decimal Total { get; set; }
    }
}
