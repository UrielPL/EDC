<%@ Page Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="AvanceFinanciero.aspx.cs" Inherits="CatalogosLTH.Web.AvanceFinanciero" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">

        <div style="margin-top: 16rem">
            <h2>Avance Financiero</h2>
            <%if (rol != "Distribuidor")
                { %>
            <div class="row">
                <div class="col-sm-2">
                    <h4>Distribuidor:</h4>
                </div>
                <div class="col-sm-4">
                    <select id="distSelect" onchange="llenaCharts()">
                </select>
                </div>
                
            </div>
            <%} %>

            <div class="row">
                <div class="col-sm-6">
                    <canvas id="muo"></canvas>
                </div>
                <div class="col-sm-6">
                    <canvas id="go"></canvas>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-sm-6">
                    <canvas id="e"></canvas>
                </div>
                <div class="col-sm-6">
                    <canvas id="ppc"></canvas>
                </div>
            </div>
        </div>
    </div>



    <script src="https://cdnjs.cloudflare.com/ajax/libs/chartjs-plugin-datalabels/2.2.0/chartjs-plugin-datalabels.min.js" integrity="sha512-JPcRR8yFa8mmCsfrw4TNte1ZvF1e3+1SdGMslZvmrzDYxS69J7J49vkFL8u6u8PlPJK+H3voElBtUCzaXj+6ig==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
    <script>
        
        let chart1, chart2, chart3, chart4;

        $(document).ready(function () {
        
            if('<%=rol%>' != 'Distribuidor'){
                $.ajax({
                url: '<%=ResolveUrl("AvanceFinanciero.aspx/traeDistribuidores")%>',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                type: 'POST',
                async: false,
                success: function (response) {
                    var select = $('#distSelect');
                    select.empty();
                    var dist = JSON.parse(response.d);
                    //console.log(dist);
                    $.each(dist, function (i) {
                        if (i == 0) {
                            select.append($("<option selected></option>").attr("value", dist[i].id).text(dist[i].nombre));
                        } else {
                            select.append($("<option></option>").attr("value", dist[i].id).text(dist[i].nombre));
                        }
                        
                        
                    });
                    llenaCharts();
                }
            });
            }
            
        
        });

        function llenaCharts(){
            var dist = $('#distSelect option:selected').text();

            if(dist != undefined && dist != null && dist != ""){

                $.ajax({
                    url: '<%=ResolveUrl("AvanceFinanciero.aspx/traeListas")%>',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    type: 'POST',
                    async: false,
                    data: JSON.stringify({dist: dist}),
                    success: function (response) {
                        var json = JSON.parse(response.d);

                        if (json != -1) {

                            const ctx = document.getElementById('muo');
                            const ctppc = document.getElementById('ppc');
                            const cte = document.getElementById('e');
                            const ctgo = document.getElementById('go');

                            const muo = json.muo;
                            const ppc = json.ppc;
                            const e = json.e;
                            const go = json.go;
                            const avMUO = json.avgMUO;
                            const avPPC = json.avgPPC;
                            const avEN = json.avgEN;
                            const avGO = json.avgGO;
                            const labels = muo.map(item => item.x);

                            const muo_val = muo.map(item => item.y);
                            const ppc_val = ppc.map(item => item.y);
                            const e_val = e.map(item => item.y);
                            const go_val = go.map(item => item.y);
                            const avmuo_val = avMUO.map(item => item);
                            const avppc_val = avPPC.map(item => item);
                            const ave_val = avEN.map(item => item);
                            const avgo_val = avGO.map(item => item);

                            if (chart1) chart1.destroy();
                            if (chart2) chart2.destroy();
                            if (chart3) chart3.destroy();
                            if (chart4) chart4.destroy();

                            lbl1 = 'Margen de utilidad operativa';
                            lbl2 = 'Periodo promedio de cobro';
                            lbl3 = 'Endeudamiento';
                            lbl4 = 'Gastos operativos';

                            chart1 = creaChart(ctx, labels, muo_val, lbl1, "Promedio", avMUO);
                            chart2 = creaChart(ctppc, labels, ppc_val, lbl2,  "Promedio", avPPC);
                            chart3 = creaChart(cte, labels, e_val, lbl3, "Promedio", avEN);
                            chart4 = creaChart(ctgo, labels, go_val, lbl4, "Promedio", avGO);

                        } else {
                            if (chart1) chart1.destroy();
                            if (chart2) chart2.destroy();
                            if (chart3) chart3.destroy();
                            if (chart4) chart4.destroy();
                            Swal.fire({
                                title: 'Ha ocurrido un error',
                                text: 'No se ha podido mostrar los graficos, intente de nuevo mas tarde.',
                                icon: 'warning',

                                confirmButtonText:
                                  '<i class="fa fa-thumbs-up"></i> ok!',
                                confirmButtonAriaLabel: 'Ok!',

                            })
                        }
                    }

                });

            }
        }

        function creaChart(grafo, labels, datos, lbl, avgLbl, avg){
            return new Chart(grafo, {
                destroy: true,
                type: 'line',
                data: {
                    labels: labels,
                    datasets: [
                        {
                            type: 'line',
                            label: lbl,
                            data: datos,
                            borderWidth: 1
                        },
                        {
                            type: 'line',
                            label: avgLbl,
                            data: avg
                        }

                    ]
                },
                options: {
                    scales: {
                        x: {
                            display: true,
                            title: {
                                display: true,
                                text: 'Fecha'
                            }
                        },
                        y: {
                            display: true,
                            title: {
                                display: true,
                                text: 'Valor'
                            }
                        }
                    },
                    plugins: {
                        datalabels: {
                            display: true,
                            color: "black", // Customize label color
                            align: 'top',
                            formatter: (value, context) => value
                        }
                    }
                },
                plugins: [ChartDataLabels]

            });
        }

        <%if (rol == "Distribuidor") {%>
            const ctx = document.getElementById('muo');
            const ctppc = document.getElementById('ppc');
            const cte = document.getElementById('e');
            const ctgo = document.getElementById('go');

            const muo = <%=listaMUO%>;
            const ppc = <%=listaPPC%>;
            const e = <%=listaE%>;
            const go = <%=listaGO%>;
            const labels = muo.map(item => item.x);

            const muo_val = muo.map(item => item.y);
            const ppc_val = ppc.map(item => item.y);
            const e_val = e.map(item => item.y);
            const go_val = go.map(item => item.y);

            new Chart(ctx, {
                type: 'line',
                data: {
                    labels: labels,
                    datasets: [
                        {
                            label: 'Margen de utilidad operativa',
                            data: muo_val,
                            borderWidth: 1
                        }

                    ]
                },
                options: {
                    scales: {
                        x: {
                            display: true,
                            title: {
                                display: true,
                                text: 'Fecha'
                            }
                        },
                        y: {
                            display: true,
                            title: {
                                display: true,
                                text: 'Valor'
                            }
                        }
                    },
                    plugins: {
                        datalabels: {
                            display: true,
                            color: "black", // Customize label color
                            align: 'top',
                            formatter: (value, context) => value
                        }
                    }
                },
                plugins: [ChartDataLabels]

            });

            new Chart(ctppc, {
                type: 'line',
                data: {
                    labels: labels,
                    datasets: [
                        {
                            label: 'Periodo Promedio de Cobro',
                            data: ppc_val,
                            borderWidth: 1
                        }

                    ]
                },
                options: {
                    scales: {
                        x: {
                            display: true,
                            title: {
                                display: true,
                                text: 'Fecha'
                            }
                        },
                        y: {
                            display: true,
                            title: {
                                display: true,
                                text: 'Valor'
                            }
                        }
                    },
                    plugins: {
                        datalabels: {
                            display: true,
                            color: "black", // Customize label color
                            align: 'top',
                            formatter: (value, context) => value
                        }
                    }
                },
                plugins: [ChartDataLabels]

            });

            new Chart(cte, {
                type: 'line',
                data: {
                    labels: labels,
                    datasets: [
                        {
                            label: 'Endeudamiento',
                            data: e_val,
                            borderWidth: 1
                        }

                    ]
                },
                options: {
                    scales: {
                        x: {
                            display: true,
                            title: {
                                display: true,
                                text: 'Fecha'
                            }
                        },
                        y: {
                            display: true,
                            title: {
                                display: true,
                                text: 'Valor'
                            }
                        }
                    },
                    plugins: {
                        datalabels: {
                            display: true,
                            color: "black", // Customize label color
                            align: 'top',
                            formatter: (value, context) => value
                        }
                    }
                },
                plugins: [ChartDataLabels]

            });

            new Chart(ctgo, {
                type: 'line',
                data: {
                    labels: labels,
                    datasets: [
                        {
                            label: 'Gastos Operativos',
                            data: go_val,
                            borderWidth: 1
                        }

                    ]
                },
                options: {
                    scales: {
                        x: {
                            display: true,
                            title: {
                                display: true,
                                text: 'Fecha'
                            }
                        },
                        y: {
                            display: true,
                            title: {
                                display: true,
                                text: 'Valor'
                            }
                        }
                    },
                    plugins: {
                        datalabels: {
                            display: true,
                            color: "black", // Customize label color
                            align: 'top',
                            formatter: (value, context) => value
                        }
                    }
                },
                plugins: [ChartDataLabels]
            })
        <%}%>
        
    </script>
</asp:Content>
