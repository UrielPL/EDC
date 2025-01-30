<%@ Page Title="" Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="reporte1.aspx.cs" Inherits="CatalogosLTH.Web.reporte1" %>
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

    <%
        string permiso = ViewState["permiso"] as string; %> 
    <div class="container-fluid" style="padding:3% 0% 3% 0%">  
      <ul class="nav nav-pills nav-justified">
        <li><a href="tablero1.aspx">Tablero 1</a></li>
        <li><a href="tablero2.aspx">Tablero 2</a></li>
        <li class="active"><a href="reporte1.aspx">Reporte profesionalización</a></li>
        <li><a href="reporte2.aspx">Nivel de profesionalización por zona</a></li>
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
                   <h4>Profesionalización</h4>
               </div>
           </div>
        </div>
         <table class="table">
                        <thead>
                            <tr style="background-color:#036CA5;">                            
                                <th ></th>
                                <th style="text-align: center; color:white">ZONA</th>
                                <th style="text-align: center; color:white">NIVEL</th>
                            </tr>
                        </thead>
                         <tbody>
                        
        <%
            double[] prof = ViewState["profesionalizacion"] as double[];
            string[] nombre = ViewState["nombrezona"] as string[];
            
            for (int i = 0; i < prof.Length; i++)
            {
            
              %>
           <tr class="default">   
               <td></td>
                <td style="font-weight: bold; text-align:center"><%:nombre[i]%></td><!--Nivel profesionalizacion-->                  
                <td style="font-weight: bold; text-align:center"><%:prof[i].ToString("0.00") %>%</td><!--Nivel profesionalizacion-->                  
           </tr>           
        <%
            }
            %>                       
            </tbody>
            </table>
        </div>

</asp:Content>
