﻿<%@ Page Title="" Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="Auditoria_movil.aspx.cs" Inherits="CatalogosLTH.Web.Auditoria_movil" %>

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

  
     <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <style>
        @media only screen and (max-width: 600px) {
            .breadcrumb {
                font-size: 0.9rem !important;
            }

            tr {
                font-size: 1rem !important;
            }

            .bread-wrapp {
                padding-left: 12px !important;
            }

            .breadcrumb-nav {
                height: 36px !important;
                line-height: 36px !important;
            }

            .hidden {
                display: none;
            }

            .colComentario {
                display: none;
            }

            .tblGrid {
                margin-left:-17px!important;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

            <%@ Import Namespace="DevExpress.Xpo" %>
            <%@ Import Namespace="CatalogosLTH.Module.BusinessObjects" %>
            <%@ Import Namespace="CatalogosLTH.Web" %>
      <%
              List<puntoarea> pa = ViewState["listaGrid"] as List<puntoarea>;
          
             %>
       <div class="section  banner-actividades remove-top remove-bottom">
		<div class="container">
			<div class="row">
				<div class="twelve columns">
					<h1>Auditoría Móvil</h1>
				</div>
			</div>
		</div>
	</div>
    <div class="container">
        <div class="col-sm-12" style="padding:0!important">
            <div class="row">
                <table>
                     <tr>
                        <td>
                            <dx:ASPxComboBox ID="cmbTipo" runat="server" OnSelectedIndexChanged="cmbTipo_SelectedIndexChanged"  AutoPostBack="True" ValueType="System.String" xmlns:dx="devexpress.web" Theme="Material" ></dx:ASPxComboBox>
                        </td>
                          <td style="padding:10px;margin:10px;"></td>
                        <td>
                            <dx:ASPxComboBox ID="cmbDist" runat="server" OnSelectedIndexChanged="cmbDist_SelectedIndexChanged"  AutoPostBack="True" ValueType="System.String" xmlns:dx="devexpress.web" Theme="Material"></dx:ASPxComboBox>
                        </td>
                       
                        </tr>
                </table>
            </div>
            <div class="row">
                <table style="margin-top:10px;">
                   
                    <tr style="margin:10px!important;padding:10px;"></tr>
                    <tr>
                        <td>
                           <dx:ASPxComboBox ID="cmbClave" runat="server"  AutoPostBack="True" OnSelectedIndexChanged="cmbClave_SelectedIndexChanged" ValueType="System.String" xmlns:dx="devexpress.web"  Theme="Material"></dx:ASPxComboBox>
                        </td>
                        <td style="padding:10px;margin:10px;"></td>
                         <td>
                            <dx:ASPxComboBox ID="cmbArea" runat="server" OnSelectedIndexChanged="cmbArea_SelectedIndexChanged"  AutoPostBack="True" ValueType="System.String" xmlns:dx="devexpress.web" Theme="Material"></dx:ASPxComboBox>
                        </td>
                    </tr>                    
                </table>               
                
                <div class="row">
                    <div class="col-sm-6">
                        <dx:ASPxTextBox ID="txtSearch" runat="server" Width="100%" Theme="Material" HelpText="Búsqueda"></dx:ASPxTextBox>
                    </div>
                    <div class="col-sm-6">
                        <dx:ASPxButton ID="btnSearch" runat="server" Text="Busca" Theme="Material" OnClick="btnSearch_Click" ></dx:ASPxButton>
                    </div>
                </div>
                <asp:Label ID="lblArea" runat="server" Text="" Visible="false"></asp:Label>
                 </div>

            <table class="table table-default tblGrid"  style="margin-top:10px">
            <colgroup>
                <col class="col-md-1">
                <col class="col-md-3">
                <col class="col-md-1" >
                <col class="col-md-1" >
                <col class="col-md-1" >
                <col class="col-md-1" >
            </colgroup>
             <thead>
                <tr style="background-color:#036CA5;">                            
                    <th style="text-align: center; color:white">CLAVE</th>
                    <th style="text-align: center; color:white">PUNTO</th>
                    <th style="text-align: center; color:white"><span class="icon" title=""><i class="fa fa-check"></i>Aceptada<span class="vertical-align"></span></span></th>
                    <th style="text-align: center; color:white"><span class="icon" title=""><i class="fa fa-times"></i>No Aceptada<span class="vertical-align"></span></span></th>
                    <th class="colComentario" style="text-align: center; color:white"><span class="icon" title=""><i class="fa fa-times"></i>Comentario<span class="vertical-align"></span></span></th>
                    <th style="text-align: center; color:white">LIMPIAR</th>
                </tr>
             </thead>
            <tbody>
                  <%
                      foreach (var item in pa)
                      {%>
                             
                                <tr class="default" >                                        
                                    <td style="font-weight: bold; text-align:center"><%:item.ClavePunto %></td><!--Nivel profesionalizacion cmn-toggle- -->                  
                                    <td style="font-weight: bold; text-align:center"><%:item.Punto %></td><!--Nivel profesionalizacion-->    
                                     
                                     <td>
                                        <div class="switch" style="width:89px;">
                                        <input name="si<%:item.ClavePunto %>" id="si<%:item.ClavePunto %>" value="si" class="cmn-toggle cmn-toggle-round" type="radio"  <%if (item.aceptada)
                                            {%>
                                                checked="checked"<%} %>
                                            > 
                                             <label for="si<%:item.ClavePunto %>" name="califal<%:item.ClavePunto %>"></label>
                                        </div>                
                                     </td>  
                                     <td>
                                        <div class="switch" style="width:89px">
                                        <input name="si<%:item.ClavePunto %>" id="<%:item.ClavePunto %>" value="no" class="cm-toggle cm-toggle-round subeEvidencia" type="radio"  <%if (item.noaceptada)
                                            {%>
                                                checked="checked"<%} %>
                                            >                                                                             
                                             <label for="<%:item.ClavePunto %>" name="si<%:item.ClavePunto %>"></label>
                                        </div>
                                     </td>  
                                    <td  class="colComentario" style="text-align:center" id="com<%:item.ClavePunto %>"><%:item.Comentario %></td><!--Nivel profesionalizacion-->
                                    <td style="text-align:center"><button type="button" punto="<%:item.ClavePunto %>" class="btn btn-success btnLimpiar"><i class="fa fa-eraser"></i></button></td>
                               </tr>       
                                <%}
                             %>
            </tbody>

        </table>
            <div class="row">             
                 <dx:ASPxTextBox ID="txtAutoriza"  runat="server" Width="100%" Theme="Material" HelpText="Persona del Distribuidor con la que se valida la auditoría"></dx:ASPxTextBox>                 
            </div>
            <div class="row">
                 <dx:ASPxButton ID="btnCerrar" runat="server" align="center" Text="Cerrar auditoría" OnClick="btnCerrar_Click" CssClass="btn btn-warning" Theme="ThemeLTH" Paddings-Padding="1%"  Border-BorderColor ="#FF8800" xmlns:dx="devexpress.web"></dx:ASPxButton>
            </div>
        
        </div>

        <div class="col-sm-3 hidden">
            <div class="col-sm-3" style="position:fixed; top:50%; background-color:#00B7A8; border-radius:5px; box-shadow:5px 5px 5px #888888" >
            <div class="row"  align="center">
                <asp:Label ID="lbl1" runat="server" Text="Sube evidencia para:" ForeColor="White"></asp:Label>
            </div>
            <div class="row"  align="center">
                <input name="idAud" id="idAud" style="font-size:22px"/>
            </div>
                <div class="row" style="position:fixed; top:80%" >
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Sube evidencia" />
                    <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Limpia estado" />
                </div>
             </div>
        </div>
        
        <!--<form id="form4" name="form4" action="auditoriamovildetalle.aspx" method="post">-->
            
        <!--</form>-->
    </div>

    <script>

        function llenar(id) {
            document.getElementById("idAud").value = id;
            
            document.getElementById("form1").submit();
            //_doPostback();
        }

        $(document).ready( function(){
            $(".btnLimpiar").click(function () {
                var idRadio = $(this).attr("punto");
                $("#si" + idRadio).removeAttr("checked");
                var comment = $('#com' + idRadio).text();
                if (comment == "") {
                    $('#' + idRadio).removeAttr("checked");
                }
                if ($('#idAud').val() != null || $('#idAud').val() != "") {
                    document.getElementById("idAud").value = $(this).attr('punto');
                    $('#' + '<%=Button2.ClientID%>').trigger('click');
                }
            });

            $(".subeEvidencia").click(function () {
                var comment = $('#com' + $(this).attr('id')).text();
                if (comment == "") {
                    document.getElementById("idAud").value = $(this).attr('id');
                    $('#' + '<%=Button1.ClientID%>').trigger('click');
                }
            });
        });
    </script>

</asp:Content>
