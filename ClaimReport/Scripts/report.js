var colors = []
colors[0] = {
    background: 'rgba(255, 0, 0, 0.2)',
    border: 'rgba(255, 0, 0, 1)',
}

colors[1] = {
    background: 'rgba(0, 255, 0, 0.2)',
    border: 'rgba(0, 255, 0, 1)',
}

colors[2] = {
    background: 'rgba(0, 0, 255, 0.2)',
    border: 'rgba(0, 0, 255, 1)',
}

colors[3] = {
    background: 'rgba(255, 255, 0, 0.2)',
    border: 'rgba(255, 255, 0, 1)',
}

colors[4] = {
    background: 'rgba(0, 255, 255, 0.2)',
    border: 'rgba(0, 255, 255, 1)',
}

colors[5] = {
    background: 'rgba(255, 0, 255, 0.2)',
    border: 'rgba(255, 0, 255, 1)',
}

$(document).ready(function () {
    // Number_of_claims_within_each_Faculty_for_each_academic_year
    $.ajax({
        type: "POST",
        dataType: "JSON",
        url: "/Manager/Number_of_claims_within_each_Faculty_for_each_academic_year",
        contentType: "application/json;charset=utf-8",
        success: function (res) {
            var ctx = document.getElementById("Number_of_claims_within_each_Faculty_for_each_academic_year").getContext("2d");

            var datasets = [];

            for (i = 0; i < res.label.length; i++) {
                datasets[i] = {
                    label: res.label[i],
                    data: res.datas[i],
                    backgroundColor: colors[i].background,
                    borderColor: colors[i].border,
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
                    title: {
                        display: true,
                        text: 'Number of claims within each Faculty for each academic year',
                        fontSize: 14,
                        fontStyle: 'normal',
                        position: 'bottom',
                    },
                    legend: {
                        position: 'bottom'
                    }

                }
            });
        }

    });

    // Number_of_students_making_a_claim_within_each_Faculty_for_each_academic_year
    $.ajax({
        type: "POST",
        dataType: "JSON",
        url: "/Manager/Number_of_students_making_a_claim_within_each_Faculty_for_each_academic_year",
        contentType: "application/json;charset=utf-8",
        success: function (res) {
            var ctx = document.getElementById("Number_of_students_making_a_claim_within_each_Faculty_for_each_academic_year").getContext("2d");

            var datasets = [];

            for (i = 0; i < res.label.length; i++) {
                datasets[i] = {
                    label: res.label[i],
                    data: res.datas[i],
                    backgroundColor: colors[i].background,
                    borderColor: colors[i].border,
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
                    title: {
                        display: true,
                        text: 'Number of students making a claim within each Faculty for each academic year',
                        fontSize: 14,
                        fontStyle: 'normal',
                        position: 'bottom',
                    },
                    legend: {
                        position: 'bottom'
                    }
                }
            });
        }
    });

    
});