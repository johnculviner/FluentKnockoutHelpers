define(['durandal/http'], function(http) {

    var root = 'api/color';

    return {
        getAll : function() {
            return $.get(root);
        },
        
        post: function (colors) {
            //posts JSON
            return http.post(root, colors);
        }
    };
})