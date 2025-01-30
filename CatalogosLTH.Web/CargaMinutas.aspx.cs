using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Newtonsoft.Json;
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
    public partial class CargaMinutas : System.Web.UI.Page
    {
        public List<Usuario> lstDist = new List<Usuario>();
        public List<mdl_DistribuidoresMinutas> lstDistMin = new List<mdl_DistribuidoresMinutas>();
        public static List<Minutas> lstMinutas = new List<Minutas>();
        public static List<Minutas> lstMinutasUser = new List<Minutas>();
        public static DevExpress.Xpo.Session session { get; set; }
        public static Usuario user { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            session = Util.getsession();
            //string userRol = Util.getrol();
            string nombreActual = Util.getusuario();
            user = new XPQuery<Usuario>(session).FirstOrDefault(x => x.UserName == nombreActual);
            lstDist.Clear();
            lstMinutas.Clear();
            switch (user.TipoUsuario)
            {
                case TipoUsuario.Admin:
                    lstDist = new XPQuery<Usuario>(session).Where(x => x.TipoUsuario == TipoUsuario.Distribuidor).OrderBy(x => x.UserName).ToList();
                    foreach (var item in lstDist)
                    {
                        lstMinutas.AddRange(new XPQuery<Module.BusinessObjects.Minutas>(session).Where(x => x.Usuario.Oid == item.Oid).ToList());
                    }
                    break;
                case TipoUsuario.Distribuidor:
                    lstDist = new XPQuery<Usuario>(session).Where(x => x.UserName == Util.getusuario()).OrderBy(x => x.UserName).ToList();
                    foreach (var item in lstDist)
                    {
                        lstMinutas.AddRange(new XPQuery<Module.BusinessObjects.Minutas>(session).Where(x => x.Usuario.Oid == item.Oid).ToList());
                    }
                    break;
                case TipoUsuario.Evaluador:
                    if (nombreActual == "alonso.sierra.siller@clarios.com")
                    {
                        lstDist = new XPQuery<Usuario>(session).Where(x => x.TipoUsuario == TipoUsuario.Distribuidor).OrderBy(x => x.UserName).ToList();
                        foreach (var item in lstDist)
                        {
                            lstMinutas.AddRange(new XPQuery<Module.BusinessObjects.Minutas>(session).Where(x => x.Usuario.Oid == item .Oid).ToList());
                        }
                    }
                    break;
                case TipoUsuario.GerenteCuenta:
                    lstDist = new XPQuery<Usuario>(session).Where(x => x.TipoUsuario == TipoUsuario.Distribuidor && x.Jefe.UserName == nombreActual).ToList();
                    lstDistMin = new XPQuery<mdl_DistribuidoresMinutas>(session).Where(x=>x.Usuario.Oid == user.Oid).ToList();
                    lstMinutas.AddRange(new XPQuery<Module.BusinessObjects.Minutas>(session).Where(x => x.Usuario.Oid == user.Oid).ToList());
                    break;
                case TipoUsuario.GerenteDesarrolloComercial:
                    lstDist = new XPQuery<Usuario>(session).Where(x => x.Jefe.Jefe.UserName == nombreActual).ToList();
                    lstDistMin = new XPQuery<mdl_DistribuidoresMinutas>(session).Where(x => x.Usuario.Jefe.ZonaPertenece.zona == user.ZonaPertenece.zona).ToList();
                    foreach (var item in lstDist)
                    {
                        lstMinutas.AddRange(new XPQuery<Minutas>(session).Where(x => x.Distribuidor.nombredist == item.UserName).ToList());
                    }
                    foreach (var item in lstDistMin)
                    {
                        lstMinutas.AddRange(new XPQuery<Minutas>(session).Where(x => x.DistribuidorMinutas.nombredist == item.nombredist).ToList());
                    }
                    break;
                case TipoUsuario.GerenteVenta:
                    Usuario gteDesComercial = new XPQuery<Usuario>(session).FirstOrDefault(x => x.TipoUsuario == TipoUsuario.GerenteDesarrolloComercial && x.ZonaPertenece == user.ZonaPertenece);
                    lstDist = new XPQuery<Usuario>(session).Where(x => x.Jefe.Jefe.UserName == gteDesComercial.UserName).ToList();
                    lstDistMin.AddRange(new XPQuery<mdl_DistribuidoresMinutas>(session).Where(x => x.Usuario.Jefe.ZonaPertenece.zona == gteDesComercial.ZonaPertenece.zona).ToList());
                    foreach (var item in lstDist)
                    {
                        lstMinutas.AddRange(new XPQuery<Minutas>(session).Where(x => x.Distribuidor.nombredist == item.UserName).ToList());
                    }
                    foreach (var item in lstDistMin)
                    {
                        lstMinutas.AddRange(new XPQuery<Minutas>(session).Where(x => x.DistribuidorMinutas.nombredist == item.nombredist).ToList());
                    }
                    break;
                default:
                    break;
            }
            
            lstMinutasUser = lstMinutas;

            if (!IsPostBack)
            {
                if (lstDist.Count() > 0)
                {
                    foreach (var item in lstDist)
                    {
                        if (item.UserName != "nohay")
                            cbxDist.Items.Add(item.UserName, item.UserName);
                    }
                    foreach (var item in lstDistMin)
                    {
                        cbxDist.Items.Add(item.nombredist, item.nombredist);
                    }
                    cbxDist.SelectedIndex = 0;
                }
            }
        }

        [WebMethod]
        public static string ActualizaTabla()
        {
            lstMinutas = lstMinutasUser;

            var minutas = (from a in lstMinutas
                           where a.Fecha.Month == DateTime.Now.Month
                           select new ReporteMinutas()
                           {
                               fecha = a.Fecha.ToShortDateString(),
                               archivo = a.Archivo.FileName,
                               dist = a.Distribuidor != null ? a.Distribuidor.nombredist : a.DistribuidorMinutas.nombredist
                           }
                        );

            return JsonConvert.SerializeObject(minutas); ;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                DevExpress.Persistent.BaseImpl.FileData dataF = null;
                string clav = "";
                string nombreDist = "";
                dataF = new DevExpress.Persistent.BaseImpl.FileData(session);
                if (archivo.HasFile)
                {
                    var fileStream = archivo.FileContent;
                    dataF.LoadFromStream(archivo.FileName, fileStream);
                }

                using (UnitOfWork uow = new UnitOfWork())
                {
                    Module.BusinessObjects.Minutas newMinuta = new Module.BusinessObjects.Minutas(session);
                    var dist = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist == cbxDist.SelectedItem.Text);
                    if (dist != null)
                    {
                        newMinuta.Distribuidor = dist;
                        nombreDist = newMinuta.Distribuidor.nombredist;
                    } else
                    {
                        newMinuta.DistribuidorMinutas = new XPQuery<mdl_DistribuidoresMinutas>(session).FirstOrDefault(x => x.nombredist == cbxDist.SelectedItem.Text);
                        nombreDist = newMinuta.DistribuidorMinutas.nombredist;
                    }
                    newMinuta.Fecha = dtFecha.Date;
                    string FY = "";
                    if(dtFecha.Date.Month < 10)
                    {
                       FY = dtFecha.Date.Year.ToString();
                    } else
                    {
                       FY = (dtFecha.Date.Year + 1).ToString();
                    }
                    newMinuta.FY = "FY" + FY.Substring(2);
                    newMinuta.Usuario = new XPQuery<Usuario>(session).FirstOrDefault(x => x.UserName == Util.getusuario());
                    if (dataF != null) { newMinuta.Archivo = dataF; }
                    newMinuta.Save();
                    uow.CommitChanges();

                    clav = newMinuta.Oid.ToString();
                }

                if (archivo.HasFile)
                {
                    //Guardar en disco duro
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + "\\archivos\\Minutas\\";
                    dataF.FileName = nombreDist + "-" + clav + dataF.FileName;
                    string nombre = dataF.FileName;

                    FileStream fileStream1 = System.IO.File.Create(ruta + nombre);

                    dataF.SaveToStream(fileStream1);
                    fileStream1.Close();
                    Encoding iso = Encoding.GetEncoding("ISO-8859-1");

                    FileData filed = null;
                    filed = new FileData(session);
                    filed.SetMemberValue("Content", File.ReadAllBytes(Path.Combine(ruta, dataF.FileName)));
                    filed.FileName = dataF.FileName;
                    filed.Save();

                    using (UnitOfWork uow = new UnitOfWork())
                    {
                        Module.BusinessObjects.Minutas newMinuta = new XPQuery<Module.BusinessObjects.Minutas>(session).FirstOrDefault(x => x.Oid.ToString() == clav);
                        if (newMinuta.Archivo != null)
                        {
                            newMinuta.Archivo = filed;
                        }
                        newMinuta.Save();
                        uow.CommitChanges();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public class ReporteMinutas
        {
            public string fecha { get; set; }
            public string archivo { get; set; }
            public string dist { get; set; }
        }
    }
}