using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesSVC.Domain
{
    public class Dosificacion
    {
        
        [Display(Name = "Id")]
        public long id_dosificacion_nb { get; set; }
        
        [Display(Name = "Empresa")]
        public Nullable<long> id_empresa_nb { get; set; }
        [Required]
        [Display(Name = "Nro de Autorizacion")]
        public string numeroAutorizacionVC { get; set; }
        [Required]
        [Display(Name = "Llave")]
        public string llaveDosificacionVC { get; set; }
        [Required]
        [Display(Name = "Fecha Limite")]
        public Nullable<System.DateTime> fechaLimiteEmisionDT { get; set; }

        [Required]
        [Display(Name = "Activa")]
        public Nullable<bool> dosificacionActiva { get; set; }
        
        [Display(Name = "Nro Factura")]
        public long numeroDefactura { get; set; }
        
        [Display(Name = "Fecha Desde")]
        public System.DateTime fechaDesdeDT { get; set; }
        
        [Display(Name = "Fecha Fin")]
        public Nullable<System.DateTime> fechaHastaDT { get; set; }
        
        [Required]
        [Display(Name = "Leyenda Fija")]
        public string leyendaFijaVC { get; set; }
        
        [Required]
        [Display(Name = "Leyenda Variable")]
        public string leyendaVariableVC { get; set; }

        public string actividadVC { get; set; }

    }
}
