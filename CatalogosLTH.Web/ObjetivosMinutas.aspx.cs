using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Xpo;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogosLTH.Web
{
    public partial class ObjetivosMinutas : System.Web.UI.Page
    {
        public List<Usuario> lstDist = new List<Usuario>();
        public static List<mdl_DistribuidoresMinutas> lstDistMin = new List<mdl_DistribuidoresMinutas>();
        public Usuario user { get; set; }
        public List<Module.BusinessObjects.Minutas> lstMinutas = new List<Module.BusinessObjects.Minutas>();
        public List<Usuario> lstGerentes = new List<Usuario>();
        public List<int[]> lstObjetivos = new List<int[]>();
        public List<int> lstTotalDist = new List<int>();
        public List<int> lstTotalMes = new List<int>();
        public static DevExpress.Xpo.Session session { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

            }
            session = Util.getsession();
            string userRol = Util.getrol();
            string nombreActual = Util.getusuario();
            user = new XPQuery<Usuario>(session).FirstOrDefault(x => x.UserName == nombreActual);
            lstDist.Clear();
            lstMinutas.Clear();
            lstGerentes.Clear();
            lstTotalMes.Clear();
            lstTotalDist.Clear();
            switch (user.TipoUsuario)
            {
                case TipoUsuario.Admin:
                    lstDist = new XPQuery<Usuario>(session).Where(x => x.TipoUsuario == TipoUsuario.Distribuidor).OrderBy(x => x.UserName).ToList();
                    lstDistMin = new XPQuery<mdl_DistribuidoresMinutas>(session).ToList();
                    foreach (var item in lstDist)
                    {
                        lstMinutas.AddRange(new XPQuery<Module.BusinessObjects.Minutas>(session).Where(x => x.Distribuidor.nombredist == item.UserName).ToList());
                    }
                    foreach (var item in lstDistMin)
                    {
                        lstMinutas.AddRange(new XPQuery<Module.BusinessObjects.Minutas>(session).Where(x => x.DistribuidorMinutas.nombredist == item.nombredist).ToList());
                    }
                    lstGerentes = new XPQuery<Usuario>(session).Where(x => x.TipoUsuario == TipoUsuario.GerenteCuenta).ToList();
                    break;
                case TipoUsuario.Distribuidor:
                    lstDist = new XPQuery<Usuario>(session).Where(x => x.UserName == Util.getusuario()).OrderBy(x => x.UserName).ToList();
                    foreach (var item in lstDist)
                    {
                        lstMinutas.AddRange(new XPQuery<Module.BusinessObjects.Minutas>(session).Where(x => x.Distribuidor.nombredist == item.UserName).ToList());
                    }
                    break;
                case TipoUsuario.Evaluador:
                    if (nombreActual == "alonso.sierra.siller@clarios.com")
                    {
                        lstDist = new XPQuery<Usuario>(session).Where(x => x.TipoUsuario == TipoUsuario.Distribuidor).OrderBy(x => x.UserName).ToList();
                        foreach (var item in lstDist)
                        {
                            lstMinutas.AddRange(new XPQuery<Module.BusinessObjects.Minutas>(session).Where(x => x.Distribuidor.nombredist == item.UserName).ToList());
                        }
                    }
                    break;
                case TipoUsuario.GerenteCuenta:
                    lstDist = new XPQuery<Usuario>(session).Where(x => x.Jefe.UserName == nombreActual).ToList();
                    lstDistMin = new XPQuery<mdl_DistribuidoresMinutas>(session).Where(x => x.Usuario.Oid == user.Oid).ToList();
                    foreach (var item in lstDist)
                    {
                        lstMinutas.AddRange(new XPQuery<Module.BusinessObjects.Minutas>(session).Where(x => x.Distribuidor.nombredist == item.UserName).ToList());
                    }
                    foreach (var item in lstDistMin)
                    {
                        lstMinutas.AddRange(new XPQuery<Module.BusinessObjects.Minutas>(session).Where(x => x.DistribuidorMinutas.nombredist == item.nombredist).ToList());
                    }
                    break;
                case TipoUsuario.GerenteDesarrolloComercial:
                    lstDist = new XPQuery<Usuario>(session).Where(x => x.Jefe.Jefe.UserName == nombreActual).ToList();
                    foreach (var item in lstDist)
                    {
                        lstMinutas.AddRange(new XPQuery<Module.BusinessObjects.Minutas>(session).Where(x => x.Distribuidor.nombredist == item.UserName).ToList());
                    }
                    lstGerentes = new XPQuery<Usuario>(session).Where(x => x.TipoUsuario == TipoUsuario.GerenteCuenta && x.Jefe.UserName == nombreActual).ToList();
                    break;
                case TipoUsuario.GerenteVenta:
                    break;
                default:
                    break;
            }

            if (!IsPostBack)
            {
                cbxFY.Items.Add("FY18", "2018");
                cbxFY.Items.Add("FY19", "2019");
                cbxFY.Items.Add("FY20", "2020");
                cbxFY.Items.Add("FY21", "2021");
                cbxFY.SelectedIndex = 0;
            }

            obtenerObjetivos(cbxFY.SelectedItem.Value.ToString());

            if(ViewState["cargaExcel"] != null && ViewState["cargaExcel"].ToString() == "1") {
                ViewState["cargaExcel"] = null;
            }
        }

        protected void cbxFY_SelectedIndexChanged(object sender, EventArgs e)
        {
            //obtenerObjetivos(cbxFY.SelectedItem.Value.ToString());
        }

        public void obtenerObjetivos(string yearP)
        {
            string year = yearP;
            string yearPrev = (int.Parse(year) - 1).ToString();
            string[] meses = { "Octubre-" + yearPrev, "Noviembre-" + yearPrev, "Diciembre-" + yearPrev, "Enero-" + year, "Febrero-" + year,
                                    "Marzo-" + year, "Abril-" + year, "Mayo-" + year, "Junio-" + year, "Julio-" + year, "Agosto-" + year,
                                    "Septiembre-" + year };

            foreach (var item in lstDist)
            {
                int[] objTemp = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                for (int i = 0; i < 12; i++)
                {
                    var temp = new XPQuery<mdl_Objetivos>(session).FirstOrDefault(x => x.MesesObjetivos.mes == meses[i] && x.Distribuidor.nombredist == item.UserName);
                    if (temp != null)
                    {
                        objTemp[i] = temp.cant;
                    }
                }
                lstObjetivos.Add(objTemp);
                lstTotalDist.Add(objTemp.Sum());
            }

            foreach (var item in lstDistMin)
            {
                int[] objTemp = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                for (int i = 0; i < 12; i++)
                {
                    var temp = new XPQuery<mdl_Objetivos>(session).FirstOrDefault(x => x.MesesObjetivos.mes == meses[i] && x.DistribuidorMinutas.nombredist == item.nombredist);
                    if (temp != null)
                    {
                        objTemp[i] = temp.cant;
                    }
                }
                lstObjetivos.Add(objTemp);
                lstTotalDist.Add(objTemp.Sum());
            }

            for (int i = 0; i < 12; i++)
            {
                int totalMes = 0;
                foreach (var item in lstObjetivos)
                {
                    totalMes += item[i];
                }
                lstTotalMes.Add(totalMes);
            }
        }

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            if (archivo != null)
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + "\\Archivos\\Objetivos\\";
                string nombre = archivo.FileName;

                archivo.SaveAs(ruta + nombre);

                Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                var reader = new StreamReader(File.OpenRead(ruta + nombre), iso);
                string line = "";
                line = reader.ReadLine();
                string FY = "";
                string[] meses = { "Octubre", "Noviembre", "Diciembre", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre" };
                //Session varses = new Session();

                using (UnitOfWork varses = new UnitOfWork())
                {

                    while (!reader.EndOfStream) {
                    
                        line = reader.ReadLine();
                        var values = line.Split(',');
                        int year = int.Parse(values[13]);

                        if(lstDist.FirstOrDefault(x=>x.UserName == values[0]) != null || lstDistMin.FirstOrDefault(x => x.nombredist == values[0]) != null)
                        {
                            for (int k = 0; k < 12; k++)
                            {
                                if (k < 3)
                                {
                                    FY = "-" + (year - 1).ToString();
                                }
                                else
                                {
                                    FY = "-" + year.ToString();
                                }

                                var objDist = new XPQuery<mdl_Objetivos>(session).FirstOrDefault(x => x.Distribuidor.nombredist == values[0] && x.MesesObjetivos.mes == (meses[k] + FY));
                                if (objDist == null)
                                {
                                    objDist = new XPQuery<mdl_Objetivos>(session).FirstOrDefault(x => x.DistribuidorMinutas.nombredist == values[0] && x.MesesObjetivos.mes == (meses[k] + FY));
                                }

                                if (objDist != null)
                                {
                                    objDist.cant = int.Parse(values[k + 1]);
                                    objDist.Save();
                                }
                                else
                                {
                                    mdl_Objetivos newObj = new mdl_Objetivos(session);
                                    if (new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist == values[0]) != null)
                                        newObj.Distribuidor = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist == values[0]);
                                    else
                                        newObj.DistribuidorMinutas = lstDistMin.FirstOrDefault(x => x.nombredist == values[0]);
                                    newObj.MesesObjetivos = new XPQuery<mdl_MesesObjetivos>(session).FirstOrDefault(x => x.mes == (meses[k] + FY));
                                    newObj.cant = int.Parse(values[k + 1]);
                                    newObj.Save();
                                }
                                ViewState["cargaExcel"] = "1";
                            }
                        }
                    }
                    varses.CommitChanges();
                }
                obtenerObjetivos(cbxFY.SelectedItem.Value.ToString());
            }
        }

        protected void btnGuardarDist_Click(object sender, EventArgs e)
        {
            mdl_DistribuidoresMinutas temp = new mdl_DistribuidoresMinutas(session);
            temp.fechaRegistro = DateTime.Now;
            temp.nombredist = txtDistMin.Text;
            temp.Usuario = new XPQuery<Usuario>(session).FirstOrDefault(x => x.UserName == Util.getusuario());
            temp.Save();
        }

        public class ventasDTO
        {
            public string dist { get; set; }
            public string CS { get; set; }
            public string idCS {get;set;}
            public List<int> totalDias = new List<int>();
        }
    }
}