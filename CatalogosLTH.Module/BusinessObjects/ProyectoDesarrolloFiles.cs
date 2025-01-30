using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("Proyecto Desarrollo Archivos")]
    [XafDefaultProperty("Id")]
    public class ProyectoDesarrolloFiles : XPLiteObject
    {
        public ProyectoDesarrolloFiles() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public ProyectoDesarrolloFiles(Session session) : base(session)
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
        public int Id { get; set; }

        private DateTime _fechaCargada;
        public DateTime fechaCargada
        {
            get { return _fechaCargada; }
            set { SetPropertyValue("fechaCargada", ref _fechaCargada, value); }
        }

        private string _fileURL;
        [Size(SizeAttribute.Unlimited)]
        public string fileURL
        {
            get { return _fileURL; }
            set { SetPropertyValue("fileURL", ref _fileURL, value); }
        }

        private string _nombreFile;
        [Size(SizeAttribute.Unlimited)]
        public string nombreFile
        {
            get { return _nombreFile; }
            set { SetPropertyValue("nombreFile", ref _nombreFile, value); }
        }

        private string _autor;
        [XafDisplayName("Compartido por")]
        [Size(SizeAttribute.Unlimited)]
        public string autor
        {
            get { return _autor; }
            set { SetPropertyValue("autor", ref _autor, value); }
        }
    }
}
