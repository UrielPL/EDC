using CatalogosLTH.Module.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace CatalogosLTH.Web
{
    public class Util
    {
        public static DevExpress.Xpo.Session getsession()
        {
            /*DevExpress.Xpo.Session session = new DevExpress.Xpo.Session();
            session.ConnectionString = MySqlConnectionProvider.GetConnectionString("172.93.106.146", "lth", "output", "lth2");*/
            DevExpress.Xpo.Session session = new DevExpress.Xpo.Session();
            if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null)
            {
                session.ConnectionString = ConfigurationManager.
                ConnectionStrings["ConnectionString"].ConnectionString;
            }

            return session;
        }
        public static TipoUsuario getPermiso()
        {
            TipoUsuario permiso;
            Usuario us = (Usuario)SecuritySystem.CurrentUser;
            permiso = us.TipoUsuario;
            return permiso;
        }

        public static string getusuario()
        {
            Usuario us = (Usuario)SecuritySystem.CurrentUser;
            return us.UserName;
        }
        public static Usuario getJefe(string nombre)
        {
            DevExpress.Xpo.Session session = getsession();
            Usuario jefe = null;
            Usuario UsDist = new XPQuery<Usuario>(session).FirstOrDefault(x => x.UserName == nombre);
            if (UsDist != null)
            {
                jefe = UsDist.Jefe;
            }

            return jefe;
        }
        public static string getrol()
        {
            string rol = "";
            Usuario us = (Usuario)SecuritySystem.CurrentUser;
            if(us.Roles.Count>1)
            {
                foreach (SecuritySystemRole item in us.Roles.Where(x => x.Name != "Administrator"))
                {
                    rol = item.Name;
                }
            }
            else
            {
                foreach (SecuritySystemRole item in us.Roles)
                {
                    rol = item.Name;
                }
            }
            
            return rol;
        }

        public static bool tieneRol(string tipo)
        {
            Usuario us = (Usuario)SecuritySystem.CurrentUser;
            var roles = us.Roles;
            if (roles.Where(x => x.Name == tipo).Count() > 0)
                return true;
            else
                return false;
        }

        public static bool alsoAdmin() {
            Usuario us = (Usuario)SecuritySystem.CurrentUser;
            if (us.Roles.Where(x => x.IsAdministrative == true).Count() > 0)
            {
                return true;
            }
            else { return false; }
        }

        public static int getUltimaAuditoria(int distribuidor)
        {
            DevExpress.Xpo.Session session = getsession();
            int auditoriasRevisa = new XPQuery<mdl_auditoria>(session).Where(x => x.Iddistribuidor.iddistribuidor == distribuidor && x.estatus == 1).Max(x => x.idaud);
            return auditoriasRevisa;
        }

        public static int getNivelActual(int distribuidor)
        {
            DevExpress.Xpo.Session session = getsession();
            int cNivel = new XPQuery<mdl_nivel>(session).Count();
            int idAuditoria = getUltimaAuditoria(distribuidor);
            int nivelActual = 1;
            for (int i = 1; i <= cNivel; i++)
            {
                int cantAct = new XPQuery<mdl_auditoriaactividad>(session)
                    .Count(x => x.Idaud.ToString() == idAuditoria.ToString() &&
                         x.fechacomp == null &&
                         x.Idactividad.NivelID.idnivel.ToString() == i.ToString() &&
                         x.Idactividad.Code != "Act-046"
                         );
                //int cantAct2 = new XPQuery<mdl_auditoriaactividad>(session).Count(x => x.Idaud.ToString() == idAuditoria.ToString() && x.fechacomp == DateTime.MinValue && x.Idactividad.NivelID.idnivel.ToString() == i.ToString());
                if (cantAct > 0)
                {
                    break;
                }
                if (i < cNivel) { nivelActual = i + 1; }
            }
            return nivelActual;
        }

        public static double getNivelCumplimientoActividades(int distribuidor)
        {
            DevExpress.Xpo.Session session = getsession();
            int nivel = getNivelActual(distribuidor);
            int idAuditoria = getUltimaAuditoria(distribuidor);
            double total = 0;
            double contestadas = 0;
            

            var Acts = new XPQuery<mdl_auditoriaactividad>(session)
                .Where(x => x.Idaud.ToString() == idAuditoria.ToString()
                    && x.Idactividad.NivelID.idnivel.ToString() == nivel.ToString());

            double cantAct = Acts.Count();
            double ActsCompletas = (from ac in Acts
                                 where ac.fechacomp != null
                                    select ac).Count();

            double avance = ActsCompletas * 100 / cantAct;

            return avance;
        }

        public static void ActualizarNivelProfesionalizacion(int distribuidor)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            mdl_distribuidor dis = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.iddistribuidor == distribuidor);
            double[] prof = getNivelProf(distribuidor);
            string nivelP = getNivelActividades(prof[0]);// nivel del 1 al 5
            
            double porcentaje = getNivelCumplimientoActividades(distribuidor);//porcentaje de cumplimiento de actividades

            using (var uow = new UnitOfWork())
            {
                //mdl_distribuidor update = uow.GetObjectByKey<mdl_distribuidor>(dis.iddistribuidor);                
                mdl_distribuidor update = dis;
                update.profesionalizacion = prof[0];
                update.avanceProd = prof[1];
                update.avanceInfra = prof[2];
                update.avanceAdministracion = prof[3];
                update.avanceEjecucion = prof[4];
                update.avancePlaneacion = prof[5];
                update.nivelAct = nivelP;
                update.cumplimientoNivel = porcentaje;
                
                update.Save();                
                uow.CommitChanges();
            }
        }
        public static void AsignarProfesionalizacion(int distribuidor, double profesionalizacion)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            mdl_distribuidor dis = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.iddistribuidor == distribuidor);
            double prof = profesionalizacion;

            using (var uow = new UnitOfWork())
            {
                mdl_distribuidor update = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.iddistribuidor == distribuidor);
                update.profesionalizacion = prof;
                update.Save();
                uow.CommitChanges();
            }
        }

        public static string correoActividadSubida(string distribuidor, string actividad, string nombre)
        {
            string cuerpo = "<h4>Estimado Evaluador: "+nombre+"</h4>" +
                "<p>El Distribuidor: " + distribuidor + ", ha enviado a su revision la actividad " + actividad + " con Fecha de " + DateTime.Now.ToShortDateString() + ".</p>" +
                "<p>Favor de entrar a la plataforma EDC-III para evaluar la actividad.</p>" +
                "<p>Muchas Gracias.</p>";
                

            return cuerpo;
        }

        public static string correoActividadRevisada(string distribuidor, string actividad, string nombre)
        {
            string cuerpo = "<h4>Actividad Evaluada EDCII</h4>" +
                            "<p>Estimado Ejecutivo de Desarrollo Comercial</p>" +
                            "<p>La actividad " + actividad + " ha sido evaluada con Fecha de " + DateTime.Now.ToShortDateString() + "</p>" +
                            "<p>Favor de entrar a la plataforma EDCII para revisar su resultado.</p>" +
                            "<p>Muchas Gracias.</p>" +
                        "<p>Evaluador: " + nombre + "</p>";


            return cuerpo;
        }

        public static string correoActividadAceptada(string actividad, string nombre)
        {
            string cuerpo = "<h4>Actividad Aceptada EDCII</h4>" +
                            "<p>Estimado Ejecutivo de Desarrollo Comercial</p>" +
                            "<p>La actividad " + actividad + " ha sido aceptada con Fecha de " + DateTime.Now.ToShortDateString() + " </p>" +
                            "<p>Favor de acceder a la plataforma EDCII para revisar su resultado.</p>" +
                            "<p>Muchas Gracias.</p><hr/>"+
                            "<p>Evaluador: " + nombre + "</p>";


            return cuerpo;
        }

        public static string correoAvisaGerente(string distribuidor, string actividad, string nombre)
        {
            string cuerpo = "<h4>Actividad enviada a revisión por su Distribuidor</h4>" +
                            "<p>Estimado Gerente  </p>" +
                            "<p>Le informamos que su Distribuidor: "+ distribuidor+" ha enviado a revision la actividad: "+actividad+" con fecha de: "+DateTime.Now.ToShortDateString()+".</p>" +
                            "<p>Para consultar su estatus favor de accesar a la plataforma EDCIII.</p>" +
                            "<p>Evaluador: "+nombre+"<p>"+
                            "<p>Muchas Gracias.</p>";

            return cuerpo;
        }

        public static string correoAvisaGerenteTabla(string distribuidor, string nombre)
        {
            string cuerpo = "<h4>Tabla Financiera enviada a revision por su Distribuidor</h4>" +
                            "<p>Estimado Gerente  </p>" +
                            "<p>Le informamos que su Distribuidor: " + distribuidor + " ha enviado a revision una tabla financiera con fecha de: " + DateTime.Now.ToShortDateString() + ".</p>" +
                            "<p>Para consultar su estatus favor de accesar a la plataforma EDC-III.</p>" +
                            "<p>Evaluador: " + nombre + "<p>" +
                            "<p>Muchas Gracias.</p>";

            return cuerpo;
        }

        public static string correoAvisaEvaluadorPendientes(string Evaluador, string pendientes)
        {
            string cuerpo = "<h4>Actividades pendientes por revisar</h4>" +
                            "<p>Estimado Evaluador  </p>" +
                            "<p>Le informamos que al dia de hoy, " + DateTime.Now.ToString("dd/MMM/yyyy") + ", cuenta con un total de " + pendientes + " actividades pendientes por revisar.</p>" +
                            "<p>Para consultar las actividades, favor de accesar a la plataforma EDC-III, módulo 'Actividades por revisar'.</p>" +
                            "<p>Evaluador: " + Evaluador + "<p>" +
                            "<p>Muchas Gracias.</p>";

            return cuerpo;
        }

        public static void SendMail(string to, string subject, string body, List<string> correos, string file)
        {
            //MailMessage msg = new MailMessage(new MailAddress("mts-noreply@lth.com.mx", new MailAddress("com"));    //  Create a MailMessage object with a from and to address
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("mts-noreply@lth.com.mx", "Plataforma EDC III");
            
            foreach (var item in correos)
            {
                msg.To.Add(new MailAddress(item));
            }
            //msg.Bcc()
            msg.Bcc.Add("edgar.ramirez@holdasociados.com, uriel.peralta@udem.edu");
            msg.Subject = subject;  //  Add your subject

            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            msg.Body = body;    //  Add the body of your message
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.IsBodyHtml = true; //  Does the body contain html

            if(file != "")
            {
                string filename = file;
                Attachment data = new Attachment(filename, MediaTypeNames.Application.Octet);
                msg.Attachments.Add(data);
            }
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            SmtpClient client = new SmtpClient("smtp.office365.com", 587); //  Create an instance of SmtpClient with your smtp host and port
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential("mts-noreply@lth.com.mx", "Mexico2016"); //  Assign your username and password to connect to gmail
            client.EnableSsl = true;  //  Enable SSL
            
            

            try
            {
                client.Send(msg);   //  Try to send your message
                //ShowMailMessage("Your message was sent successfully.", false);  //  A method to update a ui element with a message
                //Clear();
            }
            catch (Exception es)
            {
                var s = es.Message;
                //ShowMailMessage(string.Format("There was an error sending you message. {0}", ex.Message), true);
            }

            
        }

        public static void SendMailTabla(string to, string subject, string body)
        {
            //MailMessage msg = new MailMessage(new MailAddress("mts-noreply@lth.com.mx", new MailAddress("com"));    //  Create a MailMessage object with a from and to address
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("mts-noreply@lth.com.mx", "Plataforma EDC III");

            
            msg.To.Add(new MailAddress(to));
            
            //msg.Bcc()
            msg.Bcc.Add("edgar.ramirez@holdasociados.com");
            msg.Subject = subject;  //  Add your subject

            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            msg.Body = body;    //  Add the body of your message
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.IsBodyHtml = true; //  Does the body contain html

            
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            SmtpClient client = new SmtpClient("smtp.office365.com", 587); //  Create an instance of SmtpClient with your smtp host and port
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential("mts-noreply@lth.com.mx", "Mexico2016"); //  Assign your username and password to connect to gmail
            client.EnableSsl = true;  //  Enable SSL



            try
            {
                client.Send(msg);   //  Try to send your message
                //ShowMailMessage("Your message was sent successfully.", false);  //  A method to update a ui element with a message
                //Clear();
            }
            catch (Exception es)
            {
                var s = es.Message;
                //ShowMailMessage(string.Format("There was an error sending you message. {0}", ex.Message), true);
            }


        }

        //public static void enviaMail()
        //{
        //    int port = 587;
        //    string host = "smtp.office365.com";
        //    string username = "mts-noreply@lth.com.mx";
        //    string password = "Mexico2016";
        //    string mailFrom = "mts-noreply@lth.com.mx";
        //    string mailTo = "mailto@mail.com";
        //    string mailTitle = "Testtitle";
        //    string mailMessage = "Testmessage";

        //    var message = new MimeMessage();
        //    message.From.Add(new MailboxAddress(mailFrom));
        //    message.To.Add(new MailboxAddress(mailTo));
        //    message.Subject = mailTitle;
        //    message.Body = new TextPart("plain") { Text = mailMessage };

        //    using (var client = new SmtpClient())
        //    {
        //        client.Connect(host, port, SecureSocketOptions.StartTls);
        //        client.Authenticate(username, password);

        //        client.Send(message);
        //        client.Disconnect(true);
        //    }
        //}

        public static string getNivelActividades(double prof)
        {
            string nivelP = "Básico";
            if (prof >= 0 && prof < 44.5)
            {
                nivelP = "Básico";
            }
            else if (prof >= 44.5 && prof <= 65.5)
            {
                nivelP = "Transitorio";
            }
            else if (prof >= 65.5 && prof < 80.5)
            {
                nivelP = "Intermedio";
            }
            else if (prof >= 80.5 && prof < 94.5)
            {
                nivelP = "Avanzado";
            }
            else if (prof >= 94.5 && prof <= 100)
            {
                nivelP = "Sobresaliente";
            }


            return nivelP;
        }

        public static double[] getNivelProf(int distribuidor)
        {

            double idtotalv = 0;
            double avancea = 0;
            double totalp = 0;
            double totalc = 0;
            double totaldc = 0;
            double avanceTotalT = 0;
            double avanceTotalFinalDC = 0;//completadas por dc linea roja final
            double totalProfesionalizacion = 0;

            double activas = 0;
            double totalpt = 0;
            double totalct = 0;
            double activast = 0;
            double avanceat = 0;
            double comp = 0;

            int[] arrTotal = new int[5];
            int[] arrActivas = new int[5];
            string[] nomPilar = new string[5];
            arrActivas[0] = 0;
            arrActivas[1] = 0;
            arrActivas[2] = 0;
            arrActivas[3] = 0;
            arrActivas[4] = 0;

            arrTotal[0] = 0;
            arrTotal[1] = 0;
            arrTotal[2] = 0;
            arrTotal[3] = 0;
            arrTotal[4] = 0;
            int numPilar = 0;

            DevExpress.Xpo.Session session = getsession();
            
            var distribuidores = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.iddistribuidor == distribuidor);
            string idauditoria = distribuidores.UltimaAuditoria.idaud.ToString();
            mdl_auditoria auditoria = new XPQuery<mdl_auditoria>(session).FirstOrDefault(x => x.idaud.ToString() == idauditoria);
          
            XPQuery<mdl_pondnivel> ponderacionesNivel = session.Query<mdl_pondnivel>();
            XPQuery<mdl_nivel> niveles = session.Query<mdl_nivel>();
            XPQuery<mdl_catnivel> catNiveles = session.Query<mdl_catnivel>();
            XPQuery<vwtotal> totales = session.Query<vwtotal>();
            var puntostotales = new XPQuery<vwpuntostot>(session).Where(x => x.idaud.ToString() == idauditoria);
            var auditoriaActividad = new XPQuery<mdl_auditoriaactividad>(session).Where(x => x.Idaud.ToString() == idauditoria);
            int tipoaud = 0;
            string nombredist = "";
            int idaudi = Convert.ToInt32(idauditoria);

            tipoaud = auditoria.Idtipoaud.idTipoAuditoria;
            nombredist = auditoria.Iddistribuidor.nombredist;
             

            var sql3 = from ni in niveles
                       join ctn in catNiveles on ni equals ctn.Idnivel
                       where ctn.Idtipoaud.idTipoAuditoria == auditoria.Idtipoaud.idTipoAuditoria
                       select new { ni.idnivel, ni.nombreniv, ctn.ponderacion, ctn.Idtipoaud };

            foreach (var rec3 in sql3)
            {
                var sql2 = from vt in totales
                           where vt.idnivel == rec3.idnivel
                           && vt.idtipoaud == tipoaud
                           select vt;
                foreach (var record2 in sql2)//FOREACH PILARES
                {

                    var sqln = from vpt in puntostotales
                               where vpt.idaud == idaudi &&
                               vpt.idpilar == record2.idpilar &&
                               vpt.idnivel == record2.idnivel
                               select vpt;
                    idtotalv = sqln.ToList().Count();//completadas en sistema
                  
                    double completadasAuditoria = 0;
                    if (record2.total == 0)
                    {

                    }
                    else
                    {
                        completadasAuditoria = (record2.total - idtotalv);
                       // totalc = record2.total - idtotalv + totalc;
                    }
                    

                    var sqlAudAct = from audact in auditoriaActividad
                                    where audact.Idaud.idaud.ToString() == idauditoria && audact.fechacomp != null && audact.Idactividad.NivelID.nombreniv == record2.nombreniv && audact.Idactividad.PilarID.nombrepil == record2.nombrepil
                                    select audact;

                    int actividadesRealizadasDC = sqlAudAct.Count();
                    totaldc += actividadesRealizadasDC;


                    //
                    if (nomPilar[numPilar] == null)
                    {
                        nomPilar[numPilar] = record2.nombrepil;
                    }
                    arrTotal[numPilar] = arrTotal[numPilar] + record2.total;
                    //

                    double actividadesTotalesPorRealizarDeLaAuditoria = idtotalv;
               
                    if (record2.total == 0)
                    {
                        comp = record2.pondercionPilar;
                       
                    }
                    else
                    {
                        comp = (record2.total - idtotalv) / record2.total;
                        comp = comp * record2.pondercionPilar;
                        double actividadesActivas = actividadesTotalesPorRealizarDeLaAuditoria - actividadesRealizadasDC;
                        arrActivas[numPilar] = arrActivas[numPilar] + (int)actividadesActivas;

                    }
                    avancea = comp + avancea;

                    double avanceTotal = (record2.total == 0) ? record2.pondercionPilar : (completadasAuditoria + actividadesRealizadasDC) / record2.total * record2.pondercionPilar;
                    avanceTotalT += avanceTotal;
                    double profesionalizacionNivel = (rec3.ponderacion * avanceTotalT) / 100.0;
                    totalProfesionalizacion += profesionalizacionNivel;//TOTAL PROFESIONALIZACION LINEA ROJA
                 

                    avanceTotalFinalDC += totaldc;
                    totalpt = totalp + totalpt;
                    totalct = totalct + totalc;
                    activast = activast + activas;
                    avanceat = ((rec3.ponderacion * avancea) / 100) + avanceat;
                    totalp = 0;
                    totalc = 0;
                    activas = 0;
                    avancea = 0;
                    totaldc = 0; //se restaura totaldc a 0
                    avanceTotalT = 0;

                    numPilar++;

                }
                numPilar = 0;

            }

            int total1 = arrTotal[0];
            int total2 = arrTotal[1];
            int total3 = arrTotal[2];
            int total4 = arrTotal[3];
            int total5 = arrTotal[4];

            int act1 = arrActivas[0];
            int act2 = arrActivas[1];
            int act3 = arrActivas[2];
            int act4 = arrActivas[3];
            int act5 = arrActivas[4];

            double[] arrPorcentaje = new double[5];

            double porcentaje1 = 100;
            double porcentaje2 = 100;
            double porcentaje3 = 100;
            double porcentaje4 = 100;
            double porcentaje5 = 100;

            if (total1 != 0) { porcentaje1 = 100 - (act1 * 100 / total1); }
            if (total2 != 0) { porcentaje2 = 100 - (act2 * 100 / total2); }
            if (total3 != 0) { porcentaje3 = 100 - (act3 * 100 / total3); }
            if (total4 != 0) { porcentaje4 = 100 - (act4 * 100 / total4); }
            if (total5 != 0) { porcentaje5 = 100 - (act5 * 100 / total5); }

            arrPorcentaje[0] = porcentaje1;
            arrPorcentaje[1] = porcentaje2;
            arrPorcentaje[2] = porcentaje3;
            arrPorcentaje[3] = porcentaje4;
            arrPorcentaje[4] = porcentaje5;

            double[] arregloFinal = new double[6];
            arregloFinal[0] = totalProfesionalizacion;

            for (int i = 0; i < arrPorcentaje.Length; i++)
            {
                if (nomPilar[i] == "Productos y Servicios")
                {
                  arregloFinal[1]= arrPorcentaje[i];
                }
                else if(nomPilar[i] == "Infraestructura")
                {
                    arregloFinal[2]= arrPorcentaje[i];
                }
                else if (nomPilar[i] == "Administración")
                {
                    arregloFinal[3]= arrPorcentaje[i];
                }
                else if (nomPilar[i] == "Ejecución")
                {
                    arregloFinal[4]= arrPorcentaje[i];
                }
                else if (nomPilar[i] == "Planeacion")
                {
                    arregloFinal[5]= arrPorcentaje[i];
                }

            }

            return arregloFinal;
        }

        public static double getPorcentajeProf(int distribuidor, int mes)
        {
            int year = DateTime.Now.Year;
            DateTime fechaFiltro = new DateTime(year, mes, 1);
            DateTime fechaInicio = DateTime.Now;
            fechaInicio.AddYears(-15);
            double idtotalv = 0;
            double avancea = 0;
            double totalp = 0;
            double totalc = 0;
            double totaldc = 0;
            double avanceTotalT = 0;
            double avanceTotalFinalDC = 0;//completadas por dc linea roja final
            double totalProfesionalizacion = 0;

            double activas = 0;
            double totalpt = 0;
            double totalct = 0;
            double activast = 0;
            double avanceat = 0;
            double comp = 0;

            int[] arrTotal = new int[5];
            int[] arrActivas = new int[5];
            string[] nomPilar = new string[5];
            arrActivas[0] = 0;
            arrActivas[1] = 0;
            arrActivas[2] = 0;
            arrActivas[3] = 0;
            arrActivas[4] = 0;

            arrTotal[0] = 0;
            arrTotal[1] = 0;
            arrTotal[2] = 0;
            arrTotal[3] = 0;
            arrTotal[4] = 0;
            int numPilar = 0;

            DevExpress.Xpo.Session session = getsession();

            var distribuidores = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.iddistribuidor == distribuidor);
            string idauditoria = distribuidores.UltimaAuditoria.idaud.ToString();
            mdl_auditoria auditoria = new XPQuery<mdl_auditoria>(session).FirstOrDefault(x => x.idaud.ToString() == idauditoria);

            XPQuery<mdl_pondnivel> ponderacionesNivel = session.Query<mdl_pondnivel>();
            XPQuery<mdl_nivel> niveles = session.Query<mdl_nivel>();
            XPQuery<mdl_catnivel> catNiveles = session.Query<mdl_catnivel>();
            XPQuery<vwtotal> totales = session.Query<vwtotal>();
            var puntostotales = new XPQuery<vwpuntostot>(session).Where(x => x.idaud.ToString() == idauditoria);

            var auditoriaActividad = new XPQuery<mdl_auditoriaactividad>(session).Where(x => x.Idaud.ToString() == idauditoria).ToList();//AUDITORIAS ACTIVIDADES
            var auditoriaActividad2 = new XPQuery<mdl_auditoriaactividad>(session).Where(x => x.Idaud.ToString() == idauditoria&&x.fechacomp<fechaFiltro).ToList();//AUDITORIAS ACTIVIDADES
            var auditoriaActividad3 = new XPQuery<mdl_auditoriaactividad>(session).Where(x => x.Idaud.ToString() == idauditoria && x.fechacomp > fechaFiltro).ToList();//AUDITORIAS ACTIVIDADES

            int tipoaud = 0;
            string nombredist = "";
            int idaudi = Convert.ToInt32(idauditoria);

            tipoaud = auditoria.Idtipoaud.idTipoAuditoria;
            nombredist = auditoria.Iddistribuidor.nombredist;


            var sql3 = from ni in niveles
                       join ctn in catNiveles on ni equals ctn.Idnivel
                       where ctn.Idtipoaud.idTipoAuditoria == auditoria.Idtipoaud.idTipoAuditoria
                       select new { ni.idnivel, ni.nombreniv, ctn.ponderacion, ctn.Idtipoaud };

            foreach (var rec3 in sql3)
            {

                var sql2 = from vt in totales
                           where vt.idnivel == rec3.idnivel
                           && vt.idtipoaud == tipoaud
                           select vt;
                foreach (var record2 in sql2)//FOREACH PILARES
                {


                    var sqln = from vpt in puntostotales
                               where vpt.idaud == idaudi &&
                               vpt.idpilar == record2.idpilar &&
                               vpt.idnivel == record2.idnivel
                               select vpt;
                    idtotalv = sqln.ToList().Count();//completadas en sistema

                    double completadasAuditoria = 0;
                    if (record2.total == 0)
                    {

                    }
                    else
                    {
                        completadasAuditoria = (record2.total - idtotalv);
                        // totalc = record2.total - idtotalv + totalc;
                    }

                    var sqlAudAct = from audact in auditoriaActividad
                                    where audact.Idaud.idaud.ToString() == idauditoria && audact.fechacomp != null && audact.Idactividad.NivelID.nombreniv == record2.nombreniv && audact.Idactividad.PilarID.nombrepil == record2.nombrepil //&&audact.fechacomp<fechaFiltro && audact.fechacomp>fechaInicio
                                    select audact;

                    int actividadesRealizadasDC = sqlAudAct.Count();
                    totaldc += actividadesRealizadasDC;


                    //
                    if (nomPilar[numPilar] == null)
                    {
                        nomPilar[numPilar] = record2.nombrepil;
                    }
                    arrTotal[numPilar] = arrTotal[numPilar] + record2.total;
                    //

                    double actividadesTotalesPorRealizarDeLaAuditoria = idtotalv;

                    if (record2.total == 0)
                    {
                        comp = record2.pondercionPilar;

                    }
                    else
                    {
                        comp = (record2.total - idtotalv) / record2.total;
                        comp = comp * record2.pondercionPilar;
                        double actividadesActivas = actividadesTotalesPorRealizarDeLaAuditoria - actividadesRealizadasDC;
                        arrActivas[numPilar] = arrActivas[numPilar] + (int)actividadesActivas;

                    }
                    avancea = comp + avancea;

                    double avanceTotal = (record2.total == 0) ? record2.pondercionPilar : (completadasAuditoria + actividadesRealizadasDC) / record2.total * record2.pondercionPilar;
                    avanceTotalT += avanceTotal;
                    double profesionalizacionNivel = (rec3.ponderacion * avanceTotalT) / 100.0;
                    totalProfesionalizacion += profesionalizacionNivel;//TOTAL PROFESIONALIZACION LINEA ROJA


                    avanceTotalFinalDC += totaldc;
                    totalpt = totalp + totalpt;
                    totalct = totalct + totalc;
                    activast = activast + activas;
                    avanceat = ((rec3.ponderacion * avancea) / 100) + avanceat;
                    totalp = 0;
                    totalc = 0;
                    activas = 0;
                    avancea = 0;
                    totaldc = 0; //se restaura totaldc a 0
                    avanceTotalT = 0;

                    numPilar++;

                }
                numPilar = 0;

            }
            
            return totalProfesionalizacion;
        }


        public void generaRegistrosMensuales()
        {
            DevExpress.Xpo.Session session = getsession();
            /*XPQuery<mdl_distribuidor> distribuidores = session.Query<mdl_distribuidor>();
            List<mdl_distribuidor> listaD = new List<mdl_distribuidor>();
            foreach (var dis in distribuidores)
            {
                if (dis.Registro.Count() == 0)
                {
                    listaD.Add(dis);
                }
            }*/

            List<string> meses = new List<string>();

            meses.Add("OCTUBRE");
            meses.Add("NOVIEMBRE");
            meses.Add("DICIEMBRE");
            meses.Add("ENERO");
            meses.Add("FEBRERO");
            meses.Add("MARZO");
            meses.Add("ABRIL");
            meses.Add("MAYO");
            meses.Add("JUNIO");
            meses.Add("JULIO");
            meses.Add("AGOSTO");
            meses.Add("SEPTIEMBRE");
            var periodo = new XPQuery<mdl_periodo>(session).OrderByDescending(v => v.Periodo).FirstOrDefault();
            //Distribuidores sin registros mensuales (generalmente por ser agregados recientemente)
            List<mdl_distribuidor> distribuidores = session.Query<mdl_distribuidor>().Where(x => x.Registro.Count() == 0 && x.nombredist != "nohay").ToList();
            int cont = 0;

            using (UnitOfWork varses = new UnitOfWork())
            {
                 foreach (var mes in meses)
                 {
                     cont++;
                     foreach (var dis in distribuidores)
                     {
                         mdl_RegistroMensual registro = new mdl_RegistroMensual(session);

                         registro.Periodo = periodo;
                         registro.orden = cont;
                         registro.Distribuidor = dis;
                         registro.Mes = mes;

                         registro.resultado = 0;
                         registro.terminadas = 0;
                         registro.Save();
                     }
                 }

                varses.CommitChanges();
            }

            foreach (var item in distribuidores)
            {
                actualizaRegistroMensual(item.nombredist, null);
            }
        }

        public static void actualizaRegistroMensual(string distribuidor, mdl_auditoriaactividad audact)
        {
            DevExpress.Xpo.Session session = getsession();
            int mesActual = DateTime.Now.Month;
            int[] meses = new int[12] { 10, 11, 12, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int orden = Array.IndexOf(meses, mesActual)+1;// se suma uno porque el index inicial es 0 y el orden en la BD empieza en 1
            int year = (mesActual > 9) ? DateTime.Now.Year + 1 : DateTime.Now.Year;

            mdl_distribuidor dist = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist == distribuidor);

            mdl_RegistroMensual registro = new XPQuery<mdl_RegistroMensual>(session).FirstOrDefault(x => x.Periodo.Periodo == year && x.orden == orden&&x.Distribuidor==dist); //Registros del periodo y mes seleccionados

            using (UnitOfWork uow = new UnitOfWork())
            {
                int cont = 0;
               
                cont++;
               
                int actTerminadas = new XPQuery<mdl_auditoriaactividad>(session).Where(x => x.Idaud == dist.UltimaAuditoria && x.fechacomp.Month == mesActual && x.fechacomp.Year == DateTime.Now.Year).Count();
                registro.resultado = dist.profesionalizacion;
                registro.nivel = dist.nivelAct;
                registro.terminadas = actTerminadas;
                registro.Save();
               
                uow.CommitChanges();
            }
        }
        public static string getUserLogin()
        {
            string usuario = HttpContext.Current.Session["usuario"] as string;
            if(String.IsNullOrEmpty(usuario))
            {
                usuario = " ";
            }
            return usuario;
        }
        public static string getPassLogin()
        {
            string pass = HttpContext.Current.Session["pass"] as string;
            if (String.IsNullOrEmpty(pass))
            {
                pass = " ";
            }
            return pass;
        }
    }
}
