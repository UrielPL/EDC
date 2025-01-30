using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogosLTH.Web
{
    public partial class Auditoria_movil : System.Web.UI.Page
    {
        public mdl_distribuidor dist { get; set; }
        public DevExpress.Xpo.Session session { get; set; }
        public AuditoriaMovilGeneral amg { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            session = Util.getsession();
            var codigo = Request.QueryString["idamg"];
            Session["amg"] = codigo;
            amg = new XPQuery<AuditoriaMovilGeneral>(session).FirstOrDefault(x => x.IdAudMovG.ToString() == codigo);
            dist = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.iddistribuidor == amg.distribuidor.iddistribuidor);
            if (!IsPostBack)
            {
                
                string a = "Centro de Servicio";
                if(!string.IsNullOrEmpty(Session["cmbArea"] as string))
                {
                    a = Session["cmbArea"] as string;
                }
                if (!string.IsNullOrEmpty(codigo.ToString()))
                {  
                    //CARGA LA INFO DE LA ULTIMA AUDITORIA DEL DISTRIBUIDOR
                    cmbTipo.Items.Add(dist.UltimaAuditoria.Idtipoaud.Descripcion);
                    cmbDist.Items.Add(dist.nombredist);
                    cmbTipo.SelectedIndex = 0;
                    cmbDist.SelectedIndex = 0;
                    cmbTipo.Enabled = false;
                    cmbDist.Enabled = false;
                    cmbTipo.HelpText = "Tipo auditoría";
                    cmbDist.HelpText = "Distribuidor";
                    cmbTipo.HelpTextSettings.HorizontalAlign = DevExpress.Web.HelpTextHorizontalAlign.Center;
                    cmbDist.HelpTextSettings.HorizontalAlign = DevExpress.Web.HelpTextHorizontalAlign.Center;
                    int cs = 0;
                    int cont = 0;
                    var areasq = new XPQuery<mdl_Area>(session).Where(x => x.Idtipoaud == dist.UltimaAuditoria.Idtipoaud);
                    //XPCollection<mdl_Area> areas = new XPCollection<mdl_Area>(session);
                    var areas = areasq.OrderBy(x => x.Nombre);
                    foreach (var area in areas)
                    {
                        cmbArea.Items.Add(area.Nombre);
                        if (area.Nombre.Equals(a) )
                            cs = cont;
                        cont++;
                    }
                    cmbArea.HelpText="Áreas";
                    cmbArea.HelpTextSettings.HorizontalAlign = DevExpress.Web.HelpTextHorizontalAlign.Center;                    
                    cmbArea.ToolTip="Áreas";

                    cmbClave.HelpText = "Clave";
                    cmbClave.HelpTextSettings.HorizontalAlign = DevExpress.Web.HelpTextHorizontalAlign.Center;

                    cmbArea.SelectedIndex = cs;
                    llenagrid();
                    Session["cmbArea"] = null;
                    //CARGA EL TIPO DE AUDITORIA dependiendo del tipo de la ultima auditoria del query 
                    List<string> lstPuntos = new XPQuery<mdl_punto>(session).Where(c => c.Idtipoaud.Descripcion == cmbTipo.Text).Select(c=>c.clavepunto).ToList();
                    cmbClave.Items.Add("Todas");
                    foreach (var item in lstPuntos)
                    {
                        cmbClave.Items.Add(item);
                    }
                    cmbClave.SelectedIndex = 0;
                }
                lblArea.Text = cmbArea.Text;      
            }

            string v = cmbArea.SelectedItem.Text;
        }
        public void llenagrid(string filtro=null)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            string codigo = Session["amg"] as string;
            AuditoriaMovilGeneral amg = new XPQuery<AuditoriaMovilGeneral>(session).FirstOrDefault(x => x.IdAudMovG.ToString() == codigo);

            mdl_tipoauditoria tipo = new XPQuery<mdl_tipoauditoria>(session).FirstOrDefault(a => a.Descripcion == cmbTipo.SelectedItem.Text);//tipo seleccionado de aud en cmb
            mdl_distribuidor dist = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist == cmbDist.SelectedItem.Text);//dist seleccionado en cmb
            mdl_Area area = new XPQuery<mdl_Area>(session).FirstOrDefault(x => x.Nombre == cmbArea.SelectedItem.Text && x.Idtipoaud == tipo);//Area seleccionada
            mdl_auditoria ultAud = dist.UltimaAuditoria;
            //mdl_auditoriaactividad ax = new XPQuery<mdl_auditoriaactividad>(session).FirstOrDefault(z => z.idactplan == 5612);

            //List<mdl_punto> lstPuntos = new XPQuery<mdl_punto>(session).Where(c => c.Idtipoaud == tipo).ToList();
            //List<mdl_Area> lstAreas = new XPQuery<mdl_Area>(session).Where(d => d.Idtipoaud == tipo).ToList();
           // List<actpunto> lstActPunto = new List<actpunto>();
        
            List<puntoarea> lstPuntoArea = new List<puntoarea>();
            foreach (var item in amg.AudMDet)
            {
                puntoarea pa = new puntoarea();
                pa.Punto = item.punto.texto;
                pa.Area = item.punto.Areas.First().Nombre;
                pa.ClavePunto = item.punto.clavepunto;
                pa.IdArea = item.punto.Areas.First().IdArea.ToString();
                pa.aceptada = item.aceptada;
                pa.noaceptada = item.status;
                pa.Comentario = item.comentario;
                lstPuntoArea.Add(pa);
            }

            if (filtro == "btnBusca")//Si se esta filtrando por medio del searchbox
            {
                lstPuntoArea = lstPuntoArea.Where(x => x.Punto.ToUpper().Contains(txtSearch.Text.ToUpper())).ToList();
            }
            else
            {
                if (cmbClave.Text != "" && cmbClave.Text != "Todas")
                {                    
                    lstPuntoArea = lstPuntoArea.Where(c => c.ClavePunto == cmbClave.Text).ToList();

                    if(lstPuntoArea.Count()>0)
                    {
                        cmbArea.SelectedIndex = cmbArea.Items.IndexOfText(lstPuntoArea.FirstOrDefault().Area);
                    }                    
                }
                else
                {
                    lstPuntoArea = lstPuntoArea.Where(x => x.Area == area.Nombre).ToList();//datasource
                }
            }
            
            
            ViewState["listaGrid"] = lstPuntoArea;
        }

        protected void cmbTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenagrid();
        }

        protected void cmbDist_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenagrid();
        }

        protected void cmbClave_SelectedIndexChanged(object sender, EventArgs e)
        {
            guardaSeleccion();
            llenagrid();
        }

        protected void cmbArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            cmbClave.SelectedIndex = 0;
            guardaSeleccion();
            cambiaLabel();
            llenagrid();
        }
        public void cambiaLabel()
        {
            lblArea.Text = cmbArea.SelectedItem.Text;
        }
        public void guardaSeleccion()            
        {
            DevExpress.Xpo.Session session = Util.getsession();
            //String clave = Request.Form["idAud"];
            string codigo = Session["amg"] as string;
          //  mdl_distribuidor dist = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist == cmbDist.SelectedItem.Text);//dist seleccionado en cmb
          //  mdl_audidet auddet = new XPQuery<mdl_audidet>(session).FirstOrDefault(x => x.Idaud == dist.UltimaAuditoria && x.Idpunto.clavepunto == clave);
            mdl_tipoauditoria tipo = new XPQuery<mdl_tipoauditoria>(session).FirstOrDefault(a => a.Descripcion == cmbTipo.SelectedItem.Text);//tipo seleccionado de aud en cmb

            //mdl_Area area = new XPQuery<mdl_Area>(session).FirstOrDefault(x => x.Nombre == cmbArea.SelectedItem.Text && x.Idtipoaud == tipo);//Area seleccionada
            mdl_Area area = new XPQuery<mdl_Area>(session).FirstOrDefault(x => x.Nombre == lblArea.Text && x.Idtipoaud == tipo);//Area seleccionada
            AuditoriaMovilGeneral amg = new XPQuery<AuditoriaMovilGeneral>(session).FirstOrDefault(x => x.IdAudMovG.ToString() == codigo);
            var audmov = amg.AudMDet.Where(x => x.punto.Areas.First() == area).ToList();


            amg.ValidadoPor = txtAutoriza.Text;
            amg.Save();
            foreach (var item in audmov)
            {
                String si = Request.Form["si" + item.punto.clavepunto];
                if (si=="si")
                {
                    item.aceptada = true;
                    item.status = false;
                }
                else if (si=="no")
                {
                    item.aceptada = false;
                    item.status = true;
                }

                //String no = Request.Form[item.punto.clavepunto];
                
                //if (si != null && si.Equals("on"))
                //{
                //    item.aceptada = true;
                //    item.status = false;
                //}
                //if (no != null && no.Equals("on"))
                //{
                //    item.aceptada = false;
                //    item.status = true;
                //}
                item.Save();
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();

            guardaSeleccion();

            String clave = Request.Form["idAud"];
            string codigo = Session["amg"] as string;
            mdl_distribuidor dist = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist == cmbDist.SelectedItem.Text);//dist seleccionado en cmb
            mdl_audidet auddet = new XPQuery<mdl_audidet>(session).FirstOrDefault(x => x.Idaud == dist.UltimaAuditoria && x.Idpunto.clavepunto == clave);

            Response.Redirect("~/auditoriamovildetalle.aspx?codigo=" + Cryptography.Encriptar(auddet.id.ToString()));
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();

            string clave = Request.Form["idAud"];
            string codigo = Session["amg"] as string;

            using (UnitOfWork uow = new UnitOfWork())
            {
                var audmov = new XPQuery<mdl_AuditoriaMovil>(session).FirstOrDefault(x => x.punto.clavepunto == clave && x.AuditoriaGeneral.IdAudMovG == int.Parse(codigo));
                audmov.status = false;
                audmov.aceptada = false;
                audmov.Save();
                uow.CommitChanges();
            }

            Response.Redirect("~/auditoria_movil.aspx?idamg=" +codigo);
        }

        public void sendMail()
        {

            string codigo = Session["amg"] as string;            
            AuditoriaMovilGeneral amg = new XPQuery<AuditoriaMovilGeneral>(session).FirstOrDefault(x => x.IdAudMovG.ToString() == codigo);
            /***status = no aceptada****/
            var lstRevisionNoaceptadas = amg.AudMDet.Where(c => c.status == true);

            string listaActividades = "";
            foreach (var item in lstRevisionNoaceptadas)
            {
                foreach (var act in item.listaActividades)
                {
                    listaActividades += act.Code + ", ";
                }
            }

            dist = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.iddistribuidor == amg.distribuidor.iddistribuidor);
            var emailEjecutivo = new XPQuery<Usuario>(session).FirstOrDefault(x => x.UserName == dist.nombredist).email;
            //MailMessage msg = new MailMessage(new MailAddress("mts-noreply@lth.com.mx", new MailAddress("com"));    //  Create a MailMessage object with a from and to address
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("mts-noreply@lth.com.mx", "Plataforma EDC II");
            string cuerpo = "<h4>Estimado Distribuidor</h4>" +
                            "<p>Han sido evaluadas las actividades: "+ listaActividades+" por el evaluador "+Util.getusuario()+ " con Fecha de " + DateTime.Now.ToShortDateString() + ".</p>" +
                            "<p>Favor de revisar el status de sus actividades en la opción PLAN DE NEGOCIO del menú de su plataforma EDC-II.</p>" +
                            "<p>Muchas Gracias.</p><hr/>";
            if (emailEjecutivo != null)
            {
                msg.To.Add(new MailAddress(emailEjecutivo));
            }
            else { msg.To.Add(new MailAddress("edgar.ramirez@holdasociados.com")); }
                   

            //msg.Bcc()
            msg.Subject = "EDC-II - Actividad Evaluada en Auditoría Móvil";  //  Add your subject

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

        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            guardaSeleccion();
            sendMail();
            Response.Redirect("~/EvaluacionMovil.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            guardaSeleccion();
            llenagrid("btnBusca");
        }
    }
}