using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.DC;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("Logs Usuario")]
    [XafDefaultProperty("Usuario.username")]
    public class ActividadUsuario : XPObject
    {
        public ActividadUsuario() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public ActividadUsuario(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        public DateTime Fecha { get; set; }

        public string Hora
        {
            get
            {
                return this.Fecha.ToString("hh:mm:ss");
            }
        }

        private Usuario usuario;
        [Association("Actividad-Usuario")]
        public Usuario Usuario
        {
            get { return usuario; }
            set { SetPropertyValue("Usuario", ref usuario, value); }
        }
    }

}