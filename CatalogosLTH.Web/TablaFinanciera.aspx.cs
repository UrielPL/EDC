using CatalogosLTH.Module.BusinessObjects;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DevExpress.Xpo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogosLTH.Web
{
    public partial class TablaFinanciera : System.Web.UI.Page
    {
        public string tipo { get; set; }
        public bool completada { get; set; }
        public string estatus { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            string permiso = Util.getPermiso().ToString();
            string nombreActual = Util.getusuario();

            Session["username"] = nombreActual;
            Session["permiso"] = permiso;

            tipo = permiso;
            completada = false;
            string param = Request.QueryString["t"];
            if(param != null)
            {
                var tabla = new XPQuery<TablaFinancieraResultados>(session).Where(x => x.Id.ToString() == param).FirstOrDefault();
                if(tabla != null)
                {
                    completada = tabla.completa;
                    estatus = tabla.estatus;
                }
            }else
                {
                string au = Request.QueryString["a"];
                if (au != null)
                {
                    var aud = new XPQuery<NuevaAuditoria>(session).Where(x => x.Id.ToString() == au).FirstOrDefault();
                    if (aud != null)
                    {
                        completada = aud.TablaFinancieraRes.FirstOrDefault().completa;
                        estatus = aud.TablaFinancieraRes.FirstOrDefault().estatus;
                    }
                }
            }
        }


        [WebMethod]
        public static string traeTabla(string a)
        {
            DevExpress.Xpo.Session session = Util.getsession();

            //string aud = (string)HttpContext.Current.Session["audt"];
            string aud = a;

            var auditoria = new XPQuery<NuevaAuditoria>(session).FirstOrDefault(x => x.Id.ToString() == aud);

            if (auditoria != null)
            {
                var tabla = new XPQuery<TablaFinancieraResultados>(session).FirstOrDefault(x => x.IdAuditoria == auditoria);

                var r1 = tabla.respuestaMUB;
                var r2 = tabla.respuestaMUO;
                var r3 = tabla.respuestaLRC;
                var r4 = tabla.respuestaLRR;
                var r5 = tabla.respuestaRI;
                var r6 = tabla.respuestaPPC;
                var r7 = tabla.respuestaPPP;
                var r8 = tabla.respuestaE;
                var r9 = tabla.respuestaGO;
                var r10 = tabla.respuestaRAO;

                dtotabla temp = new dtotabla();
                temp.r1 = r1;
                temp.r2 = r2;
                temp.r3 = r3;
                temp.r4 = r4;
                temp.r5 = r5;
                temp.r6 = r6;
                temp.r7 = r7;
                temp.r8 = r8;
                temp.r9 = r9;
                temp.r10 = r10;

                temp.ubruta = tabla.u_bruta;
                temp.uoperativa = tabla.u_operativa;
                temp.acirc = tabla.a_circ;
                temp.acinv = tabla.a_circInv;
                temp.inv = tabla.inventario_cv;
                temp.cxc = tabla.cxc;
                temp.cxp = tabla.cxp;
                temp.ptot = tabla.pasivo_tot;
                temp.goper = tabla.gast_oper;
                temp.uoper = tabla.uti_oper;

                temp.vneta = tabla.v_netas;
                temp.vneta2 = tabla.v_netas_;
                temp.pcirc = tabla.pas_circ;
                temp.pcirc2 = tabla.pas_circInv;
                temp.cvdias = tabla.dias_periodo;
                temp.vcredper = tabla.cxc_dp;
                temp.cvper = tabla.cxp_dp;
                temp.atot = tabla.activo_tot;
                temp.vnet = tabla.vts_netas;
                temp.atotpas = tabla.actTot_pasTot;

                if(tabla.urlArchivo != null && tabla.urlArchivo != "")
                {
                    temp.url = tabla.urlArchivo;
                    temp.nombre = tabla.nombreArchivo;
                }

                return JsonConvert.SerializeObject(temp);

                //tabladto temp = new tabladto();
                //temp.id = tabla.Id;
                //temp.mub = tabla.respuestaMUB;
                //temp.muo = tabla.respuestaMUO;
                //temp.lrc = tabla.respuestaLRC;
                //temp.lrr = tabla.respuestaLRR;
                //temp.ri = tabla.respuestaRI;
                //temp.ppc = tabla.respuestaPPC;
                //temp.ppp = tabla.respuestaPPP;
                //temp.e = tabla.respuestaE;
                //temp.go = tabla.respuestaGO;
                //temp.rao = tabla.respuestaRAO;

                //return JsonConvert.SerializeObject(temp);


            }

            return "-1";
        }

        [WebMethod]
        public static string traeTablaID(string t)
        {
            DevExpress.Xpo.Session session = Util.getsession();

            //string idtabla = (string)HttpContext.Current.Session["idtabla"];
            string idtabla = t;

            var tabla = new XPQuery<TablaFinancieraResultados>(session).FirstOrDefault(x => x.Id == int.Parse(idtabla));

            if (tabla != null)
            {
                var r1 = tabla.respuestaMUB;
                var r2 = tabla.respuestaMUO;
                var r3 = tabla.respuestaLRC;
                var r4 = tabla.respuestaLRR;
                var r5 = tabla.respuestaRI;
                var r6 = tabla.respuestaPPC;
                var r7 = tabla.respuestaPPP;
                var r8 = tabla.respuestaE;
                var r9 = tabla.respuestaGO;
                var r10 = tabla.respuestaRAO;

                dtotabla temp = new dtotabla();
                temp.r1 = r1;
                temp.r2 = r2;
                temp.r3 = r3;
                temp.r4 = r4;
                temp.r5 = r5;
                temp.r6 = r6;
                temp.r7 = r7;
                temp.r8 = r8;
                temp.r9 = r9;
                temp.r10 = r10;

                temp.ubruta = tabla.u_bruta;
                temp.uoperativa = tabla.u_operativa;
                temp.acirc = tabla.a_circ;
                temp.acinv = tabla.a_circInv;
                temp.inv = tabla.inventario_cv;
                temp.cxc = tabla.cxc;
                temp.cxp = tabla.cxp;
                temp.ptot = tabla.pasivo_tot;
                temp.goper = tabla.gast_oper;
                temp.uoper = tabla.uti_oper;

                temp.vneta = tabla.v_netas;
                temp.vneta2 = tabla.v_netas_;
                temp.pcirc = tabla.pas_circ;
                temp.pcirc2 = tabla.pas_circInv;
                temp.cvdias = tabla.dias_periodo;
                temp.vcredper = tabla.cxc_dp;
                temp.cvper = tabla.cxp_dp;
                temp.atot = tabla.activo_tot;
                temp.vnet = tabla.vts_netas;
                temp.atotpas = tabla.actTot_pasTot;

                if (tabla.urlArchivo != null && tabla.urlArchivo != "")
                {
                    temp.url = tabla.urlArchivo;
                    temp.nombre = tabla.nombreArchivo;
                }

                temp.distribuidor = tabla.distribuidor != "" ? tabla.distribuidor : tabla.distribuidorSel != "" ? tabla.distribuidorSel : "";

                return JsonConvert.SerializeObject(temp);
            }

            return "-1";
        }

        [WebMethod]
        public static string guardaTabla(string r1, string r2, string r3, string r4, string r5, string r6, string r7, string r8, string r9, string r10,
            string ubruta, string vneta, string uoperativa, string vneta2, string acirc, string pcirc, string acinv, string pcirc2, string inv, string cvdias,
            string cxc, string vcredper, string cxp, string cvper, string ptot, string atot, string goper, string vnet, string uoper, string atotpas, string archivo, string nombre, bool completo, string dist, string aud, string tbl)
        {
            DevExpress.Xpo.Session session = Util.getsession();

            string usr = (string)HttpContext.Current.Session["username"];
            var usuario = new XPQuery<Usuario>(session).Where(x => x.UserName == usr).FirstOrDefault();

            if((aud == null || aud == "-1") && (tbl == null || tbl == "-1"))
            {
                TablaFinancieraResultados nTabla = new TablaFinancieraResultados(session);
                nTabla.fechaRealizada = DateTime.Now;
                nTabla.realizo = usr;
                nTabla.distribuidorSel = dist;

                nTabla.areaMUB = "Margen de utilidad bruta"; nTabla.respuestaMUB = r1;
                nTabla.areaMUO = "Margen de utilidad operativa"; nTabla.respuestaMUO = r2;
                nTabla.areaLRC = "Liquidez (razón circulante)"; nTabla.respuestaLRC = r3;
                nTabla.areaLRR = "Liquidez (razón rápida)"; nTabla.respuestaLRR = r4;
                nTabla.areaRI = "Rotación de inventario"; nTabla.respuestaRI = r5;
                nTabla.areaPPC = "Periodo promedio de cobro"; nTabla.respuestaPPC = r6;
                nTabla.areaPPP = "Periodo promedio de pago"; nTabla.respuestaPPP = r7;
                nTabla.areaE = "Endeudamiento"; nTabla.respuestaE = r8;
                nTabla.areaGO = "Gastos operativos"; nTabla.respuestaGO = r9;
                nTabla.areaRAO = "Rendimiento sobre activos operativos"; nTabla.respuestaRAO = r10;

                nTabla.u_bruta = ubruta; nTabla.v_netas = vneta;
                nTabla.u_operativa = uoperativa; nTabla.v_netas_ = vneta2;
                nTabla.a_circ = acirc; nTabla.pas_circ = pcirc;
                nTabla.a_circInv = acinv; nTabla.pas_circInv = pcirc2;
                nTabla.inventario_cv = inv; nTabla.dias_periodo = cvdias;
                nTabla.cxc = cxc; nTabla.cxc_dp = vcredper;
                nTabla.cxp = cxp; nTabla.cxp_dp = cvper;
                nTabla.pasivo_tot = ptot; nTabla.activo_tot = atot;
                nTabla.gast_oper = goper; nTabla.vts_netas = vnet;
                nTabla.uti_oper = uoper; nTabla.actTot_pasTot = atotpas;

                nTabla.completa = false;
                nTabla.estatus = "En revisión";


                nTabla.Save();

                if (archivo != "" && nombre != "")
                {
                    var url = guardaArchivo(archivo, nombre);

                    if (url != "-1")
                    {
                        nTabla.urlArchivo = url;
                        nTabla.nombreArchivo = nombre;
                    }
                }


                nTabla.Save();

                //MANDA MAIL
                string body = Util.correoAvisaGerenteTabla(usr, usuario.Jefe.email);
                if (usuario.Jefe.email != null && usuario.Jefe.email != "")
                {
                    Util.SendMailTabla(usuario.Jefe.email, "EDC-III - Tabla Financiera por aprobar EDCII", body);
                    Util.SendMailTabla("uriel.perlop@gmail.com", "EDC-III - Tabla Financiera por aprobar EDCII", body);
                }

                archivodto obj = new archivodto();
                obj.url = nTabla.urlArchivo;
                obj.nombre = nTabla.nombreArchivo;
                obj.idtabla = nTabla.Id.ToString();

                return JsonConvert.SerializeObject(obj);
            }else
            {
                if(aud != null  && aud != "-1")
                {
                    var tabla = new XPQuery<TablaFinancieraResultados>(session).Where(x => x.IdAuditoria.Id.ToString() == aud).FirstOrDefault();

                    tabla.areaMUB = "Margen de utilidad bruta"; tabla.respuestaMUB = r1;
                    tabla.areaMUO = "Margen de utilidad operativa"; tabla.respuestaMUO = r2;
                    tabla.areaLRC = "Liquidez (razón circulante)"; tabla.respuestaLRC = r3;
                    tabla.areaLRR = "Liquidez (razón rápida)"; tabla.respuestaLRR = r4;
                    tabla.areaRI = "Rotación de inventario"; tabla.respuestaRI = r5;
                    tabla.areaPPC = "Periodo promedio de cobro"; tabla.respuestaPPC = r6;
                    tabla.areaPPP = "Periodo promedio de pago"; tabla.respuestaPPP = r7;
                    tabla.areaE = "Endeudamiento"; tabla.respuestaE = r8;
                    tabla.areaGO = "Gastos operativos"; tabla.respuestaGO = r9;
                    tabla.areaRAO = "Rendimiento sobre activos operativos"; tabla.respuestaRAO = r10;

                    tabla.u_bruta = ubruta; tabla.v_netas = vneta;
                    tabla.u_operativa = uoperativa; tabla.v_netas_ = vneta2;
                    tabla.a_circ = acirc; tabla.pas_circ = pcirc;
                    tabla.a_circInv = acinv; tabla.pas_circInv = pcirc2;
                    tabla.inventario_cv = inv; tabla.dias_periodo = cvdias;
                    tabla.cxc = cxc; tabla.cxc_dp = vcredper;
                    tabla.cxp = cxp; tabla.cxp_dp = cvper;
                    tabla.pasivo_tot = ptot; tabla.activo_tot = atot;
                    tabla.gast_oper = goper; tabla.vts_netas = vnet;
                    tabla.uti_oper = uoper; tabla.actTot_pasTot = atotpas;

                    if (archivo != "" && nombre != "")
                    {
                        var url = guardaArchivo(archivo, nombre);

                        if (url != "-1")
                        {
                            tabla.urlArchivo = url;
                            tabla.nombreArchivo = nombre;
                        }
                    }

                    tabla.Save();
                    archivodto obj = new archivodto();
                    obj.url = tabla.urlArchivo;
                    obj.nombre = tabla.nombreArchivo;

                    return JsonConvert.SerializeObject(obj);

                }

                if(tbl != null && tbl != "-1")
                {
                    var tabla = new XPQuery<TablaFinancieraResultados>(session).Where(x => x.Id.ToString() == tbl).FirstOrDefault();

                    tabla.areaMUB = "Margen de utilidad bruta"; tabla.respuestaMUB = r1;
                    tabla.areaMUO = "Margen de utilidad operativa"; tabla.respuestaMUO = r2;
                    tabla.areaLRC = "Liquidez (razón circulante)"; tabla.respuestaLRC = r3;
                    tabla.areaLRR = "Liquidez (razón rápida)"; tabla.respuestaLRR = r4;
                    tabla.areaRI = "Rotación de inventario"; tabla.respuestaRI = r5;
                    tabla.areaPPC = "Periodo promedio de cobro"; tabla.respuestaPPC = r6;
                    tabla.areaPPP = "Periodo promedio de pago"; tabla.respuestaPPP = r7;
                    tabla.areaE = "Endeudamiento"; tabla.respuestaE = r8;
                    tabla.areaGO = "Gastos operativos"; tabla.respuestaGO = r9;
                    tabla.areaRAO = "Rendimiento sobre activos operativos"; tabla.respuestaRAO = r10;

                    tabla.u_bruta = ubruta; tabla.v_netas = vneta;
                    tabla.u_operativa = uoperativa; tabla.v_netas_ = vneta2;
                    tabla.a_circ = acirc; tabla.pas_circ = pcirc;
                    tabla.a_circInv = acinv; tabla.pas_circInv = pcirc2;
                    tabla.inventario_cv = inv; tabla.dias_periodo = cvdias;
                    tabla.cxc = cxc; tabla.cxc_dp = vcredper;
                    tabla.cxp = cxp; tabla.cxp_dp = cvper;
                    tabla.pasivo_tot = ptot; tabla.activo_tot = atot;
                    tabla.gast_oper = goper; tabla.vts_netas = vnet;
                    tabla.uti_oper = uoper; tabla.actTot_pasTot = atotpas;

                    if (archivo != "" && nombre != "")
                    {
                        var url = guardaArchivo(archivo, nombre);

                        if (url != "-1")
                        {
                            tabla.urlArchivo = url;
                            tabla.nombreArchivo = nombre;
                        }
                    }

                    tabla.Save();
                    archivodto obj = new archivodto();
                    obj.url = tabla.urlArchivo;
                    obj.nombre = tabla.nombreArchivo;

                    return JsonConvert.SerializeObject(obj);
                }
            }

            return "-1";
            
            
        }

        public static string guardaArchivo(string b64, string nombre)
        {
            try
            {
                var document = b64.Replace("data:image/jpeg;base64,", "").Replace("data:image/png;base64,", "");

                byte[] bytes = Convert.FromBase64String(document);
                //byte[] bytes = documento.doc;
                //string directory = AppDomain.CurrentDomain.BaseDirectory + "web\\archivos\\";
                int pos = nombre.LastIndexOf('.');
                string ext = nombre.Substring(pos, (nombre.Length - pos));
                string newname = "archivo - " + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ext;

                //System.IO.File.WriteAllBytes(Path.Combine(directory, newname), bytes);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                CloudinaryAPI acc = new CloudinaryAPI();
                var cloudinary = new Cloudinary(acc.Account);
                Stream stream = new MemoryStream(bytes);

                stream.Position = 0;

                var uploadParams = new RawUploadParams()
                {
                    File = new FileDescription(newname, stream),
                    PublicId = "archivosTablas/" + Guid.NewGuid(),

                };

                var uploadResult = cloudinary.Upload(uploadParams);

                return uploadResult.SecureUrl.AbsoluteUri;
            }
            catch (Exception e)
            {
                return "-1";
            }
        }

        [WebMethod]
        public static string traeDistribuidores()
        {
            DevExpress.Xpo.Session session = Util.getsession();

            string nombreActual = (string)HttpContext.Current.Session["username"];

            string permiso = (string)HttpContext.Current.Session["permiso"];

            List<Usuario> listaDistribuidores = new List<Usuario>();
            switch (permiso)
            {
                case "GerenteCuenta":
                    listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.Jefe.UserName == nombreActual).ToList();
                    break;
                case "GerenteDesarrolloComercial":
                    listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.Jefe.Jefe.UserName == nombreActual).ToList();
                    //listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.Jefe.UserName == nombreActual).ToList();
                    break;
                case "Distribuidor":
                    listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.UserName == nombreActual).ToList();
                    break;
                case "GerenteVenta":
                    Usuario u = new XPQuery<Usuario>(session).FirstOrDefault(x => x.UserName == nombreActual);
                    string z = u.ZonaPertenece.zona;
                    listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.ZonaPertenece.zona == z && x.TipoUsuario == TipoUsuario.Distribuidor).ToList();
                    break;
                case "Admin":
                    listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.TipoUsuario == TipoUsuario.Distribuidor).ToList();
                    break;
                case "Evaluador":
                    if (nombreActual == "alonso.sierra.siller@clarios.com")
                        listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.TipoUsuario == TipoUsuario.Distribuidor).ToList();
                    break;

                case "DCAdmin":
                    listaDistribuidores = new XPQuery<Usuario>(session)
                        .FirstOrDefault(x => x.UserName == nombreActual)
                        .DistribuidoresSupervisa.ToList();

                    break;
            }

            List<dtodistr> lstdistr = new List<dtodistr>();
            foreach (var item in listaDistribuidores)
            {
                dtodistr oDist = new dtodistr();
                oDist.id = item.Oid.ToString();
                oDist.nombre = item.UserName;

                lstdistr.Add(oDist);
            }

            lstdistr = lstdistr.OrderBy(x => x.nombre).ToList();

            return JsonConvert.SerializeObject(lstdistr);
        }

        [WebMethod]
        public static string completaTabla(string idtbl)
        {
            DevExpress.Xpo.Session session = Util.getsession();

            if (idtbl != null && idtbl != "")
            {
                var tabla = new XPQuery<TablaFinancieraResultados>(session).Where(x => x.Id.ToString() == idtbl).FirstOrDefault();

                if(tabla != null)
                {
                    tabla.completa = true;
                    tabla.estatus = "Aprobada";

                    tabla.Save();

                    return "1";
                }
            }

            return "-1";
        }
    }


    public class tabladto
    {
        public int id { get; set; }
        public string mub { get; set; }
        public string muo { get; set; }
        public string lrc { get; set; }
        public string lrr { get; set; }
        public string ri { get; set; }
        public string ppc { get; set; }
        public string ppp { get; set; }
        public string e { get; set; }
        public string go { get; set; }
        public string rao { get; set; }
    }
}