define(['durandal/http'],
function (http) {

    var root = 'api/Survey';

    return {
        getAll : function() {
            return $.getJSON(root);
        },
        
        get: function (id) {
            return $.getJSON(root + '/' + id);
        },
        
        post: function (survey) {
            return http.post(root, survey);
        },

        'delete' : function(id) {
            return $.ajax({
                url: root + '/' + id,
                type: 'DELETE'
            });
        },
    };
})