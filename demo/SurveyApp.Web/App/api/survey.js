define(function () {

    var root = 'api/survey';

    return {
        getAll : function() {
            return $.getJSON(root);
        },
        
        get: function (id) {
            return $.getJSON(root + '/' + id);
        },
        
        'delete' : function(id) {
            return $.ajax({
                url: root + '/' + id,
                type: 'DELETE'
            });
        },
    };
})