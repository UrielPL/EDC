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
    public partial class DocumentosRelacionados : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DevExpress.Xpo.Session session =Util.getsession();
                var categorias = new XPQuery<CategoriaDocumento>(session);
                foreach (CategoriaDocumento item in categorias)
                {
                    cmbCategorias.Items.Add(item.Nombre);
                }
                cmbCategorias.Items.Add("Todos");
                cmbCategorias.SelectedIndex = cmbCategorias.Items.Count - 1;

                llenagrid();
            }

        }
        public void llenagrid()
        {
            if (cmbCategorias.SelectedItem.Text == "Todos")
            {
                XPQuery<Documento> docs = new XPQuery<Documento>(session);
                grid.DataSource = docs;
            }
            else
            {
                var docs = new XPQuery<Documento>(session).Where(x => x.Categoria.Nombre == cmbCategorias.SelectedItem.Text);
                grid.DataSource = docs;
            }
            grid.DataBind();
        }
        public DevExpress.Xpo.Session session = Util.getsession();

        protected void cmbCategorias_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenagrid();
        }
    }
}