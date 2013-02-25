define(function () {

    var root = 'api/survey';

    return {
        getAll : function() {
            return $.getJSON(root);
        },
        
        'delete' : function(id) {
            return $.ajax({
                url: root + '/' + id,
                type: 'DELETE'
            });
        }
    };
})