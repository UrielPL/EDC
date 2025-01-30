<%@ Page Title="" Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="ListaGeneralAuditoriaMovil.aspx.cs" Inherits="CatalogosLTH.Web.ListaGeneralAuditoriaMovil" %>

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
        console.log("metodo");

        $.ajax({

        type: "POST",
        url: "ListaActividades.aspx/Encriptar",
        data: "{'plainText':" + plain + " }",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

          
                
            window.location.href = 'listaauditoriasmoviles.aspx?codigo=' + data.d;
           
        },
        failure: function (response) {
            console.log(response.d);
        }
    });
}

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="section  banner-actividades remove-top remove-bottom">
		<div class="container">
			<div class="row">
				<div class="twelve columns">
					<h1>Revisión Auditorias Móviles</h1>
				</div>
			</div>
		</div>
	</div>
    <div class="container">
        <div class="row">
            <dx:ASPxGridView ID="grdAuditorias" runat="server" Theme="ThemeLTH">
                <Settings ShowFilterBar="Visible" ShowHeaderFilterButton="True" />
                <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowSelectSingleRowOnly="True" />
                <SettingsSearchPanel Visible="True" />
                    <Styles>
                        <AlternatingRow Enabled="True">
                        </AlternatingRow>
                    </Styles>
                    <ClientSideEvents RowDblClick="function(s, e){
                      
                     s.GetRowValues(e.visibleIndex, 'Id', function (data) {
                          console.log(data);
                      encripta(data);                                       
                });
            }" />
            </dx:ASPxGridView>
        </div>
        <div class="row">
            <dx:ASPxButton ID="btnSiguiente" runat="server" Text="Seleccionar" OnClick="btnSiguiente_Click"></dx:ASPxButton>
        </div>
    </div>
</asp:Content>
