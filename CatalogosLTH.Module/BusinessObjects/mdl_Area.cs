using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.BaseImpl;
using System.IO;
using System.Text;
using System.Linq;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("Área")]
    [XafDefaultProperty("Nombre")]
    public class mdl_Area : XPLiteObject
    {

        [Action(Caption = "Importar_B")]
        public void Importar()
        {

            if (ArchivoFechas != null)
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + "\\archivos\\";
                string nombre = "TIPOB" + ArchivoFechas.FileName;
                
                FileStream fileStream = System.IO.File.Create(ruta + nombre);

                ArchivoFechas.SaveToStream(fileStream);
                fileStream.Close();
                Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                var reader = new StreamReader(File.OpenRead(ruta + nombre), iso);
                var line = reader.ReadLine();
                DevExpress.Xpo.Session vrs = this.Session;


                mdl_tipoauditoria tipo = new XPQuery<mdl_tipoauditoria>(vrs).FirstOrDefault(x => x.Descripcion == "B");
                

                    int i = 0;

                while (!reader.EndOfStream)
                    {
                        i++;
                        line = reader.ReadLine();
                        var values = line.Split(',');
                        mdl_Area area = new XPQuery<mdl_Area>(vrs).FirstOrDefault(x => x.Nombre.Trim() == values[1].Trim()&&x.Idtipoaud==tipo);
                        if (area==null)
                        {
                            using (UnitOfWork varses = new UnitOfWork())
                            {
                                mdl_punto punto = new XPQuery<mdl_punto>(vrs).FirstOrDefault(y => y.clavepunto == values[0].Trim());
                                if(punto==null){string v = values[1];}

                                mdl_Area a1 = new mdl_Area(vrs);
                                a1.Nombre = values[0].Trim();
                                a1.idtipoaud = tipo;
                                a1.Puntos.Add(punto);
                                a1.Save();
                                varses.CommitChanges();
                            }
                        }
                        else
                        {
                            using (UnitOfWork v2 = new UnitOfWork())
                            {
                                mdl_punto punto = new XPQuery<mdl_punto>(vrs).FirstOrDefault(y => y.clavepunto == values[0].Trim());
                                if (punto == null) { string v = values[1]; }
                                                                
                                area.Puntos.Add(punto);
                                area.Save();
                                v2.CommitChanges();
                            }
                        }

                      
                    }//END OF STREAM
               

            }
        }


        public mdl_Area() : base() { }

        public mdl_Area(Session session) : base(session) { }

        public override void AfterConstruction() { }

        [Key(true)]
        public int IdArea { get; set; }

        public string Nombre { get; set; }

        [Association("Puntos-Areas")]
        public XPCollection<mdl_punto> Puntos
        {
            get
            {
                return GetCollection<mdl_punto>("Puntos");
            }
        }

        private mdl_tipoauditoria idtipoaud;
        [Association("TipoAuditoria-Areas")]
        [XafDisplayName("Tipo Auditoria")]
        public mdl_tipoauditoria Idtipoaud
        {
            get { return idtipoaud; }
            set { SetPropertyValue("Idtipoaud", ref idtipoaud, value); }
        }


        [XafDisplayName("Relación puntos areas")]
        private FileData archivoFechas;
        [DevExpress.Xpo.Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        public FileData ArchivoFechas
        {
            get { return archivoFechas; }
            set
            {
                SetPropertyValue("ArchivoFechas", ref archivoFechas, value);

            }
        }

        [Association("Area - ActividadesNuevas")]
        public XPCollection<ActividadNueva> ActividadNueva
        {
            get
            {
                return GetCollection<ActividadNueva>("ActividadNueva");
            }
        }

        [Association("Area - ActividadesKpi")]
        public XPCollection<ActividadKpi> ActividadKpi
        {
            get
            {
                return GetCollection<ActividadKpi>("ActividadKpi");
            }
        }

    }

}