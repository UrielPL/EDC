using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using System.IO;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CentroServicio : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public CentroServicio(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        public string Nombre { get; set; }
        public string Clave { get; set; }
        public string Zona { get; set; }
        [Action(Caption = "Importar_Centros")]
        public void Importar()
        {
            if (ArchivoCS != null)
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + "\\archivos\\";
                string nombre = ArchivoCS.FileName;

                FileStream fileStream = System.IO.File.Create(ruta + nombre);

                ArchivoCS.SaveToStream(fileStream);
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

                    using (UnitOfWork v2 = new UnitOfWork()) {
                        CentroServicio temp = new CentroServicio(vrs);
                        temp.iddistribuidor = new XPQuery<mdl_distribuidor>(vrs).FirstOrDefault(x => x.nombredist == values[0]);
                        temp.Zona = values[1];
                        temp.Clave = values[2];
                        temp.Nombre = values[3];
                        temp.Save();
                        v2.CommitChanges();
                    }

                }//END OF STREAM

            }
        }

        [XafDisplayName("Archivo excel")]
        private FileData archivoCS;
        [DevExpress.Xpo.Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        public FileData ArchivoCS
        {
            get { return archivoCS; }
            set
            {
                SetPropertyValue("ArchivoCS", ref archivoCS, value);

            }
        }

        private mdl_distribuidor iddistribuidor;
        [Association("Distribuidor-Centroservicio")]
        [XafDisplayName("Distribuidor")]
        public mdl_distribuidor Iddistribuidor
        {
            get { return iddistribuidor; }
            set { SetPropertyValue("Iddistribuidor", ref iddistribuidor, value); }
        }

        public int ventaPromedio { get; set; }
        /*

        [Association("Centroservicio-Reportes")]
        public XPCollection<ReporteCS> Reportes
        {
            get
            {
                return GetCollection<ReporteCS>("Reportes");
            }
        }*/

        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}