<%@ Page Title="" Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="RevisionAuditoriaMovil.aspx.cs" Inherits="CatalogosLTH.Web.RevisionAuditoriaMovil" %>

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
            <table class="table table-default">   
                <tbody>
                    <tr>
                        <td style="text-align:start">
                            <h3>Distribuidor</h3>
                        </td>
                        <td style="vertical-align:middle; text-align:start">
                            <dx:ASPxLabel ID="lblDist" Font-Bold="true" Font-Size="Large"  runat="server" Text="Distribuidor" Theme="ThemeLTH"></dx:ASPxLabel>
                        </td>
                          
                    </tr>
                    <tr>

                        <td style="text-align:start">
                            <h3>Centro de servicio</h3>
                        </td>
                        <td style="vertical-align:middle; text-align:start">
                            <dx:ASPxLabel ID="lblCS" Font-Bold="true" Font-Size="Large"  runat="server" Text="Centro de servicio" Theme="ThemeLTH"></dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:start">
                            <h3>Persona con la que se valida la auditoria</h3>
                        </td>
                        <td style="vertical-align:middle; text-align:start">
                            <dx:ASPxLabel ID="lblValidadoPor" Font-Bold="true" Font-Size="Large"  runat="server" Text="Validado Por" Theme="ThemeLTH"></dx:ASPxLabel>
                        </td>
                          
                    </tr>
                    <tr>
                        <td style="text-align:start">
                             <h3>Tipo auditoría: </h3>
                        </td>
                        <td style="vertical-align:middle; text-align:start">
                            <dx:ASPxLabel ID="lblTipo" runat="server" Font-Bold="true" Font-Size="Large" Text="auditoría" Theme="ThemeLTH"></dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:start">
                             <h3>Área: </h3>
                        </td>
                        <td style="vertical-align:middle; text-align:start">
                            <dx:ASPxLabel ID="lblArea" Font-Bold="true" Font-Size="Large" runat="server" Text="Área" Theme="ThemeLTH"></dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:start">
                            <h3>Punto: </h3>
                        </td>
                        <td style="vertical-align:middle; text-align:start">
                            <dx:ASPxLabel ID="lblPunto" Font-Bold="true" Font-Size="Large" runat="server" Text="Punto" Theme="ThemeLTH"></dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:start">
                            <h3>Evaluador: </h3>
                        </td>
                        <td style="vertical-align:middle; text-align:start">
                            <dx:ASPxLabel ID="lblEvaluador" Font-Bold="true" Font-Size="Large" runat="server" Text="Evaluador" Theme="ThemeLTH"></dx:ASPxLabel>
                        </td>
                    </tr>                    
                    <tr>
                        <td style="text-align:start">
                            <h3>Comentario del evaluador: </h3>
                        </td>
                        <td style="vertical-align:middle; text-align:start">
                            <dx:ASPxLabel ID="lblComentario" Font-Bold="true" Font-Size="Large" runat="server" Text="Archivo" Theme="ThemeLTH"></dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:start">
                            <h3>Fecha de evaluación: </h3>
                        </td>
                        <td style="vertical-align:middle; text-align:start">
                            <dx:ASPxLabel ID="lblFecha" Font-Bold="true" Font-Size="Large" runat="server" Text="Archivo" Theme="ThemeLTH"></dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:start">
                            <h3>Cumplimiento: </h3>
                        </td>
                        <td style="vertical-align:middle; text-align:start">
                            <dx:ASPxLabel ID="lblCumplimiento" Font-Bold="true" Font-Size="Large" runat="server" Text="Archivo" Theme="ThemeLTH"></dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align:start">
                            <h3>Archivo evidencia: </h3>
                        </td>
                        <td style="vertical-align:middle; text-align:start">
                            <dx:ASPxLabel ID="lblArchivo" Font-Bold="true" Font-Size="Large" runat="server" Text="Archivo" Theme="ThemeLTH"></dx:ASPxLabel>
                        </td>                        
                    </tr>
                    <tr>
                        <td style="text-align:start">
                            <dx:ASPxButton ID="btnFile" runat="server" Text="Descarga Archivo" OnClick="btnFile_Click" Theme="ThemeLTH"></dx:ASPxButton>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
          
            
            
        
       <div class="row">
             <div class="row">
                <dx:ASPxCheckBox ClientVisible="false" ID="checkCerrada" runat="server"   CheckState="Unchecked" Visible="True" >
                    <CheckedImage Height="36px" Url="~/Images/Toggle On-64.png" Width="120px">
                    </CheckedImage>
                    <UncheckedImage Height="36px" Url="~/Images/Toggle Off-64.png" Width="120px">
                    </UncheckedImage>
                </dx:ASPxCheckBox>                 
            </div>
            <div class="row"></div>

            <div class="row" >
                <div class="col-sm-8 col-sm-offset-2" style="box-shadow:5px 5px 5px #888888";">

                
                     <div class="col-sm-3" style="margin:3%;vertical-align:middle">
                        <dx:ASPxButton ID="btnAceptar" runat="server" Text="Punto Completado" OnClick="btnAceptar_Click" CssClass="btn btn-success" Theme="ThemeLTH" Paddings-Padding="8%"></dx:ASPxButton>
                    </div>
                    <div class="col-sm-3" style="margin:3%; text-align:center" >
                        <dx:ASPxButton ID="btnRechazar" runat="server" Text="No Cumple" OnClick="btnRechazar_Click" CssClass="btn btn-warning" Theme="ThemeLTH" Paddings-Padding="8%" Border-BorderColor="#FF8800"></dx:ASPxButton>
                    </div>
                    <div class="col-sm-3" style="margin:3%">
                        <dx:ASPxButton ID="btnIgnorar" runat="server" Text="Ignorar revisión" OnClick="btnIgnorar_Click" CssClass="btn btn-default" Theme="ThemeLTH" Paddings-Padding="8%" Border-BorderColor="Gray"></dx:ASPxButton>
                    </div>
                </div>    
                
                
                
                <dx:ASPxButton ID="btnRegresar" runat="server" Text="Regresar" Visible="false" OnClick="btnRegresar_Click " Theme="ThemeLTH"></dx:ASPxButton>
            </div>
        </div>

        
    </div>
</asp:Content>
