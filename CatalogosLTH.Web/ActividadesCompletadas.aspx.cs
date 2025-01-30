using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogosLTH.Web
{
    public partial class ActividadesCompletadas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DevExpress.Xpo.Session session = Util.getsession();
                var distribuidores = new XPQuery<mdl_distribuidor>(session);
                XPCollection<mdl_distribuidor> dist = new XPCollection<mdl_distribuidor>(session);

                SortingCollection sortCollection = new SortingCollection();
                sortCollection.Add(new SortProperty("nombredist", SortingDirection.Ascending));
                dist.Sorting = sortCollection;

                string nombreActual = Util.getusuario();
                string permiso = Util.getPermiso().ToString();
                string IdAuditoria = "";
                List<Usuario> listaDistribuidores = new List<Usuario>();
                List<mdl_distribuidor> listaDistribuidoresDist = new List<mdl_distribuidor>();
                if (permiso == "GerenteCuenta")
                {
                    listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.Jefe.UserName == nombreActual).ToList();
                }
                else if (permiso == "GerenteDesarrolloComercial")
                {
                    listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.Jefe.Jefe.UserName == nombreActual).ToList();
                }
                else if (permiso == "Admin")
                {
                    listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.TipoUsuario == TipoUsuario.Distribuidor).ToList();
                    /*Usuario todos = new Usuario(session);//SE CREA USUARIO PARA PODER ELEGIR TODOS LOS DISTRIBUIDORES
                    todos.Nombre = "Todos";
                    todos.UserName = "Todos";
                    listaDistribuidores.Add(todos);*/
                }
                else if (permiso == "Distribuidor")
                {
                    listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.UserName == nombreActual).ToList();
                    dropDist.Enabled = false;
                }
                else if (permiso == "Evaluador")
                {
                    var audacts = new XPQuery<mdl_auditoriaactividad>(session).Where(x => x.Evaluador.UserName == nombreActual);
                    var bx = audacts.Where(xx =>xx.Idaud == null||xx.Idaud.idaud==0);
                    listaDistribuidoresDist = new XPQuery<mdl_distribuidor>(session).ToList();

                    var dataDist = (from au in audacts
                                 select new { au.Idaud.Iddistribuidor.nombredist, au.Idaud.idaud}).Distinct().ToList();

//                    var dat = from aa in audacts
  //                            join ld in listaDistribuidoresDist on aa.Idaud.Iddistribuidor equals ld;

                    int ds = dataDist.Count();
                    dataDist = dataDist.OrderBy(x => x.nombredist).ToList();
                    foreach (var item in dataDist)
                    {
                        if (item.nombredist!=""&&item.nombredist!=null)
                        {
                            dropDist.Items.Add(item.nombredist);
                        }                        
                    }
                   // dropDist.Items.Add("Todos");
                }
                else if (permiso == "GerenteVenta")
                {
                    Usuario u = new XPQuery<Usuario>(session).FirstOrDefault(x => x.UserName == nombreActual);
                    string z = u.ZonaPertenece.zona;
                    listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.ZonaPertenece.zona == z && x.TipoUsuario == TipoUsuario.Distribuidor).ToList();
                }
                if (listaDistribuidores!=null)
                {
                    listaDistribuidores = listaDistribuidores.OrderBy(x => x.UserName).ToList();
                    foreach (var itemDist in listaDistribuidores)
                    {
                        dropDist.Items.Add(itemDist.UserName.ToString());
                    }
                }
                if (permiso == "Admin" || permiso == "Evaluador") { dropDist.Items.Add("Todos"); }
                dropDist.SelectedIndex = 0;
               
                //SE SELECCIONA DISTRIBUIDOR EN EL DROPDOWN
                ASPxGridViewExporter1.GridViewID = "ASPxGridView1";
                
            }

            llenaGrid();
        }

        protected void dropDist_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenaGrid();
        }
        public void llenaGrid()
        {
            DevExpress.Xpo.Session session = Util.getsession();            
            int iddist = dropDist.SelectedIndex;

            string permiso = Util.getPermiso().ToString();
            string nombreActual = Util.getusuario();


            if (dropDist != null && dropDist.Text != "")
            {
                if (dropDist.Text!="Todos")
                {
                    mdl_distribuidor dist = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist.ToString() == dropDist.SelectedItem.ToString());
                    iddist = dist.iddistribuidor;
                }
                else
                {
                    iddist = 0;
                }
                
            }
            int auditoriasRevisa = Util.getUltimaAuditoria(iddist);

            var audact = new XPQuery<mdl_auditoriaactividad>(session).Where(x => x.fechacomp != null && x.Idaud.idaud==auditoriasRevisa).ToList();

            List<mdl_auditoriaactividad> listaAudact = audact;
            

            if (iddist == 0)//todos los distribuidores
            {
                if (permiso=="Evaluador")
                {//corregir
                    var listaAudActs = new XPQuery<mdl_auditoriaactividad>(session).Where(x => x.fechacomp != null);//TODAS AUDACTS COMPLETADAS
                    var audacts = new XPQuery<mdl_auditoriaactividad>(session).Where(x => x.Evaluador.UserName == nombreActual);
                    List<mdl_auditoria> ultimasAuitorias = new List<mdl_auditoria>();
                    var dataDist = (from au in audacts
                                    select new {
                                        au.Idaud.Iddistribuidor
                                    }).Distinct().ToList();

                    foreach (var item in dataDist)
                    {
                        if (item.Iddistribuidor!=null)
                        {
                            ultimasAuitorias.Add(item.Iddistribuidor.UltimaAuditoria);
                        }                        
                    }

                    var data1 = from au in ultimasAuitorias
                                join aa in listaAudActs on au equals aa.Idaud
                                where aa.Evaluador!=null && aa.Evaluador.UserName==nombreActual
                                orderby aa.Idactividad ascending
                                select new { aa.idactplan,
                                    IdActividad = aa.Idactividad.Code,
                                    Actividad = aa.Idactividad.Texto,
                                    Inicio = aa.fechainicio,
                                    Final = aa.fechafinal,
                                    Completada = aa.fechacomp,
                                    Nivel = aa.Idactividad.NivelID.nombreniv,
                                    Distribuidor = aa.Idaud.Iddistribuidor.nombredist,
                                    Pilar = aa.Idactividad.PilarID.nombrepil,
                                    Evaluador = aa.Evaluador.Nombre,
                                    Mail=aa.Evaluador.email
                                };
                    ASPxGridView1.DataSource = data1.ToList();
                }
                else//SI ES ADMINISTRADOR
                {
                    //var listaAudActs = new XPQuery<mdl_auditoriaactividad>(session).Where(x => x.fechacomp != DateTime.MinValue);
                    var listaAudActs = new XPQuery<mdl_auditoriaactividad>(session).Where(x => x.fechacomp != null);
                    var listaDist = new XPQuery<mdl_distribuidor>(session);
                    List<mdl_auditoria> ultimasAuitorias = new List<mdl_auditoria>();
                    foreach (var item in listaDist)
                    {
                        ultimasAuitorias.Add(item.UltimaAuditoria);
                    }
                    var data1 = from au in ultimasAuitorias
                                join aa in listaAudActs on au equals aa.Idaud
                                orderby aa.Idactividad ascending
                                select new { aa.idactplan,
                                    IdActividad = aa.Idactividad.Code,
                                    Actividad = aa.Idactividad.Texto,
                                    Inicio = aa.fechainicio,
                                    Final = aa.fechafinal,
                                    Completada = aa.fechacomp,
                                    Nivel = aa.Idactividad.NivelID.nombreniv,
                                    Distribuidor = aa.Idaud.Iddistribuidor.nombredist,
                                    Pilar = aa.Idactividad.PilarID.nombrepil,
                                    Evaluador =(aa.Evaluador!= null)? aa.Evaluador.Nombre:"Sin Asignar",
                                    Mail= (aa.Evaluador != null) ? aa.Evaluador.email : "Sin Asignar"
                                };
                    ASPxGridView1.DataSource = data1.ToList();
                }
               
            }
            else
            {
                if (permiso == "Evaluador")
                {
                    var data = from ba in listaAudact
                               where ba.Evaluador.UserName == nombreActual
                               orderby ba.Idactividad ascending
                               select new { ba.idactplan,
                                   IdActividad = ba.Idactividad.Code,
                                   Actividad = ba.Idactividad.Texto,
                                   Inicio = ba.fechainicio,
                                   Final = ba.fechafinal,
                                   Completada = ba.fechacomp,
                                   Nivel = ba.Idactividad.NivelID.nombreniv,
                                   Distribuidor = ba.Idaud.Iddistribuidor.nombredist,
                                   Pilar =ba.Idactividad.PilarID.nombrepil,
                                   Evaluador = (ba.Evaluador != null) ? ba.Evaluador.Nombre : "Sin Asignar",
                                   Mail = (ba.Evaluador != null) ? ba.Evaluador.email : "Sin Asignar"
                               };
                    ASPxGridView1.DataSource = data.ToList();
                }
                else
                {
                    var data = from ba in listaAudact
                               orderby ba.Idactividad ascending
                               select new
                               {
                                   ba.idactplan,
                                   IdActividad = ba.Idactividad.Code,
                                   Actividad = ba.Idactividad.Texto,
                                   Inicio = ba.fechainicio,
                                   Final = ba.fechafinal,
                                   Completada = ba.fechacomp,
                                   Nivel = ba.Idactividad.NivelID.nombreniv,
                                   Distribuidor = ba.Idaud.Iddistribuidor.nombredist,
                                   Pilar = ba.Idactividad.PilarID.nombrepil,
                                   Evaluador = (ba.Evaluador != null) ? ba.Evaluador.Nombre : "Sin Asignar",
                                   Mail = (ba.Evaluador != null) ? ba.Evaluador.email : "Sin Asignar"
                               };
                                   
                    ASPxGridView1.DataSource = data.ToList();
                }
            }
                        
            ASPxGridView1.AutoGenerateColumns = true;
            ASPxGridView1.KeyFieldName = "idactplan";
            ASPxGridView1.DataBind();

            foreach (GridViewDataColumn columna in ASPxGridView1.Columns)
            {
                string col2 = columna.FieldName.ToString();
                if (col2 == "idactplan")
                {
                    columna.Visible = false;
                }

            }
        }

        protected void ASPxButton1_Click(object sender, EventArgs e)//boton seleccionar
        {
            int indice = ASPxGridView1.FocusedRowIndex;
            string codigo = ASPxGridView1.GetRowValues(indice, new string[] { "idactplan" }).ToString();
            Response.Redirect("~/SeleccionActividad.aspx?codigo=" + Cryptography.Encriptar(codigo));
        }

        protected void Excel(object sender, ImageClickEventArgs e)
        {
            
        }

        protected void ASPxButton2_Click(object sender, EventArgs e)
        {
            ASPxGridViewExporter1.WriteXlsxToResponse(new XlsxExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }
    }
    }
