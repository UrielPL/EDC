﻿<%@ Page Title="" Language="C#" MasterPageFile="~/EDCClarios.Master" AutoEventWireup="true" CodeBehind="EdicionObjetivosMinutas.aspx.cs" Inherits="CatalogosLTH.Web.EdicionObjetivosMinutas" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .form-group {
            margin-bottom: 0rem!important;
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
    </style>
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 text-center" style="background-color: #6141b0; color: white; padding: 10px; font-size: 18px; font-weight: 600;">
                <span style="text-transform: uppercase;">OBJETIVOS DISTRIBUIDOR: <%=dist != null ? dist.nombredist : distMin.nombredist %></span>
            </div>
        </div>
        <br /><br />
        <div class="container">
            <div class="row">
                <div class="col-md-1 offset-md-11">
                    <a class="class" href="ObjetivosMinutas.aspx">
                        <span class="btnBack">Regresar</span>
                    </a>
                </div>
            </div>
            <br />
            <div class="row">
                <div id="alerta" class="col-md-8 offset-md-2">
                    <div class="alert alert-success alert-dismissible fade show" role="alert">
                        Objetivos de distribuidor actualizados.
                        <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                </div>
                <div class="col-md-6 offset-md-2">
                    <table class="table table-bordered clarios-table">
                        <thead>
                            <tr class="text-center">
                                <th style="background-color: #6141b0; color: white;" scope="col">MES</th>
                                <th style="background-color: #6141b0; color: white;" scope="col">OBJETIVOS FY <%=FY %></th>
                            </tr>
                        </thead>
                        <tbody class="text-center">
                            <tr>
                                <td class="align-middle">OCT</td>
                                <td>
                                    <div class="form-group ">
                                        <input type="text" class="form-control" id="txtOct" value="<%= lstObjetivos[0][0] %>"/>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="align-middle">NOV</td>
                                <td>
                                    <div class="form-group">
                                        <input type="text" class="form-control" id="txtNov" value="<%= lstObjetivos[0][1] %>" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="align-middle">DIC</td>
                                <td>
                                    <div class="form-group">
                                        <input type="text" class="form-control" id="txtDic" value="<%= lstObjetivos[0][2] %>" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="align-middle">ENE</td>
                                <td>
                                    <div class="form-group">
                                        <input type="text" class="form-control" id="txtEne" value="<%= lstObjetivos[0][3] %>" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="align-middle">FEB</td>
                                <td>
                                    <div class="form-group">
                                        <input type="text" class="form-control" id="txtFeb" value="<%= lstObjetivos[0][4] %>" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="align-middle">MAR</td>
                                <td>
                                    <div class="form-group">
                                        <input type="text" class="form-control" id="txtMar" value="<%= lstObjetivos[0][5] %>" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="align-middle">ABR</td>
                                <td>
                                    <div class="form-group">
                                        <input type="text" class="form-control" id="txtAbr" value="<%= lstObjetivos[0][6] %>" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="align-middle">MAY</td>
                                <td>
                                    <div class="form-group">
                                        <input type="text" class="form-control" id="txtMay" value="<%= lstObjetivos[0][7] %>" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="align-middle">JUN</td>
                                <td>
                                    <div class="form-group">
                                        <input type="text" class="form-control" id="txtJun" value="<%= lstObjetivos[0][8] %>" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="align-middle">JUL</td>
                                <td>
                                    <div class="form-group">
                                        <input type="text" class="form-control" id="txtJul" value="<%= lstObjetivos[0][9] %>"/>
                                    </div>                                                                               
                                </td>                                                                                    
                            </tr>                                                                                        
                            <tr>                                                                                         
                                <td class="align-middle">AGO</td>                                                        
                                <td>                                                                                     
                                    <div class="form-group">                                                             
                                        <input type="text" class="form-control" id="txtAgo" value="<%= lstObjetivos[0][10] %>" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="align-middle">SEP</td>
                                <td>
                                    <div class="form-group">
                                        <input type="text" class="form-control" id="txtSep" value="<%= lstObjetivos[0][11] %>"/>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="col-md-1 offset-md-1">
                    <button id="btnGuardar" style="background-color: #cc0099; border-color: #cc0099; font-size: 18px;" type="button" class="btn btn-primary">Guardar</button>
                </div>
            </div>
        </div>
    </div>
    <script>

        $(document).ready(function () {
            $("#alerta").hide();

            $('#btnGuardar').on('click', function () {
                guardarObjetivos();
            });
        });

        function guardarObjetivos() {
            var meses = ["txtOct", "txtNov", "txtDic", "txtEne", "txtFeb", "txtMar", "txtAbr", "txtMay", "txtJun", "txtJul", "txtAgo", "txtSep"];

            var newObjetivos = [];

            $.each(meses, function (index, value) {
                newObjetivos[index] = $('#' + value).val();
            });

            console.log(newObjetivos);

            $.ajax({
                type: "POST",
                url: "EdicionObjetivosMinutas.aspx/GuardaObjetivos",
                contentType: "application/json; charset=utf-8",
                data: '{ "newObjetivos" : "' + newObjetivos + '" }',
                dataType: "json",
                success: function (data) {
                    $("#alerta").show();

                    setInterval(function () {
                        $("#alerta").hide();
                    }, 3000);
                },
                failure: function (response) {
                    console.log(response.d);
                }
            });
        }

    </script>
</asp:Content>