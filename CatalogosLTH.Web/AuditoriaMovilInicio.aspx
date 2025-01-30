<%@ Page Title="" Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="AuditoriaMovilInicio.aspx.cs" Inherits="CatalogosLTH.Web.AuditoriaMovilInicio" %>

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

       <script type="text/javascript">
        function grid_SelectionChanged(s,e) {
            s.GetSelectedFieldValues("Punto", GetSelectedFieldValuesCallback);
        }
        function GetSelectedFieldValuesCallback(values) {
            selList.BeginUpdate();
            try {
                selList.ClearItems();
                for(var i=0;i<values.length;i++) {
                    selList.AddItem(values[i]);
                }
            
            } finally {
                selList.EndUpdate();
            }
            document.getElementById("selCount").innerHTML = gridPuntoArea.GetSelectedRowCount();
        }
        function OnGridFocusedRowChanged() {
            // Query the server for the "EmployeeID" and "Notes" fields from the focused row
            // The values will be returned to the OnGetRowValues() function
            DetailNotes.SetText("Loading...");
            gridPuntoArea.GetRowValues(gridPuntoArea.GetFocusedRowIndex(), 'Punto', OnGetRowValues);
        }
        // Value array contains "EmployeeID" and "Notes" field values returned from the server
        function OnGetRowValues(values) {          
            DetailNotes.SetText(values);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="section  banner-actividades remove-top remove-bottom">
		<div class="container">
			<div class="row">
				<div class="twelve columns">
					<h1>Actividades a revisar</h1>
				</div>
			</div>
		</div>
	</div>
    <div class="container">
        <div class ="jumbotron">
            <h1 align="center">Auditoría Móvil</h1>
        </div>
        <div class="col-sm-4" style="margin-top:1%">
            <div class="panel panel-default" style="padding-bottom:2%; padding-top:2%">
                <div class="row" style="padding-left:5%"> 
                    <div class="col-sm-5">
                        <h4>Típo de auditoría: </h4>
                    </div>
                    <div class="col-sm-7" style="padding:2px">
                        <dx:ASPxComboBox ID="cmbTipo" runat="server" ValueType="System.String" OnSelectedIndexChanged="cmbTipo_SelectedIndexChanged" AutoPostBack="True"></dx:ASPxComboBox>
                    </div>                    
                </div>
                <div class="row" style="padding-left:5%">
                    <div class="col-sm-5">
                        <h4>Distribuidor: </h4>
                    </div>
                    <div class="col-sm-7" style="padding:2px">
                        <dx:ASPxComboBox ID="cmbDist" runat="server" ValueType="System.String" OnSelectedIndexChanged="cmbDist_SelectedIndexChanged" AutoPostBack="True"></dx:ASPxComboBox>
                    </div>
                </div>
                <div class="row" style="padding-left:5%">
                    <div class="col-sm-5">
                        <h4>Área: </h4>
                    </div>
                    <div class="col-sm-7" style="padding:2px">
                        <dx:ASPxComboBox ID="cmbArea" runat="server" ValueType="System.String" OnSelectedIndexChanged="cmbArea_SelectedIndexChanged" AutoPostBack="True"></dx:ASPxComboBox>
                    </div>
                </div>
            </div>

             <div class="row" data-spy="affix" data-offset-top="205" style="margin-top:10%; padding:3%" >
                 <dx:ASPxListBox ID="ASPxListBox1" ClientInstanceName="selList" runat="server" Height="350px"
                 Width="100%" ItemStyle-Wrap="True" />
             </div>
            <div class="TopPadding">
             Puntos seleccionados: <span id="selCount" style="font-weight: bold">0</span>
             </div>
            <div class="row" align="center">
                <dx:ASPxButton ID="btnRevision" runat="server" Text="Envia puntos a revisión" OnClick="btnRevision_Click"></dx:ASPxButton>
            </div>
           
        </div>

        <div class="col-sm-8" align="center">
           
            <div class="row" >                            
               <dx:ASPxGridView ID="gridPuntoArea" runat="server" ClientInstanceName="gridPuntoArea" OnDataBound="gridPuntoArea_DataBound">                      
                     <Settings ShowFilterBar="Visible" ShowHeaderFilterButton="True" />
                     <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="False" AllowSelectSingleRowOnly="False" />
                     <SettingsSearchPanel Visible="True" />
                         <Styles>
                             <AlternatingRow Enabled="True">
                             </AlternatingRow>
                         </Styles>
                    <ClientSideEvents SelectionChanged="grid_SelectionChanged" />
                    <ClientSideEvents FocusedRowChanged="function(s, e) { OnGridFocusedRowChanged(); }" />
                    </dx:ASPxGridView>                 
            </div>
            <div class="row">
                <div class="col-sm-12">
                     <div class="row">
                        <dx:ASPxLabel ID="DetailNotes"  ClientInstanceName="DetailNotes" runat="server"  Text="ASPxLabel" Width="100%" Font-Size="Medium" Font-Bold="True"></dx:ASPxLabel>
                    </div>    
                    <div class="row">
                        <dx:ASPxButton ID="btnAceptar" ClientInstanceName="btnAceptar" runat="server" Text="Subir Evidencia"  CssClass="btn btn-success" OnClick="btnAceptar_Click" HorizontalAlign="Right">
                        </dx:ASPxButton>
                    </div>                                   
                </div>                
            </div>
        </div>
        
    </div>
</asp:Content>
