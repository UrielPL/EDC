using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace CatalogosLTH.Module.BusinessObjects
{
    //[DefaultClassOptions]
    //[XafDefaultProperty("Codigo")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ActividadKpi : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        private mdl_pilar pilar;
        [Association("Pilar - ActividadesKpi")]
        public mdl_pilar Pilar
        {
            get { return pilar; }
            set { SetPropertyValue("Pilar", ref pilar, value); }
        }

        private mdl_Area area;
        [Association("Area - ActividadesKpi")]
        public mdl_Area Area
        {
            get { return area; }
            set { SetPropertyValue("Area", ref area, value); }
        }
        public string TipoDeActividad { set; get; }

        private mdl_nivel nivel;
        [Association("Nivel - ActividadesKpi")]
        public mdl_nivel Nivel
        {
            get { return nivel; }
            set { SetPropertyValue("Nivel", ref nivel, value); }
        }

        public string Codigo { set; get; }

        /*[DbType("nvarchar(max)")]
        [Size(SizeAttribute.Unlimited )]*/
        public string Actividad { set; get; }

        private Puesto puesto;
        [Association("Puesto - ActividadesKpi")]
        public Puesto Puesto
        {
            get { return puesto; }
            set { SetPropertyValue("Puesto", ref puesto, value); }
        }

        public bool Configurable { set; get; }

        public enum Roles
        {
            CentroServicio,
            Distribuidor
        }
        private Roles rol;
        public Roles Rol
        {
            get { return rol; }
            set { rol = value; }
        }   

        public ActividadKpi(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}