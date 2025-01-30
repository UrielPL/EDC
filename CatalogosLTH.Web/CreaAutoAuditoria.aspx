<%@ Page Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="CreaAutoAuditoria.aspx.cs" Inherits="CatalogosLTH.Web.CreaAutoAuditoria" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .cd-accordion__input {
            position: absolute;
            opacity: 0;
        }

        .cd-accordion__label {
            position: relative;
            display: flex;
            align-items: center;
            padding: var(--space-sm) var(--space-md);
            background: var(--cd-color-1);
            box-shadow: inset 0 -1px lightness(var(--cd-color-1), 1.2);
            background-color: #003f5c;
            padding: 7px;
            border-radius: 7px;
            box-shadow: 2px 2px 4px #575757;
            color: white;
        }

        .cd-accordion__sub {
            display: none;
        }

        .cd-accordion__input:checked ~ .cd-accordion__sub {
            display: block;
        }

        .swal2-popup {
            font-size: 1.6rem !important;
        }

        .divCondicion {
            align-items: center;
            justify-content: center;
            box-shadow: 3px 3px 7px darkgray;
            border-radius: 7px;
            margin: 7px;
        }

        .div-superpuesto {
            position: absolute;
            top: 0;
            left: 0;
            background-color: rgba(255, 0, 0, 0.5); /* Fondo rojo semi-transparente */
            width: 129px;
            height: 38px;
            margin-top: 40px;
        }

        .div-superpuesto-a {
            position: absolute;
            top: 0;
            left: 0;
            background-color: rgba(255, 0, 0, 0.5); /* Fondo rojo semi-transparente */
            width: 178px;
            height: 38px;
            margin-top: 40px;
            margin-left: 134px;
        }

        @media (min-width:768px) {
            .col-sm-1 {
                width: unset;
            }
        }
    </style>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container" style="margin-top: 40px">
        <div class="panel-body" style="margin-top: 8rem" id="contenido">
            <div class="row" style="margin: 10px">
                <h1>Auditoria</h1>
                <div class="col-sm-2">
                    Distribuidor:
                </div>
                <div class="col-sm-4">

                    <asp:DropDownList ID="drpDist" runat="server"></asp:DropDownList>
                </div>

                <div class="col-sm-1"></div>
                <div class="col-sm-2" style="display: flex">
                    <input type="button" onclick="clickExpand()" style="display: flex" value="Expandir" />
                </div>
                <div class="col-sm-2" style="display: flex">
                    <input type="button" onclick="clickCheck()" style="display: flex" value="Selecciona todo" />
                </div>
                
                <div class="col-sm-2" style="margin-top: 1rem;">
                    Figura responsable:
                </div>
                
                <div class="col-sm-4" style="margin-top: 1rem;">

                    <select class="form-select" aria-label="Default select example" id="responsables" onchange="chResponsable()">
                        <option value="12" selected>TODOS</option>
                        <option value="0">GERENTE COMERCIAL</option>
                        <option value="1">CENTRO DE SERVICIO</option>
                        <option value="2">ENCARGADO DE ALMACEN</option>
                        <option value="3">TECNICO MASTER</option>
                        <option value="4">IMPULSOR DE MERCADO</option>
                        <option value="5">ENCARGADO LOGISTICA</option>
                        <option value="6">GERENTE DE COBRANZA</option>
                        <option value="7">GERENTE DE FINANZAS</option>
                        <option value="8">ENCARGADO RH</option>
                        <option value="9">TECNICO CS</option>
                        <option value="10">CS</option>
                        <option value="11">EJECUTIVO DE DESARROLLO</option>
                        <option value="12">DIRECTOR GENERAL</option>
                    </select>
                </div>
                <div class="col-sm-2" style="display: flex; margin-top: 1rem;">
                    <input type="button" onclick="descargaExcel()" style="display: flex" value="Descarga Excel" />
                </div>

            </div>

            <%  foreach (var pilar in vpilar)
                { %>

            <ul class="cd-accordion margin-top-lg margin-bottom-lg" style="list-style: none">
                <li class="cd-accordion__item cd-accordion__item--has-children">
                    <input class="cd-accordion__input" type="checkbox"
                        name="<%=pilar.Nombre%>" id="<%=pilar.Nombre%>">
                    <label class="cd-accordion__label cd-accordion__label--icon-folder"
                        for="<%=pilar.Nombre%>" style="background-color: #37465b">
                        <span><%=(pilar.Id + ". "+pilar.Nombre)%></span></label>

                    <!-- NOMBRE DE PILAR -->

                    <%foreach (var area in pilar.Areas)
                        { %>
                    <ul class="cd-accordion__sub cd-accordion__sub--l1" style="list-style: none;">
                        <li class="cd-accordion__item cd-accordion__item--has-children">
                            <input class="cd-accordion__input" type="checkbox"
                                name="<%=area.Nombre %>" id="<%=area.Nombre %>">
                            <label class="cd-accordion__label cd-accordion__label--icon-folder"
                                for="<%=area.Nombre %>">
                                <span><%=pilar.Id+"."+area.Id+" "+area.Nombre %></span></label>
                            <!-- AREA -->

                            <%foreach (var subtema in area.Subtemas)
                                {  %>
                            <ul class="cd-accordion__sub cd-accordion__sub--l2" style="list-style: none;">
                                <li class="cd-accordion__item cd-accordion__item--has-children">
                                    <input class="cd-accordion__input" type="checkbox" name="sub<%=subtema.Nombre %>" id="sub<%=subtema.Nombre %>">
                                    <label class="cd-accordion__label cd-accordion__label--icon-folder" for="sub<%=subtema.Nombre %>" style="background-color: #08C6AB"><span><%=pilar.Id+"."+area.Id+"."+subtema.Id+" "+subtema.Nombre %></span></label>
                                    <!-- Subtema -->

                                    <%foreach (var punto in puntosDetalle.Where(c => c.idsubtema == subtema.Id))
                                        {%>
                                    <%--<div> x</div>--%>
                                    <ul class="cd-accordion__sub cd-accordion__sub--l3" style="list-style: none;">
                                        <li id="li<%=punto.idpunto %>" class="cd-accordion__item cd-accordion__item--has-children lipunto ">
                                            <div class="row" style="display: flex">
                                                <div class="col-sm-7" style="display: flex">
                                                    <input class="cd-accordion__input" type="checkbox"
                                                        name="<%=punto.idpunto%>" id="<%=punto.idpunto%>">
                                                    <label class="cd-accordion__label cd-accordion__label--icon-folder"
                                                        for="<%=punto.idpunto%>" style="background-color: #726eff; width: 100%">
                                                        <span><%=punto.idpunto+". "+punto.nombrepunto%></span></label>
                                                </div>
                                                <div class="col-sm-1 divCondicion" style="display: flex; align-items: center; justify-content: center;">
                                                    <i class="fa-solid fa-question" style="cursor: pointer;" onclick="abreCondiciones(<%=punto.idpunto%>,'<%=punto.nombrepunto%>')"></i>
                                                </div>
                                                <div class="col-sm-1" style="display: flex">
                                                    <div class="switch" style="display: flex; align-items: center">
                                                        <input name="califa<%:punto.idpunto %>"
                                                            id="cmn-toggle-<%:punto.idpunto %>"
                                                            class="cmn-toggle cmn-toggle-round"
                                                            type="checkbox" onchange="focusComentario(this)">
                                                        <label for="cmn-toggle-<%:punto.idpunto %>"
                                                            name="califal<%:punto.idpunto %>">
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="col-sm-1" style="display: flex; align-items: center; justify-content: center;">
                                                    <div class="noAplica">
                                                        <%if (punto.habilitana == true)
                                                            { %>
                                                            <input type="checkbox" key="na" id="na-<%:punto.idpunto %>" name="na<%:punto.idpunto %>" style="font-size: 12px;" onchange="checkna(this)"/><span style="font-weight: bold">N/A</span>
                                                        <%} %>
                                                    </div>
                                                </div>
                                                <div id="divComentarios" class="col-sm-1" style="display: flex; align-items: center; justify-content: center;">
                                                    <div class="coment">
                                                        <input type="text" id="coments<%:punto.idpunto %>" name="com<%:punto.idpunto %>" placeholder="Comentarios" />
                                                    </div>
                                                </div>
                                            </div>


                                            <!-- Puntos -->
                                        </li>
                                    </ul>


                                    <%    } %>


                                    <%--<%foreach (var punto in subtema.Puntos)
                                    {
                                %>
                                <ul class="cd-accordion__sub cd-accordion__sub--l3">
                                    <li class="cd-accordion__item cd-accordion__item--has-children">
                                        <input class="cd-accordion__input" type="checkbox" name="<%=punto.NombrePunto%>" id="<%=punto.NombrePunto%>">
                                        <label class="cd-accordion__label cd-accordion__label--icon-folder" for="<%=punto.NombrePunto%>"><span><%=punto.NombrePunto%></span></label>
                                        <div class="switch" style="display: flex;">
                                            <input name="califa<%:punto.Id %>" id="cmn-toggle-<%:punto.Id %>" class="cmn-toggle cmn-toggle-round" type="checkbox" <%:(punto.Valor==1)?"checked":"" %>>
                                            <label for="cmn-toggle-<%:punto.Id %>" name="califal<%:punto.Id %>"></label>
                                        </div>
                                        <!-- Puntos -->
                                    </li>
                                </ul>
                                <%} %>--%>
                                </li>
                            </ul>
                            <% } %>
                        </li>
                    </ul>
                    <% } %>
                </li>

            </ul>
            <!-- cd-accordion -->
            <%} %>

            <!--BOTON PARA VER TABLA FINANCIERA -->
            <div>
                <input id="vertabla" type="button" class="btn btn-success" value="Tabla financiera" onclick="iratabla()" />
                <input id="novertabla" style="display: none;" type="button" class="btn btn-success" value="Ocultar tabla financiera" onclick="hidetabla()" />
            </div>

            <div class="container" style="display: none;" id="tblFinanciera">
                <div class="row">
                    <div class="row">
                        <div class="col-sm-12">
                            <h4>Llena tu información</h4>
                        </div>
                    </div>
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
                                    <input type="text" id="ubruta" value="0" onkeyup="Calc1()" /></td>
                                <td rowspan="2">
                                    <label id="r1" style="font-size: 18px; font-weight: 400;"></label>
                                </td>
                            </tr>
                            <tr>
                                <td>V NETAS</td>
                                <td>
                                    <input type="number" id="vneta" value="0" onkeyup="Calc1()" /></td>
                            </tr>
                            <!-- CALC 2-->
                            <tr>
                                <td rowspan="2">Margen de utilidad operativa</td>
                                <td rowspan="2">El objetivo es obtener como mínimo 8%... [Utilidad operativa / Ventas netas]</td>
                                <td>U Operativa</td>
                                <td>
                                    <input type="text" id="uoperativa" value="0" onkeyup="Calc2()" /></td>
                                <td rowspan="2">
                                    <h4 id="r2"></h4>
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
                                    <h4 id="r3"></h4>
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
                                    <h4 id="r4"></h4>
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
                                    <h4 id="r5"></h4>
                                </td>
                            </tr>
                            <tr>
                                <td>(DIAS PERIODO)</td>
                                <td>
                                    <input type="number" id="cvdias" value="0" onkeyup="Calc5()" /></td>
                            </tr>
                            <!-- CALC 6 -->
                            <tr>
                                <td rowspan="2">Periodo promedio de cobro (unidades en dias)</td>
                                <td rowspan="2">Objetivo de 30 dias <-::: [(Cuentas por cobrar / Ventas a crédito) (Dias del periodo)]</td>
                                <td>CXC / (VTS CRED)</td>
                                <td>
                                    <input type="number" id="cxc" value="0" onkeyup="Calc6()" /></td>
                                <td rowspan="2">
                                    <h4 id="r6"></h4>
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
                                    <input type="number" id="cxp" value="0" onkeyup="Calc7()" /></td>
                                <td rowspan="2">
                                    <h4 id="r7"></h4>
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
                                    <h4 id="r8"></h4>
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
                                    <h4 id="r9"></h4>
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
                                    <h4 id="r10"></h4>
                                </td>
                            </tr>
                            <tr>
                                <td>(ACT TOT- PAS TOT)</td>
                                <td>
                                    <input type="text" id="atotpas" value="0" onkeyup="Calc10()" /></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="row">
                    <label>Carga archivos</label>
                    <input type="file" id="fileTabla" /> <br />
                    <a id="linkarchivo"></a>
                </div>
            </div>
        </div>

        <div>
            <input id="saveTabla" type="button" class="btn btn-success" value=" Guarda Tabla Financiera" onclick="guardaTabla()" />
        </div>
        <!-- /.panel-body -->
        <%--<asp:Button ID="btnGuarda" runat="server" Text="Finalizar" OnClick="btnGuarda_Click" />

        //<asp:Button ID="btnGuardaAvance" runat="server" Text="Guardar Avance" OnClick="GuardaAvance_Clicked" />--%>
        <br />
        <div>
            <input id="finaliza" type="button" class="btn btn-success" value="Finalizar" onclick="finalizaAud()" />
            <input id="saveDatos" type="button" class="btn btn-success" value="Guardar avance" onclick="guardaDatos()" />
        </div>
    </div>

    <div class="panel-body" style="margin-top: 8rem; display: none;" id="resumen">
        <div class="row" style="margin: 10px">
            <h1>Resumen</h1>
            <div>
                <h2>El puntaje de esta auditoria es de: <span id="puntajeAuditoria"></span></h2>
                <h3 id="creaActividades"></h3>
            </div>
            <table id="tbl_resumen" class="table">
                <thead>
                    <tr>
                        <th style="text-align: center;">Punto</th>
                        <th style="text-align: center;">Comentarios</th>
                    </tr>
                </thead>
                <tbody id="tbody">
                    <%--<%if (puntosResumen != null)
                        {
                            foreach (var pto in puntosResumen)
                            { %>
                    <tr>
                        <td style="text-align: start;"><%=pto.punto%></td>
                        <td style="text-align: start;"><%=pto.comentario%></td>
                    </tr>
                    <%}
                        }%>--%>
                </tbody>
            </table>

        </div>
        <a id="nnivel" style="font-size: 14px; padding: 5px; border: 1px solid blue; border-radius: 5px; color: blue; cursor: pointer; text-decoration: none;">OK</a>
    </div>

    <script type="text/javascript">

        const queryString = window.location.search;
        console.log(queryString);
        const urlParams = new URLSearchParams(queryString);
        var aud = urlParams.get('aud');

        var autoSaveTimer;

        if('<%=estatus%>' == 'INCOMPLETO' || '<%=estatus%>' == ''){
            
            function iniciarAutoGuardado() {
                //console.log("iniciando conteo");
                autoSaveTimer = setInterval(function () {
                    $('#saveDatos').click();
                }, 300000); // Autoguardar cada 5 minuto 
            }
        
        
        function detenerAutoGuardado() {
            clearInterval(autoSaveTimer);
        }

        function reiniciarAutoGuardado() {
            detenerAutoGuardado();
            iniciarAutoGuardado();
        }

        // Detectar interacción del usuario
        $(document).on("mousemove keydown", function () {
            reiniciarAutoGuardado();
        });

        }
        $(document).ready(function () {
            var params = new URLSearchParams(window.location.search);
            $('#resumen').hide();
            $('#finaliza').hide();

            // Obtener el valor de un parámetro específico
            var aud = params.get('aud'); // Devuelve "valor1"

            if("<%=estatus%>" == "COMPLETADO"){
                $('#finaliza').hide();
                $('#saveDatos').hide();
            }

            console.log(aud);

            checaPendientes();
            if(<%=haytabla.ToString().ToLower()%> == true){
                $('#saveTabla').hide();
            }else{
                $('#saveTabla').show();
            }

            $.ajax({
                url: '<%=ResolveUrl("CreaAutoAuditoria.aspx/traeTablaF")%>',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                type: 'POST',
                async: false,
                data: JSON.stringify({aud:aud}),
                success: function (response) {
                    var json = JSON.parse(response.d);
                    //console.log(json);
                    if(json != -1){
                        $('#r1').text(json.r1);
                        $('#r2').text(json.r2);
                        $('#r3').text(json.r3);
                        $('#r4').text(json.r4);
                        $('#r5').text(json.r5);
                        $('#r6').text(json.r6);
                        $('#r7').text(json.r7);
                        $('#r8').text(json.r8);
                        $('#r9').text(json.r9);
                        $('#r10').text(json.r10);
                        
                        $('#ubruta').val(json.ubruta); $('#vneta').val(json.vneta);
                        $('#uoperativa ').val(json.uoperativa ); $('#vneta2').val(json.vneta2);
                        $('#acirc').val(json.acirc); $('#pcirc').val(json.pcirc);
                        $('#acinv').val(json.acinv); $('#pcirc2').val(json.pcirc2);
                        $('#inv').val(json.inv); $('#cvdias').val(json.cvdias);
                        $('#cxc').val(json.cxc); $('#vcredper').val(json.vcredper);
                        $('#cxp').val(json.cxp); $('#cvper').val(json.cvper);
                        $('#ptot').val(json.ptot); $('#atot').val(json.atot);
                        $('#goper').val(json.goper); $('#vnet').val(json.vnet);
                        $('#uoper').val(json.uoper); $('#atotpas').val(json.atotpas);
                    }
                }
            });

            $.ajax({
                url: '<%=ResolveUrl("CreaAutoAuditoria.aspx/traePuntos")%>',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                type: 'POST',
                async: false,
                data: JSON.stringify({aud:aud}),
                success: function (response) {
                    if (response.d != -1) {

                        var json = JSON.parse(response.d);
                       // console.log(json);

                        var checkall = $(".switch :checkbox:checked").length;

                        for (i = 0; i < json.length; i++) {

                            if (json[i].resultado == true) {
                                var idk = $(".switch :checkbox")[i].id;
                                //console.log(idk);
                                $('#' + idk).prop('checked', true);

                            } else {
                                var idc = $(".switch :checkbox")[i].id;
                                //console.log(idc);
                                $('#' + idc).prop('checked', false);
                                
                            }

                            if (json[i].n_a == true) {
                                //console.log($(".noAplica :checkbox")[i].id);
                                //var idNA = $(".noAplica :checkbox")[i].id;
                                var idNA = "na-"+json[i].punto;
                                //console.log(idNA);
                                $('#' + idNA).prop('checked', true);

                            }// else {
                            //    //console.log($(".noAplica :checkbox")[i]);
                            //    var idNA2 = $(".noAplica :checkbox")[i].id;
                            //    $('#' + idNA2).prop('checked', false);
                            //}

                            if (json[i].comentarios != null) {
                                //console.log($('.coment :text')[i].id);
                                var comment = $('.coment :text')[i].id;
                                $('#' + comment).val(json[i].comentarios);
                            }


                        }
                    }
                }

            });
        });

        function checkna(element){
            //console.log(element);
            var idpt = element.name.split('na');
            //console.log(idpt[1]);
            var idswitch = idpt[1];
            console.log(idswitch);
           
            if(element.checked == true){
                $('#cmn-toggle-' + idswitch).prop('disabled',true);
                $('#cmn-toggle-' + idswitch).prop('checked',false);
            }else{
                $('#cmn-toggle-' + idswitch).prop('disabled',false);
            }
        }

        function p() {
            //console.log("x");
        }

        function clickCheck() {
            var all = $("div :checkbox").length;
            //console.log(all);
            var axll = $(".switch :checkbox").length;

            var checkall = $(".switch :checkbox:checked").length;
            //console.log("cuan" + checkall);
            var axll = $(".switch :checkbox").each(function (k) {

                if (checkall > 0) {
                    $(this).prop("checked", false);
                }
                else {
                    $(this).prop("checked", true);
                }
            });
            //console.log(axll);
        }

        function clickExpand() {
            var all = $("li :checkbox").length;
            //console.log(all);
            var axll = $(".switch :checkbox").length;
            //console.log(axll);

            var checkall = $("li :checkbox:checked").length;
            //console.log("cuan" + checkall);
            var axll = $("li :checkbox").each(function (k) {

                // if (checkall > 0) {
                if((this.id[0] != "n" && this.id[1] != "a") && (this.id[0] != "c" && this.id[1] != "m" && this.id[2] != "n")){
                    $(this).prop("checked", false);
                }
                    
                ///}
                //else {


                if (this.id[0] == "n" && this.id[1] == "a") {
                    // $(this).prop("checked", false);
                    //$(this).attr('disabled', true);
                } else if (this.id[0] == "c" && this.id[1] == "m" && this.id[2] == "n") {
                    //$(this).prop("checked", false);
                } else {
                    $(this).prop("checked", true);
                }

                //}
            });
            //console.log(axll);
        }

        function abreCondiciones(x, np) {
            console.log(x);
            $.ajax({
                url: "<%= ResolveUrl("CreaAutoAuditoria.aspx/traeCondiciones")%>",
                type: "POST",
                //cache: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ idPunto: x }),
                success: function (response) {
                    //console.log(response);
                    var data = JSON.parse(response.d);
                    var html = "<ul>";
                    $.each(data, function (i) {
                        html += "<li style='text-align:justify'>" + data[i].nombre + "</li>";
                    });
                    html += "</ul>";
                    // console.log(html);


                    Swal.fire({
                        title: '<strong style="font-size:1.3rem"> <u>' + np + '</u></strong>',
                        icon: 'info',
                        html: html,

                        confirmButtonText:
                          '<i class="fa fa-thumbs-up"></i> ok!',
                        confirmButtonAriaLabel: 'Ok!',

                    })
                }
            });
        }

        function chResponsable() {
            var resp1 = $('#responsables').text();
            var resp = $("#responsables option:selected").text();

            $.ajax({
                url: '<%=ResolveUrl("CreaAutoAuditoria.aspx/PuntoxResp")%>',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                type: 'POST',
                async: false,
                data: JSON.stringify({ resp: resp }),
                success: function (response) {
                    var data = JSON.parse(response.d);

                    var axll = $(".lipunto").each(function (k) {
                        console.log(this);
                        $("#" + this.id).hide();

                    });

                    data.forEach(muestraElemento);
                    console.log();

                    console.log(data);
                }
            });
        }

        function muestraElemento(item, index) {
            console.log(item);
            console.log(index);

            $("#li" + item.idpunto).show();
        }

        function focusComentario(element) {
            var splitString = element.id.split("-");
            if (element.checked == false) {

                var campoComent = "coments" + splitString[2];

                var campoNA = "na-" + splitString[2];
                $('#' + campoNA).attr('disabled', false);

                $('#' + campoComent).focus();
                $('#' + campoComent).css('box-shadow', '0 0 10px #80D7FD');
            }

            if (element.checked == true) {
                var campoNA = "na-" + splitString[2];
                $('#' + campoNA).attr('disabled', true);

                var campoComent = "coments" + splitString[2];
                $('#' + campoComent).css('box-shadow', 'unset');
            }
        }

        function iratabla() {
            //window.location.replace("/tablafinanciera.aspx?aud=");
            $('#tblFinanciera').show();
            $('#vertabla').hide();
            $('#novertabla').show();
        }

        function hidetabla() {
            //window.location.replace("/tablafinanciera.aspx?aud=");
            $('#tblFinanciera').hide();
            $('#vertabla').show();
            $('#novertabla').hide();
        }

        function Calc1() {

            var u = $('#ubruta').val();
            var v = $('#vneta').val();
            if (v > 0) {
                var res = u / v;
                var f = new Number(res);
                console.log(f.toFixed(2));
                $('#r1').text(f.toFixed(3));
                console.log($('#r1').text());
            }
        }

        function Calc2() {
            var u = $('#uoperativa').val();
            var v = $('#vneta2').val();
            if (v > 0) {
                var res = u / v;
                var f = new Number(res);
                console.log(f.toFixed(2));
                $('#r2').text(f.toFixed(3));
            }
        }
        function Calc3() {
            var u = $('#acirc').val();
            var v = $('#pcirc').val();
            if (v > 0) {
                var res = u / v;
                var f = new Number(res);
                console.log(f.toFixed(2));
                $('#r3').text(f.toFixed(3));
            }
        }
        function Calc4() {
            var u = $('#acinv').val();
            var v = $('#pcirc2').val();
            if (v > 0) {
                var res = u / v;
                var f = new Number(res);
                console.log(f.toFixed(2));
                $('#r4').text(f.toFixed(3));
            }
        }
        function Calc5() {
            var u = $('#inv').val();
            var v = $('#cvdias').val();
            if (v > 0) {
                //var res = (u / v) * 360;
                var res = (u * v);
                var f = new Number(res);
                console.log(f.toFixed(2));
                $('#r5').text(f.toFixed(3));
            }
        }
        function Calc6() {
            var u = $('#cxc').val();
            var v = $('#vcredper').val();
            if (v > 0) {
                //var res = ((u / v)*360) / 12;
                var res = (u * v);
                var f = new Number(res);
                console.log(f.toFixed(2));
                $('#r6').text(f.toFixed(3));
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
                $('#r7').text(f.toFixed(3));
            }
        }
        function Calc8() {
            var u = $('#ptot').val();
            var v = $('#atot').val();
            if (v > 0) {
                var res = u / v;
                var f = new Number(res);
                console.log(f.toFixed(2));
                $('#r8').text(f.toFixed(3));
            }
        }
        function Calc9() {
            var u = $('#goper').val();
            var v = $('#vnet').val();
            if (v > 0) {
                var res = u / v;
                var f = new Number(res);
                console.log(f.toFixed(2));
                $('#r9').text(f.toFixed(3));
            }
        }

        function Calc10() {
            var u = $('#uoper').val();
            var v = $('#atotpas').val();
            if (v > 0) {
                var res = u / v;
                var f = new Number(res);
                console.log(f.toFixed(2));
                $('#r10').text(f.toFixed(3));
            }
        }

        function guardaTabla() {
            const queryString = window.location.search;
            console.log(queryString);
            const urlParams = new URLSearchParams(queryString);
            //const aud = urlParams.get('aud')
            //console.log(aud);

            var r1 = $('#r1').text();
            var r2 = $('#r2').text();
            var r3 = $('#r3').text();
            var r4 = $('#r4').text();
            var r5 = $('#r5').text();
            var r6 = $('#r6').text();
            var r7 = $('#r7').text();
            var r8 = $('#r8').text();
            var r9 = $('#r9').text();
            var r10 = $('#r10').text();

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

            var fileInput = document.getElementById('fileTabla');
            var file = fileInput.files[0];

            var campos = [r1, r2, r3, r4, r5, r6, r7, r8, r9, r10, ubruta, vneta, uoperativa, vneta2, acirc, pcirc, acinv, pcirc2, inv, cvdias, cxc, vcredper, cxp, cvper, ptot, atot, goper, vnet, uoper, atotpas];

            var completos = validaCampos(campos);

            if(file != null){
                var reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onloadend = function() {
                    var b64 = reader.result.split("base64,")[1];
                    var filename = file.name;
                    //console.log('Archivo convertido a Base64:', b64);
                    //console.log(filename);

                    ajaxTabla(aud, r1, r2, r3, r4, r5, r6, r7, r8, r9, r10, ubruta, vneta, uoperativa, vneta2, acirc, pcirc, acinv, pcirc2, inv, cvdias, cxc, vcredper, cxp, cvper, ptot, atot, goper, vnet, uoper, atotpas, b64, filename, completos);
                };
            }else{
                var b64 = "";
                var filename = "";

                ajaxTabla(aud, r1, r2, r3, r4, r5, r6, r7, r8, r9, r10, ubruta, vneta, uoperativa, vneta2, acirc, pcirc, acinv, pcirc2, inv, cvdias, cxc, vcredper, cxp, cvper, ptot, atot, goper, vnet, uoper, atotpas, b64, filename, completos);
            }
            <%--$.ajax({
                url: '<%=ResolveUrl("CreaAutoAuditoria.aspx/guardaTabla")%>',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                type: 'POST',
                async: false,
                data: JSON.stringify({ aud: aud, r1:r1, r2:r2, r3:r3, r4:r4, r5:r5, r6:r6, r7: r7, r8:r8, r9:r9, r10:r10,
                    ubruta:ubruta, vneta:vneta, uoperativa:uoperativa, vneta2:vneta2, acirc:acirc, pcirc:pcirc, acinv:acinv, pcirc2:pcirc2, inv:inv, cvdias:cvdias,
                    cxc:cxc, vcredper:vcredper, cxp:cxp, cvper:cvper, ptot:ptot, atot:atot, goper:goper, vnet:vnet, uoper:uoper, atotpas:atotpas}),
                success: function (response) {
                    console.log(response.d);
                    if(response.d != -1){
                        // alert("TABLA FINANCIERA GUARDADA");
                        Swal.fire({
                            title:'Tabla guardada',
                            text:'La tabla financiera se ha guardado exitosamente',
                            icon:'success',

                            confirmButtonText:
                              '<i class="fa fa-thumbs-up"></i> ok!',
                            confirmButtonAriaLabel: 'Ok!',

                        })
                    } else {
                        Swal.fire({
                            title:'Tabla no guardada',
                            text:'Debes guardar la auditoria primero',
                            icon:'info',

                            confirmButtonText:
                              '<i class="fa fa-thumbs-up"></i> ok!',
                            confirmButtonAriaLabel: 'Ok!',

                        })
                    }

                }
            });--%>
        }

        function ajaxTabla(aud, r1, r2, r3, r4, r5, r6, r7, r8, r9, r10, ubruta, vneta, uoperativa, vneta2, acirc, pcirc, acinv, pcirc2, inv, cvdias, cxc, vcredper, cxp, cvper, ptot, atot, goper, vnet, uoper, atotpas, b64, filename, completos){
            $.ajax({
                url: '<%=ResolveUrl("CreaAutoAuditoria.aspx/guardaTabla")%>',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                type: 'POST',
                async: false,
                data: JSON.stringify({ aud: aud, r1:r1, r2:r2, r3:r3, r4:r4, r5:r5, r6:r6, r7: r7, r8:r8, r9:r9, r10:r10,
                    ubruta:ubruta, vneta:vneta, uoperativa:uoperativa, vneta2:vneta2, acirc:acirc, pcirc:pcirc, acinv:acinv, pcirc2:pcirc2, inv:inv, cvdias:cvdias,
                    cxc:cxc, vcredper:vcredper, cxp:cxp, cvper:cvper, ptot:ptot, atot:atot, goper:goper, vnet:vnet, uoper:uoper, atotpas:atotpas, archivo: b64, nombre:filename, completa: completos}),
                success: function (response) {
                    var resp = JSON.parse(response.d);
                    //console.log(resp);
                    if(response.d != -1){
                        // alert("TABLA FINANCIERA GUARDADA");
                        $('#linkarchivo').attr('href','');
                        $('#linkarchivo').attr('target', '_blank');
                        $('#linkarchivo').text('');

                        $('#linkarchivo').attr('href',resp.url);
                        $('#linkarchivo').attr('target', '_blank');
                        $('#linkarchivo').text(resp.nombre);

                        Swal.fire({
                            title:'Tabla guardada',
                            text:'La tabla financiera se ha guardado exitosamente',
                            icon:'success',

                            confirmButtonText:
                              '<i class="fa fa-thumbs-up"></i> ok!',
                            confirmButtonAriaLabel: 'Ok!',

                        })
                    } else {
                        Swal.fire({
                            title:'Tabla no guardada',
                            text:'Debes guardar la auditoria primero',
                            icon:'info',

                            confirmButtonText:
                              '<i class="fa fa-thumbs-up"></i> ok!',
                            confirmButtonAriaLabel: 'Ok!',

                        })
                    }

                }
            });
        } 

        function guardaDatos(){

            var dist = $('#ctl00_ContentPlaceHolder1_drpDist').val();
            var checkboxes = document.querySelectorAll('div.switch > input[type=checkbox]');
            var valoresCheckboxes = [];

            checkboxes.forEach(function(checkbox){
                //if(checkbox.checked){
                //    valoresCheckboxes.push(checkbox.name);
                //}
                if(checkbox.checked){
                    valoresCheckboxes.push(checkbox.name + "-si");
                }else{
                    valoresCheckboxes.push(checkbox.name + "-no");
                }
            });
            //console.log("Valores de los checkboxes seleccionados:", valoresCheckboxes);

            var textfields = document.querySelectorAll('div.coment > input[type=text]');
            //console.log(textfields);
            var valoresText = [];

            textfields.forEach(function(textfield){
                ///if(textfield.value != "" && textfield.value != null){
                    valoresText.push(textfield.name + "-" + textfield.value)
                //}
            });

            //console.log("Valores de los text seleccionados:", valoresText);

            var NAs = document.querySelectorAll('div.noAplica > input[type=checkbox]');
            var valoresNA = [];

            NAs.forEach(function(checkbox){
                //if(checkbox.checked){
                //    valoresNA.push(checkbox.name);
                //}
                if(checkbox.checked){
                    valoresNA.push(checkbox.name + "-si");
                }else{
                    valoresNA.push(checkbox.name + "-no");
                }
            });

            var datos = {
                switchs: valoresCheckboxes,
                comentarios: valoresText,
                noAplica: valoresNA,
                distribuidor: dist
            }

           $.ajax({
                url: '<%=ResolveUrl("CreaAutoAuditoria.aspx/saveDatos")%>',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                type: 'POST',
                data: JSON.stringify(datos),
                async: false,
                success: function (response) {
                    var json = JSON.parse(response.d);
                    //console.log(json.replace("//","/"));
                    //window.open(json);
                    if(json != -1){
                        //alert("Auditoria guardada con exito");
                        Swal.fire({
                            title:'Auto auditoria guardada',
                            text:'Auto auditoria guardada exitosamente',
                            icon:'success',

                            confirmButtonText:
                              '<i class="fa fa-thumbs-up"></i> ok!',
                            confirmButtonAriaLabel: 'Ok!',

                        })

                        if(aud == null || aud == ""){
                            location.href = "CreaAutoAuditoria.aspx?aud=" + json;
                        }

                        if(json.estatus == "COMPLETADO"){
                            $('#finaliza').show();
                        }else{
                            $('#finaliza').hide();
                        }

                        //checaPendientes();
                    }
                    else{
                        //alert("Hubo un problema con guardar la auditoria");
                        Swal.fire({
                            title:'Auditoria no guardada',
                            text:'Hubo un problema al guardar la auditoria. Intente de nuevo mas tarde.',
                            icon:'info',

                            confirmButtonText:
                              '<i class="fa fa-thumbs-up"></i> ok!',
                            confirmButtonAriaLabel: 'Ok!',

                        })
                    }
                }
            });
        }

        function finalizaAud(){

            var dist = $('#ctl00_ContentPlaceHolder1_drpDist').val();
            var checkboxes = document.querySelectorAll('div.switch > input[type=checkbox]');
            var valoresCheckboxes = [];

            checkboxes.forEach(function(checkbox){
                //if(checkbox.checked){
                //    valoresCheckboxes.push(checkbox.name);
                //}
                if(checkbox.checked){
                    valoresCheckboxes.push(checkbox.name + "-si");
                }else{
                    valoresCheckboxes.push(checkbox.name + "-no");
                }
            });
            //console.log("Valores de los checkboxes seleccionados:", valoresCheckboxes);

            var textfields = document.querySelectorAll('div.coment > input[type=text]');
            //console.log(textfields);
            var valoresText = [];

            textfields.forEach(function(textfield){
                ///if(textfield.value != "" && textfield.value != null){
                    valoresText.push(textfield.name + "-" + textfield.value)
                //}
            });

            //console.log("Valores de los text seleccionados:", valoresText);

            var NAs = document.querySelectorAll('div.noAplica > input[type=checkbox]');
            var valoresNA = [];

            NAs.forEach(function(checkbox){
                //if(checkbox.checked){
                //    valoresNA.push(checkbox.name);
                //}
                if(checkbox.checked){
                    valoresNA.push(checkbox.name + "-si");
                }else{
                    valoresNA.push(checkbox.name + "-no");
                }
            });

            var datos = {
                switchs: valoresCheckboxes,
                comentarios: valoresText,
                noAplica: valoresNA,
                distribuidor: dist
            }

           $.ajax({
                url: '<%=ResolveUrl("CreaAutoAuditoria.aspx/finaliza")%>',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                type: 'POST',
                data: JSON.stringify(datos),
                async: false,
                success: function (response) {
                    var json = JSON.parse(response.d);

                    console.log(json);
                    //console.log(json.replace("//","/"));
                    //window.open(json);
                    if(json != -1){
                        //alert("Auditoria guardada con exito");
                        $('#puntajeAuditoria').text(json.puntajeAuditoria);
                        $('#creaActividades').text(json.creaActividades)
                        $('#nnivel').attr('href', json.nnivel);

                        html = '';
                        
                        json.puntos.forEach(function(i){
                            console.log(i);
                            html += '<tr>' +
                                        '<td>' + i.punto + '</td>' +
                                        '<td>' + i.comentario + '</td>' 
                            '</tr>'
                        });
                        console.log(html);

                        $('#tbody').append(html);
                        $('#contenido').hide();
                        $('#finaliza').hide();
                        $('#saveDatos').hide();
                        $('#saveTabla').hide();
                        $('#resumen').show();


                    }
                    else{
                        //alert("Hubo un problema con guardar la auditoria");
                        Swal.fire({
                            title:'Auditoria no guardada',
                            text:'Hubo un problema al finalizar la auditoria. Intente de nuevo mas tarde.',
                            icon:'info',

                            confirmButtonText:
                              '<i class="fa fa-thumbs-up"></i> ok!',
                            confirmButtonAriaLabel: 'Ok!',

                        })
                    }
                }
            });
        }

        function descargaExcel(){
            window.open("Archivos/EDC-III-(Auditoría).xlsx");
        }

        function validaCampos(campos) {

            var pasa = false;
            var count = 0;
            
            campos.forEach(function (i) {
                if(i == null || i == "" || i == "0"){
                    
                    count++;
                }
            });

            if (count > 0) {

                return false;

            } else {

                return true;
            }
        }

        function checaPendientes(){
            $.ajax({
                url: '<%=ResolveUrl("CreaAutoAuditoria.aspx/checaPendientes")%>',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                type: 'POST',
                async: false,
                success: function (response) {
                    var json = JSON.parse(response.d);
                    //console.log(json);

                    if(json != -1 && json != 1){
                        var html = "<ul>";
                        $.each(json, function (i) {
                            html += "<li style='text-align:justify'>" + json[i] + "</li>";
                        });
                        html += "</ul>";

                        Swal.fire({
                            title:'Faltan comentarios en los siguientes puntos:',
                            html: html,
                            icon:'warning',

                            confirmButtonText:
                              '<i class="fa fa-thumbs-up"></i> ok!',
                            confirmButtonAriaLabel: 'Ok!',

                        })
                    }

                    if(json == 1 && "<%=estatus%>" != "COMPLETADO"){
                        $('#finaliza').show();
                        //$('#saveDatos').hide();
                    }
                }
            });
        }

    </script>
</asp:Content>
