using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using System.ComponentModel;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class vwactacertadas : XPLiteObject
    {
        public vwactacertadas(Session session) : base(session){ }

        [Key, Persistent, Browsable(false)]
        public vwactacertadasKey Key;
        [Persistent("IdActividad")]
        public int IdActividad { get { return Key.IdActividad; } }
        [Persistent("idaud")]
        public int idaud { get { return Key.idaud; } }
        [Persistent("Code")]
        public string Code { get { return Key.Code; } }
        [Persistent("idpunto")]
        public int idpunto { get { return Key.idpunto; } }
    }
    public struct vwactacertadasKey
    {
        [Persistent("IdActividad"), Browsable(false)]
        public int IdActividad;
        [Persistent("idaud"), Browsable(false)]
        public int idaud;
        [Persistent("Code"), Browsable(false)]
        public string Code;
        [Persistent("idpunto"), Browsable(false)]
        public int idpunto;
       
    }
}