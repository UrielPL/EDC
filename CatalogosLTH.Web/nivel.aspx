<%@ Page Title="" Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="nivel.aspx.cs" Inherits="CatalogosLTH.Web.nivel" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link rel="stylesheet" href="css/slicknav.css" />
      <script src="//ajax.googleapis.com/ajax/libs/jquery/2.1.3/jquery.min.js"></script>
      <script src="js/jquery.bxslider/jquery.bxslider.min.js"></script>
      <script src="js/jquery.owl-carousel/owl.carousel.js"></script>
      <script src="js/jquery.fancybox/jquery.fancybox.pack.js"></script>
      <script src="js/master.js"></script>
      <script src="js/jquery.slicknav.js"></script>
      <script src="http://cdnjs.cloudflare.com/ajax/libs/modernizr/2.6.2/modernizr.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
            <%@ Import Namespace="DevExpress.Xpo" %>
            <%@ Import Namespace="CatalogosLTH.Module.BusinessObjects" %>
            <%@ Import Namespace="CatalogosLTH.Web" %>

    	<div class="section  banner-nivel remove-top remove-bottom">
		<div class="container">
			<div class="row">
				<div class="twelve columns">
					<h1>Consulta nivel</h1>
				</div>
			</div>
		</div>
	</div>	
    <div class="container">
    <asp:Label ID="lblText" runat="server" />

    <div class="panel-heading">
        Nivel Profesionalización
   <%
        string niveltotal = "-";
       //lblNivel.InnerText = niveltotal;
        %>
        <asp:Label ID="lblNivel" runat="server" />
        
       <%:niveltotal%>
    </div>
    <div class="col-sm-12">
        <div class="row">
            <dx:ASPxComboBox ID="dropDist" OnSelectedIndexChanged="dropDist_SelectedIndexChanged" runat="server" ValueType="System.String"  xmlns:dx="devexpress.web" autopostback="True"></dx:ASPxComboBox>
        </div>
    </div>

    <table class="table table-striped">
        <thead>
            <tr>
                <th style="text-align: center;">Nivel</th>
                <th style="text-align: center;">Pond x Nivel</th>
                <th style="text-align: center;">Profes</th>
                <th style="text-align: center;">Pilar</th>
                <th style="text-align: center;">Pond x Pilar</th>
                <th style="text-align: center;">Total</th>
                <th style="text-align: center;">Completadas en Auditoría</th>
                <th style="text-align: center;">Completadas por DC</th>
                <th style="text-align: center;">Activas</th>
                <th style="text-align: center;">Avance Auditoría</th>
                <th style="text-align: center;">Avance DC</th>
                <%--<th style="text-align: center;">Avance Total</th>--%>
            </tr>
        </thead>
        <tbody>
          
            <%  
                bool cerrada = false;
                if (ViewState["IdAuditoria"]==null)
                {
                    Response.Redirect("Listaactividades.aspx");
                }
                string IdAuditoria =ViewState["IdAuditoria"].ToString() ;
                int idaudi = int.Parse(IdAuditoria);
                //int.Parse( Page.Request["newidaudi"]);
                int tipoaud = 0;
                string nombredist = "";
                DevExpress.Xpo.Session session = Util.getsession();

                XPQuery<mdl_auditoria> auditorias = session.Query<mdl_auditoria>();
                XPQuery<mdl_distribuidor> distribuidores = session.Query<mdl_distribuidor>();
                XPQuery<mdl_pondnivel> ponderacionesNivel = session.Query<mdl_pondnivel>();
                XPQuery<mdl_nivel> niveles = session.Query<mdl_nivel>();
                XPQuery<mdl_catnivel> catNiveles = session.Query<mdl_catnivel>();
                XPQuery<vwtotal> totales = session.Query<vwtotal>();
                XPQuery<vwpuntostot> puntostotales = session.Query<vwpuntostot>();
                XPQuery<mdl_auditoriaactividad> auditoriaActividad = session.Query<mdl_auditoriaactividad>();

                var sqlw = from a in auditorias
                           join d in distribuidores on a.Iddistribuidor equals d
                           where a.idaud == idaudi
                           select new { a.idaud, a.Idtipoaud.idTipoAuditoria, d.nombredist };
                foreach (var item in sqlw)
                {
                    tipoaud = item.idTipoAuditoria;
                    nombredist = item.nombredist;
                    cerrada = true;
                }


                Session["profes"] = 0;
                double idtotalv = 0;
                
                double avancea = 0;
                double totalp = 0;
                double totalc = 0;

                double totaldc = 0;
                double avanceTotalT = 0;
                double avanceTotalDC = 0;
                double avanceTotalFinalDC = 0;//completadas por dc linea roja final
                double totalProfesionalizacion = 0;

                double activas = 0;
                double totalpt = 0;
                double totalct = 0;
                double activast = 0;
                double avanceat = 0;
                double comp = 0;
                string tipaudletra = "";


                var sql = from pn in ponderacionesNivel
                          where pn.Idaud.idaud == idaudi
                          select pn;

                int row_cnt = sql.Count();

                if (tipoaud == 1)
                {
                    tipaudletra = "A";
                }
                else {
                    tipaudletra = "B";
                }

                var sql3 = from ni in niveles
                           join ctn in catNiveles on ni equals ctn.Idnivel
                           where ctn.Idtipoaud.idTipoAuditoria == tipoaud
                           select new { ni.idnivel, ni.nombreniv, ctn.ponderacion, ctn.Idtipoaud };


                foreach (var rec3 in sql3)
                { %>
                        <tr>
                            <td rowspan="6"><%:rec3.nombreniv %></td>
                            <td rowspan="6" style="text-align: center;"><%:rec3.ponderacion %>%</td>
                            <td rowspan="6" style="text-align: center;"></td>
                        </tr>
                        <%
                            var sql2 = from vt in totales
                                       where vt.idnivel == rec3.idnivel
                                       && vt.idtipoaud == tipoaud
                                       select vt;
                            foreach (var record2 in sql2)
                            {
                                totalp += record2.total;

                                var sqln = from vpt in puntostotales
                                           where vpt.idaud==idaudi &&
                                           vpt.idpilar== record2.idpilar &&
                                           vpt.idnivel==record2.idnivel
                                           select vpt;
                                idtotalv = 0;
                                foreach (var item in sqln)
                                {
                                    idtotalv ++;//ACT A COMPLETAR EN DC
                                }
                    
                        %>
                        <tr>
                            <td><%:record2.nombrepil %></td>
                            <td style="text-align: center;"><%:record2.pondercionPilar %></td>
                            <td style="text-align: center;"><%:record2.total %></td>

                            <td style="text-align: center;">
                                <%
                                    double completadasAuditoria = 0;
                                    if (record2.total == 0)
                                    {
                                %>
                                <%:"0" %>
                                <%
                                    }
                                    else
                                    {
                                        completadasAuditoria = (record2.total - idtotalv);
                                %>
                                <%:completadasAuditoria %>
                                <%
                                         totalc = record2.total - idtotalv + totalc;
                                    }
                                %>
                            </td>

                            <td style="text-align: center;">
                                <%//Calculo de las actividades ya realizadas

                                    var sqlAudAct = from audact in auditoriaActividad
                                                    where audact.Idaud.idaud.ToString() == IdAuditoria && audact.fechacomp!=null  && audact.Idactividad.NivelID.nombreniv==record2.nombreniv && audact.Idactividad.PilarID.nombrepil==record2.nombrepil
                                                    select audact;

                                    int actividadesRealizadasDC = sqlAudAct.Count();
                                    totaldc += actividadesRealizadasDC;
                                     %>
                                <%:actividadesRealizadasDC %>
                                </td>
                             <td style="text-align: center;">
                                <%
                            double actividadesTotalesPorRealizarDeLaAuditoria = idtotalv;
                            if (record2.total == 0)
                            {
                                comp = 0;
                                            %>
                                            <%:0 %>
                                            <%
                            }
                            else
                            {
                                comp = idtotalv;
                                activas = comp + activas;
                           
                                double actividadesActivas = actividadesTotalesPorRealizarDeLaAuditoria - actividadesRealizadasDC;

                                %>
                                <%:actividadesActivas  %>
                                <%
                             }
                                %>
                            </td>
                            <%
                                double avanceAuditoria = 0;
                                if (record2.total == 0)
                                {
                                    comp = record2.pondercionPilar;
                                    //avanceAuditoria = 100;
                                    //avanceAuditoria = 0;
                                    avanceAuditoria = comp;
                                }
                                else
                                {
                                    comp = (record2.total - idtotalv) / record2.total;
                                    comp = comp * record2.pondercionPilar;
                                    //avanceAuditoria = completadasAuditoria / record2.total * 100;
                                    avanceAuditoria = completadasAuditoria / record2.total * record2.pondercionPilar;

                                }
                                avancea = comp + avancea;

                                //double avanceDC = (actividadesTotalesPorRealizarDeLaAuditoria==0)?100:actividadesRealizadasDC / actividadesTotalesPorRealizarDeLaAuditoria * 100;
                                double avanceDC = (actividadesTotalesPorRealizarDeLaAuditoria==0)?0: Convert.ToDouble(actividadesRealizadasDC) / Convert.ToDouble(record2.total) * Convert.ToDouble(record2.pondercionPilar);
                                avanceTotalDC += avanceDC;
                                //double avanceTotal =(record2.total==0)?100: (completadasAuditoria + actividadesRealizadasDC) / record2.total*100;
                                double avanceTotal =(record2.total==0)?record2.pondercionPilar: (completadasAuditoria + actividadesRealizadasDC) / record2.total*record2.pondercionPilar;
                                avanceTotalT += avanceTotal;

                            %>
                            <td style="text-align: center;"><%:avanceAuditoria.ToString("0.00") %> %</td>
                            <td style="text-align: center;"><%:avanceDC.ToString("0.00") %> %</td>
                            <td style="text-align: center;"><%:avanceTotal.ToString("0.00") %> %</td>

                        </tr>
                        <%}//SE CIERRA FOREACH %>
                        <tr class="success">
                            <td></td>
                            <td></td>
                            <%
                                double profesionalizacionNivel = (rec3.ponderacion * avanceTotalT) / 100.0;
                                totalProfesionalizacion += profesionalizacionNivel;//TOTAL PROFESIONALIZACION LINEA ROJA
                                niveltotal = totalProfesionalizacion.ToString("0.00");

                                activas = totalp - (totalc + totaldc);

                            %>
                            <!--<td style="text-align: center; font-weight: bold;"><%//:((rec3.ponderacion*avancea)/100.0).ToString("0.00") %> %</td>-->
                            <td style="text-align: center; font-weight: bold;"><%:(profesionalizacionNivel).ToString("0.00") %> %</td>
                            
                            <td></td>
                            <td style="text-align: center;">100%</td>
                            <td style="text-align: center;"><%:totalp %></td><!--Total-->
                            <td style="text-align: center;"><%:totalc %></td><!--Completadas en Auditoria-->
                            <td style="text-align: center;"><%:totaldc %></td><!--Completadas por DC-->
                            <td style="text-align: center;"><%:activas %></td>
                            <td style="text-align: center;"><%:avancea.ToString("0.00") %>%</td>
                            <td style="text-align: center;"><%:avanceTotalDC.ToString("0.00")%>%</td>
                            <td style="text-align: center;"><%:avanceTotalT.ToString("0.00")%>%</td>
                           <!-- <td style="text-align: center;"><%//:avancea.ToString("0.00") %>%</td>-->
                        </tr>
                        <%
                                avanceTotalFinalDC += totaldc;
                                totalpt = totalp + totalpt;
                                totalct = totalct + totalc;
                                activast = activast + activas;
                                avanceat = ((rec3.ponderacion * avancea) / 100) + avanceat;
                                totalp = 0;
                                totalc = 0;
                                activas = 0;
                                avancea = 0;
                                totaldc = 0; //se restaura totaldc a 0
                                avanceTotalT = 0;
                                avanceTotalDC = 0;


                            }//PRIMER FOREACH
            %>


             <%
                    string nivelP = "";
                    if (totalProfesionalizacion >=0&&totalProfesionalizacion<=44)
                    {
                        nivelP = "BÁSICO";
                    }
                    else if (totalProfesionalizacion>44&&totalProfesionalizacion<=65)
                    {
                        nivelP = "TRANSITORIO";
                    }
                    else if (totalProfesionalizacion>65&&totalProfesionalizacion<=80)
                    {
                        nivelP = "INTERMEDIO";
                    }
                    else if (totalProfesionalizacion>80&&totalProfesionalizacion<=94)
                    {
                        nivelP = "AVANZADO";
                    }
                    else if (totalProfesionalizacion>94&&totalProfesionalizacion<=100)
                    {
                        nivelP = "SOBRESALIENTE";
                    }
                    double porcentajeF = (totalct + avanceTotalFinalDC) / totalpt*100;
                     %>
            <tr class="danger">
                <td style="text-align: center; font-weight: bold;"><%:nivelP %></td>
                <td>100%</td>
                <!--<td style="text-align: center;"><%//:avanceat.ToString("0.00") %>%</td>-->
            
                <td style="text-align: center; font-weight: bold;"><%:totalProfesionalizacion.ToString("0.00") %>%</td>
                <td></td>
                <td style="text-align: center;">100%</td>
                <td style="text-align: center;"><%:totalpt %></td>
                <td style="text-align: center;"><%:totalct %></td>
                <td style="text-align: center;"><%:avanceTotalFinalDC %></td>
                <td style="text-align: center;"><%:activast %></td>
                <td style="text-align: center;"></td>
                <td style="text-align: center;"></td>
                <td style="text-align: center;"></td>
            </tr>

        </tbody>
    </table>
    <asp:Button ID="btnFinalizar" runat="server" class="btn btn-primary" Text="Finalizar" OnClick="btnFinalizar_Click" />

    </div>
   
</asp:Content>
