using System;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Globalization;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("Auditoria")]
    [XafDefaultProperty("idaud")]
    public class mdl_auditoria : XPLiteObject
    {
        [Action(Caption = "Importar")]
        public void Importar()
        {

            if (ArchivoImportar != null)
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + "\\archivos\\";
                string nombre = idaud + ArchivoImportar.FileName;

                FileStream fileStream = System.IO.File.Create(ruta + nombre);

                ArchivoImportar.SaveToStream(fileStream);
                fileStream.Close();
                Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                var reader = new StreamReader(File.OpenRead(ruta + nombre), iso);
                var line = reader.ReadLine();
                //Session varses = new Session();
                
                using (UnitOfWork varses = new UnitOfWork())
                {
                    
                    int i = 0;

                    while (!reader.EndOfStream)
                    {
                        i++;
                        line = reader.ReadLine();
                        var values = line.Split(',');

                        foreach (mdl_auditoriaactividad item in auditoriaActividad)
                        {
                            if (item.Idactividad.Code.Equals(values[0].Trim()))
                            {
                                item.fechacomp = this.fechaterm;// Estaba en DateTime.Now, si se daba click en un periodo posterior movia las cantidades de actividades completadas
                                item.status = "Completada";
                                item.Save();
                            }
                        }
                    }
                    varses.CommitChanges();
                }

            }
        }


        

        [Action(Caption = "Actualizar Fechas")]
        public void ActualizarFechas()
        {

            CultureInfo provider = CultureInfo.InvariantCulture;
            if (ArchivoFechas != null)
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + "\\archivos\\";
                string nombre = idaud + ArchivoFechas.FileName;

                FileStream fileStream = System.IO.File.Create(ruta + nombre);

                ArchivoFechas.SaveToStream(fileStream);
                fileStream.Close();
                Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                var reader = new StreamReader(File.OpenRead(ruta + nombre), iso);
                var line = reader.ReadLine();
                //Session varses = new Session();

                using (UnitOfWork varses = new UnitOfWork())
                {
                    int i = 0;

                    while (!reader.EndOfStream)
                    {
                        i++;
                        line = reader.ReadLine();
                        var values = line.Split(',');
                        if (values[0]== "Act-031")
                        {
                            int x = 0;
                        }
                        foreach (mdl_auditoriaactividad item in auditoriaActividad)
                        {
                            string cod = item.Idactividad.Code.ToString();
                            if (cod== "Act-031")
                            {
                                int sx = 0;

                            }
                            if (item.Idactividad.Code.Equals(values[0].Trim()))
                            {
                                if (values[1]!=null&&values[1]!="")
                                {
                                    string fechaInicio = values[1].Trim();
                                    var valuesFecha = fechaInicio.Split('/');
                                    DateTime newFi = new DateTime(int.Parse(valuesFecha[2]), int.Parse(valuesFecha[0]), int.Parse(valuesFecha[1]));

                                    //DateTime fI = DateTime.Parse(fechaInicio);
                                    //DateTime fI = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", provider);
                                    item.fechainicio = newFi;
                                }
                                if (values[2]!=null&&values[2]!="")
                                {
                                    string fechaFinal = values[2].Trim();
                                    var valuesFecha = fechaFinal.Split('/');
                                    DateTime newFf = new DateTime(int.Parse(valuesFecha[2]), int.Parse(valuesFecha[0]), int.Parse(valuesFecha[1]));
                                    //DateTime fF = DateTime.Parse(fechaFinal);
                                    //DateTime fF = DateTime.ParseExact(fechaFinal, "dd/MM/yyyy", provider);
                                    item.fechafinal = newFf;
                                }

                               // DateTime fechaOriginalInicio = item.fechainicio;
                              //  DateTime fechaOriginalFinal= item.fechafinal;
                               // item.fechacomp = DateTime.Now;
                                //item.status = "Completada";
                                item.Save();
                            }
                        }
                    }
                    varses.CommitChanges();
                }

            }




            /*
                        DevExpress.Xpo.Session session = this.Session;


                        using (UnitOfWork varses = new UnitOfWork())
                        {

                            var auditorias = new XPQuery<mdl_auditoria>(session);
                            foreach (mdl_auditoria item in auditorias)
                            {
                                DateTime fecha = item.fechaap;
                                var audact = item.auditoriaActividad;
                                foreach (mdl_auditoriaactividad aa in audact)
                                {
                                    aa.fechainicio = fecha;
                                    int duracion = aa.Idactividad.Duracion;
                                    aa.fechafinal = fecha.AddDays(duracion * 7);
                                    aa.Save();
                                }
                            }
                            varses.CommitChanges();
                        }
                      */
        }


        //revisa
        [Action(Caption = "Actualiza Actividades en revisar")]
        public void actividadesNoActualizadas()
        {

            var listaDist = new XPQuery<mdl_distribuidor>(this.Session);
            List<mdl_auditoria> ultimasAuitorias = new List<mdl_auditoria>();
            List<mdl_auditoriaactividad> audactsrev = new List<mdl_auditoriaactividad>();
            string[,] ac;
            int count = 0;
            foreach (var item in listaDist)
            {
                ultimasAuitorias.Add(item.UltimaAuditoria);
            }

            foreach (mdl_auditoria auds in ultimasAuitorias)
            {
                if (auds!=null)
                {
                    List<mdl_auditoriaactividad> audacts = auds.auditoriaActividad.Where(x => x.status.ToLower().Equals("por realizar")).ToList();



                    foreach (mdl_auditoriaactividad audact in audacts)
                    {
                        if (audact.Archivo.Count() == 1)
                        {
                            
                        }
                        if (audact.Archivo.Count()>1)
                        {
                            var raras = audact.Archivo.OrderByDescending(x => x.fecha).First();
                            if (raras.usuario=="Distribuidor")
                            {
                                count++;
                            }
                        }                                           
                    }
                }
              
            }
           int xs = count;
        }

        [Action(Caption = "SEND PRUEBA MAIL")]
        public void mailsent()
        {
            //MailMessage msg = new MailMessage(new MailAddress("mts-noreply@lth.com.mx", new MailAddress("com"));    //  Create a MailMessage object with a from and to address
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("mts-noreply@lth.com.mx", "Plataforma EDC II");
            string cuerpo = "<h4>Actividad Aceptada EDCII</h4>" +
                            "<p>Estimado Ejecutivo de Desarrollo Comercial</p>" +
                            "<p>La actividad ha sido aceptada con Fecha de " + DateTime.Now.ToShortDateString() + " </p>" +
                            "<p>Favor de acceder a la <b>plataforma EDCII</b> para revisar su resultado.</p>" +
                            "<p>Muchas Gracias.</p><hr/>" +
                            "<p>Evaluador: </p>";

            msg.To.Add(new MailAddress("ed.rmzlpz@gmail.com"));
           
            //msg.Bcc()
            msg.Subject = "EDC-II - Actividad enviada a revisión por su Distribuidor";  //  Add your subject

            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            msg.Body = cuerpo;    //  Add the body of your message
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.IsBodyHtml = true; //  Does the body contain html

            SmtpClient client = new SmtpClient("smtp.office365.com", 587); //  Create an instance of SmtpClient with your smtp host and port
            client.Credentials = new NetworkCredential("mts-noreply@lth.com.mx", "Mexico2016"); //  Assign your username and password to connect to gmail
            client.EnableSsl = true;  //  Enable SSL

            try
            {
                client.Send(msg);   //  Try to send your message
                //ShowMailMessage("Your message was sent successfully.", false);  //  A method to update a ui element with a message
                //Clear();
            }
            catch (SmtpException)
            {
                //ShowMailMessage(string.Format("There was an error sending you message. {0}", ex.Message), true);
            }



        }

        [Action(Caption = "Actualiza Evaluadores")]
        public void actualizaEvaluadores()
        {
            foreach (mdl_auditoriaactividad item in auditoriaActividad)
            {
                

                    Usuario eval = null;//Usuario que se le asignará el evaluador dependiendo del permiso seleccionado
                    Usuario usuarioDistribuidor;//traer usuario del mismo nombre que el distribuidor de la auditoria
                    usuarioDistribuidor = new XPQuery<Usuario>(this.Session).FirstOrDefault(x => x.UserName == iddistribuidor.nombredist);
                mdl_actividad act;
                act = item.Idactividad;
                    if (act.Permisos.ToString() == "Particular")
                    {
                        if (  act.Encargado!= null)
                        {
                            eval = act.Encargado;                            
                        }
                        else
                        {
                            //throw new DevExpress.ExpressApp.UserFriendlyException("DEBES ASIGNAR PRIMERO UN ENCARGADO");
                            throw new System.ArgumentException("DEBES ASIGNAR PRIMERO UN ENCARGADO");
                            //this.Permisos = (MascaraPermisos)oldValue;
                        }
                    }
                    else if ((act.Permisos.ToString() == "GerenteCuenta"))
                    {
                        if (iddistribuidor != null)
                        {
                            eval = usuarioDistribuidor.Jefe;
                        }
                    }
                    else if ((act.Permisos.ToString() == "GerenteDesarrolloComercial"))
                    {
                        if (iddistribuidor != null)
                        {
                            if (usuarioDistribuidor.Jefe != null)
                            {
                                eval = usuarioDistribuidor.Jefe.Jefe;
                            }
                        }
                    }
                item.Evaluador = eval;
                item.Save();                
        
            }
            
        }

        private FileData archivoImportar;
        [DevExpress.Xpo.Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        public FileData ArchivoImportar
        {
            get { return archivoImportar; }
            set
            {
                SetPropertyValue("ArchivoImportar", ref archivoImportar, value);

            }
        }

        [XafDisplayName("Cambio de fechas")]
        private FileData archivoFechas;
        [DevExpress.Xpo.Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        public FileData ArchivoFechas
        {
            get { return archivoFechas; }
            set
            {
                SetPropertyValue("ArchivoFechas", ref archivoFechas, value);
            }
        }

        public mdl_auditoria(Session session)
            : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }
        [Key(true)]
        public int idaud { get; set; }

        private mdl_distribuidor iddistribuidor;
        [Association("Auditoria-Distribuidores")]
        [XafDisplayName("Distribuidor")]
        public mdl_distribuidor Iddistribuidor
        {
            get { return iddistribuidor; }
            set { SetPropertyValue("Iddistribuidor", ref iddistribuidor, value); }
        }

        private mdl_tipoauditoria idtipoaud;
        [Association("Auditoria-Tipos")]
        [XafDisplayName("Tipo")]
        public mdl_tipoauditoria Idtipoaud
        {
            get { return idtipoaud; }
            set { SetPropertyValue("Idtipoaud", ref idtipoaud, value); }
        }
        public DateTime fechaap { get; set; }
        public DateTime fechaterm { get; set; }
        public int estatus { get; set; }

        [Association("Auditoria-PondNivel")]
        public XPCollection<mdl_pondnivel> PondNivel
        {
            get
            {
                return GetCollection<mdl_pondnivel>("PondNivel");
            }
        }

              [Association("Auditoria-PondPilares")]
        public XPCollection<mdl_pondpilar> PondPilar
        {
            get
            {
                return GetCollection<mdl_pondpilar>("PondPilar");
            }
        }
        
              [Association("Auditoria-AuditDetalles")]
        public XPCollection<mdl_audidet> AuditDetalle
        {
            get
            {
                return GetCollection<mdl_audidet>("AuditDetalle");
            }
        }


             [Association("AuditoriaActividad-Auditoria")]
        public XPCollection<mdl_auditoriaactividad> auditoriaActividad
        {
            get
            {
                return GetCollection<mdl_auditoriaactividad>("auditoriaActividad");
            }
        }

      

    }

}