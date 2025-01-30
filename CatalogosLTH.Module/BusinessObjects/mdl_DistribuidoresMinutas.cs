using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.DC;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("Distribuidores Minutas")]
    [XafDefaultProperty("nombredist")]
    public class mdl_DistribuidoresMinutas : XPObject
    {
        public string nombredist { get; set; }
        public DateTime fechaRegistro { get; set; }

        public mdl_DistribuidoresMinutas() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public mdl_DistribuidoresMinutas(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        [Association("DistribuidorMinutas - Minutas")]
        public XPCollection<Minutas> Minutas
        {
            get
            {
                return GetCollection<Minutas>("Minutas");
            }
        }

        [Association("DistribuidorMinutas-Objetivos")]
        public XPCollection<mdl_Objetivos> Objetivos
        {
            get
            {
                return GetCollection<mdl_Objetivos>("Objetivos");
            }
        }

        private Usuario usuario;
        [Association("Usuario - DistribuidorMinutas")]
        public Usuario Usuario
        {
            get { return usuario; }
            set { SetPropertyValue("Usuario", ref usuario, value); }
        }
    }

}