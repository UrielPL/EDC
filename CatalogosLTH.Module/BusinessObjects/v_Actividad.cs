using System;
using DevExpress.Xpo;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("v_Actividad")]
    [XafDefaultProperty("Nombre")]
    public class v_Actividad : XPLiteObject
    {
        public v_Actividad() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public v_Actividad(Session session) : base(session)
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
        [Size(SizeAttribute.Unlimited)]
        public string Nombre { get; set; }
        public string Numero { get; set; }
        public string Letra { get; set; }
        public double Valor { get; set; }

        private bool _activo;
        public bool activo
        {
            get { return _activo; }
            set { SetPropertyValue("activo", ref _activo, value); }
        }

        private string _Objetivo;
        [Size(SizeAttribute.Unlimited)]
        public string Objetivo
        {
            get { return _Objetivo; }
            set { SetPropertyValue("Objetivo", ref _Objetivo, value); }
        }

        private string _QueHacer;
        [Size(SizeAttribute.Unlimited)]
        public string QueHacer
        {
            get { return _QueHacer; }
            set { SetPropertyValue("QueHacer", ref _QueHacer, value); }
        }

        private int _Vigencia;
        public int Vigencia
        {
            get { return _Vigencia; }
            set { SetPropertyValue("Vigencia", ref _Vigencia, value); }
        }

        private v_Punto punto;
        [Association("Punto-Actividades")]
        public v_Punto Punto
        {
            get { return punto; }
            set { SetPropertyValue("Punto", ref punto, value); }
        }

        public Nivel NivelActividad { get; set; }

        
        [Association("NuevaAuditoriaActividad-v_Actividad")]
        public XPCollection<NuevaAuditoriaActividad> NuevaAuditoriaActividad
        {
            get
            {
                return GetCollection<NuevaAuditoriaActividad>("NuevaAuditoriaActividad");
            }
        }

        public enum Nivel
        {
            BÁSICO,
            INTERMEDIO,
            AVANZADO,
            SOBRESALIENTE
        }

    }

}