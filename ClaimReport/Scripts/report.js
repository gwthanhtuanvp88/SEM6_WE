$(document).ready(function () {
    

    var lables = "", lable, datas;
    $.ajax({
        type: "POST",
        dataType: "JSON",
        url: "/Manager/Number_of_claims_within_each_Faculty_for_each_academic_year",
        contentType: "application/json;charset=utf-8",
        success: function (res) {
            console.log();
            var ctx = document.getElementById("myChart").getContext("2d");
            var datasets = [];

            for (i = 0; i < res.label.length; i++) {
                datasets[i] = {
                    label: res.label[i],
                    data: res.datas[i],
                    backgroundColor: 'rgba(255, 99, 132, 0.2)',
                    borderColor: 'rgba(255,99,132,1)',
                    borderWidth: 1
                }
            };

            var data = {
                labels: res.labels,
                datasets: datasets
            };

            var myBarChart = new Chart(ctx, {
                type: 'bar',
                data: data,
                options: {
                    barValueSpacing: 20,
                    scales: {
                        yAxes: [{
                            ticks: {
                                min: 0,
                            }
                        }]
                    }
                }
            });
        }

    })   
});