<%@ Page Title="" Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="DetalleActividad.aspx.cs" Inherits="CatalogosLTH.Web.DetalleActividad" %>

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

    <div class="section  banner-biblioteca remove-top remove-bottom">
		<div class="container">
			<div class="row">
				<div class="twelve columns">
					<h1>Biblioteca de Actividades</h1>
				</div>
			</div>
		</div>
	</div>	
     <nav class="navbar navbar-default" style="background-color:#F8F8F8; border-bottom:solid; border-bottom-width:0.5px; border-color:#E7E7E7;margin-bottom:0 ">
          <div class="container">


              <div class="col-sm-12">
                  <div class="row">
                      <div class="col-sm-5"></div>
                      <div class="col-sm-7">
                          <div class="col-sm-1"></div>
                          <div class="col-sm-11">
                              <%string codigoactividad = ViewState["codigoActividad"] as string;%>
                                <h1> <span class="label label-primary"><%:codigoactividad %></span></h1>
                          </div>                          
                      </div>
                  </div>
                <div class="row">
                    <asp:Literal ID="literalInstruccion" runat="server"></asp:Literal>
                </div>
                <div class="row">

                </div>
                    <dx:ASPxLabel ID="lblid" runat="server" Text="ASPxLabel" Visible="False"></dx:ASPxLabel>         
              </div>
                  
              
              
          </div>
      </nav> 
    <div class="container">
        <div class="col-sm-3"></div>
        <div class="col-sm-3">
            <dx:ASPxButton ID="Muestra1" runat="server" Text="Archivo Muestra 1" OnClick="Muestra1_Click" Theme="ThemeLTH"></dx:ASPxButton>
        </div>
        <div class="col-sm-3">
            <dx:ASPxButton ID="Muestra2" runat="server" Text="Archivo Muestra 2" OnClick="Muestra2_Click" Theme="ThemeLTH"></dx:ASPxButton>
        </div>
    
    </div>

</asp:Content>
