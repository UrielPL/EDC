using System;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Base;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class mdl_Kardex : XPObject
    {
        [Action(Caption = "Generar Objetivos")]
        public void GenerarObjetivos()
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
                var line = reader.ReadLine();
                List<mdl_distribuidor> lstDist = new XPQuery<mdl_distribuidor>(this.Session).ToList();
                //Session varses = new Session();

                using (UnitOfWork varses = new UnitOfWork())
                {

                    int i = 0;

                    while (!reader.EndOfStream)
                    {
                        i++;
                        line = reader.ReadLine();
                        var values = line.Split(',');
                        var anioEvaluar = DateTime.Now.Year;
                        foreach (var item in lstDist)
                        {
                            if(DateTime.Now.Month >= 10)
                            {
                                anioEvaluar++;
                            }
                            if (item.nombredist.Equals(values[0].Trim()))
                            {
                                var temp = item.Registro.Where(x => x.Periodo.Periodo == anioEvaluar && x.Distribuidor.iddistribuidor == item.iddistribuidor).OrderBy(x=>x.orden).ToList();
                                int kardexObj = 1;
                                foreach (var itemReg in temp)
                                {
                                    itemReg.ObjetivoProf = float.Parse(values[kardexObj]);
                                    kardexObj++;
                                    itemReg.ObjetivoAct = int.Parse(values[kardexObj]);
                                    kardexObj++;
                                    itemReg.Save();
                                }
                            }
                        }
                    }
                    varses.CommitChanges();
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
        public double Profesionalizacion { get; set; }
        public double ProfesionalizacionObjetivo { get; set; }
        public double AvanceProf { get; set; }
        public int ActTerminadas { get; set; }
        public int ActObjetivo { get; set; }
        public int AvanceAct { get; set; }
        public bool Cerrada { get; set; }

        private mdl_distribuidor distribuidor;
        [Association("Distribuidor-Kardex")]
        public mdl_distribuidor Distribuidor
        {
            get { return distribuidor; }
            set { SetPropertyValue("Distribuidor", ref distribuidor, value); }
        }

        public string EjecutivoDC { get; set; }
        public DateTime FechaVisita { get; set; }
        public string GerenteCuenta { get; set; }
        public double PilarInfraestructura { get; set; }
        public double PilarAdministracion { get; set; }
        public double PilarPlaneacion { get; set; }
        public double PilarEjecucion { get; set; }
        public double PilarPS { get; set; }
        public DateTime UltAcceso { get; set; }
        public int DiasSinAccesar { get; set; }
        public string ActRevisadasEjec { get; set; }
        public string AcuerdosRevision { get; set; }

        public mdl_Kardex() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public mdl_Kardex(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }
    }

}