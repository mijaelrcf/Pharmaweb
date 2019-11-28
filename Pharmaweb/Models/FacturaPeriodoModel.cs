using BussinesSVC.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CerberusSVF.Models
{
    public class FacturaPeriodoModel
    {
        public FacturaPeriodoModel()
        {
            Items = new List<spConsultaFacturasPeriodo_Result>();
        }

        [Required]
        [Range(1, 12)]
        public int Mes { get; set; }
        [Required]
        [Range(2015, 2030)]
        public int Gestion { get; set; }

        public List<spConsultaFacturasPeriodo_Result> Items { get; set; }
    }
}