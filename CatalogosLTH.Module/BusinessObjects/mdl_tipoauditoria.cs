using System;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.DC;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("Tipo Auditoria")]
    [XafDefaultProperty("idTipoAuditoria")]
    public class mdl_tipoauditoria : XPLiteObject
    {
        public mdl_tipoauditoria(Session session): base(session)        {        }


        [Key(true)]
        public int idTipoAuditoria { get; set; }

        [Association("TipoAuditoria-PondNivel")]
        public XPCollection<mdl_pondnivel> PondNivel
        {
            get
            {
                return GetCollection<mdl_pondnivel>("PondNivel");
            }
        }
        [Association("CatNivel-TipoAuditoria")]
        public XPCollection<mdl_catnivel> CatNivel
        {
            get
            {
                return GetCollection<mdl_catnivel>("CatNivel");
            }
        }
        [Association("Auditoria-Tipos")]
        public XPCollection<mdl_auditoria> Auditoria
        {
            get
            {
                return GetCollection<mdl_auditoria>("Auditoria");
            }
        }
        [Association("TipoAuditoria-Puntos")]
        public XPCollection<mdl_punto> Punto
        {
            get
            {
                return GetCollection<mdl_punto>("Punto");
            }
        }

        [Association("TipoAuditoria-Areas")]
        public XPCollection<mdl_Area> Areas
        {
            get
            {
                return GetCollection<mdl_Area>("Areas");
            }
        }
        public string Descripcion { get; set; }
        

    }

    


    
}