<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/edcMaster.Master" CodeBehind="DetallesActividad.aspx.cs" Inherits="CatalogosLTH.Web.DetallesActividad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://cdn.datatables.net/2.0.3/css/dataTables.dataTables.css" />
    <style>

        .titulo{
            background-color: #641be4;
            color: white;
            padding: 10px;
            border-radius: 5px;
            width: 99%;
            margin-left: auto;
            margin-right: auto;
            font-weight: 600;
        }

        .tiempo{
            background-color: #8c35ac;
            color: white;
            padding: 10px;
            border-radius: 5px;
            width: 99%;
            margin-left: auto;
            margin-right: auto;
            font-weight: 600;
        }

        .objetivo{
            background-color: #b44f77;
            color: white;
            padding: 10px;
            border-radius: 5px;
            width: 99%;
            margin-left: auto;
            margin-right: auto;
            font-weight: 600;
        }

        .hacer{
            background-color: #e36d38;
            color: white;
            padding: 10px;
            border-radius: 5px;
            width: 99%;
            margin-left: auto;
            margin-right: auto;
            font-weight: 600;
        }

    </style>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container" style="margin-top: 40px">

        <div class="panel-body" style="margin-top: 8rem; text-align:center;" id="contenido" runat="server">

            <h3 class="titulo">ACT-<%=actividad.Numero.ToString().PadLeft(3,'0')%>-B</h3>
            <br />

            <h4 class="tiempo">Tiempo estimado</h4>
            <h4><%=actividad.Vigencia %> Semanas</h4>

        <br />

            <h4 class="objetivo">Objetivo</h4>
            <h4><%=actividad.Objetivo != null ? actividad.Objetivo : "Sin descripcion"%> </h4>

            <br />

            <h4 class="hacer">Que es lo que tengo que hacer?</h4>
            <h4><%=actividad.QueHacer != null ? actividad.QueHacer : "Sin descripcion"%></h4>
        </div>

    </div>

</asp:Content>

