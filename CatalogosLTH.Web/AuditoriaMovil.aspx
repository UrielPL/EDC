<%@ Page Title="" Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="AuditoriaMovil.aspx.cs" Inherits="CatalogosLTH.Web.AuditoriaMovil" %>
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
            <%@ Import Namespace="DevExpress.Xpo" %>
            <%@ Import Namespace="CatalogosLTH.Module.BusinessObjects" %>
            <%@ Import Namespace="CatalogosLTH.Web" %>
       <div class="section  banner-actividades remove-top remove-bottom">
		<div class="container">
			<div class="row">
				<div class="twelve columns">
					<h1>Actividades a revisar</h1>
				</div>
			</div>
		</div>
	</div>
     <div class="col-sm-12">
          <%
              List<puntoarea> pa = ViewState["listaGrid"] as List<puntoarea>;


             %>
                 <table class="table table-bordered table-hover" >
                     <colgroup>
                      <col class="col-md-1">
                      <col class="col-md-3">
                      <col class="col-md-1" >
                   
                    </colgroup>
                    <thead>
                        <tr style="background-color:#036CA5;">                            
                            <th style="text-align: center; color:white">CLAVE</th>
                            <th style="text-align: center; color:white">PUNTO</th>
                            <th style="text-align: center; color:white"><span class="icon" title=""><i class="fa fa-times"></i><span class="vertical-align"></span></span></th>
                            <th style="text-align: center; color:white"><span class="icon" title=""><i class="fa fa-check"></i><span class="vertical-align"></span></span></th>
                          <!--   <th style="text-align: center; color:white"><span class="icon" title=""><i class="fa fa-comment"></i><span class="vertical-align"></span></span></th>
                            <th style="text-align: center; color:white"><span class="icon" title=""><i class="fa fa-file-image-o "></i><span class="vertical-align"></span></span></th>
                              foreach
                            </tr>
                        </thead>
                         <tbody>
                             <%
                                --> (var item in pa)
                                {%>
                             
                                <tr class="default">                                       
                                    <td style="font-weight: bold; text-align:center"><%:item.ClavePunto %></td><!--Nivel profesionalizacion-->                  
                                    <td style="font-weight: bold; text-align:center"><%:item.Punto %></td><!--Nivel profesionalizacion-->           
                                     <td>
                                        <div class="switch" style="width:89px;">
                                        <input name="califa<%:item.ClavePunto %>" id="cmn-toggle-<%:item.ClavePunto %>" class="cmn-toggle cmn-toggle-round" type="radio">                                        
                                             <label for="cmn-toggle-<%:item.ClavePunto %>" name="califal<%:item.ClavePunto %>"></label>
                                        </div>                
                                     </td> 
                                     <td>
                                        <div class="switch" style="width:89px">
                                        <input name="califa<%:item.ClavePunto %>" id="cm-toggle-<%:item.ClavePunto %>" class="cm-toggle cm-toggle-round" type="radio">                                        
                                             <label for="cm-toggle-<%:item.ClavePunto %>" name="no<%:item.ClavePunto %>"></label>
                                        </div>                
                                     </td>
                                          
                                                            
                               </tr>          

                                <%}
                             %>
                         </tbody>
                     </table>
                        

    </div>
    <div class="col-sm-4" style="margin-top:1%">
            <div class="panel panel-default" style="padding-bottom:2%; padding-top:2%">
                <div class="row" style="padding-left:5%"> 
                    <div class="col-sm-5">
                        <h4>Típo de auditoría: </h4>
                    </div>
                    <div class="col-sm-7" style="padding:2px">
                        <dx:ASPxComboBox ID="cmbTipo" OnSelectedIndexChanged="cmbTipo_SelectedIndexChanged" runat="server" ValueType="System.String"  AutoPostBack="True" xmlns:dx="devexpress.web" Theme="ThemeLTH"></dx:ASPxComboBox>
                    </div>                    
                </div>
                <div class="row" style="padding-left:5%">
                    <div class="col-sm-5">
                        <h4>Distribuidor: </h4>
                    </div>
                    <div class="col-sm-7" style="padding:2px">
                        <dx:ASPxComboBox ID="cmbDist" OnSelectedIndexChanged="cmbDist_SelectedIndexChanged" runat="server" ValueType="System.String" AutoPostBack="True" xmlns:dx="devexpress.web" Theme="ThemeLTH"></dx:ASPxComboBox>
                    </div>
                </div>
                <div class="row" style="padding-left:5%">
                    <div class="col-sm-5">
                        <h4>Área: </h4>
                    </div>
                    <div class="col-sm-7" style="padding:2px">
                        <dx:ASPxComboBox ID="cmbArea" OnSelectedIndexChanged="cmbArea_SelectedIndexChanged" runat="server" ValueType="System.String" AutoPostBack="True" xmlns:dx="devexpress.web" Theme="ThemeLTH"></dx:ASPxComboBox>
                    </div>
                </div>
            </div>
      
             <div class="row" data-spy="affix" data-offset-top="205" style="margin-top:10%; padding:3%" >
                 <dx:ASPxListBox ID="ASPxListBox1" ClientInstanceName="selList" runat="server" Height="350px"
                 Width="100%" ItemStyle-Wrap="True" xmlns:dx="devexpress.web" Theme="ThemeLTH"/>
             </div>
            <div class="TopPadding">
             Puntos seleccionados: <span id="selCount" style="font-weight: bold">0</span>
             </div>
            <div class="row" align="center">
                <dx:ASPxButton ID="btnRevision" OnClick="btnRevision_Click" runat="server" Text="Envia puntos a revisión" xmlns:dx="devexpress.web" Theme="ThemeLTH"></dx:ASPxButton>
            </div>
           
        </div>
   
</asp:Content>
