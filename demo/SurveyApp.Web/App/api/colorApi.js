define(function () {

    var root = 'api/Color';

    return {
        getAll : function() {
            return $.getJSON(root);
        },
    };
})