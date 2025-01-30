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
    public partial class NuevoNivel : System.Web.UI.Page
    {
        //public static string uname { get; set; }
        public NuevaAuditoria _uAuditoria { get; set; }
        public string _idauditoria { get; set; }
        public double cPlaneacion { get; set; }
        public double cEjecucion { get; set; }
        public double cAdministracion { get; set; }
        public double cInfraestructura { get; set; }
        public double cProductos { get; set; }
        //
        public int ptsTotales { get; set; }
        public int adminNoCumplido { get; set; }
        public int ejecNoCumplido { get; set; }
        public int infraNoCumplido { get; set; }
        public int planNoCumplido { get; set; }
        public int pysNoCumplido { get; set; }
        public int ptsAdmin { get; set; }
        public int ptsEjec { get; set; }
        public int ptsInfra { get; set; }
        public int ptsPlan { get; set; }
        public int ptsPYS { get; set; }
        public double soPerc { get; set; }
        public double obsPerc { get; set; }
        public List<NuevaAuditoriaDetalle> ptsCumplidos { get; set; }
        public List<NuevaAuditoriaDetalle> ptsNoCumplidos { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            string uname = Request.QueryString["u"] as string; //ID de la auditoria por URL

            Session["uname"] = uname;
            NuevaAuditoria uAuditoria;
            string idauditoria = "";

            Session["idauditoria"] = idauditoria;

            //drpDist.Items.Clear();

            if (!string.IsNullOrEmpty(uname))
            {
                uAuditoria = new XPQuery<NuevaAuditoria>(session).First(x => x.Id.ToString() == uname);
                _uAuditoria = uAuditoria;
                //Session["uAuditoria"] = uAuditoria;
               
                idauditoria = uAuditoria.Id.ToString();
                _idauditoria = idauditoria;
                Session["idauditoria"] = idauditoria;

                cPlaneacion = uAuditoria.calificacionPlan;
                cEjecucion = uAuditoria.calificacionEjec;
                cAdministracion = uAuditoria.calificacionAdmin;
                cInfraestructura = uAuditoria.calificacionInfra;
                cProductos = uAuditoria.calificacionPyS;
                ptsTotales = new XPQuery<v_Punto>(session).Where(p => p.Subtema != null).Count();

                ptsCumplidos = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && (x.Resultado == true || x.n_a == true)).ToList();
                ptsNoCumplidos = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria == uAuditoria && x.Resultado == false && x.n_a == false).ToList();

                var porcentaje1 = ((double)(ptsCumplidos.Count() * 1) / ptsTotales) * 100;
                soPerc = porcentaje1;

                var porcentaje2 = ((double)(ptsNoCumplidos.Count() * 1) / ptsTotales) * 100;
                obsPerc = porcentaje2;

                adminNoCumplido = new XPQuery<NuevaAuditoriaDetalle>(session).Where(a => a.Auditoria.Id == uAuditoria.Id && a.Resultado == false && a.n_a == false && a.Punto.Subtema.Area.Pilar.Nombre == "Administración").Count();
                ejecNoCumplido = new XPQuery<NuevaAuditoriaDetalle>(session).Where(a => a.Auditoria.Id == uAuditoria.Id && a.Resultado == false && a.n_a == false && a.Punto.Subtema.Area.Pilar.Nombre == "Ejecución").Count();
                infraNoCumplido = new XPQuery<NuevaAuditoriaDetalle>(session).Where(a => a.Auditoria.Id == uAuditoria.Id && a.Resultado == false && a.n_a == false && a.Punto.Subtema.Area.Pilar.Nombre == "Infraestructura").Count();
                planNoCumplido = new XPQuery<NuevaAuditoriaDetalle>(session).Where(a => a.Auditoria.Id == uAuditoria.Id && a.Resultado == false && a.n_a == false && a.Punto.Subtema.Area.Pilar.Nombre == "Planeación").Count();
                pysNoCumplido = new XPQuery<NuevaAuditoriaDetalle>(session).Where(a => a.Auditoria.Id == uAuditoria.Id && a.Resultado == false && a.n_a == false && a.Punto.Subtema.Area.Pilar.Nombre == "Productos y servicios").Count();

                ptsAdmin = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && (x.Resultado == true || x.n_a == true) && x.Punto.Subtema != null && x.Punto.Subtema.Area.Pilar.Nombre == "Administración").Count();
                ptsEjec = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && (x.Resultado == true || x.n_a == true) && x.Punto.Subtema != null && x.Punto.Subtema.Area.Pilar.Nombre == "Ejecución").Count();
                ptsInfra = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && (x.Resultado == true || x.n_a == true) && x.Punto.Subtema != null && x.Punto.Subtema.Area.Pilar.Nombre == "Infraestructura").Count();
                ptsPlan = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && (x.Resultado == true || x.n_a == true) && x.Punto.Subtema != null && x.Punto.Subtema.Area.Pilar.Nombre == "Planeación").Count();
                ptsPYS = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && (x.Resultado == true || x.n_a == true) && x.Punto.Subtema != null && x.Punto.Subtema.Area.Pilar.Nombre == "Productos y servicios").Count();

                var distribuidor = uAuditoria.Distribuidor;

                drpDist.Items.Add(distribuidor.nombredist);
                drpDist.Enabled = false;
            }
            else
            {
                // string s = (string)HttpContext.Current.Session["username"];
                var s = Util.getusuario();

                //string p = (string)HttpContext.Current.Session["permiso"];
                var p = Util.getPermiso();

                if (p.ToString() == "Distribuidor")
                {
                    var LstDistribuidores = new XPQuery<mdl_distribuidor>(session).Where(x => x.nombredist == s).ToList();

                    LstDistribuidores = LstDistribuidores.OrderBy(b => b.nombredist).ToList();

                    foreach (var item in LstDistribuidores)
                    {
                        drpDist.Items.Add(item.nombredist);

                        var _au = item.NuevaAuditoria.Where(x => x.status == "COMPLETADO").ToList(); //SOLO AUDITORIAS COMPLETADAS

                        if (_au.Count() > 0)
                        {
                            var au = _au.Max(x => x.Id);

                            uAuditoria = new XPQuery<NuevaAuditoria>(session).FirstOrDefault(x => x.Id == au);
                            _uAuditoria = uAuditoria;

                            idauditoria = uAuditoria.Id.ToString();
                            Session["idauditoria"] = idauditoria;

                            cPlaneacion = uAuditoria.calificacionPlan;
                            cEjecucion = uAuditoria.calificacionEjec;
                            cAdministracion = uAuditoria.calificacionAdmin;
                            cInfraestructura = uAuditoria.calificacionInfra;
                            cProductos = uAuditoria.calificacionPyS;
                            ptsTotales = new XPQuery<v_Punto>(session).Where(a => a.Subtema != null).Count();

                            ptsCumplidos = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && (x.Resultado == true || x.n_a == true)).ToList();
                            ptsNoCumplidos = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && x.Resultado == false && x.n_a == false).ToList();

                            var porcentaje1 = ((double)(ptsCumplidos.Count() * 1) / ptsTotales) * 100;
                            soPerc = porcentaje1;

                            var porcentaje2 = ((double)(ptsNoCumplidos.Count() * 1) / ptsTotales) * 100;
                            obsPerc = porcentaje2;

                            adminNoCumplido = new XPQuery<NuevaAuditoriaDetalle>(session).Where(a => a.Auditoria.Id == uAuditoria.Id && a.Resultado == false && a.n_a == false && a.Punto.Subtema.Area.Pilar.Nombre == "Administración").Count();
                            ejecNoCumplido = new XPQuery<NuevaAuditoriaDetalle>(session).Where(a => a.Auditoria.Id == uAuditoria.Id && a.Resultado == false && a.n_a == false && a.Punto.Subtema.Area.Pilar.Nombre == "Ejecución").Count();
                            infraNoCumplido = new XPQuery<NuevaAuditoriaDetalle>(session).Where(a => a.Auditoria.Id == uAuditoria.Id && a.Resultado == false && a.n_a == false && a.Punto.Subtema.Area.Pilar.Nombre == "Infraestructura").Count();
                            planNoCumplido = new XPQuery<NuevaAuditoriaDetalle>(session).Where(a => a.Auditoria.Id == uAuditoria.Id && a.Resultado == false && a.n_a == false && a.Punto.Subtema.Area.Pilar.Nombre == "Planeación").Count();
                            pysNoCumplido = new XPQuery<NuevaAuditoriaDetalle>(session).Where(a => a.Auditoria.Id == uAuditoria.Id && a.Resultado == false && a.n_a == false && a.Punto.Subtema.Area.Pilar.Nombre == "Productos y servicios").Count();

                            ptsAdmin = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && (x.Resultado == true || x.n_a == true) && x.Punto.Subtema != null && x.Punto.Subtema.Area.Pilar.Nombre == "Administración").Count();
                            ptsEjec = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && (x.Resultado == true || x.n_a == true) && x.Punto.Subtema != null && x.Punto.Subtema.Area.Pilar.Nombre == "Ejecución").Count();
                            ptsInfra = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && (x.Resultado == true || x.n_a == true) && x.Punto.Subtema != null && x.Punto.Subtema.Area.Pilar.Nombre == "Infraestructura").Count();
                            ptsPlan = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && (x.Resultado == true || x.n_a == true) && x.Punto.Subtema != null && x.Punto.Subtema.Area.Pilar.Nombre == "Planeación").Count();
                            ptsPYS = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && (x.Resultado == true || x.n_a == true) && x.Punto.Subtema != null && x.Punto.Subtema.Area.Pilar.Nombre == "Productos y servicios").Count();
                        }

                    }



                }
                else if (p.ToString() == "GerenteCuenta")
                {
                    var gerente = new XPQuery<Usuario>(session).FirstOrDefault(x => x.TipoUsuario.ToString() == p.ToString() && x.UserName == s);
                    var dists = gerente.Dependientes.ToList();
                    string username = "";

                    for (int i = 0; i < dists.Count; i++)
                    {
                        if (i == 0)
                        {
                            username = dists[i].UserName;
                        }

                        drpDist.Items.Add(dists[i].UserName);
                    }

                    var distribuidor = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist == username);
                    var _au = distribuidor.NuevaAuditoria.Where(x => x.status == "COMPLETADO").ToList();

                    var au = _au.Max(x => x.Id);

                    uAuditoria = new XPQuery<NuevaAuditoria>(session).FirstOrDefault(x => x.Id == au);

                    _uAuditoria = uAuditoria;

                    idauditoria = uAuditoria.Id.ToString();
                    Session["idauditoria"] = idauditoria;

                    cPlaneacion = uAuditoria.calificacionPlan;
                    cEjecucion = uAuditoria.calificacionEjec;
                    cAdministracion = uAuditoria.calificacionAdmin;
                    cInfraestructura = uAuditoria.calificacionInfra;
                    cProductos = uAuditoria.calificacionPyS;
                    ptsTotales = new XPQuery<v_Punto>(session).Where(a => a.Subtema != null).Count();

                    ptsCumplidos = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && (x.Resultado == true || x.n_a == true)).ToList();
                    ptsNoCumplidos = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && x.Resultado == false && x.n_a == false).ToList();

                    var porcentaje1 = ((double)(ptsCumplidos.Count() * 1) / ptsTotales) * 100;
                    soPerc = porcentaje1;

                    var porcentaje2 = ((double)(ptsNoCumplidos.Count() * 1) / ptsTotales) * 100;
                    obsPerc = porcentaje2;

                    adminNoCumplido = new XPQuery<NuevaAuditoriaDetalle>(session).Where(a => a.Auditoria.Id == uAuditoria.Id && a.Resultado == false && a.n_a == false && a.Punto.Subtema.Area.Pilar.Nombre == "Administración").Count();
                    ejecNoCumplido = new XPQuery<NuevaAuditoriaDetalle>(session).Where(a => a.Auditoria.Id == uAuditoria.Id && a.Resultado == false && a.n_a == false && a.Punto.Subtema.Area.Pilar.Nombre == "Ejecución").Count();
                    infraNoCumplido = new XPQuery<NuevaAuditoriaDetalle>(session).Where(a => a.Auditoria.Id == uAuditoria.Id && a.Resultado == false && a.n_a == false && a.Punto.Subtema.Area.Pilar.Nombre == "Infraestructura").Count();
                    planNoCumplido = new XPQuery<NuevaAuditoriaDetalle>(session).Where(a => a.Auditoria.Id == uAuditoria.Id && a.Resultado == false && a.n_a == false && a.Punto.Subtema.Area.Pilar.Nombre == "Planeación").Count();
                    pysNoCumplido = new XPQuery<NuevaAuditoriaDetalle>(session).Where(a => a.Auditoria.Id == uAuditoria.Id && a.Resultado == false && a.n_a == false && a.Punto.Subtema.Area.Pilar.Nombre == "Productos y servicios").Count();

                    ptsAdmin = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && (x.Resultado == true || x.n_a == true) && x.Punto.Subtema != null && x.Punto.Subtema.Area.Pilar.Nombre == "Administración").Count();
                    ptsEjec = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && (x.Resultado == true || x.n_a == true) && x.Punto.Subtema != null && x.Punto.Subtema.Area.Pilar.Nombre == "Ejecución").Count();
                    ptsInfra = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && (x.Resultado == true || x.n_a == true) && x.Punto.Subtema != null && x.Punto.Subtema.Area.Pilar.Nombre == "Infraestructura").Count();
                    ptsPlan = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && (x.Resultado == true || x.n_a == true) && x.Punto.Subtema != null && x.Punto.Subtema.Area.Pilar.Nombre == "Planeación").Count();
                    ptsPYS = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && (x.Resultado == true || x.n_a == true) && x.Punto.Subtema != null && x.Punto.Subtema.Area.Pilar.Nombre == "Productos y servicios").Count();
                }
                else
                {
                    var LstDistribuidores = new XPQuery<mdl_distribuidor>(session).ToList();

                    LstDistribuidores = LstDistribuidores.OrderBy(b => b.nombredist).ToList();

                    var idprimerdist = 0;

                    if(drpDist.Items.Count <= 0) //SI EL SELECT DE DISTRIBUIDORES ESTA VACIO
                    {
                        foreach (var item in LstDistribuidores)
                        {
                            if (item.nombredist == "100 VOLTIOS")
                            {
                                idprimerdist = item.iddistribuidor;
                            }
                            drpDist.Items.Add(item.nombredist);

                        }
                    }else
                    {
                        idprimerdist = LstDistribuidores[0].iddistribuidor; //SINO AGARRA EL ID DEL PRIMER DISTRIBUIDOR
                    }
                    

                    var distribuidor = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.iddistribuidor == idprimerdist);

                    var _au = distribuidor.NuevaAuditoria.Where(x => x.status == "COMPLETADO").ToList();

                    var au = _au.Max(x => x.Id);

                    uAuditoria = new XPQuery<NuevaAuditoria>(session).FirstOrDefault(x => x.Id == au);
                    _uAuditoria = uAuditoria;

                    idauditoria = uAuditoria.Id.ToString();
                    Session["idauditoria"] = idauditoria;

                    cPlaneacion = uAuditoria.calificacionPlan;
                    cEjecucion = uAuditoria.calificacionEjec;
                    cAdministracion = uAuditoria.calificacionAdmin;
                    cInfraestructura = uAuditoria.calificacionInfra;
                    cProductos = uAuditoria.calificacionPyS;
                    ptsTotales = new XPQuery<v_Punto>(session).Where(a => a.Subtema != null).Count();

                    ptsCumplidos = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && (x.Resultado == true || x.n_a == true)).ToList();
                    ptsNoCumplidos = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && x.Resultado == false && x.n_a == false).ToList();

                    var porcentaje1 = ((double)(ptsCumplidos.Count() * 1) / ptsTotales) * 100;
                    soPerc = porcentaje1;

                    var porcentaje2 = ((double)(ptsNoCumplidos.Count() * 1) / ptsTotales) * 100;
                    obsPerc = porcentaje2;

                    adminNoCumplido = new XPQuery<NuevaAuditoriaDetalle>(session).Where(a => a.Auditoria.Id == uAuditoria.Id && a.Resultado == false && a.n_a == false && a.Punto.Subtema.Area.Pilar.Nombre == "Administración").Count();
                    ejecNoCumplido = new XPQuery<NuevaAuditoriaDetalle>(session).Where(a => a.Auditoria.Id == uAuditoria.Id && a.Resultado == false && a.n_a == false && a.Punto.Subtema.Area.Pilar.Nombre == "Ejecución").Count();
                    infraNoCumplido = new XPQuery<NuevaAuditoriaDetalle>(session).Where(a => a.Auditoria.Id == uAuditoria.Id && a.Resultado == false && a.n_a == false && a.Punto.Subtema.Area.Pilar.Nombre == "Infraestructura").Count();
                    planNoCumplido = new XPQuery<NuevaAuditoriaDetalle>(session).Where(a => a.Auditoria.Id == uAuditoria.Id && a.Resultado == false && a.n_a == false && a.Punto.Subtema.Area.Pilar.Nombre == "Planeación").Count();
                    pysNoCumplido = new XPQuery<NuevaAuditoriaDetalle>(session).Where(a => a.Auditoria.Id == uAuditoria.Id && a.Resultado == false && a.n_a == false && a.Punto.Subtema.Area.Pilar.Nombre == "Productos y servicios").Count();

                    ptsAdmin = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && (x.Resultado == true || x.n_a == true) && x.Punto.Subtema != null && x.Punto.Subtema.Area.Pilar.Nombre == "Administración").Count();
                    ptsEjec = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && (x.Resultado == true || x.n_a == true) && x.Punto.Subtema != null && x.Punto.Subtema.Area.Pilar.Nombre == "Ejecución").Count();
                    ptsInfra = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && (x.Resultado == true || x.n_a == true) && x.Punto.Subtema != null && x.Punto.Subtema.Area.Pilar.Nombre == "Infraestructura").Count();
                    ptsPlan = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && (x.Resultado == true || x.n_a == true) && x.Punto.Subtema != null && x.Punto.Subtema.Area.Pilar.Nombre == "Planeación").Count();
                    ptsPYS = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && (x.Resultado == true || x.n_a == true) && x.Punto.Subtema != null && x.Punto.Subtema.Area.Pilar.Nombre == "Productos y servicios").Count();

                }
            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();

            string selectedText = drpDist.SelectedItem.Text;
            var distribuidor = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist == selectedText);

            Session["selectedtext"] = selectedText;

            NuevaAuditoria uAuditoria;
            string idauditoria = "";

            try
            {
                var au = distribuidor.NuevaAuditoria.Where(x => x.status == "COMPLETADO").Max(x => x.Id);

                uAuditoria = new XPQuery<NuevaAuditoria>(session).FirstOrDefault(x => x.Id == au);
                _uAuditoria = uAuditoria;
                
                idauditoria = uAuditoria.Id.ToString();
                Session["idauditoria"] = idauditoria;

                cPlaneacion = uAuditoria.calificacionPlan;
                cEjecucion = uAuditoria.calificacionEjec;
                cAdministracion = uAuditoria.calificacionAdmin;
                cInfraestructura = uAuditoria.calificacionInfra;
                cProductos = uAuditoria.calificacionPyS;
                ptsTotales = new XPQuery<v_Punto>(session).Where(a => a.Subtema != null).Count();

                ptsCumplidos = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && (x.Resultado == true || x.n_a == true)).ToList();
                ptsNoCumplidos = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && x.Resultado == false && x.n_a == false).ToList();

                var porcentaje1 = ((double)(ptsCumplidos.Count() * 1) / ptsTotales) * 100;
                soPerc = porcentaje1;

                var porcentaje2 = ((double)(ptsNoCumplidos.Count() * 1) / ptsTotales) * 100;
                obsPerc = porcentaje2;

                adminNoCumplido = new XPQuery<NuevaAuditoriaDetalle>(session).Where(a => a.Auditoria.Id == uAuditoria.Id && a.Resultado == false && a.n_a == false && a.Punto.Subtema.Area.Pilar.Nombre == "Administración").Count();
                ejecNoCumplido = new XPQuery<NuevaAuditoriaDetalle>(session).Where(a => a.Auditoria.Id == uAuditoria.Id && a.Resultado == false && a.n_a == false && a.Punto.Subtema.Area.Pilar.Nombre == "Ejecución").Count();
                infraNoCumplido = new XPQuery<NuevaAuditoriaDetalle>(session).Where(a => a.Auditoria.Id == uAuditoria.Id && a.Resultado == false && a.n_a == false && a.Punto.Subtema.Area.Pilar.Nombre == "Infraestructura").Count();
                planNoCumplido = new XPQuery<NuevaAuditoriaDetalle>(session).Where(a => a.Auditoria.Id == uAuditoria.Id && a.Resultado == false && a.n_a == false && a.Punto.Subtema.Area.Pilar.Nombre == "Planeación").Count();
                pysNoCumplido = new XPQuery<NuevaAuditoriaDetalle>(session).Where(a => a.Auditoria.Id == uAuditoria.Id && a.Resultado == false && a.n_a == false && a.Punto.Subtema.Area.Pilar.Nombre == "Productos y servicios").Count();

                ptsAdmin = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && (x.Resultado == true || x.n_a == true) && x.Punto.Subtema != null && x.Punto.Subtema.Area.Pilar.Nombre == "Administración").Count();
                ptsEjec = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && (x.Resultado == true || x.n_a == true) && x.Punto.Subtema != null && x.Punto.Subtema.Area.Pilar.Nombre == "Ejecución").Count();
                ptsInfra = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && (x.Resultado == true || x.n_a == true) && x.Punto.Subtema != null && x.Punto.Subtema.Area.Pilar.Nombre == "Infraestructura").Count();
                ptsPlan = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && (x.Resultado == true || x.n_a == true) && x.Punto.Subtema != null && x.Punto.Subtema.Area.Pilar.Nombre == "Planeación").Count();
                ptsPYS = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id == uAuditoria.Id && (x.Resultado == true || x.n_a == true) && x.Punto.Subtema != null && x.Punto.Subtema.Area.Pilar.Nombre == "Productos y servicios").Count();
            }
            catch (Exception)
            {

            }
        }

        [WebMethod]
        public static string traeActividades()
        {
            DevExpress.Xpo.Session session = Util.getsession();
            
            var uname = (string)HttpContext.Current.Session["uname"];

            NuevaAuditoria u = new XPQuery<NuevaAuditoria>(session).First(x => x.Id.ToString() == uname);
            List<dtoNivel> lista = new List<dtoNivel>();

            var auditoria = u.Id;
            var naac = new XPQuery<NuevaAuditoriaActividad>(session).Where(b => b.Idaud == u).ToList();
            var basico = naac.Where(x => x.Idactividad.NivelActividad == v_Actividad.Nivel.BÁSICO).ToList();
            var intermedio = naac.Where(x => x.Idactividad.NivelActividad == v_Actividad.Nivel.INTERMEDIO).ToList();
            var avanzado = naac.Where(a => a.Idactividad.NivelActividad == v_Actividad.Nivel.AVANZADO).ToList();
            var sobresaliente = naac.Where(s => s.Idactividad.NivelActividad == v_Actividad.Nivel.SOBRESALIENTE).ToList();


            var actividades = new XPQuery<v_Actividad>(session).ToList();
            var tBasico = actividades.Where(x => x.NivelActividad == v_Actividad.Nivel.BÁSICO).ToList();
            var tIntermedio = actividades.Where(x => x.NivelActividad == v_Actividad.Nivel.INTERMEDIO).ToList();
            var tAvanzado = actividades.Where(x => x.NivelActividad == v_Actividad.Nivel.AVANZADO).ToList();
            var tSobre = actividades.Where(x => x.NivelActividad == v_Actividad.Nivel.SOBRESALIENTE).ToList();

            string[] level = { "BASICO", "INTERMEDIO", "AVANZADO", "SOBRESALIENTE" };
            int[] cantLevel = { basico.Count(), intermedio.Count(), avanzado.Count(), sobresaliente.Count() };
            int[] total = { tBasico.Count(), tIntermedio.Count(), tAvanzado.Count(), tSobre.Count() };

            for (int i = 0; i < level.Count(); i++)
            {
                dtoNivel oNivel = new dtoNivel();
                oNivel.Nivel = level[i];
                oNivel.TotalActividades = total[i];
                oNivel.RealizadasAuditoria = total[i] - cantLevel[i];
                oNivel.RealizadasDC = 0;

                double cumpli = ((double)oNivel.RealizadasAuditoria + (double)oNivel.RealizadasDC) * 1 / (double)total[i];
                cumpli = cumpli > 0 ? cumpli * 100 : 0;
                oNivel.Cumplimiento = Math.Round(cumpli, 2).ToString() + "%";

                lista.Add(oNivel);
            }

            return JsonConvert.SerializeObject(lista);
        }

        [WebMethod]
        public static string traePtsXPilar()
        {
            DevExpress.Xpo.Session session = Util.getsession();

            List<ptoxnivel> lista = new List<ptoxnivel>();

            var basico = new XPQuery<v_Punto>(session).Where(x => x.NivelPunto == v_Punto.Nivel.BASICO).ToList();
            var intermedio = new XPQuery<v_Punto>(session).Where(x => x.NivelPunto == v_Punto.Nivel.INTERMEDIO).ToList();
            var avanzado = new XPQuery<v_Punto>(session).Where(x => x.NivelPunto == v_Punto.Nivel.AVANZADO).ToList();
            var sobre = new XPQuery<v_Punto>(session).Where(x => x.NivelPunto == v_Punto.Nivel.SOBRESALIENTE).ToList();

            var basicoAdmn = basico.Where(x => x.Subtema != null && x.Subtema.Area.Pilar.Nombre == "Administración").Count();
            var basicoEjec = basico.Where(x => x.Subtema != null && x.Subtema.Area.Pilar.Nombre == "Ejecución").Count();
            var basicoInfra = basico.Where(x => x.Subtema != null && x.Subtema.Area.Pilar.Nombre == "Infraestructura").Count();
            var basicoPlan = basico.Where(x => x.Subtema != null && x.Subtema.Area.Pilar.Nombre == "Planeación").Count();
            var basicoPYS = basico.Where(x => x.Subtema != null && x.Subtema.Area.Pilar.Nombre == "Productos y servicios").Count();

            var interAdmn = intermedio.Where(a => a.Subtema != null && a.Subtema.Area.Pilar.Nombre == "Administración").Count();
            var interEjec = intermedio.Where(a => a.Subtema != null && a.Subtema.Area.Pilar.Nombre == "Ejecución").Count();
            var interInfra = intermedio.Where(a => a.Subtema != null && a.Subtema.Area.Pilar.Nombre == "Infraestructura").Count();
            var interPlan = intermedio.Where(a => a.Subtema != null && a.Subtema.Area.Pilar.Nombre == "Planeación").Count();
            var interPYS = intermedio.Where(a => a.Subtema != null && a.Subtema.Area.Pilar.Nombre == "Productos y servicios").Count();

            var avanAdmn = avanzado.Where(b => b.Subtema != null && b.Subtema.Area.Pilar.Nombre == "Administración").Count();
            var avanEjec = avanzado.Where(b => b.Subtema != null && b.Subtema.Area.Pilar.Nombre == "Ejecución").Count();
            var avanInfra = avanzado.Where(b => b.Subtema != null && b.Subtema.Area.Pilar.Nombre == "Infraestructura").Count();
            var avanPlan = avanzado.Where(b => b.Subtema != null && b.Subtema.Area.Pilar.Nombre == "Planeación").Count();
            var avanPYS = avanzado.Where(b => b.Subtema != null && b.Subtema.Area.Pilar.Nombre == "Productos y servicios").Count();

            var sobreAdmn = sobre.Where(s => s.Subtema != null && s.Subtema.Area.Pilar.Nombre == "Administración").Count();
            var sobreEjec = sobre.Where(s => s.Subtema != null && s.Subtema.Area.Pilar.Nombre == "Ejecución").Count();
            var sobreInfra = sobre.Where(s => s.Subtema != null && s.Subtema.Area.Pilar.Nombre == "Infraestructura").Count();
            var sobrePlan = sobre.Where(s => s.Subtema != null && s.Subtema.Area.Pilar.Nombre == "Planeación").Count();
            var sobrePYS = sobre.Where(s => s.Subtema != null && s.Subtema.Area.Pilar.Nombre == "Productos y servicios").Count();

            string[] level = { "BASICO", "INTERMEDIO", "AVANZADO", "SOBRESALIENTE" };
            int[] admin = { basicoAdmn, interAdmn, avanAdmn, sobreAdmn };
            int[] ejec = { basicoEjec, interEjec, avanEjec, sobreEjec };
            int[] infra = { basicoInfra, interInfra, avanInfra, sobreInfra };
            int[] plan = { basicoPlan, interPlan, avanPlan, sobrePlan };
            int[] pys = { basicoPYS, interPYS, avanPYS, sobrePYS };

            for (int i = 0; i < level.Count(); i++)
            {
                ptoxnivel temp = new ptoxnivel();
                temp.Nivel = level[i];
                temp.Administracion = admin[i].ToString();
                temp.Ejecucion = ejec[i].ToString();
                temp.Infraestructura = infra[i].ToString();
                temp.Planeacion = plan[i].ToString();
                temp.ProductosYServicios = pys[i].ToString();
                temp.Total = (admin[i] + ejec[i] + infra[i] + plan[i] + pys[i]).ToString();

                lista.Add(temp);
            }

            return JsonConvert.SerializeObject(lista);
        }

        [WebMethod]
        public static string traePtsXNivel(string dist)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            var distribuidor = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist == dist);
            try
            {
                var au = distribuidor.NuevaAuditoria.Where(x => x.status == "COMPLETADO").Max(x => x.Id);

                NuevaAuditoria uAuditoria;
                uAuditoria = new XPQuery<NuevaAuditoria>(session).FirstOrDefault(x => x.Id == au);
                

                var basicosSi = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria == uAuditoria && x.Punto.NivelPunto == v_Punto.Nivel.BASICO && (x.Resultado == true || x.n_a == true)).Count();
                var basicosNo = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria == uAuditoria && x.Punto.NivelPunto == v_Punto.Nivel.BASICO && x.Resultado == false && x.n_a == false).Count();

                var interSi = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria == uAuditoria && x.Punto.NivelPunto == v_Punto.Nivel.INTERMEDIO && (x.Resultado == true || x.n_a == true)).Count();
                var interNo = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria == uAuditoria && x.Punto.NivelPunto == v_Punto.Nivel.INTERMEDIO && x.Resultado == false && x.n_a == false).Count();

                var avaSi = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria == uAuditoria && x.Punto.NivelPunto == v_Punto.Nivel.AVANZADO && (x.Resultado == true || x.n_a == true)).Count();
                var avaNo = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria == uAuditoria && x.Punto.NivelPunto == v_Punto.Nivel.AVANZADO && x.Resultado == false && x.n_a == false).Count();

                var sobSi = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria == uAuditoria && x.Punto.NivelPunto == v_Punto.Nivel.SOBRESALIENTE && (x.Resultado == true || x.n_a == true)).Count();
                var sobNo = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria == uAuditoria && x.Punto.NivelPunto == v_Punto.Nivel.SOBRESALIENTE && x.Resultado == false && x.n_a == false).Count();

                dtoptxnivel temp = new dtoptxnivel();
                temp.basicoSi = basicosSi;
                temp.basicoNo = basicosNo;
                temp.interSi = interSi;
                temp.interNo = interNo;
                temp.avaSi = avaSi;
                temp.avaNo = avaNo;
                temp.sobSi = sobSi;
                temp.sobNo = sobNo;

                return JsonConvert.SerializeObject(temp);
            }
            catch (Exception)
            {
                return "-1";
            }
        }

        [WebMethod]
        public static string traePuntos()
        {
            DevExpress.Xpo.Session session = Util.getsession();

            var uname = (string)HttpContext.Current.Session["uname"];


            if (uname != "" && uname != null)
            {
                var lstPuntos = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id.ToString() == uname && x.Comentarios != "" && x.Comentarios != null && x.Resultado == false).ToList();

                List<dtopts> listaPuntos = new List<dtopts>();
                foreach (var item in lstPuntos)
                {
                    dtopts temp = new dtopts();
                    temp.Pilar = item.Punto.Subtema.Area.Pilar.Nombre;
                    temp.Punto = item.Punto.NombrePunto;
                    temp.Comentarios = item.Comentarios;
                    listaPuntos.Add(temp);
                }

                return JsonConvert.SerializeObject(listaPuntos);
            }
            else
            {
                //string selectedText = (string)HttpContext.Current.Session["selectedtext"];
                //var distribuidor = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist == selectedText);

                //var au = distribuidor.NuevaAuditoria.Where(x => x.status == "COMPLETADO").Max(x => x.Id);

                //var uAuditoria = new XPQuery<NuevaAuditoria>(session).FirstOrDefault(x => x.Id == au);

                var idAuditoria = (string)HttpContext.Current.Session["idauditoria"];

                //var uAuditoria = new XPQuery<NuevaAuditoria>(session).First(x => x.Id.ToString() == uname);
                //var uAuditoria = (string)HttpContext.Current.Session["uAuditoria"];

                //var lstPts = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id.ToString() == uAuditoria.Id.ToString() && x.Comentarios != "" && x.Comentarios != null && x.Resultado == false).ToList();
                var lstPts = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id.ToString() == idAuditoria && x.Comentarios != "" && x.Comentarios != null && x.Resultado == false).ToList();

                List<dtopts> listaPts = new List<dtopts>();
                foreach (var item in lstPts)
                {
                    dtopts temp = new dtopts();
                    temp.Pilar = item.Punto.Subtema.Area.Pilar.Nombre;
                    temp.Punto = item.Punto.NombrePunto;
                    temp.Comentarios = item.Comentarios;
                    listaPts.Add(temp);
                }

                return JsonConvert.SerializeObject(listaPts);
            }

        }

        [WebMethod]
        public static string traePuntosCorrectos()
        {
            DevExpress.Xpo.Session session = Util.getsession();
            var uname = (string)HttpContext.Current.Session["uname"];
            //var uAuditoria = new XPQuery<NuevaAuditoria>(session).First(x => x.Id.ToString() == uname);

            if (uname != "" && uname != null)
            {
                var lstPuntos = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id.ToString() == uname && x.Comentarios != "" && x.Comentarios != null && x.Resultado == true).ToList();

                List<dtopts> listaPuntos = new List<dtopts>();
                foreach (var item in lstPuntos)
                {
                    dtopts temp = new dtopts();
                    temp.Pilar = item.Punto.Subtema.Area.Pilar.Nombre;
                    temp.Punto = item.Punto.NombrePunto;
                    temp.Comentarios = item.Comentarios;
                    listaPuntos.Add(temp);
                }

                return JsonConvert.SerializeObject(listaPuntos);
            }else
            {
                var idAuditoria = (string)HttpContext.Current.Session["idauditoria"];

//                var lstPts = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id.ToString() == uAuditoria.Id.ToString() && x.Comentarios != "" && x.Comentarios != null && x.Resultado == true).ToList();
                var lstPts = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Auditoria.Id.ToString() == idAuditoria && x.Comentarios != "" && x.Comentarios != null && x.Resultado == true).ToList();

                List<dtopts> listaPts = new List<dtopts>();
                foreach (var item in lstPts)
                {
                    dtopts temp = new dtopts();
                    temp.Pilar = item.Punto.Subtema.Area.Pilar.Nombre;
                    temp.Punto = item.Punto.NombrePunto;
                    temp.Comentarios = item.Comentarios;
                    listaPts.Add(temp);
                }

                return JsonConvert.SerializeObject(listaPts);
            }

            
        }

        public class dtoNivel
        {
            public string Nivel { get; set; }
            public int TotalActividades { get; set; }
            public int RealizadasAuditoria { get; set; }
            public int RealizadasDC { get; set; }
            public string Cumplimiento { get; set; }
        }

        public class ptoxnivel
        {
            public string Nivel { get; set; }
            public string Administracion { get; set; }
            public string Ejecucion { get; set; }
            public string Infraestructura { get; set; }
            public string Planeacion { get; set; }
            public string ProductosYServicios { get; set; }
            public string Total { get; set; }
        }

        public class dtopts
        {
            public string Pilar { get; set; }
            public string Punto { get; set; }
            public string Comentarios { get; set; }
        }

        public class dtoptxnivel
        {
            public int basicoSi { get; set; }
            public int basicoNo { get; set; }
            public int interSi { get; set; }
            public int interNo { get; set; }
            public int avaSi { get; set; }
            public int avaNo { get; set; }
            public int sobSi { get; set; }
            public int sobNo { get; set; }
        }
    }
}