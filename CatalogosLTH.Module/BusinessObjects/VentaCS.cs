using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.DC;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class VentaCS : XPObject
    {
        public VentaCS() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public VentaCS(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        private mdl_distribuidor iddistribuidor;
        [Association("Distribuidor-Venta")]
        [XafDisplayName("Distribuidor")]
        public mdl_distribuidor Iddistribuidor
        {
            get { return iddistribuidor; }
            set { SetPropertyValue("Iddistribuidor", ref iddistribuidor, value); }
        }


        private int venta;
        public int Venta
        {
            get { return venta; }
            set { SetPropertyValue("Venta", ref venta, value); }            
        }

        private int ventaPromedio;
        public int VentaPromedio
        {
            get { return ventaPromedio; }
            set { SetPropertyValue("VentaPromedio", ref ventaPromedio, value); }
        }

        private string centroServicio;
        public string CentroServicio
            {
            get { return centroServicio; }
            set { SetPropertyValue("CentroServicio", ref centroServicio, value); }
            }

        private string claveCentro;
        public string ClaveCentro
        {
            get { return claveCentro; }
            set { SetPropertyValue("ClaveCentro", ref claveCentro, value); }
        }

        private bool mayoreo;
        public bool Mayoreo
        {
            get { return mayoreo; }
            set { SetPropertyValue("Mayoreo", ref mayoreo, value); }
        }

        private DateTime fecha;
        public DateTime Fecha
        {
            get { return fecha; }
            set { SetPropertyValue("Fecha", ref fecha, value); }
        }

        private string comentario;
        [Size(SizeAttribute.Unlimited)]
        public string Comentario
        {
            get { return comentario; }
            set { SetPropertyValue("Comentario", ref comentario, value); }
        }
    }

}