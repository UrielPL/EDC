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

    [DefaultClassOptions]
    public class vwactividad : XPLiteObject
    {
        public vwactividad(Session session) : base(session){}
        
        [Key, Persistent, Browsable(false)]
        public vwactividadKey Key;

        [Persistent("nombrepil")]
        public string nombrepil { get { return Key.nombrepil; } }

        [Persistent("idnivel")]
        public int idnivel { get { return Key.idnivel; } }

        [Persistent("idtipoaud")]
        public int idtipoaud { get { return Key.idtipoaud; } }

        [Persistent("idaud")]
        public int idaud { get { return Key.idaud; } }

        [Persistent("idpilar")]
        public int idpilar { get { return Key.idpilar; } }

        [Persistent("Code")]
        public string Code { get { return Key.Code; } }

        [Persistent("secuencia")]
        public int secuencia { get { return Key.secuencia; } }

        [Persistent("Texto")]
        public string Texto { get { return Key.Texto; } }


        [Persistent("IdActividad")]
        public int IdActividad { get { return Key.IdActividad; } }

        [Persistent("duracion")]
        public int duracion { get { return Key.duracion; } }

        [Persistent("fechaap")]
        public DateTime fechaap { get { return Key.fechaap; } }

        [Persistent("estatus")]
        public int estatus { get { return Key.estatus; } }

        }
        public struct vwactividadKey
        {
            [Persistent("nombrepil"), Browsable(false)]
            public string nombrepil;
            [Persistent("idnivel"), Browsable(false)]
            public int idnivel;
            [Persistent("idtipoaud"), Browsable(false)]
            public int idtipoaud;
            [Persistent("idaud"), Browsable(false)]
            public int idaud;

            [Persistent("idpilar"), Browsable(false)]
            public int idpilar;
            [Persistent("Code"), Browsable(false)]
            public string Code;
            [Persistent("secuencia"), Browsable(false)]
            public int secuencia;
            [Persistent("Texto"), Browsable(false)]
            public string Texto;

            [Persistent("IdActividad"), Browsable(false)]
            public int IdActividad;
            [Persistent("duracion"), Browsable(false)]
            public int duracion;
            [Persistent("fechaap"), Browsable(false)]
            public DateTime fechaap;
            [Persistent("estatus"), Browsable(false)]
            public int estatus;
   

        }

}