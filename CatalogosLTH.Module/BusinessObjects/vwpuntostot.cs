using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.ComponentModel;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class vwpuntostot : XPLiteObject
    {
        public vwpuntostot(Session session) : base(session) { }

        [Key, Persistent, Browsable(false)]
        public vwpuntostotKey Key;

        public int idtipoaud { get { return Key.idtipoaud; } }
        public int idaud { get { return Key.idaud; } }
        public int idnivel { get { return Key.idnivel; } }
        public int idpilar { get { return Key.idpilar; } }
        public string nombreniv { get { return Key.nombreniv; } }
        public string nombrepil { get { return Key.nombrepil; } }
        public string Code { get { return Key.Code; } }
        public string Texto { get { return Key.Texto; } }
        
        public int resultado { get { return Key.resultado; } }
        public string llave { get { return Key.llave; } }




    }
    public struct vwpuntostotKey
    {
        [Persistent("idtipoaud"), Browsable(false)]
        public int idtipoaud;
        [Persistent("idaud"), Browsable(false)]
        public int idaud;
        [Persistent("idnivel"), Browsable(false)]
        public int idnivel;
        [Persistent("idpilar"), Browsable(false)]
        public int idpilar;
        [Persistent("nombreniv"), Browsable(false)]
        public string nombreniv;
        [Persistent("nombrepil"), Browsable(false)]
        public string nombrepil;
        [Persistent("resultado"), Browsable(false)]
        public int resultado;
        [Persistent("Code"), Browsable(false)]
        public string Code;
        [Persistent("Texto"), Browsable(false)]
        public string Texto;
        [Persistent("llave"), Browsable(false)]
        public string llave;



    }
}
