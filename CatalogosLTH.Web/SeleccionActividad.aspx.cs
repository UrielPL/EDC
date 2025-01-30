using CatalogosLTH.Module.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Web;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogosLTH.Web
{
    public partial class SeleccionActividad : System.Web.UI.Page
    {
       public List<Archivos> listaArchivos;
        protected void Page_Load(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            var codigo = Request.QueryString["codigo"];
            string nombreActual = Util.getusuario();//Nombre usuario 

            if (codigo!=null)
            {
                codigo = Cryptography.Decripta(codigo.ToString());
            }
            mdl_auditoriaactividad audact = null;
            mdl_actividad activ = null;
            string idactividad = "";

            if (codigo != null)
            {
                audact = new XPQuery<mdl_auditoriaactividad>(session).FirstOrDefault(x => x.idactplan.ToString() == codigo);//CARGA AUDITORIAACTIVIDAD
                if (audact != null)
                {
                    lblTexto.Text = audact.idactplan.ToString(); 
                    lblCode.Text = "FECHA LIMITE: " + audact.fechafinal.ToShortDateString();
                    idactividad = audact.Idactividad.IdActividad.ToString();
                    lblDist.Text = "SUBIDO POR: " + audact.Idaud.Iddistribuidor.nombredist;
                }
                if (idactividad != "")
                {
                    activ = new XPQuery<mdl_actividad>(session).FirstOrDefault(x => x.IdActividad.ToString() == idactividad);//CARGA ACTIVIDAD
                    if (activ != null)
                    {
                        literalInstruccion.Text = activ.Instruccion;
                        ViewState["codigoActividad"] = activ.Code.ToString();
                        ViewState["nombreActividad"] = activ.Texto;
                    }
                }
                string usuario = Util.getrol();
                int nivelActividad = Convert.ToInt32(audact.Idactividad.NivelID.idnivel.ToString());
                int nivelActual = Util.getNivelActual(Convert.ToInt32(audact.Idaud.Iddistribuidor.iddistribuidor.ToString()));

                if ((audact.status == "En revisión" && usuario == "Distribuidor") || (audact.status == "Completada") || (nivelActividad > nivelActual)||((usuario!= "Distribuidor") &&(audact.Evaluador.UserName!=nombreActual)))//DESHABILITA BOTON DE SUBIR
                {
                    btnUpload.Enabled = false;
                    btnUpload.ToolTip = "Debes esperar a que la actividad sea revisada";
                    CheckAceptado.Enabled = false;
                    if (audact.status=="Completada")
                    {
                        CheckAceptado.Checked = true;                        
                    }
                } 


            }
            if (!IsPostBack)
            {
                string usuario = Util.getrol();

                Usuario us = (Usuario)SecuritySystem.CurrentUser;
                if (Util.tieneRol("Evaluador"))
                {
                    CheckAceptado.Visible = true;
                    btnConfirm.PostBackUrl = "~/actividadesrevisar.aspx";
                }
                if (Util.getrol() == "Evaluador")
                {
                    CheckAceptado.Visible = true;
                    btnConfirm.PostBackUrl = "~/actividadesrevisar.aspx";
                }

                //user = Session[""];



                llenaGrid(audact);
            }
            else
            {
                llenaGrid(audact);
            }


        }

        protected void btnUpload_Click(object sender, EventArgs e)//GUARDAR
        {
            string usuario = Session["Usuario"] as string;

            //GuardaActividad(Util.getusuario());
            GuardaActividad(Util.getrol());
        }

        public void llenaGrid(mdl_auditoriaactividad idaudact)
        {
            string idactplan = "";
            if (idaudact != null)
            {
                idactplan = idaudact.idactplan.ToString();
            }

            DevExpress.Xpo.Session session = Util.getsession();

            var archivos = new XPQuery<Archivos>(session).Where(x => x.IdAuditoriaActividad.idactplan.ToString() == idactplan).ToList();

             listaArchivos = archivos;
            listaArchivos = listaArchivos.OrderByDescending(x => x.fecha).ToList();
            //listaArchivos.OrderByDescending(x => x.fecha);

            var data = from ba in listaArchivos                       
                           // where ba.IdAuditoriaActividad.idactplan.ToString()==idactplan
                       select new
                       {
                           Oid=ba.Oid,
                           idact = ba.IdAuditoriaActividad.idactplan,
                           fecha = ba.fecha,
                           comentario = ba.comentario,
                           usuario = ba.usuario,
                           status = ba.substatus,
                           Archivo = ba.ArchivoImportar
                       };

//            data = data.OrderBy(x => x.fecha);
            data = data.OrderByDescending(x => x.fecha);
            

            ASPxGridView1.KeyFieldName = "Oid";
            ASPxGridView1.DataSource = data.ToList();
            ASPxGridView1.AutoGenerateColumns = true;
            ASPxGridView1.DataBind();

            foreach (GridViewDataColumn columna in ASPxGridView1.Columns)
            {
                string col2 = columna.FieldName.ToString();
                if (col2 == "Oid" ||col2=="idact")
                {
                    columna.Visible = false;
                }

            }

        }

        public void GuardaActividad(string usuario)
        {
            DevExpress.Xpo.Session session = Util.getsession();//consigue la sesion            
            XpoDefault.DataLayer = session.DataLayer;
            List<string> correos = new List<string>();

            TextBox1.Enabled = false;

            string correoEvaluador = "";
            string correoDist = "";
            string nuevoStatus = "";         



            mdl_auditoriaactividad audact = null;//TRAE AUDITORIAACTIVIDAD  
            if (lblTexto.Text != "")
            {
                audact = new XPQuery<mdl_auditoriaactividad>(session).FirstOrDefault(x => x.idactplan.ToString() == lblTexto.Text);
                if (audact != null)
                {
                    lblTexto.Text = audact.idactplan.ToString(); ;
                    lblCode.Text = audact.fechafinal.ToShortDateString();
                }
            }

          Usuario userDist = new XPQuery<Usuario>(session).FirstOrDefault(x => x.UserName.ToString() == audact.Idaud.Iddistribuidor.nombredist.ToString());//Usuario distribuidor


            //SE CARGAN LAS LISTAS DE LOS CORREOS
            string mailEvaluador = audact.Evaluador.email;
            int cont = 0;
            //if (mailEvaluador != null) { correos.Add(mailEvaluador); }
            if (userDist.Jefe != null)
            {
                if (userDist.Jefe.email != null)
                {
                    correos.Add(userDist.Jefe.email);//Agrega correo de Gerente de Cuenta

                }
                if (userDist.Jefe.Jefe != null)
                {
                    if (userDist.Jefe.Jefe.email != null)
                    {
                        correos.Add(userDist.Jefe.Jefe.email);//Agrega correo de Gerente de Desarrollo Comercial                       
                    }
                }
            }

//SE CARGA EL NUEVO STATUS
            if (CheckAceptado.Checked && audact != null)//Aceptado GUARDA LA ACTIVIDAD
            {
                nuevoStatus = "Completada";
                //**Envío de correo
                string body = Util.correoActividadAceptada(audact.Idactividad.Code, audact.Evaluador.Nombre);
                List<string> dist = new List<string>();
                dist.Add(userDist.email);

               Util.SendMail(userDist.email, "EDC-II - Actividad Aceptada EDCII", body, dist, ""); 
                
                //**
            }
            else
            {
                if (usuario == "Evaluador")
                {
                    nuevoStatus = "Por realizar";
                    try
                    {
                        string mailDistribuidor = userDist.email;
                        if (mailDistribuidor != null)
                        {
                            string body = Util.correoActividadRevisada(Util.getusuario(), audact.Idactividad.Code + " " + mailDistribuidor, audact.Evaluador.Nombre);
                           //descomentar Util.SendMail(mailDistribuidor, "EDC-II - Actividad Evaluada EDCII", body, correos, "");//correoDist as todos 
                        }
                    }
                    catch(Exception e)
                    { List < string > ec= new List<string>();
                        ec.Add("ed.rmzlpz@gmail.com");
                        Util.SendMail("", "Errorazo", e.Message, ec, "");//correoDist as todos
                    }
                }
                else
                {
                    nuevoStatus = "En revisión";
                    if (mailEvaluador != null)
                    {
                        List<string> ev = new List<string>();
                        ev.Add(mailEvaluador);
                        
                        string body = Util.correoActividadSubida(Util.getusuario(), audact.Idactividad.Code + "  " + mailEvaluador + "  ", audact.Evaluador.Nombre);
                        if (ev.Count > 0)
                        {
                            Util.SendMail(mailEvaluador, "EDC-II - Actividad por evaluar EDCII", body, ev, "");
                        }
                        else Response.Write("No hay mail de evaluador");

                        if (correos.Count > 0)
                        {
                            string body2 = Util.correoAvisaGerente(Util.getusuario(), audact.Idactividad.Code, audact.Evaluador.Nombre);
                            Util.SendMail(mailEvaluador, "EDC-II - Actividad enviada a revisión por su Distribuidor", body2, correos, "");
                        }
                        else Response.Write("No hay mail de Gerente de cuenta");
                    }
                }
            }
            
            

            //-------------------------
            try
            {
                DevExpress.Persistent.BaseImpl.FileData dataF = null;
                string clav = "";
                dataF = new DevExpress.Persistent.BaseImpl.FileData(session);
                if (FileUpload1.HasFile)
                {
                    var fileStream = FileUpload1.FileContent;
                    dataF.LoadFromStream(FileUpload1.FileName, fileStream);
                }

                using (UnitOfWork uow = new UnitOfWork())
                {
                    Archivos up = new Archivos(session);
                    up.comentario = TextBox1.Text;

                    if (dataF != null) { up.ArchivoImportar = dataF; }//Se agrega archivo

                    up.usuario = usuario;

                    if (audact != null) { up.IdAuditoriaActividad = audact; }
                    string status = audact.status.ToString();

                    up.substatus = nuevoStatus;
                    up.Save();
                    uow.CommitChanges();
                    clav = up.Oid.ToString();
                }


                if (FileUpload1.HasFile)
                {
                    using (UnitOfWork uow = new UnitOfWork())
                    {
                        Archivos up = new XPQuery<Archivos>(session).FirstOrDefault(x => x.Oid.ToString() == clav);
                        if (up.ArchivoImportar != null)
                        {                            
                            up.ArchivoImportar.FileName = clav + up.ArchivoImportar.FileName;
                        }
                        up.Save();
                        uow.CommitChanges();
                    }

                    //Guardar en disco duro
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + "\\archivos\\Evidencia\\";
                    string nombre = dataF.FileName;

                    FileStream fileStream1 = System.IO.File.Create(ruta + nombre);

                    dataF.SaveToStream(fileStream1);
                    fileStream1.Close();
                    Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                }
            }
            catch (Exception e)
            {
                List<string> ls = new List<string>();
                ls.Add("ed.rmzlpz@gmail.com");
                Util.SendMail("ed.rmzlpz@gmail.com", "error " + audact.Idaud + "-" + audact.Idactividad, e.Message, ls, "");
            }
            //*************************/

            

            if (CheckAceptado.Checked && audact != null)//Aceptado GUARDA LA ACTIVIDAD
            {
            
                using (UnitOfWork uow3 = new UnitOfWork())
                {
                    //**Cambia estado de la actividad
                        audact.fechacomp = DateTime.Now;
                        audact.status = nuevoStatus;
                        audact.Save();
                        uow3.CommitChanges();
                    //**
                }

                Util.ActualizarNivelProfesionalizacion(audact.Idaud.Iddistribuidor.iddistribuidor);
                Util.actualizaRegistroMensual(userDist.UserName, audact);                         
            }
            else//NO ACEPTADA sin check
            {
                using (UnitOfWork uow2 = new UnitOfWork())
                {                   
                    audact.status = nuevoStatus;
                    audact.fechaEnvio = DateTime.Now;                    
                    audact.Save();
                    uow2.CommitChanges();
                }
            }//GUARDA LA ACTIVIDAD

          
            // }

            llenaGrid(audact);
            btnUpload.Enabled = false;


        }



        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        

        
        [WebMethod]
        public static byte[] getfile(string id)
        {
            DevExpress.Xpo.Session session = Util.getsession();//consigue la sesion            
            XpoDefault.DataLayer = session.DataLayer;
            //   int indice = ASPxGridView1.FocusedRowIndex;
            //   string idArchivo = ASPxGridView1.GetRowValues(indice, ASPxGridView1.KeyFieldName).ToString();
            //     Archivos archivos = new XPQuery<Archivos>(session).Where(x => x.Oid.ToString() == idArchivo).First();
            Archivos archivos = new XPQuery<Archivos>(session).Where(x => x.Oid.ToString() == id).First();
            FileData oArchivo = archivos.ArchivoImportar;
            byte[] b1 = null;
            if (oArchivo != null)
            {
                MemoryStream ms = new MemoryStream();
                oArchivo.SaveToStream(ms);

                string sNombre = oArchivo.FileName;
                
                //HttpContext context = HttpContext.Current;
                //context.Response.Clear();
                //context.Response.Buffer = true;
                //context.Response.Charset = "";
                //context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //context.Response.ContentType = "application/octet-stream";
                //context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + "\"" + sNombre.Replace(" ", "") + "\"");
                //context.Response.BinaryWrite(ms.ToArray());
                //context.Response.Flush();
                //context.Response.End();

                System.IO.FileStream fs1 = null;
                //fs1 = System.IO.File.Open(oArchivo.FileName, FileMode.Open, FileAccess.Read);
                //b1 = new byte[fs1.Length];
                b1 = ms.ToArray();
                //fs1.Read(b1, 0, (int)fs1.Length);
                //fs1.Close();
                return b1;
            }



            return b1;
        }

        [WebMethod]
        public static string getnombre(string id)
        {
            string sNombre = "";
            DevExpress.Xpo.Session session = Util.getsession();//consigue la sesion            
            XpoDefault.DataLayer = session.DataLayer;
            //   int indice = ASPxGridView1.FocusedRowIndex;
            //   string idArchivo = ASPxGridView1.GetRowValues(indice, ASPxGridView1.KeyFieldName).ToString();
            //     Archivos archivos = new XPQuery<Archivos>(session).Where(x => x.Oid.ToString() == idArchivo).First();
            Archivos archivos = new XPQuery<Archivos>(session).Where(x => x.Oid.ToString() == id).First();
            FileData oArchivo = archivos.ArchivoImportar;
            byte[] b1 = null;
            if (oArchivo != null)
            {
                MemoryStream ms = new MemoryStream();
                oArchivo.SaveToStream(ms);

                sNombre = oArchivo.FileName;

           }



            return sNombre;
        }

        protected void CheckAceptado_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckAceptado.Checked)
            {
                CheckAceptado.Text = "Aceptado";
            }
            else
            {
                CheckAceptado.Text = "No Aceptado";
            }
        }

        protected void ASPxButton2_Click(object sender, EventArgs e)
        {
            ASPxButton btn = (ASPxButton)sender;
            string cn = btn.CommandName.ToString();
            string ca = btn.CommandArgument.ToString();
        }
    }
}
