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
    public partial class DetalleDocumento : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            string iddoc = Request.QueryString["iddoc"];
            ViewState["iddoc"] = iddoc;
            if (!string.IsNullOrEmpty(iddoc))
            {
                Documento dc = new XPQuery<Documento>(session).FirstOrDefault(x => x.Oid.ToString() == iddoc);
                literalInstruccion.Text = dc.Descripcion;
                lblid.Text = iddoc.ToString();
                ViewState["codigoActividad"] = dc.Nombre;
            }
        }

        protected void Muestra1_Click(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            string iddoc = ViewState["iddoc"] as string;
            Documento dc = new XPQuery<Documento>(session).FirstOrDefault(x => x.Oid.ToString() == iddoc);
            int pos = dc.Muestra1.FileName.IndexOf('.');
            string ext=dc.Muestra1.FileName.Substring(pos, (dc.Muestra1.FileName.Length - pos));
            
            string ArchivoMuestra = "/Archivos/" + dc.Nombre + "Documento"+ext;
            if (dc.Muestra1 != null)
            {
                string filePath = ArchivoMuestra;
                Response.ContentType = "doc/docx";
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + "Muestra1"+ext + "\"");
                Response.TransmitFile(Server.MapPath(".") + filePath);
                Response.End();
            }
        }
    }
}