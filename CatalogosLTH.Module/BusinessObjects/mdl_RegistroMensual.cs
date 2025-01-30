using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.DC;
using System.Linq;
using DevExpress.Persistent.BaseImpl;
using System.IO;
using System.Text;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("Registro Mensual")]
    [XafDefaultProperty("IdRegistro")]
    public class mdl_RegistroMensual : XPLiteObject
    {
        [Action(Caption = "Actualizar Valores con Profesionalización ACTUAL")]
        public void actualiza()
        {
            DevExpress.Xpo.Session session = this.Session;
            int[] meses = new int[12] { 10, 11, 12, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            mdl_distribuidor dist = Distribuidor;
            int o = meses[orden-1];
            int mesActual = DateTime.Now.Month;
            int year = (mesActual > 9) ? periodo.Periodo - 1 : periodo.Periodo;

            int actTerminadas = new XPQuery<mdl_auditoriaactividad>(session).Where(x => x.Idaud == dist.UltimaAuditoria && x.fechacomp.Month == o && x.fechacomp.Year == year ).Count();
            terminadas = actTerminadas;
            resultado = dist.profesionalizacion;
            nivel = dist.nivelAct;
            
            this.Save();
            this.Session.CommitTransaction();
        }

        [Action(Caption = "Actualiza Objetivos Mensuales")]
        public void actualizaObjetivos()
        {
            if (ArchivoImportar != null)
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + "\\archivos\\";
                string nombre = ArchivoImportar.FileName;

                FileStream fileStream = System.IO.File.Create(ruta + nombre);

                ArchivoImportar.SaveToStream(fileStream);
                fileStream.Close();
                Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                var reader = new StreamReader(File.OpenRead(ruta + nombre), iso);
                //Session varses = new Session();

                int i = 0;
                using (UnitOfWork uow = new UnitOfWork())
                {
                    while (!reader.EndOfStream)
                    {
                        i++;
                        var line = reader.ReadLine();
                        var values = line.Split(';');

                        if(values[1] == "PROFESIONALIZACIÓN") {

                        } else {

                        }
                    }
                }
            }
        }
        private FileData archivoImportar;
        [DevExpress.Xpo.Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        public FileData ArchivoImportar
        {
            get { return archivoImportar; }
            set
            {
                SetPropertyValue("ArchivoImportar", ref archivoImportar, value);

            }
        }
        public mdl_RegistroMensual() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public mdl_RegistroMensual(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        [Key(true)]
        public int IdRegistro { get; set; }
        public double resultado { get; set; }
        public string nivel { get; set; }
        public int terminadas { get; set; }
        public int orden { get; set; }
        public string Mes { get; set; }
        public double ObjetivoProf { get; set; }
        public int ObjetivoAct { get; set; }
        private mdl_periodo periodo;
        [Association("Periodo-Registro")]
        public mdl_periodo Periodo
        {
            get { return periodo; }
            set { SetPropertyValue("Periodo", ref periodo, value); }
        }


        private mdl_distribuidor distribuidor;
        [Association("Distribuidor-Registros")]
        public mdl_distribuidor Distribuidor
        {
            get { return distribuidor; }
            set { SetPropertyValue("Distribuidor", ref distribuidor, value); }
        }
    }

}