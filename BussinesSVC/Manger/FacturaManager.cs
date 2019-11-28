using BussinesSVC.Data;
using BussinesSVC.Domain;
using BussinesSVC.Util;
using Foundation.Stone.CrossCuting.File;
using Foundation.Stone.CrossCuting.Util;
using MessagingToolkit.QRCode.Codec;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesSVC.Manager
{
    public class FacturaManager
    {
        BD_CerberusSVFEntities contexto = new BD_CerberusSVFEntities();

        public byte[] GenerarQR(Factura factura, spLogin_Result usuarioContext)
        {
            byte[] resul = new byte[0];
            tEmpresa empresa = contexto.tEmpresa.First(x => x.id_empresa_nb == usuarioContext.id_empresa);
            factura.MontoFactura = decimal.Round(factura.Conceptos.Sum(x => x.Total), 2);

            string cadena = empresa.NitEmpresaVc + "|" +
                factura.NroFactura + "|" +
                factura.NroAutorizacion + "|" +
                factura.FechaFactura.ToString("dd/MM/yyyy") + "|" +
                factura.MontoFactura.ToString() + "|" +
                factura.MontoFactura.ToString() + "|" +
                factura.CodigoControl + "|" +
                factura.NitCliente + "|" +
                "0|0|0|0";

            QRCodeEncoder qe = new MessagingToolkit.QRCode.Codec.QRCodeEncoder();
            qe.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qe.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
            qe.QRCodeScale = 3;

            qe.QRCodeVersion = Convert.ToInt32("0");
            Bitmap bm = qe.Encode(cadena);

            ImageConverter converter = new ImageConverter();

            return (byte[])converter.ConvertTo(bm, typeof(byte[]));

        }

        public ResponseObject<Factura> GuardarFactura(FacturaIn factura, spLogin_Result usuarioContext)
        {
            ResponseObject<Factura> resul = new ResponseObject<Factura>();

            try
            {

                //TODO: datos de dosificacion
                List<spConsultaDosificacion_Result> resulDosificacion = contexto.spConsultaDosificacion(usuarioContext.id_empresa).ToList();
                Int64 idEmpresa = Convert.ToInt64(usuarioContext.id_empresa);
                tEmpresa empresa = contexto.tEmpresa.First(x => x.id_empresa_nb == idEmpresa);
                CodigoControl codigoControl = new CodigoControl();
                decimal totalFacturado = decimal.Round(factura.Conceptos.Sum(x => x.Total), 0);

                string codigoControlStr = codigoControl.generarCodigo(resulDosificacion[0].numeroAutorizacionVC, (resulDosificacion[0].numeroDefactura + 1).ToString(), factura.NitCliente.ToString(), factura.FechaFactura.ToString("yyyyMMdd"), ((Int64)totalFacturado).ToString(), resulDosificacion[0].llaveDosificacionVC);

                tFacturas facturaBD = new tFacturas()
                {
                    id_empresa_nb = Convert.ToInt64(usuarioContext.id_empresa),
                    nitNB = factura.NitCliente,
                    razonSocialVC = factura.NombreCliente,
                    numeroDeFacturaNB = resulDosificacion[0].numeroDefactura + 1,
                    numeroDeAutorizacion = resulDosificacion[0].numeroAutorizacionVC,
                    fechaVC = DateTime.Now.ToString("dd/MM/yyyy"),
                    importeTotalFacturaDC = factura.Conceptos.Sum(x => x.Total),
                    importeIceDC = 0,
                    importeExcentoDC = 0,
                    importeSujetoADebitoFiscalDC = factura.Conceptos.Sum(x => x.Total),
                    debitoFiscal = factura.Conceptos.Sum(x => x.Total) * 0.13M,
                    id_estado_vc = "V",
                    mesCerradoNB = 0,
                    motivoDeAnulacionVC = string.Empty,
                    usuarioVC = usuarioContext.usuario_vc,
                    codigoControlVC = codigoControlStr,
                    fechaRegistroDT = DateTime.Now,
                    descuentoDC = factura.Descuento,
                    id_dosificacion_nb = resulDosificacion[0].id_dosificacion_nb
                };

                contexto.tFacturas.Add(facturaBD);
                factura.Conceptos.ForEach(x =>
                {
                    tConceptoFactura concepto = new tConceptoFactura() { Cantidad = x.Cantidad, Concepto = x.Concepto, id_factura_nb = facturaBD.id_factura_nb, Unidad = x.Talla };
                    facturaBD.tConceptoFactura.Add(concepto);
                });

                long IdDosificacion = resulDosificacion[0].id_dosificacion_nb;
                //var dosificacion = contexto.tDosificaciones.First(x => x.id_dosificacion_nb == IdDosificacion);
                var dosificacion = contexto.tDosificaciones.First(x => x.id_dosificacion_nb == factura.IdDosificacion);
                dosificacion.numeroDefactura += 1;
                contexto.SaveChanges();


                resul.Object = new Factura()
                {
                    Ciudad = empresa.Ciudad,
                    CodigoControl = codigoControlStr,
                    Direccion = empresa.Direccion,
                    FechaFactura = factura.FechaFactura,
                    FechaLimite = Convert.ToDateTime(dosificacion.fechaLimiteEmisionDT),
                    MontoFactura = factura.Conceptos.Sum(x => x.Total),
                    Descuento = factura.Descuento,
                    Total = factura.Conceptos.Sum(x => x.Total) - factura.Descuento,
                    NIT = empresa.NitEmpresaVc,
                    NitCliente = Convert.ToString(factura.NitCliente),
                    NombreCliente = factura.NombreCliente,
                    NombreEmpresa = empresa.razonSocialVC,
                    NombreSucursal = empresa.NombreSucursal,
                    NroAutorizacion = dosificacion.numeroAutorizacionVC,
                    NroFactura = Convert.ToString(dosificacion.numeroDefactura),
                    Telefono = empresa.Telefono,
                    Zona = empresa.Zona,
                    Actividad = dosificacion.actividadVC,
                    Leyenda = dosificacion.leyendaFijaVC
                };

                tEmpresa empresaPadre = null;

                if (empresa.idPadre_nb != null && empresa.idPadre_nb != 0)
                {
                    empresaPadre = contexto.tEmpresa.First(x => x.id_empresa_nb == empresa.idPadre_nb);
                }

                if (empresaPadre != null)
                {
                    resul.Object.Direccion = empresaPadre.Direccion;
                    resul.Object.Telefono = empresaPadre.Telefono;
                    resul.Object.NombreSucursal = empresaPadre.NombreSucursal;

                    resul.Object.Sucursal = empresa.NombreSucursal;
                    resul.Object.DireccionSucursal = empresa.Direccion;
                    resul.Object.TelefonoSucursal = empresaPadre.Telefono;
                }

                factura.Conceptos.ForEach(x => resul.Object.Conceptos.Add(x));
                resul.Object.QR = GenerarQR(resul.Object, usuarioContext);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return resul;
        }

        public List<spGeneraLibroVentas_Result> ObtenerLibroVentas(long IdEmpresa, int mes, int gestion)
        {
            var LibroVentas = contexto.spGeneraLibroVentas(IdEmpresa, mes, gestion).ToList();
            return LibroVentas;
        }

        public ResponseObject<FileUtil> ExportarLibroVentas(long IdEmpresa, int mes, int gestion, string pathFile, spLogin_Result usuarioContext)
        {

            ResponseObject<FileUtil> resul = new ResponseObject<FileUtil>();
            var LibroVentas = contexto.spGeneraLibroVentas(IdEmpresa, mes, gestion).ToList();
            string data = string.Empty;
            LibroVentas.ForEach(x =>
            {
                data += x.NIT + "|";
                data += x.RAZONSOCIAL + "|";
                data += x.NUMEROFACTURA + "|";
                data += x.NUMEROAUTORIZACION + "|";
                data += x.FECHA + "|";
                data += x.IMPORTETOTALFACTURA + "|";
                data += x.IMPORTE_ICE + "|";
                data += x.IMPORTE_EXCENTO + "|";
                data += x.IMPORTE_SUJETO_A_DEB_FISCAL + "|";
                data += x.DEBITOFISCAL + "|";
                data += x.ESTADO + "|";
                data += x.CODIGO_DE_CONTROL + "\r\n";
            });

            tEmpresa empresa = contexto.tEmpresa.First(x => x.id_empresa_nb == usuarioContext.id_empresa);
            string nameFile = "Ventas_" + mes.ToString().PadLeft(2, '0') + gestion.ToString() + "_" + empresa.NitEmpresaVc + ".txt";
            if (File.Exists(pathFile + "\\" + nameFile))
            {
                File.Delete(pathFile + "\\" + nameFile);
            }

            FileUtil file = new FileUtil(pathFile + "\\" + nameFile);

            resul.Object = file;
            file.writeFile(data);
            file.closeFile();

            contexto.spCierraPeriodo(usuarioContext.id_empresa, mes, gestion);

            return resul;
        }

        public List<Dosificacion> ObtenerDosificaciones(long IdEmpresa)
        {
            List<tDosificaciones> dosificaciones = contexto.tDosificaciones.Where(x => x.id_empresa_nb == IdEmpresa && x.dosificacionActiva == true).ToList();
            return MapStone.Map<tDosificaciones, Dosificacion>(dosificaciones);
        }

        public ResponseOperation GuardarDosificacion(Dosificacion dosificacion, spLogin_Result usuarioContext)
        {
            ResponseOperation resul = new ResponseOperation();

            try
            {
                ///TODO nueva dosificacion o ya existe
                if (!contexto.tDosificaciones.Any(x => x.id_dosificacion_nb == dosificacion.id_dosificacion_nb))
                {
                    contexto.tDosificaciones.Where(x => x.id_empresa_nb == usuarioContext.id_empresa).ToList().ForEach(

                        x =>
                        {
                            var doficiaconOld = contexto.tDosificaciones.First(y => x.id_dosificacion_nb == y.id_dosificacion_nb);
                            doficiaconOld.dosificacionActiva = false;
                            doficiaconOld.usuarioVC = usuarioContext.usuario_vc;
                            doficiaconOld.fechaDeModificacion = DateTime.Now;
                            doficiaconOld.fechaHastaDT = DateTime.Now;
                        }
                        );
                    tDosificaciones newDosificacion = new tDosificaciones();
                    newDosificacion = MapStone.Map<Dosificacion, tDosificaciones>(dosificacion, newDosificacion);
                    newDosificacion.fechaDesdeDT = DateTime.Now;
                    newDosificacion.fechaHastaDT = null;
                    newDosificacion.id_empresa_nb = usuarioContext.id_empresa;
                    newDosificacion.dosificacionActiva = true;
                    newDosificacion.usuarioVC = usuarioContext.usuario_vc;
                    newDosificacion.fechaDeModificacion = DateTime.Now;
                    contexto.tDosificaciones.Add(newDosificacion);

                }
                else
                {
                    var resulDosificacion = contexto.tDosificaciones.First(x => x.id_dosificacion_nb == dosificacion.id_dosificacion_nb);
                    DateTime dt = resulDosificacion.fechaDesdeDT;
                    long idEmpresa = Convert.ToInt64(resulDosificacion.id_empresa_nb);
                    resulDosificacion = MapStone.Map<Dosificacion, tDosificaciones>(dosificacion, resulDosificacion);
                    resulDosificacion.fechaDesdeDT = dt;
                    resulDosificacion.id_empresa_nb = idEmpresa;
                }

                contexto.SaveChanges();
                resul.State = ResponseType.Success;
                resul.Message = "Resgistro exitoso";

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return resul;
        }


        public ResponseOperation EliminarDosificacion(Dosificacion dosificacion, spLogin_Result usuarioContext)
        {
            ResponseOperation resul = new ResponseOperation();

            try
            {
                ///TODO nueva dosificacion o ya existe
                if (contexto.tDosificaciones.Any(x => x.id_dosificacion_nb == dosificacion.id_dosificacion_nb))
                {

                    var resulDosificacion = contexto.tDosificaciones.First(x => x.id_dosificacion_nb == dosificacion.id_dosificacion_nb);
                    contexto.tDosificaciones.Remove(resulDosificacion);

                }
                contexto.SaveChanges();
                resul.State = ResponseType.Success;
                resul.Message = "Eliminacion exitosa";

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return resul;
        }

        public List<spConsultaFacturasPeriodo_Result> ObtenerFacturasPeriodo(long IdEmpresa, int mes, int gestion)
        {
            List<spConsultaFacturasPeriodo_Result> facturas = contexto.spConsultaFacturasPeriodo(IdEmpresa, mes, gestion).ToList();
            return facturas;
        }


        public Response AnularFactura(long IdFactura)
        {
            Response resul = new Response();
            resul.Message = contexto.spAnulaFactura(IdFactura).First();
            return resul;
        }



    }
}
