<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/edcMaster.Master" CodeBehind="auditoriaCalii.aspx.cs" Inherits="CatalogosLTH.Web.auditoriaCalii" %>

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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container" style="margin-top: 40px">
        <div class="panel-body" style="margin-top: 8rem">
            <div class="row">
                <h1>Auditoria</h1>
                Distribuidor:
    <asp:DropDownList ID="cmbDistribuidor" runat="server"></asp:DropDownList>
            </div>

                <%  foreach (var pilar in vpilar)
                    { %>

                <ul class="cd-accordion margin-top-lg margin-bottom-lg">
                    <li class="cd-accordion__item cd-accordion__item--has-children">
                        <input class="cd-accordion__input" type="checkbox"
                            name="<%=pilar.Nombre%>" id="<%=pilar.Nombre%>">
                        <label class="cd-accordion__label cd-accordion__label--icon-folder"
                            for="<%=pilar.Nombre%>">
                            <span><%=pilar.Nombre%></span></label>

                        <!-- NOMBRE DE PILAR -->

                        <%foreach (var area in pilar.Areas)
                            { %>
                        <ul class="cd-accordion__sub cd-accordion__sub--l1" style="list-style: none">
                            <li class="cd-accordion__item cd-accordion__item--has-children">
                                <input class="cd-accordion__input" type="checkbox"
                                    name="<%=area.Nombre %>" id="<%=area.Nombre %>">
                                <label class="cd-accordion__label cd-accordion__label--icon-folder"
                                    for="<%=area.Nombre %>">
                                    <span><%=area.Nombre %></span></label>
                                <!-- AREA -->

                                <%foreach (var subtema in area.Subtemas)
                                    {  %>
                                <ul class="cd-accordion__sub cd-accordion__sub--l2">
                                    <li class="cd-accordion__item cd-accordion__item--has-children">
                                        <input class="cd-accordion__input" type="checkbox" name="sub<%=subtema.Nombre %>" id="sub<%=subtema.Nombre %>">
                                        <label class="cd-accordion__label cd-accordion__label--icon-folder" for="sub<%=subtema.Nombre %>"><span><%=subtema.Nombre %></span></label>
                                        <!-- Subtema -->

                                        <%foreach (var punto in puntosDetalle.Where(c => c.idsubtema == subtema.Id))
                                            {%>

                                        <ul class="cd-accordion__sub cd-accordion__sub--l3" style="list-style: none">
                                            <li class="cd-accordion__item cd-accordion__item--has-children">
                                                <div class="row">
                                                    <div class="col-sm-7">
                                                        <input class="cd-accordion__input" type="checkbox"
                                                            name="<%=punto.idpunto%>" id="<%=punto.idpunto%>">
                                                        <label class="cd-accordion__label cd-accordion__label--icon-folder"
                                                            for="<%=punto.idpunto%>" style="background-color: #ff6361">
                                                            <span><%=punto.nombrepunto%></span></label>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <div class="switch" style="display: flex;">
                                                            <input name="califa<%:punto.idpunto %>"
                                                                id="cmn-toggle-<%:punto.idpunto %>"
                                                                class="cmn-toggle cmn-toggle-round"
                                                                type="checkbox" <%: (punto.valor==1)?"checked":"" %>>
                                                            <label for="cmn-toggle-<%:punto.idpunto %>"
                                                                name="califal<%:punto.idpunto %>">
                                                            </label>
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
            
            <!-- /.panel-body -->

            <asp:Button ID="btnGuarda" runat="server" Text="Guardar" OnClick="btnGuarda_Click" />
        </div>
    </div>
</asp:Content>
