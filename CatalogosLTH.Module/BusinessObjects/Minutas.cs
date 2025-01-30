using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.BaseImpl;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("Minutas")]
    [XafDefaultProperty("Oid")]
    public class Minutas : XPObject
    {
        public Minutas() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public Minutas(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public DateTime Fecha { get; set; }
        public FileData Archivo { get; set; }
        public string Comentario { get; set; }
        public string FY { get; set; }

        private mdl_distribuidor distribuidor;
        [Association("Distribuidor - Minutas")]
        public mdl_distribuidor Distribuidor
        {
            get { return distribuidor; }
            set { SetPropertyValue("Distribuidor", ref distribuidor, value); }
        }

        private mdl_DistribuidoresMinutas distribuidorMinutas;
        [Association("DistribuidorMinutas - Minutas")]
        public mdl_DistribuidoresMinutas DistribuidorMinutas
        {
            get { return distribuidorMinutas; }
            set { SetPropertyValue("DistribuidorMinutas", ref distribuidorMinutas, value); }
        }

        private Usuario usuario;
        [Association("Usuario - Minutas")]
        public Usuario Usuario
        {
            get { return usuario; }
            set { SetPropertyValue("Usuario", ref usuario, value); }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }
    }

}