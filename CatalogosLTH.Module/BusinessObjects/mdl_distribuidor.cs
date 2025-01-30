using System;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.DC;
using System.Linq;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("Distribuidor")]
    [XafDefaultProperty("nombredist")]
    public class mdl_distribuidor : XPLiteObject
    {

        [Action(Caption = "Actualizar Profesionalización")]
        public void ActualizarProfesionalización()
        {
            double idtotalv = 0;
            double avancea = 0;
            double totalp = 0;
            double totalc = 0;
            double totaldc = 0;
            double avanceTotalT = 0;
            double avanceTotalFinalDC = 0;//completadas por dc linea roja final
            double totalProfesionalizacion = 0;

            double activas = 0;
            double totalpt = 0;
            double totalct = 0;
            double activast = 0;
            double avanceat = 0;
            double comp = 0;

            int[] arrTotal = new int[5];
            int[] arrActivas = new int[5];
            string[] nomPilar = new string[5];
            arrActivas[0] = 0;
            arrActivas[1] = 0;
            arrActivas[2] = 0;
            arrActivas[3] = 0;
            arrActivas[4] = 0;

            arrTotal[0] = 0;
            arrTotal[1] = 0;
            arrTotal[2] = 0;
            arrTotal[3] = 0;
            arrTotal[4] = 0;
            int numPilar = 0;


            DevExpress.Xpo.Session session = this.Session;

            string idauditoria = UltimaAuditoria.idaud.ToString();
            mdl_auditoria auditoria = new XPQuery<mdl_auditoria>(session).FirstOrDefault(x => x.idaud.ToString() == idauditoria);

            XPQuery<mdl_pondnivel> ponderacionesNivel = session.Query<mdl_pondnivel>();
            XPQuery<mdl_nivel> niveles = session.Query<mdl_nivel>();
            XPQuery<mdl_catnivel> catNiveles = session.Query<mdl_catnivel>();
            XPQuery<vwtotal> totales = session.Query<vwtotal>();
            var puntostotales = new XPQuery<vwpuntostot>(session).Where(x => x.idaud.ToString() == idauditoria);
            var auditoriaActividad = new XPQuery<mdl_auditoriaactividad>(session).Where(x => x.Idaud.ToString() == idauditoria);
            int tipoaud = 0;
            string nombredist = "";
            int idaudi = Convert.ToInt32(idauditoria);

            tipoaud = auditoria.Idtipoaud.idTipoAuditoria;
            nombredist = auditoria.Iddistribuidor.nombredist;

            var sql3 = from ni in niveles
                       join ctn in catNiveles on ni equals ctn.Idnivel
                       where ctn.Idtipoaud.idTipoAuditoria == auditoria.Idtipoaud.idTipoAuditoria
                       select new { ni.idnivel, ni.nombreniv, ctn.ponderacion, ctn.Idtipoaud };

            foreach (var rec3 in sql3)
            {
                var sql2 = from vt in totales
                           where vt.idnivel == rec3.idnivel
                           && vt.idtipoaud == tipoaud
                           select vt;
                foreach (var record2 in sql2)//FOREACH PILARES
                {
                    var sqln = from vpt in puntostotales
                               where vpt.idaud == idaudi &&
                               vpt.idpilar == record2.idpilar &&
                               vpt.idnivel == record2.idnivel
                               select vpt;
                    idtotalv = sqln.ToList().Count();//completadas en sistema

                    double completadasAuditoria = 0;
                    if (record2.total == 0)
                    {

                    }
                    else
                    {
                        completadasAuditoria = (record2.total - idtotalv);
                    }

                    var sqlAudAct = from audact in auditoriaActividad
                                    where audact.Idaud.idaud.ToString() == idauditoria && audact.fechacomp != null
                                     && audact.Idactividad.NivelID.nombreniv == record2.nombreniv && audact.Idactividad.PilarID.nombrepil == record2.nombrepil
                                    select audact;

                    int actividadesRealizadasDC = sqlAudAct.Count();
                    totaldc += actividadesRealizadasDC;


                    //
                    if (nomPilar[numPilar] == null)
                    {
                        nomPilar[numPilar] = record2.nombrepil;
                    }
                    arrTotal[numPilar] = arrTotal[numPilar] + record2.total;
                    //


                    double actividadesTotalesPorRealizarDeLaAuditoria = idtotalv;

                    if (record2.total == 0)
                    {
                        comp = record2.pondercionPilar;

                    }
                    else
                    {
                        comp = (record2.total - idtotalv) / record2.total;
                        comp = comp * record2.pondercionPilar;
                        double actividadesActivas = actividadesTotalesPorRealizarDeLaAuditoria - actividadesRealizadasDC;
                        arrActivas[numPilar] = arrActivas[numPilar] + (int)actividadesActivas;

                    }
                    avancea = comp + avancea;

                    double avanceTotal = (record2.total == 0) ? record2.pondercionPilar : (completadasAuditoria + actividadesRealizadasDC) / record2.total * record2.pondercionPilar;
                    avanceTotalT += avanceTotal;
                    double profesionalizacionNivel = (rec3.ponderacion * avanceTotalT) / 100.0;
                    totalProfesionalizacion += profesionalizacionNivel;//TOTAL PROFESIONALIZACION LINEA ROJA


                    avanceTotalFinalDC += totaldc;
                    totalpt = totalp + totalpt;
                    totalct = totalct + totalc;
                    activast = activast + activas;
                    avanceat = ((rec3.ponderacion * avancea) / 100) + avanceat;
                    totalp = 0;
                    totalc = 0;
                    activas = 0;
                    avancea = 0;
                    totaldc = 0; //se restaura totaldc a 0
                    avanceTotalT = 0;

                    numPilar++;
                }


                numPilar = 0;
            }

            int total1 = arrTotal[0];
            int total2 = arrTotal[1];
            int total3 = arrTotal[2];
            int total4 = arrTotal[3];
            int total5 = arrTotal[4];

            int act1 = arrActivas[0];
            int act2 = arrActivas[1];
            int act3 = arrActivas[2];
            int act4 = arrActivas[3];
            int act5 = arrActivas[4];

            double[] arrPorcentaje = new double[5];
            double porcentaje1 = 100;
            double porcentaje2 = 100;
            double porcentaje3 = 100;
            double porcentaje4 = 100;
            double porcentaje5 = 100;

            if (total1 != 0) { porcentaje1 = 100 - (act1 * 100 / total1); }
            if (total2 != 0) { porcentaje2 = 100 - (act2 * 100 / total2); }
            if (total3 != 0) { porcentaje3 = 100 - (act3 * 100 / total3); }
            if (total4 != 0) { porcentaje4 = 100 - (act4 * 100 / total4); }
            if (total5 != 0) { porcentaje5 = 100 - (act5 * 100 / total5); }

            arrPorcentaje[0] = porcentaje1;
            arrPorcentaje[1] = porcentaje2;
            arrPorcentaje[2] = porcentaje3;
            arrPorcentaje[3] = porcentaje4;
            arrPorcentaje[4] = porcentaje5;


            for (int i = 0; i < arrPorcentaje.Length; i++)
            {
                if (nomPilar[i] == "Infraestructura")
                {
                    avanceInfra = arrPorcentaje[i];
                }
                else if (nomPilar[i] == "Administración")
                {
                    avanceAdministracion = arrPorcentaje[i];
                }
                else if (nomPilar[i] == "Planeacion")
                {
                    avancePlaneacion = arrPorcentaje[i];
                }
                else if (nomPilar[i] == "Ejecución")
                {
                    avanceEjecucion = arrPorcentaje[i];
                }
                else if (nomPilar[i] == "Productos y Servicios")
                {
                    avanceProd = arrPorcentaje[i];
                }
            }


            //CALCULA EL NIVEL DE PROFESIONALIZACION EN TEXTO

            string nivelP = "Básico";

            if (profesionalizacion >= 0 && profesionalizacion < 44.5)
            {
                nivelP = "Básico";
            }
            else if (profesionalizacion >= 44.5 && profesionalizacion <= 65.5)
            {
                nivelP = "Transitorio";
            }
            else if (profesionalizacion >= 65.5 && profesionalizacion < 80.5)
            {
                nivelP = "Intermedio";
            }
            else if (profesionalizacion >= 80.5 && profesionalizacion < 94.5)
            {
                nivelP = "Avanzado";
            }
            else if (profesionalizacion >= 94.5 && profesionalizacion <= 100)
            {
                nivelP = "Sobresaliente";
            }

            /*CALCULA AVANCE ACTIVIDADES+*/
            //  mdl_nivel nivelObj = new XPQuery<mdl_nivel>(session).FirstOrDefault(x => x.idnivel == NivelActual);

            //int nivel = nivelObj.idnivel;
            int idAuditoria = UltimaAuditoria.idaud;
            double total = 0;
            double contestadas = 0;
            

            var Acts = new XPQuery<mdl_auditoriaactividad>(session).Where(x => x.Idaud.ToString() == idAuditoria.ToString() && x.Idactividad.NivelID.idnivel.ToString() == NivelActual.ToString());
            double cantAct = Acts.Count();
            double ActsCompletas = (from ac in Acts
                                 where ac.fechacomp != null
                                 select ac).Count();

            double avance = ActsCompletas * 100 / cantAct;


            profesionalizacion = totalProfesionalizacion;
            cumplimientoNivel = avance;
            nivelAct = nivelP;

              /*            */
           // this.Save();
            //this.Session.CommitTransaction();
        }


        public mdl_distribuidor(Session session)
            : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }
        [Key(true)]
        public int iddistribuidor { get; set; }

        //public string nombredist { get; set; }
        private string Nombredist;        
        public string nombredist
        {
            get { return Nombredist; }
            set { SetPropertyValue("nombredist", ref Nombredist, (value != null) ? value.ToUpper() : "nohay"); }
            //set { SetPropertyValue("nombredist", ref Nombredist, value.ToUpper()); }
        }

        [Association("Distribuidor-Kardex")]
        public XPCollection<mdl_Kardex> Kardex
        {
            get
            {
                return GetCollection<mdl_Kardex>("Kardex");
            }
        }

        public string nombreusuario { get; set; }
        public string zona { get; }
        public string zonastr { get; set; }
        [XafDisplayName("% Avance pilar productos y servcios")]
        public double avanceProd { get; set; }
        [XafDisplayName("% Avance pilar infraestructura")]
        public double avanceInfra { get; set; }
        [XafDisplayName("% Avance pilar administración")]
        public double avanceAdministracion { get; set; }
        [XafDisplayName("% Avance pilar ejecución")]
        public double avanceEjecucion { get; set; }
        [XafDisplayName("% Avance pilar planeación")]
        public double avancePlaneacion { get; set;}

        [XafDisplayName("Nivel Profesionalización")]
        public string nivelAct { get; set; }

        public double cumplimientoNivel { get; set; }

        /*
        Avance Productos y servicios
Avance Infraestructura
Avance Administración
Avance Ejecución
Avance Planeación*/


        [ImmediatePostData]
        public double profesionalizacion { get; set; }
        /*public double profesionalizacion
        {
            get{

                double idtotalv = 0;
                bool existe = false;
                double avancea = 0;
                double totalp = 0;
                double totalc = 0;

                double totaldc = 0;
                double avanceTotalT = 0;
                double avanceTotalDC = 0;
                double avanceTotalFinalDC = 0;//completadas por dc linea roja final
                double totalProfesionalizacion = 0;

                double activas = 0;
                double totalpt = 0;
                double totalct = 0;
                double activast = 0;
                double avanceat = 0;
                double comp = 0;
                string tipaudletra = "";

                int[] arrTotal = new int[5];
                int[] arrActivas = new int[5];
                string[] nomPilar = new string[5];
                arrActivas[0] = 0;
                arrActivas[1] = 0;
                arrActivas[2] = 0;
                arrActivas[3] = 0;
                arrActivas[4] = 0;

                arrTotal[0] = 0;
                arrTotal[1] = 0;
                arrTotal[2] = 0;
                arrTotal[3] = 0;
                arrTotal[4] = 0;
                int numPilar = 0;

                double profesionalizacion = 0;
                DevExpress.Xpo.Session session = this.Session;
                //string idauditoria = getUltimaAuditoria(distribuidor).ToString();
                string idauditoria = UltimaAuditoria.idaud.ToString();
                mdl_auditoria auditoria = new XPQuery<mdl_auditoria>(session).FirstOrDefault(x => x.idaud.ToString() == idauditoria);

                XPQuery<mdl_auditoria> auditorias = session.Query<mdl_auditoria>();
                XPQuery<mdl_distribuidor> distribuidores = session.Query<mdl_distribuidor>();
                XPQuery<mdl_pondnivel> ponderacionesNivel = session.Query<mdl_pondnivel>();
                XPQuery<mdl_nivel> niveles = session.Query<mdl_nivel>();
                XPQuery<mdl_catnivel> catNiveles = session.Query<mdl_catnivel>();
                XPQuery<vwtotal> totales = session.Query<vwtotal>();
                XPQuery<vwpuntostot> puntostotales = session.Query<vwpuntostot>();
                XPQuery<mdl_auditoriaactividad> auditoriaActividad = session.Query<mdl_auditoriaactividad>();
                int tipoaud = 0;
                string nombredist = "";
                int idaudi = Convert.ToInt32(idauditoria);
                var sqlw = from a in auditorias
                           join d in distribuidores on a.Iddistribuidor equals d
                           where a.idaud == idaudi
                           select new { a.idaud, a.Idtipoaud.idTipoAuditoria, d.nombredist };
                foreach (var item in sqlw)
                {
                    tipoaud = item.idTipoAuditoria;
                    nombredist = item.nombredist;
                    //cerrada = true;
                }


                var sql = from pn in ponderacionesNivel
                          where pn.Idaud.idaud.ToString() == idauditoria
                          select pn;

                var sql3 = from ni in niveles
                           join ctn in catNiveles on ni equals ctn.Idnivel
                           where ctn.Idtipoaud.idTipoAuditoria == auditoria.Idtipoaud.idTipoAuditoria
                           select new { ni.idnivel, ni.nombreniv, ctn.ponderacion, ctn.Idtipoaud };

                foreach (var rec3 in sql3)
                {

                    var sql2 = from vt in totales
                               where vt.idnivel == rec3.idnivel
                               && vt.idtipoaud == tipoaud
                               select vt;
                    foreach (var record2 in sql2)//FOREACH PILARES
                    {

                        totalp += record2.total;

                        var sqln = from vpt in puntostotales
                                   where vpt.idaud == idaudi &&
                                   vpt.idpilar == record2.idpilar &&
                                   vpt.idnivel == record2.idnivel
                                   select vpt;
                        idtotalv = 0;
                        foreach (var item in sqln)
                        {
                            idtotalv++;//ACT A COMPLETAR EN DC
                        }

                        double completadasAuditoria = 0;
                        if (record2.total == 0)
                        {

                        }
                        else
                        {
                            completadasAuditoria = (record2.total - idtotalv);
                            totalc = record2.total - idtotalv + totalc;
                        }

                        var sqlAudAct = from audact in auditoriaActividad
                                        where audact.Idaud.idaud.ToString() == idauditoria && audact.fechacomp != null && audact.Idactividad.NivelID.nombreniv == record2.nombreniv && audact.Idactividad.PilarID.nombrepil == record2.nombrepil
                                        select audact;

                        int actividadesRealizadasDC = sqlAudAct.Count();
                        totaldc += actividadesRealizadasDC;

                        if (nomPilar[numPilar] == null)
                        {
                            nomPilar[numPilar] = record2.nombrepil;
                        }
                        arrTotal[numPilar] = arrTotal[numPilar] + record2.total;
                        double actividadesTotalesPorRealizarDeLaAuditoria = idtotalv;
                        if (record2.total == 0)
                        {
                            comp = 0;

                        }
                        else
                        {
                            comp = idtotalv;
                            activas = comp + activas;

                            double actividadesActivas = actividadesTotalesPorRealizarDeLaAuditoria - actividadesRealizadasDC;
                            arrActivas[numPilar] = arrActivas[numPilar] + (int)actividadesActivas;
                        }

                        double avanceAuditoria = 0;
                        if (record2.total == 0)
                        {
                            comp = record2.pondercionPilar;
                            //avanceAuditoria = 100;
                            avanceAuditoria = 0;
                        }
                        else
                        {
                            comp = (record2.total - idtotalv) / record2.total;
                            comp = comp * record2.pondercionPilar;
                            //avanceAuditoria = completadasAuditoria / record2.total * 100;
                            avanceAuditoria = completadasAuditoria / record2.total * record2.pondercionPilar;

                        }
                        avancea = comp + avancea;

                        //double avanceDC = (actividadesTotalesPorRealizarDeLaAuditoria==0)?100:actividadesRealizadasDC / actividadesTotalesPorRealizarDeLaAuditoria * 100;
                        double avanceDC = (actividadesTotalesPorRealizarDeLaAuditoria == 0) ? 0 : Convert.ToDouble(actividadesRealizadasDC) / Convert.ToDouble(record2.total) * Convert.ToDouble(record2.pondercionPilar);
                        avanceTotalDC += avanceDC;
                        //double avanceTotal =(record2.total==0)?100: (completadasAuditoria + actividadesRealizadasDC) / record2.total*100;
                        double avanceTotal = (record2.total == 0) ? record2.pondercionPilar : (completadasAuditoria + actividadesRealizadasDC) / record2.total * record2.pondercionPilar;
                        avanceTotalT += avanceTotal;

                        numPilar++;

                        double profesionalizacionNivel = (rec3.ponderacion * avanceTotalT) / 100.0;
                        totalProfesionalizacion += profesionalizacionNivel;//TOTAL PROFESIONALIZACION LINEA ROJA
                        double porcentaje = (totalc + totaldc) / totalp * 100;

                        avanceTotalFinalDC += totaldc;
                        totalpt = totalp + totalpt;
                        totalct = totalct + totalc;
                        activast = activast + activas;
                        avanceat = ((rec3.ponderacion * avancea) / 100) + avanceat;
                        totalp = 0;
                        totalc = 0;
                        activas = 0;
                        avancea = 0;
                        totaldc = 0; //se restaura totaldc a 0
                        avanceTotalT = 0;
                        avanceTotalDC = 0;

                        numPilar = 0;

                    }

                }
                return totalProfesionalizacion;

            }
        }*/
        public mdl_auditoria UltimaAuditoria {
            get {                
                int auditoriasRevisa = new XPQuery<mdl_auditoria>(this.Session).Where(x => x.Iddistribuidor.iddistribuidor == iddistribuidor && x.estatus == 1).Max(x => x.idaud);                
                mdl_auditoria au = new XPQuery<mdl_auditoria>(this.Session).FirstOrDefault(x => x.idaud == auditoriasRevisa);
                return au;                
                }
            }

        public int NivelActual
        {
            get
            {
                DevExpress.Xpo.Session session = this.Session;
                int cNivel = new XPQuery<mdl_nivel>(session).Count();
                int idAuditoria = 0;

                int nivelActual = 1;
                if (UltimaAuditoria!=null)
                {
                    idAuditoria = UltimaAuditoria.idaud;

                    for (int i = 1; i <= cNivel; i++)
                    {
                        int cantAct = new XPQuery<mdl_auditoriaactividad>(session).Count(x => x.Idaud.ToString() == idAuditoria.ToString() && x.fechacomp ==null&& x.Idactividad.NivelID.idnivel.ToString() == i.ToString());
                        //int cantAct2 = new XPQuery<mdl_auditoriaactividad>(session).Count(x => x.Idaud.ToString() == idAuditoria.ToString() && x.fechacomp == (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue && x.Idactividad.NivelID.idnivel.ToString() == i.ToString());
                        if (cantAct > 0)
                        {
                            break;
                        }
                        if (i < cNivel) { nivelActual = i + 1; }
                    }
                }
                else
                {
                    string x = nombreusuario;
                }

                string NombreNivel = new XPQuery<mdl_nivel>(session).FirstOrDefault(x => x.idnivel == nivelActual).nombreniv;
                if (nivelActividades!=NombreNivel)
                {
                    nivelActividades = NombreNivel;
                    this.Save();
                    this.Session.CommitTransaction();
                }
                

                return nivelActual;
            }
        }
        public string nivelActividades { get; set; }

        [Association("Distribuidor-Centroservicio")]
        public XPCollection<CentroServicio> CentrosDeServicio
        {
            get
            {
                return GetCollection<CentroServicio>("CentrosDeServicio");
            }
        }

        [Association("Distribuidor-Venta")]
        public XPCollection<VentaCS> Ventas
        {
            get
            {
                return GetCollection<VentaCS>("Ventas");
            }
        }
        [Association("Distribuidor-Reportes")]
        public XPCollection<Reporte> Reportes
        {
            get
            {
                return GetCollection<Reporte>("Reportes");
            }
        }

        /* [Association("Distribuidor-Objetivos")]
        public XPCollection<mdl_Objetivos> Objetivos
        {
            get
            {
                return GetCollection<mdl_Objetivos>("Objetivos");
            }
        } */

        [Association("Distribuidor-ReportesCS")]
        public XPCollection<ReporteCS> ReportesCS
        {
            get
            {
                return GetCollection<ReporteCS>("ReportesCS");
            }
        }


        [Association("Auditoria-Distribuidores")]
        public XPCollection<mdl_auditoria> Auditoria
        {
            get
            {
                return GetCollection<mdl_auditoria>("Auditoria");
            }

        }

        [Association("Distribuidor-Registros")]
        public XPCollection<mdl_RegistroMensual> Registro
        {
            get
            {
                return GetCollection<mdl_RegistroMensual>("Registro");
            }
        }

        public DateTime fechaCreacion { get; set; }

        [Association("Distribuidor - PuestoPorDistribuidores")]
        public XPCollection<PuestoPorDistribuidor> PuestoPorDistribuidor
        {
            get
            {
                return GetCollection<PuestoPorDistribuidor>("PuestoPorDistribuidor");
            }
        }

        [Association("Distribuidor - Minutas")]
        public XPCollection<Minutas> Minutas
        {
            get
            {
                return GetCollection<Minutas>("Minutas");
            }
        }

        [Association("Distribuidor-Objetivos")]
        public XPCollection<mdl_Objetivos> Objetivos
        {
            get
            {
                return GetCollection<mdl_Objetivos>("Objetivos");
            }
        }

        [Association("Regla - Distribuidor")]
        public XPCollection<ReglaEvaluacion> Reglas
        {
            get
            {
                return GetCollection<ReglaEvaluacion>("Reglas");
            }
        }
        [XafDisplayName("ID JCI")]
        public string IDJCI { get; set; }

        public bool EnDesarrollo { get; set; }

        public double VentaMayoreo { get; set; }
        public double VentaPromedio { get; set; }

        [Association("Distribuidor-NuevaAuditoria")]
        public XPCollection<NuevaAuditoria> NuevaAuditoria
        {
            get
            {
                return GetCollection<NuevaAuditoria>("NuevaAuditoria");
            }

        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            fechaCreacion = DateTime.Now;
        }



    }
    

}