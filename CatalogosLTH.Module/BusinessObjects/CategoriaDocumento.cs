using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.DC;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("Categoría documentos")]
    [XafDefaultProperty("Nombre")]
    public class CategoriaDocumento : XPObject
    {
        public CategoriaDocumento() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public CategoriaDocumento(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }
        public string Nombre { get; set; }

        [Association("Categoria-Documentos")]
        public XPCollection<Documento> documento
        {
            get
            {
                return GetCollection<Documento>("documento");
            }
        }
    }


}