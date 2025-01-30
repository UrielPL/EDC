using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.DC;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("ObjetivosMinutas")]
    [XafDefaultProperty("cant")]
    public class mdl_Objetivos : XPObject
    {
        public int cant { get; set; }
        public mdl_Objetivos() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public mdl_Objetivos(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        private mdl_MesesObjetivos mesesObjetivos;
        [Association("MesesObjetivos-Objetivos")]
        public mdl_MesesObjetivos MesesObjetivos
        {
            get { return mesesObjetivos; }
            set { SetPropertyValue("MesesObjetivos", ref mesesObjetivos, value); }
        }

        private mdl_distribuidor distribuidor;
        [Association("Distribuidor-Objetivos")]
        public mdl_distribuidor Distribuidor
        {
            get { return distribuidor; }
            set { SetPropertyValue("Distribuidor", ref distribuidor, value); }
        }

        private mdl_DistribuidoresMinutas distribuidorMinutas;
        [Association("DistribuidorMinutas-Objetivos")]
        public mdl_DistribuidoresMinutas DistribuidorMinutas
        {
            get { return distribuidorMinutas; }
            set { SetPropertyValue("DistribuidorMinutas", ref distribuidorMinutas, value); }
        }
    }

}