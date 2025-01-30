<%@ Page Title="" Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="TablaFinanciera.aspx.cs" Inherits="CatalogosLTH.Web.TablaFinanciera" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="row" style="margin-top: 13rem">
            <div class="row">
                <div class="col-sm-12">
                    <h4>Tabla Financiera</h4>
                </div>
                <div class="col-sm-2">
                    Distribuidor:
                </div>
                <div class="col-sm-4">
                    <select id="distribuidores">
                    </select>
                </div>

            </div>
            <br />
            <table border="1" class="table table-bordered table-striped  table-hover">
                <thead>
                    <tr>
                        <th>Area</th>
                        <th>Objetivo y Formula</th>
                        <th>Formula</th>
                        <th>Datos</th>
                        <th>Respuesta</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td rowspan="2">Margen de utilidad bruta</td>
                        <td rowspan="2">Utilidad Bruta/Ventas Netas El objetivo es obtener como mínimo 19%<-::: [ Utilidad Bruta / Ventas Netas ]</td>
                        <td>U BRUTA </td>
                        <td>
                            <input type="text" id="ubruta" value="0" onkeyup="Calc1()" step="0.00001" /></td>
                        <td rowspan="2">
                            <input id="r1" type="text" style="font-size: 18px; font-weight: 400;" step="0.00001" />
                        </td>
                    </tr>
                    <tr>
                        <td>V NETAS</td>
                        <td>
                            <input type="text" id="vneta" value="0" onkeyup="Calc1()" step="0.00001" /></td>
                    </tr>
                    <!-- CALC 2-->
                    <tr>
                        <td rowspan="2">Margen de utilidad operativa</td>
                        <td rowspan="2">El objetivo es obtener como mínimo 8%... [Utilidad operativa / Ventas netas]</td>
                        <td>U Operativa</td>
                        <td>
                            <input type="text" id="uoperativa" value="0" onkeyup="Calc2()" /></td>
                        <td rowspan="2">
                            <input id="r2" type="text" style="font-size: 18px; font-weight: 400;" />
                        </td>
                    </tr>
                    <tr>
                        <td>V NETAS</td>
                        <td>
                            <input type="text" id="vneta2" value="0" onkeyup="Calc2()" /></td>
                    </tr>
                    <!--CALC 3-->
                    <tr>
                        <td rowspan="2">Liquidez (razón circulante)</td>
                        <td rowspan="2">El objetivo para este indicador es de 1.85... [Activos circulares / Pasivos circulares]</td>
                        <td>A CIRC</td>
                        <td>
                            <input type="text" id="acirc" value="0" onkeyup="Calc3()" /></td>
                        <td rowspan="2">
                            <input id="r3" type="text" style="font-size: 18px; font-weight: 400;" />
                        </td>
                    </tr>
                    <tr>
                        <td>PAS CIRC</td>
                        <td>
                            <input type="text" id="pcirc" value="0" onkeyup="Calc3()" /></td>
                    </tr>
                    <!-- CALC 4 -->
                    <tr>
                        <td rowspan="2">Liquidez (razón rápida)</td>
                        <td rowspan="2">El objetivo del indicador es de 1.35...([Activos circulantes - Inventario] / Pasivos circulantes)</td>
                        <td>A CIRC - INV</td>
                        <td>
                            <input type="text" id="acinv" value="0" onkeyup="Calc4()" /></td>
                        <td rowspan="2">
                            <input id="r4" type="text" style="font-size: 18px; font-weight: 400;" />
                        </td>
                    </tr>
                    <tr>
                        <td>PAS CIRC</td>
                        <td>
                            <input type="text" id="pcirc2" value="0" onkeyup="Calc4()" /></td>
                    </tr>
                    <!-- CALC 5 -->
                    <tr>
                        <td rowspan="2">Rotación de inventario (unidades en dias)</td>
                        <td rowspan="2">Ojetivo de 15 a 21 dias <- ::: [(Inventario / Costo de Ventas)(Dias del periodo)]</td>
                        <td>INVENTARIO / CV </td>
                        <td>
                            <input type="text" id="inv" value="0" onkeyup="Calc5()" /></td>
                        <td rowspan="2">
                            <input id="r5" type="text" style="font-size: 18px; font-weight: 400;" />
                        </td>
                    </tr>
                    <tr>
                        <td>(DIAS PERIODO)</td>
                        <td>
                            <input type="text" id="cvdias" value="0" onkeyup="Calc5()" /></td>
                    </tr>
                    <!-- CALC 6 -->
                    <tr>
                        <td rowspan="2">Periodo promedio de cobro (unidades en dias)</td>
                        <td rowspan="2">Objetivo de 30 dias <-::: [(Cuentas por cobrar / Ventas a crédito) (Dias del periodo)]</td>
                        <td>CXC / (VTS CRED)</td>
                        <td>
                            <input type="text" id="cxc" value="0" onkeyup="Calc6()" /></td>
                        <td rowspan="2">
                            <input id="r6" type="text" style="font-size: 18px; font-weight: 400;" />
                        </td>
                    </tr>
                    <tr>
                        <td>(DIAS PERIODO)</td>
                        <td>
                            <input type="text" id="vcredper" value="0" onkeyup="Calc6()" /></td>
                    </tr>
                    <!-- CALC 7 -->
                    <tr>
                        <td rowspan="2">Periodo promedio de pago (unidades en dias)</td>
                        <td rowspan="2">Objetivo de 30 dias <-::: [(Cuentas por pagar / Costo de ventas) (Dias del periodo)]</td>
                        <td>CXP / (CV)</td>
                        <td>
                            <input type="text" id="cxp" value="0" onkeyup="Calc7()" /></td>
                        <td rowspan="2">
                            <input id="r7" type="text" style="font-size: 18px; font-weight: 400;" />
                        </td>
                    </tr>
                    <tr>
                        <td>(DIAS PERIODO)</td>
                        <td>
                            <input type="text" id="cvper" value="0" onkeyup="Calc7()" /></td>
                    </tr>
                    <!-- CALC 8 -->
                    <tr>
                        <td rowspan="2">Endeudamiento</td>
                        <td rowspan="2">El objetivo es estar por debajo del 40% <-::: [Pasivo total / Activo total]</td>
                        <td>PASITVO TOT</td>
                        <td>
                            <input type="text" id="ptot" value="0" onkeyup="Calc8()" /></td>
                        <td rowspan="2">
                            <input id="r8" type="text" style="font-size: 18px; font-weight: 400;" />
                        </td>
                    </tr>
                    <tr>
                        <td>ACTIVO TOT</td>
                        <td>
                            <input type="text" id="atot" value="0" onkeyup="Calc8()" /></td>
                    </tr>
                    <!-- CALC 9 -->
                    <tr>
                        <td rowspan="2">Gastos operativos</td>
                        <td rowspan="2">El objetivo del indicador está entre 9% y 11% <-::: [Gastos operativos / Ventas netas] </td>
                        <td>GAST OPER</td>
                        <td>
                            <input type="text" id="goper" value="0" onkeyup="Calc9()" /></td>
                        <td rowspan="2">
                            <input id="r9" type="text" style="font-size: 18px; font-weight: 400;" />
                        </td>
                    </tr>
                    <tr>
                        <td>VTS NETAS</td>
                        <td>
                            <input type="text" id="vnet" value="0" onkeyup="Calc9()" /></td>
                    </tr>
                    <!--  CALC 10 -->
                    <tr>
                        <td rowspan="2">Rendimiento sobre activos operativos</td>
                        <td rowspan="2">El objetivo mínimo es un 40% <-::: [(Utilidad Operativa) / (Activo total -  Pasivo Total)]</td>
                        <td>UTI OPER</td>
                        <td>
                            <input type="text" id="uoper" value="0" onkeyup="Calc10()" /></td>
                        <td rowspan="2">
                            <input id="r10" type="text" style="font-size: 18px; font-weight: 400;" />
                        </td>
                    </tr>
                    <tr>
                        <td>(ACT TOT- PAS TOT)</td>
                        <td>
                            <input type="text" id="atotpas" value="0" onkeyup="Calc10()" /></td>
                    </tr>
                </tbody>
            </table>
            <div class="row">
                <label>Carga archivos</label>
                <input type="file" id="fileTabla" <%=completada == true || estatus == "En revisión" ? "disabled" : ""%> />
                <br />
                <a id="linkarchivo"></a>
            </div>
        </div>

        <div class="row">
            <%if (completada != true && estatus == null || estatus == "")
                {%>
            <div class="col-sm-2" style="width: unset !important">
                <input type="button" onclick="guardar()" value="Guardar" class="btn btn-success" />
            </div>
            <%}  %>
            <%if ((tipo == "GerenteCuenta" || tipo == "Admin") && completada == false)
                {%>
            <div class="col-sm-6" style="text-align: start">
                <input type="button" value="Aprobar" id="btnCompletar" />
            </div>
            <%} %>
        </div>

    </div>

    <script>


        $(document).ready(function () {
            var params = new URLSearchParams(window.location.search);

            // Obtener el valor de un parámetro específico
            var a = params.get('a'); // Devuelve "valor1"
            var t = params.get('t');

            //console.log(a);
            //console.log(t);


            if (a != null & a != "") {
                //console.log(a);
                traeTabla(a);

            }
            else if (t != null && t != "") {

                traeTablaId(t);

            } else {
                $.ajax({
                    url: '<%=ResolveUrl("TablaFinanciera.aspx/traeDistribuidores")%>',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    type: 'POST',
                    async: false,
                    success: function (response) {
                        var select = $('#distribuidores');
                        select.empty();
                        var dist = JSON.parse(response.d);

                        $.each(dist, function (i) {
                            select.append($("<option></option>").attr("value", dist[i].id).text(dist[i].nombre));
                        });

                    }
                });
            }

        });

    function traeTabla(a) {
        $.ajax({
            url: '<%=ResolveUrl("TablaFinanciera.aspx/traeTabla")%>',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            type: 'POST',
            async: false,
            data: JSON.stringify({ a: a }),
            success: function (response) {
                var json = JSON.parse(response.d);

                if (json != -1) {
                    console.log(json);
                    $('#r1').val(json.r1);
                    $('#r2').val(json.r2);
                    $('#r3').val(json.r3);
                    $('#r4').val(json.r4);
                    $('#r5').val(json.r5);
                    $('#r6').val(json.r6);
                    $('#r7').val(json.r7);
                    $('#r8').val(json.r8);
                    $('#r9').val(json.r9);
                    $('#r10').val(json.r10);

                    $('#ubruta').val(json.ubruta); $('#vneta').val(json.vneta);
                    $('#uoperativa ').val(json.uoperativa); $('#vneta2').val(json.vneta2);
                    $('#acirc').val(json.acirc); $('#pcirc').val(json.pcirc);
                    $('#acinv').val(json.acinv); $('#pcirc2').val(json.pcirc2);
                    $('#inv').val(json.inv); $('#cvdias').val(json.cvdias);
                    $('#cxc').val(json.cxc); $('#vcredper').val(json.vcredper);
                    $('#cxp').val(json.cxp); $('#cvper').val(json.cvper);
                    $('#ptot').val(json.ptot); $('#atot').val(json.atot);
                    $('#goper').val(json.goper); $('#vnet').val(json.vnet);
                    $('#uoper').val(json.uoper); $('#atotpas').val(json.atotpas);

                    $('#linkarchivo').attr('href', '');
                    $('#linkarchivo').attr('target', '_blank');
                    $('#linkarchivo').text('');

                    $('#linkarchivo').attr('href', json.url);
                    $('#linkarchivo').attr('target', '_blank');
                    $('#linkarchivo').text(json.nombre);
                }
            }
        });
    }

    function traeTablaId(t) {
        $.ajax({
            url: '<%=ResolveUrl("TablaFinanciera.aspx/traeTablaID")%>',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            type: 'POST',
            async: false,
            data: JSON.stringify({ t: t }),
            success: function (response) {
                var json = JSON.parse(response.d);

                if (json != -1) {
                    console.log(json);

                    var select = $('#distribuidores');
                    select.empty();
                    select.append($("<option selected></option>").text(json.distribuidor));
                    select.attr('disabled', true);

                    $('#r1').val(json.r1);
                    $('#r2').val(json.r2);
                    $('#r3').val(json.r3);
                    $('#r4').val(json.r4);
                    $('#r5').val(json.r5);
                    $('#r6').val(json.r6);
                    $('#r7').val(json.r7);
                    $('#r8').val(json.r8);
                    $('#r9').val(json.r9);
                    $('#r10').val(json.r10);

                    $('#ubruta').val(json.ubruta); $('#vneta').val(json.vneta);
                    $('#uoperativa ').val(json.uoperativa); $('#vneta2').val(json.vneta2);
                    $('#acirc').val(json.acirc); $('#pcirc').val(json.pcirc);
                    $('#acinv').val(json.acinv); $('#pcirc2').val(json.pcirc2);
                    $('#inv').val(json.inv); $('#cvdias').val(json.cvdias);
                    $('#cxc').val(json.cxc); $('#vcredper').val(json.vcredper);
                    $('#cxp').val(json.cxp); $('#cvper').val(json.cvper);
                    $('#ptot').val(json.ptot); $('#atot').val(json.atot);
                    $('#goper').val(json.goper); $('#vnet').val(json.vnet);
                    $('#uoper').val(json.uoper); $('#atotpas').val(json.atotpas);

                    $('#linkarchivo').attr('href', '');
                    $('#linkarchivo').attr('target', '_blank');
                    $('#linkarchivo').text('');

                    $('#linkarchivo').attr('href', json.url);
                    $('#linkarchivo').attr('target', '_blank');
                    $('#linkarchivo').text(json.nombre);
                }
            }
        });
    }

    function salir() {
        window.location.replace("/HistorialTablaFinanciera.aspx");
    }

    function guardar() {
        var params = new URLSearchParams(window.location.search);

        // Obtener el valor de un parámetro específico
        var aud = params.get('a'); // Devuelve "valor1"
        var tbl = params.get('t');

        if (aud == undefined || aud == null) {
            aud = "-1";
        }
        if (tbl == undefined || tbl == null) {
            tbl = "-1";
        }
        console.log(aud);
        console.log(tbl);


        var r1 = $('#r1').val();
        var r2 = $('#r2').val();
        var r3 = $('#r3').val();
        var r4 = $('#r4').val();
        var r5 = $('#r5').val();
        var r6 = $('#r6').val();
        var r7 = $('#r7').val();
        var r8 = $('#r8').val();
        var r9 = $('#r9').val();
        var r10 = $('#r10').val();

        var ubruta = $('#ubruta').val(); var vneta = $('#vneta').val();
        var uoperativa = $('#uoperativa').val(); var vneta2 = $('#vneta2').val();
        var acirc = $('#acirc').val(); var pcirc = $('#pcirc').val();
        var acinv = $('#acinv').val(); var pcirc2 = $('#pcirc2').val();
        var inv = $('#inv').val(); var cvdias = $('#cvdias').val();
        var cxc = $('#cxc').val(); var vcredper = $('#vcredper').val();
        var cxp = $('#cxp').val(); var cvper = $('#cvper').val();
        var ptot = $('#ptot').val(); var atot = $('#atot').val();
        var goper = $('#goper').val(); var vnet = $('#vnet').val();
        var uoper = $('#uoper').val(); var atotpas = $('#atotpas').val();

        var campos = [r1, r2, r3, r4, r5, r6, r7, r8, r9, r10, ubruta, vneta, uoperativa, vneta2, acirc, pcirc, acinv, pcirc2, inv, cvdias, cxc, vcredper, cxp, cvper, ptot, atot, goper, vnet, uoper, atotpas];

        var completos = validaCampos(campos);

        if (completos == true) {
            var fileInput = document.getElementById('fileTabla');
            var file = fileInput.files[0];

            console.log(file);

            if (file != null) {
                var reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onloadend = function () {
                    var b64 = reader.result.split("base64,")[1];
                    var filename = file.name;
                    console.log('Archivo convertido a Base64:', b64);
                    console.log(filename);

                    ajaxTabla(r1, r2, r3, r4, r5, r6, r7, r8, r9, r10, ubruta, vneta, uoperativa, vneta2, acirc, pcirc, acinv, pcirc2, inv, cvdias, cxc, vcredper, cxp, cvper, ptot, atot, goper, vnet, uoper, atotpas, b64, filename, completos, aud, tbl);
                };
            } else {
                var b64 = "";
                var filename = "";

                ajaxTabla(r1, r2, r3, r4, r5, r6, r7, r8, r9, r10, ubruta, vneta, uoperativa, vneta2, acirc, pcirc, acinv, pcirc2, inv, cvdias, cxc, vcredper, cxp, cvper, ptot, atot, goper, vnet, uoper, atotpas, b64, filename, completos, aud, tbl);
            }
        } else {
            Swal.fire({
                title: 'Tabla no guardada',
                text: 'Completa todos los campos',
                icon: 'warning',

                confirmButtonText:
                  '<i class="fa fa-thumbs-up"></i> ok!',
                confirmButtonAriaLabel: 'Ok!',

            })
        }


    }

    function ajaxTabla(r1, r2, r3, r4, r5, r6, r7, r8, r9, r10, ubruta, vneta, uoperativa, vneta2, acirc, pcirc, acinv, pcirc2, inv, cvdias, cxc, vcredper, cxp, cvper, ptot, atot, goper, vnet, uoper, atotpas, b64, filename, completos, aud, tbl) {
        var distribuidor = $('#distribuidores option:selected').text();
        $.ajax({
            url: '<%=ResolveUrl("TablaFinanciera.aspx/guardaTabla")%>',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                type: 'POST',
                async: false,
                data: JSON.stringify({
                    r1: r1, r2: r2, r3: r3, r4: r4, r5: r5, r6: r6, r7: r7, r8: r8, r9: r9, r10: r10,
                    ubruta: ubruta, vneta: vneta, uoperativa: uoperativa, vneta2: vneta2, acirc: acirc, pcirc: pcirc, acinv: acinv, pcirc2: pcirc2, inv: inv, cvdias: cvdias,
                    cxc: cxc, vcredper: vcredper, cxp: cxp, cvper: cvper, ptot: ptot, atot: atot, goper: goper, vnet: vnet, uoper: uoper, atotpas: atotpas, archivo: b64, nombre: filename, completo: completos, dist: distribuidor, aud: aud, tbl: tbl
                }),
                success: function (response) {
                    var resp = JSON.parse(response.d);
                    if (resp != -1) {
                        // alert("TABLA FINANCIERA GUARDADA");
                        $('#linkarchivo').attr('href', '');
                        $('#linkarchivo').attr('target', '_blank');
                        $('#linkarchivo').text('');

                        $('#linkarchivo').attr('href', resp.url);
                        $('#linkarchivo').attr('target', '_blank');
                        $('#linkarchivo').text(resp.nombre);

                        Swal.fire({
                            title: 'Tabla guardada',
                            text: 'La tabla financiera se ha guardado exitosamente',
                            icon: 'success',

                            confirmButtonText:
                              '<i class="fa fa-thumbs-up"></i> ok!',
                            confirmButtonAriaLabel: 'Ok!',
                            onClose: redirect(resp.idtabla)

                        })
                    }

                }
            });
        }

        function redirect(id) {
            // Obtener la URL actual
            //let currentUrl = window.location.href;

            //// Crear un objeto URL para manipular más fácilmente
            //let url = new URL(currentUrl);

            //// Añadir o actualizar el parámetro en la query string
            //url.searchParams.set("t", id);

            //// Recargar la página con la nueva URL
            //window.location.href = url.toString();

            location.href = "historialtablafinanciera.aspx";
        }

        function validaCampos(campos) {

            var pasa = false;
            var count = 0;

            campos.forEach(function (i) {
                if (i == null || i == "" || i == "0") {

                    count++;
                }
            });

            if (count == 30) {

                return false;

            } else {

                return true;
            }

            //if (r1 == null && (r1 == "" || r1 == "0") && r2 == null && (r2 == "" || r2 == "0") && r3 == null && (r3 == "" || r3 == "0") && r4 == null && (r4 == "" || r4 == "0") && r5 == null && (r5 == "" || r5 == "0") && r6 == null && (r6 == "" || r6 == "0") && r7 == null && (r7 == "" || r7 == "0") && r8 == null && (r8 == "" || r8 == "0") && r9 == null && (r9 == "" || r9 == "0") && r10 == null && (r10 == "" || r10 == "0") && ubruta == null && (ubruta == "0" || ubruta == "") && vneta == null && (vneta == "0" || vneta == "") && uoperativa == null && (uoperativa == "0" || uoperativa == "") && vneta2 == null && (vneta2 == "0" || vneta2 == "") && acirc == null && (acirc == "0" || acirc == "") && pcirc == null && (pcirc == "0" || pcirc == "") && acinv == null && (acinv == "0" || acinv == "") && pcirc2 == null && (pcirc2 == "0" || pcirc2 == "") && inv == null && (inv == "0" || inv == "") && cvdias == null && (cvdias == "0" || cvdias == "") && cxc == null && (cxc == "0" || cxc == "") && vcredper == null && (vcredper == "0" || vcredper == "") && cxp == null && (cxp == "0" || cxp == "") && cvper == null && (cvper == "0" || cvper == "") && ptot == null && (ptot == "0" || ptot == "") && atot == null && (atot == "0" || atot == "") && goper == null && (goper == "0" || goper == "") && vnet == null && (vnet == "0" || vnet == "") && uoper == null && (uoper == "0" || uoper == "") && atotpas == null && (atotpas == "0" || atotpas == "")) {

            //    pasa = false;
            //    return pasa;

            //} else {
            //    return true;
            //}
        }

        function Calc1() {

            var u = $('#ubruta').val();
            var v = $('#vneta').val();
            if (v > 0) {
                var res = (u / v) * 100;
                var f = new Number(res);
                console.log(f.toFixed(2));
                $('#r1').val(f.toFixed(3) + "%");
            }
        }
        function Calc2() {
            var u = $('#uoperativa').val();
            var v = $('#vneta2').val();
            if (v > 0) {
                var res = u / v * 100;
                var f = new Number(res);
                console.log(f.toFixed(2));
                $('#r2').val(f.toFixed(3) + "%");
            }
        }
        function Calc3() {
            var u = $('#acirc').val();
            var v = $('#pcirc').val();
            if (v > 0) {
                var res = u / v;
                var f = new Number(res);
                console.log(f.toFixed(2));
                $('#r3').val(f.toFixed(3));
            }
        }
        function Calc4() {
            var u = $('#acinv').val();
            var v = $('#pcirc2').val();
            if (v > 0) {
                var res = u / v;
                var f = new Number(res);
                console.log(f.toFixed(2));
                $('#r4').val(f.toFixed(3));
            }
        }
        function Calc5() {
            var u = $('#inv').val();
            var v = $('#cvdias').val();
            if (v > 0) {
                var res = u * v;
                var f = new Number(res);
                console.log(f.toFixed(2));
                $('#r5').val(f.toFixed(3));
            }
        }
        function Calc6() {
            var u = $('#cxc').val();
            var v = $('#vcredper').val();
            if (v > 0) {
                var res = u * v;
                var f = new Number(res);
                console.log(f.toFixed(2));
                $('#r6').val(f.toFixed(3));
            }
        }
        function Calc7() {
            var u = $('#cxp').val();
            var v = $('#cvper').val();
            if (v > 0) {
                //var res = (u / v) * 360;
                var res = (u * v);
                var f = new Number(res);
                console.log(f.toFixed(2));
                $('#r7').val(f.toFixed(3));
            }
        }
        function Calc8() {
            var u = $('#ptot').val();
            var v = $('#atot').val();
            if (v > 0) {
                var res = u / v * 100;
                var f = new Number(res);
                console.log(f.toFixed(2));
                $('#r8').val(f.toFixed(3) + "%");
            }
        }
        function Calc9() {
            var u = $('#goper').val();
            var v = $('#vnet').val();
            if (v > 0) {
                var res = u / v * 100;
                var f = new Number(res);
                console.log(f.toFixed(2));
                $('#r9').val(f.toFixed(3) + "%");
            }
        }
        function Calc10() {
            var u = $('#uoper').val();
            var v = $('#atotpas').val();
            if (v > 0) {
                var res = u / v * 100;
                var f = new Number(res);
                console.log(f.toFixed(2));
                $('#r10').val(f.toFixed(3) + "%");
            }
        }

        $("#btnCompletar").click(function () {

            var params = new URLSearchParams(window.location.search);

            // Obtener el valor de un parámetro específic
            var t = params.get('t');

            if (t != null && t != "") {

                $.ajax({
                    url: '<%=ResolveUrl("TablaFinanciera.aspx/completaTabla")%>',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    type: 'POST',
                    async: false,
                    data: JSON.stringify({ idtbl: t }),
                    success: function (response) {
                        var json = JSON.parse(response.d);

                        if (json != -1) {

                            Swal.fire({
                                title: 'Tabla completada con exito',
                                text: 'Se ha marcado esta tabla como "completada" exitosamente.',
                                icon: 'success',

                                confirmButtonText:
                                  '<i class="fa fa-thumbs-up"></i> ok!',
                                confirmButtonAriaLabel: 'Ok!',

                            })

                            window.redirect("HistorialTablaFinanciera.aspx");
                        }
                    }
                });

            }

        });

    </script>
</asp:Content>
