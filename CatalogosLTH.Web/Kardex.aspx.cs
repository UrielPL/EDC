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
    public partial class Kardex : System.Web.UI.Page
    {
        public TipoUsuario userType { get; set; }
        public mdl_Kardex actualKardex { get; set; }
        public string idKardex { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            idKardex = Request.QueryString["idKardex"];
            actualKardex = new XPQuery<mdl_Kardex>(Util.getsession()).FirstOrDefault(x=>x.Oid.ToString() == idKardex);
            userType = Util.getPermiso();
            if ((int)userType != 4 || actualKardex.Cerrada)
            {
                btnActualizar1.Enabled = false;
                btnActualizar2.Enabled = false;
                txtAct.Enabled = false;
                txtAcu.Enabled = false;
                btnActualizar1.Visible = false;
                btnActualizar2.Visible = false;
                txtAct.Visible = false;
                txtAcu.Visible = false;
            }
        }

        protected void btnActualizar2_Click(object sender, EventArgs e)
        {
            mdl_Kardex temp = new XPQuery<mdl_Kardex>(Util.getsession()).FirstOrDefault(x => x.Oid.ToString() == idKardex);
            temp.AcuerdosRevision = txtAcu.Text;
            using (UnitOfWork uow = new UnitOfWork())
            {
                temp.Save();
                uow.CommitChanges();
            }
            txtAcu.Text = "";
            Response.Redirect("/Kardex.aspx?idKardex=" + idKardex);
        }

        protected void btnActualizar1_Click(object sender, EventArgs e)
        {
            mdl_Kardex temp = new XPQuery<mdl_Kardex>(Util.getsession()).FirstOrDefault(x=>x.Oid.ToString() == idKardex);
            temp.ActRevisadasEjec = txtAct.Text;
            using (UnitOfWork uow = new UnitOfWork())
            {
                temp.Save();
                uow.CommitChanges();
            }
            txtAct.Text = "";
            Response.Redirect("/Kardex.aspx?idKardex=" + idKardex);
        }

        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            mdl_Kardex temp = new XPQuery<mdl_Kardex>(Util.getsession()).FirstOrDefault(x => x.Oid.ToString() == idKardex);
            temp.Cerrada = true;
            using (UnitOfWork uow = new UnitOfWork())
            {
                temp.Save();
                uow.CommitChanges();
            }
            Response.Redirect("/Kardex.aspx?idKardex=" + idKardex);
        }
    }
}