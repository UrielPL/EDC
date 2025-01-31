﻿<%@ Page Title="" Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="categorias.aspx.cs" Inherits="CatalogosLTH.Web.categorias" %>
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
     <script type = "text/javascript">

         function encripta(plain)
         {
            window.location.href = 'documentos.aspx?idcat=' + plain;
         }

</script>

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
        <div class="row">
            <dx:ASPxGridView ID="grdCats" runat="server" EnableRowsCache="true" Theme="ThemeLTH">
                 <Settings ShowFilterBar="Visible" ShowHeaderFilterButton="True"  />
                         <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowSelectSingleRowOnly="True" />
                         <SettingsSearchPanel Visible="True" />
                             <Styles>
                                 <AlternatingRow Enabled="True">
                                 </AlternatingRow>
                             </Styles>
                <ClientSideEvents RowDblClick="function(s, e){
                      
                     s.GetRowValues(e.visibleIndex, 'Clave', function (data) {
                      encripta(data);                                       
                });
            }" />
            </dx:ASPxGridView>
        </div>
        <div class="row">
            <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Selecciona"  OnClick="ASPxButton1_Click"></dx:ASPxButton>
        </div>
    </div>

</asp:Content>
