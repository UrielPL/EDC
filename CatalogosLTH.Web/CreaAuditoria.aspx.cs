using CatalogosLTH.Module.BusinessObjects;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DevExpress.Xpo;
using Newtonsoft.Json;
using SpreadsheetLight;
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
    public partial class CreaAuditoria : System.Web.UI.Page
    {
        //public static string aud { get; set; }
        public List<v_Pilar> vpilar { get; set; }
        public List<v_Area> varea { get; set; }
        public List<v_Subtema> vsubtema { get; set; }
        public List<v_Punto> vpunto { get; set; }
        public double ptMaxEjec { get; set; }
        public double ptMaxAdmin { get; set; }
        public double ptMaxPlan { get; set; }
        public double ptMaxInfra { get; set; }
        public double ptMaxPYS { get; set; }
        public double puntajeTotal { get; set; }
        public List<dtoPunto> puntosDetalle { get; set; }
        public List<mdl_distribuidor> LstDistribuidores { get; set; }
        public List<dtoresumen> puntosResumen { get; set; }
        public bool haytabla { get; set; }
        public string estatus { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "Autoguardado", "iniciarAutoGuardado();", true);

            {
                string permiso = Util.getPermiso().ToString();
                string nombreActual = Util.getusuario();

                Session["username"] = nombreActual;
                Session["permiso"] = permiso;

                string aud = "";
                if (Request.QueryString["aud"] != null)
                {
                    aud = Request.QueryString["aud"];
                    Session["aud"] = aud;

                }
                else
                {
                    Session["aud"] = aud;
                }

                DevExpress.Xpo.Session session = Util.getsession();
                puntajeTotal = 0.0;
                ptMaxEjec = 0.0;
                ptMaxAdmin = 0.0;
                ptMaxInfra = 0.0;
                ptMaxPlan = 0.0;
                ptMaxPYS = 0.0;

                //btnGuarda.Visible = false;
                haytabla = false;

                vpilar = new XPCollection<v_Pilar>(session).ToList();
                puntosDetalle = new List<dtoPunto>();
                foreach (var pilar in vpilar)
                {
                    foreach (var area in pilar.Areas)
                    {
                        foreach (var subtema in area.Subtemas)
                        {
                            foreach (var punto in subtema.Puntos)
                            {
                                if (punto.Formal == true)
                                {
                                    dtoPunto p = new dtoPunto();
                                    p.idpilar = pilar.Id;
                                    p.idarea = area.Id;
                                    p.idsubtema = subtema.Id;
                                    p.idpunto = punto.Id;
                                    p.nombrepunto = punto.NombrePunto;
                                    p.valor = punto.Valor;
                                    p.habilitana = punto.habilitaNA;
                                    puntosDetalle.Add(p);

                                    switch (p.idpilar)
                                    {
                                        case 1:
                                            ptMaxEjec += punto.Valor;
                                            break;
                                        case 2:
                                            ptMaxAdmin += punto.Valor;
                                            break;
                                        case 3:
                                            ptMaxInfra += punto.Valor;
                                            break;
                                        case 4:
                                            ptMaxPlan += punto.Valor;
                                            break;
                                        case 5:
                                            ptMaxPYS += punto.Valor;
                                            break;
                                    }
                                }
                                
                            }
                        }
                    }
                }
                //var x = 2;

                if (aud == "")
                {
                    string s = (string)HttpContext.Current.Session["username"];

                    string p = (string)HttpContext.Current.Session["permiso"];

                    if (p == "Distribuidor")
                    {
                        LstDistribuidores = new XPQuery<mdl_distribuidor>(session).Where(x => x.nombredist == s).ToList();
                        LstDistribuidores = LstDistribuidores.OrderBy(b => b.nombredist).ToList();
                        foreach (var item in LstDistribuidores)
                        {
                            drpDist.Items.Add(item.nombredist);
                        }

                    }
                    else if (p == "GerenteCuenta")
                    {
                        var user = new XPQuery<Usuario>(session).Where(x => x.UserName == s).FirstOrDefault();
                        var lst = user.Dependientes.ToList();
                        lst = lst.OrderBy(x => x.UserName).ToList();
                        foreach (var item in lst)
                        {
                            drpDist.Items.Add(item.UserName);
                        }
                    }
                    else
                    {
                        LstDistribuidores = new XPQuery<mdl_distribuidor>(session).ToList();
                        LstDistribuidores = LstDistribuidores.OrderBy(b => b.nombredist).ToList();
                        foreach (var item in LstDistribuidores)
                        {
                            drpDist.Items.Add(item.nombredist);
                        }
                    }


                }
                else
                {
                    var auditoria = new XPQuery<NuevaAuditoria>(session).First(a => a.Id == int.Parse(aud));
                    var nadActual = new XPQuery<NuevaAuditoriaDetalle>(session).Where(n => n.Auditoria.Id == auditoria.Id).ToList();
                    drpDist.Items.Add(auditoria.Distribuidor.nombredist);

                    estatus = "INCOMPLETO";
                    if (auditoria.status == "COMPLETADO")
                    {
                        estatus = "COMPLETADO";
                        //btnGuarda.Visible = false;
                        //btnGuardaAvance.Visible = false;
                    }

                    var existetabla = new XPQuery<TablaFinancieraResultados>(session).Where(c => c.IdAuditoria == auditoria).ToList();

                    if (existetabla.Count() > 0 && auditoria.status == "COMPLETADO")
                    {
                        haytabla = true;
                    }
                    
                }


            }
        }

        [WebMethod]
        public static string checkSess()
        {
            string idsesion = (string)HttpContext.Current.Session["sessionID"];

            if (idsesion != null && idsesion != "" && idsesion != HttpContext.Current.Session.SessionID.ToString())
            {
                return "-1";
            }else if (idsesion == HttpContext.Current.Session.SessionID.ToString())
            {

                return "1";
            }else
            {
               return "-1";
            }
        }

        [WebMethod]
        public static string checaPendientes()
        {
            DevExpress.Xpo.Session session = Util.getsession();
            string aud = (string)HttpContext.Current.Session["aud"];

            if (aud != null && aud != "")
            {
                var audi = new XPQuery<NuevaAuditoria>(session).FirstOrDefault(x => x.Id == int.Parse(aud));
                var nadActual = new XPQuery<NuevaAuditoriaDetalle>(session).Where(n => n.Auditoria.Id == int.Parse(aud)).ToList();

                List<string> ptsFalta = new List<string>();


                foreach (var item in nadActual)
                {
                    if (item.Resultado == false && item.Comentarios == "" && item.n_a == false)
                    {
                        ptsFalta.Add(item.Punto.Id.ToString() + ". " + item.Punto.NombrePunto);
                    }
                }

                if (ptsFalta.Count() > 0)
                {
                    return JsonConvert.SerializeObject(ptsFalta);
                }
                else
                {
                    return "1";
                }

            }

            return "-1";
        }

        protected void btnGuarda_Click(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();

            puntosResumen = new List<dtoresumen>();

            string aud = (string)HttpContext.Current.Session["aud"];

            //double puntajeTotal = 0.0;
            double puntajeEjec = 0.0;
            double puntajeAdmin = 0.0;
            double puntajeInfra = 0.0;
            double puntajePlan = 0.0;
            double puntajePyS = 0.0;

            if (aud != null && aud != "")
            {
                puntajeTotal = 0.0;
                var auditoria = new XPQuery<NuevaAuditoria>(session).First(a => a.Id == int.Parse(aud));
                var nadActual = new XPQuery<NuevaAuditoriaDetalle>(session).Where(n => n.Auditoria == auditoria).ToList();

                for (int i = 0; i < puntosDetalle.Count(); i++)
                {
                    bool valor = false;
                    bool nA = false;
                    String grp = Request.Form["califa" + puntosDetalle[i].idpunto.ToString()];
                    String n_a = Request.Form["na" + puntosDetalle[i].idpunto.ToString()];

                    if (grp != null && grp.Equals("on"))
                    {
                        valor = true;
                        puntajeTotal += puntosDetalle[i].valor;

                        switch (puntosDetalle[i].idpilar)
                        {
                            case 1:
                                puntajeEjec += puntosDetalle[i].valor;
                                break;
                            case 2:
                                puntajeAdmin += puntosDetalle[i].valor;
                                break;
                            case 3:
                                puntajeInfra += puntosDetalle[i].valor;
                                break;
                            case 4:
                                puntajePlan += puntosDetalle[i].valor;
                                break;
                            case 5:
                                puntajePyS += puntosDetalle[i].valor;
                                break;
                        }
                    }

                    if (n_a != null && n_a.Equals("on"))
                    {
                        nA = true;
                        puntajeTotal += puntosDetalle[i].valor;
                        switch (puntosDetalle[i].idpilar)
                        {
                            case 1:
                                ptMaxEjec -= puntosDetalle[i].valor;
                                break;
                            case 2:
                                ptMaxAdmin -= puntosDetalle[i].valor;
                                break;
                            case 3:
                                ptMaxInfra -= puntosDetalle[i].valor;
                                break;
                            case 4:
                                ptMaxPlan -= puntosDetalle[i].valor;
                                break;
                            case 5:
                                ptMaxPYS -= puntosDetalle[i].valor;
                                break;
                        }
                    }


                    String coment = Request.Form["com" + puntosDetalle[i].idpunto.ToString()];
                    nadActual[i].Resultado = valor;
                    nadActual[i].n_a = nA;
                    nadActual[i].Save();

                    //LLENA ACTIVIDADES
                    if (valor == false && (n_a == null || n_a == ""))
                    {
                        dtoresumen oRes = new dtoresumen();

                        string namePunto = nadActual[i].Punto.NombrePunto;
                        oRes.punto = nadActual[i].Punto.Subtema.Area.Pilar.Id.ToString() + "." + nadActual[i].Punto.Subtema.Area.Id.ToString() + "." + nadActual[i].Punto.Subtema.Id.ToString() + "." + nadActual[i].Punto.Id.ToString() + "." + namePunto;

                        oRes.comentario = coment;

                        puntosResumen.Add(oRes);

                        foreach (var act in nadActual[i].Punto.Actividades)
                        {
                            NuevaAuditoriaActividad naAct = new NuevaAuditoriaActividad(session);
                            naAct.Idaud = auditoria;
                            naAct.Idactividad = act;
                            naAct.fechainicio = DateTime.Now;
                            naAct.fechafinal = DateTime.Now.AddDays(30);
                            naAct.status = "Por realizar";
                            naAct.vigencia = "Vigente";

                            naAct.Save();
                        }

                    }

                    auditoria.calificacionTotal = Math.Round(puntajeTotal, 2);
                    auditoria.calificacionAdmin = Math.Round(puntajeAdmin, 2) == Math.Round(ptMaxAdmin, 2) ? 100 : Math.Round(((puntajeAdmin * 1) / ptMaxAdmin) * 100, 2);
                    auditoria.calificacionEjec = Math.Round(puntajeEjec, 2) == Math.Round(ptMaxEjec, 2) ? 100 : Math.Round(((puntajeEjec * 1) / ptMaxEjec) * 100, 2);
                    auditoria.calificacionInfra = Math.Round(puntajeInfra, 2) == Math.Round(ptMaxInfra, 2) ? 100 : Math.Round(((puntajeInfra * 1) / ptMaxInfra) * 100, 2);
                    auditoria.calificacionPlan = Math.Round(puntajePlan, 2) == Math.Round(ptMaxPlan, 2) ? 100 : Math.Round(((puntajePlan * 1) / ptMaxPlan) * 100, 2);
                    auditoria.calificacionPyS = Math.Round(puntajePyS, 2) == Math.Round(ptMaxPYS, 2) ? 100 : Math.Round(((puntajePyS * 1) / ptMaxPYS) * 100, 2);
                    auditoria.calificacionFinal = Math.Round(puntajeTotal, 2);
                    auditoria.status = "COMPLETADO";
                    auditoria.Save();


                    //contenido.Attributes.CssStyle.Add("display", "none");
                   // resumen.Attributes.CssStyle.Add("display", "normal");
                    //btnGuarda.Visible = false;
                    //btnGuardaAvance.Visible = false;
                    //nnivel.HRef = "NuevoNivel.aspx?u=" + auditoria.Id.ToString();
                }
            }
            else
            {

                var xdist = drpDist.SelectedItem.Text;
                mdl_distribuidor md = new XPQuery<mdl_distribuidor>(session).Where(c => c.nombredist == xdist).First();
                NuevaAuditoria na = new NuevaAuditoria(session);
                na.Fecha = DateTime.Today;
                na.Distribuidor = md;
                na.Save();

                puntajeTotal = 0.0;

                foreach (var pt in puntosDetalle)
                {
                    bool valor = false;
                    bool nA = false;
                    String grp = Request.Form["califa" + pt.idpunto.ToString()];
                    String n_a = Request.Form["na" + pt.idpunto.ToString()];

                    if (grp != null && grp.Equals("on"))
                    {
                        valor = true;
                        puntajeTotal += pt.valor;

                        switch (pt.idpilar)
                        {
                            case 1:
                                puntajeEjec += pt.valor;
                                break;
                            case 2:
                                puntajeAdmin += pt.valor;
                                break;
                            case 3:
                                puntajeInfra += pt.valor;
                                break;
                            case 4:
                                puntajePlan += pt.valor;
                                break;
                            case 5:
                                puntajePyS += pt.valor;
                                break;
                        }
                    }

                    if (n_a != null && n_a.Equals("on"))
                    {
                        nA = true;
                        puntajeTotal += pt.valor;
                        switch (pt.idpilar)
                        {
                            case 1:
                                ptMaxEjec -= pt.valor;
                                break;
                            case 2:
                                ptMaxAdmin -= pt.valor;
                                break;
                            case 3:
                                ptMaxInfra -= pt.valor;
                                break;
                            case 4:
                                ptMaxPlan -= pt.valor;
                                break;
                            case 5:
                                ptMaxPYS -= pt.valor;
                                break;
                        }
                    }


                    String coment = Request.Form["com" + pt.idpunto.ToString()];
                    //if(valor == false && coment == "")
                    //{
                    //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", "alert('Debe llenar los campos de comentarios obligatorios')", true);

                    //    return;
                    //}
                    v_Punto p = new XPQuery<v_Punto>(session).FirstOrDefault(x => x.Id == pt.idpunto);
                    NuevaAuditoriaDetalle nad = new NuevaAuditoriaDetalle(session);
                    nad.Auditoria = na;
                    nad.Punto = p;
                    nad.Resultado = valor;
                    nad.n_a = nA;
                    nad.Comentarios = coment;
                    nad.Save();

                    //LLENA ACTIVIDADES
                    if (valor == false && (n_a == null || n_a == ""))
                    {
                        dtoresumen oResu = new dtoresumen();
                        string namePunto = p.NombrePunto;
                        oResu.punto = p.Subtema.Area.Pilar.Id.ToString() + "." + p.Subtema.Area.Id.ToString() + "." + p.Subtema.Id.ToString() + "." + p.Id.ToString() + "." + namePunto;

                        oResu.comentario = coment;

                        puntosResumen.Add(oResu);

                        foreach (var act in p.Actividades)
                        {
                            NuevaAuditoriaActividad naAct = new NuevaAuditoriaActividad(session);
                            naAct.Idaud = na;
                            naAct.Idactividad = act;
                            naAct.fechainicio = DateTime.Now;
                            naAct.fechafinal = DateTime.Now.AddDays(30);
                            naAct.status = "Por realizar";
                            naAct.vigencia = "Vigente";

                            naAct.Save();
                        }

                    }
                }
                na.calificacionTotal = Math.Round(puntajeTotal, 2);
                na.calificacionAdmin = Math.Round(puntajeAdmin, 2) == Math.Round(ptMaxAdmin, 2) ? 100 : Math.Round(((puntajeAdmin * 1) / ptMaxAdmin) * 100, 2);
                na.calificacionEjec = Math.Round(puntajeEjec, 2) == Math.Round(ptMaxEjec, 2) ? 100 : Math.Round(((puntajeEjec * 1) / ptMaxEjec) * 100, 2);
                na.calificacionInfra = Math.Round(puntajeInfra, 2) == Math.Round(ptMaxInfra, 2) ? 100 : Math.Round(((puntajeInfra * 1) / ptMaxInfra) * 100, 2);
                na.calificacionPlan = Math.Round(puntajePlan, 2) == Math.Round(ptMaxPlan, 2) ? 100 : Math.Round(((puntajePlan * 1) / ptMaxPlan) * 100, 2);
                na.calificacionPyS = Math.Round(puntajePyS, 2) == Math.Round(ptMaxPYS, 2) ? 100 : Math.Round(((puntajePyS * 1) / ptMaxPYS) * 100, 2);
                na.calificacionFinal = Math.Round(puntajeTotal, 2);

                na.status = "COMPLETADO";
                na.Save();

                //contenido.Attributes.CssStyle.Add("display", "none");
                //resumen.Attributes.CssStyle.Add("display", "normal");
                //nnivel.HRef = "NuevoNivel.aspx?u=" + na.Id.ToString();
            }
        }

        protected void GuardaAvance_Clicked(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            string aud = (string)HttpContext.Current.Session["aud"];

            if (aud != null && aud != "")
            {
                string ptsSinComentario = "";

                bool incompleto = false;
                var audiActual = new XPQuery<NuevaAuditoria>(session).First(x => x.Id == int.Parse(aud));
                var nadActual = new XPQuery<NuevaAuditoriaDetalle>(session).Where(n => n.Auditoria == audiActual).ToList();

                audiActual.status = "INCOMPLETO";

                for (int i = 0; i < puntosDetalle.Count(); i++)
                {
                    bool valor = false;
                    bool nAplica = false;
                    String grp = Request.Form["califa" + puntosDetalle[i].idpunto.ToString()];
                    String n_a = Request.Form["na" + puntosDetalle[i].idpunto.ToString()];


                    if (grp != null && grp.Equals("on"))
                    {
                        valor = true;
                    }

                    if (n_a != null && n_a.Equals("on"))
                    {
                        nAplica = true;
                    }
                    String coment = Request.Form["com" + puntosDetalle[i].idpunto.ToString()];
                    if (valor == false && coment == "" && nAplica == false)
                    {
                        incompleto = true;

                        string temp = puntosDetalle[i].nombrepunto;
                        ptsSinComentario = ptsSinComentario + @"\ \n -" + temp;

                    }

                    if (puntosDetalle[i].idpunto == nadActual[i].Punto.Id)
                    {
                        nadActual[i].Resultado = valor;
                        nadActual[i].n_a = nAplica;
                        nadActual[i].Comentarios = coment;
                    }

                    nadActual[i].Save();
                }

                //var R1 = r1.Attributes;
                //var R2 = r2.InnerText;
                //var R3 = r3.InnerText;
                //var R4 = r4.InnerText;
                //var R5 = r4.InnerText;
                //var R6 = r6.InnerText;
                //var R7 = r7.InnerText;
                //var R8 = r8.InnerText;
                //var R9 = r9.InnerText;
                //var R10 = r10.InnerText;

                //TablaFinancieraResultados nTabla = new XPQuery<TablaFinancieraResultados>(session).First(x => x.IdAuditoria == audiActual);
                //nTabla.IdAuditoria = audiActual;
                ////nTabla.areaMUB = "Margen de utilidad bruta"; nTabla.respuestaMUB = R1;
                //nTabla.areaMUO = "Margen de utilidad operativa"; nTabla.respuestaMUO = R2;
                //nTabla.areaLRC = "Liquidez (razón circulante)"; nTabla.respuestaLRC = R3;
                //nTabla.areaLRR = "Liquidez (razón rápida)"; nTabla.respuestaLRR = R4;
                //nTabla.areaRI = "Rotación de inventario"; nTabla.respuestaRI = R5;
                //nTabla.areaPPC = "Periodo promedio de cobro"; nTabla.respuestaPPC = R6;
                //nTabla.areaPPP = "Periodo promedio de pago"; nTabla.respuestaPPP = R7;
                //nTabla.areaE = "Endeudamiento"; nTabla.respuestaE = R8;
                //nTabla.areaGO = "Gastos operativos"; nTabla.respuestaGO = R9;
                //nTabla.areaRAO = "Rendimiento sobre activos operativos"; nTabla.respuestaRAO = R10;

                //nTabla.Save();

                if (incompleto == false)
                {
                    //btnGuarda.Visible = true;
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", script, true);
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", "alert('Debe llenar los campos de comentarios obligatorios: " + ptsSinComentario + "')", true);

                    Response.Redirect("CreaAuditoria.aspx?aud=" + aud);

                }
                //btnGuarda.Visible = true;
                //Response.Redirect("AuditoriasNuevas.aspx?");

            }
            else
            {
                string ptsSinComentario = "";

                string username = (string)HttpContext.Current.Session["username"];
                bool incompleto = false;
                var xdist = drpDist.SelectedItem.Text;
                mdl_distribuidor md = new XPQuery<mdl_distribuidor>(session).Where(c => c.nombredist == xdist).First();
                NuevaAuditoria na = new NuevaAuditoria(session);
                na.Fecha = DateTime.Today;
                na.Distribuidor = md;
                na.status = "INCOMPLETO";

                if (md.nombredist == username)
                {
                    na.autoAuditoria = true;
                }

                Usuario user = new XPQuery<Usuario>(session).Where(x => x.UserName == username).FirstOrDefault();
                if (user != null)
                {
                    na.User_Apertura = user;
                }

                na.Save();
                Session["aud"] = na.Id;
                //aud = na.Id.ToString();

                foreach (var pt in puntosDetalle)
                {

                    bool valor = false;
                    bool nAplica = false;
                    String grp = Request.Form["califa" + pt.idpunto.ToString()];
                    String n_a = Request.Form["na" + pt.idpunto.ToString()];


                    if (grp != null && grp.Equals("on"))
                    {
                        valor = true;
                    }

                    if (n_a != null && n_a.Equals("on"))
                    {
                        nAplica = true;
                    }


                    String coment = Request.Form["com" + pt.idpunto.ToString()];

                    if (valor == false && coment == "" && nAplica == false)
                    {
                        incompleto = true;

                        string temp = pt.nombrepunto;
                        ptsSinComentario = ptsSinComentario + @"\ \n -" + temp;
                    }

                    v_Punto p = new XPQuery<v_Punto>(session).FirstOrDefault(x => x.Id == pt.idpunto);
                    NuevaAuditoriaDetalle nad = new NuevaAuditoriaDetalle(session);
                    nad.Auditoria = na;
                    nad.Punto = p;
                    nad.Resultado = valor;
                    nad.n_a = nAplica;
                    nad.Comentarios = coment;
                    nad.Save();

                }

                //var R1 = r1.Attributes;
                //var R2 = r2.InnerText;
                //var R3 = r3.InnerText;
                //var R4 = r4.InnerText;
                //var R5 = r4.InnerText;
                //var R6 = r6.InnerText;
                //var R7 = r7.InnerText;
                //var R8 = r8.InnerText;
                //var R9 = r9.InnerText;
                //var R10 = r10.InnerText;

                TablaFinancieraResultados nTabla = new TablaFinancieraResultados(session);
                nTabla.IdAuditoria = na;
                nTabla.realizo = username;
                nTabla.fechaRealizada = DateTime.Now;
                ////nTabla.areaMUB = "Margen de utilidad bruta"; nTabla.respuestaMUB = R1;
                //nTabla.areaMUO = "Margen de utilidad operativa"; nTabla.respuestaMUO = R2;
                //nTabla.areaLRC = "Liquidez (razón circulante)"; nTabla.respuestaLRC = R3;
                //nTabla.areaLRR = "Liquidez (razón rápida)"; nTabla.respuestaLRR = R4;
                //nTabla.areaRI = "Rotación de inventario"; nTabla.respuestaRI = R5;
                //nTabla.areaPPC = "Periodo promedio de cobro"; nTabla.respuestaPPC = R6;
                //nTabla.areaPPP = "Periodo promedio de pago"; nTabla.respuestaPPP = R7;
                //nTabla.areaE = "Endeudamiento"; nTabla.respuestaE = R8;
                //nTabla.areaGO = "Gastos operativos"; nTabla.respuestaGO = R9;
                //nTabla.areaRAO = "Rendimiento sobre activos operativos"; nTabla.respuestaRAO = R10;

                nTabla.Save();

                if (incompleto == false)
                {
                    //btnGuarda.Visible = true;
                    Response.Redirect("CreaAuditoria.aspx?aud=" + na.Id);
                }
                else
                {
                    //string script = "<script type='text/javascript'>Swal.fire({title: 'Faltan campos por llenar',text: '" + ptsSinComentario + "',icon: 'info',confirmButtonText:'<i class='fa fa-thumbs-up'></i> ok!',confirmButtonAriaLabel: 'Ok!'});</script>";

                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", "alert('Debe llenar los campos de comentarios obligatorios: " + ptsSinComentario + "')", true);
                    //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", script, true);
                    Response.Redirect("CreaAuditoria.aspx?aud=" + na.Id);
                }

                //Response.Redirect("AuditoriasNuevas.aspx");
            }
        }

        [WebMethod]
        public static string traeCondiciones(string idPunto)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            var condiciones = new XPQuery<v_Condicion>(session).Where(x => x.Punto.Id == int.Parse(idPunto)).ToList();

            if (condiciones != null)
            {
                List<dtoCondiciones> lstCondiciones = new List<dtoCondiciones>();

                foreach (var item in condiciones)
                {
                    dtoCondiciones condicion = new dtoCondiciones();
                    condicion.idpunto = item.Punto.Id;
                    condicion.idcondicion = item.Id;
                    condicion.nombre = item.NombreCondicion;

                    lstCondiciones.Add(condicion);
                }

                return JsonConvert.SerializeObject(lstCondiciones);
            }

            return JsonConvert.SerializeObject(condiciones);


        }

        [WebMethod]
        public static string traePuntos(string aud)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            //string aud = Session["aud"].ToString();

            if (aud != null && aud != "")
            {
                var ptsAuditoria = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == int.Parse(aud)).ToList();

                List<dtonaDetalle> lst = new List<dtonaDetalle>();
                foreach (var item in ptsAuditoria)
                {
                    dtonaDetalle obj = new dtonaDetalle();
                    obj.id = item.Id;
                    obj.idauditoria = item.Auditoria.Id;
                    obj.punto = item.Punto.Id;
                    obj.resultado = item.Resultado;
                    obj.n_a = item.n_a;
                    obj.comentarios = item.Comentarios;

                    lst.Add(obj);
                }
                return JsonConvert.SerializeObject(lst);
            }

            return "-1";
        }

        [WebMethod]
        public static string PuntoxResp(string resp)
        {
            DevExpress.Xpo.Session session = Util.getsession();

            resp = resp.Replace(" ", "_");
            List<v_Punto> vpuntos = new List<v_Punto>();

            if (resp == "TODOS")
            {
                vpuntos = new XPQuery<v_Punto>(session).Where(x => x.Subtema != null && x.Formal == true).ToList();
            }
            else
            {
                vpuntos = new XPQuery<v_Punto>(session).Where(x => x.FiguraResponsable.ToString() == resp && x.Subtema != null && x.Formal == true).ToList();
            }

            List<dtoPunto> lst = new List<dtoPunto>();
            foreach (var item in vpuntos)
            {
                dtoPunto opto = new dtoPunto();
                opto.idarea = item.Subtema.Area.Id;
                opto.idpilar = item.Subtema.Area.Pilar.Id;
                opto.idsubtema = item.Subtema.Id;
                opto.idpunto = item.Id;
                opto.nombrepunto = item.NombrePunto;
                opto.valor = item.Valor;

                lst.Add(opto);
            }

            return JsonConvert.SerializeObject(lst);
        }

        [WebMethod]
        public static string guardaTabla(string aud, string r1, string r2, string r3, string r4, string r5, string r6, string r7, string r8, string r9, string r10,
            string ubruta, string vneta, string uoperativa, string vneta2, string acirc, string pcirc, string acinv, string pcirc2, string inv, string cvdias,
            string cxc, string vcredper, string cxp, string cvper, string ptot, string atot, string goper, string vnet, string uoper, string atotpas, string archivo, string nombre, bool completa)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            if (aud != null && aud != "")
            {
                var auditoria = new XPQuery<NuevaAuditoria>(session).First(x => x.Id.ToString() == aud);
                TablaFinancieraResultados nTabla = new XPQuery<TablaFinancieraResultados>(session).First(x => x.IdAuditoria == auditoria);
                nTabla.IdAuditoria = auditoria;
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

                nTabla.completa = completa;

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

                archivodto obj = new archivodto();
                obj.url = nTabla.urlArchivo;
                obj.nombre = nTabla.nombreArchivo;

                return JsonConvert.SerializeObject(obj);
            }
            else
            {
                return "-1";
            }


        }

        [WebMethod]
        public static string traeTablaF(string aud)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            if (aud != null && aud != "")
            {
                var tabla = new XPQuery<TablaFinancieraResultados>(session).Where(x => x.IdAuditoria.Id.ToString() == aud).First();

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

                temp.url = tabla.urlArchivo;
                temp.nombre = tabla.nombreArchivo;

                return JsonConvert.SerializeObject(temp);

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
        public static string saveDatos(string[] switchs, string[] comentarios, string[] noAplica, string distribuidor)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            string aud = (string)HttpContext.Current.Session["aud"];
            string idsesion = (string)HttpContext.Current.Session["sessionID"];

            if(idsesion != null && idsesion != "" && idsesion != HttpContext.Current.Session.SessionID.ToString())
            {
                return "-2";
            }

            //TRAE TODOS LOS PUNTOS
            var vpilar = new XPCollection<v_Pilar>(session).ToList();
            var puntosDetalle = new List<dtoPunto>();
            foreach (var pilar in vpilar)
            {
                foreach (var area in pilar.Areas)
                {
                    foreach (var subtema in area.Subtemas)
                    {
                        foreach (var punto in subtema.Puntos)
                        {
                            if(punto.Formal == true)
                            {
                                dtoPunto p = new dtoPunto();
                                p.idpilar = pilar.Id;
                                p.idarea = area.Id;
                                p.idsubtema = subtema.Id;
                                p.idpunto = punto.Id;
                                p.nombrepunto = punto.NombrePunto;
                                p.valor = punto.Valor;
                                p.habilitana = punto.habilitaNA;
                                puntosDetalle.Add(p);
                            }
                            
                        }
                    }
                }
            }

            try
            {
                if (aud != null && aud != "") //SI ES UNA AUDITORIA EMPEZADA
                {
                    var audiActual = new XPQuery<NuevaAuditoria>(session).First(x => x.Id == int.Parse(aud));
                    var nadActual = new XPQuery<NuevaAuditoriaDetalle>(session).Where(n => n.Auditoria == audiActual).ToList();

                    audiActual.status = "INCOMPLETO";
                    string estatus = "COMPLETADO";

                    for (int i = 0; i < puntosDetalle.Count(); i++)
                    {
                        bool sw = false;
                        bool no_aplica = false;
                        string coments = "";

                        // EVALUA EL SWITCH
                        string idpuntosplit = switchs[i].Split('-')[0];
                        string califapunto = "califa" + puntosDetalle[i].idpunto.ToString();

                        if (califapunto == idpuntosplit)
                        {
                            if (switchs[i].Split('-')[1] == "si")
                            {
                                sw = true;
                            }
                        }

                        //EVALUA EL N/A
                        //string idpunto = noAplica[i].Split('-')[0];
                        string naPunto = "na" + puntosDetalle[i].idpunto.ToString();

                        if (noAplica.Contains(naPunto + "-si"))
                        {
                            no_aplica = true;
                        }

                        //EVALUA COMENTARIOS
                        string idpuntocoment = comentarios[i].Split('-')[0];
                        string comPunto = "com" + puntosDetalle[i].idpunto.ToString();

                        if (comPunto == idpuntocoment)
                        {
                            if (comentarios[i].Split('-')[1] != null && comentarios[i].Split('-')[1] != "")
                            {
                                coments = comentarios[i].Split('-')[1];
                            }
                        }

                        if (puntosDetalle[i].idpunto == nadActual[i].Punto.Id)
                        {
                            nadActual[i].Resultado = sw;
                            nadActual[i].n_a = no_aplica;
                            nadActual[i].Comentarios = coments;

                            nadActual[i].Save();
                        }

                        if (estatus == "COMPLETADO") //SI AUN NO SE HA CAMBIADO ESE ESTATUS
                        {
                            if (sw == false && no_aplica == false && (coments == null || coments == ""))
                            {
                                estatus = "INCOMPLETO";
                            }
                        }

                    }

                    var resp = new
                    {
                        aud = aud,
                        estatus = estatus
                    };

                    return JsonConvert.SerializeObject(resp);
                }
                else //SI ES UNA NUEVA AUDITORIA
                {
                    string username = (string)HttpContext.Current.Session["username"];
                    bool incompleto = false;
                    var xdist = distribuidor;
                    mdl_distribuidor md = new XPQuery<mdl_distribuidor>(session).Where(c => c.nombredist == xdist).First();
                    NuevaAuditoria na = new NuevaAuditoria(session);
                    na.Fecha = DateTime.Today;
                    na.Distribuidor = md;
                    na.status = "INCOMPLETO";

                    if (md.nombredist == username)
                    {
                        na.autoAuditoria = true;
                    }

                    Usuario user = new XPQuery<Usuario>(session).Where(x => x.UserName == username).FirstOrDefault();
                    if (user != null)
                    {
                        na.User_Apertura = user;
                    }

                    na.Save();
                    HttpContext.Current.Session["aud"] = na.Id;

                    //LLENA LOS PUNTOS
                    for (int i = 0; i < puntosDetalle.Count(); i++)
                    {
                        bool sw = false;
                        bool no_aplica = false;
                        string coments = "";

                        // EVALUA EL SWITCH
                        string idpuntosplit = switchs[i].Split('-')[0];
                        string califapunto = "califa" + puntosDetalle[i].idpunto.ToString();

                        if (califapunto == idpuntosplit)
                        {
                            if (switchs[i].Split('-')[1] == "si")
                            {
                                sw = true;
                            }
                        }

                        //EVALUA EL N/A
                        //string idpunto = noAplica[i].Split('-')[0];
                        string naPunto = "na" + puntosDetalle[i].idpunto.ToString();

                        if (noAplica.Contains(naPunto + "-si"))
                        {
                            no_aplica = true;
                        }

                        //EVALUA COMENTARIOS
                        string idpuntocoment = comentarios[i].Split('-')[0];
                        string comPunto = "com" + puntosDetalle[i].idpunto.ToString();

                        if (comPunto == idpuntocoment)
                        {
                            if (comentarios[i].Split('-')[1] != null && comentarios[i].Split('-')[1] != "")
                            {
                                coments = comentarios[i].Split('-')[1];
                            }
                        }

                        if ((coments == null || coments == "") && no_aplica == false && sw == false)
                        {
                            incompleto = true;
                            
                        }

                        v_Punto p = new XPQuery<v_Punto>(session).FirstOrDefault(x => x.Id == puntosDetalle[i].idpunto);
                        NuevaAuditoriaDetalle nad = new NuevaAuditoriaDetalle(session);
                        nad.Auditoria = na;
                        nad.Punto = p;
                        nad.Resultado = sw;
                        nad.n_a = no_aplica;
                        nad.Comentarios = coments;
                        nad.Save();
                    }


                    //GUARDA UNA TABLA FINANCIERA
                    TablaFinancieraResultados nTabla = new TablaFinancieraResultados(session);
                    nTabla.IdAuditoria = na;
                    nTabla.realizo = username;
                    nTabla.fechaRealizada = DateTime.Now;

                    nTabla.Save();

                    if (incompleto == false)
                    {
                        
                        //btnGuarda.Visible = true;
                        //Response.Redirect("CreaAuditoria.aspx?aud=" + na.Id);
                    }
                    else
                    {
                        
                    }

                    return na.Id.ToString();
                }
            }catch(Exception e)
            {
                return "-1";
            }
            
            
        }

        [WebMethod]
        public static string finaliza(string[] switchs, string[] comentarios, string[] noAplica, string distribuidor)
        {
            DevExpress.Xpo.Session session = Util.getsession();

            string idsesion = (string)HttpContext.Current.Session["sessionID"];

            if (idsesion != null && idsesion != "" && idsesion != HttpContext.Current.Session.SessionID.ToString())
            {
                return "-2";
            }

            double ptMaxEjec = 0.0;
            double ptMaxAdmin = 0.0;
            double ptMaxPlan = 0.0;
            double ptMaxInfra = 0.0;
            double ptMaxPYS = 0.0;
            double puntajeTotal = 0.0;

            var puntosResumen = new List<dtoresumen>();
            //string aud = (string)HttpContext.Current.Session["aud"];
            string aud = "";

            var q = HttpContext.Current.Request.UrlReferrer.Query;
            
            if (q.Contains("aud"))
            {
                var arr = q.Split('=');
                aud = arr[1];
            }

            var vpilar = new XPCollection<v_Pilar>(session).ToList();
            var puntosDetalle = new List<dtoPunto>();
            foreach (var pilar in vpilar)
            {
                foreach (var area in pilar.Areas)
                {
                    foreach (var subtema in area.Subtemas)
                    {
                        foreach (var punto in subtema.Puntos)
                        {
                            if (punto.Formal == true)
                            {
                                dtoPunto p = new dtoPunto();
                                p.idpilar = pilar.Id;
                                p.idarea = area.Id;
                                p.idsubtema = subtema.Id;
                                p.idpunto = punto.Id;
                                p.nombrepunto = punto.NombrePunto;
                                p.valor = punto.Valor;
                                p.habilitana = punto.habilitaNA;
                                puntosDetalle.Add(p);


                                switch (p.idpilar)
                                {
                                    case 1:
                                        ptMaxEjec += punto.Valor;
                                        break;
                                    case 2:
                                        ptMaxAdmin += punto.Valor;
                                        break;
                                    case 3:
                                        ptMaxInfra += punto.Valor;
                                        break;
                                    case 4:
                                        ptMaxPlan += punto.Valor;
                                        break;
                                    case 5:
                                        ptMaxPYS += punto.Valor;
                                        break;
                                }
                            }
                            
                        }
                    }
                }
            }

            //double puntajeTotal = 0.0;
            double puntajeEjec = 0.0;
            double puntajeAdmin = 0.0;
            double puntajeInfra = 0.0;
            double puntajePlan = 0.0;
            double puntajePyS = 0.0;

            if (aud != null && aud != "")
            {
                var auditoria = new XPQuery<NuevaAuditoria>(session).First(a => a.Id == int.Parse(aud));
                var nadActual = new XPQuery<NuevaAuditoriaDetalle>(session).Where(n => n.Auditoria == auditoria).ToList();

                for (int i = 0; i < puntosDetalle.Count(); i++)
                {
                    bool sw = false;
                    bool no_aplica = false;
                    string coments = "";

                    // EVALUA EL SWITCH
                    string idpuntosplit = switchs[i].Split('-')[0];
                    string califapunto = "califa" + puntosDetalle[i].idpunto.ToString();

                    if (califapunto == idpuntosplit)
                    {
                        if (switchs[i].Split('-')[1] == "si")
                        {
                            sw = true;
                            puntajeTotal += puntosDetalle[i].valor;

                            switch (puntosDetalle[i].idpilar)
                            {
                                case 1:
                                    puntajeEjec += puntosDetalle[i].valor;
                                    break;
                                case 2:
                                    puntajeAdmin += puntosDetalle[i].valor;
                                    break;
                                case 3:
                                    puntajeInfra += puntosDetalle[i].valor;
                                    break;
                                case 4:
                                    puntajePlan += puntosDetalle[i].valor;
                                    break;
                                case 5:
                                    puntajePyS += puntosDetalle[i].valor;
                                    break;
                            }
                        }
                    }

                    //EVALUA EL N/A
                    //string idpunto = noAplica[i].Split('-')[0];
                    string naPunto = "na" + puntosDetalle[i].idpunto.ToString();

                    if (noAplica.Contains(naPunto + "-si"))
                    {
                        no_aplica = true;
                        puntajeTotal += puntosDetalle[i].valor;
                        switch (puntosDetalle[i].idpilar)
                        {
                            case 1:
                                ptMaxEjec -= puntosDetalle[i].valor;
                                break;
                            case 2:
                                ptMaxAdmin -= puntosDetalle[i].valor;
                                break;
                            case 3:
                                ptMaxInfra -= puntosDetalle[i].valor;
                                break;
                            case 4:
                                ptMaxPlan -= puntosDetalle[i].valor;
                                break;
                            case 5:
                                ptMaxPYS -= puntosDetalle[i].valor;
                                break;
                        }
                    }

                    //EVALUA COMENTARIOS
                    string idpuntocoment = comentarios[i].Split('-')[0];
                    string comPunto = "com" + puntosDetalle[i].idpunto.ToString();

                    if (comPunto == idpuntocoment)
                    {
                        if (comentarios[i].Split('-')[1] != null && comentarios[i].Split('-')[1] != "")
                        {
                            coments = comentarios[i].Split('-')[1];
                        }
                    }

                    nadActual[i].Resultado = sw;
                    nadActual[i].n_a = no_aplica;
                    nadActual[i].Save();

                    //LLENA ACTIVIDADES
                    if (sw == false && no_aplica == false)
                    {
                        dtoresumen oRes = new dtoresumen();

                        string namePunto = nadActual[i].Punto.NombrePunto;
                        oRes.punto = nadActual[i].Punto.Subtema.Area.Pilar.Id.ToString() + "." + nadActual[i].Punto.Subtema.Area.Id.ToString() + "." + nadActual[i].Punto.Subtema.Id.ToString() + "." + nadActual[i].Punto.Id.ToString() + "." + namePunto;

                        oRes.comentario = coments;

                        puntosResumen.Add(oRes);


                        var actis = nadActual[i].Punto.Actividades.Where(a => a.activo == true).ToList();

                        foreach (var act in actis)
                        {
                            NuevaAuditoriaActividad naAct = new NuevaAuditoriaActividad(session);
                            naAct.Idaud = auditoria;
                            naAct.Idactividad = act;
                            naAct.fechainicio = DateTime.Now;
                            naAct.fechafinal = DateTime.Now.AddDays(act.Vigencia * 7); //Vigencia en semanas * 7 dias
                            naAct.status = "Por realizar";
                            naAct.vigencia = "Vigente";

                            if(act.Punto.Evaluador != null)
                            {
                                naAct.Evaluador = act.Punto.Evaluador;
                            }else
                            {
                                if (auditoria.Distribuidor != null)
                                {
                                    var dist = auditoria.Distribuidor;
                                    var user = new XPQuery<Usuario>(session).Where(c => c.UserName == dist.nombredist).FirstOrDefault();

                                    if (user != null && user.Jefe != null)
                                    {
                                        naAct.Evaluador = user.Jefe;

                                    }
                                    else
                                    {
                                        naAct.Evaluador = null;
                                    }

                                }
                                else
                                {
                                    naAct.Evaluador = null;
                                }
                            }

                            naAct.Save();
                        }
                    }

                    if(nadActual[i].Punto.Id == 23 && sw == true)
                    {
                        foreach (var act in nadActual[i].Punto.Actividades)
                        {
                            NuevaAuditoriaActividad naAct = new NuevaAuditoriaActividad(session);
                            naAct.Idaud = auditoria;
                            naAct.Idactividad = act;
                            naAct.fechainicio = DateTime.Now;
                            naAct.fechafinal = DateTime.Now.AddDays(act.Vigencia * 7); //Vigencia en semanas * 7 dias
                            naAct.status = "Por realizar";
                            naAct.vigencia = "Vigente";

                            if (act.Punto.Evaluador != null)
                            {
                                naAct.Evaluador = act.Punto.Evaluador;
                            }
                            else
                            {
                                if (auditoria.Distribuidor != null)
                                {
                                    var dist = auditoria.Distribuidor;
                                    var user = new XPQuery<Usuario>(session).Where(c => c.UserName == dist.nombredist).FirstOrDefault();

                                    if (user != null && user.Jefe != null)
                                    {
                                        naAct.Evaluador = user.Jefe;

                                    }
                                    else
                                    {
                                        naAct.Evaluador = null;
                                    }

                                }
                                else
                                {
                                    naAct.Evaluador = null;
                                }
                            }

                            naAct.Save();
                        }
                    }
                }

                auditoria.calificacionTotal = Math.Round(puntajeTotal, 2);
                auditoria.calificacionAdmin = Math.Round(puntajeAdmin, 2) == Math.Round(ptMaxAdmin, 2) ? 100 : Math.Round(((puntajeAdmin * 1) / ptMaxAdmin) * 100, 2);
                auditoria.calificacionEjec = Math.Round(puntajeEjec, 2) == Math.Round(ptMaxEjec, 2) ? 100 : Math.Round(((puntajeEjec * 1) / ptMaxEjec) * 100, 2);
                auditoria.calificacionInfra = Math.Round(puntajeInfra, 2) == Math.Round(ptMaxInfra, 2) ? 100 : Math.Round(((puntajeInfra * 1) / ptMaxInfra) * 100, 2);
                auditoria.calificacionPlan = Math.Round(puntajePlan, 2) == Math.Round(ptMaxPlan, 2) ? 100 : Math.Round(((puntajePlan * 1) / ptMaxPlan) * 100, 2);
                auditoria.calificacionPyS = Math.Round(puntajePyS, 2) == Math.Round(ptMaxPYS, 2) ? 100 : Math.Round(((puntajePyS * 1) / ptMaxPYS) * 100, 2);
                auditoria.calificacionFinal = Math.Round(puntajeTotal, 2);
                auditoria.status = "COMPLETADO";
                auditoria.Fecha_Cierre = DateTime.Now;
                auditoria.Save();

                string x = "Se crearan actividades para estos puntos:";
                if (Math.Round(puntajeTotal, 2) == 100)
                {
                    x = "No se realizaran actividades";
                }
                var resp = new
                {
                    puntajeAuditoria = auditoria.calificacionTotal,
                    creaActividades = x,
                    puntos = puntosResumen,
                    nnivel = "NuevoNivel.aspx?u=" + auditoria.Id.ToString()
                };

                return JsonConvert.SerializeObject(resp);
            }

            return "-1";
        }

        //[WebMethod]
        //public static string exportaExcel(string[] switchs, string[] comentarios, string[] noAplica)
        //{
        //    string pathfile = AppDomain.CurrentDomain.BaseDirectory + "\\Archivos\\Excels\\archivo.xlsx";

        //    SLDocument osldoc = new SLDocument();

        //    System.Data.DataTable dt = new System.Data.DataTable();

        //    //columnas
        //    dt.Columns.Add("PUNTO", typeof(string));
        //    dt.Columns.Add("CHECKED", typeof(string));
        //    dt.Columns.Add("NA", typeof(string));
        //    dt.Columns.Add("COMENTARIOS", typeof(string));

        //    //registros, rows


        //    foreach (var pt in puntosDetalle)
        //    {
        //        dtocelda oCelda = new dtocelda();
        //        oCelda.punto = pt.idpunto.ToString() + "." + pt.nombrepunto;

        //        var idpunto = pt.idpunto;

        //        if (switchs.Contains("califa" + idpunto.ToString()))
        //        {
        //            oCelda.check = "SI";
        //        }else { oCelda.check = "NO"; }

        //        if (noAplica.Contains("na" + idpunto.ToString()))
        //        {
        //            oCelda.na = "SI";
        //        }else { oCelda.na = "NO"; }

        //        for (int i = 0; i < comentarios.Count(); i++)
        //        {
        //            var split = comentarios[i].Split('-');
        //            var namecoment = split[0];
        //            var idcoment = namecoment.Substring(3);
        //            if (idpunto.ToString() == idcoment)
        //            {
        //                oCelda.comentarios = split[1];
        //                break;
        //            }
        //        }

        //        dt.Rows.Add(oCelda.punto,oCelda.check,oCelda.na, oCelda.comentarios);

        //    }

        //    osldoc.ImportDataTable(1, 1, dt, true);

        //    osldoc.SaveAs(pathfile);

        //    var ruta = pathfile.Replace("\\", "/");
        //    return ruta;

        //}

        //[WebMethod]
        //public static void DescargarExcel(string filePath)
        //{
        //    // Verifica si el archivo existe
        //    if (System.IO.File.Exists(filePath))
        //    {
        //        // Lee el archivo en bytes
        //        byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

        //        // Establece el tipo de contenido y el nombre del archivo
        //        HttpContext.Current.Response.Clear();
        //        HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=archivo.xlsx");
        //        HttpContext.Current.Response.BinaryWrite(fileBytes);
        //        HttpContext.Current.Response.Flush();
        //        HttpContext.Current.Response.End();
        //    }
        //    else
        //    {
        //        // Puedes manejar el caso donde el archivo no existe
        //        throw new Exception("El archivo no existe.");
        //    }
        //}
    }


    public class archivodto
    {
        public string url { get; set; }
        public string nombre { get; set; }
        public string idtabla { get; set; }
    }

    public class dtoPunto
    {
        public int idpilar { get; set; }
        public int idarea { get; set; }
        public int idsubtema { get; set; }
        public int idpunto { get; set; }
        public string nombrepunto { get; set; }
        public double valor { get; set; }
        public bool habilitana { get; set; }
    }

    public class dtoCondiciones
    {
        public int idpunto { get; set; }
        public int idcondicion { get; set; }
        public string nombre { get; set; }

    }

    public class dtonaDetalle
    {
        public int id { get; set; }
        public int idauditoria { get; set; }
        public int punto { get; set; }
        public bool resultado { get; set; }
        public bool n_a { get; set; }
        public string comentarios { get; set; }
    }

    public class dtoresumen
    {
        public string punto { get; set; }
        public string comentario { get; set; }
    }

    public class dtocelda
    {
        public string punto { get; set; }
        public string na { get; set; }
        public string check { get; set; }
        public string comentarios { get; set; }
    }

    public class dtotabla
    {
        public string r1 { get; set; }
        public string r2 { get; set; }
        public string r3 { get; set; }
        public string r4 { get; set; }
        public string r5 { get; set; }
        public string r6 { get; set; }
        public string r7 { get; set; }
        public string r8 { get; set; }
        public string r9 { get; set; }
        public string r10 { get; set; }

        public string ubruta { get; set; }
        public string uoperativa { get; set; }
        public string acirc { get; set; }
        public string acinv { get; set; }
        public string inv { get; set; }
        public string cxc { get; set; }
        public string cxp { get; set; }
        public string ptot { get; set; }
        public string goper { get; set; }
        public string uoper { get; set; }

        public string vneta { get; set; }
        public string vneta2 { get; set; }
        public string pcirc { get; set; }
        public string pcirc2 { get; set; }
        public string cvdias { get; set; }
        public string vcredper { get; set; }
        public string cvper { get; set; }
        public string atot { get; set; }
        public string vnet { get; set; }
        public string atotpas { get; set; }

        public string url { get; set; }
        public string nombre { get; set; }
        public string distribuidor { get; set; }
    }
}