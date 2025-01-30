<%@ Page Title="" Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="ActividadesCompletadas.aspx.cs" Inherits="CatalogosLTH.Web.ActividadesCompletadas" %>

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

          
                
            window.location.href = 'SeleccionActividad.aspx?codigo=' + data.d;
           
        },
        failure: function (response) {
         
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
					<h1>Actividades Completadas</h1>
				</div>
			</div>
		</div>
	</div>
    
    <div class="section  remove-bottom">
		<div class="container">
			<div class="row">
				<div class="six columns">
					<p>Selecciona el Plan de Negocio:</p>
                     <asp:DropDownList ID="dropDist" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dropDist_SelectedIndexChanged" >
                    </asp:DropDownList>
				</div>
				<div class="six columns green-button" align="right">
                    <dx:ASPxButton ID="ASPxButton2" runat="server"  Text="Exporta Excel" Theme="ThemeLTH" OnClick="ASPxButton2_Click"  ></dx:ASPxButton>
					<!--<a href="#filtro" class="add-bottom fancybox">Descargar Excel</a>-->
				</div>
			</div>
		</div>
	</div>	
    	
    <div class="container">
    <div class="col-sm-12">
       
        <div class="row">
           
        </div>
        <div class="row">
            <dx:ASPxGridView ID="ASPxGridView1" runat="server"  Theme="ThemeLTH" SettingsText-SearchPanelEditorNullText="Búsqueda rápida">
                 <Settings ShowFilterBar="Visible" ShowHeaderFilterButton="True" />
                 <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowSelectSingleRowOnly="True" />
                 <SettingsSearchPanel Visible="True" />
                 <Styles>
                     <AlternatingRow Enabled="True">
                     </AlternatingRow>
                 </Styles>

                  <ClientSideEvents RowDblClick="function(s, e){
                      
                     s.GetRowValues(e.visibleIndex, 'idactplan', function (data) {
                          
                      encripta(data);                                       
                });
            }" />
            </dx:ASPxGridView>
        </div>
        <div class="row">
            <div class="col-sm-10">
                <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Seleccionar" OnClick="ASPxButton1_Click"></dx:ASPxButton>
            </div>
            <div class="col-sm-2">
                
                 
            </div>
        </div>
    </div>
        </div>
    <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server"></dx:ASPxGridViewExporter>
</asp:Content>
