﻿<%@ Page Title="" Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="NivelDistribuidor.aspx.cs" Inherits="CatalogosLTH.Web.NivelDistribuidor" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.XtraCharts.v22.1.Web, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web" TagPrefix="dx" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
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
       
        <div class="row">
           
            <dx:ASPxComboBox ID="dropDist2" OnSelectedIndexChanged="dropDist2_SelectedIndexChanged" runat="server" ValueType="System.String" autopostback="True" Theme="ThemeLTH" ></dx:ASPxComboBox>
        </div>
  

      <table class="table table-striped">
          <thead>
            <tr style="background-color:#036CA5;">
                <th style="text-align: center; color:white">Nivel</th>
                <th style="text-align: center;color:white">Total Actividades</th>
                <th style="text-align: center;color:white">Realizadas en Auditoria</th>
                <th style="text-align: center;color:white">Realizadas en DC</th>
                <th style="text-align: center;color:white">% Cumplimiento</th>
            </tr>
          </thead>
          <tbody>
            <%@ Import Namespace="DevExpress.Xpo" %>
            <%@ Import Namespace="DevExpress.XtraCharts" %>
            <%@ Import Namespace="CatalogosLTH.Module.BusinessObjects" %>
            <%@ Import Namespace="CatalogosLTH.Web" %>
            <%@ Import Namespace="System.Data" %>
            <%@ Import Namespace="System.Drawing" %>
            
            
            

            <% 
                DevExpress.Xpo.Session session = Util.getsession();
                string IdAuditoria = ViewState["IdAuditoria"].ToString();
                int idaudi = int.Parse(IdAuditoria);
                int tipoaud = 0;
                string nombredist = "";
                int iddistribuidor = 0;

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
                           select new { a.idaud, a.Idtipoaud.idTipoAuditoria, d.nombredist, d.iddistribuidor };
                foreach (var item in sqlw)
                {
                    tipoaud = item.idTipoAuditoria;
                    nombredist = item.nombredist;
                    iddistribuidor = item.iddistribuidor;
                    //cerrada = true;
                }


                Session["profes"] = 0;
                double idtotalv = 0;
                bool existe = false;
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

                int[] arrTotal = new int[5];
                int[] arrActivas = new int[5];
                string[] nomPilar = new string[5];
                arrActivas[0] = 0;
                arrActivas[1] = 0;
                arrActivas[2] = 0;
                arrActivas[3] = 0;
                arrActivas[4] = 0;

                arrTotal[0] = 0;
                arrTotal[1] = 0;
                arrTotal[2] = 0;
                arrTotal[3] = 0;
                arrTotal[4] = 0;
                int numPilar = 0;

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

                int contFila = 1;
                foreach (var rec3 in sql3)//FOREACH NIVELES
                { %>
                       
                        <%

                            var sql2 = from vt in totales
                                       where vt.idnivel == rec3.idnivel
                                       && vt.idtipoaud == tipoaud
                                       select vt;
                            foreach (var record2 in sql2)//FOREACH PILARES
                            {

                                totalp += record2.total;
                               

                                var sqln = from vpt in puntostotales
                                           where vpt.idaud == idaudi &&
                                           vpt.idpilar == record2.idpilar &&
                                           vpt.idnivel == record2.idnivel
                                           select vpt;
                               
                                idtotalv = 0;
                                foreach (var item in sqln)
                                {
                                    idtotalv++;//ACT A COMPLETAR EN DC
                                }

                        %>
                        
                          
                                <%
                                    double completadasAuditoria = 0;
                                    if (record2.total == 0)
                                    {//
                                %>
                                <% %>
                                <%
                                    }
                                    else
                                    {
                                        completadasAuditoria = (record2.total - idtotalv);
                                %>
                                <% %>
                                <%
                                        totalc = record2.total - idtotalv + totalc;
                                    }
                                %>
                           

                           
                                <%//Calculo de las actividades ya realizadas

                                    var sqlAudAct = from audact in auditoriaActividad
                                                    where audact.Idaud.idaud.ToString() == IdAuditoria && audact.fechacomp != null && audact.Idactividad.NivelID.nombreniv == record2.nombreniv && audact.Idactividad.PilarID.nombrepil == record2.nombrepil
                                                    select audact;

                                    int actividadesRealizadasDC = sqlAudAct.Count();
                                    totaldc += actividadesRealizadasDC;
                                     %>
                                <% %>
                               
                            
                                <%
                                    if (nomPilar[numPilar] == null)
                                    {
                                        nomPilar[numPilar] = record2.nombrepil;
                                    }
                                    arrTotal[numPilar] = arrTotal[numPilar] + record2.total;
                                    double actividadesTotalesPorRealizarDeLaAuditoria = idtotalv;
                                    if (record2.total == 0)
                                    {
                                        comp = 0;

                                            %>
                                            <% %>
                                            <%

                                                }
                                                else
                                                {
                                                    comp = idtotalv;
                                                    activas = comp + activas;

                                                    double actividadesActivas = actividadesTotalesPorRealizarDeLaAuditoria - actividadesRealizadasDC;

                                %>
                                <%  %>
                                <%
                                        arrActivas[numPilar] = arrActivas[numPilar] + (int)actividadesActivas;
                                    }
                                %>
                          
                            <%
                                double avanceAuditoria = 0;
                                if (record2.total == 0)
                                {
                                    comp = record2.pondercionPilar;
                                    //avanceAuditoria = 100;
                                    avanceAuditoria = 0;
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
                                double avanceDC = (actividadesTotalesPorRealizarDeLaAuditoria == 0) ? 0 : Convert.ToDouble(actividadesRealizadasDC) / Convert.ToDouble(record2.total) * Convert.ToDouble(record2.pondercionPilar);
                                avanceTotalDC += avanceDC;
                                //double avanceTotal =(record2.total==0)?100: (completadasAuditoria + actividadesRealizadasDC) / record2.total*100;
                                double avanceTotal = (record2.total == 0) ? record2.pondercionPilar : (completadasAuditoria + actividadesRealizadasDC) / record2.total * record2.pondercionPilar;
                                avanceTotalT += avanceTotal;

                                numPilar++;
                            %>
                          

                        </tr>
                        <%}//SE CIERRA FOREACH  PILARES%>
              
                        <%if (contFila % 2 == 0)
                            {
                                /**/
	                         %>     
                                <tr style="background-color:lightgray;">
                        <%
                            }
                            else
                            {
                            %>   
                                <tr style="background-color:white">
                        <%
                            }
                             %>
                                          
                        
                            <td><%:rec3.nombreniv %></td><!--Nombre nivel-->
                            <td style="text-align: center;"><%:totalp %></td><!--Total actividades-->
                            <%
                                double profesionalizacionNivel = (rec3.ponderacion * avanceTotalT) / 100.0;
                                totalProfesionalizacion += profesionalizacionNivel;//TOTAL PROFESIONALIZACION LINEA ROJA
                                double porcentaje = (totalc + totaldc) / totalp * 100;

                            %>
                            <!--<td style="text-align: center; font-weight: bold;"><%//:((rec3.ponderacion*avancea)/100.0).ToString("0.00") %> %</td>-->
                            <td style="text-align: center;"><%:totalc%> </td><!--Completadas en Auditoria-->
                            <td style="text-align: center;"><%:totaldc %></td><!--Completadas en DC-->
                            <td style="text-align: center;"><%:porcentaje.ToString("0.00") %>%</td><!--nivel prof-->
                            
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

                                numPilar = 0;

                            }//PRIMER FOREACH
            %>
            <tr style="border-top:solid 2px #036CA5">
                <%
                    Util.AsignarProfesionalizacion(iddistribuidor, totalProfesionalizacion);
                    string nivelP = "";
                  /*  if (totalProfesionalizacion >=0&&totalProfesionalizacion<=44)
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
                    */

                    
                    if (totalProfesionalizacion >= 0 && totalProfesionalizacion < 44.5)
                    {
                        nivelP = "Básico";
                    }
                    else if (totalProfesionalizacion >= 44.5 && totalProfesionalizacion <= 65.5)
                    {
                        nivelP = "Transitorio";
                    }
                    else if (totalProfesionalizacion >= 65.5 && totalProfesionalizacion < 80.5)
                    {
                        nivelP = "Intermedio";
                    }
                    else if (totalProfesionalizacion >= 80.5 && totalProfesionalizacion < 94.5)
                    {
                        nivelP = "Avanzado";
                    }
                    else if (totalProfesionalizacion >= 94.5 && totalProfesionalizacion <= 100)
                    {
                        nivelP = "Sobresaliente";
                    }

                    double porcentajeF = (totalct + avanceTotalFinalDC) / totalpt*100;

                    int total1 = arrTotal[0];
                    int total2 = arrTotal[1];
                    int total3 = arrTotal[2];
                    int total4 = arrTotal[3];
                    int total5 = arrTotal[4];

                    int act1 = arrActivas[0];
                    int act2 = arrActivas[1];
                    int act3 = arrActivas[2];
                    int act4 = arrActivas[3];
                    int act5 = arrActivas[4];

                    double[] arrPorcentaje = new double[5];
                    double porcentaje1 = 100;
                    double porcentaje2 = 100;
                    double porcentaje3 = 100;
                    double porcentaje4 = 100;
                    double porcentaje5 = 100;

                    if (total1!=0){ porcentaje1 = 100-(act1 * 100 / total1);}
                    if (total2!=0){ porcentaje2 = 100-(act2 * 100 / total2);}
                    if (total3!=0){ porcentaje3 = 100-(act3 * 100 / total3);}
                    if (total4!=0){ porcentaje4 = 100-(act4 * 100 / total4);}
                    if (total5!=0){ porcentaje5 = 100-(act5 * 100 / total5);}

                    arrPorcentaje[0] = porcentaje1;
                    arrPorcentaje[1] = porcentaje2;
                    arrPorcentaje[2] = porcentaje3;
                    arrPorcentaje[3] = porcentaje4;
                    arrPorcentaje[4] = porcentaje5;

                    DataTable dt = new DataTable();
                    dt.Columns.Add("PILAR", typeof(String));
                    dt.Columns.Add("AVANCE", typeof(int));
                    for (int i = 0; i < 5; i++)
                    {
                        DataRow newRow = dt.NewRow();
                        newRow["PILAR"] = nomPilar[i];
                        newRow["AVANCE"] = arrPorcentaje[i];
                        dt.Rows.Add(newRow);
                    }



                    /* WebChartControl1.DataSource = dt;
                     WebChartControl1.Series["PILAR"].ArgumentDataMember = "PILAR";
                     WebChartControl1.Series["AVANCE"].ArgumentDataMember = "AVANCE";*/



                    // Create an empty Bar series and add it to the chart.
                    DevExpress.XtraCharts.Series series = new DevExpress.XtraCharts.Series("Cumplimiento", ViewType.Bar);


                    WebChartControl1.Series.Add(series);

                    // Generate a data table and bind the series to it.
                    series.DataSource = dt;

                    // Specify data members to bind the series.
                    //series.ArgumentScaleType = ScaleType.Auto;
                    series.ArgumentDataMember = "PILAR";
                    series.ValueScaleType = ScaleType.Numerical;
                    series.ValueDataMembers.AddRange(new string[] { "AVANCE" });


                    // Set some properties to get a nice-looking chart.
                    ((SideBySideBarSeriesView)series.View).ColorEach = false;
                    ((DevExpress.XtraCharts.XYDiagram)WebChartControl1.Diagram).Rotated = true;
                    ((SideBySideBarSeriesView)series.View).FillStyle.FillMode = FillMode.Solid;
                    ((SideBySideBarSeriesView)series.View).Color = Color.YellowGreen;


                    ((XYDiagram)WebChartControl1.Diagram).AxisY.Visibility = DevExpress.Utils.DefaultBoolean.True;
                    WebChartControl1.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;
                    WebChartControl1.Height = 500;
                    WebChartControl1.Width = 500;

                    XYDiagram diagram = (XYDiagram)WebChartControl1.Diagram;


                    diagram.AxisY.VisualRange.Auto = false;
                    diagram.AxisX.VisualRange.SetMinMaxValues(0, 100);//This line throws as //error
                    diagram.AxisX.Label.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;
                    WebChartControl1.DataBind();

                    // Dock the chart into its parent and add it to the current form.
                    //WebChartControl1.Dock = DockStyle.Fill;


                    /*
                    WebChartControl1.DataSource = dt;
                    WebChartControl1.SeriesDataMember = "AVANCE";
                    WebChartControl1.SeriesTemplate.ArgumentDataMember = "PILAR";
                    //WebChartControl1.SeriesTemplate.ValueDataMembers.AddRange(new string[] {"PILAR"});
                    WebChartControl1.SeriesTemplate.View = new SideBySideBarSeriesView();
                    WebChartControl1.DataBind();*/


                     %>
                <td></td>
                <td style="text-align: center;font-weight: bold; font-size:18px;  color: yellowgreen"><%:totalpt %></td><!--Total actividades-->
                <td style="text-align: center;font-weight: bold; color: yellowgreen"><%:totalct %></td>
                <td style="text-align: center;font-weight: bold; color: yellowgreen"><%:avanceTotalFinalDC %></td>
                <!--<td style="text-align: center;"><%//:avanceat.ToString("0.00") %>%</td>-->
                <td style="text-align: center;font-weight: bold; color: yellowgreen"><%:porcentajeF.ToString("0.00") %>%</td>
             
            </tr>

              <tr>
                  <td></td>
                  <td></td>
                  <td></td>
                  <td></td>
                  <td></td>
              </tr>

             
        </tbody>
    </table>
    <div class="container">
        
        <div class="row">
            <div class="col-sm-4"></div>
            <div class="col-sm-4">
                
              </div>
        </div>

        <div class="row">
            <div class="col-sm-4"></div>
            <div class="col-sm-4">
                <%
                    if("X"=="kk")//Tabla que no se despliega
                    {
                    
                 %>
                <p><%:nomPilar[0] %></p>
                       <table class="table" style=<%:"width:"+arrPorcentaje[0]+"%"%>>
                          <tbody>
                            <tr>
                               <th class="info" style="width:10%"><%:arrPorcentaje[0]%>%</th>
                            </tr>
                          </tbody>
                        </table>
                <%
                    }
                     %>

            </div>
            <div class="col-sm-4"></div>
        </div>
      

     	<div class="section  remove-top">
		<div class="container">
			<div class="row tabla">
				<div class="six columns" align="center">
					 <table class="table">
                        <thead>
                            <tr style="background-color:#036CA5;">                            
                                <th ></th>
                                <th style="text-align: center; color:white">RESULTADO</th>
                                <th style="text-align: center; color:white">NIVEL</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr class="default">      
                                <th style="background-color:#036CA5; color:white">PROFESIONALIZACIÓN</th>
                                <td style="font-weight: bold"><%:totalProfesionalizacion.ToString("0.00") %>%</td><!--Nivel profesionalizacion-->                  
                                <td style="font-weight: bold"><%:nivelP %></td><!--Nivel profesionalizacion-->
                            </tr>
                        </tbody>
                    </table>
                    <div class="text-center">
                        <dx:ASPxButton ID="btnLista" OnClick="btnLista_Click" runat="server" Text="Historial Kardex" theme="ThemeLTH"></dx:ASPxButton>
                        <dx:ASPxButton ID="btnGenKardex" OnClick="ASPxButton1_Click" runat="server" Text="Generar Kardex" theme="ThemeLTH"></dx:ASPxButton>
                    </div>
				</div>
				<div class="six columns" align="center">
                <dx:WebChartControl ID="WebChartControl1" runat="server" AutoLayout="False" LoadingPanelStyle-HorizontalAlign="NotSet" LoadingPanelStyle-Wrap="True" ></dx:WebChartControl>
				</div>
			</div>
		</div>
	</div>


    </div>

   </div>
   
</asp:Content>
