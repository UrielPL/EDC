<%@ Page Title="" Language="C#" MasterPageFile="~/EDCClarios.Master" AutoEventWireup="true" CodeBehind="CargaMinutas.aspx.cs" Inherits="CatalogosLTH.Web.CargaMinutas" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .labelCampo {
            padding: 5px; 
            background-color: #6141b0; 
            color: white;
        }

        .labelCampoInv {
            padding: 5px; 
            background-color: #00b0f0; 
            color: white;
        }

        a.class, a.class:hover {
            text-decoration: none;
        }

        .btnBack {
            padding: 10px;
            background-color: #6141b0;
            color: white;
            font-size: 18px;
        }

         .tarjeta{
            -webkit-box-shadow: 7px 7px 14px -1px rgba(0,0,0,0.75);
            -moz-box-shadow: 7px 7px 14px -1px rgba(0,0,0,0.75);
            box-shadow: 7px 7px 14px -1px rgba(0,0,0,0.75);
        }
    </style>

    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 text-center" style="background-color: #6141b0; color: white; padding: 10px; font-size: 18px; font-weight: 600;">
                <span style="text-transform: uppercase;">Minutas de visita a distribuidores</span>
            </div>
        </div>
        <br /><br />
        <div class="container">
            <div class="row">
                <div class="col-md-1 offset-md-11">
                    <a class="class tarjeta" href="MinutasIndex.aspx">
                        <span class="btnBack">Regresar</span>
                    </a>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-6">
                    <div class="row">
                        <div class="col-md-6">
                             <div class="labelCampo text-center">
                                <span>DISTRIBUIDOR</span>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <dx:ASPxComboBox ID="cbxDist" Theme="ThemeLTH" runat="server" ValueType="System.String"></dx:ASPxComboBox>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-6">
                             <div class="labelCampo text-center">
                                <span>FECHA</span>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <dx:ASPxDateEdit ID="dtFecha" Theme="ThemeLTH" runat="server"></dx:ASPxDateEdit>
                        </div>
                    </div>
                    <br /><br />
                    <div class="row ">
                        <div class="col-md-6">
                             <div class="labelCampoInv text-center">
                                <span>ADJUNTAR ARCHIVO</span>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <asp:FileUpload ID="archivo" runat="server"  />
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-4 offset-md-4 ">
                            <dx:ASPxButton ID="btnGuardar" CssClass="tarjeta" OnClick="btnGuardar_Click" Theme="ThemeLTH" SkinID="SkinClarios" runat="server" Text="GUARDAR MINUTA"></dx:ASPxButton>
                        </div>
                    </div>
                </div>
                <div class="col-md-5 offset-md-1">
                    <table class="table table-striped tarjeta">
                        <thead>
                            <tr class="text-center">
                                <th style="background-color: #6141b0; color: white;" scope="col" colspan="3">CARGAS DEL MES</th>
                            </tr>
                        </thead>
                        <tbody id="registroMinutas">
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <script>

        // Variables para los valores actuales de los select
        var dist;
        var fy;
        var mes;

        $(document).ready(function () {
            actualizaTabla();
        });

        function actualizaTabla() {
            $.ajax({
                type: "POST",
                url: "CargaMinutas.aspx/ActualizaTabla",
                contentType: "application/json; charset=utf-8",
                data: '{ "Dist" : "' + dist + '", "FY" : "' + fy + '", "mes" : "' + mes + '" }',
                dataType: "json",
                success: function (data) {
                    $('#registroMinutas').empty();
                    var json = JSON.parse(data.d);
                    $.each(json, function (index, value) {
                        var link = '<a download href="Archivos/Minutas/' + value.archivo + '" style="font-size:12px!important" target="_blank">' + value.archivo + '</a>';
                        $("#registroMinutas").append("<tr><td>" + value.fecha + "</td><td>" + link + "</td><td>" + value.dist + "</td></tr>");
                    });
                },
                failure: function (response) {
                    
                }
            });
        }

    </script>
</asp:Content>
