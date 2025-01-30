using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.ComponentModel;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class vwtotal : XPLiteObject
    {
        public vwtotal(Session session) : base(session) { }
        [Key, Persistent, Browsable(false)]
        public vwtotalKey Key;
        public int idcatnivel { get { return Key.idcatnivel; } }
        public int idnivel { get { return Key.idnivel; } }
        public int idtipoaud { get { return Key.idtipoaud; } }
        public int ponderacionNivel { get { return Key.ponderacionNivel; } }
        public int idpilar { get { return Key.idpilar; } }
        public int pondercionPilar { get { return Key.pondercionPilar; } }
        public int total { get { return Key.total; } }

        public string nombreniv { get { return Key.nombreniv; } }
        public string nombrepil { get { return Key.nombrepil; } }



    }
    public struct vwtotalKey
    {
        [Persistent("idcatnivel"), Browsable(false)]
        public int idcatnivel;
        [Persistent("idnivel"), Browsable(false)]
        public int idnivel;
        [Persistent("idtipoaud"), Browsable(false)]
        public int idtipoaud;
        [Persistent("ponderacionNivel"), Browsable(false)]
        public int ponderacionNivel;
        [Persistent("idpilar"), Browsable(false)]
        public int idpilar;
        [Persistent("pondercionPilar"), Browsable(false)]
        public int pondercionPilar;
        [Persistent("total"), Browsable(false)]
        public int total;
        [Persistent("nombreniv"), Browsable(false)]
        public string nombreniv;
        [Persistent("nombrepil"), Browsable(false)]
        public string nombrepil;

    }
}
