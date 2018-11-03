'use strict';
function getBarChartData() {
    $.ajax('/Post/GetPostsCountByWriter').then(function (data) {
        if (data && data.length) {
            // set the dimensions and margins of the graph
            var margin = { top: 20, right: 20, bottom: 30, left: 40 },
                width = 400 - margin.left - margin.right,
                height = 200 - margin.top - margin.bottom;

            var x = d3.scaleBand()
                .range([0, width])
                .padding(0.1);
            var y = d3.scaleLinear()
                .range([height, 0]);

            // append the svg object to the body of the page
            // append a 'group' element to 'svg'
            // moves the 'group' element to the top left margin
            var svg = d3.select("#barChartSection").append("svg")
                .attr("width", width + margin.left + margin.right)
                .attr("height", height + margin.top + margin.bottom)
                .append("g")
                .attr("transform",
                "translate(" + margin.left + "," + margin.top + ")");

                // format the data
                data.forEach(function (d) {
                    d.Count = +d.Count;
                });

                // Scale the range of the data in the domains
                x.domain(data.map(function (d) { return d.Writer; }));
                y.domain([0, d3.max(data, function (d) { return d.Count; })]);

                // append the rectangles for the bar chart
                svg.selectAll(".bar")
                    .data(data)
                    .enter().append("rect")
                    .attr("class", "bar")
                    .attr("x", function (d) { return x(d.Writer); })
                    .attr("width", x.bandwidth())
                    .attr("y", function (d) { return y(d.Count); })
                    .attr("height", function (d) { return height - y(d.Count); });

                // add the x Axis
                svg.append("g")
                    .attr("transform", "translate(0," + height + ")")
                    .call(d3.axisBottom(x));

                // add the y Axis
                svg.append("g")
                    .call(d3.axisLeft(y));
        }
    });
}

getBarChartData();