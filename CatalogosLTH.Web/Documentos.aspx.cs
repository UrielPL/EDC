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
    public partial class Documentos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
                DevExpress.Xpo.Session session = Util.getsession();
                string idCat = Request.QueryString["idcat"];
                CategoriaDocumento cat = new XPQuery<CategoriaDocumento>(session).FirstOrDefault(x => x.Oid.ToString() == idCat);
             //   var categorias = new XPQuery<CategoriaDocumento>(session);
                var docs = new XPQuery<Documento>(session).Where(x=>x.Categoria==cat).ToList();

                var d = from au in docs
                        select new
                        {
                            Clave=au.Oid.ToString(),
                            Categoría = au.Categoria.Nombre,
                            Documento = au.Nombre
                           };

                //data = data.OrderByDescending(x => x.fecha);
                d = d.OrderBy(x => x.Categoría);
                grdDocs.DataSource = d.ToList();
                grdDocs.AutoGenerateColumns = true;
                grdDocs.KeyFieldName = "Clave";
                grdDocs.EnableRowsCache = true;
                grdDocs.DataBind();
            
        }

        protected void ASPxButton1_Click(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            int indice = grdDocs.FocusedRowIndex;
            string codigo = grdDocs.GetRowValues(indice, new string[] { "Categoría" }).ToString();
            string documento = grdDocs.GetRowValues(indice, new string[] { "Documento" }).ToString();
            var key = new XPQuery<Documento>(session).FirstOrDefault(x => x.Categoria.Nombre.Equals(codigo) && x.Nombre.Equals(documento)).Oid;
            Response.Redirect("~/DetalleDocumento.aspx?iddoc=" + key);

        }
    }
}