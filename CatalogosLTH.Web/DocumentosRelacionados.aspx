﻿<%@ Page Title="" Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="DocumentosRelacionados.aspx.cs" Inherits="CatalogosLTH.Web.DocumentosRelacionados" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function OnGridFocusedRowChanged() {
            // Query the server for the "EmployeeID" and "Notes" fields from the focused row
            // The values will be returned to the OnGetRowValues() function
            DetailNotes.SetText("Loading...");
            grid.GetRowValues(grid.GetFocusedRowIndex(), 'Descripcion', OnGetRowValues);
        }
        // Value array contains "EmployeeID" and "Notes" field values returned from the server
        function OnGetRowValues(values) {
            
            DetailNotes.SetText(values[1]);
        }
    </script>
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
    <div class="col-sm-12">
        <div class="row">
            <dx:ASPxComboBox ID="cmbCategorias" runat="server" ValueType="System.String" OnSelectedIndexChanged="cmbCategorias_SelectedIndexChanged"></dx:ASPxComboBox>
        </div>
        <div class="row">
            <dx:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" KeyFieldName="Nombre" PreviewFieldName="Descripcion" AutoGenerateColumns="False"
                EnableRowsCache="false" Width="100%">
                <Columns>
                    <dx:GridViewDataColumn FieldName="Nombre" VisibleIndex="1" />
                    <dx:GridViewDataColumn FieldName="categoria" VisibleIndex="0" />                    
                </Columns>
                <SettingsBehavior AllowFocusedRow="True" />
                <ClientSideEvents FocusedRowChanged="function(s, e) { OnGridFocusedRowChanged(); }" />
            </dx:ASPxGridView>
        </div>
        <div class="row">
            <asp:Literal ID="DetailNotes" runat="server"></asp:Literal>
        </div>
    </div>
    
</asp:Content>
