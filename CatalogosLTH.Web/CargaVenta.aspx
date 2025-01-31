﻿<%@ Page Title="" Language="C#" MasterPageFile="~/EDCClarios.Master" AutoEventWireup="true" CodeBehind="CargaVenta.aspx.cs" Inherits="CatalogosLTH.Web.CargaVenta" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .tarjeta {
            -webkit-box-shadow: 7px 7px 14px -1px rgba(0,0,0,0.75);
            -moz-box-shadow: 7px 7px 14px -1px rgba(0,0,0,0.75);
            box-shadow: 7px 7px 14px -1px rgba(0,0,0,0.75);
            padding: 2%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 text-center" style="background-color: #6141b0; color: white; padding: 10px; font-size: 18px; font-weight: 600;">
                <span style="text-transform: uppercase;">CARGA VENTAS</span>
            </div>
        </div>
    </div>

    <div class="container">
        <div class="col-sm-12">
            <div class="row">
                <div class="col-md-1 offset-md-11">
                </div>
            </div>
            <div class="container" style="margin-top: 2%">

                <table class="table table-dark ">
                    <thead>
                        <tr>
                            <th class="align-middle">Distribuidor:</th>
                            <th></th>
                            <th class="align-middle"><%:NombreDist %></th>
                            <th>
                                <a class="btn btn-info float-right" href="mainpage.aspx">
                                    <span class="btnBack">Regresar</span>
                                </a>
                            </th>
                        </tr>
                    </thead>

                </table>
            </div>
            <div class="container tarjeta" style="margin-top: 2%">

                <div class="row" style="margin: 2%">
                    <div class="col-md-6">
                        <div class="labelCampo text-center">
                            <h5><span>FECHA DE CARGA</span></h5>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <dx:ASPxDateEdit ID="dtFecha" Theme="ThemeLTH" runat="server" CalendarProperties-HighlightToday="true" OnDateChanged="dtFecha_DateChanged" AutoPostBack="true">
                            <ClientSideEvents ValueChanged="function(s, e){
                               
                               

                            }" />

                        </dx:ASPxDateEdit>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12 text-center">
                    <div id="alertaEnvio" class="alert alert-warning alert-dismissible fade show text-center" role="alert" style="display: none">
                        <strong>Ventas cargadas</strong> La venta de la fecha seleccionada fue cargada correctamente
                      <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                          <span aria-hidden="true">&times;</span>
                      </button>
                    </div>
</div>
                    <div class="col-sm-12 text-center " style="color:#343a40; margin-top:3%">
                        <h4>Carga Ventas por centro de servicio</h4>
                    </div>
                    <table class="table table-striped">
                        <thead>

                            <tr>
                                <th scope="col">Clave</th>
                                <th scope="col">Nombre centro</th>
                                <th scope="col">Venta Promedio Diaria</th>
                                <th scope="col">Venta diaria</th>
                            </tr>
                        </thead>
                        <tbody>
                            <%foreach (var item in lstVentas)
                                {
                                    if (item.clave != "Mayoreo")
                                    {%>
                            <tr>
                                <th scope="row"><%:item.clave %></th>
                                <td>
                                    <h5><%:item.nombre%></h5>
                                </td>
                                <td>
                                    <input id="ventaProm<%:item.clave %>" placeholder="Venta promedio diaria" class="form-control VentaPromCS" type="number" value="<%:item.ventaPromedio %>" />
                                </td>
                                <td style="display: none">
                                    <!--Venta Promedio diaria -->
                                    <input id="prom<%:item.clave %>" placeholder="Venta promedio normal" class="form-control " type="number" value="<%:item.ventaPromedio %>" />
                                </td>
                                <td>
                                    <input id="<%:item.clave %>" placeholder="Venta" class="form-control valorVenta" type="number" value="<%:item.venta %>" />
                                </td>
                            </tr>
                            <%}
                                }%>
                        </tbody>
                    </table>
                </div>

                <div class="row">
                     <div class="col-sm-12 text-center " style="color:#343a40; margin-top:3%">
                        <h4>Carga ventas por mayoreo</h4>
                    </div>
                    <table class="table table-striped">
                        <thead>

                            <tr>
                                <th scope="col">Clave</th>
                                <th scope="col">Venta Mayoreo</th>
                                <th scope="col">Venta Promedio Diaria</th>
                                <th scope="col">Venta diaria</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <th scope="row">#</th>
                                <td>
                                    <h5>Venta Mayoreo</h5>
                                </td>
                              

                                <td>
                                    <input id="VentaMayoreoTotal" placeholder="Venta mayoreo" class="form-control " type="number" value="<%:mayoreo %>" /></td>
                                <td>
                                    <%if (lstVentas.Where(x => x.clave == "Mayoreo").Count() == 1)
                                        { %>
                                    <input id="Mayoreo" placeholder="Venta" class="form-control valorVenta" type="number" value="<%:lstVentas.First(x => x.clave == "Mayoreo").venta %>" />
                                    <%}
                                        else
                                        {%>
                                    <input id="Mayoreo" placeholder="Venta" class="form-control valorVenta" type="number" value="0" />
                                    <%} %>
                                </td>

                            </tr>


                        </tbody>

                    </table>
                </div>

            </div>
            <div class="row" style="margin-top: 3%">
                <div class="col-sm-12  align-content-end">
                    <button type="button" class="btn btn-success float-right" onclick="sendVenta()">Guardar</button>
                </div>
            </div>
        </div>

        <script>

            $(document).ready(function () {

                $("#alertaEnvio").hide();
                //called when key is pressed in textbox
                $(".valorVenta").keypress(function (e) {
                    //if the letter is not digit then display error and don't type anything
                    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                        //display error message

                        return false;
                    }
                });
            });

            function sendVenta() {
                $('.alert').alert();
                var arrVenta = new Array();

                $('.valorVenta').each(function () {
                    var cla = $(this).attr('id');
                    //console.log(cla);
                    //console.log($("#prom" + cla).val());
                    //ventaPromedio = $("#prom" + cla).val();
                    ventaPromedio = $("#ventaProm" + cla).val();

                    var obj = {
                        Clave: $(this).attr('id'),
                        ValorProm: ventaPromedio,
                        Valor: $(this).val()
                    }
                    arrVenta.push(obj);
                });
                var objMayoreo = {
                    Clave: 'VentaMayoreoTotal',
                    ValorProm: $("#VentaMayoreoTotal").val(),
                    Valor: $("#VentaDiariaPromedio").val()
                }
                arrVenta.push(objMayoreo);

                $.ajax({
                    type: 'POST',
                    url: 'CargaVenta.aspx/sendValor',
                    async: false,
                    cache: false,
                    data: "{Data:'" + JSON.stringify(arrVenta) + "'}",
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    error: function (xmlHttpRequest, textStatus, errorThrown) {
                      
                    },
                    success: function (msgJ) {
                     
                        $("#alertaEnvio").show();

                    }
                });
            }
        </script>

    </div>
</asp:Content>
