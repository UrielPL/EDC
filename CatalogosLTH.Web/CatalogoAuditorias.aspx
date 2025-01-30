<%@ Page Title="" Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="CatalogoAuditorias.aspx.cs" Inherits="CatalogosLTH.Web.CatalogoAuditorias" %>

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
    function encripta(plain) {

        $.ajax({

        type: "POST",
        url: "ListaActividades.aspx/Encriptar",
        data: "{'plainText':" + plain + " }",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {          
                
            window.location.href = 'nivel.aspx?IdAuditoria=' + data.d;
           
        },
        failure: function (response) {
        }
    });
}

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="section  banner-biblioteca remove-top remove-bottom">
		<div class="container">
			<div class="row">
				<div class="twelve columns">
					<h1>Biblioteca de auditorias</h1>
				</div>
			</div>
		</div>
	</div>
    <div class="container">
        <dx:ASPxGridView ID="ASPxGridView1" runat="server" SettingsBehavior-AllowFocusedRow="True" SettingsBehavior-AllowSelectByRowClick="True" SettingsBehavior-AllowSelectSingleRowOnly="True" Theme="ThemeLTH" Width="95%" SettingsText-SearchPanelEditorNullText="Búsqueda rápida">
         <SettingsSearchPanel Visible="True" />
           <Styles>
                <AlternatingRow Enabled="True">
                </AlternatingRow>
            </Styles>
             <ClientSideEvents RowDblClick="function(s, e){
                      
                     s.GetRowValues(e.visibleIndex, 'IdAuditoria', function (data) {
                      encripta(data);                                       
                });
            }" />
    </dx:ASPxGridView>
    <dx:ASPxButton ID="btnSeleccionar" runat="server" Text="Ver Nivel" OnClick="btnSeleccionar_Click" Theme="ThemeLTH"></dx:ASPxButton>
    </div>
    
    
</asp:Content>
