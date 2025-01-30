using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogosLTH.Web
{
    public partial class DetalleActividad : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            string idActividad = Request.QueryString["idactividad"];
            ViewState["idactividad"] = idActividad;
            if (idActividad!=null||idActividad!="")
            {
                mdl_actividad act = new XPQuery<mdl_actividad>(session).FirstOrDefault(x => x.IdActividad.ToString() == idActividad.ToString());
                literalInstruccion.Text = act.Instruccion;
                lblid.Text = idActividad.ToString();
                ViewState["codigoActividad"] = act.Code;
            }
            
        }

        protected void Muestra2_Click(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            string idact = ViewState["idactividad"] as string;
            mdl_actividad act = new XPQuery<mdl_actividad>(session).FirstOrDefault(x => x.IdActividad.ToString() == idact);
            string ArchivoMuestra = "/Archivos/"+ lblid.Text+"Muestra2.docx";

            if (act.Muestra2 != null)        
            {
                string filePath = ArchivoMuestra;            
                Response.ContentType = "doc/docx";
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "Muestra2.docx" + "\"");              
                Response.TransmitFile(Server.MapPath(".")+filePath);             
                Response.End();         
            }                    
        }

        protected void Muestra1_Click(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            string idact = ViewState["idactividad"] as string;
            mdl_actividad act = new XPQuery<mdl_actividad>(session).FirstOrDefault(x => x.IdActividad.ToString() == idact);

            string ArchivoMuestra = "/Archivos/" + lblid.Text + "Muestra1.docx";
            if (act.Muestra1 != null)
            {
                string filePath = ArchivoMuestra;
                Response.ContentType = "doc/docx";
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "Muestra1.docx" + "\"");
                Response.TransmitFile(Server.MapPath(".")+filePath);
                Response.End();
            }
        }
    }
}