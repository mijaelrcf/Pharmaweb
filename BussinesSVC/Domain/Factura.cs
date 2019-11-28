using Foundation.Stone.CrossCuting.Formater;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesSVC.Domain
{
    [Serializable]
    public class Factura
    {
        public Factura()
        {
            Conceptos = new List<ConceptoFactura>();
        }

        [Required]
        [Display(Name = "Nombre de la Empresa")]
        public string NombreEmpresa { get; set; }

        [Required]
        [Display(Name = "Nombre de la Sucrusal")]
        public string NombreSucursal { get; set; }

        [Required]
        [Display(Name = "Direccion")]
        public string Direccion { get; set; }

        [Required]
        [Display(Name = "Zona")]
        public string Zona { get; set; }

        [Required]
        [Display(Name = "Telefono")]
        public string Telefono { get; set; }

        [Required]
        [Display(Name = "Ciudad")]
        public string Ciudad { get; set; }

        [Required]
        [Display(Name = "NIT")]
        public string NIT { get; set; }

        [Required]
        [Display(Name = "Nro de Factura")]
        public string NroFactura { get; set; }

        [Required]
        [Display(Name = "Nro de Autorización")]
        public string NroAutorizacion { get; set; }

        [Required]
        [Display(Name = "Monto")]
        [DisplayFormat(DataFormatString = "{0:###,###.00}", ApplyFormatInEditMode = true)]
        public decimal MontoFactura { get; set; }

        [Required]
        [Display(Name = "Descuento")]
        [DisplayFormat(DataFormatString = "{0:###,###.00}", ApplyFormatInEditMode = true)]
        public decimal Descuento { get; set; }


        [Required]
        [Display(Name = "Total")]
        [DisplayFormat(DataFormatString = "{0:###,###.00}", ApplyFormatInEditMode = true)]
        public decimal Total { get; set; }

        [Required]
        [Display(Name = "Literal")]
        public string MontoLiteral
        {
            get
            {
                int total = HelperFormaterES.ConvertNumberLiteral(Total.ToString(), true).Split(' ').Count();
                int decimales = Convert.ToInt32(HelperFormaterES.ConvertNumberLiteral(Total.ToString(), true).Split(' ')[total - 1]);
                string cero = "0";
                if (decimales > 10)
                {
                    cero = string.Empty;
                }
                return (HelperFormaterES.ConvertNumberLiteral(Total.ToString(), true) + cero + "/100 Bs.").Replace("000/100", "00/100");
            }
        }

        [Required]
        [Display(Name = "Fecha")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaFactura { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string NombreCliente { get; set; }

        [Required]
        [Display(Name = "NIT/CI")]
        public string NitCliente { get; set; }

        [Required]
        [Display(Name = "Código de Control")]
        public string CodigoControl { get; set; }

        [Required]
        [Display(Name = "FECHA LIMITE DE EMISIÓN")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaLimite { get; set; }

        [Required]
        [Display(Name = "Actividad")]
        public string Actividad { get; set; }


        [Required]
        [Display(Name = "Sucursal")]
        public string Sucursal { get; set; }

        [Required]
        [Display(Name = "Direccion Sucursal")]
        public string DireccionSucursal { get; set; }

        [Required]
        [Display(Name = "Teléfono")]
        public string TelefonoSucursal { get; set; }

        public byte[] QR { get; set; }

        public List<ConceptoFactura> Conceptos { get; set; }


        [Display(Name = "Leyenda")]
        public string Leyenda { get; set; }
    }
}
