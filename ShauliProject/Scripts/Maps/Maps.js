var map;

function initMap() {
    map = new google.maps.Map($('#map')[0], {
        zoom: 15
    });
}

var searchAddress;

function convertAddressToGeolocation(address) {
    searchAddress = address;
    var geocoder = new google.maps.Geocoder();
    geocoder.geocode({ address: address }, handleGeocoderResult);
}

function handleGeocoderResult(results, status) {
    if (status == 'OK') {
        putMarkerOnMapAndSetMapCenter(results[0]);
    }
    else if (status == 'ZERO_RESULTS') {
        defaultMap();
        showAddressIsNotValid();
        deleteMap();
    }
    else {
        console.log(status);
    }
}

function defaultMap() {
    map.setCenter({ lat: 31.771959, lng: 35.217018 });
}

function showAddressIsNotValid() {
    $('#' + searchAddress).show();
}

function deleteMap() {
    $('#map').remove();
}

function putMarkerOnMapAndSetMapCenter(geolocation) {
    var location = geolocation.geometry.location;
    var position = { lat: location.lat(), lng: location.lng() };

    map.setCenter(position);

    var marker = new google.maps.Marker({
        position: position
    });
    marker.setMap(map);

    setInfoWindowForMarker(geolocation.formatted_address, marker);
}

function setInfoWindowForMarker(content, marker) {
    var infoWindow = new google.maps.InfoWindow({
        content: content
    });
    infoWindow.open(map, marker);
}

function createAutoCompletePlacesSearchBox(searchBoxId) {
    new google.maps.places.SearchBox($('#' + searchBoxId)[0]);
}