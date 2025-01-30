<%@ Page Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="AvanceFinancieroGeneral.aspx.cs" Inherits="CatalogosLTH.Web.AvanceFinancieroGeneral" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
          <iframe
                src=<%=iframeUrl %>
                frameborder="0"
                width="100%"
                height="1800px"
                allowtransparency
                style="margin-top:16rem; height: 100vh"
            ></iframe>
   <%-- <div class="container" style="width: 100% !important; margin-top:16rem;">
        
        <div >
         <h2>Avance Financiero General</h2>
            
           <div class="row">
                <div class="">
                    <canvas id="muo"></canvas>
                </div><br>
                <div class="">
                    <canvas id="go"></canvas>
                </div><br>
            </div>
            <br />
            <div class="row">
                <div class="">
                    <canvas id="e"></canvas>
                </div><br>
                <div class="">
                    <canvas id="ppc"></canvas>
                </div>
            </div>
        </div>
    </div>--%>



    <script src="https://cdnjs.cloudflare.com/ajax/libs/chartjs-plugin-datalabels/2.2.0/chartjs-plugin-datalabels.min.js" integrity="sha512-JPcRR8yFa8mmCsfrw4TNte1ZvF1e3+1SdGMslZvmrzDYxS69J7J49vkFL8u6u8PlPJK+H3voElBtUCzaXj+6ig==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
    <script>
        
      <%--  let chart1, chart2, chart3, chart4;

        $(document).ready(function () {
            llenaCharts();
        });

        function llenaCharts(){
            
                $.ajax({
                    url: '<%=ResolveUrl("AvanceFinancieroGeneral.aspx/promedios")%>',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    type: 'POST',
                    async: false,
                    success: function (response) {
                        var json = JSON.parse(response.d);
                        console.log(json);

                        if (json != -1) {
                            var i = 0;
                            var lbls = [
                                    <%= avggMUO%>, 
                                    <%= avggGO %>, 
                                    <%= avggE %>, 
                                    <%= avggPPC %>
                                ];
                            json.forEach(item => {
                                
                                const canva = document.getElementById(`${item.tipo}`)
                                console.log(lbls)
                                creaChart(canva, item.Labels, item.Data, item.nombre[0], "Promedio", lbls[i]);
                                i++;
                            });
                        }
                    }

                });

        }

        function creaChart(grafo, labels, datos, lbl, avg, promedio){
            return new Chart(grafo, {
                destroy: true,
                type: 'bar',
                data: {
                    labels: labels,
                    datasets: [
                        {
                            label: lbl,
                            data: datos,
                            borderWidth: 1,
                            datalabels: {
                                display: true,
                                color: "black",
                                align: 'top',
                                formatter: (value, context) => value
                            }
                        },
                        {
                            type: 'line',
                            label: avg,
                            data: promedio,
                            borderWidth: 1,
                            dataLabels: {
                                display: false
                            }
                        }

                    ]
                },
                options: {
                    scales: {
                        x: {
                            display: true,
                            title: {
                                display: true,
                                text: 'Distribuidor'
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
                            display: false,
                            color: "black", // Customize label color
                            align: 'top',
                            formatter: (value, context) => value
                        }
                    }
                },
                plugins: [ChartDataLabels]

            });
        }--%>

        
    </script>
</asp:Content>
