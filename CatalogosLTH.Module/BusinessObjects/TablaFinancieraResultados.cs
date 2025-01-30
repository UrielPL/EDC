using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("Tabla Financiera")]
    [XafDefaultProperty("Id")]
    public class TablaFinancieraResultados: XPLiteObject
    {
        public TablaFinancieraResultados() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public TablaFinancieraResultados(Session session) : base(session)
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

        private DateTime _fechaRealizada;
        public DateTime fechaRealizada
        {
            get { return _fechaRealizada; }
            set { SetPropertyValue("fechaRealizada", ref _fechaRealizada, value); }
        }

        private bool _completa;
        [XafDisplayName("Completada")]
        public bool completa
        {
            get { return _completa; }
            set { SetPropertyValue("completa", ref _completa, value); }
        }

        private string _estatus;
        public string estatus
        {
            get { return _estatus; }
            set { SetPropertyValue("estatus", ref _estatus, value); }
        }

        #region
        private string _areaMUB;
        [Size(SizeAttribute.Unlimited)]
        public string areaMUB
        {
            get { return _areaMUB; }
            set { SetPropertyValue("areaMUB", ref _areaMUB, value); }
        }

        private string _areaMUO;
        [Size(SizeAttribute.Unlimited)]
        public string areaMUO
        {
            get { return _areaMUO; }
            set { SetPropertyValue("areaMUO", ref _areaMUO, value); }
        }

        private string _areaLRR;
        [Size(SizeAttribute.Unlimited)]
        public string areaLRR
        {
            get { return _areaLRR; }
            set { SetPropertyValue("areaLRR", ref _areaLRR, value); }
        }
        
        private string _areaLRC;
        [Size(SizeAttribute.Unlimited)]
        public string areaLRC
        {
            get { return _areaLRC; }
            set { SetPropertyValue("areaLRC", ref _areaLRC, value); }
        }

        private string _areaRI;
        [Size(SizeAttribute.Unlimited)]
        public string areaRI
        {
            get { return _areaRI; }
            set { SetPropertyValue("areaRI", ref _areaRI, value); }
        }

        private string _areaPPC;
        [Size(SizeAttribute.Unlimited)]
        public string areaPPC
        {
            get { return _areaPPC; }
            set { SetPropertyValue("areaPPC", ref _areaPPC, value); }
        }

        private string _areaPPP;
        [Size(SizeAttribute.Unlimited)]
        public string areaPPP
        {
            get { return _areaPPP; }
            set { SetPropertyValue("areaPPP", ref _areaPPP, value); }
        }

        private string _areaE;
        [Size(SizeAttribute.Unlimited)]
        public string areaE
        {
            get { return _areaE; }
            set { SetPropertyValue("areaE", ref _areaE, value); }
        }

        private string _areaGO;
        [Size(SizeAttribute.Unlimited)]
        public string areaGO
        {
            get { return _areaGO; }
            set { SetPropertyValue("areaGO", ref _areaGO, value); }
        }

        private string _areaRAO;
        [Size(SizeAttribute.Unlimited)]
        public string areaRAO
        {
            get { return _areaRAO; }
            set { SetPropertyValue("areaRAO", ref _areaRAO, value); }
        }

        #endregion

        private string _u_bruta;
        public string u_bruta
        {
            get { return _u_bruta; }
            set { SetPropertyValue("u_bruta", ref _u_bruta, value); }
        }

        private string _v_netas;
        public string v_netas
        {
            get { return _v_netas; }
            set { SetPropertyValue("v_netas", ref _v_netas, value); }
        }

        private string _u_operativa;
        public string u_operativa
        {
            get { return _u_operativa; }
            set { SetPropertyValue("u_operativa", ref _u_operativa, value); }
        }

        private string _v_netas_;
        public string v_netas_
        {
            get { return _v_netas_; }
            set { SetPropertyValue("v_netas_", ref _v_netas_, value); }
        }

        private string _a_circ;
        public string a_circ
        {
            get { return _a_circ; }
            set { SetPropertyValue("a_circ", ref _a_circ, value); }
        }

        private string _pas_circ;
        public string pas_circ
        {
            get { return _pas_circ; }
            set { SetPropertyValue("pas_circ", ref _pas_circ, value); }
        }

        private string _a_circInv;
        public string a_circInv
        {
            get { return _a_circInv; }
            set { SetPropertyValue("a_circInv", ref _a_circInv, value); }
        }

        private string _pas_circInv;
        public string pas_circInv
        {
            get { return _pas_circInv; }
            set { SetPropertyValue("pas_circInv", ref _pas_circInv, value); }
        }

        private string _inventario_cv;
        public string inventario_cv
        {
            get { return _inventario_cv; }
            set { SetPropertyValue("inventario_cv", ref _inventario_cv, value); }
        }

        private string _dias_periodo;
        public string dias_periodo
        {
            get { return _dias_periodo; }
            set { SetPropertyValue("dias_periodo", ref _dias_periodo, value); }
        }

        private string _cxc;
        public string cxc
        {
            get { return _cxc; }
            set { SetPropertyValue("cxc", ref _cxc, value); }
        }

        private string _cxc_dp;
        public string cxc_dp
        {
            get { return _cxc_dp; }
            set { SetPropertyValue("cxc_dp", ref _cxc_dp, value); }
        }

        private string _cxp;
        public string cxp
        {
            get { return _cxp; }
            set { SetPropertyValue("cxp", ref _cxp, value); }
        }

        private string _cxp_dp;
        public string cxp_dp
        {
            get { return _cxp_dp; }
            set { SetPropertyValue("cxp_dp", ref _cxp_dp, value); }
        }

        private string _pasivo_tot;
        public string pasivo_tot
        {
            get { return _pasivo_tot; }
            set { SetPropertyValue("pasivo_tot", ref _pasivo_tot, value); }
        }

        private string _activo_tot;
        public string activo_tot
        {
            get { return _activo_tot; }
            set { SetPropertyValue("activo_tot", ref _activo_tot, value); }
        }

        private string _gast_oper;
        public string gast_oper
        {
            get { return _gast_oper; }
            set { SetPropertyValue("gast_oper", ref _gast_oper, value); }
        }

        private string _vts_netas;
        public string vts_netas
        {
            get { return _vts_netas; }
            set { SetPropertyValue("vts_netas", ref _vts_netas, value); }
        }

        private string _uti_oper;
        public string uti_oper
        {
            get { return _uti_oper; }
            set { SetPropertyValue("uti_oper", ref _uti_oper, value); }
        }

        private string _actTot_pasTot;
        public string actTot_pasTot
        {
            get { return _actTot_pasTot; }
            set { SetPropertyValue("actTot_pasTot", ref _actTot_pasTot, value); }
        }

        private string _respuestaMUB;
        [XafDisplayName("Margen de utilidad bruta")]
        [Size(SizeAttribute.Unlimited)]
        public string respuestaMUB
        {
            get { return _respuestaMUB; }
            set { SetPropertyValue("respuestaMUB", ref _respuestaMUB, value); }
        }

        
        private string _respuestaMUO;
        [XafDisplayName("Margen de utilidad operativa")]
        [Size(SizeAttribute.Unlimited)]
        public string respuestaMUO
        {
            get { return _respuestaMUO; }
            set { SetPropertyValue("respuestaMUO", ref _respuestaMUO, value); }
        }

        
        private string _respuestaLRC;
        [XafDisplayName("Liquidez (razon circulante)")]
        [Size(SizeAttribute.Unlimited)]
        public string respuestaLRC
        {
            get { return _respuestaLRC; }
            set { SetPropertyValue("respuestaLRC", ref _respuestaLRC, value); }
        }

        
        private string _respuestaLRR;
        [XafDisplayName("Liquidez (razon rapida)")]
        [Size(SizeAttribute.Unlimited)]
        public string respuestaLRR
        {
            get { return _respuestaLRR; }
            set { SetPropertyValue("respuestaLRR", ref _respuestaLRR, value); }
        }

        
        private string _respuestaRI;
        [XafDisplayName("Rotacion de inventario)")]
        [Size(SizeAttribute.Unlimited)]
        public string respuestaRI
        {
            get { return _respuestaRI; }
            set { SetPropertyValue("respuestaRI", ref _respuestaRI, value); }
        }

        
        private string _respuestaPPC;
        [XafDisplayName("Periodo promedio de cobro")]
        [Size(SizeAttribute.Unlimited)]
        public string respuestaPPC
        {
            get { return _respuestaPPC; }
            set { SetPropertyValue("respuestaPPC", ref _respuestaPPC, value); }
        }

        
        private string _respuestaPPP;
        [XafDisplayName("Periodo promedio de pago")]
        [Size(SizeAttribute.Unlimited)]
        public string respuestaPPP
        {
            get { return _respuestaPPP; }
            set { SetPropertyValue("respuestaPPP", ref _respuestaPPP, value); }
        }

        
        private string _respuestaE;
        [XafDisplayName("Endeudamiento")]
        [Size(SizeAttribute.Unlimited)]
        public string respuestaE
        {
            get { return _respuestaE; }
            set { SetPropertyValue("respuestaE", ref _respuestaE, value); }
        }

        
        private string _respuestaGO;
        [XafDisplayName("Gastos operativos")]
        [Size(SizeAttribute.Unlimited)]
        public string respuestaGO
        {
            get { return _respuestaGO; }
            set { SetPropertyValue("respuestaGO", ref _respuestaGO, value); }
        }

       
        private string _respuestaRAO;
        [XafDisplayName("Rendimiento sobre activos operativos")]
        [Size(SizeAttribute.Unlimited)]
        public string respuestaRAO
        {
            get { return _respuestaRAO; }
            set { SetPropertyValue("respuestaRAO", ref _respuestaRAO, value); }
        }

        private string _urlArchivo;
        [Size(SizeAttribute.Unlimited)]
        public string urlArchivo
        {
            get { return _urlArchivo; }
            set { SetPropertyValue("urlArchivo", ref _urlArchivo, value); }
        }


        private string _nombreArchivo;
        [Size(SizeAttribute.Unlimited)]
        public string nombreArchivo
        {
            get { return _nombreArchivo; }
            set { SetPropertyValue("nombreArchivo", ref _nombreArchivo, value); }
        }

        private NuevaAuditoria idAuditoria;
        [Association("NuevaAuditoria-TablaFinancieraResultados")]
        public NuevaAuditoria IdAuditoria
        {
            get { return idAuditoria; }
            set { SetPropertyValue("IdAuditoria", ref idAuditoria, value); }
        }

        public string realizo { get; set; }

        [DisplayName("Distribuidor de Auditoria")]
        public string distribuidor
        {
            get
            {
                if (idAuditoria != null)
                {
                    return idAuditoria.Distribuidor.nombredist;
                }
                else if(distribuidorSel != null)
                {
                    return distribuidorSel;
                }
                else
                {
                    return "";
                }
            }
        }

        private string _distribuidorSel;
        [DisplayName("Distribuidor")]
        public string distribuidorSel
        {
            get { return _distribuidorSel; }
            set { SetPropertyValue("distribuidorSel", ref _distribuidorSel, value); }
        }


        //[Action(Caption ="Llena fecha")]

        //public void llenafecha()
        //{
        //    DevExpress.Xpo.Session session = this.Session;
        //    var tablas = new XPQuery<TablaFinancieraResultados>(session).Where(x => x.fechaRealizada == null && x.IdAuditoria != null).ToList();

        //    foreach(var item in tablas)
        //    {
        //        item.fechaRealizada = item.IdAuditoria.Fecha;
        //        item.Save();
        //    }
        //}
    }
}
