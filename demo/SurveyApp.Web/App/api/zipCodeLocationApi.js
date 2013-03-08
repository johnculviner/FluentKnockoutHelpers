define(function () {

    var root = 'api/ZipCodeLocation';

    return {
        get: function (id) {
            return $.getJSON(root + '/' + id);
        },
    };
})