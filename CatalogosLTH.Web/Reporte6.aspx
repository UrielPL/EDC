<%@ Page Title="" Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="Reporte6.aspx.cs" Inherits="CatalogosLTH.Web.Reporte6" %>

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
        <style>
        li{
            font-size:12px; 
        }
    </style>  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
     <div class="section  banner-reportes remove-top remove-bottom">
		<div class="container">
			<div class="row">
				<div class="twelve columns">
					<h1>Reportes</h1>
				</div>
			</div>
		</div>
	</div>	
      <%string permiso = ViewState["permiso"] as string; %> 
    <div class="container-fluid" style="padding:3% 0% 3% 0%">  
  <ul class="nav nav-pills nav-justified">
    <li><a href="tablero1.aspx">Tablero 1</a></li>
    <li><a href="tablero2.aspx">Tablero 2</a></li>
    <li><a href="reporte1.aspx">Reporte profesionalización</a></li>
    <li><a href="reporte2.aspx">Nivel de profesionalización por zona</a></li>
    <li><a href="reporte3.aspx">Profesionalización mensual por zona</a></li>
    <%if (permiso != "Distribuidor"){%> 
    <li><a href="reporte4.aspx">Reporte actividades completadas</a></li><%} %>  
    <li><a href="reporte5.aspx">Actividades completadas por zona por mes</a></li> 
    <%if (permiso != "Distribuidor"){%>           
    <li class="active"><a href="reporte6.aspx">Profesionalización por gerente de cuenta</a></li>
    <li><a href="reporte7.aspx">Profesionalización por pilar </a></li><%} %>
    <%if (permiso != "h" && permiso!= "y"){%>           
    <li><a href="reporte8.aspx">Profesionalización mensual y actividades completadas por zona</a></li>
    <li><a href="reporte9.aspx">Profesionalización mensual y actividades completadas por distribuidor</a></li>
    <li class=""><a href="reporte10.aspx">Actividades mensuales por distribuidor</a></li>

    <%} %>
  </ul>
</div>

    <div class="container">
        <div class="row">
            <div class="col-sm-12">
           <div class="page-header">
               <h4>Profesionalización por gerente de cuenta</h4>
           </div>
                </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <div class="row">
                    <dx:ASPxGridView ID="ASPxGridView1" runat="server" Theme="ThemeLTH"></dx:ASPxGridView>
                </div>
                <div class="row">
                    <div class="col-sm-3">  
                        <dx:ASPxButton Theme="ThemeLTH" ID="ASPxButton1" runat="server" Text="Exporta a Excel" CssClass="btn btn-default" OnClick="ASPxButton1_Click"></dx:ASPxButton>
                    </div>
                <div class="col-sm-2">
                    <div class="col-sm-1"><dx:ASPxButton ID="btnPdf" Theme="ThemeLTH" runat="server" Text="Exportar a PDF"  CssClass="btn btn-success" OnClick="btnPdf_Click" HorizontalAlign="Right"></dx:ASPxButton></div>
                </div>  
                </div>                
            </div>
        </div>
        <div class="row">
            <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server"></dx:ASPxGridViewExporter>
        </div>
        <div class="row">
              <div id="chart_div"></div>

        </div>
    </div>
<script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>

    <script type="text/javascript">
               google.charts.load('current', { packages: ['corechart', 'bar'] });
        google.charts.setOnLoadCallback(drawBasic);

        function drawBasic() {

          var elementosS = [];
            elementosS.push(['Nombre', 'Nivel', { role: 'annotation' }]);
          <%
        var lsttemp = lstGerentes.OrderByDescending(v => v.ProfPromedio).Where(v=>v.ProfPromedio!="0.00").ToList();
        for (int i = 0; i < lsttemp.Count(); i++)
        {%>
          elementosS.push(['<%:lsttemp.ElementAt(i).Nombre.Replace("á","a").Replace("é","e").Replace("í","i")%>',
                           <%:lsttemp.ElementAt(i).ProfPromedio%>,
                           '<%:lsttemp.ElementAt(i).ProfPromedio%>'])
          <%}%>
         

            var data = google.visualization.arrayToDataTable(elementosS);

            var options = {
                title: 'Profesionalización por gerente',
                chartArea: { width: '40%' },
                hAxis: {
                    title: 'Profesionalización promedio',
                    minValue: 0
                },
                annotations: {
                    textStyle: {
                        fontName: 'Roboto',
                        fontSize: 12,
                        bold: true,
                        opacity: 0.8
                    }
                },
                vAxis: {
                    title: 'Gerente'
                },
                width: 850,
                height: 1400
            };

            var chart = new google.visualization.BarChart(document.getElementById('chart_div'));

            chart.draw(data, options);
        }
    </script>
</asp:Content>
