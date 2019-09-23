// Load the Visualization API and the corechart package.
google.charts.load('current', { 'packages': ['corechart', 'line', 'bar'] });

// Set a callback to run when the Google Visualization API is loaded.
google.charts.setOnLoadCallback(drawGamerPercentAgeGroupChart);
google.charts.setOnLoadCallback(drawAvgGameTimeByYear);
google.charts.setOnLoadCallback(drawRecommendActivity);

//$(window).resize(function () {
//    drawGamerPercentAgeGroupChart();
//    drawAvgGameTimeByYear();
//    drawRecommendActivity();
//});

window.onresize = function () {
    drawGamerPercentAgeGroupChart();
    drawAvgGameTimeByYear();
    drawRecommendActivity();
};

function drawGamerPercentAgeGroupChart() {
    // Create the data table.
    var data = new google.visualization.DataTable();
    data.addColumn('string', 'Age Group');
    data.addColumn('number', '2015');
    data.addColumn('number', '2017');
    data.addColumn('number', '2019');
    data.addRows([
        ["1-4", 0.39, 0.36, 0.25],
        ["5-14", 0.91, 0.9, 0.81],
        ["15-24", 0.84, 0.82, 0.83],
        ["25-34", 0.85, 0.75, 0.75],
        ["35-44", 0.76, 0.69, 0.63],
        ["45-54", 0.6, 0.54, 0.52],
        ["55-64", 0.51, 0.49, 0.37],
        ["65-74", 0.41, 0.44, 0.43],
        ["75-84", 0.35, 0.41, 0.41],
        ["85-94", 0.17, 0.22, 0.24]
    ]);

    var options = {
        title: 'Percentage of Gamer Within Each Group',
        colors: ["#ef5255", "#e1e7e7", "#07abcc"],
        chartArea: { width: '70%', height: '80%' },
        legend: { position: 'right' },
        animation: {
            startup: true,
            duration: 1000
        },
        hAxis: {
            title: 'Age Group'
        },
        vAxis: {
            format: 'percent'
        }
    };

    // var chart = new google.visualization.ColumnChart(document.getElementById('chart_gamerPercentAgeGroup_div'));
    // chart.draw(data, options);

    var chart = new google.charts.Bar(document.getElementById('chart_gamerPercentAgeGroup_div'));
    chart.draw(data, google.charts.Bar.convertOptions(options));
}

function drawAvgGameTimeByYear() {
    var data = new google.visualization.DataTable();
    data.addColumn('string', "Age Group");
    data.addColumn('number', "2015");
    data.addColumn('number', "2017");
    data.addColumn('number', "2019");
    data.addRows([
        ["1-4", 45, 25, 82.5],
        ["5-14", 91, 86.5, 71.5],
        ["15-24", 117, 120.5, 105.5],
        ["25-34", 92.5, 99.5, 91.5],
        ["35-44", 80, 82.5, 75],
        ["45-54", 65.5, 77.5, 72],
        ["55-64", 79.5, 69.5, 52.5],
        ["65-74", 62.5, 56.5, 69],
        ["75-84", 61, 66.5, 41]
    ]);

    var options = {
        'title': 'Average Time Spent Daily by Different Age Groups Playing Games',
        colors: ["#ef5255", "#e1e7e7", "#07abcc"],
        pointSize: 5,
        curveType: 'function',
        legend: { position: 'right' },
        vAxis: {
            title: "Duration (mins)"
        },
        hAxis: { title: "Age Group" }
    };

    // Instantiate and draw our chart, passing in some options.
    var chart = new google.charts.Line(document.getElementById('chart_avg_gametime_div'));
    chart.draw(data, google.charts.Line.convertOptions(options));
}

function drawRecommendActivity() {
    var data = new google.visualization.DataTable();
    data.addColumn('string', "Recommendation");
    data.addColumn('number', "5-8");
    data.addColumn('number', "9-12");
    data.addColumn('number', "13-14");
    data.addColumn('number', "15-17");
    data.addRows([
        ["Physical activity", 0.36, 0.18, 0.12, 0.06],
        ["Screen time", 0.43, 0.3, 0.24, 0.2],
        ["Both guidelines", 0.2, 0.07, 0.05, 0.02]
    ]);

    var options = {
        title: 'Percentage of Children who Met Physical Activity and Screen Time Guideline',
        colors: ["#e1e7e7", "#ef5255", "#e1e7e7", "#e1e7e7"],
        hAxis: {
            title: 'Guideline'
        },
        vAxis: {
            format: 'percent'
        }
    };

    var chart = new google.charts.Bar(document.getElementById('chart_meet_guideline_div'));
    chart.draw(data, google.charts.Bar.convertOptions(options));
}