var locationQuery = escape("select item.condition from weather.forecast where woeid in (select woeid from geo.places(1) where text= 'telaviv, isr') and u='c'")
    locationUrl = "http://query.yahooapis.com/v1/public/yql?q=" + locationQuery + "&format=json&callback=?";

$.ajax({
    type: 'POST',
    url: locationUrl,
    data: { 'patientID': 1 },
    dataType: 'json',
    success: function (jsonData) {
        var results = jsonData.query.results
        var firstResult = results.channel.item.condition
        console.log(firstResult);

        var location = 'Tel Aviv'
        var temp = firstResult.temp
        var text = firstResult.text

        $('#weather').append('The temperature in ' + location + ' is ' + temp + '. Forecast looks ' + text);
    },
    error: function () {
        alert('Error loading PatientID=' + id);
    }
});