<%@ Page Title="" Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="reporte2.aspx.cs" Inherits="CatalogosLTH.Web.reporte2" %>

<%@ Register Assembly="DevExpress.XtraCharts.v22.1.Web, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.XtraCharts.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="dx" %>
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
        .card{
            -webkit-box-shadow: 0px 14px 26px -4px rgba(0,0,0,0.75);
-moz-box-shadow: 0px 14px 26px -4px rgba(0,0,0,0.75);
box-shadow: 0px 14px 26px -4px rgba(0,0,0,0.75);

        }
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
    <li class="active"><a href="reporte2.aspx">Nivel de profesionalización por zona</a></li>
    <li><a href="reporte3.aspx">Profesionalización mensual por zona</a></li>
    <%if (permiso != "Distribuidor"){%> 
    <li><a href="reporte4.aspx">Reporte actividades completadas</a></li><%} %>  
    <li><a href="reporte5.aspx">Actividades completadas por zona por mes</a></li> 
    <%if (permiso != "Distribuidor"){%>           
    <li><a href="reporte6.aspx">Profesionalización por gerente de cuenta</a></li>
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
            <div class="col-sm-1"></div>
            <div class="col-sm-11">
           <div class="page-header">
               <h4>Nivel de profesionalización por zona</h4>
           </div>
                </div>
        </div>

        <div class="row">
            <div class="col-sm-12">
             <dx:ASPxGridView ID="ASPxGridView1" runat="server" Width="100%" Theme="ThemeLTH"></dx:ASPxGridView>
            </div>
        </div>
        <div class="row">
             <div class="col-sm-3">
                    <div class="col-sm-1"><dx:ASPxButton ID="ASPxButton1" runat="server" Text="Exportar a excel"  CssClass="btn btn-success" OnClick="Excel" HorizontalAlign="Right" Theme="ThemeLTH"></dx:ASPxButton></div>
                </div>
                <div class="col-sm-2">
                    <div class="col-sm-1"><dx:ASPxButton ID="btnPdf" runat="server" Text="Exportar a PDF"  CssClass="btn btn-success" OnClick="btnPdf_Click" HorizontalAlign="Right" Theme="ThemeLTH"></dx:ASPxButton></div>
                </div>           
            <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server"></dx:ASPxGridViewExporter>
        </div>
        <div class="row ">
            <div class="col-sm-3 card">
                    <div id="piechartN" style="width: 400px; height: 400px;"></div>
            </div><div class="col-sm-1"></div>
            <div class="col-sm-3 card ">
                    <div id="piechartS" style="width: 400px; height: 400px;"></div>
            </div><div class="col-sm-1"></div>
            <div class="col-sm-3 card ">
                    <div id="piechartC" style="width: 400px; height: 400px;"></div>
            </div>

        </div>
        <div class="row">
            <div class="col-sm-12" style="display:none">
                <asp:Panel ID="Panel1" runat="server"></asp:Panel>
            </div>
        </div>
                        <!--<canvas id="myChart" width="400" height="400"></canvas>-->

        
    </div>

    <script>
        var vNorte = [
         <%foreach (var n in valNorte)
        {%>
            <%=n.ToString("0.00")%>,
         <%}%>
        ];
        console.log(vNorte);
         var lNorte = [
         <%foreach (var l in lblNorte)
        {%>
            "<%=l.Replace("á", "a")%>",
         <%}%>
         ];
        console.log("2w");
        console.log(lNorte);
       /* var ctx = document.getElementById("myChart");
        var myChart = new Chart(ctx, {
            type: 'doughnut',
            data: {
                
                labels:lNorte,
                datasets: [{
                    
                    label: '# of Votes',
                    data: vNorte,
                    backgroundColor: [
                        'rgba(255, 99, 132, 0.2)',
                        'rgba(54, 162, 235, 0.2)',
                        'rgba(255, 206, 86, 0.2)',
                        'rgba(75, 192, 192, 0.2)',
                        'rgba(153, 102, 255, 0.2)',
                        'rgba(255, 159, 64, 0.2)'
                    ],
                    borderColor: [
                        'rgba(255,99,132,1)',
                        'rgba(54, 162, 235, 1)',
                        'rgba(255, 206, 86, 1)',
                        'rgba(75, 192, 192, 1)',
                        'rgba(153, 102, 255, 1)',
                        'rgba(255, 159, 64, 1)'
                    ],
                    borderWidth: 1
                }]
            },
            
            options: {
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                }
            },

            plugins: {
                datalabels: {
                    display: true
                },
                tooltips: {
                    callbacks: {
                        label: function (tooltipItem, data) {
                            var lblnumberrep = data.datasets[0].data[tooltipItem.index];

                            return "f";
                        }
                    }
                }
            }
        });*/
    </script>

    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <script type="text/javascript">
      google.charts.load('current', {'packages':['corechart']});
      google.charts.setOnLoadCallback(drawChart);
      google.charts.setOnLoadCallback(drawChartS);
      google.charts.setOnLoadCallback(drawChartC);


      function drawChart() {

          var elementos = [];
          elementos.push(['Nivel', 'Porcentaje', { role: 'annotation' }]);
          <%
        for (int i = 0; i < valNorte.Count(); i++)
        {%>
          elementos.push(['<%:lblNorte.ElementAt(i).Replace("á", "a")%>',
                           <%:valNorte.ElementAt(i).ToString("0.0")%>,
                           '<%:valNorte.ElementAt(i).ToString("0.0")%>'])
          <%}%>
          var arr = ([['Task', 'Hours per Day'],
          ['Work', 11],
          ['Eat', 2],
          ['Commute', 2],
          ['Watch TV', 2],
          ['Sleep', 7]]);
          console.log("dddd:");

          console.log(elementos);
          console.log(arr);

        var data = google.visualization.arrayToDataTable(elementos);

        var options = {
            title: 'Niveles Norte',
            annotations: {
                textStyle: {
                    fontName: 'Roboto',
                    fontSize: 20,
                    bold: true,
                    opacity: 0.8
                }
            },
            pieHole: 0.3,

            hAxis: {
                textStyle: {
                  fontSize: 12     // or the number you want
                }
            },
        };

        var chart = new google.visualization.PieChart(document.getElementById('piechartN'));

        chart.draw(data, options);
      }

    function drawChartS() {

          var elementosS = [];
          elementosS.push(['Nivel', 'Porcentaje', { role: 'annotation' }]);
          <%
        for (int i = 0; i < valSur.Count(); i++)
        {%>
          elementosS.push(['<%:lblSur.ElementAt(i).Replace("á", "a")%>',
                           <%:valSur.ElementAt(i).ToString("0.0")%>,
                           '<%:valSur.ElementAt(i).ToString("0.0")%>'])
          <%}%>
         
        var data = google.visualization.arrayToDataTable(elementosS);

        var options = {
            title: 'Niveles Sur',
            annotations: {
                textStyle: {
                    fontName: 'Roboto',
                    fontSize: 20,
                    bold: true,
                    opacity: 0.8
                }
            },
            pieHole: 0.3,

            hAxis: {
                textStyle: {
                  fontSize: 12     // or the number you want
                }
            },
        };

        var chart = new google.visualization.PieChart(document.getElementById('piechartS'));

        chart.draw(data, options);
    }

        function drawChartC() {

          var elementosC = [];
          elementosC.push(['Nivel', 'Porcentaje', { role: 'annotation' }]);
          <%
        for (int i = 0; i < valCam.Count(); i++)
        {%>
          elementosC.push(['<%:lblCam.ElementAt(i).Replace("á", "a")%>',
                           <%:valCam.ElementAt(i).ToString("0.0")%>,
                           '<%:valCam.ElementAt(i).ToString("0.0")%>'])
          <%}%>
         
        var data = google.visualization.arrayToDataTable(elementosC);

        var options = {
            title: 'Niveles CAM',
            annotations: {
                textStyle: {
                    fontName: 'Roboto',
                    fontSize: 20,
                    bold: true,
                    opacity: 0.8
                }
            },
            pieHole: 0.3,

            hAxis: {
                textStyle: {
                  fontSize: 12     // or the number you want
                }
            },
        };

        var chart = new google.visualization.PieChart(document.getElementById('piechartC'));

        chart.draw(data, options);
      }
    </script>

    
</asp:Content>

