define(function () {

    var root = 'api/FoodGroup';

    return {
        getAll : function() {
            return $.getJSON(root);
        },
    };
})