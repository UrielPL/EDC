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
    public partial class auditoriaCalii : System.Web.UI.Page
    {
        //public XPQuery<v_Pilar> pilares { get; set; }
        public List<v_Pilar> vpilar { get; set; }
        public List<v_Area> varea { get; set; }
        public List<v_Subtema> vsubtema { get; set; }
        public List<v_Punto> vpunto { get; set; }
        public List<mdl_distribuidor> LstDistribuidores { get; set; }
        public List<_dtoPunto> puntosDetalle { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            
                DevExpress.Xpo.Session session = Util.getsession();


                vpilar = new XPCollection<v_Pilar>(session).ToList();
                puntosDetalle = new List<_dtoPunto>();
                foreach (var pilar in vpilar)
                {
                    foreach (var area in pilar.Areas)
                    {
                        foreach (var subtema in area.Subtemas)
                        {
                            foreach (var punto in subtema.Puntos)
                            {
                                _dtoPunto p = new _dtoPunto();
                                p.idpilar = pilar.Id;
                                p.idarea = area.Id;
                                p.idsubtema = subtema.Id;
                                p.idpunto = punto.Id;
                                p.nombrepunto = punto.NombrePunto;
                                puntosDetalle.Add(p);
                            }
                        }
                    }
                }

                //pilares = session.Query<v_Pilar>();

                //XPQuery<mdl_distribuidor> distribuidores = session.Query<mdl_distribuidor>();
                //var sqlDi = from di in distribuidores
                //            orderby di.nombredist
                //            select di;
                //cmbDistribuidor.DataSource = sqlDi;
                //cmbDistribuidor.DataTextField = "nombredist";
                //cmbDistribuidor.DataValueField = "iddistribuidor";
                //cmbDistribuidor.DataBind();

                LstDistribuidores = new XPQuery<mdl_distribuidor>(session).ToList();
                LstDistribuidores = LstDistribuidores.OrderBy(b => b.nombredist).ToList();
                foreach (var item in LstDistribuidores)
                {
                    cmbDistribuidor.Items.Add(item.nombredist);
                }
            
        }

        protected void btnGuarda_Click(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();

            var xdist = cmbDistribuidor.SelectedItem.Text;
            mdl_distribuidor md = new XPQuery<mdl_distribuidor>(session).Where(c => c.nombredist == xdist).First();
            NuevaAuditoria na = new NuevaAuditoria(session);
            na.Fecha = DateTime.Today;
            na.Distribuidor = md;
            na.Save();

            foreach (var pt in puntosDetalle)
            {
                bool valor = false;
                String grp = Request.Form["califa" + pt.idpunto.ToString()];
                if (grp != null && grp.Equals("on"))
                    valor = true;

                v_Punto p = new XPQuery<v_Punto>(session).FirstOrDefault(x => x.Id == pt.idpunto);
                NuevaAuditoriaDetalle nad = new NuevaAuditoriaDetalle(session);
                nad.Auditoria = na;
                nad.Punto = p;
                nad.Resultado = valor;
                nad.Save();

                //item.resultado = valor;
                //item.texto = Request.Form["coment" + item.id.ToString()];
                //iddistribuidor = item.Idaud.Iddistribuidor.iddistribuidor;
                //item.Save();
            }
        }
    }

    public class _dtoPunto
    {
        public int idpilar { get; set; }
        public int idarea { get; set; }
        public int idsubtema { get; set; }
        public int idpunto { get; set; }
        public string nombrepunto { get; set; }
        public int valor { get; set; }
    }
}