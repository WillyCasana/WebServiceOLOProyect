using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.IO;
using System.Web;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;

namespace WcfServiceOLO
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Billing" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Billing.svc o Billing.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class Billing : IBilling
    {
        public void DoWork()
        {
        }


        public string GetData(int value)
        {
            return "You entered:"+ value.ToString();
        }
        
        public DetalleEnvio AddBilling(AuxBilling bill)
        {
            DetalleEnvio detalle = new DetalleEnvio();
            string nombreFichero = "";
            string respuesta = "";
            try
            {
                respuesta = bill.AddBill(bill);
            }
            catch (Exception ex)
            {
                detalle.Code = 406;
                detalle.Titulo = "ERROR";
                detalle.Mensaje = "ERROR EN RESPUESTA:" + ex.Message.ToString();
                nombreFichero = "KO_";                
            }


            
            if (respuesta == "OK")
            { 
            detalle.Code = 200;
            detalle.Titulo = "ENVIO OK";
            detalle.Mensaje = "ENVIO REALIZADO CON ÉXITO";
            nombreFichero = "OK_";
            }
            else
            {                                    
            detalle.Code = 406;
            detalle.Titulo = "ERROR EN PROCESO";
            detalle.Mensaje = respuesta;
            nombreFichero = "KO_";
            }

            try
            {
                StringWriter sw1 = new StringWriter();
                XmlSerializer xs1 = new XmlSerializer(typeof(AuxBilling));
                xs1.Serialize(new System.Xml.XmlTextWriter(sw1), bill);
                using (StreamWriter sw = File.CreateText(HttpContext.Current.Server.MapPath("errores/xml/") + nombreFichero + System.Guid.NewGuid().ToString()))
                {
                    sw.WriteLine(sw1.ToString());
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                detalle.Code = 406;
                detalle.Titulo = "ERROR";
                detalle.Mensaje = "ERROR EN FICHERO:" + ex.Message.ToString();
                nombreFichero = "KO_";
            }
            return detalle;
        }

    }

    public class CustomValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            if (userName == "olows" && password == "salesland2014")
                return;

            using (StreamWriter sw = File.CreateText(HttpContext.Current.Server.MapPath("errores/login/") + "errorLOGIN_" + System.Guid.NewGuid().ToString()))
            {
                sw.WriteLine("ERROR LOGIN:");
                sw.WriteLine("Usuario:" + userName + " Password:" + password);
                sw.Close();
            }

            throw new SecurityTokenException("Unknown Username or Password");
        }
    }

    public class DetalleEnvio
    {
        public int Code;
        public string Titulo;
        public string Mensaje;    
    }
    
    //public class AuxBilling
    //{
    //    public string IDOLOPV;
    //    public string IDOLOUsuario;
    //    public string FechaVenta;
    //    public int IDCliente;
    //    public int IDTipoDocumento;
    //    public string NumDocumentoCliente;
    //    public string NombreCliente;
    //    public string FNacCliente;
    //    public int? IDGeneroCliente;
    //    public string OcupacionCliente;
    //    public string NacionalidadCliente;
    //    public string DireccionCliente;
    //    public int IDProvincia;
    //    public string ProvinciaCliente;
    //    public int? IDDistrito;
    //    public string DistritoCliente;
    //    public string TelefonoCliente;
    //    public string Celular1Cliente;
    //    public string Celular2Cliente;
    //    public string eMailCliente;
    //    public string DesTipoDispositivo;
    //    public string MAC;
    //    public int IDPlan;
    //    public string DesPlan;
    //    public int IDTipoVenta;
    //    public decimal? Importe;

    //    public string AddBill(AuxBilling auxBill)
    //    {
    //        String error = "";
    //        OLOEntities dbcontext = new OLOEntities();
    //        try
    //        {
    //            DateTime auxFechaVenta = new DateTime();
    //            DateTime auxFNacCliente = new DateTime();
    //            if (auxBill.IDTipoVenta == null)
    //            {
    //                auxBill.IDTipoVenta = 1;
    //            }

    //            if (auxBill.FechaVenta == null)
    //            {
    //                error= "El campo FechaVenta es Obligatorio." + System.Environment.NewLine;
    //            }
    //            else
    //            {
    //                try
    //                {
    //                    auxFechaVenta = Convert.ToDateTime(auxBill.FechaVenta);
    //                }
    //                catch (Exception)
    //                {
    //                    error += "Error al convertir el campo Fecha Venta." + System.Environment.NewLine;
    //                }
    //            }
    //            if (auxBill.IDCliente == null)
    //            {
    //                error += "El campo IDCliente es Obligatorio." + System.Environment.NewLine;
    //            }
    //            if (auxBill.FNacCliente == null)
    //            {
    //                error += "El campo IDCliente es Obligatorio." + System.Environment.NewLine;
    //            }
    //            else
    //            {
    //                try
    //                {
    //                    auxFNacCliente = Convert.ToDateTime(auxBill.FNacCliente);
    //                }
    //                catch (Exception)
    //                {
    //                    error += "Error al convertir el campo FNacCliente" + System.Environment.NewLine;
    //                }
    //            }

    //            if (error.Length != 0)
    //                return error;

    //            dbcontext.GrabaBillingWS(
    //                auxBill.IDOLOPV, auxBill.IDOLOUsuario, auxFechaVenta,
    //                auxBill.IDCliente, auxBill.IDTipoDocumento, auxBill.NumDocumentoCliente,
    //                auxBill.NombreCliente, auxFNacCliente.Date, auxBill.IDGeneroCliente,
    //                auxBill.OcupacionCliente, auxBill.NacionalidadCliente, auxBill.DireccionCliente,
    //                auxBill.IDProvincia, auxBill.ProvinciaCliente, auxBill.IDDistrito, auxBill.DistritoCliente,
    //                auxBill.TelefonoCliente, auxBill.Celular1Cliente, auxBill.Celular2Cliente,
    //                auxBill.eMailCliente, auxBill.DesTipoDispositivo, auxBill.MAC, auxBill.IDPlan,
    //                auxBill.DesPlan, auxBill.IDTipoVenta, auxBill.Importe);
    //        }
    //        catch (Exception ex)
    //        {
    //            return ex.InnerException.Message.ToString();
    //        }
    //        return "OK";

    //    }
    //}
    [DataContract]
    public class AuxBilling
    {
        [DataMember]
        public string IDOLOPV;
        [DataMember]
        public string IDOLOUsuario;
        [DataMember]
        [XmlElementAttribute(IsNullable = false)]
        public string FechaVenta;
        [DataMember(IsRequired = true )]
        [Required(AllowEmptyStrings = false)]
        public int IDCliente;
        [DataMember]
        public int IDTipoDocumento;
        [DataMember]
        public string NumDocumentoCliente;
        [DataMember]
        public string NombreCliente;
        [DataMember]
        public string FNacCliente;
        [DataMember]
        public int? IDGeneroCliente;
        [DataMember]
        public string OcupacionCliente;
        [DataMember]
        public string NacionalidadCliente;
        [DataMember]
        public string DireccionCliente;
        [DataMember]
        public int IDProvincia;
        [DataMember]
        public string ProvinciaCliente;
        [DataMember]
        public int? IDDistrito;
        [DataMember]
        public string DistritoCliente;
        [DataMember]
        public string TelefonoCliente;
        [DataMember]
        public string Celular1Cliente;
        [DataMember]
        public string Celular2Cliente;
        [DataMember]
        public string eMailCliente;
        [DataMember]
        public string DesTipoDispositivo;
        [DataMember]
        public string MAC;
        [DataMember]
        public int IDPlan;
        [DataMember]
        public string DesPlan;
        [DataMember]
        [Required(ErrorMessage = "IDTipoVenta is required.")]
        public int IDTipoVenta;
        [DataMember]
        public decimal? Importe;

        public string AddBill(AuxBilling auxBill)
        {
            String error = "";
            OLOEntities dbcontext = new OLOEntities();
            
            try
            {
                DateTime auxFechaVenta = new DateTime();
                DateTime auxFNacCliente = new DateTime();
                //OBLIGATORIOS SIEMPRE
                if (auxBill.IDTipoVenta == null)
                {
                    error += "El campo IDTipoVenta es Obligatorio." + System.Environment.NewLine;
                }
                if (auxBill.IDOLOUsuario == null)
                {
                    error += "El campo IDOLOUsuario es Obligatorio." + System.Environment.NewLine;
                }
                if (auxBill.IDCliente == null)
                {
                    error += "El campo IDCliente es Obligatorio." + System.Environment.NewLine;
                }
                if (auxBill.FechaVenta == null)
                {
                    error += "El campo FechaVenta es Obligatorio." + System.Environment.NewLine;
                }
                else
                {
                    try
                    {
                        auxFechaVenta = Convert.ToDateTime(auxBill.FechaVenta);
                    }
                    catch (Exception)
                    {
                        error += "Error al convertir el campo Fecha Venta." + System.Environment.NewLine;
                    }
                }

                if (auxBill.FNacCliente != null)
                {                    
                    try
                    {
                        auxFNacCliente = Convert.ToDateTime(auxBill.FNacCliente);
                    }
                    catch (Exception)
                    {
                        error += "Error al convertir el campo FNacCliente" + System.Environment.NewLine;
                    }
                }


                // Si es recargas: Obligatorios: 
                if (auxBill.IDTipoVenta == 2)
                {
                    if (auxBill.IDPlan == null)
                    {
                        error += "El campo IDPlan es Obligatorio." + System.Environment.NewLine;
                    }
                    if (auxBill.DesPlan == null)
                    {
                        error += "El campo Desplan es Obligatorio." + System.Environment.NewLine;
                    }
                    if (auxBill.Importe == null)
                    {
                        error += "El campo Importe es Obligatorio." + System.Environment.NewLine;
                    }
                }

                if (error.Length != 0)
                    return error;

                dbcontext.GrabaBillingWS(
                    auxBill.IDOLOPV, auxBill.IDOLOUsuario, auxFechaVenta,
                    auxBill.IDCliente, auxBill.IDTipoDocumento, auxBill.NumDocumentoCliente,
                    auxBill.NombreCliente, auxFNacCliente.Date, auxBill.IDGeneroCliente,
                    auxBill.OcupacionCliente, auxBill.NacionalidadCliente, auxBill.DireccionCliente,
                    auxBill.IDProvincia, auxBill.ProvinciaCliente, auxBill.IDDistrito, auxBill.DistritoCliente,
                    auxBill.TelefonoCliente, auxBill.Celular1Cliente, auxBill.Celular2Cliente,
                    auxBill.eMailCliente, auxBill.DesTipoDispositivo, auxBill.MAC, auxBill.IDPlan,
                    auxBill.DesPlan, auxBill.IDTipoVenta, auxBill.Importe);
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message.ToString();
            }
            return "OK";

        }

    }
}

