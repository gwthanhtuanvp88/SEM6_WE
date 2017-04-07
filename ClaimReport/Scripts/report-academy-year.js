$(document).ready(function () {
    $("#academy-year-select").change(function () {
        var val = $(this).val();
        if (val != null) {
            Percentage_of_claims_by_each_Faculty_for_any_cademic_year(val);
            Claims_without_uploaded_evidence(val);
            Claims_without_a_decision_after_14_days(val);
            day_has_the_most_claim(val) 
        }
    });
});

// Percentage of claims by each Faculty for any academic year.
function Percentage_of_claims_by_each_Faculty_for_any_cademic_year(academyYearID) {
    $.ajax({
        type: "POST",
        dataType: "JSON",
        data: { academyYearID: academyYearID },
        url: "/Manager/Percentage_of_claims_by_each_Faculty_for_any_cademic_year",
        success: function (res) {
            var ctx = document.getElementById("Percentage_of_claims_by_each_Faculty_for_any_cademic_year").getContext("2d");

            var data = {
                labels: res.labels,
                datasets: [
                    {
                        data: res.data,
                        backgroundColor: [
                            "#FF6384",
                            "#36A2EB",
                            "#FFCE56"
                        ],
                        hoverBackgroundColor: [
                            "#FF6384",
                            "#36A2EB",
                            "#FFCE56"
                        ]
                    }]
            };

            var options = {
                title: {
                    display: true,
                    text: 'Percentage of claims by each Faculty for a academic year',
                    fontSize: 14,
                    fontStyle: 'normal',
                    position: 'bottom',
                },
                legend: {
                    position: 'bottom'
                },
                tooltips: {
                    callbacks: {
                        label: function (tooltipItem, data) {
                            var dataset = data.datasets[tooltipItem.datasetIndex];
                            var total = dataset.data.reduce(function (previousValue, currentValue, currentIndex, array) {
                                return previousValue + currentValue;
                            });
                            var currentValue = dataset.data[tooltipItem.index];
                            var precentage = Math.floor(((currentValue / total) * 100) + 0.5);
                            return precentage + "%";
                        }
                    }
                }
            }

            var pieChart = new Chart(ctx, {
                type: 'pie',
                data: data,
                options: options
            });
        }
    })

}

// Claims_without_uploaded_evidence
function Claims_without_uploaded_evidence(academyYearID) {
    $.ajax({
        type: "POST",
        dataType: "JSON",
        data: { academyYearID: academyYearID },
        url: "/Manager/Claims_without_uploaded_evidence",
        success: function (res) {
            var ctx = document.getElementById("Claims_without_uploaded_evidence").getContext("2d");

            var data = {
                labels: res.labels,
                datasets: [
                    {
                        data: res.data,
                        backgroundColor: [
                            "#FF6384",
                            "#36A2EB",
                            "#FFCE56"
                        ],
                        hoverBackgroundColor: [
                            "#FF6384",
                            "#36A2EB",
                            "#FFCE56"
                        ]
                    }]
            };

            var options = {
                title: {
                    display: true,
                    text: 'Percentage of claims without uploaded evidence',
                    fontSize: 14,
                    fontStyle: 'normal',
                    position: 'bottom',
                },
                legend: {
                    position: 'bottom'
                },
                tooltips: {
                    callbacks: {
                        label: function (tooltipItem, data) {
                            var dataset = data.datasets[tooltipItem.datasetIndex];
                            var total = dataset.data.reduce(function (previousValue, currentValue, currentIndex, array) {
                                return previousValue + currentValue;
                            });
                            var currentValue = dataset.data[tooltipItem.index];
                            var precentage = Math.floor(((currentValue / total) * 100) + 0.5);
                            return precentage + "%";
                        }
                    }
                }
            }

            var pieChart = new Chart(ctx, {
                type: 'pie',
                data: data,
                options: options
            });
        }
    })

}

// Claims without a decision after 14 days
function Claims_without_a_decision_after_14_days(academyYearID) {
    $.ajax({
        type: "POST",
        dataType: "JSON",
        data: { academyYearID: academyYearID },
        url: "/Manager/Claims_without_a_decision_after_14_days",
        success: function (res) {
            var ctx = document.getElementById("Claims_without_a_decision_after_14_days").getContext("2d");

            var data = {
                labels: res.labels,
                datasets: [
                    {
                        data: res.data,
                        backgroundColor: [
                            "#FF6384",
                            "#36A2EB",
                            "#FFCE56"
                        ],
                        hoverBackgroundColor: [
                            "#FF6384",
                            "#36A2EB",
                            "#FFCE56"
                        ]
                    }]
            };

            var options = {
                title: {
                    display: true,
                    text: 'Claims without a decision after 14 days',
                    fontSize: 14,
                    fontStyle: 'normal',
                    position: 'bottom',
                },
                legend: {
                    position: 'bottom'
                },
                tooltips: {
                    callbacks: {
                        label: function (tooltipItem, data) {
                            var dataset = data.datasets[tooltipItem.datasetIndex];
                            var total = dataset.data.reduce(function (previousValue, currentValue, currentIndex, array) {
                                return previousValue + currentValue;
                            });
                            var currentValue = dataset.data[tooltipItem.index];
                            var precentage = Math.floor(((currentValue / total) * 100) + 0.5);
                            return precentage + "%";
                        }
                    }
                }
            }

            var pieChart = new Chart(ctx, {
                type: 'pie',
                data: data,
                options: options
            });
        }
    })

}

// Day has the most claim
function day_has_the_most_claim(academyYearID) {
    $.ajax({
        type: "POST",
        dataType: "JSON",
        data: { academyYearID: academyYearID },
        url: "/Manager/day_has_the_most_claim",
        success: function (res) {
            console.log(res);
            var ctx = document.getElementById("day_has_the_most_claim").getContext("2d");

            var data = {
                labels: res.labels,
                datasets: [
                    {
                        label: "Day has the most claim",
                        fill: false,
                        lineTension: 0.1,
                        backgroundColor: "rgba(75,192,192,0.4)",
                        borderColor: "rgba(75,192,192,1)",
                        borderCapStyle: 'butt',
                        borderDash: [],
                        borderDashOffset: 0.0,
                        borderJoinStyle: 'miter',
                        pointBorderColor: "rgba(75,192,192,1)",
                        pointBackgroundColor: "#fff",
                        pointBorderWidth: 1,
                        pointHoverRadius: 5,
                        pointHoverBackgroundColor: "rgba(75,192,192,1)",
                        pointHoverBorderColor: "rgba(220,220,220,1)",
                        pointHoverBorderWidth: 2,
                        pointRadius: 1,
                        pointHitRadius: 10,
                        data: res.data,
                        spanGaps: false,
                    },
                ]
            };

            var options = {
                title: {
                    display: true,
                    text: 'Day has the most claim',
                    fontSize: 14,
                    fontStyle: 'normal',
                    position: 'bottom',
                },
                legend: {
                    position: 'bottom'
                },
            }

            var LineChart = new Chart(ctx, {
                type: 'line',
                data: data,
                options: options
            });
        }
    })

}