<%@ Page Title="" Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="Reporte3.aspx.cs" Inherits="CatalogosLTH.Web.Reporte3" %>

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
    <li><a href="reporte2.aspx">Nivel de profesionalización por zona</a></li>
    <li class="active"><a href="reporte3.aspx">Profesionalización mensual por zona</a></li>
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
               <h4>Profesionalización mensual por zona</h4>
           </div>
                </div>
        </div>
       <div class="row">
           <div class="col-sm-4">
               <dx:ASPxComboBox ID="comboYear" runat="server" ValueType="System.String" OnSelectedIndexChanged="comboYear_SelectedIndexChanged" AutoPostBack="True" Theme="ThemeLTH"></dx:ASPxComboBox>
           </div>
           <div class="col-sm-4">
               <dx:ASPxButton ID="ASPxButton1" Theme="Material" runat="server" Text="Excel" CssClass="btn btn-default"  OnClick="Excel"></dx:ASPxButton>
           </div>
           <div class="col-sm-4">
               <dx:ASPxButton ID="btnPdf" Theme="Material" runat="server" Text="PDF"  CssClass="btn btn-success" OnClick="btnPdf_Click" HorizontalAlign="Right"></dx:ASPxButton>
           </div>
            
       </div>
        <div class="row" style="margin-top:2%">
            <div class="col-sm-12">
             <dx:ASPxGridView ID="ASPxGridView1" runat="server" Width="100%" Theme="ThemeLTH"></dx:ASPxGridView>
            </div>
        </div>
        <div class="row">                
            <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server"></dx:ASPxGridViewExporter>
        </div>
       <div class="row" style="margin-top:2%">
           <div class="col-sm-1"></div>
           <div class="col-sm-10 card">
             <div id="line_top_x"></div>
            </div>
       </div>
       <div class="row" style="margin-top:2%; display:none" >
           <div id="panel1" class="col-sm-6">
               <asp:Panel ID="pnl1" runat="server"></asp:Panel>
           </div>
           <div id="panel2" class="col-sm-6" >
                <asp:Panel ID="pnl2" runat="server"></asp:Panel>
           </div>
       </div>
       <div class="row" style="display:none">
           <div id="panel3" class="col-sm-6">
                <asp:Panel ID="pnl3" runat="server"></asp:Panel>
           </div>
           <div id="panel4" class="col-sm-6">

           </div>
       </div>
    </div>
     <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>

    <script type="text/javascript">
        google.charts.load('current', { 'packages': ['line'] });
        google.charts.setOnLoadCallback(drawChart);

     function drawChart() {

           //var elementos = [];
         // elementos.push(['Mes', 'Porcentaje', { role: 'annotation' }]);
          <%        

        string[] meses = new string[12] { "Oct", "Nov", "Dec", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Ago", "Sep" };

       %>
      var data = new google.visualization.DataTable();
      data.addColumn('string', 'Mes');
     data.addColumn('number', 'Norte');
     data.addColumn('number', 'Sur');
     data.addColumn('number', 'CAM');


     data.addRows([
        <%
        for (int i = 0; i < 12; i++)
        {
        %>
        ['<%:meses.ElementAt(i)%>',
          <%:valoresN.Count > 0 ? valoresN.ElementAt(i).valor.ToString("0.00") : "0"%>,
          <%:valoresS.Count > 0 ? valoresS.ElementAt(i).valor.ToString("0.00") : "0"%>,
          <%:valoresC.Count > 0 ? valoresC.ElementAt(i).valor.ToString("0.00") : "0"%>],
        <%}%>
        
     ]);
         
      var options = {
        chart: {
          title: 'Avance en niveles de profesionalización',
        },
        width: 650,
        height: 300
      };

      var chart = new google.charts.Line(document.getElementById('line_top_x'));

      chart.draw(data, google.charts.Line.convertOptions(options));
    }
    </script>
</asp:Content>
