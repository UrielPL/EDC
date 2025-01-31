﻿<%@ Page Title="" Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="CatalogoActividades.aspx.cs" Inherits="CatalogosLTH.Web.CatalogoActividades" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Xpo.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Xpo" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link rel="stylesheet" href="css/slicknav.css" />

      <script src="//ajax.googleapis.com/ajax/libs/jquery/2.1.3/jquery.min.js"></script>
  <script src="js/jquery.bxslider/jquery.bxslider.min.js"></script>
  <script src="js/jquery.owl-carousel/owl.carousel.js"></script>
  <script src="js/jquery.fancybox/jquery.fancybox.pack.js"></script>
  <script src="js/master.js"></script>
  <script src="js/jquery.slicknav.js"></script>
  <script src="http://cdnjs.cloudflare.com/ajax/libs/modernizr/2.6.2/modernizr.min.js"></script>
      <script type = "text/javascript">
    function encripta(plain) {
        window.location.href = 'DetalleActividad.aspx?idactividad=' + plain;
    
}

</script>
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
    <div class="container">
       
        <div class="row">
            <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" EnableTheming="True" Theme="ThemeLTH" SettingsText-SearchPanelEditorNullText="Búsqueda rápida">
            <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowSelectSingleRowOnly="True" />
            <Styles AdaptiveDetailButtonWidth="22">
                     <AlternatingRow Enabled="True">
                     </AlternatingRow>
            </Styles>
                 
            <SettingsSearchPanel Visible="True" />
                   <ClientSideEvents RowDblClick="function(s, e){
                      
                     s.GetRowValues(e.visibleIndex, 'IdActividad', function (data) {
                      encripta(data);                                       
                });
            }" />
            </dx:ASPxGridView>
        </div>
    <dx:ASPxButton ID="btnSeleccionar" runat="server" Text="Seleccionar" OnClick="btnSeleccionar_Click" Theme="ThemeLTH"></dx:ASPxButton>
    </div>
   
</asp:Content>
