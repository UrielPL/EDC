<%@ Page Title="" Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="DetalleDocumento.aspx.cs" Inherits="CatalogosLTH.Web.DetalleDocumento" %>
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
					<h1>Documentos relacionados</h1>
				</div>
			</div>
		</div>
	</div>	
     <div class="container">


              <div class="col-sm-12">
                  <div class="row">
                      <div class="col-sm-5 " align="center"></div>
                      <%string codigoactividad = ViewState["codigoActividad"] as string;%>
                         <h1> <span class="label label-primary"><%:codigoactividad %></span></h1>
                  </div>
                <div class="row">
                    <asp:Literal ID="literalInstruccion" runat="server"></asp:Literal>
                </div>
                <div class="row">

                </div>
                    <dx:ASPxLabel ID="lblid" runat="server" Text="ASPxLabel" Visible="False"></dx:ASPxLabel>         
              </div>
                  
              
              
          </div>
      
    <div class="container">
        <div class="col-sm-3"></div>
        <div class="col-sm-3">
            <dx:ASPxButton ID="Muestra1" runat="server" Text="Descargar archivo" OnClick="Muestra1_Click" Theme="ThemeLTH"></dx:ASPxButton>
        </div>
        
    
    </div>
</asp:Content>
