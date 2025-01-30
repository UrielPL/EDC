using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.DC;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("MesesObjetivos")]
    [XafDefaultProperty("mes")]
    public class mdl_MesesObjetivos : XPObject
    {
        public string mes { get; set; }

        public mdl_MesesObjetivos() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public mdl_MesesObjetivos(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        [Association("MesesObjetivos-Objetivos")]
        public XPCollection<mdl_Objetivos> Objetivos
        {
            get
            {
                return GetCollection<mdl_Objetivos>("Objetivos");
            }
        }
    }

}