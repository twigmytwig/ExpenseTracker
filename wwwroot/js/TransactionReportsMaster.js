﻿window.addEventListener("load", (event) => {
    CalcTotalExpenseAndProfit(transactionList); 
    CreateCategoryByDateRangeLineChart();
    CreateChart();
    CreatePieChart();
    
});




//Standard date range
$(function () {
    $('input[name="daterange"]').daterangepicker({
        opens: 'left'
    }, function (start, end, label) {
        console.log("A new date selection was made: " + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD'));
    });
});

//Detailed date range
$(function () {

    var start = moment().subtract(29, 'days');
    var end = moment();

    function cb(start, end) {
        $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
        GatherTransactionsByDate(start, end); //starts transaction filtering
    }

    $('#reportrange').daterangepicker({
        startDate: start,
        endDate: end,
        ranges: {
            'Today': [moment(), moment()],
            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        }
    }, cb);

    cb(start, end);

});

function toggleReports(reportId) {
    let report = document.getElementById(reportId);
    if (report.style.display != 'none') {
        report.style.display = 'none';
        //report.style.visibility = 'hidden';
    }
    else {
        report.style.display = '';
    }
}