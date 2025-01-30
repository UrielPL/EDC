<%@ Page Title="" Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="ListaActividades.aspx.cs" Inherits="CatalogosLTH.Web.ListaActividades" %>
<%@ Register assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
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
        console.log("metodo");

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
            console.log(response.d);
        }
    });
}

</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="section  banner-plan remove-top remove-bottom">
		<div class="container">
			<div class="row">
				<div class="twelve columns">
					<h1>Plan de Negocio</h1>
				</div>
			</div>
		</div>
	</div>	
	
	
	<div class="section  remove-bottom">
		<div class="container">
			<div class="row">
				<div class="six columns">
					<p>Selecciona el Plan de Negocio:</p>
				</div>
				<div class="six columns green-button" align="right">
					                <dx:ASPxButton ID="imgButton" runat="server"  Text="Exporta Excel" CssClass="btn btn-default"  OnClick="Excel"></dx:ASPxButton>

				</div>
			</div>
		</div>
	</div>	

    <div class="container">
 <div class="section  remove-bottom remove-top">
		<div class="container">
				<div class="row">
					<div class="four columns auditoria">
						<label><p>Elige el nivel</p></label>
						<asp:DropDownList ID="dropNivel" runat="server" EnableTheming="true" Theme="Material" AutoPostBack="True" OnSelectedIndexChanged="dropNivel_SelectedIndexChanged">
                        </asp:DropDownList> 
					</div>
					<div class="four columns auditoria">
						<label><p>Distribuidor</p></label>
						 <asp:DropDownList ID="dropDist" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dropDist_SelectedIndexChanged">
                         </asp:DropDownList>
					</div>
					<div class="four columns auditoria search">
						
					</div>
				</div>
		</div>
	</div>

        <div class="row">
            <div class="col-sm-2">
                
            </div>
            <div class="col-sm-2">
               
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <asp:Label ID="lblMensaje" backcolor="LightYellow" runat="server" Text=""></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <dx:ASPxGridView ID="ASPxGridView1" runat="server" EnableTheming="True" Theme="ThemeLTH" OnHtmlDataCellPrepared="ASPxGridView1_HtmlDataCellPrepared"  Style="" SettingsText-SearchPanelEditorNullText="Búsqueda rápida">
                <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowSelectSingleRowOnly="True" />
                <SettingsDataSecurity AllowEdit="False" AllowInsert="False" AllowDelete="False"></SettingsDataSecurity>

                <SettingsSearchPanel Visible="True" />
                <Styles AdaptiveDetailButtonWidth="22">
                </Styles>
                     <ClientSideEvents RowDblClick="function(s, e){
                      
                     s.GetRowValues(e.visibleIndex, 'idactplan', function (data) {
                          console.log(data);
                      encripta(data);                                       
                });
            }" />
                </dx:ASPxGridView>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-1">
                <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Seleccionar" OnClick="ASPxButton1_Click"  CssClass="btn btn-default"></dx:ASPxButton>
            </div>
            <div class="col-sm-8"></div>
            <div class="col-sm-1">                
            </div>
        </div>
    </div>
    
    <dx:ASPxGridViewExporter ID="Exporter1" runat="server" OnRenderBrick="Exporter1_RenderBrick"></dx:ASPxGridViewExporter>
</asp:Content>
