define(['durandal/http'],
function (http) {

    var root = 'api/FoodGroup';

    return {
        getAll : function() {
            return $.getJSON(root);
        },
        post: function (foodGroups) {
            return http.post(root, foodGroups);
        },
    };
})