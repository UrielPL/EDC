<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/edcMaster.Master" CodeBehind="newAuditoria.aspx.cs" Inherits="CatalogosLTH.Web.newAuditoria" %>

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
  <div class="section  banner-auditoria remove-top remove-bottom">
		<div class="container">
			<div class="row">
				<div class="twelve columns">
					<h1>Crear Auditoría</h1>
				</div>
			</div>
		</div>
	</div>	
    <div class="container">


        	<div class="section  remove-bottom">
		<div class="container">
			<div class="row">
				<div class="twelve columns">
					<p>Personaliza la configuración para crear la auditoría:</p>
				</div>
			</div>
		</div>
	</div>	
	
	<div class="section  remove-bottom remove-top">
		<div class="container">
			<div class="forma-contacto remove-bottom">
				<div class="row">
					<div class="four columns auditoria">
						<label><p>Tipo de Auditoría</p></label>
						<asp:DropDownList ID="cmbTipoAuditoria" runat="server"></asp:DropDownList>
					</div>
					<div class="four columns auditoria">
						<label><p>Distribuidor</p></label>
						<asp:DropDownList ID="cmbDistribuidor" runat="server"></asp:DropDownList>
					</div>
					<div class="four columns auditoria">
					 <asp:Button ID="btnEmpezar" runat="server" Text="Empezar" CssClass="button" OnClick="btnEmpezar_Click" />
					</div>
				</div>
			</div>
		</div>
	</div>
	


       
        <asp:Button ID="btnNueva" runat="server" Text="Nueva" class="btn btn-primary" OnClick="btnNueva_Click" Visible="false" />
        <asp:Button ID="btnContinuar" runat="server" Text="Continuar" class="btn btn-primary" OnClick="btnContinuar_Click"  Visible="false"/>
    </div>
    

    

</asp:Content>
