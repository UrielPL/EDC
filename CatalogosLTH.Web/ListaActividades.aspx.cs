using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogosLTH.Web
{
    public partial class ListaActividades : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (IsCallback)
            {
              //  ASPxGridView1.DataBind();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
     
            if(!IsPostBack)
            {   DevExpress.Xpo.Session session = Util.getsession();
                string permiso = Util.getPermiso().ToString();
                string nombreActual = Util.getusuario();


                //LLENA DROPDOWN LIST DE NIVELES
                var niveles = new XPQuery<mdl_nivel>(session);
                foreach (var item in niveles)
                {
                    dropNivel.Items.Add(item.nombreniv.ToString());
                }
                dropNivel.Items.Add("Todos");
                List<Usuario> listaDistribuidores = new List<Usuario>();
              //  XPCollection<Usuario> dist = new XPCollection<Usuario>(session);


                if (Util.getrol() == "DC Administrativo"){permiso = "DCAdmin";}
                switch (permiso)
                {
                    case "GerenteCuenta":
                        listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.Jefe.UserName == nombreActual).ToList();
                        break;
                    case "GerenteDesarrolloComercial":
                        listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.Jefe.Jefe.UserName == nombreActual).ToList();
                        //listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.Jefe.UserName == nombreActual).ToList();
                        break;
                    case "Distribuidor":
                        listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.UserName == nombreActual).ToList();
                        break;
                    case "GerenteVenta":
                        Usuario u = new XPQuery<Usuario>(session).FirstOrDefault(x => x.UserName == nombreActual);
                        string z = u.ZonaPertenece.zona;
                        listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.ZonaPertenece.zona == z && x.TipoUsuario == TipoUsuario.Distribuidor).ToList();
                        break;
                    case "Admin":
                        listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.TipoUsuario == TipoUsuario.Distribuidor).ToList();                        
                        break;
                    case "Evaluador":
                        if(nombreActual == "alonso.sierra.siller@clarios.com")
                            listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.TipoUsuario == TipoUsuario.Distribuidor).ToList();
                        break;

                    case "DCAdmin":
                        listaDistribuidores = new XPQuery<Usuario>(session)
                            .FirstOrDefault(x => x.UserName == nombreActual)
                            .DistribuidoresSupervisa.ToList();
                      
                        break;
                }            

                
                listaDistribuidores = listaDistribuidores.OrderBy(x => x.UserName).ToList();

                foreach (var itemDist in listaDistribuidores)
                {
                    dropDist.Items.Add(itemDist.UserName.ToString());
                }

                if (permiso == "Admin" || Util.alsoAdmin()){ dropDist.Items.Add("Todos"); }              
              
                    dropDist.SelectedIndex = 0;                           

            }
            
                llenaGrid();

            Exporter1.GridViewID = "ASPxGridView1";
            
            //ridExport.WriteXlsxToResponse(new XlsxExportOptionsEx { ExportType = ExportType.WYSIWYG });


        }

        public void llenaGrid()
        {
            DevExpress.Xpo.Session session = Util.getsession();

            XPQuery<mdl_auditoriaactividad> audacts = session.Query<mdl_auditoriaactividad>();
            XPQuery<mdl_auditoria> auditorias = session.Query<mdl_auditoria>();
            XPQuery<mdl_actividad> actividades = session.Query<mdl_actividad>();

            int idnivel = 1;
            int iddist = 62;
            if (dropNivel != null && dropNivel.Text != "")
            {
                if (dropNivel.Text!="Todos")
                {
                    mdl_nivel niveles = new XPQuery<mdl_nivel>(session).FirstOrDefault(x => x.nombreniv.ToString() == dropNivel.SelectedItem.ToString());
                    idnivel = niveles.idnivel;
                }
                else
                {
                    idnivel = 0;
                }
            }
            if (dropDist != null && dropDist.Text != "")
            {
                if (dropDist.Text!="Todos")
                {
                    mdl_distribuidor dist = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist.ToString() == dropDist.SelectedItem.ToString());
                    iddist = dist.iddistribuidor;
                }
                else
                {
                    iddist = 0;
                }
            }

            int nivelActual=Util.getNivelActual(iddist);
            if (idnivel > nivelActual)
            {
                lblMensaje.Text = "No podrás realizar actividades de este nivel si no completas las actividades del nivel anterior";
            }
            else
            {
                lblMensaje.Text = "";
            }
            int auditoriasRevisa = Util.getUltimaAuditoria(iddist);


      


            if (idnivel != 0 && iddist!=0)
            {
                var data = from aa in audacts
                           join ac in actividades on aa.Idactividad equals ac
                           join au in auditorias on aa.Idaud equals au
                           where au.idaud == auditoriasRevisa && ac.NivelID.idnivel == idnivel && aa.fechacomp == null
                           select new { ac.Code,
                               Actividad = ac.Texto,
                                   au.Idtipoaud,
                                   aa.idactplan,
                                   aa.Idaud,
                                   aa.Idactividad,
                                   aa.secuencia,
                               Duración =aa.duracion,
                               FechaInicio = aa.fechainicio,
                               FechaFinal = aa.fechafinal,
                                   aa.fechacomp,
                               Estatus =aa.status,
                               Nivel = ac.NivelID.nombreniv,
                               Evaluador = aa.Evaluador.Nombre,
                               Distribuidor = au.Iddistribuidor.nombredist,
                               Correo = aa.Evaluador.UserName.ToString(),
                               Pilar = ac.PilarID.nombrepil,
                               Estatus2 = aa.comentario
                           };

                ASPxGridView1.DataSource = data.ToList();
            }
            else if(idnivel==0&&iddist!=0)
            {
                var data = from aa in audacts
                           join ac in actividades on aa.Idactividad equals ac
                           join au in auditorias on aa.Idaud equals au
                           where au.idaud == auditoriasRevisa && aa.fechacomp == null
                           select new
                           {
                               ac.Code,
                               Actividad = ac.Texto,
                               au.Idtipoaud,
                               aa.idactplan,
                               aa.Idaud,
                               aa.Idactividad,
                               aa.secuencia,
                               Duración = aa.duracion,
                               FechaInicio = aa.fechainicio,
                               FechaFinal = aa.fechafinal,
                               aa.fechacomp,
                               Estatus = aa.status,
                               Nivel = ac.NivelID.nombreniv,
                               Evaluador = aa.Evaluador.Nombre,
                               Distribuidor = au.Iddistribuidor.nombredist,
                               Correo = aa.Evaluador.UserName.ToString(),
                               Pilar = ac.PilarID.nombrepil,
                               Estatus2 = aa.comentario
                           };

                ASPxGridView1.DataSource = data.ToList();
            }
            else if (iddist==0)//todos los distribuidores
            {
                var listaDist = new XPQuery<mdl_distribuidor>(session);
                List<mdl_auditoria> ultimasAuitorias = new List<mdl_auditoria>();
                List<mdl_actividad> lstActividades = new List<mdl_actividad>();
                List<mdl_auditoriaactividad> lstAudAct = new List<mdl_auditoriaactividad>();
                
                foreach (var item in listaDist)
                {
                    ultimasAuitorias.Add(item.UltimaAuditoria);
                }

                int cantdrop = dropNivel.Items.Count;
                lstActividades = actividades.ToList();
                if (idnivel != 0)
                {
                    lstActividades = actividades.Where(c => c.NivelID.idnivel == idnivel).ToList();
                }
                
                
                lstAudAct = audacts.ToList();
                
                lstAudAct = lstAudAct.Where(x => x.fechacomp.GetHashCode() == 0).ToList();
                

                //ALL DIST NOT ALL LEVELS
               
                    var data2 = from au in ultimasAuitorias
                                join aa in lstAudAct on au equals aa.Idaud
                                join ac in lstActividades on aa.Idactividad equals ac
                                select new
                                {
                                    ac.Code,
                                    Actividad = ac.Texto,
                                    au.Idtipoaud,
                                    aa.idactplan,
                                    aa.Idaud,
                                    aa.Idactividad,
                                    aa.secuencia,
                                    Duración = aa.duracion,
                                    FechaInicio = aa.fechainicio,
                                    FechaFinal = aa.fechafinal,
                                    aa.fechacomp,
                                    Estatus = aa.status,
                                    Nivel = (ac.NivelID.nombreniv != null || ac.NivelID.nombreniv != null) ? ac.NivelID.nombreniv : "",
                                    Evaluador = (aa.Evaluador != null) ? aa.Evaluador.Nombre : "",
                                    Distribuidor = (au.Iddistribuidor != null) ? au.Iddistribuidor.nombredist : "",
                                    Correo = (aa.Evaluador != null) ? aa.Evaluador.UserName.ToString() : "",
                                    Pilar = (ac.PilarID != null) ? ac.PilarID.nombrepil : "",
                                    Estatus2 = aa.comentario
                                };
                    ASPxGridView1.DataSource = data2.ToList();
               

                
            }


            ASPxGridView1.AutoGenerateColumns = true;
            ASPxGridView1.KeyFieldName = "idactplan";
            ASPxGridView1.DataBind();
            ASPxGridView1.EnableRowsCache = false;
            
            
            foreach (GridViewDataColumn columna in ASPxGridView1.Columns)
            {
                string col2 = columna.FieldName.ToString();
                if (col2=="idactplan"||col2=="Idtipoaud"||col2=="Idaud" || col2 == "Idactividad"||col2=="secuencia"||col2== "fechacomp")
                {
                    columna.Visible = false;
                }

            }

           // myConnection.Close();   
        }

        protected void dropNivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenaGrid();
        }

        protected void dropDist_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenaGrid();
        }
        protected void ASPxGridView1_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName != "Estatus2") return;
            DateTime enteredDate = new DateTime();
            DateTime hoy = DateTime.Now;
            string fecha = e.GetFieldValue("FechaFinal").ToString();
            enteredDate = DateTime.Parse(fecha);
            int y = enteredDate.CompareTo(hoy);

            double semanas = (enteredDate - hoy).TotalDays / 7;

            if (y < 0)
            {
               e.DisplayText = "Vencida";
            }
            else 
            {
                e.DisplayText = "En tiempo";
            }
           
        }
        private static string password = "sdfjhsldjfk";
      

        
        [WebMethod]
        public static string Encriptar(string plainText)
        {  

                if (Cryptography.Key == null)
            {
                Cryptography.GenerateKey(password);
            }
 
            byte[] data = new ASCIIEncoding().GetBytes(plainText);
 
            RijndaelManaged crypto = new RijndaelManaged();

            crypto.Padding = PaddingMode.ISO10126;

            ICryptoTransform encryptor = crypto.CreateEncryptor(Cryptography.Key, Cryptography.Vector);
 
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream crptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
 
            crptoStream.Write(data, 0, data.Length);
            crptoStream.FlushFinalBlock();
 
            crptoStream.Close();
            memoryStream.Close();
 
            return Cryptography.ConvertToHex(Convert.ToBase64String(memoryStream.ToArray()));
    
        }

        protected void ASPxGridView1_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            
            string columna = e.DataColumn.FieldName;
          
            if (columna != "FechaFinal"&&columna!="Estatus2" ) return;
            DateTime enteredDate = new DateTime();
            DateTime hoy = DateTime.Now;
                if (columna == "Estatus2")
                {
                    string fecha = e.GetValue("FechaFinal").ToString();
                    enteredDate = DateTime.Parse(fecha);
                }
                if (columna=="FechaFinal")
                {
                    enteredDate = DateTime.Parse(e.CellValue.ToString());
                }
            int x = hoy.CompareTo(enteredDate);
            int y = enteredDate.CompareTo(hoy);

            double semanas = (enteredDate - hoy).TotalDays / 7;

            if (y < 0)
            {
                if (columna=="FechaFinal")
                {
                    e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                    e.Cell.ForeColor = System.Drawing.Color.White;
                    e.Cell.BorderColor = System.Drawing.Color.White;
                    e.Cell.BorderStyle = BorderStyle.Solid;
                    e.Cell.Font.Bold = true;
                }
                else if (columna=="Estatus2")
                {
                    e.Cell.Text = "Vencida";
                }
                
            }
            else if (semanas >= 2)
            {
                if (columna == "FechaFinal")
                {
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                    e.Cell.ForeColor = System.Drawing.Color.White;
                    e.Cell.BorderColor = System.Drawing.Color.White;
                    e.Cell.BorderStyle = BorderStyle.Solid;
                    e.Cell.Font.Bold = true;
                }
                else if (columna=="Estatus2")
                {
                    e.Cell.Text = "En tiempo";
                }
                
            }
            else if (semanas < 2 && semanas >= 0)
            {
                if (columna == "FechaFinal")
                {
                    e.Cell.BackColor = System.Drawing.Color.Orange;
                e.Cell.ForeColor = System.Drawing.Color.White;
                e.Cell.BorderColor = System.Drawing.Color.White;
                e.Cell.BorderStyle = BorderStyle.Solid;
                e.Cell.Font.Bold = true;
                }
                 else if (columna == "Estatus2")
                {
                    e.Cell.Text = "En tiempo";
                }
            }
         }

        protected void ASPxButton1_Click(object sender, EventArgs e)
        {
            int indice = ASPxGridView1.FocusedRowIndex;
            string codigo = ASPxGridView1.GetRowValues(indice, new string[] { "idactplan" }).ToString();          
            Response.Redirect("~/SeleccionActividad.aspx?codigo=" + Cryptography.Encriptar(codigo));            
        }

        protected void Excel(object sender, EventArgs e)
        {
            Exporter1.WriteXlsxToResponse(new XlsxExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }

        protected void Exporter1_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {

            if (e.RowType  !=GridViewRowType.Data)
                return;

            string columna = (e.Column as GridViewDataColumn).FieldName;

            if (columna != "FechaFinal" && columna != "Estatus2") return;
            DateTime enteredDate = new DateTime();
            DateTime hoy = DateTime.Now;
            if (columna == "Estatus2")
            {
                string fecha = e.GetValue("FechaFinal").ToString();
                enteredDate = DateTime.Parse(fecha);
            }
            if (columna == "FechaFinal")
            {
                if (e.TextValue.ToString()!="")
                {
                    enteredDate = DateTime.Parse(e.TextValue.ToString());
                }
                
            }
            int x = hoy.CompareTo(enteredDate);
            int y = enteredDate.CompareTo(hoy);

            double semanas = (enteredDate - hoy).TotalDays / 7;

            if (y < 0)
            {
                if (columna == "FechaFinal")
                {
                    e.BrickStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                    e.BrickStyle.ForeColor = System.Drawing.Color.White;
                    e.BrickStyle.BorderColor = System.Drawing.Color.White;
                    //e.Cell.BorderStyle = BorderStyle.Solid;
                    //e.Cell.Font.Bold = true;
                }
                else if (columna == "Estatus2")
                {
                    e.TextValue = "Vencida";
                }

            }
            else if (semanas >= 2)
            {
                if (columna == "FechaFinal")
                {
                    e.BrickStyle.BackColor = System.Drawing.Color.LightGreen;
                    e.BrickStyle.ForeColor = System.Drawing.Color.White;
                    e.BrickStyle.BorderColor = System.Drawing.Color.White;
                   // e.Cell.BorderStyle = BorderStyle.Solid;
                    //e.Cell.Font.Bold = true;
                }
                else if (columna == "Estatus2")
                {
                    e.TextValue = "En tiempo";
                }

            }
            else if (semanas < 2 && semanas >= 0)
            {
                if (columna == "FechaFinal")
                {
                    //e.Cell.BackColor = System.Drawing.Color.Orange;
                    e.BrickStyle.BackColor= System.Drawing.Color.Orange;
                    e.BrickStyle.ForeColor = System.Drawing.Color.White;
                    e.BrickStyle.BorderColor = System.Drawing.Color.White;
                    //e.BrickStyle.BorderStyle = BorderStyle.Solid;
                    //e.BrickStyle.Font.Bold = true;
                }
                else if (columna == "Estatus2")
                {
                    e.TextValue = "En tiempo";
                }
            }
           /* if ((e.Column as GridViewDataColumn).FieldName == "CategoryName")
            {
                e.BrickStyle.BackColor = System.Drawing.Color.Yellow;
                e.BrickStyle.ForeColor = System.Drawing.Color.Red;
            }
            if ((e.Column as GridViewDataColumn).FieldName == "CategoryID")
            {
                e.TextValue = Convert.ToInt32(e.Value) + 20;
            }-*/

        }


    }

    public class elementosGrid
    {
       public string Code { get; set; }
        public string Actividad { get; set; }
        public mdl_tipoauditoria tipoaud { get; set; }
        public int CodigoAct { get; set; }
        public mdl_auditoria IdAuditoria { get; set; }
        public mdl_actividad IdActividad { get; set; }
        public int Secuencia { get; set; }
        public int Duracion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public DateTime FechaCompletada { get; set; }
        public string Comentario { get; set; }
        public string Status { get; set; }
        public string Nivel { get; set; }
        public string Evaluador { get; set; }
        public string Distribuidor { get; set; }
        public string Correo { get; set; }
        public string Pilar { get; set; }
        
    }
       
}