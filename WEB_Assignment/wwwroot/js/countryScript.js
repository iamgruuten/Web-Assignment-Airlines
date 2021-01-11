//Getting Countries
window.onload = function () {
    alert("let's go!");
}
//This section is used for input on booking search page
function search_country(country, id) {
    window.onload = function () {
        alert("let's go!");
    }
    //Country name to ISO 3 letters format
    $.getJSON("https://cdn.jsdelivr.net/npm/world_countries_lists@1.0.2/data/en/countries.json", function (json) {
        // iteratation
        var countryObject = json.find(item => item.name === country)
        console.log(countryObject.alpha3);
        document.getElementById(id).innerHTML = countryObject.alpha3;
    });
}