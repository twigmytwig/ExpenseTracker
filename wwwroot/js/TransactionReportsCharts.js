const ctx = document.getElementById('myChart');
let names = [];
let catTransData = [];
let chart;


function CreateChart() {
        chart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: names,
                datasets: [{
                    label: '# of Votes',
                    data: catTransData,
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    y: {
                        beginAtZero: false
                    }
                }
            }
        });

    
}

function GetCategoriesNames() {
    
    for (let i = 0; i < categories.length; i++) {
        names.push(categories[i].Name);
    }
}

function OrganizeChartData(chartData) {
    if (names.length == 0) {
        GetCategoriesNames();
    }
    catTransData = [];
    
    for (let i = 0; i < names.length; i++) {
        var curVal = chartData[names[i]];
        catTransData.push(curVal);
    }
    if (chart.data.datasets[0].data != undefined) {
        chart.data.datasets[0].data = catTransData;
        chart.update();
    }
    
}