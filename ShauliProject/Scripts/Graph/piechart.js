
function getPieChartData() {
    $.ajax('/Post/GetPostStats').then(function (data) {
        if (data && data.length) {
            var formattedData = data.map(function (obj) {
                return {
                    value: obj.Counter,
                    label: obj.Title
                }
            });
            var width = 300,
                height = 300,
                radius = Math.min(width, height) / 2;

            var color = d3.scaleOrdinal()
                .range(["#98abc5", "#8a89a6", "#7b6888"]);

            var arc = d3.arc()
                .outerRadius(radius - 10)
                .innerRadius(0);

            var labelArc = d3.arc()
                .outerRadius(radius - 40)
                .innerRadius(radius - 40);

            var pie = d3.pie()
                .value(function (d) { return d.value; })(formattedData);

            var svg = d3.select("#postStatsSection").append("svg")
                .attr("width", width)
                .attr("height", height)
                .append("g")
                .attr("transform", "translate(" + width / 2 + "," + height / 2 + ")");

            var g = svg.selectAll(".arc")
                .data(pie)
                .enter().append("g")
                .attr("class", "arc");

            g.append("path")
                .attr("d", arc)
                .style("fill", function (d) { return color(d.data.label); });

            g.append("text")
                .attr("transform", function (d) { return "translate(" + labelArc.centroid(d) + ")"; })
                .attr("dy", ".35em")
                .text(function (d) { return d.data.label; });

        }
    });
}

getPieChartData();