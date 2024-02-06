const ctx = document.getElementById('myChart');
const ctxPie = document.getElementById('pieChart');
const ctxCategoryByDate = document.getElementById('lineCategoryByDate');
let names = [];
let catTransData = [];
let catTrendDataByMonth = [];
let chart;
let pieChart;
const initial = arr => arr.slice(0, -1);

function CreatePieChart() { //category expense data only as of now
    let noDepositData = initial(catTransData);
    let expenseCategoryNames = initial(names);
    pieChart = new Chart(ctxPie, {
        type: 'pie',
        data: {
            labels: expenseCategoryNames,
            datasets: [{
                label: 'Spending Distribution',
                data: noDepositData,
                backgroundColor: [
                    'rgb(255, 99, 132)',
                    'rgb(54, 162, 235)',
                    'rgb(255, 205, 86)',
                    'rgb(255, 80, 125)',
                    'rgb(54, 70, 215)',
                    'rgb(255, 50, 186)',

                ]
            }]
        },
        options: {
        }
    });
}

function CreateChart() { //THIS IS A BAR CHART, i JUST AM STUPID
        chart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: names,
                datasets: [{
                    label: '$ Spent',
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

function CreateCategoryByDateRangeLineChart() {
    let curDate = new Date();
    let yearPriorDate = new Date(new Date().setFullYear(new Date().getFullYear() - 1));
    let tempData = GatherCategoryDataByMonth(1, yearPriorDate, curDate);
    let months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'June', 'July', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    chart = new Chart(ctxCategoryByDate, {
        type: 'line',
        data: {
            labels: months,
            datasets: [{
                label: 'Category Trend by Month',
                data: tempData,
                fill: false,
                borderColor: 'rgb(75, 192, 192)',
                tension: 0.1
            }]
        },
        options: {

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
    if (chart != undefined) {
        if (chart.data.datasets[0].data != undefined) {
            chart.data.datasets[0].data = catTransData;
            chart.update();
        }
    }

    if (pieChart != undefined) {
        if (pieChart.data.datasets[0].data != undefined) {
            pieChart.data.datasets[0].data = initial(catTransData);
            pieChart.data.datasets[0].labels = initial(names);
            pieChart.update();
        }
    }
    
    
}