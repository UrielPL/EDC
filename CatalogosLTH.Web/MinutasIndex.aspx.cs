using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Xpo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogosLTH.Web
{
    public partial class MinutasIndex : System.Web.UI.Page
    {
        public static List<Usuario> lstDist = new List<Usuario>();
        public static List<Module.BusinessObjects.Minutas> lstMinutas = new List<Module.BusinessObjects.Minutas>();
        public static List<Module.BusinessObjects.Minutas> lstMinutasUser = new List<Module.BusinessObjects.Minutas>();
        public static List<mdl_DistribuidoresMinutas> lstDistMin = new List<mdl_DistribuidoresMinutas>();
        public static DevExpress.Xpo.Session session { get; set; }
        public Usuario user { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            session = Util.getsession();
            string nombreActual = Util.getusuario();
            user = new XPQuery<Usuario>(session).FirstOrDefault(x => x.UserName == nombreActual);
            lstDist.Clear();
            lstMinutas.Clear();
            lstDistMin.Clear();
            switch (user.TipoUsuario)
            {
                case TipoUsuario.Admin:
                    lstDist = new XPQuery<Usuario>(session).Where(x => x.TipoUsuario == TipoUsuario.Distribuidor).OrderBy(x => x.UserName).ToList();
                    lstDistMin = new XPQuery<mdl_DistribuidoresMinutas>(session).ToList();
                    foreach (var item in lstDist)
                    {
                        lstMinutas.AddRange(new XPQuery<Minutas>(session).Where(x => x.Distribuidor.nombredist == item.UserName).ToList());
                    }
                    foreach (var item in lstDistMin)
                    {
                        lstMinutas.AddRange(new XPQuery<Minutas>(session).Where(x => x.DistribuidorMinutas.nombredist == item.nombredist).ToList());
                    }
                    break;
                case TipoUsuario.Distribuidor:
                    lstDist = new XPQuery<Usuario>(session).Where(x => x.UserName == Util.getusuario()).OrderBy(x => x.UserName).ToList();
                    foreach (var item in lstDist)
                    {
                        lstMinutas.AddRange(new XPQuery<Minutas>(session).Where(x => x.Distribuidor.nombredist == item.UserName).ToList());
                    }
                    break;
                case TipoUsuario.Evaluador:
                    if (nombreActual == "alonso.sierra.siller@clarios.com")
                    {
                        lstDist = new XPQuery<Usuario>(session).Where(x => x.TipoUsuario == TipoUsuario.Distribuidor).OrderBy(x => x.UserName).ToList();
                        foreach (var item in lstDist)
                        {
                            lstMinutas.AddRange(new XPQuery<Minutas>(session).Where(x => x.Distribuidor.nombredist == item.UserName).ToList());
                        }
                    }
                    break;
                case TipoUsuario.GerenteCuenta:
                    lstDist = new XPQuery<Usuario>(session).Where(x => x.Jefe.UserName == nombreActual).ToList();
                    lstDistMin = new XPQuery<mdl_DistribuidoresMinutas>(session).Where(x => x.Usuario.Oid == user.Oid).ToList();
                    foreach (var item in lstDist)
                    {
                        lstMinutas.AddRange(new XPQuery<Minutas>(session).Where(x => x.Distribuidor.nombredist == item.UserName).ToList());
                    }
                    foreach (var item in lstDistMin)
                    {
                        lstMinutas.AddRange(new XPQuery<Minutas>(session).Where(x => x.DistribuidorMinutas.nombredist == item.nombredist).ToList());
                    }
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
        }

        [WebMethod]
        public static string ActualizaGrafica()
        {
            List<int> realizadas = new List<int>();
            for (int i = 0; i < 12; i++)
            {
                realizadas.Add(lstMinutas.Where(x => x.Fecha.Month == i + 1).Count());
            }
            realizadas.Add(realizadas.Sum());

            return JsonConvert.SerializeObject(realizadas);
        }

        [WebMethod]
        public static string ActualizaGraficaObjetivos(string Dist, string FY, string mes)
        {
            string year = "20" + FY.Substring(2);

            int Mes = -1;
            if (mes != "Todos")
            {
                switch (mes)
                {
                    case "Enero":
                        Mes = 3;
                        break;
                    case "Febrero":
                        Mes = 4;
                        break;
                    case "Marzo":
                        Mes = 5;
                        break;
                    case "Abril":
                        Mes = 6;
                        break;
                    case "Mayo":
                        Mes = 7;
                        break;
                    case "Junio":
                        Mes = 8;
                        break;
                    case "Julio":
                        Mes = 9;
                        break;
                    case "Agosto":
                        Mes = 10;
                        break;
                    case "Septiembre":
                        Mes = 11;
                        break;
                    case "Octubre":
                        Mes = 0;
                        break;
                    case "Noviembre":
                        Mes = 1;
                        break;
                    case "Diciembre":
                        Mes = 2;
                        break;
                    default:
                        break;
                }
            }

            //Lista de arreglo de enteros. Contendrá los objetivos por mes de cada distribuidor.
            List<int[]> objetivos = new List<int[]>();

            string yearPrev = (int.Parse(year) - 1).ToString();

            //Arreglo de meses por año fiscal
            string[] meses = { "Octubre-" + yearPrev, "Noviembre-" + yearPrev, "Diciembre-" + yearPrev, "Enero-" + year, "Febrero-" + year,
                                    "Marzo-" + year, "Abril-" + year, "Mayo-" + year, "Junio-" + year, "Julio-" + year, "Agosto-" + year,
                                    "Septiembre-" + year };

            if (Dist != "Todos") //Cuando solo se selecciona 1 distribuidor
            {
                int[] objTemp = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                if (Mes == -1)
                {
                    //Si no hay un mes especifico seleccionado, entonces se obtienen los objetivos de todos los meses.
                    for (int i = 0; i < 12; i++)
                    {
                        var temp = new XPQuery<mdl_Objetivos>(session).FirstOrDefault(x => x.MesesObjetivos.mes == meses[i] && x.Distribuidor != null && x.Distribuidor.nombredist == Dist);
                        if (temp != null)
                        {
                            objTemp[i] = temp.cant;
                        }
                        else //Si no encuentra objetivos del distribuidor normal los buscará como distribuidor de minutas
                        {
                            var temp2 = new XPQuery<mdl_Objetivos>(session).FirstOrDefault(x => x.MesesObjetivos.mes == meses[i] && x.DistribuidorMinutas != null && x.DistribuidorMinutas.nombredist == Dist);
                            if (temp2 != null)
                            {
                                objTemp[i] = temp2.cant;
                            }
                        }
                    }
                }
                else
                {
                    //Si hay un mes especifico seleccionado, entonces se obtienen los objetivos del mes seleccionado.
                    var temp = new XPQuery<mdl_Objetivos>(session).FirstOrDefault(x => x.MesesObjetivos.mes == meses[Mes] && x.Distribuidor != null && x.Distribuidor.nombredist == Dist);
                    if (temp != null)
                    {
                        objTemp[Mes] = temp.cant;
                    }
                    else //Si no encuentra objetivos del distribuidor normal los buscará como distribuidor de minutas
                    {
                        var temp2 = new XPQuery<mdl_Objetivos>(session).FirstOrDefault(x => x.MesesObjetivos.mes == meses[Mes] && x.DistribuidorMinutas != null && x.DistribuidorMinutas.nombredist == Dist);
                        if (temp2 != null)
                        {
                            objTemp[Mes] = temp2.cant;
                        }
                    }
                }
                //Se suman los objetivos de todos los meses para sacar el total del año.
                objTemp[12] = objTemp.Sum();
                objetivos.Add(objTemp);

                return JsonConvert.SerializeObject(objTemp);
            }
            else //Cuando se seleccionan a todos los distribuidores
            {
                //Se crea otro array que contendrá el total por mes de objetivos de cada distribuidor
                int[] objTotal = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                foreach (var item in lstDist)
                {
                    //Array temporal que contiene los objetivos por mes del distribuidor actual
                    int[] objTemp = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    if (Mes == -1)
                    {
                        //Si no hay un mes especifico seleccionado, entonces se obtienen los objetivos de todos los meses.
                        for (int i = 0; i < 12; i++)
                        {
                            var temp = new XPQuery<mdl_Objetivos>(session).FirstOrDefault(x => x.MesesObjetivos.mes == meses[i] && x.Distribuidor.nombredist == item.UserName);
                            if (temp != null)
                            {
                                objTemp[i] = temp.cant;
                            }
                        }
                    }
                    else
                    {
                        //Si hay un mes especifico seleccionado, entonces se obtienen los objetivos del mes seleccionado.
                        var temp = new XPQuery<mdl_Objetivos>(session).FirstOrDefault(x => x.MesesObjetivos.mes == meses[Mes] && x.Distribuidor.nombredist == item.UserName);
                        if (temp != null)
                        {
                            objTemp[Mes] = temp.cant;
                        }
                    }
                    objetivos.Add(objTemp);
                }
                //Ahora iteramos entre los distribuidores de minutas
                foreach (var item in lstDistMin)
                {
                    //Array temporal que contiene los objetivos por mes del distribuidor actual
                    int[] objTemp = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    if (Mes == -1)
                    {
                        //Si no hay un mes especifico seleccionado, entonces se obtienen los objetivos de todos los meses.
                        for (int i = 0; i < 12; i++)
                        {
                            var temp = new XPQuery<mdl_Objetivos>(session).FirstOrDefault(x => x.MesesObjetivos.mes == meses[i] && x.DistribuidorMinutas.nombredist == item.nombredist);
                            if (temp != null)
                            {
                                objTemp[i] = temp.cant;
                            }
                        }
                    }
                    else
                    {
                        //Si hay un mes especifico seleccionado, entonces se obtienen los objetivos del mes seleccionado.
                        var temp = new XPQuery<mdl_Objetivos>(session).FirstOrDefault(x => x.MesesObjetivos.mes == meses[Mes] && x.DistribuidorMinutas.nombredist == item.nombredist);
                        if (temp != null)
                        {
                            objTemp[Mes] = temp.cant;
                        }
                    }
                    objetivos.Add(objTemp);
                }
                //En este caso se seleccionaron todos los distribuidores, asi que hay que recorrer la lista que contiene sus objetivos por mes.
                foreach (var item in objetivos)
                {
                    int index = 0;
                    for (int i = 0; i < 12; i++)
                    {
                        //Hay que ir sumando los objetivos de cada distribuidor por mes, en nuestro arreglo de objetivos totales.
                        objTotal[i] += item[i];
                    }
                    index++;
                }
                //Se suman los objetivos de todos los meses para sacar el total del año.
                objTotal[12] = objTotal.Sum();

                return JsonConvert.SerializeObject(objTotal);
            }
        }
    }
}