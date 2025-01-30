using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.DC;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDefaultProperty("Distribuidor")]

    public class ReglaEvaluacion : XPObject
    {
        public ReglaEvaluacion() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public ReglaEvaluacion(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        public string Comentario { get; set; }

        private mdl_distribuidor distribuidor;
        [Association("Regla - Distribuidor")]
        public mdl_distribuidor Distribuidor
        {
            get { return distribuidor; }
            set { SetPropertyValue("Distribuidor ", ref distribuidor, value); }
        }

        private mdl_actividad act;
        [Association("Regla - Actividad")]
        public mdl_actividad Actividad
        {
            get { return act; }
            set { SetPropertyValue("Actividad ", ref act, value); }
        }

        private Usuario evaluador;
        [Association("Regla - Evaluador")]
        public Usuario Evaluador
        {
            get { return evaluador; }
            set { SetPropertyValue("Evaluador ", ref evaluador, value); }
        }
    }

}