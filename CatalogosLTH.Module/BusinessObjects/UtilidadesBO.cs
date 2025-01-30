using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogosLTH.Module.BusinessObjects
{
    public class UtilidadesBO
    {
        public static DevExpress.Xpo.Session getsession()
        {
            /*DevExpress.Xpo.Session session = new DevExpress.Xpo.Session();
            session.ConnectionString = MySqlConnectionProvider.GetConnectionString("172.93.106.146", "lth", "output", "lth2");*/
            DevExpress.Xpo.Session session = new DevExpress.Xpo.Session();
            if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null)
            {
                session.ConnectionString = ConfigurationManager.
                   ConnectionStrings["ConnectionString"].ConnectionString;
            }

            return session;
        }

        public static void generaRegistrosMensuales()
        {
            DevExpress.Xpo.Session session = getsession();
            /*XPQuery<mdl_distribuidor> distribuidores = session.Query<mdl_distribuidor>();
            List<mdl_distribuidor> listaD = new List<mdl_distribuidor>();
            foreach (var dis in distribuidores)
            {
                if (dis.Registro.Count() == 0)
                {
                    listaD.Add(dis);
                }
            }*/

            List<string> meses = new List<string>();

            meses.Add("OCTUBRE");
            meses.Add("NOVIEMBRE");
            meses.Add("DICIEMBRE");
            meses.Add("ENERO");
            meses.Add("FEBRERO");
            meses.Add("MARZO");
            meses.Add("ABRIL");
            meses.Add("MAYO");
            meses.Add("JUNIO");
            meses.Add("JULIO");
            meses.Add("AGOSTO");
            meses.Add("SEPTIEMBRE");
            var periodo = new XPQuery<mdl_periodo>(session).OrderByDescending(v => v.Periodo).FirstOrDefault();
            //Distribuidores sin registros mensuales (generalmente por ser agregados recientemente)
            List<mdl_distribuidor> distribuidores = session.Query<mdl_distribuidor>().Where(x => x.Registro.Count() == 0 && x.nombredist != "nohay").ToList();
            int cont = 0;

            using (UnitOfWork varses = new UnitOfWork())
            {
                foreach (var mes in meses)
                {
                    cont++;
                    foreach (var dis in distribuidores)
                    {
                        mdl_RegistroMensual registro = new mdl_RegistroMensual(session);

                        registro.Periodo = periodo;
                        registro.orden = cont;
                        registro.Distribuidor = dis;
                        registro.Mes = mes;

                        registro.resultado = 0;
                        registro.terminadas = 0;
                        registro.Save();
                    }
                }

                varses.CommitChanges();
            }
           distribuidores = new XPQuery<mdl_distribuidor>(session).Where(x => x.iddistribuidor == 103 || x.iddistribuidor == 104 || x.iddistribuidor == 105 || x.iddistribuidor == 107).ToList();


            foreach (var item in distribuidores)
            {
                cont = 1;
                foreach (var mes in meses)
                {
                    actualizaRegistroMensual(item.nombredist, null, cont);
                    cont++;
                }
                
            }
        }

        public static void actualizaRegistroMensual(string distribuidor, mdl_auditoriaactividad audact, int mes)
        {
            DevExpress.Xpo.Session session = getsession();
            int mesActual = DateTime.Now.Month;
            int[] meses = new int[12] { 10, 11, 12, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int orden = Array.IndexOf(meses, mes) + 1;// se suma uno porque el index inicial es 0 y el orden en la BD empieza en 1
            int year = (mesActual > 9) ? DateTime.Now.Year + 1 : DateTime.Now.Year;

            mdl_distribuidor dist = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist == distribuidor);

            mdl_RegistroMensual registro = new XPQuery<mdl_RegistroMensual>(session)
                .FirstOrDefault(x => x.Periodo.Periodo == year && x.orden == orden && x.Distribuidor == dist); //Registros del periodo y mes seleccionados

            using (UnitOfWork uow = new UnitOfWork())
            {
                int cont = 0;

                cont++;

                int actTerminadas = new XPQuery<mdl_auditoriaactividad>(session)
                    .Where(x => x.Idaud == dist.UltimaAuditoria && x.fechacomp.Month == mes && x.fechacomp.Year == DateTime.Now.Year).Count();
                registro.resultado = dist.profesionalizacion;
                registro.nivel = dist.nivelAct;
                registro.terminadas = actTerminadas;
                registro.Save();

                uow.CommitChanges();
            }
        }
    }
}
