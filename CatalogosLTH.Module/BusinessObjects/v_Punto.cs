using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.DC;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("v_Punto")]
    [XafDefaultProperty("NombrePunto")]

    public class v_Punto : XPLiteObject
    {
        public v_Punto() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public v_Punto(Session session) : base(session)
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

        private v_Subtema subtema;
        [Association("Subtema-Puntos")]
        public v_Subtema Subtema
        {
            get { return subtema; }
            set { SetPropertyValue("Subtema", ref subtema, value); }
        }

        [Association("Punto-Condiciones")]
        public XPCollection<v_Condicion> Condiciones
        {
            get
            {
                return GetCollection<v_Condicion>("Condiciones");
            }
        }
        [Association("Punto-Actividades")]
        public XPCollection<v_Actividad> Actividades
        {
            get
            {
                return GetCollection<v_Actividad>("Actividades");
            }
        }

        public string Nombre { get; set; }
        public double Valor { get; set; }

        private bool _habilitaNA;
        public bool habilitaNA
        {
            get { return _habilitaNA; }
            set { SetPropertyValue("habilitaNA", ref _habilitaNA, value); }
        }

        private bool _Lubricantes;
        public bool Lubricantes
        {
            get { return _Lubricantes; }
            set { SetPropertyValue("Lubricantes", ref _Lubricantes, value); }
        }

        private bool _Formal;
        public bool Formal
        {
            get { return _Formal; }
            set { SetPropertyValue("Formal", ref _Formal, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string NombrePunto { get; set; }

        public Nivel NivelPunto { get; set; }
        public LugarEvaluacion Lugar { get; set; }
        public Responsable FiguraResponsable { get; set; }
        public Usuario Evaluador { get; set; }
        public enum Nivel
        {
            BASICO,
            INTERMEDIO,
            AVANZADO,
            SOBRESALIENTE
        }

        public enum LugarEvaluacion
        {
            RUTA,
            OFICINA,
            CENTRO_DE_SERVICIO
        }

        public enum Responsable
        {
            GERENTE_COMERCIAL,
            CENTRO_DE_SERVICIO,
            ENCARGADO_DE_ALMACEN,
            TECNICO_MASTER,
            IMPULSOR_DE_MERCADO,
            ENCARGADO_LOGISTICA,
            GERENTE_DE_COBRANZA,
            GERENTE_DE_FINANZAS,
            ENCARGADO_RH,
            TECNICO_CS,
            CS,
            EJECUTIVO_DE_DESARROLLO,
            DIRECTOR_GENERAL

        }
    }

}