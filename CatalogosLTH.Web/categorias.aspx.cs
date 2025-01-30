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
    public partial class categorias : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            var categorias = new XPQuery<CategoriaDocumento>(session);

            var c= from cat in categorias
                  select new
                  {
                      Clave = cat.Oid,
                      Categoría = cat.Nombre
                  };

            c = c.OrderBy(x => x.Clave);
            grdCats.DataSource = c.ToList();
            grdCats.AutoGenerateColumns = true;
            grdCats.KeyFieldName = "Clave";
            grdCats.EnableRowsCache = true;
            grdCats.DataBind();
        }
        //data = data.OrderByDescending(x => x.fecha);
      
        

        protected void ASPxButton1_Click(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            int indice = grdCats.FocusedRowIndex;
            string codigo = grdCats.GetRowValues(indice, new string[] { "Clave" }).ToString();
            
            Response.Redirect("~/documentos.aspx?idcat=" + codigo);
        }
    }
}