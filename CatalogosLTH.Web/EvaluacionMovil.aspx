<%@ Page Title="" Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="EvaluacionMovil.aspx.cs" Inherits="CatalogosLTH.Web.EvaluacionMovil" %>
<%@ Register assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="section  banner-actividades remove-top remove-bottom">
		<div class="container">
			<div class="row">
				<div class="twelve columns">
					            <h1></h1>

				</div>
			</div>
		</div>
	</div>
    <div class="container" style="margin-top:10%">
       
        <div class="row" style="padding:5%; -webkit-box-shadow: 6px 7px 6px 4px rgba(195,195,195,1);
        -moz-box-shadow: 6px 7px 6px 4px rgba(195,195,195,1);
        box-shadow: 6px 7px 6px 4px rgba(195,195,195,1);">
             <div class="row">
                 <div class="col-sm-10">
                     <div class="row">
                        <h1 style="color:#7e6b6b">Genera tu evaluación móvil</h1>
                     </div>
                       <dx:ASPxComboBox ID="cmbDist" runat="server"   HelpText="Distribuidor"  HelpTextSettings-Position="Top" OnSelectedIndexChanged="cmbDist_SelectedIndexChanged"  AutoPostBack="True" ValueType="System.String" Theme="ThemeLTH"></dx:ASPxComboBox>
            <dx:ASPxButton ID="btnSig" runat="server" Text="Comenzar" OnClick="btnSig_Click" Theme="ThemeLTH"></dx:ASPxButton>
                 </div>
            
            </div>
          
        </div>
    </div>
     
</asp:Content>
