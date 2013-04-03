define(function() {

    return function(techProduct) {
        var self = this;

        self.techProduct = techProduct;

        self.ok = function() {
            self.modal.close(null);
        };
    };
});