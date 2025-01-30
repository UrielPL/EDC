using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using System.IO;

namespace CatalogosLTH.Web
{
    public partial class RevisionAuditoriaMovil : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            DevExpress.Xpo.Session session = Util.getsession();
            var codigo = Request.QueryString["codigo"];
            if (codigo != null)
            {
                codigo = Cryptography.Decripta(codigo.ToString());
            }
            if (codigo != null)
            {
                mdl_AuditoriaMovil aud = new XPQuery<mdl_AuditoriaMovil>(session).FirstOrDefault(x => x.IdAuditoriaMovil.ToString() == codigo.ToString());
                lblArea.Text = (aud.punto!=null)?aud.punto.Areas.FirstOrDefault().Nombre:"NA";
                lblDist.Text = (aud.distribuidor!=null)?aud.distribuidor.nombredist:"NA";
                lblEvaluador.Text = (aud.evaluador!=null)?aud.evaluador.Nombre:"NA";
                lblPunto.Text = (aud.punto != null) ? aud.punto.texto:"NA";
                lblTipo.Text = (aud.punto != null) ? aud.punto.Idtipoaud.Descripcion:"NA";
                lblArchivo.Text = (aud.ArchivoImportar != null) ? aud.ArchivoImportar.FileName:"NA";
                lblComentario.Text = aud.comentario;
                lblCS.Text = aud.centroServicio;
                lblValidadoPor.Text = (aud.AuditoriaGeneral.ValidadoPor) != null ? aud.AuditoriaGeneral.ValidadoPor : "NA";
                lblFecha.Text = aud.fecha.ToLongDateString() + " a las: " + aud.fecha.ToShortTimeString();
                if (aud.status)
                    lblCumplimiento.Text = "No aceptada por evaluador";
                else
                    lblCumplimiento.Text = "Aceptada por evaluador";

                if(aud.cerrada)
                {
                    btnAceptar.Enabled = false;
                    btnIgnorar.Enabled = false;
                    btnRechazar.Enabled = false;
                    btnAceptar.Text = "Auditoria cerrada";
                    btnRegresar.Visible = true;
                    checkCerrada.Enabled = false;
                }
                ViewState["oidAud"] = aud.IdAuditoriaMovil.ToString();
            }
        }

        protected void btnFile_Click(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();//consigue la sesion            
            XpoDefault.DataLayer = session.DataLayer;
            string idAud = ViewState["oidAud"] as string;
            mdl_AuditoriaMovil aud = new XPQuery<mdl_AuditoriaMovil>(session).FirstOrDefault(x => x.IdAuditoriaMovil.ToString() == idAud);

            FileData oArchivo = aud.ArchivoImportar;
            if (oArchivo != null)
            {
                MemoryStream ms = new MemoryStream();
                oArchivo.SaveToStream(ms);
                string sNombre = oArchivo.FileName;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + sNombre.Replace(" ", ""));
                Response.BinaryWrite(ms.ToArray());
                Response.Flush();
                Response.End();
            }
            else
            {
                btnFile.Text = "No hay archivo disponible";
                btnFile.Enabled = false;
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            botonGuardar();
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ListaAuditoriasMoviles.aspx");
        }

        private void botonGuardar()
        {

            DevExpress.Xpo.Session session = Util.getsession();//consigue la sesion            
            XpoDefault.DataLayer = session.DataLayer;
            string idAud = ViewState["oidAud"] as string;
            mdl_AuditoriaMovil aud = new XPQuery<mdl_AuditoriaMovil>(session).FirstOrDefault(x => x.IdAuditoriaMovil.ToString() == idAud);
            mdl_audidet auddet = new XPQuery<mdl_audidet>(session).FirstOrDefault(x => x.Idaud == aud.auditoria && x.Idpunto == aud.punto);
            if (auddet.resultado > 0)//SI FUE ACEPTADA EN AUDITORIA
            {
                if (!checkCerrada.Checked)//SI SE ESTA ANULANDO LA ACTIVIDAD
                {
                ///    using (UnitOfWork uow = new UnitOfWork())
                   // {
                        auddet.resultado = 0;
                       // auddet.Save();
                        foreach (mdl_actividad ac in auddet.Idpunto.Actividades)
                        {
                            //mdl_auditoriaactividad aa = new mdl_auditoriaactividad(session);
                            //aa.Idaud = auddet.Idaud;
                            //aa.Idactividad = ac;
                            //aa.secuencia = ac.Secuencia;
                            //aa.fechainicio = DateTime.Now;
                            //aa.duracion = ac.Duracion;
                            //aa.fechafinal = DateTime.Now.AddDays(ac.Duracion * 7);
                        //    aa.Save();                            
                        }
                      //  uow.CommitChanges();
                   // }
                  //  Util.ActualizarNivelProfesionalizacion(aud.distribuidor.iddistribuidor);
                   // Util.actualizaRegistroMensual(aud.distribuidor.nombredist, null);
                    //nivel n = new nivel();
                    //n.ActualizarEncargados(auddet.Idaud.idaud);
                }

            }
            else//SI NO FUE ACEPTADA EN AUDITORIA
            {
                List<mdl_auditoriaactividad> audact = new List<mdl_auditoriaactividad>();
                foreach (var item in aud.listaActividades)
                {
                    mdl_auditoriaactividad audactx = new XPQuery<mdl_auditoriaactividad>(session)
                        .FirstOrDefault(x => x.Idaud == aud.auditoria
                            && x.Idactividad.IdActividad == item.IdActividad);
                    audact.Add(audactx);
                }

                foreach (var item in audact)
                {
                    if (item.fechacomp != DateTime.MinValue)//SE COMPLETO EN DC
                    {
                        //if (checkCerrada.Checked && !aud.status)//EVALUADOR LA CANCELA Y ADMIN AUTORIZA
                        //{
                        //    using (UnitOfWork uow = new UnitOfWork())
                        //    {
                        //        item.fechacomp = DateTime.MinValue;
                        //        item.status = "Por Realizar";
                        //        item.Save();
                                
                        //        uow.CommitChanges();
                        //        Util.ActualizarNivelProfesionalizacion(aud.distribuidor.iddistribuidor);
                        //        Util.actualizaRegistroMensual(aud.distribuidor.nombredist, item);
                        //    }
                        //}
                    }
                    else if (item.fechacomp == DateTime.MinValue)//NO SE HA COMPLETADO EN DC
                    {
                        //if (checkCerrada.Checked && aud.status)//EVALUADOR AUTORIZA Y ADMIN AUTORIZA
                        //{
                            using (UnitOfWork uow = new UnitOfWork())
                            {
                                item.fechacomp = DateTime.Now;
                                item.status = "Completada";
                                item.Save();

                              uow.CommitChanges();
                                Util.ActualizarNivelProfesionalizacion(aud.distribuidor.iddistribuidor);
                                Util.actualizaRegistroMensual(aud.distribuidor.nombredist, item);
                            }
                        //}
                    }
                }              
               
            }

            using (UnitOfWork uow = new UnitOfWork())
            {
               // aud.aceptada = checkCerrada.Checked;
                aud.cerrada = true;
                aud.resultado = resultadov.Cumple.ToString();
                aud.Save();
                uow.CommitChanges();
            }
            btnAceptar.Enabled = false;
            btnIgnorar.Enabled = false;
            btnRechazar.Enabled = false;
            btnAceptar.Text = "Auditoria cerrada";
            btnRegresar.Visible = true;
            checkCerrada.Enabled = false;
        }

        protected void btnIgnorar_Click(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();//consigue la sesion            
            XpoDefault.DataLayer = session.DataLayer;
            string idAud = ViewState["oidAud"] as string;
            mdl_AuditoriaMovil aud = new XPQuery<mdl_AuditoriaMovil>(session).FirstOrDefault(x => x.IdAuditoriaMovil.ToString() == idAud);
            mdl_audidet auddet = new XPQuery<mdl_audidet>(session).FirstOrDefault(x => x.Idaud == aud.auditoria && x.Idpunto == aud.punto);
            using (UnitOfWork uow = new UnitOfWork())
            {
             //   aud.aceptada = checkCerrada.Checked;
                aud.cerrada = true;
                aud.resultado=resultadov.Ignorado.ToString();
                aud.Save();
                uow.CommitChanges();  
            }

            btnAceptar.Enabled = false;
            btnIgnorar.Enabled = false;
            btnRechazar.Enabled = false;
                
            btnAceptar.Text = "Auditoria cerrada";
            btnRegresar.Visible = true;
            checkCerrada.Enabled = false;
        }

        protected void btnRechazar_Click(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();//consigue la sesion            
            XpoDefault.DataLayer = session.DataLayer;
            string idAud = ViewState["oidAud"] as string;
            mdl_AuditoriaMovil aud = new XPQuery<mdl_AuditoriaMovil>(session).FirstOrDefault(x => x.IdAuditoriaMovil.ToString() == idAud);
            mdl_audidet auddet = new XPQuery<mdl_audidet>(session).FirstOrDefault(x => x.Idaud == aud.auditoria && x.Idpunto == aud.punto);

            if (auddet.resultado > 0)//SI FUE ACEPTADA EN AUDITORIA
            {
               
                    using (UnitOfWork uow = new UnitOfWork())
                    {
                        auddet.resultado = 0;
                        auddet.Save();
                        foreach (mdl_actividad ac in auddet.Idpunto.Actividades)
                        {
                            mdl_auditoriaactividad aa = new mdl_auditoriaactividad(session);
                            aa.Idaud = auddet.Idaud;
                            aa.Idactividad = ac;
                            aa.secuencia = ac.Secuencia;
                            aa.fechainicio = DateTime.Now;
                            aa.duracion = ac.Duracion;
                            aa.fechafinal = DateTime.Now.AddDays(ac.Duracion * 7);
                            aa.Save();
                        }
                        uow.CommitChanges();
                    }
                    Util.ActualizarNivelProfesionalizacion(aud.distribuidor.iddistribuidor);
                    Util.actualizaRegistroMensual(aud.distribuidor.nombredist, null);
                    nivel n = new nivel();
                    n.ActualizarEncargados(auddet.Idaud.idaud,auddet.Idaud.Iddistribuidor.iddistribuidor);
            }
            else
            {
                List<mdl_auditoriaactividad> audact = new List<mdl_auditoriaactividad>();
                foreach (var item in aud.listaActividades)
                {
                    mdl_auditoriaactividad audactx = new XPQuery<mdl_auditoriaactividad>(session).FirstOrDefault(x => x.Idaud == aud.auditoria && x.Idactividad.IdActividad == item.IdActividad);
                    audact.Add(audactx);
                }

                foreach (var item in audact)
                {
                    using (UnitOfWork uow = new UnitOfWork())
                    {
                        item.fechacomp = DateTime.MinValue;
                        item.status = "Por Realizar";
                        item.Save();

                        uow.CommitChanges();
                        Util.ActualizarNivelProfesionalizacion(aud.distribuidor.iddistribuidor);
                        Util.actualizaRegistroMensual(aud.distribuidor.nombredist, item);
                    }
                }
            }

            using (UnitOfWork uow = new UnitOfWork())
            {
               // aud.aceptada = checkCerrada.Checked;
                aud.cerrada = true;
                aud.resultado =resultadov.NoCumple.ToString();
                    aud.Save();
                  uow.CommitChanges();
            }

            btnAceptar.Enabled = false;
            btnIgnorar.Enabled = false;
            btnRechazar.Enabled = false;

            btnAceptar.Text = "Auditoria cerrada";
            btnRegresar.Visible = true;
            checkCerrada.Enabled = false;
        }//btnrechazar
    }
}